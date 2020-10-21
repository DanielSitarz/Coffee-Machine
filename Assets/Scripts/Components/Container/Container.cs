using Machine.Events;
using UnityEngine;

namespace Machine.Component
{
    [ExecuteInEditMode, RequireComponent(typeof(ContainerSensor))]
    public class Container : MonoBehaviour
    {
        [SerializeField, Header("Capacity and Amount in grams."), Min(0)]
        private float maxCapacity;

        [SerializeField, Min(0)]
        private float currentAmount;

        public FloatEvent OnAmountChange;
        public FloatEvent OnAmount01Change; // Normalized to 0..1

        public ContainerSensor Sensor { get { return Sensor; } }
        private ContainerSensor sensor;

        public float MaxCapacity { get { return maxCapacity; } }
        public float CurrentAmount { get { return currentAmount; } set { SetAmount(value); } }
        public float Current01Amount { get { return currentAmount / maxCapacity; } }

        void Start()
        {
            sensor = GetComponent<ContainerSensor>();
        }

        void OnEnable()
        {
            SendEvents();
        }

        void OnValidate()
        {
            CurrentAmount = currentAmount;
            SendEvents();
        }

        public void Fill(float amountToAdd)
        {
            CurrentAmount += amountToAdd;
        }

        public float Take(float amountToTake)
        {
            var amountTaken = amountToTake;

            if (currentAmount - amountToTake < 0)
            {
                amountTaken = currentAmount;
            }

            CurrentAmount -= amountTaken;

            return amountTaken;
        }

        private void SetAmount(float value)
        {
            if (value <= maxCapacity)
            {
                currentAmount = value;
            }
            else
            {
                currentAmount = maxCapacity;
            }

            if (currentAmount < 0) currentAmount = 0;

            SendEvents();
        }

        private void SendEvents()
        {
            OnAmountChange.Invoke(currentAmount);
            OnAmount01Change.Invoke(Current01Amount);
        }
    }
}