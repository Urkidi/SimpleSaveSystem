using SimpleSaveSystem.Core.Services;

namespace SimpleSaveSystem.Example
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