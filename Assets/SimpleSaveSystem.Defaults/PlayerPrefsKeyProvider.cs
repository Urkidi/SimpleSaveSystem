using SimpleSaveSystem.Core.Services;
using UnityEngine;

namespace SimpleSaveSystem.Defaults
{
    public class PlayerPrefsKeyProvider : IEncryptionKeyProvider
    {
        public string Key { get; }
        
        public PlayerPrefsKeyProvider(string playerPrefsKey)
        {
            Key = PlayerPrefs.GetString(playerPrefsKey);
        }
    }
}