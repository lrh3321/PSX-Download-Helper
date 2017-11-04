using System;
using System.IO;

namespace PSXDH.Model
{
    public class LocalFile : IDisposable
    {
        public LocalFile(string filepath)
        {
            if (File.Exists(filepath))
            {
                Filepath = filepath;
                FileStream = File.OpenRead(filepath);
                LastModified = File.GetLastWriteTime(filepath);
                Filesize = FileStream.Length;
            }
        }

        public string Filepath { get; private set; }

        public long Filesize { get; private set; }

        public FileStream FileStream { get; private set; }

        public DateTime LastModified { get; private set; }

        public void Dispose()
        {
            this.FileStream?.Dispose();
        }
    }
}