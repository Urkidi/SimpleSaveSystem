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

        private static string Prefs_EncryptionKey = "Save_Key";

        [SerializeField]
        [Tooltip("This key must have exactly 32 Characters")]
        private string _encryptionKey;
        [SerializeField]
        private TextMeshProUGUI _dataText;
        [SerializeField]
        private TMP_InputField _saveNewInputText;
        [SerializeField]
        private TMP_Dropdown _loadDropdown;
        
        private SaveDefaultProvider _defaultSaveProvider;
        private int _selectedDropdownItem;

        private void Awake()
        {
            SaveKeyInPlayerPrefs();
            
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
            var encryptionService = new AesEncryptionService(new PlayerPrefsKeyProvider(Prefs_EncryptionKey)); 
            var hashService = new Sha256HashService();
            var uriProvider = new DefaultPathProvider();
            var versionProvider = new SaveVersionProvider();
            _defaultSaveProvider = new SaveDefaultProvider();
            
            var saveService = new SaveIOService<MainSavegame>(dataRW, dataRW,serializationService,encryptionService,hashService, uriProvider, versionProvider, _defaultSaveProvider);
            _saveSystem = new SaveSystem<MainSavegame>(saveService);

            Load();
            
            if(_loadDropdown != null)
            {
                _loadDropdown.ClearOptions();
                _loadDropdown.AddOptions(_saveSystem.SaveGameIds);
                _loadDropdown.onValueChanged.AddListener(UpdateSelected);
            }
        }

        private void SaveKeyInPlayerPrefs()
        {
            PlayerPrefs.SetString(Prefs_EncryptionKey, _encryptionKey);
        }

        private void UpdateSelected(int arg0)
        {
            _selectedDropdownItem = arg0;
        }

        public void Save()
        {
            _saveSystem.SaveCurrentSave();
        }
        
        public void CreateNewSave()
        {
            if(_saveNewInputText.text != string.Empty)
                _saveSystem.CreateNewSave(_saveNewInputText.text);
            else
                _saveSystem.CreateNewSave();
            
            _loadDropdown.ClearOptions();
            _loadDropdown.AddOptions(_saveSystem.SaveGameIds);
            _loadDropdown.value = _saveSystem.SaveGameIds.Count - 1;
        }

        public void Load()
        {
           _saveSystem.LoadSave(_saveSystem.SaveGameIds[_selectedDropdownItem]);
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