using MyCBZ;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
using static System.Net.WebRequestMethods;

namespace CBZMage
{
    internal class CBZProjectModel
    {
        public String Name { get; set; }

        public String FileName { get; set; }

        public long FileSize { get; set; } = 0;

        public String Description { get; set; }

        public String WorkingDir { get; set; }

        public int ArchiveState { get; set; }

        protected String ProjectGUID { get; set; }

        public Boolean IsNew = false;

        public Boolean IsSaved = false;

        public Boolean IsChanged = false;

        public Boolean IsClosed = false;

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

        public event EventHandler<FileOperationEvent> FileOperation;

        public event EventHandler<ArchiveOperationEvent> ArchiveOperation;

        private Thread LoadArchiveThread;

        private Thread ExtractArchiveThread;

        private Thread CloseArchiveThread;

        private Thread SaveArchiveThread;


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

        public void Save()
        {

            if (LoadArchiveThread != null)
            {
                if (LoadArchiveThread.IsAlive)
                {

                    return;
                }
            }

            if (CloseArchiveThread != null)
            {
                if (CloseArchiveThread.IsAlive)
                {

                    return;
                }
            }

            SaveArchiveThread = new Thread(new ThreadStart(SaveArchiveProc));
            SaveArchiveThread.Start();

            // return SaveArchiveThread;
        }

