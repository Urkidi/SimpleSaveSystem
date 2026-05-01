using SimpleSaveSystem.Services;

namespace SaveSystem.Example.SaveExample
{
    public class SaveDefaultProvider : IDefaultSaveProvider<MainSavegame>
    {
        public MainSavegame CreateSave()
        {
            return new MainSavegame();
        }
    }
}