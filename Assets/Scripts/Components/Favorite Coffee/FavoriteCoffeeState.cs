using System;
using System.Collections.Generic;

namespace Machine.State
{
    [Serializable]
    public class FavoriteCoffeeState
    {
        public List<Coffee> favoriteCoffees = new List<Coffee>();
        public int currentIndex = 0;
    }
}
