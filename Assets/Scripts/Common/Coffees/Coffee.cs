using System;
using Machine.Enums;
using UnityEngine;

namespace Machine
{
    [CreateAssetMenu(fileName = "BasicCoffee", menuName = "Coffees/Basic", order = 1), Serializable]
    public class Coffee : ScriptableObject
    {
        public string coffeeName = "";
        public CoffeeStrength strength = CoffeeStrength.Normal;
        public CoffeeSize size = CoffeeSize.Medium;
    }
}
