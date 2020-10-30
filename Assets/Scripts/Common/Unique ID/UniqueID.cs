using System;
using UnityEngine;

namespace Machine
{
    ///<summary>
    /// Holds unique ID. Useful for Save/Load system.
    ///</summary>
    public class UniqueID : MonoBehaviour
    {
        public string uid = GetUID();

        public static string GetUID()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
