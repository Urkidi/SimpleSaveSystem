namespace SaveSystem
{
    public class SaveSystem<T> : ISaveSystem
    {
        private T _saveData;

        public SaveSystem(T data)
        {
            _saveData = data;
        }
        
        public void Save()
        {
            throw new System.NotImplementedException();
        }

        public void Load()
        {
            throw new System.NotImplementedException();
        }
    }
}