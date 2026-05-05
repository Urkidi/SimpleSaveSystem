using System;
using System.Collections.Generic;

namespace SimpleSaveSystem.Core.Data
{
    [Serializable]
    public class SaveIndexData
    {
        public string LastSavedSlotId;
        public List<SaveSlotMetaData> SaveSlots;
    }
}