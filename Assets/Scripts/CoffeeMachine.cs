using System.Collections;
using System.Collections.Generic;
using Machine.Component;
using UnityEngine;


namespace Machine
{
    public class CoffeeMachine : MonoBehaviour
    {
        private Status status = Status.Off;

        [SerializeField] private Display display;

        [SerializeField] private Supplier waterPump;
        [SerializeField] private Supplier coffeeGrinder;

        [SerializeField] private ContainerSensor dripLevelSensor;
        [SerializeField] private ContainerSensor groundsLevelSensor;
        [SerializeField] private ContainerSensor waterLevelSensor;

        private bool hasWarnings = false;

        void Update()
        {
            if (status == Status.Off) return;

            CheckForWarnings();
        }

        public void ToggleOnOff()
        {
            if (status == Status.Off) TurnOn(); else TurnOff();

            if (status == Status.Idle)
            {
                if (display != null) display.Clear();
                CheckForWarnings();
            }
        }

        private void TurnOn()
        {
            status = Status.Idle;
        }

        private void TurnOff()
        {
            switch (status)
            {
                case Status.Idle:
                    status = Status.Off;
                    break;
                case Status.Busy:
                    StopBrewing();
                    TurnOff();
                    break;
            }
        }

        private void StopBrewing()
        {
            coffeeGrinder.StopProcessing();
            waterPump.StopProcessing();
            display.Clear();

            status = Status.Idle;
        }

        public void StartBrew()
        {
            if (status != Status.Idle) return;
            if (hasWarnings) return;

            StartCoroutine(Brew());
        }

        private IEnumerator Brew()
        {
            status = Status.Busy;

            display.DisplatStatus(status);

            coffeeGrinder.StartProcessing(12);

            yield return new WaitUntil(() => coffeeGrinder.Status == Status.Idle);

            waterPump.StartProcessing(100);

            yield return new WaitUntil(() => waterPump.Status == Status.Idle);

            StopBrewing();
        }

        private void CheckForWarnings()
        {
            List<Warning> warnings = new List<Warning>();

            if (groundsLevelSensor.Status == ContainerSensorStatus.High) warnings.Add(Warning.GroundsContainerFull);
            if (dripLevelSensor.Status == ContainerSensorStatus.High) warnings.Add(Warning.DripTrayFull);
            if (waterLevelSensor.Status == ContainerSensorStatus.Low) warnings.Add(Warning.LowOnWater);

            hasWarnings = warnings.Count > 0;

            if (hasWarnings)
            {
                if (display != null) display.DisplayWarning(warnings[0]);
                if (status == Status.Busy) StopBrewing();
            }
        }
    }
}