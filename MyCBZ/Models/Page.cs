using Win_CBZ;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Win_CBZ
{
    internal class Page
    {

        public String Id { get; set; }

        public String TemporaryFileId { get; set; }

        public String Filename { get; set; }

        public String Name { get; set; }

        public String OriginalName { get; set; }

        public String EntryName { get; set; } 

        public String ImageType { get; set; } = "Story";

        public int Number { get; set; }

        public long Size { get; set; }

        public String WorkingDir { get; set; }

        public String LocalPath { get; set; }

        public String TempPath { get; set; }    

        public bool Compressed { get; set; }

        public bool Changed { get; set; }

        public bool Deleted { get; set; }

        public bool ReadOnly { get; set; }

        public bool Selected { get; set; }

        public bool Invalidated { get; set; }

        public bool ThumbnailInvalidated { get; set; }

        public int W { get; set; }

        public int H { get; set; }

        public int Index { get; set; }

        public String Key { get; set; }

        public DateTimeOffset LastModified { get; set; }

        protected int ThumbW { get; set; } = 128;

        protected int ThumbH { get; set; } = 256;

        private Image Image;

        private Image Thumbnail;

        private Stream ImageStream;

        private FileInfo ImageFileInfo;

        private ZipArchiveEntry ImageEntry;

        public delegate EventHandler<FileOperationEvent> FileOperationEventHandler();
        
        public Page(String fileName, FileAccess mode = FileAccess.Read)
        {
            ImageFileInfo = new FileInfo(fileName);
            ReadOnly = mode == FileAccess.Read || ImageFileInfo.IsReadOnly;
            if ((mode == FileAccess.Write || mode == FileAccess.ReadWrite) && ImageFileInfo.IsReadOnly)
            {
                RemoveReadOnlyAttribute(ref ImageFileInfo);
            }
            FileStream ImageStream = ImageFileInfo.Open(FileMode.Open, mode, FileShare.ReadWrite);
            Filename = ImageFileInfo.Name;       
            LocalPath = ImageFileInfo.Directory.FullName;               
            Name = ImageFileInfo.Name;
            LastModified = ImageFileInfo.LastWriteTime;
            Size = ImageFileInfo.Length;
            Id = Guid.NewGuid().ToString();
        }
        

        public Page(FileInfo ImageFileInfo, FileAccess mode = FileAccess.Read)
        {
            this.ImageFileInfo = ImageFileInfo;
            ReadOnly = mode == FileAccess.Read || ImageFileInfo.IsReadOnly;
            try
            {
                if ((mode == FileAccess.Write || mode == FileAccess.ReadWrite) && ImageFileInfo.IsReadOnly)
                {
                    RemoveReadOnlyAttribute(ref ImageFileInfo);
                }
                ImageStream = ImageFileInfo.Open(FileMode.Open, mode, FileShare.ReadWrite);
                ReadOnly = ImageStream.CanWrite;
            } catch (UnauthorizedAccessException)
            {
                ImageStream = ImageFileInfo.Open(FileMode.Open, FileAccess.Read, FileShare.Read);
                ReadOnly = true;
            }

            Filename = ImageFileInfo.FullName;
            LocalPath = ImageFileInfo.Directory.FullName;
            Name = ImageFileInfo.Name;
            Size = ImageFileInfo.Length;
            Id = Guid.NewGuid().ToString();
        }


        public Page(ZipArchiveEntry entry, String workingDir, String randomId)
        {
            ImageEntry = entry;
            Compressed = true;

            Filename = entry.FullName;
            Name = entry.Name;
            Size = entry.Length;
            LastModified = entry.LastWriteTime;
            Id = Guid.NewGuid().ToString();
            TemporaryFileId = randomId;
            WorkingDir = workingDir;
        }


        public Page(Stream fileInputStream, String name)
        {
            ImageStream = fileInputStream;
            Name = name;
            EntryName = name;
            Id = Guid.NewGuid().ToString();
        }

        public Page(GZipStream zipInputStream, String name)
        {
            ImageStream = zipInputStream;
            Name = name;
            EntryName = name;
            Compressed = true;
            Id = Guid.NewGuid().ToString();
        }


        protected bool RemoveReadOnlyAttribute(ref FileInfo ImageFileInfo)
        {
            FileAttributes fileAttributes = ImageFileInfo.Attributes & ~FileAttributes.ReadOnly;
            File.SetAttributes(ImageFileInfo.FullName, fileAttributes);
            ImageFileInfo.Attributes = fileAttributes;

            return !ImageFileInfo.IsReadOnly;
        }

        protected void RequestTemporaryFile()
        {
            if (Compressed)
            {
                if (ImageEntry != null)
                {
                    if (TempPath == null)
                    {
                        ImageEntry.ExtractToFile(WorkingDir + TemporaryFileId);

                        TempPath = WorkingDir + TemporaryFileId;
                    }
                }
                else
                {
                    MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_WARNING, "No Entry with name [" + Name + "] exists in archive!");
                }
            } else
            {
                if (ReadOnly)
                {
                    FileInfo tempFileInfo = new FileInfo(TempPath);
                    FileStream ImageStream = File.Open(TempPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                    FileStream localFile = File.OpenRead(LocalPath);

                    localFile.CopyTo(ImageStream);

                    ImageStream.Close();
                    localFile.Close();
                }
            }
        }

        public void DeleteTemporaryFile()
        {
            if (TempPath != null)
            {
                if (Compressed)
                {
                    
                }

                ImageStream.Close();
                File.Delete(TempPath);
            }
        }

        public void DeleteLocalFile()
        {
            if (!ReadOnly)
            {
                File.Delete(LocalPath);
            }           
        }

        public Image GetImage()
        {
            this.LoadImage();

            return Image;
        }

        public Image GetThumbnail(Image.GetThumbnailImageAbort callback, IntPtr data)
        {
            this.LoadImage();

            Thumbnail = Image.GetThumbnailImage(ThumbW, ThumbH, callback, data);

            Image.Dispose();
            Image = null;

            return Thumbnail;
        }


        public String CreateLocalWorkingCopy(String destination)
        {
            if (Compressed)
            {
                RequestTemporaryFile();

                if (TempPath != null)
                {
                    return TempPath;
                }
            }
            else
            {
                FileInfo copyFileInfo = new FileInfo(destination);
                if (ImageStream != null)
                {
                    if (ImageStream.CanRead)
                    {
                        FileStream localCopyStream = copyFileInfo.Open(FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read);

                        ImageStream.CopyTo(localCopyStream);
                        localCopyStream.Close();                     
                    }
                } else
                {
                    if (LocalPath != null)
                    {
                        FileStream localCopyStream = copyFileInfo.Open(FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read);
                        FileStream destinationStream = File.Create(destination);

                        localCopyStream.CopyTo(destinationStream);
                    }
                }

                if (copyFileInfo.Exists)
                {
                    TempPath = destination;

                    return destination;
                }
            }

            return "";
        }


        private void LoadImage()
        {
            if (Image == null && ImageStream != null && ImageStream.CanRead)
            {
                Image = Image.FromStream(ImageStream);
            }

            if (Image == null)
            {
                RequestTemporaryFile();
                ImageStream = File.Open(TempPath, FileMode.Open, FileAccess.ReadWrite);
                Image = Image.FromStream(ImageStream);  
            }

            if (Image != null)
            {
                if (!Image.Size.IsEmpty)
                {
                    W = Image.Width;
                    H = Image.Height;
                }

                ImageStream.Close();
            } else
            {
                throw new Exception("Failed to load/extract image!");
            }
        }


        public void FreeImage()
        {
            if (Image != null)
            {
                Image.Dispose();
            }

            if (ImageStream != null)
            {
                ImageStream.Close();
                ImageStream.Dispose();
            }
        }
    }
}
