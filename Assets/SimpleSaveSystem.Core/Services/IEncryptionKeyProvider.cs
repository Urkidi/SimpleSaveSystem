namespace SimpleSaveSystem.Services
{
    public interface IEncryptionKeyProvider
    {
        byte[] Key { get; }
    }
}