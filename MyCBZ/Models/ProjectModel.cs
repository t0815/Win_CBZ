using Win_CBZ;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.WebRequestMethods;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using File = System.IO.File;

namespace Win_CBZ
{
    internal class ProjectModel
    {
        public String Name { get; set; }

        public String FileName { get; set; }

        public String TemporaryFileName { get; set; }

        public long FileSize { get; set; } = 0;

        public String Description { get; set; }

        public String WorkingDir { get; set; }

        public int ArchiveState { get; set; }

        protected String ProjectGUID { get; set; }

        public Boolean IsNew = false;

        public Boolean IsSaved = false;

        public Boolean IsChanged = false;

        public Boolean IsClosed = false;

        public Boolean ApplyRenaming = false;

        public String RenameStoryPagePattern { get; set; }

        public String RenameSpecialPagePattern { get; set; }

        public bool PreloadPageImages { get; set; }

        public BindingList<Page> Pages { get; set; }

        public MetaData MetaData { get; set; }

        public long TotalSize { get; set; }

        private System.IO.Compression.ZipArchive Archive { get; set; }

        private ZipArchiveMode Mode;

        public event EventHandler<TaskProgressEvent> TaskProgress;

        public event EventHandler<PageChangedEvent> PageChanged;

        public event EventHandler<CBZArchiveStatusEvent> ArchiveStatusChanged;

        public event EventHandler<MetaDataLoadEvent> MetaDataLoaded;

        public event EventHandler<OperationFinishedEvent> OperationFinished;

        public event EventHandler<ItemExtractedEvent> ItemExtracted;

        public event EventHandler<FileOperationEvent> FileOperation;

        public event EventHandler<ArchiveOperationEvent> ArchiveOperation;

        private Thread LoadArchiveThread;

        private Thread ExtractArchiveThread;

        private Thread CloseArchiveThread;

        private Thread SaveArchiveThread;

        private Thread DeleteFileThread;

        private Thread PageUpdateThread;

        private Random RandomProvider;


        public ProjectModel(String workingDir)
        {
            WorkingDir = workingDir;
            Pages = new BindingList<Page>();
            MetaData = new MetaData(false);
            RandomProvider = new Random();

            ProjectGUID = Guid.NewGuid().ToString();
            if (!Directory.Exists(PathHelper.ResolvePath(WorkingDir) + ProjectGUID))
            {
                DirectoryInfo di = Directory.CreateDirectory(PathHelper.ResolvePath(WorkingDir) + ProjectGUID);
            }
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

            SaveArchiveThread = new Thread(new ThreadStart(SaveArchiveProc));
            SaveArchiveThread.Start();
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

        public void AddImages(List<LocalFile> fileList, int maxIndex = 0)
        {
            int index = maxIndex + 1;

            foreach (LocalFile fileObject in fileList)
            {
                try
                {
                    FileInfo localCopyInfo = fileObject.FileInfo.CopyTo(PathHelper.ResolvePath(WorkingDir) + ProjectGUID + "\\" + fileObject.FileInfo.Name);
                    
                    Page page = new Page(localCopyInfo, FileAccess.ReadWrite);
                    page.Size = fileObject.FileSize;
                    page.Number = index + 1;
                    page.Index = index;
                    page.LocalPath = fileObject.FullPath;
                    page.Compressed = false;
                    page.LastModified = fileObject.FileInfo.LastWriteTime;
                    page.Name = fileObject.FileInfo.Name;
                    page.TempPath = localCopyInfo.FullName;
                    fileObject.FileInfo = localCopyInfo;

                    Pages.Add(page);

                    OnPageChanged(new PageChangedEvent(page, PageChangedEvent.IMAGE_STATUS_NEW));
                    
                    index++;
                } catch (Exception ef)
                {
                    MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_ERROR, ef.Message);
                } finally
                {
                    if (index > maxIndex)
                    {
                        OnArchiveStatusChanged(new CBZArchiveStatusEvent(this, CBZArchiveStatusEvent.ARCHIVE_FILE_ADDED));

                        this.IsChanged = true;
                    }
                }
            }

            OnOperationFinished(new OperationFinishedEvent(index, Pages.Count));

            UpdatePageIndices();
        }

