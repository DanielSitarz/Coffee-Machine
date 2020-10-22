using System.Collections;
using System.Collections.Generic;
using Machine.Enums;
using Machine.Events;
using UnityEngine;

namespace Machine.Components
{
    public class SensorsListener : MonoBehaviour
    {
        [Header("Times per second.")]
        public float checkFrequency = 10;

        [Header("The importance of the sensors depends on the order.")]
        [SerializeField] private ContainerSensor[] sensors;

        public bool HasWarnings { get { return warnings.Count > 0; } }

        public MultiWarningsEvent OnWarnings;

        private List<Warning> warnings = new List<Warning>();
        private Status status = Status.Off;

        public void TurnOn()
        {
            StartListening();
        }

        public void TurnOff()
        {
            StopListening();
        }

        private void StartListening()
        {
            status = Status.Busy;

            StartCoroutine(Check());
        }

        private void StopListening()
        {
            status = Status.Off;
        }

        private IEnumerator Check()
        {
            while (status == Status.Busy)
            {
                CheckWarnings();

                yield return new WaitForSeconds(1.0f / checkFrequency);
            }
        }

        private void CheckWarnings()
        {
            warnings.Clear();

            foreach (ContainerSensor sensor in sensors)
            {
                if (sensor.Warning != Warning.None) warnings.Add(sensor.Warning);
            }

            OnWarnings.Invoke(warnings.ToArray());
        }
    }
}
