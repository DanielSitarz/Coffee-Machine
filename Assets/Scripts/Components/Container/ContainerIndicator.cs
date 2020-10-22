using UnityEngine;
using Machine.Events;

namespace Machine.Components
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

            containerToObserve.OnAmount01Change.AddListener(OnAmountPercentChange);
        }

        void OnDisable()
        {
            if (containerToObserve == null) { return; }

            containerToObserve.OnAmount01Change.RemoveListener(OnAmountPercentChange);
        }

        public void OnAmountPercentChange(float value)
        {
            amountPercentChangeEvent.Invoke(value);
        }
    }
}
