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
using System.Drawing;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Forms.VisualStyles;
using System.Collections;

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

        public String OutputDirectory { get; set; }

        public int ArchiveState { get; set; }

        protected String ProjectGUID { get; set; }

        public Boolean IsNew = false;

        public Boolean IsSaved = false;

        public Boolean IsChanged = false;

        public Boolean IsClosed = false;

        public Boolean ApplyRenaming = false;

        public String RenameStoryPagePattern { get; set; }

        public String RenameSpecialPagePattern { get; set; }

        public ArrayList RenamerExcludes { get; set; }

        public bool PreloadPageImages { get; set; }

        public BindingList<Page> Pages { get; set; }

        private ArrayList FileNamesToAdd { get; set; }

        private BindingList<LocalFile> Files { get; set; }

        private int MaxFileIndex = 0;

        private bool InitialPageIndexRebuild = false;

        public MetaData MetaData { get; set; }

        public long TotalSize { get; set; }

        private System.IO.Compression.ZipArchive Archive { get; set; }

        private ZipArchiveMode Mode;

        public event EventHandler<TaskProgressEvent> TaskProgress;

        public event EventHandler<PageChangedEvent> PageChanged;

        public event EventHandler<ApplicationStatusEvent> ApplicationStateChanged;

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

        private Thread ProcessAddedFiles;

        private Thread ParseAddedFileNames;

        private Thread RenamingThread;

        private Thread RestoreRenamingThread;


        public ProjectModel(String workingDir)
        {
            WorkingDir = workingDir;
            Pages = new BindingList<Page>();
            Files = new BindingList<LocalFile>();
            MetaData = new MetaData(false);
            RandomProvider = new Random();
            FileNamesToAdd = new ArrayList();
            RenamerExcludes = new ArrayList();

            //Pipeline += HandlePipelene;

            ProjectGUID = Guid.NewGuid().ToString();
            if (!Directory.Exists(PathHelper.ResolvePath(WorkingDir) + ProjectGUID))
            {
                try
                {
                    DirectoryInfo di = Directory.CreateDirectory(PathHelper.ResolvePath(WorkingDir) + ProjectGUID);
                } catch ( Exception e )
                {
                    MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_ERROR, e.Message);
                }
            }
        }

        private void HandlePipelene(PipelineEvent e)
        {
            if (e.State == PipelineEvent.PIPELINE_FILES_PARSED)
            {
                Task.Factory.StartNew(() =>
                {

                    if (LoadArchiveThread != null)
                    {
                        if (LoadArchiveThread.IsAlive)
                        {
                            LoadArchiveThread.Abort();
                        }
                    }

                    if (ExtractArchiveThread != null)
                    {
                        if (ExtractArchiveThread.IsAlive)
                        {
                            ExtractArchiveThread.Abort();
                        }
                    }

                    if (PageUpdateThread != null)
                    {
                        if (PageUpdateThread.IsAlive)
                        {
                            PageUpdateThread.Abort();
                        }
                    }

                    if (CloseArchiveThread != null)
                    {
                        while (CloseArchiveThread.IsAlive)
                        {
                            System.Threading.Thread.Sleep(100);
                        }
                    }

                    ProcessAddedFiles = new Thread(new ThreadStart(AddImagesProc));
                    ProcessAddedFiles.Start();

                });
            }

            if (e.State == PipelineEvent.PIPELINE_PAGES_ADDED)
            {
                FileNamesToAdd.Clear();
                Files.Clear();
                InitialPageIndexRebuild = true;
                Task.Factory.StartNew(() =>
                {
                    UpdatePageIndices();
                });
            }

            if (e.State == PipelineEvent.PIPELINE_SAVE_REQUESTED)
            {
                if (e.payload != null)
                {
                    String value = e.payload.GetAttribute(PipelinePayload.PAYLOAD_EXECUTE_RENAME_SCRIPT);
                }
            }

            if (e.State == PipelineEvent.PIPELINE_SAVE_RUN_RENAMING)
            {
                Task.Factory.StartNew(() =>
                {

                    if (LoadArchiveThread != null)
                    {
                        if (LoadArchiveThread.IsAlive)
                        {
                            LoadArchiveThread.Abort();
                        }
                    }

                    if (ExtractArchiveThread != null)
                    {
                        if (ExtractArchiveThread.IsAlive)
                        {
                            ExtractArchiveThread.Abort();
                        }
                    }

                    if (PageUpdateThread != null)
                    {
                        if (PageUpdateThread.IsAlive)
                        {
                            PageUpdateThread.Abort();
                        }
                    }

                    if (RenamingThread != null)
                    {
                        while (RenamingThread.IsAlive)
                        {
                            System.Threading.Thread.Sleep(100);
                        }
                    }

                    RenamingThread = new Thread(new ThreadStart(AutoRenameAllPagesProc));
                    RenamingThread.Start();

                });
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

                if (ExtractArchiveThread != null)
                {
                    if (ExtractArchiveThread.IsAlive)
                    {
                        ExtractArchiveThread.Abort();
                    }
                }

                if (PageUpdateThread != null)
                {
                    if (PageUpdateThread.IsAlive)
                    {
                        PageUpdateThread.Abort();
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
                MaxFileIndex = 0;
                Files.Clear();
                FileNamesToAdd.Clear();


                ProjectGUID = Guid.NewGuid().ToString();
                if (!Directory.Exists(PathHelper.ResolvePath(WorkingDir) + ProjectGUID))
                {
                    try
                    {
                        DirectoryInfo di = Directory.CreateDirectory(PathHelper.ResolvePath(WorkingDir) + ProjectGUID);
                    }
                    catch (Exception e)
                    {
                        MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_ERROR, e.Message);
                    }
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
                    //LoadArchiveThread.Abort();
                    return null;
                }
            }

            if (CloseArchiveThread != null)
            {
                while (CloseArchiveThread.IsAlive)
                {
                    System.Threading.Thread.Sleep(100);
                }
            }

            LoadArchiveThread = new Thread(new ThreadStart(OpenArchiveProc));
            LoadArchiveThread.Start();

            return LoadArchiveThread;
        }

        public Thread Save()
        {

            if (LoadArchiveThread != null)
            {
                if (LoadArchiveThread.IsAlive)
                {
                    return null;
                }
            }

            if (CloseArchiveThread != null)
            {
                if (CloseArchiveThread.IsAlive)
                {
                    return null;
                }
            }

            SaveArchiveThread = new Thread(new ThreadStart(SaveArchiveProc));
            SaveArchiveThread.Start();

            return SaveArchiveThread;
        }

        public Thread SaveAs(String path, ZipArchiveMode mode)
        {
            FileName = path;
            Mode = mode;

            return Save();
        }

        public Thread Extract(String outputPath = null)
        {
            this.OutputDirectory = outputPath;

            if (LoadArchiveThread != null)
            {
                if (LoadArchiveThread.IsAlive)
                {
                    return null;
                }
            }

            if (CloseArchiveThread != null)
            {
                if (CloseArchiveThread.IsAlive)
                {
                    return null;
                }
            }

            ExtractArchiveThread = new Thread(new ThreadStart(ExtractArchiveProc));
            ExtractArchiveThread.Start();

            return ExtractArchiveThread;
        } 

        
        public void AddImagesProc()
        {
            int index = MaxFileIndex;
            String targetPath = "";
            Page page = null;

            foreach (LocalFile fileObject in Files)
            {
                try
                {
                    targetPath = PathHelper.ResolvePath(WorkingDir) + ProjectGUID + "\\" + fileObject.FileName;

                    this.CopyFile(fileObject.FullPath, targetPath);

                    FileInfo fi = new FileInfo(targetPath);
                    page = GetPageByName(fileObject.FileName);

                    if (page == null)
                    {
                        page = new Page(fi, FileAccess.ReadWrite);
                        page.Number = index + 1;
                        page.Index = index;
                    } else
                    {
                        page.Changed = true;
                    }
                     
                    page.Size = fileObject.FileSize;                 
                    page.LocalPath = fileObject.FullPath;
                    page.Compressed = false;
                    page.LastModified = fileObject.LastModified;
                    page.Name = fileObject.FileName;
                    page.TempPath = fi.FullName;

                    if (!page.Changed)
                    {
                        Pages.Add(page);
                    }

                    OnPageChanged(new PageChangedEvent(page, PageChangedEvent.IMAGE_STATUS_NEW));
                    OnTaskProgress(new TaskProgressEvent(page, index, Files.Count));

                    index++;
                }
                catch (Exception ef)
                {
                    MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_ERROR, ef.Message);
                }
                finally
                {
                    if (index > MaxFileIndex)
                    {
                        OnArchiveStatusChanged(new CBZArchiveStatusEvent(this, CBZArchiveStatusEvent.ARCHIVE_FILE_ADDED));

                        this.IsChanged = true;
                        MaxFileIndex = index;
                    }
                }
            }

            OnOperationFinished(new OperationFinishedEvent(index, Pages.Count));
            HandlePipelene(new PipelineEvent(this, PipelineEvent.PIPELINE_PAGES_ADDED));
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
                    localFile.FileName = fi.Name;
                    localFile.FileSize = fi.Length;
                    localFile.LastModified = fi.LastWriteTime;

                    files.Add(localFile);

                }
            } catch (Exception ex)
            {
                MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_ERROR, ex.Message);
            } finally
            {
                //
            }

            return files;
        }

        public void ParseFiles(List<String> files)
        {
            

            if (ParseAddedFileNames != null)
            {
                if (ParseAddedFileNames.IsAlive)
                {
                    ParseAddedFileNames.Abort();
                }
            }

            FileNamesToAdd.Clear();
            FileNamesToAdd.AddRange(files);

            ParseAddedFileNames = new Thread(new ThreadStart(ParseFilesProc));
            ParseAddedFileNames.Start();
        }

        public void ParseFilesProc()
        {
            OnApplicationStateChanged(new ApplicationStatusEvent(this, ApplicationStatusEvent.STATE_ANALYZING));

            int index = 0;
            foreach (String fname in FileNamesToAdd)
            {

                try
                {
                    var fi = new FileInfo(fname);

                    LocalFile localFile = new LocalFile(fi.FullName);
                    localFile.FilePath = fi.Directory.FullName;
                    localFile.FileName = fi.Name;
                    localFile.FileSize = fi.Length;
                    localFile.LastModified = fi.LastWriteTime;

                    Files.Add(localFile);

                    index++;

                    OnTaskProgress(new TaskProgressEvent(null, index, FileNamesToAdd.Count));

                    Thread.Sleep(10);
                }
                catch (Exception ex)
                {
                    MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_ERROR, ex.Message);
                }
                finally
                {
                    //MaxFileIndex = index;
                }
            }

            HandlePipelene(new PipelineEvent(this, PipelineEvent.PIPELINE_FILES_PARSED));
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
                    //PageUpdateThread.Abort();
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
            bool isUpdated = false;
            foreach (Page page in Pages)
            {
                if (page.Deleted)
                {
                    page.Index = -1;
                    page.Number = -1;
                    isUpdated = true;
                } else
                {
                    if (page.Index != newIndex)
                    {
                        isUpdated = true;
                    }
                    page.Index = newIndex;
                    page.Number = newIndex + 1;
                    newIndex++;
                }

                if (!InitialPageIndexRebuild && isUpdated)
                {
                    OnPageChanged(new PageChangedEvent(page, PageChangedEvent.IMAGE_STATUS_CHANGED));
                }
                OnTaskProgress(new TaskProgressEvent(page, updated, Pages.Count));
                OnArchiveStatusChanged(new CBZArchiveStatusEvent(this, CBZArchiveStatusEvent.ARCHIVE_FILE_UPDATED));

                this.IsChanged = true;
                isUpdated = false;
                updated++;

                Thread.Sleep(50);
            }

            MaxFileIndex = newIndex;

            OnOperationFinished(new OperationFinishedEvent(0, Pages.Count));

            InitialPageIndexRebuild = false;
        }

        public Page GetPageById(String id)
        {
            foreach (Page page1 in Pages)
            {
                if (page1.Id == id)
                {
                    return page1;
                }
            }

            return null;
        }

        public Page GetPageByName(String name)
        {
            foreach (Page page1 in Pages)
            {
                if (page1.Name == name)
                {
                    return page1;
                }
            }

            return null;
        }

        public Thread AutoRenameAllPages()
        {
            if (LoadArchiveThread != null)
            {
                if (LoadArchiveThread.IsAlive)
                {
                    //LoadArchiveThread.Abort();
                    return null;
                }
            }

            if (SaveArchiveThread != null)
            {
                if (SaveArchiveThread.IsAlive)
                {
    
                    return null;
                }
            }

            if (CloseArchiveThread != null)
            {
                if (CloseArchiveThread.IsAlive)
                {
  
                    return null;
                }
            }

            if (RestoreRenamingThread != null)
            {
                if (RestoreRenamingThread.IsAlive)
                {

                    return null;
                }
            }

            if (RenamingThread != null)
            {
                while (RenamingThread.IsAlive)
                {
                    System.Threading.Thread.Sleep(100);
                }
            }

            RenamingThread = new Thread(new ThreadStart(AutoRenameAllPagesProc));
            RenamingThread.Start();

            return RenamingThread;
        }

        public void AutoRenameAllPagesProc()
        {
            OnApplicationStateChanged(new ApplicationStatusEvent(this, ApplicationStatusEvent.STATE_RENAMING));

            foreach (Page page in Pages)
            {
                if (RenamerExcludes.IndexOf(page.Name) == -1)
                {
                    PageScriptRename(page);

                    OnTaskProgress(new TaskProgressEvent(page, page.Index + 1, Pages.Count));
                }
            }

            OnOperationFinished(new OperationFinishedEvent(0, Pages.Count));
            OnApplicationStateChanged(new ApplicationStatusEvent(this, ApplicationStatusEvent.STATE_READY));
        }

        public Thread RestoreOriginalNames()
        {
            if (LoadArchiveThread != null)
            {
                if (LoadArchiveThread.IsAlive)
                {
                    //LoadArchiveThread.Abort();
                    return null;
                }
            }

            if (SaveArchiveThread != null)
            {
                if (SaveArchiveThread.IsAlive)
                {

                    return null;
                }
            }

            if (CloseArchiveThread != null)
            {
                if (CloseArchiveThread.IsAlive)
                {

                    return null;
                }
            }

            if (RenamingThread != null)
            {
                if (RenamingThread.IsAlive)
                {

                    return null;
                }
            }

            if (RestoreRenamingThread != null)
            {
                while (RestoreRenamingThread.IsAlive)
                {
                    System.Threading.Thread.Sleep(100);
                }
            }

            RestoreRenamingThread = new Thread(new ThreadStart(RestoreOriginalNamesProc));
            RestoreRenamingThread.Start();

            return RestoreRenamingThread;
        }

        public void RestoreOriginalNamesProc()
        {
            OnApplicationStateChanged(new ApplicationStatusEvent(this, ApplicationStatusEvent.STATE_RENAMING));

            foreach (Page page in Pages)
            {
                if (RenamerExcludes.IndexOf(page.Name) == -1)
                {
                    if (page.OriginalName != null && page.OriginalName != "")
                    {
                        try
                        {
                            RenamePage(page, page.OriginalName);
                        }
                        catch (PageDuplicateNameException) { }

                        OnTaskProgress(new TaskProgressEvent(page, page.Index + 1, Pages.Count));
                    }
                }
            }

            OnOperationFinished(new OperationFinishedEvent(0, Pages.Count));
            OnApplicationStateChanged(new ApplicationStatusEvent(this, ApplicationStatusEvent.STATE_READY));
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void RenamePage(Page page, String name, bool isRestored = false)
        {
            if (name == null || name == "")
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

            page.Name = name;
            this.IsChanged = true;

            OnPageChanged(new PageChangedEvent(page, PageChangedEvent.IMAGE_STATUS_RENAMED));
            OnArchiveStatusChanged(new CBZArchiveStatusEvent(this, CBZArchiveStatusEvent.ARCHIVE_FILE_RENAMED));            
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public String PageScriptRename(Page page)
        {
            string oldName = page.Name;
            string newName = page.Name;
            string pattern;

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
                    newName = pattern;
                    foreach(String placeholder in Win_CBZSettings.Default.RenamerPlaceholders)
                    {
                        newName = newName.Replace(placeholder, ValueForPlaceholder(placeholder, page));
                    }

                    if (newName != oldName)
                    {
                        if (page.OriginalName == null || page.OriginalName == "")
                        {
                            page.OriginalName = oldName;
                        }

                        try
                        {
                            RenamePage(page, newName);
                        } catch (PageDuplicateNameException) {
                            return oldName;
                        }
                    }
                }
            }

            return newName;
        }


        public String ValueForPlaceholder(String placeholder, Page page)
        {
            switch (placeholder.ToLower())
            {
                case "{name}":
                    return page.EntryName;

                case "{ext}":
                    return page.FileExtension;

                case "{index}":
                    return page.Index.ToString();

                case "{page}":
                    return FormatLeadingZeros(page.Index + 1, Pages.Count).ToString();

                case "{pages}":
                    return Pages.Count.ToString();

                case "{size}":
                    return page.Size.ToString();

                case "{type}":
                    return TransformImageType(page.ImageType.ToString());

                default:
                    return MetaData.ValueForKey(placeholder.ToLower().Trim('{', '}'));

                    /*
                        */
            }
        }

        protected String TransformImageType(String imageType)
        {
            switch (imageType.ToLower())
            {
                case "frontcover":
                    return "cover";

                case "backcover":
                    return "cover_b_";

                case "innercover":
                    return "cover_i_";

                default:
                    return imageType;
            }
        }


        public string FormatLeadingZeros(int value, int max)
        {
            String stringValue = value.ToString();
            String stringMax = max.ToString();

            int numDigits = stringValue.Length;
            int maxDigits = stringMax.Length;

            //String.Format("{0,:D3}", value)

            return String.Format("{0,-" + maxDigits + ":D" + maxDigits +"}", value);
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

            if (CloseArchiveThread != null)
            {
                if (CloseArchiveThread.IsAlive)
                {
                    return null;
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

                    Thread.Sleep(10);
                }
            } catch (Exception e)
            {
                MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_ERROR, "Error opening Archive\n" + e.Message);
            }

            FileSize = totalSize;
            MaxFileIndex = index;

            OnArchiveStatusChanged(new CBZArchiveStatusEvent(this, CBZArchiveStatusEvent.ARCHIVE_OPENED));
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void RunRenameScriptsForPages()
        {
            OnApplicationStateChanged(new ApplicationStatusEvent(this, ApplicationStatusEvent.STATE_RENAMING));

            foreach (Page page in Pages)
            {
                if (RenamerExcludes.IndexOf(page.Name) == -1)
                {
                    RenamePageScript(page);

                    OnTaskProgress(new TaskProgressEvent(page, page.Index + 1, Pages.Count));

                    OnArchiveStatusChanged(new CBZArchiveStatusEvent(this, CBZArchiveStatusEvent.ARCHIVE_FILE_RENAMED));

                    Thread.Sleep(10);
                }
            }
        }

        protected void SaveArchiveProc()
        {            

            int index = 0;
            List<Page> deletedPages = new List<Page>();

            TemporaryFileName = MakeNewTempFileName("", ".cbz").FullName;

            ZipArchive BuildingArchive = null;
            ZipArchiveEntry updatedEntry = null;
            try
            {
                BuildingArchive = ZipFile.Open(TemporaryFileName, ZipArchiveMode.Create);

                // Apply renaming rules
                if (ApplyRenaming)
                {
                    RunRenameScriptsForPages();
                }

                OnArchiveStatusChanged(new CBZArchiveStatusEvent(this, CBZArchiveStatusEvent.ARCHIVE_SAVING));

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
                            page.FreeImage();
                            
                            updatedEntry = BuildingArchive.CreateEntryFromFile(page.TempPath, page.Name);
                            page.UpdateImageEntry(updatedEntry, MakeNewRandomId());
                            page.Changed = false;
                          
                            OnPageChanged(new PageChangedEvent(page, PageChangedEvent.IMAGE_STATUS_COMPRESSED));
                        } else
                        {
                            // collect all deleted items
                            deletedPages.Add(page);
                        }
                    }
                    catch (Exception efile)
                    {
                        MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_ERROR, "Error compressing File [" + page.TempPath +"] to Archive [" + efile.Message + "]");
                    } finally
                    {
                        Thread.Sleep(10);
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
            String newName = PageScriptRename(page);
            MetaData.UpdatePageIndexMetaDataEntry(newName, page);
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
                FileStream fs = new FileStream(inputFilePath, FileMode.Open, FileAccess.Read);
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

            int count;
            int index = 0;
            DirectoryInfo di = null;

            try
            {
                Archive = ZipFile.Open(FileName, ZipArchiveMode.Read);
                count = Archive.Entries.Count;

                try
                {
                    if (OutputDirectory != null)
                    {
                        di = new DirectoryInfo(OutputDirectory);
                    }

                    ZipArchiveEntry metaDataEntry = Archive.GetEntry("ComicInfo.xml");

                    if (metaDataEntry != null)
                    {
                        MetaData = new MetaData(metaDataEntry.Open(), metaDataEntry.FullName);
                        count--;
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
                            if (di != null)
                            {
                                fileEntry.ExtractToFile(PathHelper.ResolvePath(di.FullName) + fileEntry.Name);
                                OnItemExtracted(new ItemExtractedEvent(index, Pages.Count, PathHelper.ResolvePath(di.FullName) + fileEntry.Name));
                            }
                            else
                            {
                                fileEntry.ExtractToFile(PathHelper.ResolvePath(WorkingDir) + ProjectGUID + "\\" + fileEntry.Name);
                                page.TempPath = PathHelper.ResolvePath(WorkingDir) + ProjectGUID + "\\" + fileEntry.Name;
                                OnItemExtracted(new ItemExtractedEvent(index, Pages.Count, PathHelper.ResolvePath(WorkingDir) + ProjectGUID + "\\" + fileEntry.Name));
                            } 
                            index++;
                        } else
                        {
                            MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_ERROR, "Error extracting File from Archive [" + page.Name + "]. File no longer present in Archive!");
                        }
                    } catch (Exception efile)
                    {
                        MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_ERROR, "Error extracting File from Archive [" + efile.Message + "]");
                    } finally
                    {
                        Thread.Sleep(10);
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
            } finally
            {
                OutputDirectory = null;
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
            MaxFileIndex = 0;
            foreach (Page page in Pages)
            {
                try { 
                    page.Close();
                } catch (Exception e) 
                {
                    MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_ERROR, "Error freeing page [" + page.Name + "] with message [" + e.Message + "]");
                }
                OnPageChanged(new PageChangedEvent(page, PageChangedEvent.IMAGE_STATUS_CLOSED));
                OnTaskProgress(new TaskProgressEvent(page, page.Index, Pages.Count));
                Thread.Sleep(10);
            }

            Name = "";
            FileName = "";
            IsSaved = false;
            IsNew = false;
            IsChanged = false;
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

        protected virtual void OnApplicationStateChanged(ApplicationStatusEvent e)
        {
            ApplicationStateChanged?.Invoke(this, e);
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
