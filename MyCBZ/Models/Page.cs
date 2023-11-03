using Win_CBZ;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using Win_CBZ.Models;
using System.Xml.Linq;
using System.Runtime.CompilerServices;
using System.Diagnostics.Eventing.Reader;
using System.Drawing.Imaging;

namespace Win_CBZ
{
    internal class Page
    {

        public String Id { get; set; }

        public String Hash { get; set; }

        public String TemporaryFileId { get; set; }

        public String Filename { get; set; }

        public String Name { get; set; }

        public String OriginalName { get; set; }

        public String EntryName { get; set; } 

        public String FileExtension { get; set; }

        public String ImageType { get; set; } = "Story";

        public bool ImageLoaded { get; set; }

        public ImageTask ImageTask { get; set; }

        public int Number { get; set; }

        public long Size { get; set; }

        public String WorkingDir { get; set; }

        public LocalFile LocalFile { get; set; }

        public LocalFile TemporaryFile { get; set; }

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

        public ImageFormat Format { get; set; }

        public PixelFormat PixelFormat { get; set; }

        public ColorPalette Palette { get; set; }

        public float DPI { get; set; }

        protected int ThumbW { get; set; } = 212;

        protected int ThumbH { get; set; } = 256;

        private Image Image;

        private Image ImageInfo;

        private Image Thumbnail;

        private Stream ImageStream;

        private MemoryStream ImageStreamMemoryCopy;

        private FileInfo ImageFileInfo;

        private ZipArchiveEntry CompressedEntry;

        public delegate EventHandler<FileOperationEvent> FileOperationEventHandler();
        
