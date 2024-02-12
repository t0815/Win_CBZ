using System;
using System.IO;


namespace Win_CBZ
{
    internal class LocalFile
    {       

        public String FileName { get; set; }

        public String FilePath { get; set; }

        public String Name { get; set; }

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
            Name = Path.GetFileNameWithoutExtension(fileName);
            FilePath = LocalFileInfo.Directory.FullName;
            try
            {
                if (LocalFileInfo.Exists)
                {
                    FileSize = LocalFileInfo.Length;
                } else
                {
                    FileSize = 0;
                }
            } catch (Exception)
            {
                FileSize = 0;
            }
            LastModified = LocalFileInfo.LastWriteTime;
            FileExtension = LocalFileInfo.Extension;
        }

        public bool Exists()
        {
            Refresh();
            
            return LocalFileInfo.Exists;
        }

        public void Refresh()
        {
            LocalFileInfo.Refresh();

            try
            {
                if (LocalFileInfo.Exists)
                {
                    FileSize = LocalFileInfo.Length;
                } else
                {
                    FileSize = 0;
                }               
            }
            catch (Exception)
            {
                FileSize = 0;
            }

            LastModified = LocalFileInfo.LastWriteTime;
        }


        public void Dispose()
        {
            LocalFileInfo = null;
        }
    }
}
