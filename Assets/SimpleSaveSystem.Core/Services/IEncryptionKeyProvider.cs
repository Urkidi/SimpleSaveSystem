namespace SimpleSaveSystem.Core.Services
{
    public interface IEncryptionKeyProvider
    {
        string Key { get; }
    }
}