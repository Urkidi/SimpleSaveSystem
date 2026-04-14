namespace SaveSystem
{
    public interface IDataReader<out T>
    {
        T ReadData();
    }
}