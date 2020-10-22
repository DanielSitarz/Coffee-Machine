using Machine.Enums;
using UnityEngine;

namespace Machine.Components
{
    public class Supplier : MonoBehaviour
    {
        [Header("Takes from container. Fills other container with leftovers.")]
        public float processingAmountPerSecond = 10.0f;
        public float leftoverAmountPerGram = 1.0f;

        [SerializeField] private Container container;
        [SerializeField] private Container leftoverContainer;

        private float amountToProcessLeft = 0.0f;

        public Status Status { get { return amountToProcessLeft > 0 ? Status.Busy : Status.Idle; } }

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
            float processedAmount = processingAmountPerSecond * Time.deltaTime;
            float takenAmountFromContainer = container.Take(processedAmount);
            float leftoverAmount = takenAmountFromContainer * leftoverAmountPerGram;

            if (leftoverContainer != null)
                leftoverContainer.Fill(leftoverAmount);

            amountToProcessLeft -= takenAmountFromContainer;
        }
    }
}