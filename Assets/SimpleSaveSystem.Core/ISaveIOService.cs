using System;
using System.Collections.Generic;

namespace SimpleSaveSystem.Core
{
    public interface ISaveIOService<T>
    {
        List<string> SaveSlotIds { get; }
        string LastSavedId { get; }
        bool TryLoadCreate(string saveId, out T save);
        bool TryLoad(string saveId, out T save);
        bool TrySave(string saveId, T save);
        
        DateTime GetLastModifiedSaveDate(string saveId);
        string GetSaveVersion(string saveId);
    }
}