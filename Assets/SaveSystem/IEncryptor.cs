namespace SaveSystem
{
    public interface IEncryptor<T>
    {
        string EncryptData(T data);
        T DecryptData(string data);
    }
}