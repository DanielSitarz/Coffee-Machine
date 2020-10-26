using Machine.Enums;
using UnityEngine;

namespace Machine.Components
{
    public abstract class ControlPanel : MonoBehaviour, ITurnable
    {
        [Tooltip("Enables debug logs.")]
        public bool debug = false;

        protected abstract void OnStatusChange(Status newStatus);
        protected abstract void OnWarnings(Warning[] warnings);
        public abstract void TurnOff();
        public abstract void TurnOn();
    }
}