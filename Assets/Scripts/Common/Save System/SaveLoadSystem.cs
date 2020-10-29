using Newtonsoft.Json;
using UnityEngine;

namespace Machine
{
    public static class SaveLoadSystem
    {
        public static void Save<T>(T objToSave, string baseId, string objId)
        {
            PlayerPrefs.SetString($"{baseId}/{objId}", JsonConvert.SerializeObject(objToSave));
        }

        public static T Load<T>(string baseId, string objId)
        {
            var json = PlayerPrefs.GetString($"{baseId}/{objId}", "{}");
            T state = JsonConvert.DeserializeObject<T>(json);

            if (state == null) state = default(T);

            return state;
        }
    }
}