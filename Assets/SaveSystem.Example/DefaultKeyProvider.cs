using SimpleSaveSystem.Services;

namespace SaveSystem.Example
{
    public class DefaultKeyProvider : IEncryptionKeyProvider
    {
        public byte[] Key { get; }

        public DefaultKeyProvider()
        {
            Key = System.Text.Encoding.UTF8.GetBytes("ExampleKey");
        }
    }
}