namespace SimpleSaveSystem.Core.Services
{
    public interface IDataWriteService
    {
        void WriteData(string uri, byte[] data);
    }
}