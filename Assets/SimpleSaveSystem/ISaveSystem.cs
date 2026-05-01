using System.Collections.Generic;

namespace SimpleSaveSystem
{
    public interface ISaveSystem<out T>
    {
        T SaveData { get;}
        List<string> SaveGameIds { get; }
        void SaveCurrentSave();
        void LoadSave(string id);
        void CreateSave();
    }
}