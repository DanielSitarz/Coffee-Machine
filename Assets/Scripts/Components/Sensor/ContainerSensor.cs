using System;
using Machine.Dictionaries;
using Machine.Enums;
using Machine.Events;
using UnityEngine;

namespace Machine.Component
{
    public class ContainerSensor : MonoBehaviour
    {
        public Container observedContainer;

        public SensorStatus Status { get { return status; } }
        private SensorStatus status;

        [SerializeField, Range(0, 100)] private float lowLevelThreshold = 10;
        [SerializeField, Range(0, 100)] private float highLevelThreshold = 90;

        public SensorStatusWarningDictionary sensorStatusToWarning;

        public SensorStatusEvent OnStatusChange;
        public SingleWarningEvent OnWarning;

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
                    status = SensorStatus.Low;
                    break;
                case float _ when value > lowLevelThreshold && value < highLevelThreshold:
                    status = SensorStatus.Normal;
                    break;
                case float _ when value >= highLevelThreshold:
                    status = SensorStatus.High;
                    break;
            }

            OnStatusChange.Invoke(status);
            CheckWarning(status);
        }

        private void CheckWarning(SensorStatus status)
        {
            Warning warning;
            bool hasWarningForThisStatus = sensorStatusToWarning.TryGetValue(status, out warning);

            if (hasWarningForThisStatus)
            {
                OnWarning.Invoke(warning);
            }
        }
    }
}