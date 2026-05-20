using System;
using System.Collections.Generic;
using System.Linq;
using SimpleSaveSystem.Core.Data;
using SimpleSaveSystem.Core.Services;
using UnityEngine;

namespace SimpleSaveSystem.Core
{
    public class SaveIOService<T> : ISaveIOService<T>
    {
        private readonly IDataReadService _dataReadService;
        private readonly IDataWriteService _dataWriteService;
        private readonly ISerializationService _serializationService;
        private readonly IEncryptionService _encryptionService;
        private readonly IHashService _hashService;
        private readonly IUriProvider _uriProvider;
        private readonly ISaveVersionProvider _saveVersionProvider;
        private readonly IDefaultSaveProvider<T> _defaultSaveProvider;

        private SaveIndexData _indexData;

        public List<string> SaveSlotIds { get; private set; }
        public string LastSavedId => _indexData.LastSavedSlotId;

        public SaveIOService(IDataReadService dataReadService,
            IDataWriteService dataWriteService,
            ISerializationService serializationService,
            IEncryptionService encryptionService,
            IHashService hashService,
            IUriProvider uriProvider,
            ISaveVersionProvider saveVersionProvider,
            IDefaultSaveProvider<T> defaultSaveProvider)
        {
            _dataReadService = dataReadService;
            _dataWriteService = dataWriteService;
            _serializationService = serializationService;
            _encryptionService = encryptionService;
            _hashService = hashService;
            _uriProvider = uriProvider;
            _saveVersionProvider = saveVersionProvider;
            _defaultSaveProvider = defaultSaveProvider;

            LoadCreateIndexData();
        }

        private void LoadCreateIndexData()
        {
            if (_dataReadService.TryRead(_uriProvider.MetaDataUri, out var metaBytes))
            {
                _indexData = _serializationService.Deserialize<SaveIndexData>(metaBytes);
            }
            else
            {
                _indexData = new SaveIndexData
                {
                    SaveSlots = new List<SaveSlotMetaData>()
                };
            }

            SaveSlotIds = _indexData.SaveSlots.Select(slot => slot.Id).ToList();
        }

        public bool TryLoadCreate(string saveId, out T data)
        {
            if (SaveSlotIds.Contains(saveId))
            {
                return TryLoad(saveId, out data);
            }

            data = _defaultSaveProvider.CreateSave();
            AddSaveIndex(saveId);
            return true;
        }

        public bool TryLoad(string saveId, out T save)
        {
            if (_dataReadService.TryRead(_uriProvider.GetSlotUri(saveId), out var saveBytes))
            {
                if (_hashService.VerifyHash(saveBytes, _indexData.SaveSlots.First(slot => slot.Id == saveId).Hash))
                {
                    var decryptedSave = _encryptionService.DecryptData(saveBytes);
                    save = _serializationService.Deserialize<T>(decryptedSave);
                    return true;
                }

                Debug.LogError(SaveErrorMessage.HashDoesntMatch);
            }
            else
            {
                Debug.LogError(String.Format(SaveErrorMessage.UnableToReadPath, _uriProvider.GetSlotUri(saveId)));
            }

            save = default;
            return false;
        }

        public bool TrySave(string saveId, T save)
        {
            if (SaveSlotIds.Contains(saveId))
            {
                var saveBytes = _serializationService.Serialize(save);
                var encryptedSave = _encryptionService.EncryptData(saveBytes);

                _dataWriteService.WriteData(_uriProvider.GetSlotUri(saveId), encryptedSave);

                var slot = _indexData.SaveSlots.First(slot => slot.Id == saveId);
                slot.Hash = _hashService.GetHash(encryptedSave);
                slot.Version = _saveVersionProvider.Version;
                slot.LastModified = DateTime.UtcNow;

                _indexData.LastSavedSlotId = saveId;
                WriteIndexData();
                return true;
            }

            Debug.LogError(String.Format(SaveErrorMessage.UnableToFindSaveWithId, saveId));
            return false;
        }

        public DateTime GetLastModifiedSaveDate(string saveId)
        {
            var lastModified = DateTime.MinValue;
            
            if (SaveSlotIds.Contains(saveId))
                lastModified = _indexData.SaveSlots.First(slot => slot.Id == saveId).LastModified;
            
            return lastModified;
        }

        public string GetSaveVersion(string saveId)
        {
            var saveVersion = string.Empty;
            
            if (SaveSlotIds.Contains(saveId))
                saveVersion = _indexData.SaveSlots.First(slot => slot.Id == saveId).Version;
            
            return saveVersion;;
        }

        private void AddSaveIndex(string saveId)
        {
            _indexData.SaveSlots.Add(new SaveSlotMetaData()
            {
                Id = saveId,
                Version = _saveVersionProvider.Version
            });
            SaveSlotIds = _indexData.SaveSlots.Select(slot => slot.Id).ToList();
        }


        private void WriteIndexData()
        {
            var indexDataBytes = _serializationService.Serialize(_indexData);
            _dataWriteService.WriteData(_uriProvider.MetaDataUri, indexDataBytes);
        }
    }
}