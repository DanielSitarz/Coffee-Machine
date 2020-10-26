using Machine.Enums;
using UnityEngine;

namespace Machine.Components
{
    // TODO: Maybe coroutine?
    // TODO: StartProcessing until empty?
    public class Supplier : MonoBehaviour
    {
        [Header("Transfers stuff from one container to another. Can also only take or fill.")]
        public float processingAmountPerSecond = 10.0f;
        [Tooltip("How much units are transferred? E.g. 0.5 means that half will land in second container.")]
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
            amountToProcessLeft = amount;
        }

        public void StopProcessing()
        {
            amountToProcessLeft = 0.0f;
        }

        private void Process()
        {
            float processedAmount = processingAmountPerSecond * Time.deltaTime;
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