namespace SimpleSaveSystem.Services
{
    public interface IUriProvider
    {
        string MetaDataUri { get; }
        string SaveDataUri { get; }
        
        string GetSlotUri(string saveId);
    }
}