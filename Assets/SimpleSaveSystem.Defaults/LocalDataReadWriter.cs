using System.IO;
using SimpleSaveSystem.Core.Services;

namespace SimpleSaveSystem.Defaults
{
    public class LocalDataReadWriter : IDataReadService, IDataWriteService
    {
        private byte[] Read(string uri)
        {
            return File.ReadAllBytes(uri);
        }

        public void WriteData(string uri, byte[] data)
        {
            var path = Path.GetDirectoryName(uri);
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            
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