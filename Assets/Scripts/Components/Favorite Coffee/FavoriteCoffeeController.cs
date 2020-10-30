using System.Collections.Generic;
using DanielSitarz.MyLog;
using Machine.Enums;
using Machine.State;
using UnityEngine;

namespace Machine.Components
{
    ///<summary>
    /// Holds list of coffees. Handles adding new favorite coffee, setting it as current in CoffeeMachine and toggling between saved ones. Can save/load its state. 
    ///</summary>
    public class FavoriteCoffeeController : MonoBehaviour, ISaveable
    {
        public bool debug = false;

        [SerializeField]
        private CoffeeMachine coffeeMachine;
        [SerializeField]
        private Display display;
        [SerializeField, Space()]
        private List<Coffee> favoriteCoffees = new List<Coffee>();

        private int currentIndex = 0;
        private string customName = ""; // propably from InputField somewhere.

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

            if (coffee == null)
            {
                display.DisplayTimedMsg(DisplayMessage.NoFavorite);
                return;
            }

            coffeeMachine.SetCoffee(coffee);

            MyLog.TryLog(this, $"Set favorite - i: {currentIndex}", debug);
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

            MyLog.TryLog(this, $"Saved as favorite - {favCoffee.coffeeName}", debug);
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

        public void Save(string baseId)
        {
            FavoriteCoffeeState state = new FavoriteCoffeeState()
            {
                favoriteCoffees = favoriteCoffees,
                currentIndex = currentIndex
            };

            SaveLoadSystem.Save<FavoriteCoffeeState>(state, baseId, "FavoriteCoffeeController");
        }

        public void Load(string baseId)
        {
            var state = SaveLoadSystem.Load<FavoriteCoffeeState>(baseId, "FavoriteCoffeeController");
            if (state == null) state = new FavoriteCoffeeState();

            favoriteCoffees = state.favoriteCoffees;
            currentIndex = state.currentIndex;
        }
    }
}