using System.Collections;
using Machine.Components;
using Machine.Enums;
using Machine.Events;
using UnityEngine;

namespace Machine
{
    public class BrewModule : MonoBehaviour, ITurnable
    {
        public bool debug;

        public Status Status { get { return status; } }
        private Status status = Status.Off;

        [SerializeField]
        private Supplier coffeeGrinder;
        [SerializeField]
        private Supplier waterPump;

        [HideInInspector]
        public CoffeeEvent OnBrewSuccess;
        [HideInInspector]
        public StatusEvent OnStatusChange;

        private Coroutine brewingProcess;

        public void StartBrew(Coffee coffeeToBrew)
        {
            if (status != Status.Idle)
            {
                Debug.LogWarning($"Can't brew because machine is {status}");
                return;
            }

            SetStatus(Status.Busy);

            brewingProcess = StartCoroutine(Brew(coffeeToBrew));
        }

        public void TurnOn()
        {
            SetStatus(Status.Idle);

            Utils.DebugLog(this, "Turn on", debug);
        }

        public void TurnOff()
        {
            StopBrewing();

            SetStatus(Status.Off);

            Utils.DebugLog(this, "Turn off", debug);
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

            OnBrewSuccess.Invoke(coffeeToBrew);
            SetStatus(Status.Idle);
        }

        public void StopBrewing()
        {
            if (status != Status.Busy || brewingProcess == null) return;

            StopCoroutine(brewingProcess);

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