using System;
using System.Collections.Generic;

namespace SimpleSaveSystem.Core
{
    public interface ISaveSystem<out T>
    {
        T SaveData { get;}
        string LastSavedId { get; }
        List<string> SaveGameIds { get; }
        
        void SaveCurrentSave();
        void LoadSave(string saveId);
        void CreateNewSave(string saveId);
        
        DateTime GetLastModifiedSaveDate(string saveId);
        string GetSaveVersion(string saveId);
    }
}