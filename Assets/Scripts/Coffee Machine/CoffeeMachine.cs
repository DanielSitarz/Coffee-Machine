﻿using System;
using System.Collections;
using DanielSitarz.MyLog;
using Machine.Dictionaries;
using Machine.Enums;
using Machine.Events;
using Newtonsoft.Json;
using UnityEngine;

namespace Machine.Components
{
    public class CoffeeMachine : MonoBehaviour, ITurnable, ISaveable
    {
        public bool debug = false;

        [SerializeField, Tooltip("Handles coffee brewing process.")]
        private BrewModule brewModule;
        [SerializeField, Tooltip("Used to react to warnings. Not required.")]
        private SensorsListener sensorsListener;

        [SerializeField, Tooltip("Handles status/warning events and other. Not required.")]
        private Display display;

        // From my research it turns out that Coffee Strength is controlled by the flow rate of the water (or temperature).
        // The slower water goes through coffee, the stronger coffee is.
        [SerializeField, Space()]
        private CoffeeStrengthToFlowRateDictionary strengthToFlowRate = new CoffeeStrengthToFlowRateDictionary() {
            {CoffeeStrength.Weak, 120.0f},
            {CoffeeStrength.Normal, 100.0f},
            {CoffeeStrength.Strong, 80.0f}
        };
        [SerializeField]
        private CoffeeStrength coffeeStrength = CoffeeStrength.Normal;

        [SerializeField, Space()]
        private CoffeeSizeToWaterAmountDictionary sizeToWaterAmount = new CoffeeSizeToWaterAmountDictionary() {
            {CoffeeSize.Small, 40},
            {CoffeeSize.Medium, 80},
            {CoffeeSize.Big, 120}
        };
        [SerializeField]
        private CoffeeSize coffeeSize = CoffeeSize.Medium;

        public Coffee CurrentCoffee { get { return currentCoffee; } }
        private Coffee currentCoffee;

        public StatusEvent OnStatusChange;

        public Status Status { get { return status; } }
        private Status status = Status.Off;

        public bool Operational
        {
            get { return status == Status.Idle && brewModule.Status == Status.Idle; }
        }

        private bool coffeeSetFromOutside = false;
        private bool hasWarnings = false;

        void OnEnable()
        {
            if (display == null) display = new NullDisplay();
        }

        void OnValidate()
        {
            if (currentCoffee != null)
            {
                currentCoffee.strength = coffeeStrength;
                currentCoffee.size = coffeeSize;
            }
        }

        public void ToggleOnOff()
        {
            MyLog.TryLog(this, "Toggled On/Off", debug);

            if (status == Status.Off) TurnOn(); else TurnOff();
        }

        public void TurnOn()
        {
            MyLog.TryLog(this, "Turning on", debug);

            EnableEvents();

            if (sensorsListener != null) sensorsListener.TurnOn();
            brewModule.TurnOn();
            display.TurnOn();

            SetStatus(Status.Idle);
        }

        public void TurnOff()
        {
            MyLog.TryLog(this, "Turning off", debug);

            if (sensorsListener != null) sensorsListener.TurnOff();
            brewModule.TurnOff();
            display.TurnOff();

            SetStatus(Status.Off);

            DisableEvents();
        }

        public void Brew()
        {
            MyLog.TryLog(this, "Started brewing", debug);

            if (hasWarnings)
            {
                MyLog.TryLog(this, "Has warnings, aborting brewing", debug);
                return;
            }

            SetCurrentCoffeeName();
            SetStatus(Status.Busy);
            display.ClearTimedMsg();

            CoffeeMakeModel model = ConstructCoffeeMakeModel(currentCoffee);
            brewModule.StartBrew(model);
        }

        public void SetCoffee(Coffee coffee)
        {
            if (!Operational) return;
            if (coffee == null)
            {
                display.DisplayTimedMsg(DisplayMessage.NoFavorite);
                MyLog.TryLog(this, $"Trying to set null coffee", debug);
                return;
            }

            MyLog.TryLog(this, $"Set coffee - {coffee.coffeeName}", debug);

            currentCoffee = coffee;
            coffeeSetFromOutside = true;
            display.DisplayTimedMsg(DisplayMessage.SelectedCoffee, coffee.coffeeName);
        }

