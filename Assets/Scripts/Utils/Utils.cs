using System.Collections.Generic;
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

            if (num < start)
            {
                num = max;
            }

            return num;
        }

        public static T TryGetValueOrDefault<T, D>(D dict, T key, T defaultVal) where D : IDictionary<T, T>
        {
            T val;

            bool dictValue = dict.TryGetValue(key, out val);

            if (!dictValue)
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