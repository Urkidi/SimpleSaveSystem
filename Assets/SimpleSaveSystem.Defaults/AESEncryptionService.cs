using SimpleSaveSystem.Core.Services;

namespace SimpleSaveSystem.Defaults
{
    public class AesEncryptionService : IEncryptionService
    {
        private readonly IEncryptionKeyProvider _keyProvider;

        public AesEncryptionService(IEncryptionKeyProvider keyProvider)
        {
            _keyProvider = keyProvider;
        }

        public byte[] EncryptData(byte[] rawData)
        {
            return rawData;
        }

        public byte[] DecryptData(byte[] data)
        {
            return data;
        }
    }
}