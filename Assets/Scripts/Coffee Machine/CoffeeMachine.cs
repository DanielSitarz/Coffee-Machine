using System;
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

        [SerializeField, Space()]
        private CoffeeStrengthToFlowRateDictionary strengthToFlowRate = new CoffeeStrengthToFlowRateDictionary() {
            {CoffeeStrength.Weak, 120.0f},
            {CoffeeStrength.Normal, 100.0f},
            {CoffeeStrength.Strong, 80.0f}
        };
        [SerializeField]
        private CoffeeStrength coffeeStrength = CoffeeStrength.Normal;
        private int coffeeStrengthIndex;

        [SerializeField, Space()]
        private CoffeeSizeToWaterAmountDictionary sizeToWaterAmount = new CoffeeSizeToWaterAmountDictionary() {
            {CoffeeSize.Small, 40},
            {CoffeeSize.Medium, 80},
            {CoffeeSize.Big, 120}
        };
        [SerializeField]
        private CoffeeSize coffeeSize = CoffeeSize.Medium;
        private int coffeeSizeIndex;

        private Coffee currentCoffee;

        public StatusEvent OnStatusChange;

        public Status Status { get { return status; } }
        private Status status = Status.Off;

        private bool hasWarnings = false;

        private bool Operational
        {
            get { return brewModule.Status == Status.Idle; }
        }

        void Start()
        {
            currentCoffee = new Coffee();
        }

        void OnDisable()
        {
            TurnOff();
        }

        public void ToggleOnOff()
        {
            if (status == Status.Off) TurnOn(); else TurnOff();

            Utils.DebugLog(this, "Toggle on/off", debug);
        }

        public void TurnOn()
        {
            EnableEvents();

            coffeeStrengthIndex = (int)coffeeStrength;
            coffeeSizeIndex = (int)coffeeSize;

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

            CoffeeMakeModel model = ConstructCoffeeMakeModel(currentCoffee);

            brewModule.StartBrew(model);

            Utils.DebugLog(this, "Brew", debug);
        }

        public void ChangeCoffeeStrength()
        {
            if (!Operational) return;

            coffeeStrengthIndex = Utils.ToggleNumber(coffeeStrengthIndex + 1, strengthToFlowRate.Count - 1);

            SetCoffeeStrength(coffeeStrengthIndex);
        }

        public void ChangeCoffeeSize()
        {
            if (!Operational) return;

            coffeeSizeIndex = Utils.ToggleNumber(coffeeSizeIndex + 1, sizeToWaterAmount.Count - 1);

            SetCoffeeSize(coffeeSizeIndex);
        }

        private void SetCoffeeStrength(int index)
        {
            currentCoffee.strength = (CoffeeStrength)index;

            display.DisplayTimedMsg($"Coffee power: {currentCoffee.strength}");

            Utils.DebugLog(this, "Change coffee power", debug);
        }

        private void SetCoffeeSize(int index)
        {
            currentCoffee.size = (CoffeeSize)index;

            display.DisplayTimedMsg($"Coffee size: {currentCoffee.size}");

            Utils.DebugLog(this, "Change coffee size", debug);
        }

        private CoffeeMakeModel ConstructCoffeeMakeModel(Coffee currentCoffee)
        {
            return new CoffeeMakeModel()
            {
                waterAmount = sizeToWaterAmount[currentCoffee.size],
                waterFlowRate = strengthToFlowRate[currentCoffee.strength]
            };
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

        private void ClearWarnings()
        {
            hasWarnings = false;

            display.ClearWarning();
        }

        private void OnBrewSuccess()
        {
            display.DisplayTimedMsg($"{currentCoffee.size}&{currentCoffee.strength} coffee ready.");
        }

        private void SetStatus(Status newStatus)
        {
            status = newStatus;

            OnStatusChange.Invoke(status);

            Utils.DebugLog(this, $"Changed status - {newStatus}", debug);
        }
    }
}