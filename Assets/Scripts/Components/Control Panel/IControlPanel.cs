using Machine.Enums;

namespace Machine.Components
{
    public interface IControlPanel
    {
        void OnStatusChange(Status status);
        void OnWarning(Warning warning);
    }
}