using System.Collections.Generic;

namespace SimpleSaveSystem.Core
{
    public interface ISaveIOService<T>
    {
        List<string> SaveSlotIds { get; }
        string LastSavedId { get; }
        bool TryLoadCreate(string id, out T save);
        bool TryLoad(string id, out T save);
        bool TrySave(string id, T saveData);
    }
}