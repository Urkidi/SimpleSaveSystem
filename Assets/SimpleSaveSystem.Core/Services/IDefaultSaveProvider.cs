namespace SimpleSaveSystem.Core.Services
{
    public interface IDefaultSaveProvider<T>
    {
        T CreateSave();
    }
}