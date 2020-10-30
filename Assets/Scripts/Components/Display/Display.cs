using Machine.Enums;
using UnityEngine;

namespace Machine.Components
{
    ///<summary>
    /// Handles diplaying various warnings/statuses/messages.
    ///</summary>
    public abstract class Display : MonoBehaviour, ITurnable
    {
        public Status status;

        public abstract void DisplayWarning(Warning warning, string additionalText = null);
        public abstract void DisplayStatus(Status status, string additionalText = null);
        public abstract void DisplayTimedMsg(DisplayMessage msg, string additionalText = null);
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
