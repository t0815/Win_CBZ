using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Win_CBZ
{
    public class LocalFile
    {

        public String FileName { get; set; }

        public String FilePath { get; set; }

        public String FileExtension { get; set; }

        public String FullPath { get; set; }

        public long FileSize { get; set; }

        public DateTimeOffset LastModified { get; set; }

        
        public LocalFile(String fileName)
        {
            FullPath = fileName;
            FileInfo localFileInfo = new FileInfo(fileName);
            FileName = localFileInfo.Name;
            FilePath = localFileInfo.Directory.FullName;
            FileSize = localFileInfo.Length;
            LastModified = localFileInfo.LastWriteTime;
            FileExtension = localFileInfo.Extension;

        }

        public bool Exists()
        {
            FileInfo localFileInfo = new FileInfo(FullPath);

            return localFileInfo.Exists;
        }
    }
}
