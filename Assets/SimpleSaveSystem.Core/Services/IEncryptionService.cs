namespace SimpleSaveSystem.Core.Services
{
    public interface IEncryptionService
    {
        byte[] EncryptData(byte[] rawData);
        byte[] DecryptData(byte[] data);
    }
    
    
}