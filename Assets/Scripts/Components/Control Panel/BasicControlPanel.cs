using Machine.Enums;

namespace Machine.Components
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

        public override void OnWarnings(Warning[] warnings)
        {
            if (warnings.Length == 0)
            {
                ClearWarnings();
                return;
            }

            display.DisplayWarning(warnings[0]);
        }

        private void ClearWarnings()
        {
            display.DisplayStatus(oldStatus);
        }
    }
}