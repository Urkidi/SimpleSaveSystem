using SimpleSaveSystem.Services;

namespace SaveSystem.Example
{
    public class DefaultPathProvider : IUriProvider
    {
        public string MetaDataUri { get; }
        public string SaveDataUri { get; }
        public string GetSlotUri(string saveId)
        {
            throw new System.NotImplementedException();
        }
    }
}