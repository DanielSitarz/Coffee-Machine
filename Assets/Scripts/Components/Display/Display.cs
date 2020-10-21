using Machine.Enums;
using UnityEngine;

namespace Machine
{
    public abstract class Display : MonoBehaviour, IDisplay
    {
        public abstract void DisplayWarning(Warning warning);
        public abstract void DisplayStatus(Status status);
    }
}
