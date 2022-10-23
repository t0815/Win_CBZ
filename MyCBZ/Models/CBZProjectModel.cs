using MyCBZ;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CBZMage
{
    internal class CBZProjectModel
    {
        public String Name { get; set; }

        public String FileName { get; set; }

        public long FileSize { get; set; } = 0;

        public String Description { get; set; }

        public String WorkingDir { get; set; }

        protected String ProjectGUID { get; set; }

        protected Boolean IsNew = false;

        protected Boolean IsSaved = false;

        protected Boolean IsChanged = false;

        protected Boolean IsClosed = false;

        public BindingList<CBZImage> Pages { get; set; }

        public CBZMetaData MetaData { get; set; }

        public long TotalSize { get; set; }

        private System.IO.Compression.ZipArchive Archive { get; set; }

        private ZipArchiveMode Mode;

        public event EventHandler<CBZProjectModel> ArchiveChanged;

        public event EventHandler<ItemChangedEvent> ItemChanged;

        public event EventHandler<ItemLoadProgressEvent> ImageProgress;

        public event EventHandler<ItemFailedEvent> ItemFailed;

        public event EventHandler<CBZArchiveStatusEvent> ArchiveStatusChanged;

        public event EventHandler<MetaDataLoadEvent> MetaDataLoaded;

        public event EventHandler<OperationFinishedEvent> OperationFinished;

        public event EventHandler<LogMessageEvent> LogMessage;

        public event EventHandler<ItemExtractedEvent> ItemExtracted;

        private Thread LoadArchiveThread;

        private Thread ExtractArchiveThread;

        private Thread CloseArchiveThread;

       


        public CBZProjectModel(String workingDir)
        {
            WorkingDir = workingDir;
            Pages = new BindingList<CBZImage>();
            MetaData = new CBZMetaData(true);          
        }


        public Task New()
        {
            CloseArchiveThread = Close();

            return Task.Factory.StartNew(() =>
            {
                if (LoadArchiveThread != null)
                {
                    if (LoadArchiveThread.IsAlive)
                    {
                        LoadArchiveThread.Abort();
                    }
                }

                if (CloseArchiveThread != null)
                {
                    while (CloseArchiveThread.IsAlive)
                    {
                        System.Threading.Thread.Sleep(100);
                    }
                }

                Pages.Clear();
                MetaData.Free();

                ProjectGUID = Guid.NewGuid().ToString();

                if (!Directory.Exists(PathHelper.ResolvePath(WorkingDir) + ProjectGUID))
                {
                    DirectoryInfo di = Directory.CreateDirectory(PathHelper.ResolvePath(WorkingDir) + ProjectGUID);
                }
            });
        }


        public Thread Open(String path, ZipArchiveMode mode)
        {
            FileName = path;
            Mode = mode;
            
            if (LoadArchiveThread != null)
            {
                if (LoadArchiveThread.IsAlive)
                {
                    LoadArchiveThread.Abort();
                }
            }

            LoadArchiveThread = new Thread(new ThreadStart(OpenArchiveProc));
            LoadArchiveThread.Start();

            return LoadArchiveThread;
        }


        public Thread Extract()
        {

            if (LoadArchiveThread != null)
            {
                if (LoadArchiveThread.IsAlive)
                {
                    LoadArchiveThread.Abort();
                }
            }

            if (CloseArchiveThread != null)
            {
                if (CloseArchiveThread.IsAlive)
                {
                    CloseArchiveThread.Abort();
                }
            }

            ExtractArchiveThread = new Thread(new ThreadStart(ExtractArchiveProc));
            ExtractArchiveThread.Start();

            return ExtractArchiveThread;
        }

        public void AddImages(List<CBZLocalFile> fileList, int maxIndex = 0)
        {
            int index = maxIndex;

            foreach (CBZLocalFile fileObject in fileList)
            {
                try
                {
                    fileObject.FileInfo = fileObject.FileInfo.CopyTo(PathHelper.ResolvePath(WorkingDir) + ProjectGUID + "\\" + fileObject.FileInfo.Name);

                    CBZImage cBZImage = new CBZImage(fileObject.FileInfo);
                    cBZImage.Size = fileObject.FileSize;
                    cBZImage.Number = index + 1;
                    cBZImage.Index = index;
                    cBZImage.Filename = fileObject.FullPath;
                    cBZImage.Compressed = false;
                    cBZImage.LastModified = fileObject.FileInfo.LastWriteTime;

                    Pages.Add(cBZImage);

                    OnImageLoaded(new ItemLoadProgressEvent(index, Pages.Count, cBZImage));

                    index++;
                } catch (Exception ef)
                {
                    OnLogMessage(new LogMessageEvent(LogMessageEvent.LOGMESSAGE_TYPE_ERROR, ef.Message));
                }
            }

            OnOperationFinished(new OperationFinishedEvent(index, Pages.Count));
        }

        public List<CBZLocalFile> LoadDirectory(String path)
        {
            List<CBZLocalFile> files = new List<CBZLocalFile>();

            DirectoryInfo di = new DirectoryInfo(path);

            try
            {
                foreach (var fi in di.EnumerateFiles())
                {
                    CBZLocalFile localFile = new CBZLocalFile(fi.FullName);
                    localFile.FilePath = di.FullName;
                    localFile.FileName = fi.FullName;
                    localFile.FileSize = fi.Length;

                    files.Add(localFile);
                }
            } catch (Exception ex)
            {
                OnLogMessage(new LogMessageEvent(LogMessageEvent.LOGMESSAGE_TYPE_ERROR, ex.Message));
            } finally
            {
                
            }

            return files;
        }

        public List<CBZLocalFile> ParseFiles(List<String> files)
        {
            List<CBZLocalFile> filesObjects = new List<CBZLocalFile>();

            try
            {
                foreach (String fname in files)
                {
                    var fi = new FileInfo(fname);
                     
                    CBZLocalFile localFile = new CBZLocalFile(fi.FullName);
                    localFile.FilePath = fi.Directory.FullName;
                    localFile.FileName = fi.Name;
                    localFile.FileSize = fi.Length;
                    localFile.FileInfo = fi;

                    //fi.SetAccessControl()

                    filesObjects.Add(localFile);
                }

            } catch (Exception ex)
            {
                OnLogMessage(new LogMessageEvent(LogMessageEvent.LOGMESSAGE_TYPE_ERROR, ex.Message));
            } finally
            {
                //
            }

            return filesObjects;
        }

        public Thread Close()
        {
            if (LoadArchiveThread != null)
            {
                if (LoadArchiveThread.IsAlive)
                {
                    LoadArchiveThread.Abort();
                }
            }

            CloseArchiveThread = new Thread(new ThreadStart(CloseArchiveProc));
            CloseArchiveThread.Start();

            return CloseArchiveThread;
        }

        protected void OpenArchiveProc()
        {
            long itemSize = 0;
            int index = 0;
            long totalSize = 0;

            OnArchiveStatusChanged(new CBZArchiveStatusEvent(this, CBZArchiveStatusEvent.ARCHIVE_OPENING));

            int count = 0;
            try
            {
                Archive = ZipFile.Open(FileName, Mode);
                count = Archive.Entries.Count;

                try
                {
                    ZipArchiveEntry metaDataEntry = Archive.GetEntry("ComicInfo.xml");

                    if (metaDataEntry != null)
                    {
                        MetaData = new CBZMetaData(metaDataEntry.Open(), metaDataEntry.FullName);

                        OnMetaDataLoaded(new MetaDataLoadEvent(MetaData.Values));
                    } else
                    {
                        OnLogMessage(new LogMessageEvent(LogMessageEvent.LOGMESSAGE_TYPE_WARNING, "No Metadata (ComicInfo.xml) found in Archive!"));
                    }
                } catch (Exception)
                {
                    OnLogMessage(new LogMessageEvent(LogMessageEvent.LOGMESSAGE_TYPE_ERROR, "Error loading Metadata (ComicInfo.xml) from Archive!"));
                }

                foreach (ZipArchiveEntry entry in Archive.Entries)
                {
                    if (!entry.FullName.ToLower().Contains("comicinfo.xml"))
                    {
                        itemSize = entry.Length;
                        CBZImage cBZImage = new CBZImage(entry.Open(), entry.FullName);
                        cBZImage.Size = itemSize;
                        cBZImage.Number = index + 1;
                        cBZImage.Index = index;
                        cBZImage.Filename = entry.FullName;
                        cBZImage.Compressed = true;
                        cBZImage.LastModified = entry.LastWriteTime;

                        Pages.Add(cBZImage);
                        OnImageLoaded(new ItemLoadProgressEvent(index, count, cBZImage));

                        totalSize += itemSize;
                        index++;
                    }

                    Thread.Sleep(50);
                }
            } catch (Exception e)
            {
                OnLogMessage(new LogMessageEvent(LogMessageEvent.LOGMESSAGE_TYPE_ERROR, "Error opening Archive\n" + e.Message));
            }

            FileSize = totalSize;

            OnArchiveStatusChanged(new CBZArchiveStatusEvent(this, CBZArchiveStatusEvent.ARCHIVE_OPENED));
        }

        protected void ExtractArchiveProc()
        {
            OnArchiveStatusChanged(new CBZArchiveStatusEvent(this, CBZArchiveStatusEvent.ARCHIVE_EXTRACTING));

            int count = 0;
            int index = 0;
            try
            {
                Archive = ZipFile.Open(Name, Mode);
                count = Archive.Entries.Count;

                try
                {
                    ZipArchiveEntry metaDataEntry = Archive.GetEntry("ComicInfo.xml");

                    if (metaDataEntry != null)
                    {
                        MetaData = new CBZMetaData(metaDataEntry.Open(), metaDataEntry.FullName);

                        OnMetaDataLoaded(new MetaDataLoadEvent(MetaData.Values));
                    }
                }
                catch (Exception)
                { }

                foreach (ZipArchiveEntry entry in Archive.Entries)
                {
                    if (!entry.FullName.ToLower().Contains("comicinfo.xml"))
                    {
                        entry.ExtractToFile(WorkingDir + ProjectGUID + "\\" + entry.Name);
                        OnItemExtracted(new ItemExtractedEvent(index, count, WorkingDir + ProjectGUID + "\\" + entry.Name));
                        index++;
                    }

                    Thread.Sleep(50);
                }
            }
            catch (Exception e)
            {
                OnLogMessage(new LogMessageEvent(LogMessageEvent.LOGMESSAGE_TYPE_ERROR, "Error extracting File from Archive [" + e.Message + "]"));
            }

            OnArchiveStatusChanged(new CBZArchiveStatusEvent(this, CBZArchiveStatusEvent.ARCHIVE_EXTRACTED));
        }

        protected void CloseArchiveProc()
        {
            OnArchiveStatusChanged(new CBZArchiveStatusEvent(this, CBZArchiveStatusEvent.ARCHIVE_CLOSING));

            if (Archive != null)
            {
                Archive.Dispose();
            }

            FileSize = 0;
            FileName = "";
            foreach (CBZImage page in Pages)
            {
                page.FreeImage();
                OnItemChanged(new ItemChangedEvent(page.Index, Pages.Count, page));
                Thread.Sleep(100);
            }

            Pages.Clear();
            MetaData.Free();

            Name = "";

            OnArchiveStatusChanged(new CBZArchiveStatusEvent(this, CBZArchiveStatusEvent.ARCHIVE_CLOSED));
        }

        protected virtual void OnImageLoaded(ItemLoadProgressEvent e)
        {
            EventHandler<ItemLoadProgressEvent> handler = ImageProgress;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnItemChanged(ItemChangedEvent e)
        {
            EventHandler<ItemChangedEvent> handler = ItemChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnArchiveStatusChanged(CBZArchiveStatusEvent e)
        {
            EventHandler<CBZArchiveStatusEvent> handler = ArchiveStatusChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnMetaDataLoaded(MetaDataLoadEvent e)
        {
            EventHandler<MetaDataLoadEvent> handler = MetaDataLoaded;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnOperationFinished(OperationFinishedEvent e)
        {
            EventHandler<OperationFinishedEvent> handler = OperationFinished;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnLogMessage(LogMessageEvent e)
        {
            EventHandler<LogMessageEvent> handler = LogMessage;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnItemExtracted(ItemExtractedEvent e)
        {
            EventHandler<ItemExtractedEvent> handler = ItemExtracted;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }
}
