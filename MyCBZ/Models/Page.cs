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
using Win_CBZ.Models;
using System.Xml.Linq;
using System.Runtime.CompilerServices;
using System.Diagnostics.Eventing.Reader;
using System.Windows.Media.Imaging;

namespace Win_CBZ
{
    public class Page
    {

        public String Id { get; set; }

        public String TemporaryFileId { get; set; }

        public String Filename { get; set; }

        public String Name { get; set; }

        public String OriginalName { get; set; }

        public String EntryName { get; set; } 

        public String FileExtension { get; set; }

        public String ImageType { get; set; } = "Story";

        public ImageTask ImageTask { get; set; }

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

        public bool IsMemoryCopy { get; set; }

        public bool ImageInfoRequested { get; set; }

        public bool Closed { get; set; }

        public bool ThumbnailInvalidated { get; set; }

        public int W { get; set; }

        public int H { get; set; }

        public int Index { get; set; }

        public int OriginalIndex { get; set; }

        public String Key { get; set; }

        public DateTimeOffset LastModified { get; set; }

        protected int ThumbW { get; set; } = 212;

        protected int ThumbH { get; set; } = 256;

        private Image Image;

        private Image ImageInfo;

        private Image Thumbnail;

        private Stream ImageStream;

        private MemoryStream ImageStreamMemoryCopy;

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
            FileExtension = ImageFileInfo.Extension;
            LocalPath = ImageFileInfo.Directory.FullName;               
            Name = ImageFileInfo.Name;
            LastModified = ImageFileInfo.LastWriteTime;
            Size = ImageFileInfo.Length;
            Id = Guid.NewGuid().ToString();
            ImageTask = new ImageTask();
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
            } finally
            {

            }

