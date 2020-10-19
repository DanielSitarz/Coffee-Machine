namespace CoffeeMachine
{
    internal interface IContainer
    {
        float MaxCapacity { get; }
        float CurrentAmount { get; set; }

        void Fill(float amount);
        void Take(float amount);
    }
}
