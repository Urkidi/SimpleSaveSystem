using System;

namespace SimpleSaveSystem
{
    [Serializable]
    public class SaveSlotMetaData
    {
        public string Id;
        public DateTime LastModified;
        public string Version;
        public string Hash;
    }
}