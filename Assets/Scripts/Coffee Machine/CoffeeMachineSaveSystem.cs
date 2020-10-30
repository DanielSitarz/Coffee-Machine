using UnityEngine;

namespace Machine
{
    [RequireComponent(typeof(UniqueID))]
    public class CoffeeMachineSaveSystem : MonoBehaviour
    {
        UniqueID baseId;
        ISaveable[] saveables;

        void Start()
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
}

