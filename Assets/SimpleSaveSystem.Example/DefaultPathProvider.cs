using System.IO;
using SimpleSaveSystem.Core.Services;
using UnityEngine;

namespace SimpleSaveSystem.Example
{
    public class DefaultPathProvider : IUriProvider
    {
        private string _metaFileName = "metafile.json";
        private string _saveFolderName = "Saves";
        private string _slotFolderName = "Slots";
        private string _slotFileType = ".dat";
        public string MetaDataUri => Path.Combine(Application.persistentDataPath, _saveFolderName, _metaFileName);
        public string SaveDataUri => Path.Combine(Application.persistentDataPath, _saveFolderName, _slotFolderName);

        public string GetSlotUri(string saveId)
        {
            return Path.Combine(SaveDataUri, saveId + _slotFileType);
        }
    }
}