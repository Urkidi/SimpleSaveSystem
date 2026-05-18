namespace SimpleSaveSystem.Core.Services
{
    public interface IEncryptionKeyProvider
    {
        byte[] Key { get; }
    }
}