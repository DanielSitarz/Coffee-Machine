using Machine.Enums;
using UnityEngine;

namespace Machine.Components
{
    // TODO: Maybe coroutine instead of update?
    // TODO: Add StartProcessing until empty?

    public class Supplier : MonoBehaviour
    {
        [Header("Transfers stuff from one container to another. Can also only take or fill."), Tooltip("How much units are transferred in 1 second.")]
        [SerializeField]
        private float defaultFlowRate = 10.0f;
        private float currentFlowRate;

        [Tooltip("How much units are transferred? E.g. 0.5 means that half will land in second container. 2.0 will double.")]
        public float ratio = 1.0f;

        [SerializeField, Tooltip("If left null it will only fill the second container.")]
        private Container from;
        [SerializeField, Tooltip("If left null it will only take out from the first container.")]
        private Container to;

        private float amountToProcessLeft = 0.0f;

        public Status Status { get { return amountToProcessLeft > 0 ? Status.Busy : Status.Idle; } }

        void Update()
        {
            if (amountToProcessLeft > 0)
            {
                Process();
            }
        }

        public void StartProcessing(float amount)
        {
            StartProcessing(amount, 0);
        }

        public void StartProcessing(float amount, float flowRate = 0)
        {
            if (flowRate <= 0) flowRate = defaultFlowRate;

            currentFlowRate = flowRate;
            amountToProcessLeft = amount;
        }

        public void StopProcessing()
        {
            amountToProcessLeft = 0.0f;
        }

        private void Process()
        {
            float processedAmount = currentFlowRate * Time.deltaTime;
            float takenAmountFromContainer = processedAmount;

            if (from != null)
            {
                takenAmountFromContainer = from.Take(processedAmount);

                if (takenAmountFromContainer <= 0)
                {
                    StopProcessing();
                    return;
                }
            }

            if (to != null)
            {
                float transferredAmount = takenAmountFromContainer * ratio;
                to.Fill(transferredAmount);
            }

            amountToProcessLeft -= takenAmountFromContainer;
        }
    }
}