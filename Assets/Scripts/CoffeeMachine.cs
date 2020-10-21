using System;
using System.Collections;
using System.Collections.Generic;
using Machine.Component;
using Machine.Enums;
using Machine.Events;
using UnityEngine;


namespace Machine
{
    public class CoffeeMachine : MonoBehaviour
    {
        [SerializeField] private ControlPanel controlPanel;

        [SerializeField] private Supplier waterPump;
        [SerializeField] private Supplier coffeeGrinder;

        [SerializeField] private ContainerSensor dripLevelSensor;
        [SerializeField] private ContainerSensor groundsLevelSensor;
        [SerializeField] private ContainerSensor waterLevelSensor;

        private Status status = Status.Off;

        private bool hasWarnings = false;

        public StatusEvent OnStatusChange;
        public SingleWarningEvent OnWarningHappen;

        void Start()
        {
            if (controlPanel == null) throw new Exception($"Attach Control Panel to Coffee Machine ({name}) first.");
        }

        void OnDisable()
        {
            TurnOff();
        }

        public void ToggleOnOff()
        {
            if (status == Status.Off) TurnOn(); else TurnOff();
        }

        public void StartBrew()
        {
            if (status != Status.Idle) return;
            if (hasWarnings) return;

            SetStatus(Status.Busy);

            StartCoroutine(Brew());
        }

        private IEnumerator Brew()
        {
            coffeeGrinder.StartProcessing(12);

            yield return new WaitUntil(() => coffeeGrinder.Status == Status.Idle);

            waterPump.StartProcessing(100);

            yield return new WaitUntil(() => waterPump.Status == Status.Idle);

            StopBrewing();
        }

        private void TurnOn()
        {
            SetupEvents();

            SetStatus(Status.Idle);
        }

        private void TurnOff()
        {
            switch (status)
            {
                case Status.Idle:
                    SetStatus(Status.Off);
                    break;
                case Status.Busy:
                    StopBrewing();
                    TurnOff();
                    break;
            }

            RemoveEvents();
        }

        private void StopBrewing()
        {
            coffeeGrinder.StopProcessing();
            waterPump.StopProcessing();

            SetStatus(Status.Idle);
        }

        private void SetupEvents()
        {
            OnStatusChange.AddListener(controlPanel.OnStatusChange);
            OnWarningHappen.AddListener(controlPanel.OnWarning);

            groundsLevelSensor.OnWarning.AddListener(ListenForWarnings);
            waterLevelSensor.OnWarning.AddListener(ListenForWarnings);
            dripLevelSensor.OnWarning.AddListener(ListenForWarnings);
        }

        private void RemoveEvents()
        {
            OnStatusChange.RemoveAllListeners();
            OnWarningHappen.RemoveAllListeners();

            groundsLevelSensor.OnWarning.RemoveListener(ListenForWarnings);
            waterLevelSensor.OnWarning.RemoveListener(ListenForWarnings);
            dripLevelSensor.OnWarning.RemoveListener(ListenForWarnings);
        }

        private void SetStatus(Status newStatus)
        {
            status = newStatus;
            OnStatusChange.Invoke(status);
        }

        private void ListenForWarnings(Warning warning)
        {
            Debug.Log(warning);
            hasWarnings = true;

            OnWarningHappen.Invoke(warning);

            // if (hasWarnings)
            // {
            //     OnWarnings(warnings);
            // }
        }

        private void OnWarnings(List<Warning> warnings)
        {
            if (status == Status.Busy) StopBrewing();
        }
    }
}