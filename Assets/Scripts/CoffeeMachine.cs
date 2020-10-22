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
        [SerializeField] private Supplier waterPump;
        [SerializeField] private Supplier coffeeGrinder;

        public UnityEvent OnTurnOn;
        public UnityEvent OnTurnOff;

        public StatusEvent OnStatusChange;

        private Status status = Status.Off;
        private Warning warning = Warning.None;

        void OnDisable()
        {
            TryToTurnOff();
        }

        public void ToggleOnOff()
        {
            if (status == Status.Off) TurnOn(); else TryToTurnOff();
        }

        public void StartBrew()
        {
            if (status != Status.Idle || warning != Warning.None) return;

            SetStatus(Status.Busy);

            StartCoroutine(Brew());
        }

        public void OnWarnings(Warning[] warnings)
        {
            if (warnings.Length == 0)
            {
                warning = Warning.None;
                return;
            }

            warning = warnings[0];

            StopBrewing();
        }

        private void TurnOn()
        {
            OnTurnOn.Invoke();

            SetStatus(Status.Idle);
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

        private void TurnOff()
        {
            OnTurnOff.Invoke();

            SetStatus(Status.Off);
        }

        private IEnumerator Brew()
        {
            coffeeGrinder.StartProcessing(12);

            yield return new WaitUntil(() => coffeeGrinder.Status == Status.Idle);

            waterPump.StartProcessing(100);

            yield return new WaitUntil(() => waterPump.Status == Status.Idle);

            SetStatus(Status.Idle);
        }

        private void StopBrewing()
        {
            StopCoroutine(Brew());

            coffeeGrinder.StopProcessing();
            waterPump.StopProcessing();

            SetStatus(Status.Idle);
        }

        private void SetStatus(Status newStatus)
        {
            status = newStatus;
            OnStatusChange.Invoke(status);
        }
    }
}