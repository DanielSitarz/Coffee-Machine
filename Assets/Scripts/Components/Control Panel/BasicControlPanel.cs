using Machine.Enums;
using UnityEngine;

namespace Machine.Component
{
    public class BasicControlPanel : ControlPanel
    {
        public Display display;

        private Status oldStatus = Status.Off;

        public override void OnStatusChange(Status newStatus)
        {
            if (oldStatus == newStatus) return;

            oldStatus = newStatus;

            display.DisplayStatus(newStatus);
        }

        public override void OnWarning(Warning warning)
        {
            display.DisplayWarning(warning);
        }

        private void ClearWarnings()
        {
            display.DisplayStatus(oldStatus);
        }
    }
}