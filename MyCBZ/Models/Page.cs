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
using Win_CBZ.Events;
using System.Xml;
using SharpCompress.Common;
using System.IO.Pipes;
using System.Threading.Tasks;
using System.Drawing.Drawing2D;
using System.Runtime.Versioning;
using System.Threading;
using Win_CBZ.Data;
using Win_CBZ.Handler;
using Win_CBZ.Tasks;
using static System.Windows.Forms.AxHost;


namespace Win_CBZ
{
    [SupportedOSPlatform("windows")]
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

        /**
         * <remarks>Deprecated</remarks>
         */
        public String LocalPath { get; set; }

        /**
         * <remarks>Deprecated</remarks>
         */
        public String TempPath { get; set; }    

        public bool Compressed { get; set; }

        public bool Changed { get; set; }

        public bool Deleted { get; set; }

        public bool ReadOnly { get; set; }

        public bool Selected { get; set; }

        public bool Invalidated { get; set; }

        public bool Renamed { get; set; }   

        public bool IsMemoryCopy { get; set; }

        public bool ImageInfoRequested { get; set; }

        public bool Closed { get; set; }

        public bool ThumbnailInvalidated { get; set; }

        public bool ThumbnailError { get; set; }

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
        
        public Page()
        {
            Id = Guid.NewGuid().ToString();
            ImageTask = new ImageTask(Id);
            ReadOnly = true;
            Format = new PageImageFormat();
        }

        /// <summary>
        /// Create a page from a local file on disk
        /// </summary>
        /// <param name="fileName">full path to image</param>
        /// <param name="workingDir">set projects working dir (temp folder)</param>
        /// <param name="mode">set id for new page</param>
        public Page(String fileName, String workingDir, FileAccess mode = FileAccess.Read)
        {
            LocalFile = new LocalFile(fileName);
            ImageFileInfo = new FileInfo(fileName);
            Format = new PageImageFormat(LocalFile.FileExtension);

            TemporaryFileId = RandomId.GetInstance().Make();

            WorkingDir = PathHelper.ResolvePath(workingDir);

            ReadOnly = mode == FileAccess.Read || ImageFileInfo.IsReadOnly;
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
                ////LocalFile = localFile;
                //WorkingDir = TemporaryFile.FilePath;
                if (ReadOnly)
                {
                    TemporaryFile = CreateLocalWorkingCopy();
                }
            }