        public Page(String fileName, FileAccess mode = FileAccess.Read)
        {
            LocalFile = new LocalFile(fileName);
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


        public Page(LocalFile localFile, FileInfo tempFileName, FileAccess mode = FileAccess.Read)
        {
            try {
                Copy(localFile.FullPath, tempFileName.FullName);

                TemporaryFile = new LocalFile(tempFileName.FullName);

                ImageFileInfo = new FileInfo(tempFileName.FullName);
                ReadOnly = (mode == FileAccess.Read && mode != FileAccess.ReadWrite) || ImageFileInfo.IsReadOnly;
                try
                {
                    if ((mode == FileAccess.Write || mode == FileAccess.ReadWrite) && ImageFileInfo.IsReadOnly)
                    {
                        RemoveReadOnlyAttribute(ref ImageFileInfo);
                    }
                    ImageStream = ImageFileInfo.Open(FileMode.Open, mode, FileShare.ReadWrite);
                    ReadOnly = !ImageStream.CanWrite;
                }
                catch (UnauthorizedAccessException)
                {
                    ImageStream = ImageFileInfo.Open(FileMode.Open, FileAccess.Read, FileShare.Read);
                    ReadOnly = true;
                }
                finally
                {
                    LocalFile = localFile;
                    WorkingDir = tempFileName.Directory.FullName;
                    if (ReadOnly)
                    {
                        TemporaryFile = CreateLocalWorkingCopy();
                    }
                }


            } catch { 
                
            }
          
            Filename = ImageFileInfo.FullName;
            FileExtension = localFile.FileExtension;
            LocalPath = ImageFileInfo.Directory.FullName;
            Name = localFile.FileName;
            Size = ImageFileInfo.Length;
            Id = Guid.NewGuid().ToString();
            ImageTask = new ImageTask();
        }


        public Page(ZipArchiveEntry entry, String workingDir, String randomId)
        {
            CompressedEntry = entry;
            Compressed = true;

            Filename = entry.FullName;
            FileExtension = ExtractFileExtension(entry.Name);
            Name = entry.Name;
            Size = entry.Length;
            EntryName = entry.Name;
            LastModified = entry.LastWriteTime;
            Id = Guid.NewGuid().ToString();
            TemporaryFileId = randomId;
            WorkingDir = workingDir;
            ImageTask = new ImageTask();
        }


        public Page(Stream fileInputStream, String name)
        {
            
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
 
            Filename = sourcePage.Filename;
            LocalPath = sourcePage.LocalPath;
            //ImageStream = sourcePage.ImageStream;
            LocalFile = sourcePage.LocalFile;

            FileExtension = sourcePage.FileExtension;
            Compressed = sourcePage.Compressed;
            TemporaryFileId = RandomId;

            EntryName = sourcePage.EntryName;
            CompressedEntry = sourcePage.CompressedEntry;

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

            TemporaryFile = RequestTemporaryFile();

            if (sourcePage.ImageStream != null)
            {
                if (sourcePage.ImageStream.CanRead)
                {
                    sourcePage.ImageStream.Position = 0;

                    ImageStreamMemoryCopy = new MemoryStream();
                    sourcePage.ImageStream.CopyTo(ImageStreamMemoryCopy);
                    IsMemoryCopy = true;
                }
            }
                          
            
            Thumbnail = sourcePage.Thumbnail;
            ThumbnailInvalidated = sourcePage.ThumbnailInvalidated;
            
            ImageTask = new ImageTask();
        }

        public Page(Page sourcePage)
        {
            if (sourcePage != null)
            {
                WorkingDir = sourcePage.WorkingDir;
                Name = sourcePage.Name;
                EntryName = sourcePage.EntryName;
                TempPath = sourcePage.TempPath;
                Filename = sourcePage.Filename;
                LocalPath = sourcePage.LocalPath;
                LocalFile = sourcePage.LocalFile;
                ImageStream = sourcePage.ImageStream;
                Compressed = sourcePage.Compressed;
                TemporaryFileId = sourcePage.TemporaryFileId;
                EntryName = sourcePage.EntryName;
                CompressedEntry = sourcePage.CompressedEntry;
                ImageStream = sourcePage.ImageStream;
                IsMemoryCopy = sourcePage.IsMemoryCopy;
                //ImageStreamMemoryCopy = sourcePage.ImageStreamMemoryCopy;

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

                TemporaryFile = sourcePage.TemporaryFile;

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

                ImageTask = sourcePage.ImageTask;
            }
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
            LocalFile = page.LocalFile;
            TemporaryFileId = page.TemporaryFileId;
            Changed = page.Changed;
            ImageType = page.ImageType;
            ImageTask = page.ImageTask;


            TemporaryFile = page.TemporaryFile;

            if (!skipName)
            {
                Name = page.Name;
            }

            if (!skipIndex)
            {
                Index = page.Index;
            }
            //OriginalIndex = page.OriginalIndex;
        }

        public void UpdateImageEntry(ZipArchiveEntry entry, String randomId)
        {
            CompressedEntry = entry;
            Compressed = true;

            Filename = entry.FullName;
            Name = entry.Name;
            //Size = entry.Length;
            LastModified = entry.LastWriteTime;
            Id = Guid.NewGuid().ToString();
            TemporaryFileId = randomId;
        }

        public void UpdateLocalWorkingCopy(LocalFile localFile, FileInfo tempFileName = null)
        {
            Copy(localFile.FullPath, tempFileName.FullName);

            ImageFileInfo = new FileInfo(tempFileName.FullName);

            LocalFile = new LocalFile(localFile.FullPath);
            TemporaryFile = new LocalFile(tempFileName.FullName);
            Size = ImageFileInfo.Length;
            LocalPath = localFile.FullPath;
            Compressed = false;
            LastModified = localFile.LastModified;
            Name = localFile.FileName;

            //String newTempFileName = CreateLocalWorkingCopy(ExtractFileExtension(localFile.FullPath));
            //TempPath = new FileInfo(newTempFileName).FullName;

            
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

        public ZipArchiveEntry GetCompressedEntry()
        {
            return CompressedEntry;
        }

        protected LocalFile RequestTemporaryFile(String destination = null, bool overwrite = false)
        {
            String result = null;

            if (TemporaryFileId == null)
            {
                MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_WARNING, "Cant create new temporary file for page [" + Name + "]! No FileID available/set.");

                return null;
            }

            if (Compressed)
            {
                if (CompressedEntry != null)
                {
                    if ((TemporaryFile == null || !TemporaryFile.Exists()) || destination != null)
                    {
                        if (destination == null)
                        {
                            destination = Path.Combine(PathHelper.ResolvePath(WorkingDir), TemporaryFileId + ".tmp");
                        }

                        if (!File.Exists(destination) || overwrite)
                        {
                            CompressedEntry.ExtractToFile(destination, overwrite);
                        }
                        

                        result = destination;
                    }
                }
                else
                {
                    MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_WARNING, "No Entry with name [" + Name + "] exists in archive!");
                }
            } else
            {
                if (ReadOnly || TemporaryFile == null || !TemporaryFile.Exists())
                {
                    if (TemporaryFile == null || !TemporaryFile.Exists() || destination != null)
                    {
                        if (destination == null)
                        {
                            destination = Path.Combine(PathHelper.ResolvePath(WorkingDir), TemporaryFileId + ".tmp");
                        }

                        result = destination;
                    }

                    FileInfo tempFileInfo = new FileInfo(result);

                    if (!IsMemoryCopy)
                    {
                        FileStream localFile = File.OpenRead(LocalFile.FullPath);
                        try
                        {

                            FileStream CopyImageStream = File.Open(result, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                            try
                            {
                                localFile.CopyTo(CopyImageStream);
                            }
                            catch (Exception ewr)
                            {
                                throw new PageException(this, ewr.Message, true, ewr);
                            }
                            finally
                            {
                                CopyImageStream.Close();
                                CopyImageStream.Dispose();
                            }
                        }
                        catch (Exception e)
                        {
                            throw new PageException(this, e.Message, true, e);
                        }
                        finally
                        {
                            localFile.Close();
                            localFile.Dispose();
                        }
                    } else
                    {
                        try
                        {
                            FileStream CopyImageStream = File.Open(result, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                            try
                            {
                                ImageStreamMemoryCopy.CopyTo(CopyImageStream);
                            }
                            catch (Exception ewr)
                            {
                                throw new PageException(this, ewr.Message, true, ewr);
                            }
                            finally
                            {
                                CopyImageStream.Close();
                                CopyImageStream.Dispose();
                            }


                        }
                        catch (Exception e)
                        {
                            throw new PageException(this, e.Message, true, e);
                        }
                        finally
                        {

                        }
                    }
                    
                } else
                {
                    return TemporaryFile;
                }
            }

            return new LocalFile(result);
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
            if (TemporaryFile != null && TemporaryFile.Exists())
            {
                if (Compressed)
                {
                    
                }

                if (ImageStream != null)
                {
                    ImageStream.Close();
                    ImageStream.Dispose();
                }

                File.Delete(TemporaryFile.FullPath);
            }
        }

        public void DeleteLocalFile()
        {
            if (!ReadOnly)
            {
                File.Delete(LocalFile.FullPath);
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

        public Stream GetImageStream()
        {
            if (!Closed)
            {
                this.LoadImage();
            }

            return ImageStream;
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
                        ImageInfo = Image.FromStream(CompressedEntry.Open());
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
                try
                {
                    Thumbnail = Image.GetThumbnailImage(ThumbW, ThumbH, callback, data);

                    Image.Dispose();
                    Image = null;
                } catch (Exception ex)
                {
                    throw new PageException(this, ex.Message, true, ex);
                }
            }

            return Thumbnail;
        }


        public LocalFile CreateLocalWorkingCopy(String destination = null)
        {

            if (Compressed)
            {
                if (TemporaryFile == null || !TemporaryFile.Exists())
                {
                    TemporaryFile = RequestTemporaryFile(destination);

                    if (TemporaryFile != null)
                    {
                        return TemporaryFile;
                    }
                } else
                {
                    return TemporaryFile;
                }
            }
            else
            {
                if (destination == null)
                {
                    destination = Path.Combine(PathHelper.ResolvePath(WorkingDir), TemporaryFileId + ".tmp");
                }

                FileInfo copyFileInfo = new FileInfo(destination);   // Target
                if (ImageStream != null)
                {
                    if (ImageStream.CanSeek && ImageStream.Position > 0)
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
                                    throw new PageException(this, fwe.Message, true, fwe);
                                } finally 
                                { 
                                    destinationStream.Close();
                                    destinationStream.Dispose();
                                }
                                
                            }
                            catch (Exception ce)
                            {
                                throw new PageException(this, ce.Message, true, ce);
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
                    if (LocalFile != null)
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
                                throw new PageException(this, fwe.Message, true, fwe);
                            }
                            finally
                            {
                                destinationStream.Close();
                                destinationStream.Dispose();
                            }
                        } catch (Exception fwe)
                        {
                            throw new PageException(this, fwe.Message, true, fwe);
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

                    TemporaryFile = new LocalFile(TempPath);

                    return TemporaryFile;
                }
            }

            TemporaryFile = new LocalFile(TempPath);

            return TemporaryFile;
        }


        public void LoadImage()
        {
            bool reloadImageStream =false;

            if (!Closed)
            {
                if (ImageStream != null)
                {
                    if (!ImageStream.CanRead)
                    {
                        reloadImageStream = true;
                    }
                }

                if (Image == null)
                {
                    if (IsMemoryCopy)
                    {
                        if (ImageStreamMemoryCopy != null && ImageStreamMemoryCopy.CanRead)
                        {
                            try
                            {
                                Image = Image.FromStream(ImageStreamMemoryCopy);
                                ImageLoaded = true;
                            } catch (Exception ioe) {
                                throw new PageException(this, "Error loading image [" + Name + "]! Invalid or corrupted image", true, ioe);
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
                                ImageLoaded = true;
                            }
                            catch (Exception ioi) {
                                throw new PageException(this, "Error loading image [" + Name + "]! Invalid or corrupted image", true, ioi);
                            }
                        }
                    }
                }

                if (Image == null || reloadImageStream)
                {
                    if (TemporaryFile == null || !TemporaryFile.Exists())
                    {
                        TemporaryFile = RequestTemporaryFile();
                    }
                    
                    if (TemporaryFile != null && TemporaryFile.Exists()) {
                        ImageStream = File.Open(TemporaryFile.FullPath, FileMode.Open, FileAccess.ReadWrite);
                        Image = Image.FromStream(ImageStream);
                        ImageLoaded = true;
                    } else {
                        throw new PageException(this, "Failed to extract image [" + Name + "] from Archive!");
                    } 
                }

                if (Image != null)
                {
                    try
                    {
                        if (!Image.Size.IsEmpty)
                        {
                            W = Image.Width;
                            H = Image.Height;
                        }
                        Format = Image.RawFormat;
                        PixelFormat = Image.PixelFormat;
                        Palette = Image.Palette;
                        DPI = Image.VerticalResolution;
                    } catch (Exception ie)
                    {
                        throw new PageException(this, ie.Message);
                    }
                }
                else
                {
                    throw new PageException(this, "Failed to load/extract image!");
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
                    ImageStreamMemoryCopy.Dispose();
                }
            }
        }
    }
}
