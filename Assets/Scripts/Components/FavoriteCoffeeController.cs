using System.Collections.Generic;
using Machine.Enums;
using UnityEngine;
using UnityEngine.UI;

namespace Machine.Components
{
    public class FavoriteCoffeeController : MonoBehaviour
    {
        [SerializeField]
        private CoffeeMachine coffeeMachine;
        [SerializeField]
        private Display display;
        [SerializeField, Space()]
        private List<Coffee> favoriteCoffees = new List<Coffee>();

        private int currentIndex = 0;
        private string customName = "";

        void Start()
        {
            if (coffeeMachine == null)
            {
                Debug.LogError("Favorite controller needs Coffee Machine.");
            }
        }

        public void SetFavorite()
        {
            if (!coffeeMachine.Operational) return;

            var coffee = GetFavorite(currentIndex);

            coffeeMachine.SetCoffee(coffee);
        }

        public void SaveAsFavorite()
        {
            if (!coffeeMachine.Operational) return;

            string coffeeName = customName != null ? customName : $"My favorite#{favoriteCoffees.Count + 1}";

            Coffee favCoffee = new Coffee()
            {
                coffeeName = coffeeName,
                size = coffeeMachine.CurrentCoffee.size,
                strength = coffeeMachine.CurrentCoffee.strength
            };

            favoriteCoffees.Add(favCoffee);

            customName = null;

            if (display != null) display.DisplayTimedMsg(DisplayMessage.SaveCoffeeAsFavorite, coffeeName);
        }

        public void SetCustomName(string newName)
        {
            customName = newName;
        }

        public void Next()
        {
            currentIndex = Utils.ToggleNumber(currentIndex + 1, favoriteCoffees.Count - 1);
            SetFavorite();
        }

        public void Previous()
        {
            currentIndex = Utils.ToggleNumber(currentIndex - 1, favoriteCoffees.Count - 1);
            SetFavorite();
        }

        private Coffee GetFavorite(int index)
        {
            if (index > favoriteCoffees.Count - 1) return null;

            var coffee = favoriteCoffees[index];

            return coffee;
        }
    }
}