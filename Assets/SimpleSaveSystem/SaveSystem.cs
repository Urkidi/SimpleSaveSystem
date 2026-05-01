using System.Collections.Generic;
using UnityEngine;

namespace SimpleSaveSystem
{
    public class SaveSystem<T> : ISaveSystem<T>
    {
        private readonly ISaveIOService<T> _saveIOService;
        private string _currentSaveId;
        public T SaveData { get; private set; }
        public List<string> SaveGameIds => _saveIOService.SaveSlotIds;

        public SaveSystem(ISaveIOService<T> saveIOService)
        {
            _saveIOService = saveIOService;
            if (SaveGameIds.Count == 0)
            {
                LoadCreateSave(SaveGameIds.Count.ToString());
            }
            else
            {
                _currentSaveId = _saveIOService.LastSavedId;
                LoadSaveInternal(_currentSaveId);
            }
        }

        public void SaveCurrentSave()
        {
            _saveIOService.Save(_currentSaveId, SaveData);
        }

        public void LoadSave(string id)
        {
            SaveCurrentSave();
            LoadSaveInternal(id);
        }

        public void CreateSave()
        {
            SaveCurrentSave();
            LoadCreateSave(SaveGameIds.Count.ToString());
        }

        private void LoadCreateSave(string id)
        {
            if (_saveIOService.TryLoadCreate(id, out var save))
            {
                SaveData = save;
                _currentSaveId = SaveGameIds[0];
            }
            else
            {
                Debug.LogError($"Save Error: Unable to create new savegame of id: {SaveGameIds.Count.ToString()}");
            }
        }

        private void LoadSaveInternal(string saveId)
        {
            if(_saveIOService.TryLoad(saveId, out var save))
            {
                SaveData = save;
            }
            else
            {
                Debug.LogError($"Save Error: Unable to load new savegame of id: {_currentSaveId}");
            }
        }
    }
}