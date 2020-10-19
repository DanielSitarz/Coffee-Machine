using UnityEngine;
using Utils;

namespace CoffeeMachine
{
    [ExecuteInEditMode]
    public class ContainerIndicator : MonoBehaviour
    {
        [SerializeField] private Container containerToObserve;

        [SerializeField, Header("Amount in 0..1")]
        private FloatEvent amountPercentChangeEvent;

        void OnEnable()
        {
            if (containerToObserve == null) { return; }

            containerToObserve.OnAmountPercentChange.AddListener(OnAmountPercentChange);
        }

        void OnDisable()
        {
            if (containerToObserve == null) { return; }

            containerToObserve.OnAmountPercentChange.RemoveListener(OnAmountPercentChange);
        }

        public void OnAmountPercentChange(float value)
        {
            amountPercentChangeEvent.Invoke(value);
        }
    }
}