        public List<LocalFile> LoadDirectory(String path)
        {
            List<LocalFile> files = new List<LocalFile>();

            DirectoryInfo di = new DirectoryInfo(path);

            try
            {
                foreach (var fi in di.EnumerateFiles())
                {
                    LocalFile localFile = new LocalFile(fi.FullName);
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

        public List<LocalFile> ParseFiles(List<String> files)
        {
            List<LocalFile> filesObjects = new List<LocalFile>();

            try
            {
                foreach (String fname in files)
                {
                    var fi = new FileInfo(fname);
                     
                    LocalFile localFile = new LocalFile(fi.FullName);
                    localFile.FilePath = fi.Directory.FullName;
                    localFile.FileName = fi.Name;
                    localFile.FileSize = fi.Length;
                    localFile.FileInfo = fi;

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
            List<Page> deletedPagesList = new List<Page>();

            foreach (Page page in Pages)
            {
                if (page.Deleted)
                {
                    deletedPagesList.Add(page);
                }
            }

            foreach (Page page in deletedPagesList)
            {
                Pages.Remove(page);
            }

            return deletedPagesCount;
        }

        public void UpdatePageIndices()
        {
            if (LoadArchiveThread != null)
            {
                if (LoadArchiveThread.IsAlive)
                {
                    LoadArchiveThread.Abort();
                }
            }

            if (PageUpdateThread != null)
            {
                if (PageUpdateThread.IsAlive)
                {
                    return;
                }
            }

            PageUpdateThread = new Thread(new ThreadStart(UpdatePageIndicesProc));
            PageUpdateThread.Start();

            //return PageUpdateThread;
        }

        protected void UpdatePageIndicesProc()
        {
            int newIndex = 0;
            int updated = 1;
            foreach (Page page in Pages)
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

                OnPageChanged(new PageChangedEvent(page, PageChangedEvent.IMAGE_STATUS_CHANGED));
                OnTaskProgress(new TaskProgressEvent(page, updated, Pages.Count));
                OnArchiveStatusChanged(new CBZArchiveStatusEvent(this, CBZArchiveStatusEvent.ARCHIVE_FILE_UPDATED));

                this.IsChanged = true;
                updated++;
            }

            OnOperationFinished(new OperationFinishedEvent(0, Pages.Count));
        }

        public void RenamePage(Page page, String name)
        {
            if (name == null)
            {
                throw new PageException(page, "Failed to rename page '" + page.Name + "' (" + page.Id + ")! The new name must not be NULL.");
            }

            foreach (Page oldPage in Pages)
            {
                if (oldPage.Name.ToLower().Equals(name.ToLower()))
                {
                    throw new PageDuplicateNameException(page, "Failed to rename page '" + page.Name + "' (" + page.Id + ")! A different page with the same name already exists.");
                }
            }

            OnArchiveStatusChanged(new CBZArchiveStatusEvent(this, CBZArchiveStatusEvent.ARCHIVE_FILE_UPDATED));

            page.Name = name;
        }

        public String RenameEntryScript(Page page)
        {
            string newName = page.Name;
            string pattern = "";

            switch (page.ImageType)
            {
                case MetaDataEntryPage.COMIC_PAGE_TYPE_STORY:
                    pattern = RenameStoryPagePattern;
                    break;
                default:
                    pattern = RenameSpecialPagePattern;
                    break;
            }

            if (pattern != null)
            {
                if (pattern.Length > 0)
                {
                    OnPageChanged(new PageChangedEvent(page, PageChangedEvent.IMAGE_STATUS_RENAMED));

                    this.IsChanged = true;
                }
            }

            return newName;
        }

        public String RequestTemporaryFile(Page page)
        {
            String tempFileName = MakeNewTempFileName(page.Name).FullName;
            if (page.Compressed)
            {
                ExtractSingleFile(page, tempFileName);
            }

            return page.TempPath;
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
                        MetaData = new MetaData(metaDataEntry.Open(), metaDataEntry.FullName);

                        OnMetaDataLoaded(new MetaDataLoadEvent(MetaData.Values));
                    } else
                    {
                        MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_WARNING, "No Metadata (ComicInfo.xml) found in Archive!");
                    }
                } catch (Exception)
                {
                    MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_ERROR, "Error loading Metadata (ComicInfo.xml) from Archive!");
                }

                // String tempFileName = "";
                MetaDataEntryPage pageIndexEntry;
                foreach (ZipArchiveEntry entry in Archive.Entries)
                {
                    if (!entry.FullName.ToLower().Contains("comicinfo.xml"))
                    {
                        Page page = new Page(entry, PathHelper.ResolvePath(WorkingDir) + ProjectGUID + "\\", MakeNewRandomId());

                        page.Number = index + 1;
                        page.Index = index;

                        pageIndexEntry = MetaData.FindIndexEntryForPage(page);
                        if (pageIndexEntry != null)
                        {
                            page.ImageType = pageIndexEntry.GetAttribute(MetaDataEntryPage.COMIC_PAGE_ATTRIBUTE_TYPE);
                            page.Key = pageIndexEntry.GetAttribute(MetaDataEntryPage.COMIC_PAGE_ATTRIBUTE_KEY);
                        }

                        // tempFileName = RequestTemporaryFile(page);

                        Pages.Add(page);
                        
                        OnPageChanged(new PageChangedEvent(page, PageChangedEvent.IMAGE_STATUS_NEW));
                        OnTaskProgress(new TaskProgressEvent(page, index, count));

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

        public void RunRenameScriptsForPages()
        {
            foreach (Page page in Pages)
            {
                RenamePageScript(page);

                OnArchiveStatusChanged(new CBZArchiveStatusEvent(this, CBZArchiveStatusEvent.ARCHIVE_FILE_RENAMED));
            }
        }

        protected void SaveArchiveProc()
        {
            OnArchiveStatusChanged(new CBZArchiveStatusEvent(this, CBZArchiveStatusEvent.ARCHIVE_SAVING));

            int index = 0;
            List<Page> deletedPages = new List<Page>();

            TemporaryFileName = MakeNewTempFileName("", ".cbz").FullName;

            ZipArchive BuildingArchive = null;
            try
            {
                BuildingArchive = ZipFile.Open(TemporaryFileName, ZipArchiveMode.Create);

                // Rebuild ComicInfo.xml's PageIndex
                MetaData.RebuildPageMetaData(Pages.ToList<Page>());

                // Write files to new temporary archive
                foreach (Page page in Pages)
                {
                    try
                    {
                        if (page.Compressed)
                        {
                            FileInfo NewTemporaryFileName = MakeNewTempFileName(page.Name);
                            page.CreateLocalWorkingCopy(NewTemporaryFileName.FullName);
                            if (page.TempPath == null)
                            {
                                MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_WARNING, "Failed to extract to or create temporary file for entry [" + page.Name + "]");
                            }
                        } 

                        if (!page.Deleted)
                        {
                            if (ApplyRenaming)
                            {
                                RenamePageScript(page);
                            }
                            BuildingArchive.CreateEntryFromFile(page.TempPath, page.Name);
                        } else
                        {
                            // collect all deleted items
                            deletedPages.Add(page);
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
                if (MetaData.Values.Count > 0 || MetaData.PageIndex.Count > 0)
                {
                    MemoryStream ms = MetaData.BuildComicInfoXMLStream();
                    ZipArchiveEntry metaDataEntry = BuildingArchive.CreateEntry("ComicInfo.xml");
                    Stream entryStream = metaDataEntry.Open();
                    ms.CopyTo(entryStream);
                    entryStream.Close();
                    ms.Close();
                }
            } catch (Exception ex)
            {
                MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_ERROR, "Error opening Archive for writing! [" + ex.Message + "]");
            } finally
            {
                try
                {
                    if (BuildingArchive != null)
                    {
                        BuildingArchive.Dispose();
                    }

                    if (Archive != null)
                    {
                        Archive.Dispose();
                    }

                    CopyFile(TemporaryFileName, FileName);

                    int deletedIndex = 0;
                    foreach (Page deletedPage in deletedPages)
                    {
                        Pages.Remove(deletedPage);

                        OnPageChanged(new PageChangedEvent(deletedPage, PageChangedEvent.IMAGE_STATUS_DELETED));
                        OnTaskProgress(new TaskProgressEvent(deletedPage, deletedIndex, deletedPages.Count));
                        deletedIndex++;
                    }

                } catch (Exception mvex)
                {
                    MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_ERROR, "Error finalizing CBZ [" + mvex.Message + "]");
                } finally
                {
                    try
                    {
                        File.Delete(TemporaryFileName);
                        Archive = ZipFile.Open(FileName, ZipArchiveMode.Read);
                        IsChanged = false;
                    } catch (Exception rex)
                    {
                        MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_ERROR, "Error finalizing CBZ [" + rex.Message + "]");
                    }
                }
            }

            OnArchiveStatusChanged(new CBZArchiveStatusEvent(this, CBZArchiveStatusEvent.ARCHIVE_SAVED));
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void RenamePageScript(Page page)
        {
            String newName = RenameEntryScript(page);
            MetaData.UpdatePageIndexMetaDataEntry(newName, page);
            page.Name = newName;
            page.Changed = true;
        }


        [MethodImpl(MethodImplOptions.Synchronized)]
        public void ExtractSingleFile(Page page, String path = null)
        {
            try
            {
                ZipArchiveEntry fileEntry = Archive.GetEntry(page.Name);
                if (fileEntry != null)
                {
                    FileInfo NewTemporaryFileName = MakeNewTempFileName(fileEntry.Name);
                    fileEntry.ExtractToFile(NewTemporaryFileName.FullName);
                    page.TempPath = NewTemporaryFileName.FullName;
                    OnItemExtracted(new ItemExtractedEvent(1, 1, NewTemporaryFileName.FullName));
                } else
                {
                    MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_WARNING, "No Entry with name [" + page.Name + "] exists in archive!");
                }
            }
            catch (Exception efile)
            {
                MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_ERROR, "Error extracting File from Archive [" + efile.Message + "]");
            }
        }

        protected FileInfo MakeNewTempFileName(String entryName, String extension = "")
        {
            return new FileInfo(PathHelper.ResolvePath(WorkingDir) + ProjectGUID + "\\" + MakeNewRandomId() + extension + ".tmp");
        }

        protected String MakeNewRandomId()
        {
            return RandomProvider.Next().ToString("X");
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void CopyFile(string inputFilePath, string outputFilePath)
        {
            int bufferSize = 1024 * 1024;

            using (FileStream fileStream = new FileStream(outputFilePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite))
            {
                FileStream fs = new FileStream(inputFilePath, FileMode.Open, FileAccess.ReadWrite);
                fileStream.SetLength(fs.Length);
                int bytesRead = -1;
                byte[] bytes = new byte[bufferSize];

                while ((bytesRead = fs.Read(bytes, 0, bufferSize)) > 0)
                {
                    fileStream.Write(bytes, 0, bytesRead);
                }
                fs.Close();
            }
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
                        MetaData = new MetaData(metaDataEntry.Open(), metaDataEntry.FullName);

                        OnMetaDataLoaded(new MetaDataLoadEvent(MetaData.Values));
                    }
                }
                catch (Exception)
                { }

                ZipArchiveEntry fileEntry;
                foreach (Page page in Pages)
                {
                    try
                    {
                        
                        fileEntry = Archive.GetEntry(page.Name);
                        if (fileEntry != null)
                        {
                            fileEntry.ExtractToFile(PathHelper.ResolvePath(WorkingDir) + ProjectGUID + "\\" + fileEntry.Name);
                            page.TempPath = PathHelper.ResolvePath(WorkingDir) + ProjectGUID + "\\" + fileEntry.Name;
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

            MetaData.Free();

            FileSize = 0;
            FileName = "";
            foreach (Page page in Pages)
            {
                page.FreeImage();
                OnPageChanged(new PageChangedEvent(page, PageChangedEvent.IMAGE_STATUS_CLOSED));
                OnTaskProgress(new TaskProgressEvent(page, page.Index, Pages.Count));
                Thread.Sleep(100);
            }

            Pages.Clear();
            Name = "";
            FileName = "";
            IsSaved = false;
            IsNew = true;
            IsClosed = true;

            OnArchiveStatusChanged(new CBZArchiveStatusEvent(this, CBZArchiveStatusEvent.ARCHIVE_CLOSED));
        }

        public Thread ClearTempFolder()
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

            if (ExtractArchiveThread != null)
            {
                if (ExtractArchiveThread.IsAlive)
                {
                    ExtractArchiveThread.Abort();
                }
            }

            DeleteFileThread = new Thread(new ThreadStart(DeleteTempFolderItems));
            DeleteFileThread.Start();

            return DeleteFileThread;
        }

        public void DeleteTempFolderItems()
        {
            String path = PathHelper.ResolvePath(WorkingDir);
            
            DirectoryInfo dir = new DirectoryInfo(path);
            var folders = dir.EnumerateDirectories();
           

        }

        public void CopyTo(ProjectModel destination)
        {
            if (destination != null)
            {
                Page[] copyPages = new Page[this.Pages.Count];

                this.Pages.CopyTo(copyPages, 0);
                destination.Pages = new BindingList<Page>(copyPages);
                destination.MetaData = this.MetaData;
                destination.Name = this.Name;
                destination.ProjectGUID = this.ProjectGUID;
                destination.RenameStoryPagePattern = this.RenameStoryPagePattern;
                destination.RenameSpecialPagePattern = this.RenameSpecialPagePattern;
                destination.WorkingDir = this.WorkingDir;
                destination.IsChanged = this.IsChanged;
                destination.IsNew = this.IsNew;
                destination.IsSaved = this.IsSaved;
                destination.FileSize = this.FileSize;
                destination.IsClosed = this.IsClosed;
                destination.TemporaryFileName = this.TemporaryFileName;
                destination.TotalSize = this.TotalSize;
            }
        }

        protected virtual void OnPageChanged(PageChangedEvent e)
        {
            PageChanged?.Invoke(this, e);
        }

        protected virtual void OnTaskProgress(TaskProgressEvent e)
        {
            TaskProgress?.Invoke(this, e);
        }

        protected virtual void OnArchiveStatusChanged(CBZArchiveStatusEvent e)
        {
            ArchiveStatusChanged?.Invoke(this, e);
        }

        protected virtual void OnMetaDataLoaded(MetaDataLoadEvent e)
        {
            MetaDataLoaded?.Invoke(this, e);
        }

        protected virtual void OnOperationFinished(OperationFinishedEvent e)
        {
            OperationFinished?.Invoke(this, e);
        }

        protected virtual void OnItemExtracted(ItemExtractedEvent e)
        {
            ItemExtracted?.Invoke(this, e);
        }

        protected virtual void OnFileOperation(FileOperationEvent e)
        {
            FileOperation?.Invoke(this, e);
        }

        protected virtual void OnArchiveOperation(ArchiveOperationEvent e)
        {
            ArchiveOperation?.Invoke(this, e);
        }
    }
}
