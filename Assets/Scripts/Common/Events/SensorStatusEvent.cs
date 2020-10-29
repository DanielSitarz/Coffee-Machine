using System;
using Machine.Enums;
using UnityEngine.Events;

namespace Machine.Events
{
    [Serializable]
    public class SensorStatusEvent : UnityEvent<SensorStatus> { }
}