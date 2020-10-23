using System;
using System.Collections;
using Machine.Components;
using Machine.Enums;
using Machine.Events;
using UnityEngine;
using UnityEngine.Events;

namespace Machine
{
    public class CoffeeMachine : MonoBehaviour
    {
        public bool debug;

        [SerializeField] private Supplier coffeeGrinder;
        [SerializeField] private Supplier waterPump;

        public UnityEvent OnTurnOn;
        public UnityEvent OnTurnOff;

        public StatusEvent OnStatusChange;

        private Status status = Status.Off;

        void OnDisable()
        {
            TryToTurnOff();
        }

        public void ToggleOnOff()
        {
            if (status == Status.Off) TurnOn(); else TryToTurnOff();
        }

        public void StartBrew(Coffee coffeeToBrew)
        {
            if (status != Status.Idle)
            {
                Debug.LogWarning($"Can't brew because machine is {status}");
                return;
            }

            SetStatus(Status.Busy);

            StartCoroutine(Brew(coffeeToBrew));
        }

        private void TryToTurnOff()
        {
            switch (status)
            {
                case Status.Idle:
                    TurnOff();
                    break;
                case Status.Busy:
                    StopBrewing();
                    TurnOff();
                    break;
            }
        }

        private void TurnOn()
        {
            OnTurnOn.Invoke();

            SetStatus(Status.Idle);

            Utils.DebugLog(this, "Turn on", debug);
        }

        private void TurnOff()
        {
            OnTurnOff.Invoke();

            SetStatus(Status.Off);

            Utils.DebugLog(this, "Turn on", debug);
        }

        private IEnumerator Brew(Coffee coffeeToBrew)
        {
            Utils.DebugLog(this, $"Brewing {coffeeToBrew}", debug);
            Utils.DebugLog(this, $"{coffeeToBrew.coffeeAmount}", debug);
            Utils.DebugLog(this, $"{coffeeToBrew.waterAmount}", debug);

            coffeeGrinder.StartProcessing(coffeeToBrew.coffeeAmount);

            yield return new WaitUntil(() => coffeeGrinder.Status == Status.Idle);

            waterPump.StartProcessing(coffeeToBrew.waterAmount);

            yield return new WaitUntil(() => waterPump.Status == Status.Idle);

            SetStatus(Status.Idle);
        }

        public void StopBrewing()
        {
            if (status != Status.Busy) return;

            StopCoroutine("Brew");

            coffeeGrinder.StopProcessing();
            waterPump.StopProcessing();

            SetStatus(Status.Idle);

            Utils.DebugLog(this, $"Stopped brewing", debug);
        }

        private void SetStatus(Status newStatus)
        {
            status = newStatus;
            OnStatusChange.Invoke(status);
            Utils.DebugLog(this, $"Changed status - {newStatus}", debug);
        }
    }
}