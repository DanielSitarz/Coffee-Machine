using Machine.Events;
using UnityEngine;

namespace Machine.Components
{
    [ExecuteInEditMode]
    public class BasicContainer : Container
    {
        void OnEnable()
        {
            SendEvents();
        }

        void OnValidate()
        {
            CurrentAmount = currentAmount;
            SendEvents();
        }

        public override void Fill(float amountToAdd)
        {
            CurrentAmount += amountToAdd;
        }

        public override float Take(float amountToTake)
        {
            var amountTaken = amountToTake;

            if (currentAmount - amountToTake < 0)
            {
                amountTaken = currentAmount;
            }

            CurrentAmount -= amountTaken;

            return amountTaken;
        }

        public override void SetAmount(float value)
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