            Filename = ImageFileInfo.Name;
            FileExtension = ImageFileInfo.Extension;
            //LocalPath = ImageFileInfo.Directory.FullName;               
            Name = ImageFileInfo.Name;
            LastModified = ImageFileInfo.LastWriteTime;
            Size = ImageFileInfo.Length;
            Id = Guid.NewGuid().ToString();
            ImageTask = new ImageTask(Id);
            
        }

        /// <summary>
        /// Create a page from a local file on disk
        /// </summary>
        /// <param name="localFile">localfile of source image</param>
        /// <param name="workingDir">set projects working dir (temp folder)</param>
        /// <param name="mode">set id for new page</param>
        public Page(LocalFile localFile, String workingDir, FileAccess mode = FileAccess.Read)
        {
            try {
                //Copy(localFile.FullPath, tempFileName.FullName);
                LocalFile = new LocalFile(localFile.FullPath);
                TemporaryFileId = RandomId.GetInstance().Make();

                WorkingDir = PathHelper.ResolvePath(workingDir);

                Format = new PageImageFormat(localFile.FileExtension);
                TemporaryFile = RequestTemporaryFile();
                ImageFileInfo = new FileInfo(TemporaryFile.FullPath);
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
                    ////LocalFile = localFile;
                    //WorkingDir = TemporaryFile.FilePath;
                    if (ReadOnly)
                    {
                        TemporaryFile = CreateLocalWorkingCopy();
                    }
                }

            } catch (Exception e) {
                throw new PageException(this, "Error creating new page from local file! ['"+e.Message+"']", true, e);
            }
          
            Filename = ImageFileInfo.FullName;
            FileExtension = localFile.FileExtension;
            //LocalPath = ImageFileInfo.Directory.FullName;
            Name = localFile.FileName;
            Size = ImageFileInfo.Length;
            Id = Guid.NewGuid().ToString();
            ImageTask = new ImageTask(Id);
            //Key = RandomId.GetInstance().Make();
            LastModified = localFile.LastModified;
        }

        /// <summary>
        /// Create a page from archive entry
        /// </summary>
        /// <param name="entry">the ziparchive entry</param>
        /// <param name="workingDir">set projects working dir (temp folder)</param>
        /// <param name="randomId">set id for new page</param>
        public Page(ZipArchiveEntry entry, String workingDir, String randomId = null)
        {
            CompressedEntry = entry;
            Compressed = true;

            Filename = entry.FullName;
            FileExtension = ExtractFileExtension(entry.Name);
            Name = entry.Name;
            Size = entry.Length;
            Hash = entry.Crc32.ToString("X");
            EntryName = entry.Name;
            LastModified = entry.LastWriteTime;
            Id = Guid.NewGuid().ToString();
            TemporaryFileId = randomId ?? RandomId.GetInstance().Make();
            WorkingDir = workingDir;
            ImageTask = new ImageTask(Id);
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
            ImageTask = new ImageTask(Id);
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
            ImageTask = new ImageTask(Id);
        }

        // <deprecated></deprecated>
        public Page(Page sourcePage, String RandomId, int ThumbWidth = 212, int ThumbHeight = 256)
        {
            WorkingDir = sourcePage.WorkingDir;
            Name = sourcePage.Name;
            EntryName = sourcePage.EntryName;
 
            Filename = sourcePage.Filename;
            //LocalPath = sourcePage.LocalPath;
            //ImageStream = sourcePage.ImageStream;
            LocalFile = sourcePage.LocalFile;
            Format = sourcePage.Format;
            ImageType = sourcePage.ImageType;

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
            Hash = sourcePage.Hash;
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

            if (ImageStreamMemoryCopy == null)
            {
                ImageStream = File.Open(TemporaryFile.FullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                ImageStreamMemoryCopy = new MemoryStream();              
                ImageStream.CopyTo(ImageStreamMemoryCopy);
                ImageStreamMemoryCopy.Position = 0;
                IsMemoryCopy = true;

                ImageStream?.Close();
                ImageStream = null;
            }

            ThumbnailInvalidated = sourcePage.ThumbnailInvalidated;
            Thumbnail = sourcePage.Thumbnail;
            if (Thumbnail == null)
            {
                ThumbnailInvalidated = true;
            }
            
            
            ImageTask = new ImageTask(Id);
        }

        /// <summary>
        /// Create a page from a source-page, copying all relevant attributes
        /// </summary>
        /// <param name="sourcePage">Source Page</param>
        /// <param name="inMemory">Image will be kept in Memory</param>
        /// <param name="newCopy">create a new file and id</param>
        public Page(Page sourcePage, bool inMemory = false, bool newCopy = false)
        {
            if (sourcePage != null)
            {
                WorkingDir = sourcePage.WorkingDir;
                Name = sourcePage.Name;
                EntryName = sourcePage.EntryName;
           
                Filename = sourcePage.Filename;
                FileExtension = sourcePage.FileExtension;
              
                LocalFile = sourcePage.LocalFile;
                
                Compressed = sourcePage.Compressed;
                TemporaryFileId = newCopy ? RandomId.GetInstance().Make() : sourcePage.TemporaryFileId;
                EntryName = sourcePage.EntryName;
                CompressedEntry = sourcePage.CompressedEntry;
                
                Format = sourcePage.Format;
                ImageType = sourcePage.ImageType;
                LastModified = sourcePage.LastModified;
                Hash = sourcePage.Hash;

                Changed = sourcePage.Changed;
                ReadOnly = sourcePage.ReadOnly;
                Size = sourcePage.Size;
                Id = newCopy ? Guid.NewGuid().ToString() : sourcePage.Id;
                Index = sourcePage.Index;
                OriginalIndex = sourcePage.OriginalIndex;
                Number = sourcePage.Number;
                //Closed = sourcePage.Closed;
                DoublePage = sourcePage.DoublePage;

                Deleted = sourcePage.Deleted;
                OriginalName = sourcePage.OriginalName;
                Key = sourcePage.Key;
                ThumbH = sourcePage.ThumbH;
                ThumbW = sourcePage.ThumbW;
                Thumbnail = sourcePage.Thumbnail;
                ThumbnailInvalidated = sourcePage.ThumbnailInvalidated;

                if (sourcePage.ImageStream != null && !sourcePage.IsMemoryCopy && !newCopy)
                {
                    if (sourcePage.ImageStream.CanRead)
                    {
                        //sourcePage.ImageStream.Position = 0;

                        if (inMemory)
                        {
                            sourcePage.ImageStream.Position = 0;
                            ImageStreamMemoryCopy = new MemoryStream();
                            sourcePage.ImageStream.CopyTo(ImageStreamMemoryCopy);
                            ImageStreamMemoryCopy.Position = 0;
                            IsMemoryCopy = true;
                        }
                    } else
                    {
                        sourcePage.FreeImage();

                        if (Image != null)
                        {
                            Image?.Dispose();
                            Image = null;
                        }

                        ImageStream?.Close();
                        ImageStream = null;
                    }
                } else
                {
                    if (sourcePage.IsMemoryCopy)
                    {
                        if (sourcePage.ImageStreamMemoryCopy.CanRead)
                        {
                            ImageStreamMemoryCopy = new MemoryStream();
                            sourcePage.ImageStreamMemoryCopy.Position = 0;
                            sourcePage.ImageStreamMemoryCopy.CopyTo(ImageStreamMemoryCopy);
                            ImageStreamMemoryCopy.Position = 0;
                            IsMemoryCopy = true;
                        } else
                        {
                            throw new PageException(this, "Failed to create new copy from page!\r\nUnable to read Memorystream!", true);
                        }                
                    }
                }

                if (ImageStream == null || newCopy)
                {
                    if (ImageStreamMemoryCopy == null || 
                        !ImageStreamMemoryCopy.CanRead || 
                        ImageStreamMemoryCopy.Length == 0 || 
                        newCopy)
                    { 
                        if (sourcePage.IsMemoryCopy && ImageStreamMemoryCopy == null)
                        {
                            IsMemoryCopy = false;
                        }

                        TemporaryFile = RequestTemporaryFile();

                        if (TemporaryFile != null)
                        {
                            if (ImageStream != null && ImageStream.CanRead)
                            {
                                ImageStream?.Close();
                                ImageStream?.Dispose();
                            }

                            try
                            {
                                IsMemoryCopy = false;
                                ImageStream = File.Open(TemporaryFile.FullPath, FileMode.Open, FileAccess.ReadWrite, FileShare.Read);
                                if (Compressed || inMemory)
                                {
                                    ImageStreamMemoryCopy = new MemoryStream();
                                    ImageStream.CopyTo(ImageStreamMemoryCopy);
                                    ImageStreamMemoryCopy.Position = 0;
                                    IsMemoryCopy = true;
                                }

                            }
                            catch (Exception ex)
                            {
                                throw new PageException(this, "Failed to create page from source ", true, ex);
                            }
                            finally
                            {
                                if (Compressed || inMemory)
                                {
                                    ImageStream?.Close();
                                    ImageStream?.Dispose();
                                    ImageStream = null;
                                }
                            }
                        }
                    }
                }

                ImageTask = sourcePage.ImageTask;
                ImageMetaDataLoaded = sourcePage.ImageMetaDataLoaded;
            } else
            {
                throw new PageException(this, "Failed to create page from source! [NULL]");
            }
        }


        /// <summary>
        /// Create a page from XML- Data
        /// </summary>
        /// <param name="inputStream">XML Stream</param>
        /// <param name="mode">Fileaccess mode</param>
        /// <returns></returns>
        public Page(Stream inputStream, FileAccess mode = FileAccess.Read)
        {
            XmlDocument Document = new XmlDocument();
                        
            XmlReader MetaDataReader = XmlReader.Create(inputStream);
            MetaDataReader.Read();
            Document.Load(MetaDataReader);

            String sourceProjectId = "";

            XmlNode Root;

            Format = new PageImageFormat(FileExtension);

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
                            case "format":
                                HandlePageMetaData(subNode, "Format");
                                break;
                            case "imagetask":
                                HandlePageMetaData(subNode, "ImageTask");
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

            //TemporaryFileId = RandomId.getInstance().make();
            Id = Guid.NewGuid().ToString();
            
            ImageLoaded = false;
            IsMemoryCopy = false;
            Compressed = false;

            //if (LocalFile == null)
            //{
                //if (Program.ProjectModel.ProjectGUID != sourceProjectId)
                //{
                    if (WorkingDir != null)
                    {
                        DirectoryInfo currentWorkingDir = new DirectoryInfo(WorkingDir);
                        String baseDir = currentWorkingDir.Parent.FullName;

                        String targetPath = Path.Combine(baseDir, sourceProjectId);
                        String targetFile = Path.Combine(targetPath, RandomId.GetInstance().Make() + FileExtension);

                        //String targetFile = Path.Combine(targetPath, TemporaryFileId + ".tmp");

                        if (File.Exists(targetFile))
                        {
                            targetFile = Path.Combine(targetPath, RandomId.GetInstance().Make() + ".000");
                        }

                        if (LocalFile != null && LocalFile.Exists())
                        {
                            Copy(LocalFile.FullPath, targetFile);
                            LocalFile = new LocalFile(targetFile);
                        } else
                        {
                            Copy(TemporaryFile.FullPath, targetFile);
                            LocalFile = new LocalFile(targetFile);
                        }

                        
                    }
            //} else
            //{
            //    LocalFile = new LocalFile(TemporaryFile.FullPath);
            //}

            //
            //}

            TemporaryFileId = RandomId.GetInstance().Make(); // new id

            if (LocalFile != null && LocalFile.Exists())
            {
                DirectoryInfo currentWorkingDir = new DirectoryInfo(WorkingDir);
                String baseDir = currentWorkingDir.Parent.FullName;

                String targetPath = Path.Combine(baseDir, sourceProjectId);
                String targetFile = Path.Combine(targetPath, TemporaryFileId + ".tmp");

                TemporaryFile = RequestTemporaryFile(targetFile, true);
            }

           
            ImageFileInfo = new FileInfo(TemporaryFile.FullPath);
            //
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

            if (IsMemoryCopy)
            {
                if (ImageStream.CanRead)
                {
                    ImageStream.Position = 0;
                    ImageStreamMemoryCopy = new MemoryStream();
                    ImageStream.CopyTo(ImageStreamMemoryCopy);
                    ImageStreamMemoryCopy.Position = 0;
                } else
                {
                    throw new PageException(this, "Failed to create MemoryCopy from Image.\nImageStream not readable!", true);
                }
            }
        }

        protected Point HandlePoint(XmlNode node)
        {
            int w = 0;
            int h = 0;

            foreach (XmlNode subNode in node.ChildNodes)
            {
                if (subNode.Name == "X")
                {
                    w = int.Parse(subNode.InnerText);
                }

                if (subNode.Name == "Y")
                {
                    h = int.Parse(subNode.InnerText);
                }

            }

            return new Point(w, h);
        }

        protected PageImageFormat HandleFormat(XmlNode node)
        {
            PageImageFormat parsedFormat = new PageImageFormat();
            foreach (XmlNode subNode in node.ChildNodes)
            {
                if (subNode.Name == "W")
                {
                    parsedFormat.W = int.Parse(subNode.InnerText);
                }

                if (subNode.Name == "H")
                {
                    parsedFormat.H = int.Parse(subNode.InnerText);
                }

                if (subNode.Name == "DPI")
                {
                    parsedFormat.DPI = float.Parse(subNode.InnerText);
                }

                if (subNode.Name == "Name")
                {
                    parsedFormat.Name = subNode.InnerText;
                }

                if (subNode.Name == "Format")
                {
                    parsedFormat.FormatFromGUID(subNode.InnerText);
                }
            }
            return parsedFormat;
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
            else if (type == "Format")
            {
                Format = HandleFormat(node);
            }
            else if (type == "ImageTask")
            {
                ImageTask = new ImageTask(Id);
                foreach (XmlNode subNode in node.ChildNodes)
                {
                    if (subNode.Name == "Tasks")
                    {
                        ImageTask.Tasks = new List<string>();
                        foreach (XmlNode subNode2 in subNode.ChildNodes)
                        {
                            if (subNode2.Name == "Task")
                            {
                                ImageTask.Tasks.Add(subNode2.InnerText);
                            }
                        }
                    }

                    if (subNode.Name == "TaskOrder")
                    {
                        foreach (XmlNode subNode2 in subNode.ChildNodes)
                        {
                            if (subNode2.Name == "Resize")
                            {
                                ImageTask.TaskOrder.Resize = Enum.Parse<ImageTaskOrderValue>(subNode2.InnerText);
                            }

                            if (subNode2.Name == "Rotate")
                            {
                                ImageTask.TaskOrder.Rotate = Enum.Parse<ImageTaskOrderValue>(subNode2.InnerText);
                            }

                            if (subNode2.Name == "Convert")
                            {
                                ImageTask.TaskOrder.Convert = Enum.Parse<ImageTaskOrderValue>(subNode2.InnerText);
                            }

                            if (subNode2.Name == "Split")
                            {
                                ImageTask.TaskOrder.Split = Enum.Parse<ImageTaskOrderValue>(subNode2.InnerText);
                            }
                        }
                    }

                    if (subNode.Name == "ImageAdjustments")
                    {
                        ImageTask.ImageAdjustments = new ImageAdjustments();
                        foreach (XmlNode subNode2 in subNode.ChildNodes)
                        {
                            if (subNode2.Name == "SplitPage")
                            {
                                ImageTask.ImageAdjustments.SplitPage = Boolean.Parse(subNode2.InnerText);
                            }

                            if (subNode2.Name == "SplitType")
                            {
                                ImageTask.ImageAdjustments.SplitType = int.Parse(subNode2.InnerText);
                            }

                            if (subNode2.Name == "SplitPageAt")
                            {
                                ImageTask.ImageAdjustments.SplitPageAt = int.Parse(subNode2.InnerText);
                            }

                            if (subNode2.Name == "DetectSplitAtColor")
                            {
                                ImageTask.ImageAdjustments.DetectSplitAtColor = HTMLColor.ToColor(subNode2.InnerText);
                            }

                            if (subNode2.Name == "ResizeToPageNumber")
                            {
                                ImageTask.ImageAdjustments.ResizeToPageNumber = int.Parse(subNode2.InnerText);
                            }

                            if (subNode2.Name == "ResizeMode")
                            {
                                ImageTask.ImageAdjustments.ResizeMode = int.Parse(subNode2.InnerText);
                            }

                            if (subNode2.Name == "ResizeTo")
                            {
                                ImageTask.ImageAdjustments.ResizeTo = HandlePoint(subNode2);
                            }

                            if (subNode2.Name == "MaxDimensions")
                            {
                                ImageTask.ImageAdjustments.MaxDimensions = HandlePoint(subNode2);
                            }

                            if (subNode2.Name == "KeepAspectRatio")
                            {
                                ImageTask.ImageAdjustments.KeepAspectRatio = bool.Parse(subNode2.InnerText);
                            }

                            if (subNode2.Name == "ConvertType")
                            {
                                ImageTask.ImageAdjustments.ConvertType = int.Parse(subNode2.InnerText);
                            }

                            if (subNode.Name == "ConvertFormat")
                            {
                                ImageTask.ImageAdjustments.ConvertFormat = HandleFormat(subNode);
                            }

                            if (subNode2.Name == "ResizeToPercentage")
                            {
                                ImageTask.ImageAdjustments.ResizeToPercentage = float.Parse(subNode2.InnerText);
                            }

                            if (subNode2.Name == "DontStretch")
                            {
                                ImageTask.ImageAdjustments.DontStretch = bool.Parse(subNode2.InnerText);
                            }

                            if (subNode2.Name == "Grayscale")
                            {
                                ImageTask.ImageAdjustments.Grayscale = bool.Parse(subNode2.InnerText);
                            }

                            if (subNode2.Name == "RotateMode")
                            {
                                ImageTask.ImageAdjustments.RotateMode = int.Parse(subNode2.InnerText);
                            }

                            if (subNode2.Name == "Interpolation")
                            {
                                ImageTask.ImageAdjustments.Interpolation = (InterpolationMode)Enum.Parse(typeof(InterpolationMode), subNode2.InnerText);
                            }

                            if (subNode2.Name == "SplitDoublePagesFirstResizingToPage")
                            {
                                ImageTask.ImageAdjustments.SplitDoublePagesFirstResizingToPage = bool.Parse(subNode2.InnerText);
                            }

                            if (subNode2.Name == "SplitOnlyDoublePages")
                            {
                                ImageTask.ImageAdjustments.SplitOnlyDoublePages = bool.Parse(subNode2.InnerText);
                            }

                            if (subNode2.Name == "IgnoreDoublePagesResizingToPage")
                            {
                                ImageTask.ImageAdjustments.IgnoreDoublePagesResizingToPage = bool.Parse(subNode2.InnerText);
                            }
                        }
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

                    //case "LocalPath":
                    //    LocalPath = node.InnerText;
                    //    break;

                    //case "TempPath":
                    //    TempPath = node.InnerText;
                    //    break;

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

        /// <summary>
        /// Update a pages attributes with those from specified page
        /// </summary>
        /// <param name="page">Page to update attributes from</param>
        /// <param name="skipIndex">dont update any indices</param>
        /// <param name="skipName">dont update name</param>
        public void UpdatePage(Page page, bool skipIndex = false, bool skipName = false)
        {
            Compressed = page.Compressed;
            Filename = page.Filename;
            
            EntryName = page.EntryName;
            Size = page.Size;
            Id = page.Id;
            Key = page.Key;
            
            Deleted = page.Deleted;
            LocalFile = page.LocalFile;
            TemporaryFileId = page.TemporaryFileId;
            Changed = page.Changed;
            ImageType = page.ImageType;
            ImageTask = page.ImageTask;
            Format = page.Format;
            DoublePage = page.DoublePage;
            Hash = page.Hash;
            LastModified = page.LastModified;

            

            TemporaryFile = page.TemporaryFile;

            if (!skipName)
            {
                Name = page.Name;
            }

            if (!skipIndex)
            {
                Index = page.Index;
                Number = page.Number;
            }
            //OriginalIndex = page.OriginalIndex;
        }

        /// <summary>
        /// Update a pages attributes with those from specified page
        /// </summary>
        /// <param name="page">Page to update attributes from</param>
        /// <param name="skipIndex">dont update any indices</param>
        /// <param name="skipName">dont update name</param>
        public void UpdatePageAttributes(Page page, bool skipIndex = false, bool skipName = false)
        {
            Compressed = page.Compressed;
            
            Filename = page.Filename;
            FileExtension = page.FileExtension;

            EntryName = page.EntryName;
            CompressedEntry = page.CompressedEntry;
            //Size = page.Size;
            Id = page.Id;
            Key = page.Key;

            Deleted = page.Deleted;
            Changed = page.Changed;
            ImageType = page.ImageType;
            ImageTask = page.ImageTask;
            Format = page.Format;
            DoublePage = page.DoublePage;
            Hash = page.Hash;
            LastModified = page.LastModified;

            if (!skipName)
            {
                Name = page.Name;
                OriginalName = page.OriginalName;
            }

            if (!skipIndex)
            {
                Index = page.Index;
                Number = page.Number;
            }
            //OriginalIndex = page.OriginalIndex;
        }

        /// <summary>
        /// Update the compressed entry for given page
        /// </summary>
        /// <param name="entry">Archive entry to update image from</param>
        /// <param name="randomId">set a given random id</param>
        public void UpdateImageEntry(ZipArchiveEntry entry, String randomId)
        {
            CompressedEntry = entry;
            Compressed = true;

            Filename = entry.FullName;
            Name = entry.Name;
            EntryName = entry.Name;
            //Size = entry.Length;
            LastModified = entry.LastWriteTime;
            //Id = Guid.NewGuid().ToString();    // dont create new ID
            TemporaryFileId = randomId;
            Hash = entry.Crc32.ToString("X");
        }

        /// <summary>
        /// Update the local working-copy (temporary file) for given page
        /// </summary>
        /// <param name="localFile">Localfile info for source imagefile</param>
        /// <param name="tempFileName">Targetfile info</param>
        public void UpdateLocalWorkingCopy(LocalFile localFile, FileInfo tempFileName = null)
        {
            Copy(localFile.FullPath, tempFileName.FullName);

            ImageFileInfo = new FileInfo(tempFileName.FullName);

            LocalFile = new LocalFile(localFile.FullPath);
            TemporaryFile = new LocalFile(tempFileName.FullName);
            Size = ImageFileInfo.Length;
            //LocalPath = localFile.FullPath;
            Compressed = false;
            LastModified = localFile.LastModified;
            Name = localFile.FileName;

            Changed = true;

            //Key = RandomId.getInstance().make();

            //TemporaryFileId = RandomId.getInstance().make();
            Format = new PageImageFormat();
            Image = null;
            ImageLoaded = false;
            ImageInfoRequested = false;
            ThumbnailInvalidated = true;

            ImageStream?.Close();
            ImageStream?.Dispose();

            ThumbnailInvalidated = true;
            //Key = RandomId.getInstance().make();

            //String newTempFileName = CreateLocalWorkingCopy(ExtractFileExtension(localFile.FullPath));
            //TempPath = new FileInfo(newTempFileName).FullName;
        }

        /// <summary>
        /// Update the local souce for given page
        /// </summary>
        /// <param name="newLocalFile">Localfile info for new file</param>
        public void UpdateLocalFileWith(LocalFile newLocalFile)
        {
            Compressed = false;

            if (newLocalFile.Exists())
            {
                if (LocalFile == null || !LocalFile.Exists())
                {
                    LocalFile = new LocalFile(Path.Combine(WorkingDir, RandomId.GetInstance().Make() + FileExtension));
                }

                Copy(newLocalFile.FullPath, LocalFile.FullPath);

                LocalFile.Refresh();
            }
        }

        /// <summary>
        /// Update the local working-copy (temporary file) for given page
        /// </summary>
        /// <param name="newTempFile">Localfile info for new temporary file</param>
        public void UpdateTemporaryFile(LocalFile newTempFile)
        {
            Close();

            if (TemporaryFile == null)
            {
               TemporaryFile = RequestTemporaryFile();
            }

            Copy(newTempFile.FullPath, TemporaryFile.FullPath);

            TemporaryFile.Refresh();

            if (TemporaryFile.Exists())
            {
                Closed = false;
            }

            //ImageLoaded = false;
            //ThumbnailInvalidated = true;
            //ImageInfoRequested = false;

            //Image = null;
        }

        /// <summary>
        /// Update the image stream for given page
        /// </summary>
        /// <param name="updateStream">Sourc- Imagestream</param>
        /// <param name="newTempFile">Localfile info for new temporary file</param>
        public void UpdateImage(Stream updateStream, LocalFile newImageFile = null)
        {
            if (updateStream != null && updateStream.CanRead)
            {
                FileStream newImageFileStream = null;
                try
                {
                    if (newImageFile != null) 
                    { 
                        newImageFileStream = File.Open(newImageFile.FullPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                        updateStream.CopyTo(newImageFileStream);
                    }

                    Stream fs = TemporaryFile.LocalFileInfo.OpenWrite();

                    updateStream.CopyTo(fs);

                    fs.Close();

                    updateStream.Position = 0;

                    updateStream.CopyTo(ImageStream);

                    ImageStream.Position = 0;

                    TemporaryFile.Refresh();

                } catch (FileNotFoundException fe)
                {
                    throw new PageException(this, "Failed to update page [" + Name + "] from source.", false, fe);
                } catch (IOException ioe) 
                {
                    throw new PageException(this, "Failed to update page [" + Name + "] from source stream!", false, ioe);
                } finally 
                {
                    newImageFileStream?.Close();
                    newImageFileStream?.Dispose();
                    //imageStream?.Close(); 
                }


                //ImageFileInfo = new FileInfo(newImageFile.FullPath);

                //LocalFile = new LocalFile(newImageFile.FullPath);
                //TemporaryFile = null;
                Size = TemporaryFile.FileSize;
                
                //Compressed = false;
                Changed = true;
                LastModified = TemporaryFile.LastModified;
                
                //Key = RandomId.getInstance().make();

                //TemporaryFileId = RandomId.getInstance().make();
                Format = new PageImageFormat();
                Image = null;
                ImageLoaded = false;
                ImageMetaDataLoaded = false;
                ImageInfoRequested = false;
                
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

        public static string ExtractFileExtension(String fileName)
        {
            return Path.GetExtension(fileName);

            /*
            string[] entryExtensionParts = fileName.Split('.');

            if (entryExtensionParts.Length == 0) return null;

            else return "." + entryExtensionParts.Last<string>();
            */
        }

        public static bool NameEqualsIndex(Page page)
        {
            try
            {
                String name = page.Name.Replace(page.FileExtension, "");
                int pageNumber = 0;

                var isNummeric = int.TryParse(name, out pageNumber);
                return pageNumber.Equals(page.Number);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void UpdateImageExtension(string newExtension)
        {
            Name = Path.ChangeExtension(Name, newExtension);
            Filename = Path.ChangeExtension(Name, newExtension);

            /*
            string[] entryExtensionParts = Name.Split('.');

            if (entryExtensionParts.Length == 0) return;

            else {
                entryExtensionParts[entryExtensionParts.Length - 1] = "." + newExtension;
            }

            Name = String.Join(null, entryExtensionParts);

            entryExtensionParts = Filename.Split('.');

            if (entryExtensionParts.Length == 0) return;

            else
            {
                entryExtensionParts[entryExtensionParts.Length - 1] = "." + newExtension;
            }

            Filename = String.Join(null, entryExtensionParts);
            */
            FileExtension = ExtractFileExtension(Filename);
        }

        public string NameWithoutExtension()
        {
            return Name.Replace(ExtractFileExtension(Name), "");
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

        /// <summary>
        /// Serialize the page to XML- Fragment
        /// </summary>
        /// <param name="sourceProjectId">Source- Project ID</param>
        /// <param name="newCopy">create a new copy from given page</param>
        /// <param name="withoutXMLHeaderTag">if TRUE, creates only an XML- Fragment</param>
        public MemoryStream Serialize(String sourceProjectId, bool newCopy = false, bool withoutXMLHeaderTag = false)
        {
            MemoryStream ms = new MemoryStream();
            bool MemoryCopyState = IsMemoryCopy;
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
            
            xmlWriter.WriteElementString("Filename", Filename);
            xmlWriter.WriteElementString("OriginalName", OriginalName);
            xmlWriter.WriteElementString("Number", Number.ToString());
            xmlWriter.WriteElementString("Index", Index.ToString());
            xmlWriter.WriteElementString("OriginalIndex", OriginalIndex.ToString());
            xmlWriter.WriteElementString("EntryName", EntryName);
            
            xmlWriter.WriteElementString("ImageType", ImageType);
            xmlWriter.WriteElementString("DoublePage", DoublePage.ToString());
            xmlWriter.WriteElementString("Size", Size.ToString());
            xmlWriter.WriteElementString("WorkingDir", WorkingDir);
            

            if (LocalFile != null)
            {
                // LocalFile
                xmlWriter.WriteStartElement("LocalFile");
                xmlWriter.WriteElementString("FullPath", LocalFile.FullPath);
                xmlWriter.WriteElementString("FileSize", LocalFile.FileSize.ToString());
                xmlWriter.WriteElementString("FilePath", LocalFile.FilePath);
                xmlWriter.WriteElementString("Exists", LocalFile.Exists().ToString());

                xmlWriter.WriteEndElement();
            }

            if (Compressed && newCopy)
            {
                FileInfo NewTemporaryFileName = Program.ProjectModel.MakeNewTempFileName();
                TemporaryFile = CreateLocalWorkingCopy(NewTemporaryFileName.FullName);

                xmlWriter.WriteElementString("TemporaryFileId", NewTemporaryFileName.Name.Replace(TemporaryFile.FileExtension, ""));
            } else
            {
                xmlWriter.WriteElementString("TemporaryFileId", TemporaryFileId);
            }

            //
            if (newCopy && Compressed && TemporaryFile == null)
            {
                TemporaryFile = RequestTemporaryFile();
            }


            if (TemporaryFile != null)
            {
                // TemporaryFile
                xmlWriter.WriteStartElement("TemporaryFile");
                xmlWriter.WriteElementString("FullPath", TemporaryFile.FullPath);
                xmlWriter.WriteElementString("FileSize", TemporaryFile.FileSize.ToString());
                xmlWriter.WriteElementString("FilePath", TemporaryFile.FilePath);
                xmlWriter.WriteElementString("Exists", TemporaryFile.Exists().ToString());

                xmlWriter.WriteEndElement();
            }

            //
            if (Format != null)
            {
                xmlWriter.WriteStartElement("Format");
                xmlWriter.WriteElementString("W", Format.W.ToString());
                xmlWriter.WriteElementString("H", Format.H.ToString());
                xmlWriter.WriteElementString("DPI", Format.DPI.ToString());
                xmlWriter.WriteElementString("Format", Format.Format.Guid.ToString());
                xmlWriter.WriteElementString("Name", Format.Name.ToString());
                xmlWriter.WriteElementString("PixelFormat", Format.PixelFormat.ToString());

                xmlWriter.WriteEndElement();
            }

            //
            if (ImageTask != null)
            {
                xmlWriter.WriteStartElement("ImageTask");
                xmlWriter.WriteElementString("TaskCount", ImageTask.TaskCount().ToString());
                xmlWriter.WriteStartElement("Tasks");

                foreach (String task in ImageTask.Tasks)
                {
                    xmlWriter.WriteElementString("Task", task.ToString());
                }

                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("TaskOrder");
                xmlWriter.WriteElementString("Convert", ImageTask.TaskOrder.Convert.ToString());
                xmlWriter.WriteElementString("Resize", ImageTask.TaskOrder.Resize.ToString());
                xmlWriter.WriteElementString("Rotate", ImageTask.TaskOrder.Rotate.ToString());
                xmlWriter.WriteElementString("Split", ImageTask.TaskOrder.Split.ToString());


                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("ImageAdjustments");
                xmlWriter.WriteElementString("ResizeMode", ImageTask.ImageAdjustments.ResizeMode.ToString());
                xmlWriter.WriteElementString("ResizeToPageNumber", ImageTask.ImageAdjustments.ResizeToPageNumber.ToString());
                xmlWriter.WriteElementString("SplitPage", ImageTask.ImageAdjustments.SplitPage.ToString());
                xmlWriter.WriteElementString("SplitType", ImageTask.ImageAdjustments.SplitType.ToString());
                xmlWriter.WriteElementString("SplitPageAt", ImageTask.ImageAdjustments.SplitPageAt.ToString());
                xmlWriter.WriteElementString("DetectSplitAtColor", HTMLColor.ToHexColor(ImageTask.ImageAdjustments.DetectSplitAtColor));
                xmlWriter.WriteElementString("KeepAspectRatio", ImageTask.ImageAdjustments.KeepAspectRatio.ToString());
                xmlWriter.WriteElementString("ResizeToPercentage", ImageTask.ImageAdjustments.ResizeToPercentage.ToString());

                xmlWriter.WriteElementString("SplitDoublePagesFirstResizingToPage", ImageTask.ImageAdjustments.SplitDoublePagesFirstResizingToPage.ToString());
                xmlWriter.WriteElementString("SplitOnlyDoublePages", ImageTask.ImageAdjustments.SplitOnlyDoublePages.ToString());
                xmlWriter.WriteElementString("IgnoreDoublePagesResizingToPage", ImageTask.ImageAdjustments.IgnoreDoublePagesResizingToPage.ToString());

                xmlWriter.WriteElementString("DontStretch", ImageTask.ImageAdjustments.DontStretch.ToString());
                xmlWriter.WriteElementString("Grayscale", ImageTask.ImageAdjustments.Grayscale.ToString());
                xmlWriter.WriteElementString("RotateMode", ImageTask.ImageAdjustments.RotateMode.ToString());
                xmlWriter.WriteElementString("Interpolation", ImageTask.ImageAdjustments.Interpolation.ToString());

                xmlWriter.WriteStartElement("ResizeTo");
                xmlWriter.WriteElementString("X", ImageTask.ImageAdjustments.ResizeTo.X.ToString());
                xmlWriter.WriteElementString("Y", ImageTask.ImageAdjustments.ResizeTo.Y.ToString());
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("MaxDimensions");
                xmlWriter.WriteElementString("X", ImageTask.ImageAdjustments.MaxDimensions.X.ToString());
                xmlWriter.WriteElementString("Y", ImageTask.ImageAdjustments.MaxDimensions.Y.ToString());
                xmlWriter.WriteEndElement();

                xmlWriter.WriteElementString("ConvertType", ImageTask.ImageAdjustments.ConvertType.ToString());

                if (ImageTask.ImageAdjustments.ConvertFormat != null)
                {
                    xmlWriter.WriteStartElement("ConvertFormat");
                    xmlWriter.WriteElementString("W", ImageTask.ImageAdjustments.ConvertFormat.W.ToString());
                    xmlWriter.WriteElementString("H", ImageTask.ImageAdjustments.ConvertFormat.H.ToString());
                    xmlWriter.WriteElementString("DPI", ImageTask.ImageAdjustments.ConvertFormat.DPI.ToString());
                    xmlWriter.WriteElementString("Format", ImageTask.ImageAdjustments.ConvertFormat.Format.Guid.ToString());
                    xmlWriter.WriteElementString("Name", ImageTask.ImageAdjustments.ConvertFormat.Name.ToString());
                    xmlWriter.WriteElementString("PixelFormat", ImageTask.ImageAdjustments.ConvertFormat.PixelFormat.ToString());

                    xmlWriter.WriteEndElement();
                }

                xmlWriter.WriteEndElement();

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
            //xmlWriter.WriteElementString("LocalPath", LocalPath);
            //xmlWriter.WriteElementString("TempPath", TempPath);

            xmlWriter.WriteElementString("Compressed", Compressed.ToString());
            xmlWriter.WriteElementString("Changed", Changed.ToString());
            xmlWriter.WriteElementString("Deleted", Deleted.ToString());
            xmlWriter.WriteElementString("ReadOnly", ReadOnly.ToString());
            xmlWriter.WriteElementString("Selected", Selected.ToString());
            xmlWriter.WriteElementString("Invalidated", Invalidated.ToString());
            xmlWriter.WriteElementString("IsMemoryCopy", MemoryCopyState.ToString());
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

        public void Reload()
        {
            FreeImage();
            DeleteTemporaryFile();
            TemporaryFileId = RandomId.GetInstance().Make();
            TemporaryFile = RequestTemporaryFile();
            LoadImageInfo();
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Close(bool keepTemporaryFiles = true)
        {
            FreeImage();

            if (ImageStream != null)
            {
                ImageStream.Close();
                ImageStream.Dispose();
            }

            if (ImageStreamMemoryCopy != null)
            {
                ImageStreamMemoryCopy.Close();
                ImageStreamMemoryCopy.Dispose();
            }

            if (!keepTemporaryFiles)
            {
                DeleteTemporaryFile();
            }

            Closed = true;
            Invalidated = false;
            ImageInfoRequested = false;
            ImageFileInfo = null;           
        }

        public void FreeCompressedEntry()
        {
            if (Compressed || CompressedEntry != null)
            {
                CompressedEntry = null;
                //Compressed = false;  // dont reset page compressed state!
            }
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
                if (CompressedEntry != null && EntryName == CompressedEntry.Name)
                {
                    if ((TemporaryFile == null || 
                        !TemporaryFile.Exists()) || 
                        destination != null || 
                        overwrite)
                    {
                        if (destination == null)
                        {
                            destination = Path.Combine(PathHelper.ResolvePath(WorkingDir), TemporaryFileId + ".tmp");
                        }

                        LocalFile destinationFile = new LocalFile(destination);

                        if (!destinationFile.Exists() || overwrite)
                        {
                            try { 
                                CompressedEntry.ExtractToFile(destination, overwrite);
                            } catch (Exception e)
                            {
                                throw new PageException(this, e.Message, true, e);
                            }
                        } else
                        {
                            if (destinationFile.FileSize == 0)
                            {
                                try
                                {
                                    CompressedEntry.ExtractToFile(destination, true);
                                }
                                catch (Exception e)
                                {
                                    throw new PageException(this, e.Message, true, e);
                                }
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
                    MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_WARNING, "No Entry with name [" + EntryName + "] exists in archive!");
                }
            } else
            {
                if (ReadOnly || overwrite || TemporaryFile == null || !TemporaryFile.Exists())
                {
                    if (TemporaryFile == null || !TemporaryFile.Exists() || destination != null || overwrite)
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
                        FileStream CopyImageStream = null;
                        FileStream localFile = File.OpenRead(LocalFile.FullPath);
                        try
                        {

                            CopyImageStream = File.Open(result, FileMode.OpenOrCreate, FileAccess.ReadWrite);
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
                                CopyImageStream?.Close();
                                CopyImageStream?.Dispose();
                            }
                        }
                        catch (Exception e)
                        {
                            throw new PageException(this, e.Message, true, e);
                        }
                        finally
                        {
                            localFile?.Close();
                            localFile?.Dispose();

                            CopyImageStream?.Close();
                            CopyImageStream?.Dispose();
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
                                CopyImageStream?.Close();
                                CopyImageStream?.Dispose();
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
            TokenStore.GetInstance().ResetCancellationToken(TokenStore.TOKEN_SOURCE_GLOBAL);

            TaskResult result = null;
            Task<TaskResult> copyFile = CopyFileTask.CopyFile(new LocalFile(inputFilePath), new LocalFile(outputFilePath), null, TokenStore.GetInstance().CancellationTokenSourceForName(TokenStore.TOKEN_SOURCE_GLOBAL).Token);

            copyFile.Start();
            copyFile.Wait(TokenStore.GetInstance().CancellationTokenSourceForName(TokenStore.TOKEN_SOURCE_GLOBAL).Token); // run synchronously and wait for completion
        
            result = copyFile.Result;
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
                    ImageStream?.Close();
                    ImageStream?.Dispose();
                }

                try
                {
                    File.Delete(TemporaryFile.FullPath);
                    TemporaryFile.Refresh();
                } catch (Exception e)
                {
                    throw new PageException(this, "Unable to delete temporary files [" + TemporaryFile.FullPath + "] from disk!", true, e);
                }
            }
        }

        public void DeleteLocalFile()
        {
            if (!ReadOnly)
            {
                File.Delete(LocalFile.FullPath);
                LocalFile.Refresh();
            } else
            {
               throw new PageException(this, "Unable to delete local file [" + LocalFile.FullPath + "] from disk! File is read-only!", true);
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

            if (!IsMemoryCopy)
            {
                if ((ImageStream == null) || (!ImageStream.CanRead)) 
                {
                    throw new PageException(this, "Imagestream is not readable.", false);
                }

                return ImageStream;
            } else
            {
                if ((ImageStreamMemoryCopy == null) || (!ImageStreamMemoryCopy.CanRead))
                {
                    throw new PageException(this, "Image- memorystream is not readable.", false);
                }

                return ImageStreamMemoryCopy;
            }
            
        }

        //[MethodImpl(MethodImplOptions.Synchronized)]
        public void LoadImageInfo(bool force = false)
        {
            if ((!Closed && Format.W == 0 && Format.H == 0 && !ImageInfoRequested) || force)
            {
                ImageInfoRequested = true;
                if (ImageStream == null || !ImageStream.CanRead)
                {
                    if (TemporaryFile != null)
                    {
                        Stream ImageFileStream = null;
                        try
                        {
                            ImageFileStream = File.Open(TemporaryFile.FullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                            Image ImageInfo = Image.FromStream(stream: ImageFileStream,
                                                     useEmbeddedColorManagement: false,
                                                     validateImageData: false);

                            Format = new PageImageFormat(ImageInfo);
                            ImageMetaDataLoaded = true;
                            
                        } catch ( Exception e)
                        {
                            throw new PageException(this, e.Message, true, e);
                        } finally
                        {
                            ImageFileStream?.Close();
                            ImageFileStream?.Dispose();
                            ImageInfo?.Dispose();
                            ImageInfo = null;
                        }
                    }

                    if (Compressed && (TemporaryFile == null || !TemporaryFile.Exists()))
                    {
                        ImageInfo = Image.FromStream(CompressedEntry.Open());
                        Format = new PageImageFormat(ImageInfo);

                        ImageMetaDataLoaded = true;

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

                        ImageMetaDataLoaded = true;

                        ImageInfo?.Dispose();
                        ImageInfo = null;
                    } catch (Exception e) {
                        MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_WARNING, "Unable to read image [" + Filename + "]");

                        throw new PageException(this, e.Message, true, e);
                    }
                }
            }
        }

        public Image GetThumbnail(int w = 0, int h = 0)
        {
            if (!Closed)
            {
                LoadImage();
            }

            if (w == 0)
            {
                w = ThumbW;
            }

            if (h == 0)
            {
                h = ThumbH;
            }

            if (Image != null)
            {
                ThumbnailError = false;
                try
                {
                    var newBitmap = new Bitmap(w, h);
                    using (Graphics g = Graphics.FromImage(newBitmap))
                    {
                        g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                        g.CompositingQuality = CompositingQuality.HighQuality;
                        g.SmoothingMode = SmoothingMode.HighQuality;
                        g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        g.DrawImage(Image, new Rectangle(0, 0, w, h));
                    }
                    Thumbnail = Image.FromHbitmap(newBitmap.GetHbitmap());
                    //openBitmap.Dispose(); //Clear The Old Large Bitmap From Memory
                }
                catch (Exception et)
                {
                    ThumbnailError = true;
                    throw new PageException(this, et.Message, true, et);
                } finally 
                {
                    Image?.Dispose();
                    Image = null;
                }
            }

            return Thumbnail;
        }

        public Bitmap GetThumbnailBitmap(int w = 0, int h = 0)
        {
            if (!Closed)
            {
                LoadImage();
            }

            if (w == 0) {
                w = ThumbW;
            }

            if (h == 0)
            {
                h = ThumbH;
            }

            if (Image != null)
            {
                ThumbnailError = false;
                try
                {
                    var newBitmap = new Bitmap(w, h);
                    using (Graphics g = Graphics.FromImage(newBitmap))
                    {
                        g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                        g.CompositingQuality = CompositingQuality.HighQuality;
                        g.SmoothingMode = SmoothingMode.HighQuality;
                        g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        g.DrawImage(Image, new Rectangle(0, 0, w, h));
                    }
                    
                    return newBitmap;
                    //openBitmap.Dispose(); //Clear The Old Large Bitmap From Memory
                }
                catch (Exception et)
                {
                    ThumbnailError = true;
                    
                    throw new PageException(this, et.Message, true, et);
                }
                finally
                {
                    //Image?.Dispose();
                    //Image = null;
                }
            }

            return null;
        }

        /*
         * This will use Windows api fetching cached explorer thumbs
         */
        public Image GetWindowsThumbnail(Image.GetThumbnailImageAbort callback, IntPtr callbackData)
        {
            if (!Closed)
            {
                LoadImage();
            }

            if (Image != null)
            {
                ThumbnailError = false;
                try
                {
                    Thumbnail = Image.GetThumbnailImage(ThumbW, ThumbH, callback, callbackData);

                    Image?.Dispose();
                    Image = null;
                } catch (Exception ex)
                {
                    ThumbnailError = true;
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

                    if (Path.Equals(destination, TemporaryFile.FullPath))
                    {
                        if (TemporaryFile.Exists())
                        {
                            return TemporaryFile;
                        }
                    }

                    try
                    {
                        destinationStream = File.Create(destination);
                        
                        if (ImageStream != null && ImageStream.CanRead)
                        {
                            localCopyStream = ImageStream;
                            
                        } else
                        {
                            localCopyStream = copyFileInfo.Open(FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
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
                            localCopyStream?.Close();
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
                        if (ImageStreamMemoryCopy != null && ImageStreamMemoryCopy.CanRead && ImageStreamMemoryCopy.Length > 0)
                        {
                            try
                            {
                                if (!metaDataOnly)
                                {
                                    Image = Image.FromStream(ImageStreamMemoryCopy);
                                    Format.W = Image.Width;
                                    Format.H = Image.Height;
                                    Format.DPI = Image.VerticalResolution;
                                    Format.Format = Image.RawFormat;
                                    Format.Update();
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
                        } else
                        {
                            throw new PageMemoryIOException(this, "Error loading image [" + Name + "] from System-Memory! Invalid or corrupted image", true, true);
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
                                    Stream MetaStream = null;
                                    try
                                    {
                                        MetaStream = File.Open(TemporaryFile.FullPath, FileMode.Open, FileAccess.Read);
                                        imgMeta = Image.FromStream(stream: MetaStream,
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
                                        MetaStream?.Close();
                                        MetaStream?.Dispose();
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
            Image = null;

            Thumbnail?.Dispose();
            Thumbnail = null;

            ImageInfo?.Dispose();
            ImageInfo = null;

            ImageMetaDataLoaded = false;

            ImageLoaded = false;

            if (ImageStreamMemoryCopy != null)
            {
                ImageStreamMemoryCopy.Close();
                ImageStreamMemoryCopy.Dispose();
                ImageStreamMemoryCopy = null;

                IsMemoryCopy = false;
            }

            Invalidated = true;
        }

        public void FreeStreams()
        {
            if (TemporaryFile != null)
            {
                if (ImageStream != null)
                {
                    ImageStream.Close();
                    ImageStream.Dispose();
                    ImageStream = null;
                }
            }

            if (ImageStreamMemoryCopy != null)
            {
                ImageStreamMemoryCopy.Close();
                ImageStreamMemoryCopy.Dispose();
                ImageStreamMemoryCopy = null;

                IsMemoryCopy = false;
            }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
