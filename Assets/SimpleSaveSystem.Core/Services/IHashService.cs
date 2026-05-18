namespace SimpleSaveSystem.Core.Services
{
    public interface IHashService
    {
        byte[] HashData(byte[] data);
        bool VerifyHash(byte[] data, byte[] hash);
    }
}