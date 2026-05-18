using System.Text;
using SimpleSaveSystem.Core.Services;
using UnityEngine;

namespace SimpleSaveSystem.Defaults
{
    public class JsonSerializationService : ISerializationService
    {
        public byte[] Serialize<T>(T data)
        {
            return Encoding.UTF8.GetBytes(JsonUtility.ToJson(data));
        }

        public T Deserialize<T>(byte[] rawData)
        {
            return JsonUtility.FromJson<T>(Encoding.UTF8.GetString(rawData));
        } 
    }
}