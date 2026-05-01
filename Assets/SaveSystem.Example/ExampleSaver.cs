using System;
using System.Collections.Generic;
using SaveSystem.Example.SaveExample;
using SimpleSaveSystem;
using SimpleSaveSystem.Services;
using TMPro;
using UnityEngine;

namespace SaveSystem.Example
{
    public class ExampleSaver : MonoBehaviour
    {
        private SaveSystem<MainSavegame> _saveSystem;
        private MainSavegame _mainSave;

        [SerializeField]
        private TextMeshProUGUI _textMesh;
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
            var defaultSaveProvider = new SaveDefaultProvider();
            
            var saveService = new SaveIOService<MainSavegame>(dataRW, dataRW,serializationService,encryptionService,hashService, uriProvider, versionProvider, defaultSaveProvider);
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

        public void PopulateSaveData()
        {
            _saveSystem.SaveData.Name = _mainSave.Name;
            _saveSystem.SaveData.Items = _mainSave.Items;
            _saveSystem.SaveData.ItemContainer = _mainSave.ItemContainer;
        }

        public void PrintSaveData()
        {
            _textMesh.text = JsonUtility.ToJson(_saveSystem.SaveData);
        }
    }
}