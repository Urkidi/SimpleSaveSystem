using SimpleSaveSystem.Core.Services;

namespace SimpleSaveSystem.Example.SaveExample
{
    public class SaveVersionProvider : ISaveVersionProvider
    {
        public string Version => "1";
    }
}