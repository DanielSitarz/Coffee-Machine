using Machine.Enums;
using UnityEngine;

namespace Machine.Components
{
    public abstract class ControlPanel : MonoBehaviour, IControlPanel
    {
        public abstract void OnStatusChange(Status newStatus);
        public abstract void OnWarnings(Warning[] warnings);
    }
}