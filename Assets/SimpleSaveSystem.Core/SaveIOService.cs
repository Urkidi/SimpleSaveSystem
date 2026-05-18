using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public bool TryLoadCreate(string id, out T data)
        {
            if (SaveSlotIds.Contains(id))
            {
                return TryLoad(id, out data);
            }

            data = _defaultSaveProvider.CreateSave();
            AddSaveIndex(id);
            return true;
        }
        public bool TryLoad(string id, out T save)
        {
            if (_dataReadService.TryRead(_uriProvider.GetSlotUri(id), out var saveBytes))
            {
                if (_hashService.VerifyHash(saveBytes,
                        Encoding.UTF8.GetBytes(_indexData.SaveSlots.First(slot => slot.Id == id).Hash)))
                {
                    var decryptedSave = _encryptionService.DecryptData(saveBytes);
                    save = _serializationService.Deserialize<T>(decryptedSave);
                    return true;
                }

                Debug.LogError(SaveErrorMessage.HashDoesntMatch);
            }
            else
            {
                Debug.LogError(String.Format(SaveErrorMessage.UnableToReadPath, _uriProvider.GetSlotUri(id)));
            }

            save = default;
            return false;
        }

        public bool TrySave(string id, T saveData)
        {
            if (SaveSlotIds.Contains(id))
            {
                var saveBytes = _serializationService.Serialize(saveData);
                var encryptedSave = _encryptionService.EncryptData(saveBytes);

                _dataWriteService.WriteData(_uriProvider.GetSlotUri(id), encryptedSave);

                var slot = _indexData.SaveSlots.First(slot => slot.Id == id);
                slot.Hash = Encoding.UTF8.GetString(_hashService.HashData(encryptedSave));
                slot.Version = _saveVersionProvider.Version;
                slot.LastModified = DateTime.UtcNow;

                WriteIndexData();
                return true;
            }

            Debug.LogError(String.Format(SaveErrorMessage.UnableToFindSaveWithId, id));
            return false;
        }

        private void AddSaveIndex(string id)
        {
            _indexData.SaveSlots.Add(new SaveSlotMetaData()
            {
                Id = id,
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