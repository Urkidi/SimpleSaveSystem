using System;
using System.Security.Cryptography;
using System.Text;
using SimpleSaveSystem.Core.Services;

namespace SimpleSaveSystem.Defaults
{
    public class Sha256HashService : IHashService
    {
        public bool VerifyHash(byte[] data, string hash)
        {
            var dataHash = GetHash(data);
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;
            return comparer.Equals(dataHash, hash);
        }

        public string GetHash(byte[] data)
        {
            using HashAlgorithm hashAlgorithm = SHA256.Create();
            var byteHash = hashAlgorithm.ComputeHash(data);
            var stringBuilder = new StringBuilder();

            for (int i = 0; i < byteHash.Length; i++)
            {
                stringBuilder.Append(byteHash[i].ToString("x2"));
            }

            return stringBuilder.ToString();
        }
    }
}