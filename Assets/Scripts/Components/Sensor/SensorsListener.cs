using System.Collections;
using System.Collections.Generic;
using Machine.Enums;
using Machine.Events;
using UnityEngine;

namespace Machine.Components
{
    // TODO: Change from pooling sensors every x seconds to be event driven.

    ///<summary>
    /// Check all attached sensors x times per second and listens for their warnings.
    ///</summary>
    public class SensorsListener : MonoBehaviour, ITurnable
    {
        [Header("Times per second.")]
        public float checkFrequency = 10;

        [SerializeField, Header("The importance of the sensors depends on the order.")]
        private ContainerSensor[] sensors;

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
