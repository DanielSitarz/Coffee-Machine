using UnityEngine;

namespace Machine
{
    [CreateAssetMenu(fileName = "BasicCoffee", menuName = "Coffees/Basic", order = 1)]
    public class Coffee : ScriptableObject
    {
        public string id;
        public string coffeeName;
        public float coffeeAmount;
        public float waterAmount;
    }
}
