using UnityEngine;
using Utils;

namespace CoffeeMachine
{
    [ExecuteInEditMode]
    public class Container : MonoBehaviour, IContainer
    {
        [Header("Capacity and Amount in grams."), Min(0)]
        [SerializeField] private float maxCapacity;

        [Min(0)]
        [SerializeField] private float currentAmount;

        public FloatEvent OnAmountChange;
        public FloatEvent OnAmountPercentChange; // Normalized to 0..1

        public float MaxCapacity { get { return maxCapacity; } }
        public float CurrentAmount
        {
            get
            {
                return currentAmount;
            }
            set
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
        }

        void OnEnable()
        {
            SendEvents();
        }

        public void Fill(float amount)
        {
            CurrentAmount += amount;
        }

        public void Take(float amount)
        {
            CurrentAmount -= amount;
        }

        private void SendEvents()
        {
            OnAmountChange.Invoke(currentAmount);
            OnAmountPercentChange.Invoke(currentAmount / maxCapacity);
        }

        void OnValidate()
        {
            CurrentAmount = currentAmount;
            SendEvents();
        }
    }
}