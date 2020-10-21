using Machine.Enums;
using UnityEngine;

namespace Machine.Component
{
    public abstract class ControlPanel : MonoBehaviour, IControlPanel
    {
        public abstract void OnStatusChange(Status newStatus);
        public abstract void OnWarning(Warning warning);
    }
}