            Filename = ImageFileInfo.FullName;
            FileExtension = ImageFileInfo.Extension;
            LocalPath = ImageFileInfo.Directory.FullName;
            Name = ImageFileInfo.Name;
            Size = ImageFileInfo.Length;
            Id = Guid.NewGuid().ToString();
            ImageTask = new ImageTask();
        }


        public Page(ZipArchiveEntry entry, String workingDir, String randomId)
        {
            ImageEntry = entry;
            Compressed = true;

            Filename = entry.FullName;
            FileExtension = ExtractFileExtension(entry.Name);
            Name = entry.Name;
            Size = entry.Length;
            LastModified = entry.LastWriteTime;
            Id = Guid.NewGuid().ToString();
            TemporaryFileId = randomId;
            WorkingDir = workingDir;
            ImageTask = new ImageTask();
        }


        public Page(Stream fileInputStream, String name)
        {
            string[] entryExtensionParts = name.Split('.');

            ImageStream = fileInputStream;
            Name = name;
            EntryName = name;
            FileExtension = ExtractFileExtension(name);
            Size = fileInputStream.Length;
            Id = Guid.NewGuid().ToString();
            ImageTask = new ImageTask();
        }

        public Page(GZipStream zipInputStream, String name)
        {
            ImageStream = zipInputStream;
            Name = name;
            EntryName = name;
            Compressed = true;
            Size = zipInputStream.Length;
            Id = Guid.NewGuid().ToString();
            ImageTask = new ImageTask();
        }

        public Page(Page sourcePage, String RandomId, int ThumbWidth = 212, int ThumbHeight = 256)
        {
            WorkingDir = sourcePage.WorkingDir;
            Name = sourcePage.Name;
            EntryName = sourcePage.EntryName;
            //TempPath = sourcePage.TempPath;
            Filename = sourcePage.Filename;
            LocalPath = sourcePage.LocalPath;
            ImageStream = sourcePage.ImageStream;
            Compressed = sourcePage.Compressed;
            TemporaryFileId = RandomId;
            EntryName = sourcePage.EntryName;
            ImageEntry = sourcePage.ImageEntry;

            if (ImageStream != null)
            {
                if (ImageStream.CanRead)
                {
                    sourcePage.ImageStream.Position = 0;

                    ImageStreamMemoryCopy = new MemoryStream();
                    sourcePage.ImageStream.CopyTo(ImageStreamMemoryCopy);
                    IsMemoryCopy = true;
                }
            }
                          
            Changed = sourcePage.Changed;
            ReadOnly = sourcePage.ReadOnly;
            Size = sourcePage.Size;
            Id = sourcePage.Id;
            Index = sourcePage.Index;
            OriginalIndex = sourcePage.OriginalIndex;
            Number = sourcePage.Number;
            Closed = sourcePage.Closed;
                      
            Deleted = sourcePage.Deleted;
            OriginalName = sourcePage.OriginalName;
            W = sourcePage.W;
            H = sourcePage.H;
            Key = sourcePage.Key;
            ThumbH = ThumbHeight;
            ThumbW = ThumbWidth;
            Thumbnail = sourcePage.Thumbnail;
            ThumbnailInvalidated = sourcePage.ThumbnailInvalidated;
            
            ImageTask = new ImageTask();
        }

        public Page(Page sourcePage)
        {
            WorkingDir = sourcePage.WorkingDir;
            Name = sourcePage.Name;
            EntryName = sourcePage.EntryName;
            TempPath = sourcePage.TempPath;
            Filename = sourcePage.Filename;
            LocalPath = sourcePage.LocalPath;
            ImageStream = sourcePage.ImageStream;
            Compressed = sourcePage.Compressed;
            TemporaryFileId = sourcePage.TemporaryFileId;
            EntryName = sourcePage.EntryName;
            ImageEntry = sourcePage.ImageEntry;
            ImageStream = sourcePage.ImageStream;
            IsMemoryCopy = sourcePage.IsMemoryCopy;
            ImageStreamMemoryCopy = sourcePage.ImageStreamMemoryCopy;

            Changed = sourcePage.Changed;
            ReadOnly = sourcePage.ReadOnly;
            Size = sourcePage.Size;
            Id = sourcePage.Id;
            Index = sourcePage.Index;
            OriginalIndex = sourcePage.OriginalIndex;
            Number = sourcePage.Number;
            Closed = sourcePage.Closed;

            Deleted = sourcePage.Deleted;
            OriginalName = sourcePage.OriginalName;
            W = sourcePage.W;
            H = sourcePage.H;
            Key = sourcePage.Key;
            ThumbH = sourcePage.ThumbH;
            ThumbW = sourcePage.ThumbW;
            Thumbnail = sourcePage.Thumbnail;
            ThumbnailInvalidated = sourcePage.ThumbnailInvalidated;

            ImageTask = sourcePage.ImageTask;
        }

        public void UpdatePage(Page page, bool skipIndex = false, bool skipName = false)
        {
            Compressed = page.Compressed;
            Filename = page.Filename;
            EntryName = page.EntryName;
            Size = page.Size;
            Id = page.Id;
            W = page.W; 
            H = page.H;
            Key = page.Key;
            Number = page.Number;
            Deleted = page.Deleted;

            if (!skipName)
            {
                Name = page.Name;
            }

            if (!skipIndex)
            {
                Index = page.Index;
            }
            //OriginalIndex = page.OriginalIndex;
            TemporaryFileId = page.TemporaryFileId;
            Changed = page.Changed;
        }

        public void UpdateImageEntry(ZipArchiveEntry entry, String randomId)
        {
            ImageEntry = entry;
            Compressed = true;

            Filename = entry.FullName;
            Name = entry.Name;
            //Size = entry.Length;
            LastModified = entry.LastWriteTime;
            Id = Guid.NewGuid().ToString();
            TemporaryFileId = randomId;
        }

        protected bool RemoveReadOnlyAttribute(ref FileInfo ImageFileInfo)
        {
            FileAttributes fileAttributes = ImageFileInfo.Attributes & ~FileAttributes.ReadOnly;
            File.SetAttributes(ImageFileInfo.FullName, fileAttributes);
            ImageFileInfo.Attributes = fileAttributes;

            return !ImageFileInfo.IsReadOnly;
        }

        public string ExtractFileExtension(String fileName)
        {
            string[] entryExtensionParts = fileName.Split('.');

            if (entryExtensionParts.Length == 0) return null;

            else return entryExtensionParts.Last<string>();
        }

        public string SizeFormat()
        {
            double size = Size;
            string[] units = new string[] { "Bytes", "KB", "MB", "GB" };
            string selectedUnit = "Bytes";

            foreach (string unit in units)
            {
                if (size > 1024)
                    size /= 1024;
                else
                {
                    selectedUnit = unit;
                    break;
                }
            }

            return size.ToString("n2") + " " + selectedUnit;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Close(bool dontDeleteTemporaryFiles = false)
        {
            FreeImage();

            if (dontDeleteTemporaryFiles)
            {
                DeleteTemporaryFile();
            }

            Closed = true;
            Invalidated = false;
            ImageInfoRequested = false;
            ImageFileInfo = null;           
        }

        protected void RequestTemporaryFile(String destination = null)
        {
            if (TemporaryFileId == null)
            {
                MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_WARNING, "Cant create new temporary file for page [" + Name + "]! No FileID available/set.");

                return;
            }

            if (Compressed)
            {
                if (ImageEntry != null)
                {
                    if (TempPath == null || destination != null)
                    {
                        if (destination == null)
                        {
                            destination = WorkingDir + TemporaryFileId + ".tmp";
                        }
                       
                        ImageEntry.ExtractToFile(destination);
                        
                        TempPath = destination;
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
                    if (TempPath == null || destination != null)
                    {
                        if (destination == null)
                        {
                            destination = WorkingDir + TemporaryFileId + ".tmp";
                        }

                        TempPath = destination;
                    }

                    FileInfo tempFileInfo = new FileInfo(TempPath);

                    FileStream localFile = File.OpenRead(LocalPath);
                    try
                    {
                        FileStream ImageStream = File.Open(TempPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                        try
                        {
                            localFile.CopyTo(ImageStream);
                        } catch (Exception ewr)
                        {
                            throw ewr;
                        } finally 
                        { 
                            ImageStream.Close(); 
                            ImageStream.Dispose();
                        }

                    } catch (Exception e)
                    {
                        throw e;
                    } finally
                    {
                        localFile.Close();
                        localFile.Dispose();
                    }
                    
                }
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Copy(string inputFilePath, string outputFilePath)
        {           
            int bufferSize = 1024 * 1024;

            using (FileStream fileStream = new FileStream(outputFilePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite))
            {
                FileStream fs = new FileStream(inputFilePath, FileMode.Open, FileAccess.Read);
                fileStream.SetLength(fs.Length);
                int bytesRead = -1;
                int byesTotal = 0;
                byte[] bytes = new byte[bufferSize];

                while ((bytesRead = fs.Read(bytes, 0, bufferSize)) > 0)
                {
                    fileStream.Write(bytes, 0, bytesRead);
                    byesTotal += bytesRead;
                        
                }

                fs.Close();      
            }
        }

        public void DeleteTemporaryFile()
        {
            if (TempPath != null)
            {
                if (Compressed)
                {
                    
                }

                if (ImageStream != null)
                {
                    ImageStream.Close();
                    ImageStream.Dispose();
                }

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
            if (!Closed)
            {
                this.LoadImage();
            }

            return Image;
        }

        //[MethodImpl(MethodImplOptions.Synchronized)]
        public void LoadImageInfo(bool force = false)
        {
            if ((!Closed && W == 0 && H == 0 && !ImageInfoRequested) || force)
            {
                ImageInfoRequested = true;
                if (ImageStream == null)
                {
                    if (TempPath != null)
                    {

                        ImageInfo = Image.FromFile(TempPath);
                        W = ImageInfo.Width;
                        H = ImageInfo.Height;

                        ImageInfo.Dispose();
                        ImageInfo = null;
                    }

                    if (Compressed)
                    {
                        ImageInfo = Image.FromStream(ImageEntry.Open());
                        W = ImageInfo.Width;
                        H = ImageInfo.Height;


                        ImageInfo.Dispose();
                        ImageInfo = null;
                    }
                } else
                {
                    try
                    {
                        ImageInfo = Image.FromStream(ImageStream);
                        W = ImageInfo.Width;
                        H = ImageInfo.Height;

                        ImageInfo.Dispose();
                        ImageInfo = null;
                    } catch {
                        MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_WARNING, "Unable to read image [" + Filename + "]");
                    }

                }
            }
        }


        public Image GetThumbnail(Image.GetThumbnailImageAbort callback, IntPtr data)
        {
            if (!Closed)
            {
                LoadImage();
            }

            if (Image != null)
            {
                Thumbnail = Image.GetThumbnailImage(ThumbW, ThumbH, callback, data);

                Image.Dispose();
                Image = null;
            }

            return Thumbnail;
        }


        public String CreateLocalWorkingCopy(String destination = null)
        {
            bool tempFileExists = false;

            if (Compressed)
            {
                try
                {
                    FileInfo temporaryFileInfo = new FileInfo(TempPath);
                    tempFileExists = temporaryFileInfo.Exists;
                } catch {
                    tempFileExists = false;
                }

                if (!tempFileExists)
                {
                    RequestTemporaryFile(destination);

                    if (TempPath != null)
                    {
                        return TempPath;
                    }
                } else
                {
                    
                }
            }
            else
            {
                if (destination == null)
                {
                    destination = WorkingDir + TemporaryFileId + ".tmp";
                }

                FileInfo copyFileInfo = new FileInfo(destination);
                if (ImageStream != null)
                {
                    if (ImageStream.CanSeek && ImageStream.Position == copyFileInfo.Length)
                    {
                        ImageStream.Seek(0, SeekOrigin.Begin);  
                    }

                    if (ImageStream.CanRead)
                    {
                        
                        FileStream localCopyStream = copyFileInfo.Open(FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read);
                        try
                        {
                            ImageStream.CopyTo(localCopyStream);
                        } catch (Exception ce) {
                            throw ce;
                        } finally
                        {                          
                            localCopyStream.Close();
                            localCopyStream.Dispose();
                        }
                                          
                    } else
                    {
                        if (LocalPath != null)
                        {
                            FileStream localCopyStream = copyFileInfo.Open(FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read);
                            try
                            {
                                FileStream destinationStream = File.Create(destination);

                                try
                                {
                                    localCopyStream.CopyTo(destinationStream);
                                } catch (Exception fwe)
                                {
                                    throw fwe;
                                } finally 
                                { 
                                    destinationStream.Close();
                                    destinationStream.Dispose();
                                }
                                
                            }
                            catch (Exception ce)
                            {
                                throw ce;
                            }
                            finally
                            {
                                localCopyStream.Close();
                                localCopyStream.Dispose();
                            }
                        }
                    }
                } else
                {
                    if (LocalPath != null)
                    {
                        FileStream localCopyStream = copyFileInfo.Open(FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read);
                        
                        try
                        {
                            FileStream destinationStream = File.Create(destination);
                            try
                            {
                                localCopyStream.CopyTo(destinationStream);
                            }
                            catch (Exception fwe)
                            {
                                throw fwe;
                            }
                            finally
                            {
                                destinationStream.Close();
                                destinationStream.Dispose();
                            }
                        } catch (Exception fwe)
                        {
                            throw fwe;
                        } finally
                        {
                            localCopyStream.Close();
                            localCopyStream.Dispose();
                        }
                    }
                }

                if (copyFileInfo.Exists)
                {
                    TempPath = destination;

                    return destination;
                }
            }

            return TempPath;
        }


        private void LoadImage()
        {
            if (!Closed)
            {
                if (Image == null)
                {
                    if (IsMemoryCopy)
                    {
                        if (ImageStreamMemoryCopy != null && ImageStreamMemoryCopy.CanRead)
                        {
                            try
                            {
                                Image = Image.FromStream(ImageStreamMemoryCopy);
                            } catch {
                                throw new Exception("Error loading image [" + Name + "]! Invalid or corrupted image");
                            }
                        }
                    }
                    else
                    {
                        if (ImageStream != null && ImageStream.CanRead)
                        {
                            try
                            {
                                Image = Image.FromStream(ImageStream);
                            }
                            catch {
                                throw new Exception("Error loading image [" + Name + "]! Invalid or corrupted image");
                            }
                        }
                    }
                }

                if (Image == null)
                {
                    RequestTemporaryFile();
                    if (TempPath != null) {
                        ImageStream = File.Open(TempPath, FileMode.Open, FileAccess.ReadWrite);
                        Image = Image.FromStream(ImageStream);
                    } else {
                        throw new Exception("Failed to extract image [" + Name + "] from Archive!");
                    } 
                }

                if (Image != null)
                {
                    if (!Image.Size.IsEmpty)
                    {
                        W = Image.Width;
                        H = Image.Height;
                    }

                    ImageStream.Close();
                }
                else
                {
                    throw new Exception("Failed to load/extract image!");
                }
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

            if (IsMemoryCopy)
            {
                if (ImageStreamMemoryCopy != null)
                {
                    ImageStreamMemoryCopy.Close();
                }
            }
        }
    }
}
