using Machine.Enums;
using UnityEngine;

namespace Machine
{
    public abstract class Display : MonoBehaviour, IDisplay, ITurnable
    {
        public Status status;

        public abstract void DisplayWarning(Warning warning);
        public abstract void DisplayStatus(Status status);
        public abstract void DisplayTimedMsg(string msg);
        public abstract void ClearTimedMsg();

        public virtual void TurnOn()
        {
            status = Status.Idle;
        }

        public virtual void TurnOff()
        {
            status = Status.Off;
        }
    }
}
