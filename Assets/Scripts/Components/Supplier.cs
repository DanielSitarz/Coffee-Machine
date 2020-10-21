using UnityEngine;

namespace Machine.Component
{
    public class Supplier : MonoBehaviour
    {
        public float processingAmountPerSecond = 10.0f;
        public float leftoutAmountPerGram = 1.0f;

        public Status Status { get { return GetStatus(); } }

        [SerializeField] private Container container;
        [SerializeField] private Container leftoutContainer;

        private float amountToProcessLeft = 0.0f;

        void Update()
        {
            if (amountToProcessLeft > 0)
            {
                Process();
            }
        }

        public void StartProcessing(float amountInGrams)
        {
            amountToProcessLeft = amountInGrams;
        }

        public void StopProcessing()
        {
            amountToProcessLeft = 0.0f;
        }

        private void Process()
        {
            float grindedAmount = processingAmountPerSecond * Time.deltaTime;
            float takenAmountFromContainer = container.Take(grindedAmount);
            float leftoutsAmount = takenAmountFromContainer * leftoutAmountPerGram;

            leftoutContainer.Fill(leftoutsAmount);

            amountToProcessLeft -= takenAmountFromContainer;
        }

        private Status GetStatus()
        {
            if (amountToProcessLeft > 0)
            {
                return Status.Busy;
            }

            return Status.Idle;
        }
    }
}