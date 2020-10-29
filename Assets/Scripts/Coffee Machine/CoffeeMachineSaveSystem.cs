using Machine;
using UnityEngine;

[RequireComponent(typeof(UniqueID))]
public class CoffeeMachineSaveSystem : MonoBehaviour
{
    UniqueID baseId;
    ISaveable[] saveables;

    void OnEnable()
    {
        baseId = GetComponent<UniqueID>();
        saveables = GetComponentsInChildren<ISaveable>();

        foreach (var saveable in saveables)
        {
            saveable.Load(baseId.uid);
        }
    }

    void OnDisable()
    {
        foreach (var saveable in saveables)
        {
            saveable.Save(baseId.uid);
        }
    }
}
