using System;
using UnityEngine;

public class UniqueID : MonoBehaviour
{
    public string uid = GetUID();

    public static string GetUID()
    {
        return Guid.NewGuid().ToString();
    }
}
