using Machine.Enums;

namespace Machine
{
    public interface IDisplay
    {
        void DisplayWarning(Warning warning);
        void DisplayStatus(Status status);
    }
}