namespace SimpleSaveSystem.Services
{
    public interface IDefaultSaveProvider<T>
    {
        T CreateSave();
    }
}