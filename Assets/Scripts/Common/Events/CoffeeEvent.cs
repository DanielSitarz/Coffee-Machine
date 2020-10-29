using System;
using UnityEngine.Events;

namespace Machine.Events
{
    [Serializable]
    public class CoffeeEvent : UnityEvent<Coffee> { }
}