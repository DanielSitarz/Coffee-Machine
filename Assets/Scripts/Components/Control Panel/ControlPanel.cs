using Machine.Enums;
using UnityEngine;

namespace Machine.Components
{
    public abstract class ControlPanel : MonoBehaviour, IControlPanel, ITurnable
    {
        [Tooltip("Enables debug logs.")]
        public bool debug = false;

        public abstract void OnStatusChange(Status newStatus);
        public abstract void OnWarnings(Warning[] warnings);
        public abstract void TurnOff();
        public abstract void TurnOn();
    }
}