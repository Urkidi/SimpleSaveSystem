using System;
using System.Collections.Generic;
using SimpleSaveSystem.Core;
using SimpleSaveSystem.Defaults;
using SimpleSaveSystem.Example.SaveExample;
using TMPro;
using UnityEngine;

namespace SimpleSaveSystem.Example
{
    public class ExampleSaver : MonoBehaviour
    {
        private SaveSystem<MainSavegame> _saveSystem;
        private MainSavegame _mainSave;

        [SerializeField]
        private TextMeshProUGUI _dataText;

        private SaveDefaultProvider _defaultSaveProvider;

        private void Awake()
        {
            _mainSave = new MainSavegame()
            {
                Name = "ExampleSave",
                Items = new List<ItemSave>()
                {
                    new ItemSave()
                    {
                        Amount = 0,
                        ItemDate = DateTime.Now.AddDays(-1),
                        ItemName = "ExampleItem0"
                    },
                    new ItemSave()
                    {
                        Amount = 10,
                        ItemDate = DateTime.Now,
                        ItemName = "ExampleItem1"
                    }
                },
                ItemContainer = new ItemContainerSave()
                {
                    Items = new List<ItemSave>()
                    {
                        new ItemSave()
                        {
                            Amount = 11,
                            ItemDate = DateTime.Now.AddDays(1),
                            ItemName = "ExampleItem2"
                        }
                    }
                }
            };
            var dataRW = new LocalDataReadWriter();
            var serializationService = new JsonSerializationService();
            var encryptionService = new AesEncryptionService(new DefaultKeyProvider());
            var hashService = new Sha256HashService();
            var uriProvider = new DefaultPathProvider();
            var versionProvider = new SaveVersionProvider();
            _defaultSaveProvider = new SaveDefaultProvider();
            
            var saveService = new SaveIOService<MainSavegame>(dataRW, dataRW,serializationService,encryptionService,hashService, uriProvider, versionProvider, _defaultSaveProvider);
            _saveSystem = new SaveSystem<MainSavegame>(saveService);
        }

        public void Save()
        {
            _saveSystem.SaveCurrentSave();
        }

        public void Load()
        {
           _saveSystem.LoadSave(_saveSystem.SaveGameIds[0]);
            PrintSaveData();
        }

        public void AddData()
        {
            _saveSystem.SaveData.Name = _mainSave.Name;
            _saveSystem.SaveData.Items = _mainSave.Items;
            _saveSystem.SaveData.ItemContainer = _mainSave.ItemContainer;
            PrintSaveData();
        }

        public void ResetData()
        {
            var defaultSave = _defaultSaveProvider.CreateSave();
            _saveSystem.SaveData.Name = defaultSave.Name;
            _saveSystem.SaveData.Items = defaultSave.Items;
            _saveSystem.SaveData.ItemContainer = defaultSave.ItemContainer;
            PrintSaveData();
        }

        public void PrintSaveData()
        {
            _dataText.text = JsonUtility.ToJson(_saveSystem.SaveData);
        }
    }
}