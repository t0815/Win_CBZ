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

        public string SizeFormatted { get; set; }

        public DateTimeOffset LastModified { get; set; }

        public FileInfo LocalFileInfo { get; set; }

        
        public LocalFile(String fileName)
        {
            FullPath = fileName;
            LocalFileInfo = new FileInfo(fileName);
            FileName = LocalFileInfo.Name;
            Name = Path.GetFileNameWithoutExtension(fileName);
            FilePath = LocalFileInfo.Directory.FullName + Path.DirectorySeparatorChar;
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

            SizeFormatted = FormatSize(FileSize);
            
            FileExtension = LocalFileInfo.Extension;

            try
            {
                LastModified = LocalFileInfo.LastWriteTime;
            }
            catch (IOException)
            {

            }
        }

        public bool Exists()
        {
            Refresh();
            
            return LocalFileInfo.Exists;
        }

        public string FormatSize(long length = -1)
        {
            double size = length > -1 ? length : FileSize;
            string[] units = new string[] { "Bytes", "KB", "MB", "GB" };
            string selectedUnit = "Bytes";

            foreach (string unit in units)
            {
                if (size > 999)
                    size /= 1024;
                else
                {
                    selectedUnit = unit;
                    break;
                }
            }

            return size.ToString("n2") + " " + selectedUnit;
        }

        public void Refresh()
        {
            LocalFileInfo?.Refresh();

            try
            {
                FullPath = LocalFileInfo?.FullName;
                Name = LocalFileInfo?.Name;

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

            SizeFormatted = FormatSize(FileSize);
            LastModified = LocalFileInfo.LastWriteTime;
        }


        public void Dispose()
        {
            LocalFileInfo = null;
        }
    }
}
