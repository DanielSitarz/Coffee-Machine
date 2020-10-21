using Machine.Enums;

namespace Machine.Component
{
    public interface IControlPanel
    {
        void OnStatusChange(Status status);
        void OnWarning(Warning warning);
    }
}