        public void ChangeCoffeeStrength()
        {
            if (!Operational) return;

            coffeeStrength = NextSetting<CoffeeStrength>(coffeeStrength, sizeToWaterAmount.Count - 1);
            currentCoffee.strength = coffeeStrength;

            display.DisplayTimedMsg(DisplayMessage.SetCoffeeStrength, coffeeStrength.ToString());
        }

        public void ChangeCoffeeSize()
        {
            if (!Operational) return;

            coffeeSize = NextSetting<CoffeeSize>(coffeeSize, sizeToWaterAmount.Count - 1);
            currentCoffee.size = coffeeSize;

            display.DisplayTimedMsg(DisplayMessage.SetCoffeeSize, coffeeSize.ToString());
        }

        private T NextSetting<T>(T val, int max) where T : Enum
        {
            var nextIndex = Utils.ToggleNumber(((int)(object)val) + 1, max);
            coffeeSetFromOutside = false;

            MyLog.TryLog(this, $"Changed coffee size to {(T)(object)nextIndex}", debug);

            return (T)(object)nextIndex;
        }

        private CoffeeMakeModel ConstructCoffeeMakeModel(Coffee currentCoffee)
        {
            return new CoffeeMakeModel()
            {
                waterAmount = sizeToWaterAmount[currentCoffee.size],
                waterFlowRate = strengthToFlowRate[currentCoffee.strength]
            };
        }

        #region -----------------Events

        private void EnableEvents()
        {
            if (sensorsListener != null) sensorsListener.OnWarnings.AddListener(OnWarnings);
            brewModule.OnBrewSuccess.AddListener(OnBrewSuccess);

            MyLog.TryLog(this, "Events enabled", debug);
        }

        private void DisableEvents()
        {
            if (sensorsListener != null) sensorsListener.OnWarnings.RemoveListener(OnWarnings);
            brewModule.OnBrewSuccess.RemoveListener(OnBrewSuccess);

            MyLog.TryLog(this, "Events disabled", debug);
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
            SetStatus(Status.Idle);
            display.DisplayWarning(warnings[0]);

            MyLog.TryLog(this, $"Got warning - {warnings[0]}", debug);
        }

        private void OnBrewSuccess()
        {
            var name = currentCoffee.coffeeName;

            SetStatus(Status.Idle);
            display.DisplayTimedMsg(DisplayMessage.CoffeeReady, name);

            MyLog.TryLog(this, $"Brew succeeded.", debug);
        }

        #endregion

        private void SetStatus(Status newStatus)
        {
            status = newStatus;

            OnStatusChange.Invoke(status);

            display.DisplayStatus(status);

            MyLog.TryLog(this, $"Changed status - {newStatus}", debug);
        }

        private void ClearWarnings()
        {
            hasWarnings = false;

            display.ClearWarning();

            MyLog.TryLog(this, $"Cleared warnings", debug);
        }

        private void SetCurrentCoffeeName()
        {
            if (!coffeeSetFromOutside) currentCoffee.coffeeName = $"{currentCoffee.size}&{currentCoffee.strength}";
        }

        public void Save(string baseId)
        {
            CoffeeMachineState coffeeMachineState = new CoffeeMachineState()
            {
                status = status,
                currentCoffee = currentCoffee,
                coffeeSetFromOutside = coffeeSetFromOutside
            };

            var UID = GetComponent<UniqueID>().uid;
            SaveLoadSystem.Save<CoffeeMachineState>(coffeeMachineState, UID, "");


            MyLog.TryLog(this, $"Saved", debug);
            MyLog.TryLog(this, JsonConvert.SerializeObject(coffeeMachineState), debug);
        }

        public void Load(string baseId)
        {
            var UID = GetComponent<UniqueID>().uid;
            CoffeeMachineState state = SaveLoadSystem.Load<CoffeeMachineState>(UID, "");
            if (state == null) state = new CoffeeMachineState();

            currentCoffee = state.currentCoffee;
            coffeeSetFromOutside = state.coffeeSetFromOutside;

            if (state.status == Status.Idle || state.status == Status.Busy)
            {
                TurnOn();
            }

            MyLog.TryLog(this, $"Loaded", debug);
            MyLog.TryLog(this, JsonConvert.SerializeObject(state), debug);
        }
    }
}