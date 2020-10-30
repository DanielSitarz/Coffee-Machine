using Machine.Dictionaries;
using Machine.Enums;
using Machine.Events;
using UnityEngine;

namespace Machine.Components
{
    ///<summary>
    /// Observes Container and react to it's current state. Raises events based on set thresholds.
    ///</summary>
    public class ContainerSensor : MonoBehaviour
    {
        public Container observedContainer;

        public SensorStatus Status { get { return status; } }
        private SensorStatus status = SensorStatus.Normal;

        public Warning Warning { get { return warning; } }
        private Warning warning = Warning.None;

        [SerializeField, Range(0, 100)] private float lowLevelThreshold = 10;
        [SerializeField, Range(0, 100)] private float highLevelThreshold = 90;

        public SensorStatusWarningDictionary sensorStatusToWarning;

        public SensorStatusEvent OnStatusChange;
        public SingleWarningEvent OnWarning;

        void OnEnable()
        {
            if (observedContainer == null)
            {
                Debug.LogWarning("Container sensor without container to observe.");
                return;
            }

            UpdateStatus(observedContainer.Current01Amount);
            observedContainer.OnAmount01Change.AddListener(UpdateStatus);
        }

        void OnDisable()
        {
            if (observedContainer == null) return;

            observedContainer.OnAmount01Change.RemoveListener(UpdateStatus);
        }

        private void UpdateStatus(float value01)
        {
            float percent = value01 * 100f;

            switch (percent)
            {
                case float _ when percent <= lowLevelThreshold:
                    status = SensorStatus.Low;
                    break;
                case float _ when percent > lowLevelThreshold && percent < highLevelThreshold:
                    status = SensorStatus.Normal;
                    break;
                case float _ when percent >= highLevelThreshold:
                    status = SensorStatus.High;
                    break;
            }

            OnStatusChange.Invoke(status);
            TryGetWarningForStatus(status);
        }

        private void TryGetWarningForStatus(SensorStatus status)
        {
            Warning warning;
            bool hasWarningForThisStatus = sensorStatusToWarning.TryGetValue(status, out warning);

            this.warning = hasWarningForThisStatus ? warning : Warning.None;

            OnWarning.Invoke(this.warning);
        }
    }
}