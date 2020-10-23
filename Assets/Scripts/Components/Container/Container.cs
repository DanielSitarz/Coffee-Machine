using Machine.Events;
using UnityEngine;

namespace Machine.Components
{
    public abstract class Container : MonoBehaviour, IContainer
    {
        [SerializeField, Header("Capacity and Amount in grams."), Min(0)]
        protected float maxCapacity = 100;

        [SerializeField, Min(0)]
        protected float currentAmount = 0;

        public FloatEvent OnAmountChange = new FloatEvent();
        public FloatEvent OnAmount01Change = new FloatEvent(); // Normalized to 0..1

        public float MaxCapacity { get { return maxCapacity; } }
        public float CurrentAmount { get { return currentAmount; } set { SetAmount(value); } }
        public float Current01Amount { get { return currentAmount / maxCapacity; } }

        public abstract void Fill(float amountToAdd);
        public abstract float Take(float amountToTake);
        public abstract void SetAmount(float amount);
    }
}