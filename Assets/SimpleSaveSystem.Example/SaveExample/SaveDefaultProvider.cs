using SimpleSaveSystem.Core.Services;

namespace SimpleSaveSystem.Example.SaveExample
{
    public class SaveDefaultProvider : IDefaultSaveProvider<MainSavegame>
    {
        public MainSavegame CreateSave()
        {
            return new MainSavegame();
        }
    }
}