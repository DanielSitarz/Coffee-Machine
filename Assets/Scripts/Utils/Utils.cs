using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine;

namespace Machine
{
    public static class Utils
    {
        public static int ToggleNumber(int num, int max, int start = 0)
        {
            if (num > max)
            {
                num = start;
            }

            return num;
        }

        public static T TryGetValueWithDefault<T, D>(D dict, T key, T defaultVal) where D : SerializableDictionaryBase<T, T>
        {
            T val;

            bool knowsAmount = dict.TryGetValue(key, out val);

            if (!knowsAmount)
            {
                val = defaultVal;
            }

            return val;
        }

        public static void DebugLog(Object obj, string msg, bool debug = false)
        {
            if (!debug) return;
            Debug.Log($"{obj.name} - {msg}");
        }
    }
}