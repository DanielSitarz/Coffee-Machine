using System;
using Machine.Enums;
using RotaryHeart.Lib.SerializableDictionary;

namespace Machine.Dictionaries
{
    [Serializable]
    public class SensorStatusWarningDictionary : SerializableDictionaryBase<SensorStatus, Warning> { }
}