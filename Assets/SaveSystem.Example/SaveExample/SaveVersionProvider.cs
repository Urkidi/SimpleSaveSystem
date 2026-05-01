using SimpleSaveSystem.Services;

namespace SaveSystem.Example.SaveExample
{
    public class SaveVersionProvider : ISaveVersionProvider
    {
        public string Version => "1";
    }
}