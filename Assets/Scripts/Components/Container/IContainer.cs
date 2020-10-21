using Machine.Events;

namespace Machine.Component
{
    public interface IContainer
    {
        float MaxCapacity { get; }
        float CurrentAmount { get; set; }
        float CurrentPercentAmount { get; }

        void Fill(float amount);
        void Take(float amount);

        FloatEvent OnAmountPercentChange { get; set; }
    }
}
