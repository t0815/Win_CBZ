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
using Win_CBZ.Helper;
using System.Xml;
using SharpCompress.Common;
using System.IO.Pipes;

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

        public bool DoublePage { get; set; } = false;

        public bool ImageLoaded { get; set; }

        public bool ImageMetaDataLoaded { get; set; }

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

        public int Index { get; set; }

        public int OriginalIndex { get; set; }

        public String Key { get; set; }

        public DateTimeOffset LastModified { get; set; }

        public PageImageFormat Format { get; set; }

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
            Format = new PageImageFormat(LocalFile.FileExtension);
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
            Key = RandomId.getInstance().make();
        }


        public Page(LocalFile localFile, FileInfo tempFileName, FileAccess mode = FileAccess.Read)
        {
            try {
                Copy(localFile.FullPath, tempFileName.FullName);

                Format = new PageImageFormat(localFile.FileExtension);
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
            Key = RandomId.getInstance().make();
            LastModified = localFile.LastModified;
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
            Format = new PageImageFormat(FileExtension);
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
            Format = new PageImageFormat(FileExtension);
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
            Format = sourcePage.Format;

            FileExtension = sourcePage.FileExtension;
            Compressed = sourcePage.Compressed;
            TemporaryFileId = RandomId;

            LastModified = sourcePage.LastModified;

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
            DoublePage = sourcePage.DoublePage;

            Deleted = sourcePage.Deleted;
            OriginalName = sourcePage.OriginalName;
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
                } else
                {
                    if (Image != null)
                    {
                        Image?.Dispose();
                        Image = null;
                    }

                    ImageStream?.Close();
                    ImageStream = null;
                }
            }
                                    
            Thumbnail = sourcePage.Thumbnail;
            ThumbnailInvalidated = sourcePage.ThumbnailInvalidated;
            
            ImageTask = new ImageTask();
        }

        public Page(Page sourcePage, bool newCopy = false)
        {
            if (sourcePage != null)
            {
                WorkingDir = sourcePage.WorkingDir;
                Name = sourcePage.Name;
                EntryName = sourcePage.EntryName;
                TempPath = sourcePage.TempPath;
                Filename = sourcePage.Filename;
                FileExtension = sourcePage.FileExtension;
                LocalPath = sourcePage.LocalPath;
                LocalFile = sourcePage.LocalFile;
                ImageStream = sourcePage.ImageStream;
                Compressed = sourcePage.Compressed;
                TemporaryFileId = RandomId.getInstance().make();
                EntryName = sourcePage.EntryName;
                CompressedEntry = sourcePage.CompressedEntry;
                ImageStream = sourcePage.ImageStream;
                IsMemoryCopy = sourcePage.IsMemoryCopy;
                //ImageStreamMemoryCopy = sourcePage.ImageStreamMemoryCopy;
                Format = sourcePage.Format;
                LastModified = sourcePage.LastModified;

                Changed = sourcePage.Changed;
                ReadOnly = sourcePage.ReadOnly;
                Size = sourcePage.Size;
                Id = newCopy ? Guid.NewGuid().ToString() : sourcePage.Id;
                Index = sourcePage.Index;
                OriginalIndex = sourcePage.OriginalIndex;
                Number = sourcePage.Number;
                Closed = sourcePage.Closed;
                DoublePage = sourcePage.DoublePage;

                Deleted = sourcePage.Deleted;
                OriginalName = sourcePage.OriginalName;
                Key = sourcePage.Key;
                ThumbH = sourcePage.ThumbH;
                ThumbW = sourcePage.ThumbW;
                Thumbnail = sourcePage.Thumbnail;
                ThumbnailInvalidated = sourcePage.ThumbnailInvalidated;

                if (newCopy)
                {
                    TemporaryFile = RequestTemporaryFile();
                } else
                {
                    TemporaryFile = sourcePage.TemporaryFile;
                }
                

                if (ImageStream != null)
                {
                    if (ImageStream.CanRead)
                    {
                        sourcePage.ImageStream.Position = 0;

                        ImageStreamMemoryCopy = new MemoryStream();
                        sourcePage.ImageStream.CopyTo(ImageStreamMemoryCopy);
                        IsMemoryCopy = true;
                    } else
                    {
                        if (Image != null)
                        {
                            Image?.Dispose();
                            Image = null;
                        }

                        ImageStream?.Close();
                        ImageStream = null;
                    }
                }

                ImageTask = sourcePage.ImageTask;
                ImageMetaDataLoaded = sourcePage.ImageMetaDataLoaded;
            }
        }


        public Page(Stream fileInputStream, FileAccess mode = FileAccess.Read)
        {
            XmlDocument Document = new XmlDocument();
                        
            XmlReader MetaDataReader = XmlReader.Create(fileInputStream);
            MetaDataReader.Read();
            Document.Load(MetaDataReader);

            String sourceProjectId = "";

            XmlNode Root;

            foreach (XmlNode node in Document.ChildNodes)
            {
                if (node.Name.ToLower().Equals("win_cbz_page"))
                {
                    Root = node;
                    foreach (XmlNode subNode in node.ChildNodes)
                    {
                        switch (subNode.Name.ToLower())
                        {
                            case "projectguid":
                                sourceProjectId = subNode.InnerText;
                                break;
                            case "localfile":
                                HandlePageMetaData(subNode, "LocalFile");
                                break;
                            case "temporaryfile":
                                HandlePageMetaData(subNode, "TemporaryFile");
                                break;
                            default:
                                HandlePageMetaData(subNode);
                                break;
                        }
                    }
                }
            }

            MetaDataReader?.Close();
            MetaDataReader?.Dispose();

            TemporaryFileId = RandomId.getInstance().make();
            Id = Guid.NewGuid().ToString();
            IsMemoryCopy = false;
            ImageLoaded = false;

            if (LocalFile == null)
            {
                //if (Program.ProjectModel.ProjectGUID != sourceProjectId)
                //{
                    if (WorkingDir != null)
                    {
                        DirectoryInfo currentWorkingDir = new DirectoryInfo(WorkingDir);
                        String baseDir = currentWorkingDir.Parent.FullName;

                        String targetPath = Path.Combine(baseDir, sourceProjectId);
                        String targetFile = Path.Combine(targetPath, Name);

                        if (File.Exists(targetFile))
                        {
                            targetFile = Path.Combine(targetPath, RandomId.getInstance().make() + ".tmp");
                        }

                        Copy(TemporaryFile.FullPath, targetFile);
                        LocalFile = new LocalFile(targetFile);
                    }

                    if (Compressed)
                    {
                        Compressed = false;
                    }
                //} else
                //{
                //    LocalFile = new LocalFile(TemporaryFile.FullPath);
                //}

                //
            }

           
            ImageFileInfo = new FileInfo(TemporaryFile.FullPath);
            Format = new PageImageFormat(FileExtension);
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

      
        }

        protected void HandlePageMetaData(XmlNode node, string type = null)
        {
            if (type == "LocalFile")
            {
                foreach (XmlNode subNode in node.ChildNodes)
                {
                    if (subNode.Name == "FullPath")
                    {
                        LocalFile = new LocalFile(subNode.InnerText);
                    }
                    
                }      
            }
            else if (type == "TemporaryFile")
            {
                foreach (XmlNode subNode in node.ChildNodes)
                {
                    if (subNode.Name == "FullPath")
                    {
                        TemporaryFile = new LocalFile(subNode.InnerText);
                    }

                }
            }
            else
            {
                switch (node.Name)
                {
                    case "Id":
                        Id = node.InnerText;
                        break;

                    case "Name":
                        Name = node.InnerText;
                        break;

                    case "Hash":
                        Hash = node.InnerText;
                        break;

                    case "Key":
                        Key = node.InnerText;
                        break;

                    case "Size":
                        Size = long.Parse(node.InnerText);
                        break;

                    case "ImageType":
                        ImageType = node.InnerText;
                        break;

                    case "TemporaryFileId":
                        TemporaryFileId = node.InnerText;
                        break;

                    case "Filename":
                        Filename = node.InnerText;
                        break;

                    case "FileExtension":
                        FileExtension = node.InnerText;
                        break;

                    case "EntryName":
                        EntryName = node.InnerText;
                        break;

                    case "OriginalName":
                        OriginalName = node.InnerText;
                        break;

                    case "LocalPath":
                        LocalPath = node.InnerText;
                        break;

                    case "TempPath":
                        TempPath = node.InnerText;
                        break;

                    case "WorkingDir":
                        WorkingDir = node.InnerText;
                        break;

                    case "Index":
                        Index = int.Parse(node.InnerText);
                        break;

                    case "OriginalIndex":
                        OriginalIndex = int.Parse(node.InnerText);
                        break;

                    case "Number":
                        Number = int.Parse(node.InnerText);
                        break;

                    case "DoublePage":
                        DoublePage = Boolean.Parse(node.InnerText);
                        break;

                    case "Deleted":
                        Deleted = Boolean.Parse(node.InnerText);
                        break;

                    case "Changed":
                        Changed = Boolean.Parse(node.InnerText);
                        break;

                    case "Compressed":
                        Compressed = Boolean.Parse(node.InnerText);
                        break;

                    case "ReadOnly":
                        ReadOnly = Boolean.Parse(node.InnerText);
                        break;

                    case "Selected":
                        Selected = Boolean.Parse(node.InnerText);
                        break;

                    case "Invalidated":
                        Invalidated = Boolean.Parse(node.InnerText);
                        break;

                    case "IsMemoryCopy":
                        IsMemoryCopy = Boolean.Parse(node.InnerText);
                        break;

                    case "ImageInfoRequested":
                        ImageInfoRequested = Boolean.Parse(node.InnerText);
                        break;

                    case "ImageMetaDataLoaded":
                        ImageMetaDataLoaded = Boolean.Parse(node.InnerText);
                        break;

                    case "ImageLoaded":
                        ImageLoaded = Boolean.Parse(node.InnerText);
                        break;

                    case "Closed":
                        Closed = Boolean.Parse(node.InnerText);
                        break;

                    case "ThumbnailInvalidated":
                        ThumbnailInvalidated = Boolean.Parse(node.InnerText);
                        break;

                    case "LastModified":
                        LastModified = DateTimeOffset.Parse(node.InnerText);
                        break;
                }
            }

        }

        public void UpdatePage(Page page, bool skipIndex = false, bool skipName = false)
        {
            Compressed = page.Compressed;
            Filename = page.Filename;
            
            EntryName = page.EntryName;
            Size = page.Size;
            Id = page.Id;
            Key = page.Key;
            Number = page.Number;
            Deleted = page.Deleted;
            LocalFile = page.LocalFile;
            TemporaryFileId = page.TemporaryFileId;
            Changed = page.Changed;
            ImageType = page.ImageType;
            ImageTask = page.ImageTask;
            Format = page.Format;
            DoublePage = page.DoublePage;


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
            Key = RandomId.getInstance().make();

            //String newTempFileName = CreateLocalWorkingCopy(ExtractFileExtension(localFile.FullPath));
            //TempPath = new FileInfo(newTempFileName).FullName;
        }

        public void UpdateImage(Stream imageStream, LocalFile newImageFile)
        {
            if (imageStream == null && imageStream.CanRead && newImageFile != null)
            {
                FileStream newImageFileStream = null;
                try
                {
                    newImageFileStream = File.Open(newImageFile.FullPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                    imageStream.CopyTo(newImageFileStream);
                } catch (FileNotFoundException fe)
                {

                } catch (IOException ioe) 
                { 
                    
                } finally 
                {
                    newImageFileStream?.Close();
                    //imageStream?.Close(); 
                }


                ImageFileInfo = new FileInfo(newImageFile.FullPath);

                LocalFile = new LocalFile(newImageFile.FullPath);
                TemporaryFile = null;
                Size = ImageFileInfo.Length;
                
                Compressed = false;
                Changed = true;
                LastModified = LocalFile.LastModified;
                Name = LocalFile.FileName;
                Key = RandomId.getInstance().make();

                TemporaryFileId = RandomId.getInstance().make();
                Format = new PageImageFormat();
                Image = null;
                ImageLoaded = false;
                ImageStream = imageStream;
                ThumbnailInvalidated = true;
                

            }
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


        public MemoryStream Serialize(String sourceProjectId, bool withoutXMLHeaderTag = false)
        {
            MemoryStream ms = new MemoryStream();
            XmlWriterSettings writerSettings = new XmlWriterSettings
            {
                OmitXmlDeclaration = withoutXMLHeaderTag
            };

            XmlWriter xmlWriter = XmlWriter.Create(ms, writerSettings);

            xmlWriter.WriteStartDocument();

            xmlWriter.WriteStartElement("Win_CBZ_Page");
            xmlWriter.WriteElementString("ProjectGUID", sourceProjectId);
            xmlWriter.WriteElementString("Id", Id);
            xmlWriter.WriteElementString("Name", Name);
            xmlWriter.WriteElementString("Hash", Hash);
            xmlWriter.WriteElementString("Key", Key);
            xmlWriter.WriteElementString("TemporaryFileId", TemporaryFileId);
            xmlWriter.WriteElementString("Filename", Filename);
            xmlWriter.WriteElementString("OriginalName", OriginalName);
            xmlWriter.WriteElementString("Number", Number.ToString());
            xmlWriter.WriteElementString("Index", Index.ToString());
            xmlWriter.WriteElementString("OriginalIndex", OriginalIndex.ToString());
            xmlWriter.WriteElementString("EntryName", EntryName);
            
            xmlWriter.WriteElementString("ImageType", ImageType);
            xmlWriter.WriteElementString("DoublePage", DoublePage.ToString());
            xmlWriter.WriteElementString("ImageLoaded", "False");
            xmlWriter.WriteElementString("ImageMetaDataLoaded", "False");
            xmlWriter.WriteElementString("Number", Number.ToString());
            xmlWriter.WriteElementString("Size", Size.ToString());
            xmlWriter.WriteElementString("WorkingDir", WorkingDir);
            

            if (LocalFile != null)
            {
                // LocalFile
                xmlWriter.WriteStartElement("LocalFile");
                xmlWriter.WriteElementString("FullPath", LocalFile.FullPath);

                xmlWriter.WriteEndElement();
            }

            if (Compressed)
            {
                FileInfo NewTemporaryFileName = Program.ProjectModel.MakeNewTempFileName();
                TemporaryFile = CreateLocalWorkingCopy(NewTemporaryFileName.FullName);
            }

            //
            if (TemporaryFile == null)
            {
                TemporaryFile = RequestTemporaryFile();
            }


            if (TemporaryFile != null)
            {
                // TemporaryFile
                xmlWriter.WriteStartElement("TemporaryFile");
                xmlWriter.WriteElementString("FullPath", TemporaryFile.FullPath);

                xmlWriter.WriteEndElement();
            }

            //
            if (Format != null)
            {
                xmlWriter.WriteStartElement("Format");
                xmlWriter.WriteElementString("W", Format.W.ToString());
                xmlWriter.WriteElementString("H", Format.H.ToString());
                xmlWriter.WriteElementString("DPI", Format.DPI.ToString());
                xmlWriter.WriteElementString("Format", Format.Format.ToString());
                xmlWriter.WriteElementString("Name", Format.Name.ToString());
                xmlWriter.WriteElementString("PixelFormat", Format.PixelFormat.ToString());

                xmlWriter.WriteEndElement();
            }

            if (Compressed && CompressedEntry != null)
            {
                // Zip Entry
                xmlWriter.WriteStartElement("CompressedEntry");
                xmlWriter.WriteElementString("Name", CompressedEntry.Name);
                xmlWriter.WriteElementString("Source", Program.ProjectModel.FileName);

                xmlWriter.WriteEndElement();
            }

            xmlWriter.WriteElementString("FileExtension", FileExtension);

            // ----
            xmlWriter.WriteElementString("LocalPath", LocalPath);
            xmlWriter.WriteElementString("TempPath", TempPath);

            xmlWriter.WriteElementString("Compressed", Compressed.ToString());
            xmlWriter.WriteElementString("Changed", Changed.ToString());
            xmlWriter.WriteElementString("Deleted", Deleted.ToString());
            xmlWriter.WriteElementString("ReadOnly", ReadOnly.ToString());
            xmlWriter.WriteElementString("Selected", Selected.ToString());
            xmlWriter.WriteElementString("Invalidated", Invalidated.ToString());
            xmlWriter.WriteElementString("IsMemoryCopy", IsMemoryCopy.ToString());
            xmlWriter.WriteElementString("ImageInfoRequested", ImageInfoRequested.ToString());
            xmlWriter.WriteElementString("Closed", Closed.ToString());
            xmlWriter.WriteElementString("ThumbnailInvalidated", ThumbnailInvalidated.ToString());
            xmlWriter.WriteElementString("ImageLoaded", ImageLoaded.ToString());
            xmlWriter.WriteElementString("ImageMetaDataLoaded", ImageMetaDataLoaded.ToString());
            xmlWriter.WriteElementString("LastModified", LastModified.ToString());

            xmlWriter.WriteEndElement();

            //if (!withoutXMLHeaderTag)
            //{
            xmlWriter.WriteEndDocument();
            //}

            xmlWriter.Close();

            ms.Position = 0;

            return ms;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Close(bool keepTemporaryFiles = true)
        {
            FreeImage();

            if (!keepTemporaryFiles)
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
                    if ((TemporaryFile == null || !TemporaryFile.Exists()) || destination != null || overwrite)
                    {
                        if (destination == null)
                        {
                            destination = Path.Combine(PathHelper.ResolvePath(WorkingDir), TemporaryFileId + ".tmp");
                        }

                        if (!File.Exists(destination) || overwrite)
                        {
                            try { 
                                CompressedEntry.ExtractToFile(destination, overwrite);
                            } catch (Exception e)
                            {
                                throw new PageException(this, e.Message, true, e);
                            }
                        }
                        

                        result = destination;
                    } else
                    {
                        return TemporaryFile;
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
                    } else
                    {
                        return TemporaryFile;
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
                LoadImage();
            }

            return Image;
        }

        public Stream GetImageStream()
        {
            if (!Closed)
            {
                LoadImage();
            }

            return ImageStream;
        }

        //[MethodImpl(MethodImplOptions.Synchronized)]
        public void LoadImageInfo(bool force = false)
        {
            if ((!Closed && Format.W == 0 && Format.H == 0 && !ImageInfoRequested) || force)
            {
                ImageInfoRequested = true;
                if (ImageStream == null)
                {
                    if (TempPath != null)
                    {
                        Stream ImageFileStream = null;
                        try
                        {
                            ImageFileStream = File.Open(TempPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                            Image ImageInfo = Image.FromStream(stream: ImageFileStream,
                                                     useEmbeddedColorManagement: false,
                                                     validateImageData: false);

                            Format = new PageImageFormat(ImageInfo);

                            
                        } catch ( Exception e)
                        {

                        } finally
                        {
                            ImageFileStream?.Close();
                            ImageInfo?.Dispose();
                            ImageInfo = null;
                        }
                    }

                    if (Compressed)
                    {
                        ImageInfo = Image.FromStream(CompressedEntry.Open());
                        Format = new PageImageFormat(ImageInfo);


                        ImageInfo?.Dispose();
                        ImageInfo = null;
                    }
                } else
                {
                    try
                    {
                        ImageInfo = Image.FromStream(stream: ImageStream,
                                                     useEmbeddedColorManagement: false,
                                                     validateImageData: false);
                        Format = new PageImageFormat(ImageInfo);

                        ImageInfo?.Dispose();
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

                    Image?.Dispose();
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
                if (TemporaryFile == null || !TemporaryFile.Exists() || destination != null)
                {
                    TemporaryFile = RequestTemporaryFile(destination);

                    if (TemporaryFile != null)
                    {
                        return TemporaryFile;
                    }
                } else
                {
                    FileInfo copyFileInfo = new FileInfo(TemporaryFile.FullPath);   // Source
                    FileStream destinationStream = null;
                    Stream localCopyStream = null;

                    if (destination == null)
                    {
                        destination = Path.Combine(PathHelper.ResolvePath(WorkingDir), TemporaryFileId + ".tmp");
                    }

                    try
                    {
                        destinationStream = File.Create(destination);
                        
                        if (ImageStream != null)
                        {
                            if (ImageStream.CanRead)
                            {
                                localCopyStream = ImageStream;
                            }
                        } else
                        {
                            localCopyStream = copyFileInfo.Open(FileMode.Open, FileAccess.Read, FileShare.Read);
                        }

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
                            
                        }

                    }
                    catch (Exception ce)
                    {
                        throw new PageException(this, ce.Message, true, ce);
                    }
                    finally
                    {
                        destinationStream?.Close();
                        destinationStream?.Dispose();

                        localCopyStream?.Close();
                        localCopyStream?.Dispose();
                    }
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
                            throw new PageException(this, ce.Message, true, ce);
                        } finally
                        {                          
                            localCopyStream?.Close();
                            localCopyStream?.Dispose();
                        }
                                          
                    } else
                    {
                        if (LocalFile != null)
                        {
                            FileInfo copyFileInfoLocal = new FileInfo(LocalFile.FullPath);   // Source
                            FileStream localCopyStream = copyFileInfoLocal.Open(FileMode.Open, FileAccess.Read, FileShare.Read);
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
                                    destinationStream?.Close();
                                    destinationStream?.Dispose();
                                }
                                
                            }
                            catch (Exception ce)
                            {
                                throw new PageException(this, ce.Message, true, ce);
                            }
                            finally
                            {
                                localCopyStream?.Close();
                                localCopyStream?.Dispose();
                            }
                        }
                    }
                } else
                {
                    if (LocalFile != null)
                    {
                        FileInfo copyFileInfoLocal = new FileInfo(LocalFile.FullPath);   // Source
                        FileStream localCopyStream = copyFileInfoLocal.Open(FileMode.Open, FileAccess.Read, FileShare.Read);
                        
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
                                destinationStream?.Close();
                                destinationStream?.Dispose();
                            }
                        } catch (Exception fwe)
                        {
                            throw new PageException(this, fwe.Message, true, fwe);
                        } finally
                        {
                            localCopyStream?.Close();
                            localCopyStream?.Dispose();
                        }
                    }
                }
            }

            LocalFile result = new LocalFile(destination);

            if (result.Exists())
            {
                TemporaryFile = result;
            }
            

            return TemporaryFile;
        }

        public void LoadImage(bool metaDataOnly = false, bool reloadImageStream = false)
        {
            
            if (!Closed || reloadImageStream)
            {
                if (ImageStream != null)
                {
                    if (!ImageStream.CanRead)
                    {
                        reloadImageStream = true;
                    }
                }

                if (Image == null || reloadImageStream)
                {
                    if (IsMemoryCopy)
                    {
                        if (ImageStreamMemoryCopy != null && ImageStreamMemoryCopy.CanRead)
                        {
                            try
                            {
                                if (!metaDataOnly)
                                {
                                    Image = Image.FromStream(ImageStreamMemoryCopy);
                                    ImageLoaded = true;
                                } else
                                {
                                    Image imgMeta = null;
                                    try
                                    {
                                        imgMeta = Image.FromStream(stream: ImageStreamMemoryCopy,
                                                                            useEmbeddedColorManagement: false,
                                                                            validateImageData: false);
                                        Format.W = imgMeta.Width;
                                        Format.H = imgMeta.Height;
                                        Format.DPI = imgMeta.VerticalResolution;
                                        Format.Format = imgMeta.RawFormat;
                                        Format.Update();
                                    } catch (Exception eim) 
                                    {
                                        throw new PageException(this, "Error loading image properties [" + Name + "]! Invalid or corrupted image", true, eim);
                                    } finally 
                                    {
                                        imgMeta?.Dispose();
                                    }                                  
                                }
                            } catch (Exception ioe) {
                                throw new PageException(this, "Error loading image [" + Name + "]! Invalid or corrupted image", true, ioe);
                            }
                        }
                    }
                    else
                    {
                        if (ImageStream != null && ImageStream.CanRead && !reloadImageStream)
                        {
                            try
                            {
                                if (!metaDataOnly)
                                {
                                    Image = Image.FromStream(ImageStream);
                                    ImageLoaded = true;
                                } else
                                {
                                    Image imgMeta = null;
                                    try
                                    {
                                        imgMeta = Image.FromStream(stream: ImageStream,
                                                                   useEmbeddedColorManagement: false,
                                                                   validateImageData: false);
                                        Format.W = imgMeta.Width;
                                        Format.H = imgMeta.Height;
                                        Format.DPI = imgMeta.VerticalResolution;
                                        Format.Format = imgMeta.RawFormat;
                                        Format.Update();
                                    }
                                    catch (Exception eii)
                                    {
                                        throw new PageException(this, "Error loading image properties [" + Name + "]! Invalid or corrupted image", true, eii);
                                    }
                                    finally
                                    {
                                        imgMeta?.Dispose();
                                    }
                                }                               
                            }
                            catch (Exception ioi) 
                            {
                                throw new PageException(this, "Error loading image [" + Name + "]! Invalid or corrupted image", true, ioi);
                            }
                        }
                        else
                        {
                            if (TemporaryFile == null || !TemporaryFile.Exists() || reloadImageStream)
                            {
                                TemporaryFile = RequestTemporaryFile();
                            }

                            if (TemporaryFile != null && TemporaryFile.Exists())
                            {
                                if (!metaDataOnly)
                                {
                                    try
                                    {
                                        ImageStream = File.Open(TemporaryFile.FullPath, FileMode.Open, FileAccess.ReadWrite);
                                        Image = Image.FromStream(ImageStream);
                                        ImageLoaded = true;
                                    }
                                    catch (Exception e)
                                    {
                                        throw new PageException(this, "Failed to load image [" + Name + "] from local File [" + TemporaryFile.FileName + "]!", true, e);
                                    }
                                }
                                else
                                {
                                    Image imgMeta = null;
                                    try
                                    {
                                        imgMeta = Image.FromStream(stream: File.Open(TemporaryFile.FullPath, FileMode.Open, FileAccess.Read),
                                                                            useEmbeddedColorManagement: false,
                                                                            validateImageData: false);
                                        Format.W = imgMeta.Width;
                                        Format.H = imgMeta.Height;
                                        Format.DPI = imgMeta.VerticalResolution;
                                        Format.Format = imgMeta.RawFormat;
                                        Format.Update();
                                    }
                                    catch (Exception eim)
                                    {
                                        throw new PageException(this, "Error loading image properties [" + Name + "]! Invalid or corrupted image", true, eim);
                                    }
                                    finally
                                    {
                                        imgMeta?.Dispose();
                                    }
                                }
                            }
                            else
                            {
                                throw new PageException(this, "Failed to extract image [" + Name + "] from Archive!");
                            }
                        }
                    }
                } else
                {

                }

                if (Image != null)
                {
                    try
                    {
                        Format.Update(Image);
                        Closed = false;
                        ImageMetaDataLoaded = true;
                    } catch (Exception ie)
                    {
                        if (LocalFile != null)
                        {
                            Format = new PageImageFormat(LocalFile.FileExtension);
                        } else
                        {
                            Format = new PageImageFormat(TemporaryFile.FileExtension);
                        }
                        
                        throw new PageException(this, ie.Message);
                    } finally 
                    { 
                        
                    }
                }
                else
                {
                    if (!metaDataOnly)
                    {
                        throw new PageException(this, "Failed to load/extract image!");
                    }                  
                }
            }
        }


        public void FreeImage()
        {
            Image?.Dispose();

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
