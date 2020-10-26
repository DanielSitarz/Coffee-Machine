using Machine.Dictionaries;
using Machine.Enums;
using Machine.Events;
using UnityEngine;

namespace Machine.Components
{
    // TODO: Move power, size, coffee success texts to display.
    public class CoffeeMachine : MonoBehaviour, ITurnable
    {
        public bool debug = false;

        [SerializeField, Tooltip("Handles coffee brewing process.")]
        private BrewModule brewModule;
        [SerializeField, Tooltip("Used to react to warnings. Not required.")]
        private SensorsListener sensorsListener;
        [SerializeField, Tooltip("Handles status/warning events and other.")]
        private Display display;
        [SerializeField]
        private Coffee defaultCoffee;

        [SerializeField, Space()]
        private IntIntDictionary coffeeAmountPerPower = new IntIntDictionary() {
            {0, 8},
            {1, 12},
            {2, 16},
        };
        [SerializeField, Tooltip("Current power, limited by entries in dictionary.")]
        private int coffeePower = 1;

        [SerializeField, Space()]
        private IntIntDictionary waterAmountPerSize = new IntIntDictionary() {
            {0, 80},
            {1, 120},
            {2, 160},
        };
        [SerializeField, Tooltip("Current size, limited by entires in dictionary.")]
        private int coffeeSize = 2;

        public StatusEvent OnStatusChange;

        public Status Status { get { return status; } }
        private Status status = Status.Off;

        private Coffee currentCoffee;
        private bool hasWarnings = false;

        private bool Operational
        {
            get { return brewModule.Status == Status.Idle; }
        }

        void Start()
        {
            currentCoffee = Instantiate(defaultCoffee);
        }

        void OnDisable()
        {
            TurnOff();
        }

        void OnValidate()
        {
            coffeeSize = Mathf.Clamp(coffeeSize, 0, coffeeAmountPerPower.Count - 1);
            coffeePower = Mathf.Clamp(coffeePower, 0, waterAmountPerSize.Count - 1);
        }

        public void ToggleOnOff()
        {
            if (status == Status.Off) TurnOn(); else TurnOff();

            Utils.DebugLog(this, "Toggle on/off", debug);
        }

        public void TurnOn()
        {
            EnableEvents();

            if (sensorsListener != null) sensorsListener.TurnOn();
            brewModule.TurnOn();
            display.TurnOn();

            SetStatus(Status.Idle);

            Utils.DebugLog(this, "Turn on", debug);
        }

        public void TurnOff()
        {
            if (sensorsListener != null) sensorsListener.TurnOff();
            brewModule.TurnOff();
            display.TurnOff();

            SetStatus(Status.Off);

            DisableEvents();

            Utils.DebugLog(this, "Turn off", debug);
        }

        public void Brew()
        {
            if (hasWarnings) return;

            display.ClearTimedMsg();

            brewModule.StartBrew(currentCoffee);

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

        private void EnableEvents()
        {
            if (sensorsListener != null) sensorsListener.OnWarnings.AddListener(OnWarnings);
            brewModule.OnStatusChange.AddListener(SetStatus);
            brewModule.OnBrewSuccess.AddListener(OnBrewSuccess);
            OnStatusChange.AddListener(display.DisplayStatus);
        }

        private void DisableEvents()
        {
            if (sensorsListener != null) sensorsListener.OnWarnings.RemoveListener(OnWarnings);
            brewModule.OnStatusChange.RemoveListener(SetStatus);
            brewModule.OnBrewSuccess.RemoveListener(OnBrewSuccess);
            OnStatusChange.RemoveListener(display.DisplayStatus);
        }

        private void OnWarnings(Warning[] warnings)
        {
            if (warnings.Length == 0)
            {
                if (hasWarnings) ClearWarnings();
                return;
            }

            hasWarnings = true;

            brewModule.StopBrewing();

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

            display.ClearWarning();
        }

        private void SetStatus(Status newStatus)
        {
            status = newStatus;

            OnStatusChange.Invoke(status);

            Utils.DebugLog(this, $"Changed status - {newStatus}", debug);
        }
    }
}