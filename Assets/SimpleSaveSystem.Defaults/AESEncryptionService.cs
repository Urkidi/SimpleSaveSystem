using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using SimpleSaveSystem.Core.Services;

namespace SimpleSaveSystem.Defaults
{
    public class AesEncryptionService : IEncryptionService
    {
        private readonly IEncryptionKeyProvider _keyProvider;

        private byte[] _key;

        public AesEncryptionService(IEncryptionKeyProvider keyProvider)
        {
            _keyProvider = keyProvider;
        }

        public byte[] EncryptData(byte[] rawData)
        {
            SetUp();
            if (rawData == null)
                return null;

            using var aes = Aes.Create();
            aes.Key = _key;
            aes.GenerateIV();
            var iv = aes.IV;

            using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            using var ms = new MemoryStream();
            using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                cs.Write(rawData, 0, rawData.Length);
            
            var msBytes = ms.ToArray();
            var result = new byte[iv.Length + msBytes.Length];
            Array.Copy(iv, 0, result, 0, iv.Length);
            Array.Copy(msBytes, 0, result, iv.Length, msBytes.Length);
            
            return result;
        }

        private void SetUp()
        {
            _key = GetKeyOrIV(_keyProvider.Key, 32);
        }

        public byte[] DecryptData(byte[] cipherData)
        {
            SetUp();
            if (cipherData == null)
                return null;

            using var aes = Aes.Create();

            aes.Key = _key;

            // Extract IV
            byte[] iv = new byte[16];
            Array.Copy(cipherData, 0, iv, 0, iv.Length);

            aes.IV = iv;

            // Extract actual encrypted payload
            byte[] actualCipher = new byte[cipherData.Length - iv.Length];
            Array.Copy(cipherData, iv.Length, actualCipher, 0, actualCipher.Length);


            using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            using var ms = new MemoryStream(actualCipher);
            using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
            using var resultStream = new MemoryStream();
            cs.CopyTo(resultStream);
            return resultStream.ToArray();
        }

        private byte[] GetKeyOrIV(string data, int requiredLength)
        {
            if (string.IsNullOrWhiteSpace(data) || data.Length != requiredLength)
            {
                throw new InvalidOperationException(
                    $"Encryption key must be set and exactly {requiredLength} characters long.");
            }

            return Encoding.UTF8.GetBytes(data);
        }
    }
}