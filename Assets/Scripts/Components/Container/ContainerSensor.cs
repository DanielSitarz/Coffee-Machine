using UnityEngine;

namespace Machine.Component
{
    public class ContainerSensor : MonoBehaviour
    {
        public Container observedContainer;

        public ContainerSensorStatus Status { get { return status; } }
        private ContainerSensorStatus status;

        [SerializeField, Range(0, 100)] private float lowLevelThreshold = 10;
        [SerializeField, Range(0, 100)] private float highLevelThreshold = 90;

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
            float value = value01 * 100f;

            switch (value)
            {
                case float _ when value <= lowLevelThreshold:
                    status = ContainerSensorStatus.Low;
                    break;
                case float _ when value > lowLevelThreshold && value < highLevelThreshold:
                    status = ContainerSensorStatus.Normal;
                    break;
                case float _ when value >= highLevelThreshold:
                    status = ContainerSensorStatus.High;
                    break;
            }
        }
    }
}