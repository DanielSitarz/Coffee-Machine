using System;
using Machine.Enums;

namespace Machine.State
{
    [Serializable]
    public class CoffeeMachineState
    {
        public Status status = Status.Off;
        public Coffee currentCoffee = new Coffee();
        public CoffeeStrength coffeeStrength = CoffeeStrength.Normal;
        public CoffeeSize coffeeSize = CoffeeSize.Medium;
        public bool coffeeSetFromOutside = false;
    }
}
