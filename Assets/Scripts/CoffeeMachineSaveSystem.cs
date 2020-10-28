using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoffeeMachineSaveSystem : MonoBehaviour
{
    ISaveable[] saveables;

    void OnEnable()
    {
        saveables = GetComponentsInChildren<ISaveable>();

        foreach (var saveable in saveables)
        {
            saveable.Load();
        }
    }

    void OnDisable()
    {
        foreach (var saveable in saveables)
        {
            saveable.Save();
        }
    }
}
