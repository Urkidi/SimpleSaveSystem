using System;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleSaveSystem.Core
{
    public class SaveSystem<T> : ISaveSystem<T>
    {
        private readonly ISaveIOService<T> _saveIOService;
        private string _currentSaveId;
        public T SaveData { get; private set; }
        public string LastSavedId => _saveIOService.LastSavedId;
        public List<string> SaveGameIds => _saveIOService.SaveSlotIds;
        
        public SaveSystem(ISaveIOService<T> saveIOService)
        {
            _saveIOService = saveIOService;
            if (SaveGameIds.Count != 0)
                _currentSaveId = LastSavedId;
        }

        public void SaveCurrentSave()
        {
            _saveIOService.TrySave(_currentSaveId, SaveData);
        }

        public void LoadSave(string saveId)
        {
            LoadSaveInternal(saveId);
        }

        public void CreateNewSave(string saveId)
        {
            if(!string.IsNullOrEmpty(saveId) && !SaveGameIds.Contains(saveId))
                LoadCreateSave(saveId);
            else 
                Debug.LogError( String.Format( SaveErrorMessage.SaveIdAlreadyExists, saveId));
        }

        public DateTime GetLastModifiedSaveDate(string saveId)
        {
            return _saveIOService.GetLastModifiedSaveDate(saveId);
        }

        public string GetSaveVersion(string saveId)
        {
            return _saveIOService.GetSaveVersion(saveId);
        }

        public void CreateNewSave()
        {
            if(!string.IsNullOrEmpty(SaveGameIds.Count.ToString()) && !SaveGameIds.Contains(SaveGameIds.Count.ToString()))
                LoadCreateSave(SaveGameIds.Count.ToString());
            else 
                Debug.LogError( String.Format( SaveErrorMessage.IncrementalIdNamingIssue, SaveGameIds.Count.ToString()));
        }

        private void LoadCreateSave(string saveId)
        {
            if (_saveIOService.TryLoadCreate(saveId, out var save))
            {
                SaveData = save;
                _currentSaveId = saveId;
            }
            else
            {
                Debug.LogError(String.Format(SaveErrorMessage.LoadCreateCannotLoad, saveId));
            }
        }

        private void LoadSaveInternal(string saveId)
        {
            if(_saveIOService.TryLoad(saveId, out var save))
            {
                SaveData = save;
                _currentSaveId = saveId;
            }
            else
            {
                Debug.LogError(String.Format(SaveErrorMessage.GenericCannotLoadSave, saveId));
            }
        }
    }
}