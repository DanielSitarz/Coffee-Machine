using System;
using Machine.Enums;
using UnityEngine;

namespace Machine
{
    ///<summary>
    /// Basic coffee. In future we could make different basic types of coffees like Latte or Americano and set as default in CoffeeMachine.
    ///</summary>
    [CreateAssetMenu(fileName = "BasicCoffee", menuName = "Coffees/Basic", order = 1), Serializable]
    public class Coffee : ScriptableObject
    {
        public string coffeeName = "";
        public CoffeeStrength strength = CoffeeStrength.Normal;
        public CoffeeSize size = CoffeeSize.Medium;
    }
}
