using System.Collections.Generic;

namespace SimpleSaveSystem
{
    public interface ISaveIOService<T>
    {
        List<string> SaveSlotIds { get; }
        string LastSavedId { get; }
        bool TryLoadCreate(string id, out T save);
        bool TryLoad(string id, out T save);
        void Save(string id, T saveData);
    }
}