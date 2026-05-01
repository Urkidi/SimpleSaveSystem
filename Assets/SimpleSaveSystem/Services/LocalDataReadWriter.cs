using System.IO;

namespace SimpleSaveSystem.Services
{
    public class LocalDataReadWriter : IDataReadService, IDataWriteService
    {
        private byte[] Read(string uri)
        {
            return File.ReadAllBytes(uri);
        }

        public void WriteData(string uri, byte[] data)
        {
            File.WriteAllBytes(uri, data);
        }

        public bool TryRead(string uri, out byte[] data)
        {
            var exists = File.Exists(uri);

            data = exists ? Read(uri) : null;
            return exists;
        }
    }
}