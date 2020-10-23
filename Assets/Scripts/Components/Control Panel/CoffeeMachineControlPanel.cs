using Machine.Dictionaries;
using Machine.Enums;
using UnityEngine;

namespace Machine.Components
{
    public class CoffeeMachineControlPanel : ControlPanel
    {
        public CoffeeMachine coffeeMachine;

        [SerializeField]
        private SensorsListener sensorsListener;

        [SerializeField]
        private Display display;

        [SerializeField, Space()]
        private Coffee defaultCoffee;

        [SerializeField, Space()]
        private IntIntDictionary coffeeAmountPerPower = new IntIntDictionary() {
            {0, 8},
            {1, 12},
            {2, 16},
        };

        [SerializeField]
        private IntIntDictionary waterAmountPerSize = new IntIntDictionary() {
            {0, 80},
            {1, 120},
            {2, 160},
        };

        [SerializeField, Space()]
        private int coffeePower = 1;
        [SerializeField]
        private int coffeeSize = 2;

        private Coffee currentCoffee;
        private Status oldStatus = Status.Off;
        private bool hasWarnings = false;

        void Start()
        {
            currentCoffee = Instantiate(defaultCoffee);
        }

        void OnEnable()
        {
            coffeeMachine.OnTurnOn.AddListener(TurnOn);
            coffeeMachine.OnTurnOff.AddListener(TurnOff);
            coffeeMachine.OnStatusChange.AddListener(OnStatusChange);
        }

        void OnDisable()
        {
            coffeeMachine.OnTurnOn.RemoveListener(TurnOn);
            coffeeMachine.OnTurnOff.RemoveListener(TurnOff);
            coffeeMachine.OnStatusChange.RemoveListener(OnStatusChange);
        }

        void OnValidate()
        {
            coffeeSize = Mathf.Clamp(coffeeSize, 0, coffeeAmountPerPower.Count - 1);
            coffeePower = Mathf.Clamp(coffeePower, 0, waterAmountPerSize.Count - 1);
        }

        public override void TurnOn()
        {
            if (sensorsListener != null)
            {
                sensorsListener.OnWarnings.AddListener(OnWarnings);
                sensorsListener.TurnOn();
            }

            display.TurnOn();

            Utils.DebugLog(this, "Turn on", debug);
        }

        public override void TurnOff()
        {
            if (sensorsListener != null)
            {
                sensorsListener.OnWarnings.RemoveListener(OnWarnings);
                sensorsListener.TurnOff();
            }

            display.TurnOff();

            Utils.DebugLog(this, "Turn off", debug);
        }

        public void Brew()
        {
            if (hasWarnings) return;

            display.ClearTimedMsg();

            coffeeMachine.StartBrew(currentCoffee);

            Utils.DebugLog(this, "Brew", debug);
        }

        public void ChangeCoffeePower()
        {
            coffeePower = Utils.ToggleNumber(coffeePower + 1, coffeeAmountPerPower.Count - 1);

            SetCoffeePower(coffeePower);
        }

        public void ChangeCoffeeSize()
        {
            coffeeSize = Utils.ToggleNumber(coffeeSize + 1, waterAmountPerSize.Count - 1);

            SetCoffeeSize(coffeeSize);
        }

        public override void OnStatusChange(Status newStatus)
        {
            if (oldStatus == newStatus) return;

            oldStatus = newStatus;

            display.DisplayStatus(newStatus);

            Utils.DebugLog(this, $"Status changes - {newStatus}", debug);
        }

        public override void OnWarnings(Warning[] warnings)
        {
            if (warnings.Length == 0)
            {
                ClearWarnings();
                return;
            }

            hasWarnings = true;

            coffeeMachine.StopBrewing();

            display.DisplayWarning(warnings[0]);
        }

        private void SetCoffeePower(int power)
        {
            currentCoffee.coffeeAmount = Utils.TryGetValueWithDefault(coffeeAmountPerPower, power, defaultCoffee.coffeeAmount);

            display.DisplayTimedMsg($"Coffee power: {power}.");

            Utils.DebugLog(this, "Change coffee power", debug);
        }

        private void SetCoffeeSize(int size)
        {
            currentCoffee.waterAmount = Utils.TryGetValueWithDefault(waterAmountPerSize, size, defaultCoffee.waterAmount);

            display.DisplayTimedMsg($"Coffee size: {size}.");

            Utils.DebugLog(this, "Change coffee size", debug);
        }

        private void ClearWarnings()
        {
            hasWarnings = false;
            display.DisplayStatus(oldStatus);
        }
    }
}