        public void SaveAs(String path, ZipArchiveMode mode)
        {
            FileName = path;
            Mode = mode;

            Save();
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
                    FileInfo localCopyInfo = fileObject.FileInfo.CopyTo(PathHelper.ResolvePath(WorkingDir) + ProjectGUID + "\\" + fileObject.FileInfo.Name);
                    
                    CBZImage cBZImage = new CBZImage(localCopyInfo, FileAccess.ReadWrite);
                    cBZImage.Size = fileObject.FileSize;
                    cBZImage.Number = index + 1;
                    cBZImage.Index = index;
                    cBZImage.LocalPath = fileObject.FullPath;
                    cBZImage.Compressed = false;
                    cBZImage.LastModified = fileObject.FileInfo.LastWriteTime;
                    cBZImage.Name = fileObject.FileInfo.Name;
                    cBZImage.TempPath = localCopyInfo.FullName;
                    fileObject.FileInfo = localCopyInfo;

                    Pages.Add(cBZImage);

                    OnImageLoaded(new ItemLoadProgressEvent(index, Pages.Count, cBZImage));

                    index++;
                } catch (Exception ef)
                {
                    MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_ERROR, ef.Message);
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
                MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_ERROR, ex.Message);
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
                MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_ERROR, ex.Message);
            } finally
            {
                //
            }

            return filesObjects;
        }

        public int RemoveDeletedPages()
        {
            int deletedPagesCount = 0;
            List<CBZImage> deletedPagesList = new List<CBZImage>();

            foreach (CBZImage page in Pages)
            {
                if (page.Deleted)
                {
                    deletedPagesList.Add(page);
                }
            }

            foreach (CBZImage page in deletedPagesList)
            {
                Pages.Remove(page);
            }

            return deletedPagesCount;
        }

        public void UpdatePageIndices()
        {
            int newIndex = 0;
            int updated = 1;
            foreach (CBZImage page in Pages)
            {
                if (page.Deleted)
                {
                    page.Index = -1;
                    page.Number = -1;
                } else
                {
                    page.Index = newIndex;
                    page.Number = newIndex + 1;
                    newIndex++;
                }

                OnImageLoaded(new ItemLoadProgressEvent(updated, Pages.Count, page));
                updated++;
            }
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
                        MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_WARNING, "No Metadata (ComicInfo.xml) found in Archive!");
                    }
                } catch (Exception)
                {
                    MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_ERROR, "Error loading Metadata (ComicInfo.xml) from Archive!");
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
                        cBZImage.Name = entry.Name;

                        Pages.Add(cBZImage);
                        
                        OnImageLoaded(new ItemLoadProgressEvent(index, count, cBZImage));

                        totalSize += itemSize;
                        index++;
                    }

                    Thread.Sleep(50);
                }
            } catch (Exception e)
            {
                MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_ERROR, "Error opening Archive\n" + e.Message);
            }

            FileSize = totalSize;

            OnArchiveStatusChanged(new CBZArchiveStatusEvent(this, CBZArchiveStatusEvent.ARCHIVE_OPENED));
        }

        protected void SaveArchiveProc()
        {
            OnArchiveStatusChanged(new CBZArchiveStatusEvent(this, CBZArchiveStatusEvent.ARCHIVE_SAVING));

            int count = 0;
            int index = 0;
            try
            {
                Archive = ZipFile.Open(FileName, Mode);
                count = Archive.Entries.Count;

                // Write files to archive
                ZipArchiveEntry processingEntry;
                foreach (CBZImage page in Pages)
                {
                    try
                    {
                        if (page.Compressed)
                        {
                            processingEntry = Archive.GetEntry(page.EntryName);
                            processingEntry.Delete();

                            if (!page.Deleted)
                            {
                                Archive.CreateEntryFromFile(page.TempPath, page.Name);
                            }
                        } else
                        {
                            if (!page.Deleted)
                            {
                                Archive.CreateEntryFromFile(page.TempPath, page.Name);
                            }
                        }
                    }
                    catch (Exception efile)
                    {
                        MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_ERROR, "Error compressing File [" + page.TempPath +"] to Archive [" + efile.Message + "]");
                    }

                    OnArchiveOperation(new ArchiveOperationEvent(ArchiveOperationEvent.OPERATION_COMPRESS, ArchiveOperationEvent.STATUS_SUCCESS, index, Pages.Count + 1, page));

                    index++;
                }

                // Create Metadata


            } catch (Exception ex)
            {
                MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_ERROR, "Error opening Archive for writing! [" + ex.Message + "]");
            }

            OnArchiveStatusChanged(new CBZArchiveStatusEvent(this, CBZArchiveStatusEvent.ARCHIVE_SAVED));
        }


        protected void ExtractArchiveProc()
        {
            OnArchiveStatusChanged(new CBZArchiveStatusEvent(this, CBZArchiveStatusEvent.ARCHIVE_EXTRACTING));

            int count = 0;
            int index = 0;
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
                    }
                }
                catch (Exception)
                { }

                ZipArchiveEntry fileEntry;
                foreach (CBZImage cBZImage in Pages)
                {
                    try
                    {
                        fileEntry = Archive.GetEntry(cBZImage.Name);
                        if (fileEntry != null)
                        {
                            fileEntry.ExtractToFile(PathHelper.ResolvePath(WorkingDir) + ProjectGUID + "\\" + fileEntry.Name);
                            cBZImage.TempPath = PathHelper.ResolvePath(WorkingDir) + ProjectGUID + "\\" + fileEntry.Name;
                            OnItemExtracted(new ItemExtractedEvent(index, Pages.Count, PathHelper.ResolvePath(WorkingDir) + ProjectGUID + "\\" + fileEntry.Name));
                            index++;
                        }
                    } catch (Exception efile)
                    {
                        MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_ERROR, "Error extracting File from Archive [" + efile.Message + "]");
                    }

                }

                /*  Bulk Extract??!?
                foreach (ZipArchiveEntry entry in Archive.Entries)
                {
                    if (!entry.FullName.ToLower().Contains("comicinfo.xml"))
                    {
                        entry.ExtractToFile(PathHelper.ResolvePath(WorkingDir) + ProjectGUID + "\\" + entry.Name);
                        OnItemExtracted(new ItemExtractedEvent(index, count, PathHelper.ResolvePath(WorkingDir) + ProjectGUID + "\\" + entry.Name));
                        index++;
                    }

                    Thread.Sleep(50);
                }
                */
            }
            catch (Exception e)
            {
                MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_ERROR, "Error opening Archive [" + e.Message + "]");
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

        protected virtual void OnItemExtracted(ItemExtractedEvent e)
        {
            EventHandler<ItemExtractedEvent> handler = ItemExtracted;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnFileOperation(FileOperationEvent e)
        {
            EventHandler<FileOperationEvent> handler = FileOperation;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnArchiveOperation(ArchiveOperationEvent e)
        {
            EventHandler<ArchiveOperationEvent> handler = ArchiveOperation;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }
}
