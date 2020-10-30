namespace Machine.Components
{
    public interface IContainer
    {
        float MaxCapacity { get; }
        float CurrentAmount { get; set; }
        float Current01Amount { get; }

        void Fill(float amount);
        float Take(float amount);
        void SetAmount(float amount);
    }
}
