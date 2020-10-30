using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using UnityEngine;

namespace Machine
{
    ///<summary>
    /// Saves or loads serializable objects. Now using binary JSON format.
    ///</summary>
    public static class SaveLoadSystem
    {
        public static void Save<T>(T objToSave, string baseId, string objId)
        {
            MemoryStream ms = new MemoryStream();
            using (BsonWriter writer = new BsonWriter(ms))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(writer, objToSave);
            }
            string data = Convert.ToBase64String(ms.ToArray());

            PlayerPrefs.SetString($"{baseId}/{objId}", data);
        }

        public static T Load<T>(string baseId, string objId)
        {
            var bson = PlayerPrefs.GetString($"{baseId}/{objId}", "e30=");

            byte[] data = Convert.FromBase64String(bson);

            T state = default(T);

            MemoryStream ms = new MemoryStream(data);
            using (BsonReader reader = new BsonReader(ms))
            {
                JsonSerializer serializer = new JsonSerializer();

                state = serializer.Deserialize<T>(reader);
            }

            return state;
        }
    }
}