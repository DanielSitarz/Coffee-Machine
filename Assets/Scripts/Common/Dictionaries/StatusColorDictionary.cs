using System;
using Machine.Enums;
using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine;

namespace Machine.Dictionaries
{
    [Serializable]
    public class StatusColorDictionary : SerializableDictionaryBase<Status, Color> { }
}