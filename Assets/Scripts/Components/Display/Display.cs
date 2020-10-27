using Machine.Enums;
using UnityEngine;

namespace Machine
{
    public abstract class Display : MonoBehaviour, ITurnable
    {
        public Status status;

        public abstract void DisplayWarning(Warning warning);
        public abstract void DisplayStatus(Status status);
        public abstract void DisplayTimedMsg(DisplayMessage msg, string additionalText);
        public abstract void ClearTimedMsg();
        public abstract void ClearWarning();

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
