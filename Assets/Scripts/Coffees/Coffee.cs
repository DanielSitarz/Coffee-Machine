using UnityEngine;

namespace Machine
{
    [CreateAssetMenu(fileName = "BasicCoffee", menuName = "Coffees/Basic", order = 1)]
    public class Coffee : ScriptableObject
    {
        public string id;
        public string coffeeName;
        [Tooltip("In grams.")]
        public int coffeeAmount;
        [Tooltip("In milliliters.")]
        public int waterAmount;
    }
}
