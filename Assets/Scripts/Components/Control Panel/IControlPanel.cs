using Machine.Enums;

namespace Machine.Components
{
    public interface IControlPanel
    {
        void OnStatusChange(Status status);
        void OnWarnings(Warning[] warnings);
    }
}