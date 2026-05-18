using SimpleSaveSystem.Core.Services;

namespace SimpleSaveSystem.Example
{
    public class DefaultKeyProvider : IEncryptionKeyProvider
    {
        public string Key => "ExampleKeyThisHasToBe32Charactrs";
    }
}