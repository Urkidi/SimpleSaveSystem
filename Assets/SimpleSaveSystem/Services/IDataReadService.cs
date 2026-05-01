namespace SimpleSaveSystem.Services
{
    public interface IDataReadService
    {
        bool TryRead(string uri, out byte[] data);
    }
}