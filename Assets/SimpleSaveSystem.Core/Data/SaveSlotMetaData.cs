using System;

namespace SimpleSaveSystem.Core.Data
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