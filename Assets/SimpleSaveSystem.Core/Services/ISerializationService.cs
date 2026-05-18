namespace SimpleSaveSystem.Core.Services
{
    public interface ISerializationService
    {
        byte[] Serialize<T>(T data);
        T Deserialize<T>(byte[] rawData);
    }
}