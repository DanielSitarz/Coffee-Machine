using UnityEngine;

namespace Machine
{
    public static class SaveLoadSystem
    {
        public static void Save<T>(T objToSave, string baseId, string objId)
        {
            PlayerPrefs.SetString($"{baseId}/{objId}", JsonUtility.ToJson(objToSave));
        }

        public static T Load<T>(string baseId, string objId)
        {
            var json = PlayerPrefs.GetString($"{baseId}/{objId}", "{}");
            T state = JsonUtility.FromJson<T>(json);

            if (state == null) state = default(T);

            return state;
        }
    }
}