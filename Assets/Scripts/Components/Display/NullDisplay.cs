using Machine.Enums;

namespace Machine.Components
{
    ///<summary>
    /// Does nothing. CoffeeMachine's display is not required, so this one is used if we don't set any other.
    ///</summary>
    public class NoopDisplay : Display
    {
        public override void ClearTimedMsg()
        {

        }

        public override void ClearWarning()
        {

        }

        public override void DisplayStatus(Status status, string additionalText = null)
        {

        }

        public override void DisplayTimedMsg(DisplayMessage msg, string additionalText = null)
        {

        }

        public override void DisplayWarning(Warning warning, string additionalText = null)
        {

        }
    }
}
