using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Win_CBZ
{
    internal class LocalFile
    {

        public String FileName { get; set; }

        public String FilePath { get; set; }

        public String FileExtension { get; set; }

        public String FullPath { get; set; }

        public long FileSize { get; set; }

        public DateTimeOffset LastModified { get; set; }

        public FileInfo LocalFileInfo { get; set; }

        
        public LocalFile(String fileName)
        {
            FullPath = fileName;
            LocalFileInfo = new FileInfo(fileName);
            FileName = LocalFileInfo.Name;
            FilePath = LocalFileInfo.Directory.FullName;
            FileSize = LocalFileInfo.Length;
            LastModified = LocalFileInfo.LastWriteTime;
            FileExtension = LocalFileInfo.Extension;

        }

        public bool Exists()
        {
            LocalFileInfo.Refresh();
            

            return LocalFileInfo.Exists;
        }
    }
}
