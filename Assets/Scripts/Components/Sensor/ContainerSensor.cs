using Machine.Enums;
using Machine.Events;
using UnityEngine;

namespace Machine.Components
{
    public class ContainerSensor : MonoBehaviour
    {
        public Container observedContainer;

        public SensorStatus Status { get { return status; } }
        private SensorStatus status;

        [SerializeField, Range(0, 100)] private float lowLevelThreshold = 10;
        [SerializeField, Range(0, 100)] private float highLevelThreshold = 90;

        public SensorStatusEvent OnStatusChange;

        void OnEnable()
        {
            UpdateStatus(observedContainer.Current01Amount);
            observedContainer.OnAmount01Change.AddListener(UpdateStatus);
        }

        void OnDisable()
        {
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
        }
    }
}