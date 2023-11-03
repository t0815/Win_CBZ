using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Win_CBZ
{
    internal class LocalFile
    {

        protected Dictionary<string, ImageFormat> ImageFormatMap = new Dictionary<string, ImageFormat>()
        {
            { "jpg", ImageFormat.Jpeg },
            { "png", ImageFormat.Png },
            { "bmp", ImageFormat.Bmp  },
            { "tif", ImageFormat.Tiff },
        };

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
            try
            {
                FileSize = LocalFileInfo.Length;
            } catch (Exception)
            {
                FileSize = 0;
            }
            LastModified = LocalFileInfo.LastWriteTime;
            FileExtension = LocalFileInfo.Extension;

        }

        public ImageFormat GuessImageFormat()
        {
            String ext = FileExtension.ToLower();
            ImageFormat result = null;

            ImageFormatMap.TryGetValue(ext.ToLower().TrimStart('.'), out result);
            
            return result;
        }

        public bool Exists()
        {
            LocalFileInfo.Refresh();
            
            return LocalFileInfo.Exists;
        }
    }
}
