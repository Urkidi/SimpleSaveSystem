using System.Linq;
using System.Security.Cryptography;

namespace SimpleSaveSystem.Services
{
    public class Sha256HashService : IHashService
    {
        public byte[] HashData(byte[] data)
        {
            using HashAlgorithm hashAlgorithm = SHA256.Create();
            return hashAlgorithm.ComputeHash(data);
        }

        public bool VerifyHash(byte[] data, byte[] hash)
        {
            using HashAlgorithm hashAlgorithm = SHA256.Create();
            var target = hashAlgorithm.ComputeHash(data);
            return target.SequenceEqual(hash);
        }
    }
}