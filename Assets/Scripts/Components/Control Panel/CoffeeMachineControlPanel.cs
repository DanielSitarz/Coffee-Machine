using Machine.Dictionaries;
using Machine.Enums;
using UnityEngine;

namespace Machine.Components
{
    // TODO: Move power, size, coffee success texts to display.
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

        private bool Operational
        {
            get { return coffeeMachine.Status == Status.Idle; }
        }

        void Start()
        {
            currentCoffee = Instantiate(defaultCoffee);
        }

        void OnEnable()
        {
            coffeeMachine.OnTurnOn.AddListener(TurnOn);
            coffeeMachine.OnTurnOff.AddListener(TurnOff);
            coffeeMachine.OnStatusChange.AddListener(OnStatusChange);
            coffeeMachine.OnBrewSuccess.AddListener(OnBrewSuccess);
        }

        void OnDisable()
        {
            coffeeMachine.OnTurnOn.RemoveListener(TurnOn);
            coffeeMachine.OnTurnOff.RemoveListener(TurnOff);
            coffeeMachine.OnStatusChange.RemoveListener(OnStatusChange);
            coffeeMachine.OnBrewSuccess.RemoveListener(OnBrewSuccess);
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
            if (!Operational) return;

            coffeePower = Utils.ToggleNumber(coffeePower + 1, coffeeAmountPerPower.Count - 1);

            SetCoffeePower(coffeePower);
        }

        public void ChangeCoffeeSize()
        {
            if (!Operational) return;

            coffeeSize = Utils.ToggleNumber(coffeeSize + 1, waterAmountPerSize.Count - 1);

            SetCoffeeSize(coffeeSize);
        }

        protected override void OnStatusChange(Status newStatus)
        {
            if (oldStatus == newStatus) return;

            oldStatus = newStatus;

            display.DisplayStatus(newStatus);

            Utils.DebugLog(this, $"Status changes - {newStatus}", debug);
        }

        protected override void OnWarnings(Warning[] warnings)
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

        private void OnBrewSuccess(Coffee coffee)
        {
            display.DisplayTimedMsg("Coffee ready.");
        }

        private void SetCoffeePower(int power)
        {
            currentCoffee.coffeeAmount = Utils.TryGetValueOrDefault(coffeeAmountPerPower, power, defaultCoffee.coffeeAmount);

            display.DisplayTimedMsg($"Coffee power: {power}.");

            Utils.DebugLog(this, "Change coffee power", debug);
        }

        private void SetCoffeeSize(int size)
        {
            currentCoffee.waterAmount = Utils.TryGetValueOrDefault(waterAmountPerSize, size, defaultCoffee.waterAmount);

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