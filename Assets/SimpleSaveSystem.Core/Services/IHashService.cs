namespace SimpleSaveSystem.Core.Services
{
    public interface IHashService
    {
        string GetHash(byte[] data);
        bool VerifyHash(byte[] data, string hash);
    }
}