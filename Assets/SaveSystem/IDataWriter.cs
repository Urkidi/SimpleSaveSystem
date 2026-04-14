namespace SaveSystem
{
    public interface IDataWriter<in T>
    {
        void WriteData(T data);
    }
}