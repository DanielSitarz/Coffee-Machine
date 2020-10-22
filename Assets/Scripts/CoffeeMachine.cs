using System;
using System.Collections;
using Machine.Components;
using Machine.Enums;
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
        [SerializeField] private ContainerSensor coffeeLevelSensor;

        private Status status = Status.Off;

        private bool hasWarnings = false;

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

        private void TurnOn()
        {
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
        }

        private IEnumerator Brew()
        {
            coffeeGrinder.StartProcessing(12);

            yield return new WaitUntil(() => coffeeGrinder.Status == Status.Idle);

            waterPump.StartProcessing(100);

            yield return new WaitUntil(() => waterPump.Status == Status.Idle);

            StopBrewing();
        }

        private void StopBrewing()
        {
            coffeeGrinder.StopProcessing();
            waterPump.StopProcessing();

            SetStatus(Status.Idle);
        }

        private void SetStatus(Status newStatus)
        {
            status = newStatus;
        }

        private void CheckWarnings()
        {
            Warning warning;

            if (waterLevelSensor.Status == SensorStatus.Low)
            {
                warning = Warning.LowOnWater;
            }

            if (coffeeLevelSensor.Status == SensorStatus.Low)
            {
                warning = Warning.LowOnBeans;
            }

            if (groundsLevelSensor.Status == SensorStatus.High)
            {
                warning = Warning.GroundsContainerFull;
            }

            if (dripLevelSensor.Status == SensorStatus.High)
            {
                warning = Warning.DripTrayFull;
            }

            // if (warning != null)
            // {
            //     hasWarnings = true;

            //     controlPanel.OnWarning(warning);
            //     OnWarnings(warning);
            // }
        }

        private void OnWarnings(Warning warning)
        {
            if (status == Status.Busy) StopBrewing();
        }
    }
}