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
using Zip = System.IO.Compression.ZipArchive;
using System.Drawing;
using System.Windows.Forms.VisualStyles;
using System.Collections;
using Win_CBZ.Data;
using Win_CBZ.Tasks;
using System.Security.Principal;
using System.Windows.Controls;
using Win_CBZ.Models;
using System.Windows.Input;
using System.Drawing.Imaging;
using Path = System.IO.Path;
using Win_CBZ.Result;
using Win_CBZ.Exceptions;
using Win_CBZ.Helper;
using System.Xml.Linq;
using System.Runtime.InteropServices.ComTypes;
using static Win_CBZ.MetaData;
using System.Runtime.Versioning;

namespace Win_CBZ
{
    [SupportedOSPlatform("windows")]
    internal class ProjectModel
    {
        public String Name { get; set; }

        public String FileName { get; set; }

        public String TemporaryFileName { get; set; }

        public long FileSize { get; set; } = 0;

        public String Description { get; set; }

        public CompressionLevel CompressionLevel { get; set; }

        public Encoding FileEncoding { get; set; }

        public String WorkingDir { get; set; }

        public String OutputDirectory { get; set; }

        public int ArchiveState { get; set; }

        public int ApplicationState { get; set; }

        public String ProjectGUID { get; set; }

        public Boolean IsNew = false;

        public Boolean IsSaved = false;

        public Boolean IsChanged = false;

        //public Boolean IndexNeedsUpdate = false;

        public Boolean IsClosed = false;

        public Boolean ApplyRenaming = false;

        public Boolean CompatibilityMode = false;

        public Boolean MetaDataPageIndexMissingData = false;

        public Boolean MetaDataPageIndexFileMissing = false;

        public String RenameStoryPagePattern { get; set; }

        public String RenameSpecialPagePattern { get; set; }

        public ArrayList RenamerExcludes { get; set; }

        public bool PreloadPageImages { get; set; }

        public List<Page> Pages { get; set; }

        private int MaxFileIndex = 0;

        private bool InitialPageIndexRebuild = false;

        public MetaData MetaData { get; set; }

        public ImageTask GlobalImageTask { get; set; }

        protected Task<TaskResult> imageInfoUpdater;

        protected Task<ImageTaskResult> imageProcessingTask;

        public long TotalSize { get; set; }

        public ImageFormat OutputFormat { get; set; }

        private Zip Archive { get; set; }

        private ZipArchiveMode Mode;

        private MetaData.PageIndexVersion PageIndexVersionWriter { get; set; } = MetaData.PageIndexVersion.VERSION_1;

        DataValidation Validation;

        public event EventHandler<TaskProgressEvent> TaskProgress;

        public event EventHandler<PageChangedEvent> PageChanged;

        public event EventHandler<ApplicationStatusEvent> ApplicationStateChanged;

        public event EventHandler<CBZArchiveStatusEvent> ArchiveStatusChanged;

        public event EventHandler<MetaDataLoadEvent> MetaDataLoaded;

        public event EventHandler<MetaDataChangedEvent> MetaDataChanged;

        public event EventHandler<MetaDataEntryChangedEvent> MetaDataEntryChanged;

        public event EventHandler<OperationFinishedEvent> OperationFinished;

        public event EventHandler<ItemExtractedEvent> ItemExtracted;

        public event EventHandler<FileOperationEvent> FileOperation;

        public event EventHandler<ArchiveOperationEvent> ArchiveOperation;

        public event EventHandler<GlobalActionRequiredEvent> GlobalActionRequired;

        public event EventHandler<GeneralTaskProgressEvent> GeneralTaskProgress;

        public event EventHandler<PipelineEvent> PipelineEventHandler;

        public event EventHandler<ValidationFinishedEvent> CBZValidationEventHandler;



        private Thread LoadArchiveThread;

        private Thread ExtractArchiveThread;

        private Thread CloseArchiveThread;

        private Thread SaveArchiveThread;

        private Thread DeleteFileThread;

        private Thread PageUpdateThread;

        private Thread ProcessAddedFiles;

        private Thread ParseAddedFileNames;

        private Thread RenamingThread;

        private Thread AutoRenameThread;

        private Thread RestoreRenamingThread;

        private Thread ArchiveValidationThread;



        private CancellationTokenSource CancellationTokenSourceLoadArchive;

        private CancellationToken CancellationToken;


        public ProjectModel(String workingDir, String metaDataFilename)
        {
            WorkingDir = workingDir;
            Pages = new List<Page>();
            RenamerExcludes = new ArrayList();
            GlobalImageTask = new ImageTask();
            CompressionLevel = CompressionLevel.Optimal;
            FileEncoding = Encoding.UTF8;
            Validation = new DataValidation();
            Validation.TaskProgress += TaskProgress;

            CancellationTokenSource = new CancellationTokenSource();
            CancellationToken = CancellationTokenSource.CreateLinkedTokenSource(CancellationTokenSource.Token);

            MaxFileIndex = 0;
            Name = "";
            FileName = "";
            IsSaved = false;
            IsNew = true;
            IsChanged = false;
            IsClosed = false;

            NewMetaData(false, metaDataFilename);

            PipelineEventHandler += HandlePipeline;

            ProjectGUID = Guid.NewGuid().ToString();
            if (ProjectGUID.Length > 0)
            { 
                if (!Directory.Exists(Path.Combine(PathHelper.ResolvePath(WorkingDir), ProjectGUID)))
                {
                    try
                    {
                        DirectoryInfo di = Directory.CreateDirectory(Path.Combine(PathHelper.ResolvePath(WorkingDir), ProjectGUID));
                    }
                    catch (Exception e)
                    {
                        MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_ERROR, e.Message);
                    }
                }
            } else
            {
                ApplicationMessage.ShowWarning("System error! Failed to create new GUID", "System error");
            }
        }

        public MetaData NewMetaData(bool createDefaultValues = false, String metaDataFilename = "ComicInfo.xml")
        {
            MetaData = new MetaData(createDefaultValues, metaDataFilename);
            MetaData.MetaDataEntryChanged += MetaDataEntryChanged;

            OnMetaDataChanged(new MetaDataChangedEvent(MetaDataChangedEvent.METADATA_NEW, MetaData));

            return MetaData;
        }

        public MetaData NewMetaData(Stream fileInputStream, String metaDataFilename)
        {
            MetaData = new MetaData(fileInputStream, metaDataFilename);
            MetaData.MetaDataEntryChanged += MetaDataEntryChanged;

            OnMetaDataLoaded(new MetaDataLoadEvent(MetaData.Values));

            return MetaData;
        }

        private void HandlePipeline(object sender, PipelineEvent e)
        {
            StackItem nextTask = null;
            int currentPerformed = 0;
            List<StackItem> remainingStack = e.Stack;
            if (remainingStack.Count > 0)
            {
                nextTask = remainingStack[0];
                remainingStack.RemoveAt(0);
            }  
                     
            if (nextTask?.TaskId == PipelineEvent.PIPELINE_MAKE_PAGES)
            {
                Task.Factory.StartNew(() =>
                {

                    if (LoadArchiveThread != null)
                    {
                        if (LoadArchiveThread.IsAlive)
                        {
                            throw new ApplicationException("failed to open. thread already running!", true);
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
                            CloseArchiveThread.Join();
                        }
                    }

                    ProcessAddedFiles = new Thread(AddImagesProc);
                    ProcessAddedFiles.Start(new AddImagesThreadParams() 
                    { 
                        LocalFiles = new List<LocalFile>((IEnumerable<LocalFile>)e.Data), 
                        Stack = remainingStack
                    });

                    currentPerformed = e.Task;
                });

            }

            if (nextTask?.TaskId == PipelineEvent.PIPELINE_UPDATE_INDICES)
            {
           
                Task.Factory.StartNew(() =>
                {
                    if (MetaData.Exists())
                    {
                        MetaData.RebuildPageMetaData(Pages, (nextTask.ThreadParams as UpdatePageIndicesThreadParams).PageIndexVerToWrite);
                    }
                    UpdatePageIndices(true, true, remainingStack);
                }, (nextTask.ThreadParams as UpdatePageIndicesThreadParams).CancelToken);

                currentPerformed = e.Task;
            }

            if (nextTask?.TaskId == PipelineEvent.PIPELINE_UPDATE_IMAGE_METADATA)
            {
                if (imageInfoUpdater == null)
                {
                    imageInfoUpdater = ReadImageMetaDataTask.UpdateImageMetadata(Pages, GeneralTaskProgress);
                    imageInfoUpdater.Start();

                    currentPerformed = e.Task;
                }
                else
                {
                    if (imageInfoUpdater.IsCompleted || imageInfoUpdater.IsCanceled)
                    {
                        imageInfoUpdater = ReadImageMetaDataTask.UpdateImageMetadata(Pages, GeneralTaskProgress);
                        imageInfoUpdater.Start();

                        currentPerformed = e.Task;
                    }
                }
            }

            if (nextTask?.TaskId == PipelineEvent.PIPELINE_PROCESS_IMAGES)
            {
                //RenamePagesThreadParams p = nextTask.ThreadParams as RenamePagesThreadParams;

                if (imageProcessingTask == null)
                {
                    imageProcessingTask = ProcessImagesTask.ProcessImages(Pages, GeneralTaskProgress);
                    imageProcessingTask.Start();

                    currentPerformed = e.Task;
                }
                else
                {
                    if (imageProcessingTask.IsCompleted || imageProcessingTask.IsCanceled)
                    {
                        imageProcessingTask = ProcessImagesTask.ProcessImages(Pages, GeneralTaskProgress);
                        imageProcessingTask.Start();

                        currentPerformed = e.Task;
                    }
                }

                if (currentPerformed != e.Task)
                {
                    if (remainingStack.Count > 0)
                    {
                        nextTask = remainingStack[0];

                        OnPipelineNextTask(new PipelineEvent(this, e.Task, nextTask, remainingStack));
                    }
                }
            }

            if (nextTask?.TaskId == PipelineEvent.PIPELINE_RUN_RENAMING)
            {
                RenamePagesThreadParams p = nextTask.ThreadParams as RenamePagesThreadParams;
                p.Stack = remainingStack;
                // Apply renaming rules
                if (p.ApplyRenaming && !p.CompatibilityMode)
                {
                    try
                    {
                        RenamingThread = new Thread(RunRenameScriptsForPages);
                        RenamingThread.Start(p);
                        currentPerformed = e.Task;
                    }
                    catch (Exception ee)
                    {
                        MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_ERROR, "Error in Renamer-Script  [" + ee.Message + "]");

                    }
                }

                // Force renaming every page to its index in compatibility mode
                if (p.CompatibilityMode)
                {
                    String restoreOriginalPatternPage = RenameSpecialPagePattern;
                    String restoreOriginalPatternSpecialPage = RenameSpecialPagePattern;

                    p.RenameStoryPagePattern = "{page}.{ext}";
                    p.RenameSpecialPagePattern = "{page}.{ext}";
                    p.SkipIndexUpdate = true;

                    try
                    {
                        RenamingThread = new Thread(RunRenameScriptsForPages);
                        RenamingThread.Start(p);
                        currentPerformed = e.Task;
                    }
                    catch (Exception ee)
                    {
                        MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_ERROR, "Error in Renamer-Script  [" + ee.Message + "]");

                    }
                    finally
                    {
                        RenameStoryPagePattern = restoreOriginalPatternPage;
                        RenameSpecialPagePattern = restoreOriginalPatternSpecialPage;
                    }
                }

                if (currentPerformed != e.Task)
                {
                    if (remainingStack.Count > 0)
                    {
                        nextTask = remainingStack[0];

                        OnPipelineNextTask(new PipelineEvent(this, e.Task, nextTask, remainingStack));
                    }
                }
            }

            if (nextTask?.TaskId == PipelineEvent.PIPELINE_SAVE_ARCHIVE)
            {
                SaveArchiveThread = new Thread(SaveArchiveProc);
                SaveArchiveThread.Start(nextTask.ThreadParams);

                if (e.Payload != null)
                {
                    Task task = e.Payload.GetAttribute(PipelinePayload.PAYLOAD_EXECUTE_RENAME_SCRIPT);
                    task?.Start();

                    task = e.Payload.GetAttribute(PipelinePayload.PAYLOAD_EXECUTE_IMAGE_PROCESSING);
                    task?.Start();
                }
            }

            if (nextTask?.TaskId == PipelineEvent.PIPELINE_RUN_RENAMING)
            {
                /*
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
                        if (RenamingThread.IsAlive)
                        {
                            RenamingThread.Join();
                        }
                    }

                    RenamingThread = new Thread(AutoRenameAllPagesProc);
                    RenamingThread.Start();

                });
                */
            }

            //if (remainingStack.Count == 0)
            //{
            //    OnApplicationStateChanged(new ApplicationStatusEvent(this, ApplicationStatusEvent.STATE_READY));
            //}
        }

        public Task New()
        {
            if (ThreadRunning())
            {
                throw new ConcurrentOperationException("There are still operations running in the Background.\r\nPlease wait until those have completed and try again!", true);
            }

            CloseArchiveThread = Close();

            return Task.Factory.StartNew(() =>
            {
                CloseArchiveThread?.Join();

                //Pages.Clear();
                MetaData.Free();
                MaxFileIndex = 0;
                IsNew = true;
                IsChanged = false;
                ArchiveState = CBZArchiveStatusEvent.ARCHIVE_NEW;
                ApplicationState = ApplicationStatusEvent.STATE_READY;

                ProjectGUID = Guid.NewGuid().ToString();

                if (!Directory.Exists(Path.Combine(PathHelper.ResolvePath(WorkingDir), ProjectGUID)))
                {
                    try
                    {
                        DirectoryInfo di = Directory.CreateDirectory(Path.Combine(PathHelper.ResolvePath(WorkingDir), ProjectGUID));
                    }
                    catch (Exception e)
                    {
                        MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_ERROR, e.Message);
                    }
                }
            }, CancellationToken);
        }

        public Thread Open(String path, ZipArchiveMode mode, MetaData.PageIndexVersion currentMetaDataVersionWriting)
        {
            FileName = path;
            Mode = mode;

            if (LoadArchiveThread != null)
            {
                if (LoadArchiveThread.IsAlive)
                {
                    throw new ConcurrentOperationException("failed to open. thread already running!", true);
                }
            }

            CloseArchiveThread?.Join();

            LoadArchiveThread = new Thread(OpenArchiveProc);
            LoadArchiveThread.Start(new OpenArchiveThreadParams()
            {
                FileName = path,
                Mode = mode,
                CurrentPageIndexVer = currentMetaDataVersionWriting,
                CancelToken = CancellationToken
            });

            return LoadArchiveThread;
        }

        protected void OpenArchiveProc(object threadParams)
        {
            OpenArchiveThreadParams tParams = threadParams as OpenArchiveThreadParams;

            ArrayList missingPages = new ArrayList();
            long itemSize = 0;
            int index = 0;
            long totalSize = 0;
            MetaDataPageIndexMissingData = false;
            MetaDataPageIndexFileMissing = false;
            String IndexUpdateReasonMessage = "";
            bool MetaDataPageIndexFileMissingShown = false;

            OnArchiveStatusChanged(new CBZArchiveStatusEvent(this, CBZArchiveStatusEvent.ARCHIVE_OPENING));
            OnApplicationStateChanged(new ApplicationStatusEvent(this, ApplicationStatusEvent.STATE_OPENING));

            int countEntries = 0;
            try
            {
                Archive = ZipFile.Open(tParams.FileName, tParams.Mode);
                countEntries = Archive.Entries.Count;

                try
                {
                    ZipArchiveEntry metaDataEntry = Archive.GetEntry(MetaData.MetaDataFileName);

                    if (metaDataEntry != null)
                    {
                        MetaData = NewMetaData(metaDataEntry.Open(), metaDataEntry.FullName);
                    }
                    else
                    {
                        MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_WARNING, "No Metadata ['" + MetaData.MetaDataFileName + "'] found in Archive!");
                    }
                }
                catch (Exception)
                {
                    MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_ERROR, "Error loading Metadata ['" + MetaData.MetaDataFileName + "'] from Archive!");
                }

                MetaDataEntryPage pageIndexEntry;
                foreach (ZipArchiveEntry entry in Archive.Entries)
                {
                    if (!entry.FullName.ToLower().Contains(MetaData.MetaDataFileName.ToLower()))
                    {
                        Page page = new Page(entry, Path.Combine(PathHelper.ResolvePath(WorkingDir), ProjectGUID), RandomId.getInstance().make())
                        {
                            Number = index + 1,
                            Index = index,
                            OriginalIndex = index,
                            Size = entry.Length,
                            Hash = entry.Crc32.ToString("X"),
                            LastModified = entry.LastWriteTime
                        };
                        // too slow
                        //page.LoadImageInfo();
                        pageIndexEntry = MetaData.FindIndexEntryForPage(page, MetaData.PageIndexVersion.VERSION_2);
                        MetaData.IndexVersionSpecification = PageIndexVersion.VERSION_2;
                        if (pageIndexEntry == null)
                        {
                            pageIndexEntry = MetaData.FindIndexEntryForPage(page, MetaData.PageIndexVersion.VERSION_1);
                            MetaData.IndexVersionSpecification = PageIndexVersion.VERSION_1;

                            if (pageIndexEntry == null)
                            {

                            }
                        }

                        if (MetaData.IndexVersionSpecification != tParams.CurrentPageIndexVer && !MetaDataPageIndexFileMissingShown)
                        {
                            IndexUpdateReasonMessage = "Pageindex has invalid/outdated format! Rebuild index to update to current specifications?";
                            MetaDataPageIndexFileMissing = true;
                            MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_WARNING, "Warning! Archive page-index metadata has outdated/invalid format!");
                            MetaDataPageIndexFileMissingShown = true;
                        }

                        if (pageIndexEntry != null)
                        { 
                            try
                            {
                                page.ImageType = pageIndexEntry.GetAttribute(MetaDataEntryPage.COMIC_PAGE_ATTRIBUTE_TYPE);
                                page.Key = pageIndexEntry.GetAttribute(MetaDataEntryPage.COMIC_PAGE_ATTRIBUTE_KEY);
                                if (page.Key == null && tParams.CurrentPageIndexVer.HasFlag(PageIndexVersion.VERSION_2))
                                {
                                    page.Key = RandomId.getInstance().make();
                                    MetaDataPageIndexMissingData = true;
                                }
                                page.DoublePage = Boolean.Parse(pageIndexEntry.GetAttribute(MetaDataEntryPage.COMIC_PAGE_ATTRIBUTE_DOUBLE_PAGE) ?? "False");
                            }
                            catch
                            {
                                //MetaDataPageIndexMissingData = true;
                                //MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_WARNING, "Warning! Archive page metadata does not have image dimensions for page [" + page.Name + "]!");
                            }

                            try
                            {
                                page.Format.W = int.Parse(pageIndexEntry.GetAttribute(MetaDataEntryPage.COMIC_PAGE_ATTRIBUTE_IMAGE_WIDTH));
                                page.Format.H = int.Parse(pageIndexEntry.GetAttribute(MetaDataEntryPage.COMIC_PAGE_ATTRIBUTE_IMAGE_HEIGHT));
                            }
                            catch
                            {

                                MetaDataPageIndexMissingData = true;
                                IndexUpdateReasonMessage = "Image metadata missing from pageindex! Reload image metadata and rebuild pageindex now?";
                                MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_WARNING, "Warning! Archive page metadata does not have image dimensions for page [" + page.Name + "]!");
                            }
                        }
                        else
                        {
                            MetaDataPageIndexFileMissing = true;
                            IndexUpdateReasonMessage = "Image metadata missing from pageindex! Reload image metadata and rebuild pageindex now?";
                            MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_WARNING, "Warning! Archive page metadata missing for page [" + page.Name + "]!");
                        }

                        Pages.Add(page);

                        OnPageChanged(new PageChangedEvent(page, null, PageChangedEvent.IMAGE_STATUS_NEW));
                        OnApplicationStateChanged(new ApplicationStatusEvent(this, ApplicationStatusEvent.STATE_OPENING));
                        OnTaskProgress(new TaskProgressEvent(page, index, countEntries));

                        totalSize += page.Size;
                        index++;
                    }

                    Thread.Sleep(5);
                 
                    tParams.CancelToken.ThrowIfCancellationRequested();
                }


                // check index and compare with files
                OnApplicationStateChanged(new ApplicationStatusEvent(this, ApplicationStatusEvent.STATE_CHECKING_INDEX));
                OnTaskProgress(new TaskProgressEvent(null, 0, 100));
               
                String pageIndexName = "";
                Page pageCheck = null;
                index = 0;
                foreach (MetaDataEntryPage entry in MetaData.PageIndex)
                {
                    if (MetaData.IndexVersionSpecification == PageIndexVersion.VERSION_2)
                    {
                        pageIndexName = entry.GetAttribute(MetaDataEntryPage.COMIC_PAGE_ATTRIBUTE_IMAGE);
                    } else if (MetaData.IndexVersionSpecification == PageIndexVersion.VERSION_1)
                    {
                        pageIndexName = entry.GetAttribute(MetaDataEntryPage.COMIC_PAGE_ATTRIBUTE_KEY);
                    }

                    pageCheck = GetPageByName(pageIndexName);

                    if (pageCheck == null)
                    {
                        missingPages.Add(pageIndexName);
                    }

                    pageCheck = null;
                    OnTaskProgress(new TaskProgressEvent(null, index, MetaData.PageIndex.Count));
                    Thread.Sleep(5);
                    index++;

                    tParams.CancelToken.ThrowIfCancellationRequested();
                }

                IsChanged = false;
                IsNew = false;
            }
            catch (OperationCanceledException oce)
            {
                //Archive.Dispose();

                //OnArchiveStatusChanged(new CBZArchiveStatusEvent(this, CBZArchiveStatusEvent.ARCHIVE_CLOSED));
            }
            catch (Exception e)
            {
                MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_ERROR, "Error opening Archive\n" + e.Message);
            }


            if (!tParams.CancelToken.IsCancellationRequested)
            {
                FileSize = totalSize;
                MaxFileIndex = index;

                if (MetaDataPageIndexMissingData)
                {
                    OnGlobalActionRequired(new GlobalActionRequiredEvent(this, 0, "Image metadata missing from pageindex! Reload image metadata and rebuild pageindex now?", "Rebuild", GlobalActionRequiredEvent.TASK_TYPE_UPDATE_IMAGE_METADATA, ReadImageMetaDataTask.UpdateImageMetadata(Pages, GeneralTaskProgress)));
                }

                if (MetaDataPageIndexFileMissing)
                {
                    OnGlobalActionRequired(new GlobalActionRequiredEvent(this, 0, IndexUpdateReasonMessage, "Rebuild", GlobalActionRequiredEvent.TASK_TYPE_INDEX_REBUILD, RebuildPageIndexMetaDataTask.UpdatePageIndexMetadata(Pages, MetaData, MetaData.IndexVersionSpecification, GeneralTaskProgress, PageChanged)));
                }

                if (missingPages.Count > 0)
                {
                    MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_WARNING, "Page(s) missing from archive but are present in page-index!");
                }


                OnArchiveStatusChanged(new CBZArchiveStatusEvent(this, CBZArchiveStatusEvent.ARCHIVE_OPENED));
            }

            OnApplicationStateChanged(new ApplicationStatusEvent(this, ApplicationStatusEvent.STATE_READY));
        }

        public bool Save(bool continueOnError = false)
        {
            return SaveAs(FileName, Mode, PageIndexVersionWriter, continueOnError);      
        }

        public bool SaveAs(String path, ZipArchiveMode mode, MetaData.PageIndexVersion metaDataVersionWriting, bool continueOnError = false)
        {
            if (ThreadRunning())
            {
                throw new ConcurrentOperationException("There are still operations running in the Background.\r\nPlease wait until those have completed and try again!", true);
            }
            
            ArrayList invalidKeys = new ArrayList();

            bool tagValidationFailed;
            bool metaDataValidationFailed;
            if (Win_CBZSettings.Default.ValidateTags)
            {
                tagValidationFailed = Validation.ValidateTags();

                if (tagValidationFailed)
                {
                    OnApplicationStateChanged(new ApplicationStatusEvent(this, ApplicationStatusEvent.STATE_READY));

                    return false;
                }
            }

            metaDataValidationFailed = Validation.ValidateMetaDataDuplicateKeys(ref invalidKeys);
            if (metaDataValidationFailed)
            {
                OnApplicationStateChanged(new ApplicationStatusEvent(this, ApplicationStatusEvent.STATE_READY));

                return false;
            }

            metaDataValidationFailed = Validation.ValidateMetaDataInvalidKeys(ref invalidKeys);
            if (metaDataValidationFailed)
            {
                OnApplicationStateChanged(new ApplicationStatusEvent(this, ApplicationStatusEvent.STATE_READY));

                return false;
            }

            foreach (MetaDataEntry entry in MetaData.Values)
            {          
                MetaDataFieldConfig.GetInstance().UpdateAutoCompleteOptions(entry.Key, entry.ValueAsList());                
            }

            PageIndexVersionWriter = metaDataVersionWriting;

            OnPipelineNextTask(new PipelineEvent(
                this,
                PipelineEvent.PIPELINE_RUN_RENAMING,
                null,
                new List<StackItem>()
                {
                    new StackItem()
                    {
                        TaskId = PipelineEvent.PIPELINE_RUN_RENAMING,
                        ThreadParams = new RenamePagesThreadParams()
                        {
                            ApplyRenaming = ApplyRenaming,
                            CompatibilityMode = CompatibilityMode,
                            IgnorePageNameDuplicates = CompatibilityMode,
                            RenameStoryPagePattern = CompatibilityMode ? "" : RenameStoryPagePattern,
                            RenameSpecialPagePattern = CompatibilityMode ? "" : RenameSpecialPagePattern,
                            ContinuePipeline = true
                        }
                    },
                    new StackItem()
                    {
                        TaskId = PipelineEvent.PIPELINE_UPDATE_INDICES,
                        ThreadParams = new UpdatePageIndicesThreadParams() 
                        {
                            ContinuePipeline = true,
                            InitialIndexRebuild = false,
                            Stack = new List<StackItem>(),
                            PageIndexVerToWrite = metaDataVersionWriting,
                            CancelToken = CancellationToken
                        }
                    },
                    new StackItem()
                    {
                        TaskId = PipelineEvent.PIPELINE_SAVE_ARCHIVE,
                        ThreadParams = new SaveArchiveThreadParams()
                        {
                            FileName = path,
                            Mode = mode,
                            ContinueOnError = continueOnError,
                            CompressionLevel = CompressionLevel,
                            PageIndexVerToWrite = metaDataVersionWriting,
                            CancelToken = CancellationToken
                        }
                    }
                }
            ));

            return true;
        }

        protected void SaveArchiveProc(object threadParams)
        {
            SaveArchiveThreadParams tParams = threadParams as SaveArchiveThreadParams;

            int index = 0;
            bool errorSavingArchive = false;
            
            List<Page> deletedPages = new List<Page>();
            LocalFile fileToCompress = null;
        
            TemporaryFileName = MakeNewTempFileName(".cbz").FullName;

            ZipArchive BuildingArchive = null;
            ZipArchiveEntry updatedEntry;

            OnArchiveStatusChanged(new CBZArchiveStatusEvent(this, CBZArchiveStatusEvent.ARCHIVE_SAVING));

            // Rebuild ComicInfo.xml's PageIndex
            MetaData.RebuildPageMetaData(Pages.ToList<Page>(), tParams.PageIndexVerToWrite);

            try
            {
                BuildingArchive = ZipFile.Open(TemporaryFileName, ZipArchiveMode.Create);

                // Write files to new temporary archive
                Thread.BeginCriticalRegion();
                foreach (Page page in Pages)
                {
                    String sourceFileName = "";
                    try
                    {
                        if (!page.Deleted)
                        {
                            if (page.Compressed && (page.TemporaryFile == null || !page.TemporaryFile.Exists()))
                            {
                                FileInfo NewTemporaryFileName = MakeNewTempFileName();
                                page.CreateLocalWorkingCopy(NewTemporaryFileName.FullName);
                                if (page.TemporaryFile == null || !page.TemporaryFile.Exists())
                                {
                                    MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_WARNING, "Failed to extract or create temporary file for entry [" + page.Name + "]");
                                }
                            }

                            page.FreeImage();  // Dont delete any temporary files here! Free resource

                            if (page.Changed || page.Compressed)
                            {
                                sourceFileName = page.TemporaryFile.FullPath;
                                try
                                {
                                    if (!page.TemporaryFile.Exists())
                                    {
                                        page.CreateLocalWorkingCopy();


                                    }
                                }
                                catch (Exception)
                                {

                                    sourceFileName = page.TemporaryFile.FullPath;

                                    //MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_WARNING, "Failed to open temporary file! ["+ex.Message+"] Compressing original file [" + page.LocalFile.FullPath + "] instead of [" + page.TempPath + "]");
                                }
                            }
                            else
                            {
                                sourceFileName = page.LocalFile.FullPath;
                            }

                            fileToCompress = new LocalFile(sourceFileName);

                            updatedEntry = BuildingArchive.CreateEntryFromFile(fileToCompress.FullPath, page.Name, CompressionLevel);
                            if (IsNew)
                            {
                                page.UpdateImageEntry(updatedEntry, RandomId.getInstance().make());
                                page.Compressed = true;
                            }
                            page.Changed = false;
                            if (page.ImageLoaded)
                            {
                                try
                                {
                                    page.LoadImage();
                                }
                                catch (PageException pe)
                                {
                                    MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_WARNING, "Error reloading image [" + fileToCompress.FileName + "] for page [" + pe.Message + "]");
                                }
                            }

                            OnPageChanged(new PageChangedEvent(page, null, PageChangedEvent.IMAGE_STATUS_COMPRESSED));
                        }
                        else
                        {
                            // collect all deleted items
                            deletedPages.Add(page);
                        }
                    }
                    catch (Exception efile)
                    {
                        MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_ERROR, "Error compressing File [" + fileToCompress.FileName + "] to Archive [" + efile.Message + "]");

                        if (!Win_CBZSettings.Default.IgnoreErrorsOnSave)
                        {
                            OnArchiveStatusChanged(new CBZArchiveStatusEvent(this, CBZArchiveStatusEvent.ARCHIVE_ERROR_SAVING));
                            OnApplicationStateChanged(new ApplicationStatusEvent(this, ApplicationStatusEvent.STATE_READY));
                            errorSavingArchive = true;
                            return;
                        }
                    }
                    finally
                    {
                        Thread.Sleep(5);
                    }

                    OnArchiveOperation(new ArchiveOperationEvent(ArchiveOperationEvent.OPERATION_COMPRESS, ArchiveOperationEvent.STATUS_SUCCESS, index, Pages.Count + 1, page));

                    index++;
                }
                Thread.EndCriticalRegion();

                // Create Metadata
                try
                {
                    if (MetaData.Values.Count > 0 || MetaData.PageIndex.Count > 0)
                    {
                        MemoryStream ms = MetaData.BuildComicInfoXMLStream();
                        ZipArchiveEntry metaDataEntry = BuildingArchive.CreateEntry(MetaData.MetaDataFileName);
                        using (Stream entryStream = metaDataEntry.Open())
                        {
                            ms.CopyTo(entryStream);
                            entryStream.Close();
                            ms.Close();
                        }
                    }
                }
                catch (Exception me)
                {
                    MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_WARNING, "Error creating metadata entry in archive! No meta-information will be available. [" + me.Message + "]");
                }               
            }
            catch (Exception ex)
            {
                errorSavingArchive = true;
                MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_ERROR, "Error opening Archive for writing! [" + ex.Message + "]");
            }
            finally
            {
                if (!errorSavingArchive)
                {
                    try
                    {
                        BuildingArchive?.Dispose();
                        Archive?.Dispose();

                        CopyFile(TemporaryFileName, tParams.FileName, true);

                        int deletedIndex = 0;
                        foreach (Page deletedPage in deletedPages)
                        {
                            Pages.Remove(deletedPage);

                            OnPageChanged(new PageChangedEvent(deletedPage, null, PageChangedEvent.IMAGE_STATUS_CLOSED));
                            OnTaskProgress(new TaskProgressEvent(deletedPage, deletedIndex, deletedPages.Count));
                            deletedIndex++;
                        }

                        deletedIndex = 0;
                        if (Win_CBZ.Win_CBZSettings.Default.AutoDeleteTempFiles)
                        {
                            foreach (Page page in Pages)
                            {
                                if (page.TemporaryFile.Exists())
                                {
                                    try
                                    {
                                        page.DeleteTemporaryFile();
                                      
                                    } catch (Exception exd)
                                    {
                                        MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_ERROR, "Error finalizing CBZ [" + exd.Message + "]");
                                    }
                                }
                                OnTaskProgress(new TaskProgressEvent(page, deletedIndex, Pages.Count));
                                deletedIndex++;
                            }
                                
                        }

                    }
                    catch (Exception mvex)
                    {
                        errorSavingArchive = true;
                        MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_ERROR, "Error finalizing CBZ [" + mvex.Message + "]");
                    }
                    finally
                    {
                        if (!errorSavingArchive)
                        {
                            try
                            {
                                if (Win_CBZ.Win_CBZSettings.Default.AutoDeleteTempFiles)
                                {
                                    File.Delete(TemporaryFileName);
                                }
                                // Reopen source file and update image entries
                                Archive = ZipFile.Open(tParams.FileName, ZipArchiveMode.Read);
                                foreach (ZipArchiveEntry entry in Archive.Entries)
                                {
                                    Page page = GetPageByName(entry.Name);
                                    page?.UpdateImageEntry(entry, RandomId.getInstance().make());
                                }

                                if (Win_CBZ.Win_CBZSettings.Default.AutoDeleteTempFiles)
                                {
                                    foreach (Page page in Pages)
                                    {
                                        if (!page.TemporaryFile.Exists())
                                        {
                                            page.ThumbnailInvalidated = true;
                                        }
                                    }
                                }

                                IsChanged = false;
                                IsNew = false;
                                FileName = tParams.FileName;
                            }
                            catch (Exception rex)
                            {
                                MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_ERROR, "Error finalizing CBZ [" + rex.Message + "]");
                            }
                        }
                        else
                        {
                            
                        }
                    }
                }
            }

            if (!errorSavingArchive) 
            {
                OnArchiveStatusChanged(new CBZArchiveStatusEvent(this, CBZArchiveStatusEvent.ARCHIVE_SAVED));
            } else
            {
                OnArchiveStatusChanged(new CBZArchiveStatusEvent(this, CBZArchiveStatusEvent.ARCHIVE_ERROR_SAVING));
            }
                
            OnApplicationStateChanged(new ApplicationStatusEvent(this, ApplicationStatusEvent.STATE_READY));
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void RenamePageScript(Page page, bool ignoreDuplicates = false, bool skipIndex = false, String storyPagePattern = "", String specialPagePattern = "")
        {
            String oldName = page.Name;
            String newName = PageScriptRename(page, ignoreDuplicates, storyPagePattern, specialPagePattern);

            try
            {
                if (!skipIndex)
                {
                    MetaData.UpdatePageIndexMetaDataEntry(page, oldName, newName);
                }
                
                if (MetaDataVersionFlavorHandler.GetInstance().HandlePageIndexVersion() == PageIndexVersion.VERSION_1)
                {
                    page.Key = newName;
                }
            }
            catch (Exception ex)
            {
                MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_WARNING, "Error updating PageIndex for page [" + newName + "]. " + ex.Message);
            }
        }

        public void Extract(String outputPath = null, List<Page> pagesToExtract = null)
        {
            if (LoadArchiveThread != null)
            {
                if (LoadArchiveThread.IsAlive)
                {
                    throw new ConcurrentOperationException("Extract::Other threads are currently running", true);
                }
            }

            if (SaveArchiveThread != null)
            {
                if (SaveArchiveThread.IsAlive)
                {
                    throw new ConcurrentOperationException("Extract::Other threads are currently running", true);
                }
            }

            if (CloseArchiveThread != null)
            {
                if (CloseArchiveThread.IsAlive)
                {
                    throw new ApplicationException("Extract::Other threads are currently running", true);
                }
            }

            if (pagesToExtract == null)
            {
                pagesToExtract = Pages;
            } else {}

            ExtractArchiveThread = new Thread(ExtractArchiveProc);
            ExtractArchiveThread.Start(new ExtractArchiveThreadParams() 
            { 
                OutputPath = outputPath,
                Pages = Pages,
            });
        }

        public bool Exists()
        {
            try
            {
                FileInfo fileInfo = new FileInfo(FileName);

                return fileInfo.Exists;
            }
            catch (Exception) { return false; }
        }

        public void Validate(MetaData.PageIndexVersion pageIndexVersion, bool showErrorsDialog = false)
        {
            if (ArchiveValidationThread != null)
            {
                if (ArchiveValidationThread.IsAlive)
                {
                    throw new ConcurrentOperationException("Validation alrady running!", true);
                }
            }

            ArchiveValidationThread = new Thread(ValidateProc);
            ArchiveValidationThread.Start(new CBZValidationThreadParams() 
            { 
                ShowDialog = showErrorsDialog, 
                PageIndexVersion = pageIndexVersion 
            });

        }

        public void ValidateProc(object threadParams)
        {
            List<String> problems = new List<String>();
            ArrayList unknownTags = new ArrayList();
            ArrayList invalidKeys = new ArrayList();
            string[] duplicateTags = new string[0];
            bool hasFiles = Pages.Count > 0;
            bool hasMetaData = MetaData.Values.Count > 0;
            bool tagValidationFailed = false;
            bool keyValidationFailed = false;
            bool coverDefined = false;
            int deletedPageCount = 0;
            int progressIndex = 0;
            int maxStoryPageHeight = 0;
            int totalItemsToProcess = 0;
            Dictionary<String, int> pageTypeCounts = new Dictionary<string, int>();
            int pageTypeCountValue = 0;
            String tags = null;
            Page frontCover = null;
            bool frontPageHeightErrorLogged = false;

            CBZValidationThreadParams tParams = threadParams as CBZValidationThreadParams;
            OnApplicationStateChanged(new ApplicationStatusEvent(this, ApplicationStatusEvent.STATE_PROCESSING));

            totalItemsToProcess = Pages.Count;
            if (hasMetaData)
            {
                tags = Program.ProjectModel.MetaData.ValueForKey("Tags");
                if (tags != null && tags.Length > 0)
                {
                    totalItemsToProcess += tags.Split(',').Length;
                }
                
            }

            if (hasFiles)
            {
                foreach (Page page in Pages)
                {
                    if (page.Deleted)
                    {
                        deletedPageCount++;
                    }

                    if (!pageTypeCounts.ContainsKey(page.ImageType))
                    {
                        pageTypeCounts.Add(page.ImageType, 1);
                    } else
                    {
                        if (pageTypeCounts.TryGetValue(page.ImageType, out pageTypeCountValue))
                        {
                            pageTypeCounts[page.ImageType] = pageTypeCountValue + 1;
                        }
                    }
                   

                    if (page.Format.H == 0 || page.Format.W == 0)
                    {
                        problems.Add("Pages->Page: Invalid dimensions for page [" + page.Id + "] with [" + page.Format.W + "x" + page.Format.H + "]");
                    }

                    if (page.LocalFile != null)
                    {
                        if (!page.LocalFile.Exists())
                        {
                            problems.Add("Pages->Page: Local image file not found for page [" + page.Name + "] @(" + page.LocalFile.FullPath + ")");
                        }
                        //fileInfo.
                    }
                    else
                    {
                        if (!page.Compressed)
                        {
                            problems.Add("Pages->Page: Local image file not found for page [" + page.Name + "] @(" + page.LocalFile.FullPath + ")");
                        }
                    }

                    if (hasMetaData)
                    {
                        MetaDataEntryPage pageMeta = MetaData.FindIndexEntryForPage(page, tParams.PageIndexVersion);

                        if (pageMeta != null)
                        {
                            String metaSize = pageMeta.GetAttribute("ImageSize");
                            String metaName;

                            if (tParams.PageIndexVersion == PageIndexVersion.VERSION_2)
                            {
                                metaName = pageMeta.GetAttribute("Image");
                            } else
                            {
                                metaName = pageMeta.GetAttribute("Key");
                            }
                            
                            String metaType = pageMeta.GetAttribute("Type");
                            String metaWidth = pageMeta.GetAttribute("ImageWidth");
                            String metaHeight = pageMeta.GetAttribute("ImageHeight");

                            if (metaName != page.Name)
                            {
                                problems.Add("Metadata->PageIndex->Image: value mismatch for page [" + page.Name + "]. Rebuild index to fix.");
                            }

                            if (metaSize == null || long.Parse(metaSize) != page.Size)
                            {
                                problems.Add("Metadata->PageIndex->ImageSize: value mismatch for page [" + page.Name + "]. Rebuild index to fix.");
                            }

                            if (metaWidth == null || int.Parse(metaWidth) != page.Format.W)
                            {
                                problems.Add("Metadata->PageIndex->ImageWidth: value mismatch for page [" + page.Name + "]. Rebuild index to fix.");
                            }

                            if (metaHeight == null || int.Parse(metaHeight) != page.Format.H)
                            {
                                problems.Add("Metadata->PageIndex->ImageHeight: value mismatch for page [" + page.Name + "]. Rebuild index to fix.");
                            }

                            if (metaType == null) 
                            {
                                problems.Add("Metadata->PageIndex->Type: value type missing for page [" + page.Name + "]. Rebuild index to fix.");
                            } else 
                            { 
                                if (metaType == "FrontCover")
                                {
                                    pageTypeCounts.TryGetValue(page.ImageType, out pageTypeCountValue);
                                   
                                    coverDefined = true;
                                    frontCover = new Page(page, true, true);
                                    if (page.Index > 0 && pageTypeCountValue == 1)
                                    {
                                        problems.Add("Metadata->PageIndex->Type: value of type 'FrontCover' should be at index 0 (page 1) for page [" + page.Name + "]");
                                    } else
                                    {
                                        if (pageTypeCountValue > 1)
                                        {
                                            problems.Add("Metadata->PageIndex->Type: only 1 page of type 'FrontCover' is allowed! [" + page.Name + "]");

                                        }
                                    }

                                    // todo: check max allowed page types....
                                } else if (metaType == "Story")
                                {
                                    if (maxStoryPageHeight < page.Format.H)
                                    {
                                        maxStoryPageHeight = page.Format.H;
                                    }
                                    
                                    if (maxStoryPageHeight > 0)
                                    {
                                        if (frontCover != null && !frontPageHeightErrorLogged && frontCover.Format.H < maxStoryPageHeight)
                                        {
                                            problems.Add("Pages->Page: Height for page [" + frontCover.Id + "] of type 'FrontCover' is less than max height of page with type 'Story' [" + frontCover.Format.H + " < " + maxStoryPageHeight + "], which may result in distorted covers being generated!");
                                            frontPageHeightErrorLogged = true;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (!page.Deleted)
                            {
                                problems.Add("Metadata->PageIndex: entry missing for page [" + page.Name + "]");
                            }
                        }
                    }
                    else
                    {

                    }

                    OnTaskProgress(new TaskProgressEvent(page, progressIndex, totalItemsToProcess));
                    Thread.Sleep(5);
                    progressIndex++;
                }
            }
            else
            {
                problems.Add("Pages: No pages found in Archive [count = 0]! Nothing to display.");
            }

            if (frontCover != null)
            {
                frontCover.Close();
            }

            if (deletedPageCount == Pages.Count && deletedPageCount > 0)
            {
                problems.Add("Pages: All pages have been deleted [0 pages remaining]! Nothing to display.");
            }

            if (hasMetaData)
            {
                if (!coverDefined)
                {
                    problems.Add("Metadata->PageIndex: no Page of type 'FrontCover' defined!");
                }


                keyValidationFailed = Validation.ValidateMetaDataDuplicateKeys(ref invalidKeys, false);

                if (keyValidationFailed)
                {
                    foreach (String key in invalidKeys)
                    {
                        problems.Add("Metadata->Values: duplicate Key '" + key + "'");
                    }
                }

                keyValidationFailed = Validation.ValidateMetaDataInvalidKeys(ref invalidKeys, false);

                if (keyValidationFailed)
                {
                    foreach (String key in invalidKeys)
                    {
                        problems.Add("Metadata->Values: invalid Key '" + key + "'");
                    }
                }

                String title = Program.ProjectModel.MetaData.ValueForKey("Title");
                if (title != null)
                {
                    if (title.Length == 0)
                    {
                        problems.Add("Metadata->Values->Title: Value missing!");
                    }
                }
                else
                {
                    problems.Add("Metadata->Values->Title: Value missing!");
                }

                String writer = Program.ProjectModel.MetaData.ValueForKey("Writer");
                if (writer != null)
                {
                    if (writer.Length == 0)
                    {
                        problems.Add("Metadata->Values->Writer: Value missing!");
                    }
                }
                else
                {
                    problems.Add("Metadata->Values->Writer: Value missing!");
                }

                String lang = Program.ProjectModel.MetaData.ValueForKey("LanguageISO");
                if (lang != null)
                {
                    if (lang.Length == 0)
                    {
                        problems.Add("Metadata->Values->LanguageISO: Value missing!");
                    }
                }
                else
                {
                    problems.Add("Metadata->Values->LanguageISO missing!");
                }

                
                if (tags != null && tags.Length > 0)
                {
                    if (Win_CBZSettings.Default.ValidateTags)
                    {
                        tagValidationFailed = Validation.ValidateTags(ref unknownTags, false, true, progressIndex, totalItemsToProcess);
                        if (tagValidationFailed)
                        {
                            foreach (String tag in unknownTags)
                            {
                                problems.Add("[Metadata->Values->Tags: Unknown Tag '" + tag + "']");
                            }
                        }
                    }

                    string[] tagList = tags.Split(',');
                    duplicateTags = Validation.ValidateDuplicateStrings(tagList);

                    if (duplicateTags != null)
                    {
                        foreach (String duplicateTag in duplicateTags)
                        {
                            problems.Add("[Metadata->Values->Tags: duplicate Tag '" + duplicateTag + "']");
                        }
                    }
                }
            }
            else
            {
                problems.Add("Metadata->Values: Metadata missing! Manually add new set of Metadata.");
            }

            OnTaskProgress(new TaskProgressEvent(null, 0, 0));

            OnApplicationStateChanged(new ApplicationStatusEvent(this, ApplicationStatusEvent.STATE_READY));
            OnArchiveValidationFinished(new ValidationFinishedEvent(problems.ToArray(), tParams.ShowDialog));
        }

        public bool ThreadRunning()
        {
            if (LoadArchiveThread != null)
            {
                if (LoadArchiveThread.IsAlive)
                {
                    return true;
                }
            }

            if (ExtractArchiveThread != null)
            {
                if (ExtractArchiveThread.IsAlive)
                {
                    return true;
                }
            }

            if (PageUpdateThread != null)
            {
                if (PageUpdateThread.IsAlive)
                {
                    return true;
                }
            }

            if (RenamingThread != null)
            {
                if (RenamingThread.IsAlive)
                {
                    return true;
                }
            }

            if (CloseArchiveThread != null)
            {
                if (CloseArchiveThread.IsAlive)
                {
                    return true;
                }
            }

            if (SaveArchiveThread != null)
            {
                if (SaveArchiveThread.IsAlive)
                {
                    return true;
                }
            }

            if (PageUpdateThread != null)
            {
                if (PageUpdateThread.IsAlive)
                {
                    return true;
                }
            }

            if (ProcessAddedFiles != null)
            {
                if (ProcessAddedFiles.IsAlive)
                {
                    return true;
                }
            }

            if (ParseAddedFileNames != null)
            {
                if (ParseAddedFileNames.IsAlive)
                {
                    return true;
                }
            }

            if (RestoreRenamingThread != null)
            {
                if (RestoreRenamingThread.IsAlive)
                {
                    return true;
                }
            }

            if (AutoRenameThread != null)
            {
                if (AutoRenameThread.IsAlive)
                {
                    return true;
                }
            }

            if (ArchiveValidationThread != null)
            {
                if (ArchiveValidationThread.IsAlive)
                {
                    return true;
                }
            }

            return false;
        }

        public void CancelAllThreads()
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
                    if (RenamingThread.IsAlive)
                    {
                        RenamingThread.Abort();
                    }
                }

                if (CloseArchiveThread != null)
                {
                    if (CloseArchiveThread.IsAlive)
                    {
                        CloseArchiveThread.Abort();
                    }
                }

                if (SaveArchiveThread != null)
                {
                    if (SaveArchiveThread.IsAlive)
                    {
                        SaveArchiveThread.Abort();
                    }
                }

                if (PageUpdateThread != null)
                {
                    if (PageUpdateThread.IsAlive)
                    {
                        PageUpdateThread.Abort();
                    }
                }

                if (ProcessAddedFiles != null)
                {
                    if (ProcessAddedFiles.IsAlive)
                    {
                        ProcessAddedFiles.Abort();
                    }
                }

                if (ParseAddedFileNames != null)
                {
                    if (ParseAddedFileNames.IsAlive)
                    {
                        ParseAddedFileNames.Abort();
                    }
                }

                if (RestoreRenamingThread != null)
                {
                    if (RestoreRenamingThread.IsAlive)
                    {
                        RestoreRenamingThread.Abort();
                    }
                }

                if (ArchiveValidationThread != null)
                {
                    if (ArchiveValidationThread.IsAlive)
                    {
                        ArchiveValidationThread.Abort();
                    }
                }               
            });
        }

        public void AddImagesProc(object threadParams)
        {
            AddImagesThreadParams tParams = threadParams as AddImagesThreadParams;

            int index = MaxFileIndex;
            int realNewIndex = MaxFileIndex;
            int total = tParams?.LocalFiles.Count ?? 0;
            int progressIndex = 0;
            FileInfo targetPath;
            FileInfo localPath;
            Page page;

            if (tParams.LocalFiles != null)
            {
                foreach (LocalFile fileObject in tParams?.LocalFiles)
                {
                    try
                    {
                        localPath = new FileInfo(fileObject.FileName);
                        targetPath = MakeNewTempFileName();

                        //CopyFile(fileObject.FullPath, targetPath.FullName);

                        page = GetPageByName(fileObject.FileName);

                        if (page == null)
                        {
                            page = new Page(fileObject, targetPath.Directory.FullName, FileAccess.ReadWrite)
                            {
                                Number = realNewIndex + 1,
                                Index = realNewIndex,
                                OriginalIndex = realNewIndex,
                                Key = RandomId.getInstance().make(),
                            };
                            realNewIndex++;
                        } else
                        {
                            page.UpdateLocalWorkingCopy(fileObject, targetPath);
                            page.Key = RandomId.getInstance().make();
                            page.Changed = true;
                        }

                        try
                        {
                            page.LoadImage(true);    // dont load full image here!
                        } catch (PageException pe)
                        {
                            MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_WARNING, "Failed to load image metadata for page ['" + page.Name + "']! [" + pe.Message + "]");
                        } finally
                        {
                            page.FreeImage();  
                        }

                        if (!page.Changed)
                        {
                            Pages.Add(page);
                        }

                        OnPageChanged(new PageChangedEvent(page, null, PageChangedEvent.IMAGE_STATUS_NEW));
                        OnTaskProgress(new TaskProgressEvent(page, progressIndex, total));

                        index++;
                        progressIndex++;
                        Thread.Sleep(5);
                    }
                    catch (Exception ef)
                    {
                        MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_ERROR, ef.Message);
                    }
                    finally
                    {
                        if (realNewIndex > MaxFileIndex)
                        {
                            OnArchiveStatusChanged(new CBZArchiveStatusEvent(this, CBZArchiveStatusEvent.ARCHIVE_FILE_ADDED));

                            IsChanged = true;
                            MaxFileIndex = realNewIndex;
                        }
                    }
                }
            }

            OnOperationFinished(new OperationFinishedEvent(progressIndex, Pages.Count));
            OnApplicationStateChanged(new ApplicationStatusEvent(this, ApplicationStatusEvent.STATE_READY));
            OnPipelineNextTask(new PipelineEvent(this, PipelineEvent.PIPELINE_MAKE_PAGES, null, tParams.Stack));            
        }

        public List<String> LoadDirectory(String path)
        {
            List<String> files = new List<String>();

            DirectoryInfo di = new DirectoryInfo(path);

            try
            {
                foreach (var fi in di.EnumerateFiles())
                {
                    files.Add(fi.FullName);

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
                    throw new ApplicationException("File Analyzer- Thread already running!", true);
                }
            }

            ParseAddedFileNames = new Thread(ParseFilesProc);
            ParseAddedFileNames.Start(new ParseFilesThreadParams() 
            { 
                FileNamesToAdd = files,
                Stack = null,
            });
        }

        public void ParseFilesProc(object threadParams)
        {
            ParseFilesThreadParams tParams = threadParams as ParseFilesThreadParams;

            OnApplicationStateChanged(new ApplicationStatusEvent(this, ApplicationStatusEvent.STATE_ANALYZING));

            List<LocalFile> files = new List<LocalFile>();
            int index = 0;
            foreach (String fname in tParams.FileNamesToAdd)
            {

                try
                {
                    files.Add(new LocalFile(fname));
                    index++;

                    OnTaskProgress(new TaskProgressEvent(null, index, tParams.FileNamesToAdd.Count));

                    Thread.Sleep(5);
                }
                catch (Exception ex)
                {
                    MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_ERROR, ex.Message);
                }
                finally
                {
                    tParams.Stack = new List<StackItem>()
                    {
                        new StackItem
                        {
                            TaskId = PipelineEvent.PIPELINE_MAKE_PAGES,
                            ThreadParams = new AddImagesThreadParams
                            {
                                LocalFiles = files.ToList(),
                            }
                        },
                        new StackItem
                        {
                            TaskId = MetaData.Exists() ? PipelineEvent.PIPELINE_UPDATE_INDICES : -1,
                            ThreadParams = new UpdatePageIndicesThreadParams()
                            {
                                ContinuePipeline = true,
                                InitialIndexRebuild = false,
                            }
                        }
                    };
                }
            }

            OnTaskProgress(new TaskProgressEvent(null, 0, tParams.FileNamesToAdd.Count));
            OnApplicationStateChanged(new ApplicationStatusEvent(this, ApplicationStatusEvent.STATE_READY));
            OnPipelineNextTask(new PipelineEvent(this, PipelineEvent.PIPELINE_PARSE_FILES, files, tParams.Stack));          
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

        public void UpdatePageIndices(bool initialIndexBulid, bool continuePipeline = false, List<StackItem> stack = null)
        {
            if (LoadArchiveThread != null)
            {
                if (LoadArchiveThread.IsAlive)
                {
                    return;
                }
            }

            if (PageUpdateThread != null)
            {
                if (PageUpdateThread.IsAlive)
                {
                    PageUpdateThread.Join();
                }
            }

            PageUpdateThread = new Thread(UpdatePageIndicesProc);
            PageUpdateThread.Start(new UpdatePageIndicesThreadParams() 
            { 
                ContinuePipeline = continuePipeline,
                InitialIndexRebuild = initialIndexBulid,
                Stack = stack ?? new List<StackItem> { new StackItem() { TaskId = 0, ThreadParams = null } }
            });
        }

        protected void UpdatePageIndicesProc(object threadParams)
        {
            UpdatePageIndicesThreadParams tparams = threadParams as UpdatePageIndicesThreadParams;

            int newIndex = 0;
            int updated = 1;
            bool isUpdated = false;

            OnApplicationStateChanged(new ApplicationStatusEvent(this, ApplicationStatusEvent.STATE_UPDATING_INDEX));

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
                    page.OriginalIndex = newIndex;
                    page.Number = newIndex + 1;
                    newIndex++;
                }

                if (!tparams.InitialIndexRebuild && isUpdated)
                {
                    OnPageChanged(new PageChangedEvent(page, null, PageChangedEvent.IMAGE_STATUS_CHANGED));
                }

                OnGeneralTaskProgress(new GeneralTaskProgressEvent(GeneralTaskProgressEvent.TASK_UPDATE_PAGE_INDEX, GeneralTaskProgressEvent.TASK_STATUS_RUNNING, "Rebuilding index...", updated, Pages.Count, true));
                //OnArchiveStatusChanged(new CBZArchiveStatusEvent(this, CBZArchiveStatusEvent.ARCHIVE_FILE_UPDATED));

                IsChanged = true;
                isUpdated = false;
                updated++;

                Thread.Sleep(5);
            }

            MaxFileIndex = newIndex;

            OnOperationFinished(new OperationFinishedEvent(0, Pages.Count));
            OnApplicationStateChanged(new ApplicationStatusEvent(this, ApplicationStatusEvent.STATE_READY));

            Thread.Sleep(50);

            if (tparams.ContinuePipeline || tparams.Stack.Count == 0)
            {
                OnArchiveStatusChanged(new CBZArchiveStatusEvent(this, CBZArchiveStatusEvent.ARCHIVE_READY));
            }

            if (tparams.ContinuePipeline)
            {
                OnPipelineNextTask(new PipelineEvent(this, PipelineEvent.PIPELINE_UPDATE_INDICES, null, tparams.Stack));
            }         
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

        public Page GetPageByIndex(int index)
        {
            foreach (Page page1 in Pages)
            {
                if (page1.Index == index)
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

        public Page GetPageByKey(String key)
        {
            foreach (Page page1 in Pages)
            {
                if (page1.Key == key)
                {
                    return page1;
                }
            }

            return null;
        }

        public List<Page> GetPagesByKey(String key)
        {
            List<Page> pages = new List<Page>();

            foreach (Page page1 in Pages)
            {
                if (page1.Key == key)
                {
                    pages.Add(page1);
                }
            }

            return pages;
        }

        public Page GetNextAvailablePage(int index, int direction = 1)
        {
            int startIndex = index;
            bool condition = false;
            foreach (Page page in Pages)
            {
                if (direction == 1)
                {
                    condition = page.Index == index + direction && page.Index > -1;
                } else if (direction == -1)
                {
                    condition = page.Index == index + direction && page.Index > -1;
                }

                if (condition)
                {
                    if (!page.Closed && !page.Deleted)
                    {
                        return page;
                    }
                    index++;
                }
            }

            return null;
        }

        public void AutoRenameAllPages()
        {
            if (LoadArchiveThread != null)
            {
                if (LoadArchiveThread.IsAlive)
                {
                    //LoadArchiveThread.Abort();
                    throw new ConcurrentOperationException("Other operations still running!", true);
                }
            }

            if (SaveArchiveThread != null)
            {
                if (SaveArchiveThread.IsAlive)
                {

                    throw new ConcurrentOperationException("Other operations still running!", true);
                }
            }

            if (CloseArchiveThread != null)
            {
                if (CloseArchiveThread.IsAlive)
                {

                    throw new ConcurrentOperationException("Other operations still running!", true);
                }
            }

            if (RestoreRenamingThread != null)
            {
                if (RestoreRenamingThread.IsAlive)
                {

                    throw new ConcurrentOperationException("Other operations still running!", true);
                }
            }

            if (RenamingThread != null)
            {
                if (RenamingThread.IsAlive)
                {
                    throw new ConcurrentOperationException("Other operations still running!", true);
                }
            }

            if (AutoRenameThread != null)
            {
                while (AutoRenameThread.IsAlive)
                {
                    System.Threading.Thread.Sleep(100);
                }
            }

            AutoRenameThread = new Thread(AutoRenameAllPagesProc);
            AutoRenameThread.Start(new RenamePagesThreadParams()
            {
                ApplyRenaming = true,
                CompatibilityMode = false,
                IgnorePageNameDuplicates = false,
                RenameStoryPagePattern = RenameStoryPagePattern,
                RenameSpecialPagePattern = RenameSpecialPagePattern,
                ContinuePipeline = false,
                Stack = new List<StackItem> { new StackItem() }
            });
        }

        public void AutoRenameAllPagesProc(object threadParams)
        {
            OnApplicationStateChanged(new ApplicationStatusEvent(this, ApplicationStatusEvent.STATE_RENAMING));

            RenamePagesThreadParams tParams = threadParams as RenamePagesThreadParams;

            foreach (Page page in Pages)
            {
                if (RenamerExcludes.IndexOf(page.Name) == -1 && !page.Deleted)
                {
                    PageScriptRename(page, tParams.IgnorePageNameDuplicates, tParams.RenameStoryPagePattern, tParams.RenameSpecialPagePattern);

                    OnTaskProgress(new TaskProgressEvent(page, page.Index + 1, Pages.Count));
                }
            }

            OnOperationFinished(new OperationFinishedEvent(0, Pages.Count));
            OnApplicationStateChanged(new ApplicationStatusEvent(this, ApplicationStatusEvent.STATE_READY));
        }

        public void RestoreOriginalNames()
        {
            if (ThreadRunning())
            {
                throw new ConcurrentOperationException("Please wait until other operations are finished!", true);
            }

            if (LoadArchiveThread != null)
            {
                if (LoadArchiveThread.IsAlive)
                {
                    throw new ConcurrentOperationException("Invalid operation", true);
                }
            }

            if (SaveArchiveThread != null)
            {
                if (SaveArchiveThread.IsAlive)
                {

                    throw new ConcurrentOperationException("Invalid operation", true);
                }
            }

            if (CloseArchiveThread != null)
            {
                if (CloseArchiveThread.IsAlive)
                {
                    throw new ConcurrentOperationException("Invalid operation", true);
                }
            }

            if (RenamingThread != null)
            {
                if (RenamingThread.IsAlive)
                {
                    throw new ConcurrentOperationException("Invalid operation", true);
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
                            page.Renamed = false;
                        }
                        catch (PageDuplicateNameException) { }

                        OnTaskProgress(new TaskProgressEvent(page, page.Index + 1, Pages.Count));
                    }
                }
            }

            OnOperationFinished(new OperationFinishedEvent(0, Pages.Count));
            OnApplicationStateChanged(new ApplicationStatusEvent(this, ApplicationStatusEvent.STATE_READY));
        }

        
        public void RenamePage(Page page, String name, bool ignoreDuplicates = false)
        {
            if (name == null || name == "")
            {
                throw new PageException(page, "Failed to rename page ['" + page.Name + "'] with ID [" + page.Id + "]! The new name must not be NULL.");
            }

            if (!ignoreDuplicates)
            {
                foreach (Page existingPage in Pages)
                {
                    if (existingPage.Id != page.Id)
                    {
                        if (existingPage.Name.ToLower().Equals(name.ToLower()))
                        {
                            throw new PageDuplicateNameException(page, "Failed to rename page ['" + page.Name + "'] with ID [" + page.Id + "]! A different page with the same name already exists at Index " + existingPage.Index + ".", true);
                        }
                    }
                }
            }

            //Page oldPage = new Page(page);  // dont create new page obj - file will get locked

            if (!page.Name.Equals(name))
            {
                page.Name = name;
                if (MetaDataVersionFlavorHandler.GetInstance().HandlePageIndexVersion() == PageIndexVersion.VERSION_2)
                {
                    page.Key = name;
                }
                IsChanged = true;

                OnPageChanged(new PageChangedEvent(page, null, PageChangedEvent.IMAGE_STATUS_RENAMED));
                OnArchiveStatusChanged(new CBZArchiveStatusEvent(this, CBZArchiveStatusEvent.ARCHIVE_FILE_RENAMED));
            }          
        }

        
        public String PageScriptRename(Page page, bool ignoreDuplicates = false, String renameStoryPattern = "", String renameSpecialPattern = "")
        {
            string oldName = page.Name;
            string newName = page.Name;
            string pattern;

            switch (page.ImageType)
            {
                case MetaDataEntryPage.COMIC_PAGE_TYPE_STORY:
                    pattern = renameStoryPattern;
                    break;
                default:
                    pattern = renameSpecialPattern;
                    break;
            }

            if (pattern != null)
            {
                if (pattern.Length > 0)
                {
                    newName = pattern;
                    foreach (String placeholder in Win_CBZSettings.Default.RenamerPlaceholders)
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
                            RenamePage(page, newName, ignoreDuplicates);
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
                    return page.FileExtension.TrimStart('.');

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
            //String stringValue = value.ToString();
            String stringMax = max.ToString();

            //int numDigits = stringValue.Length;
            int maxDigits = stringMax.Length;

            //String.Format("{0,:D3}", value)

            return String.Format("{0,-" + maxDigits + ":D" + maxDigits + "}", value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>DEPRECATED</remarks>
        /// <param name="page"></param>
        /// <returns></returns>
        public String RequestTemporaryFile(Page page)
        {
            String tempFileName = MakeNewTempFileName().FullName;
            if (page.Compressed)
            {
                ExtractSingleFile(page, tempFileName);
            } else
            {
                page.CreateLocalWorkingCopy(tempFileName);
            }

            return tempFileName;
        }
        
        public void RunRenameScriptsForPages(object threadParams)
        {
            RenamePagesThreadParams tParams = threadParams as RenamePagesThreadParams;

            OnApplicationStateChanged(new ApplicationStatusEvent(this, ApplicationStatusEvent.STATE_RENAMING));

            foreach (Page page in Pages)
            {
                if (!page.Deleted)
                {
                    if (tParams.CompatibilityMode && !PageNameEqualsIndex(page) && RenamerExcludes.IndexOf(page.Name) == -1)
                    {
                        RenamePageScript(page, tParams.IgnorePageNameDuplicates, tParams.SkipIndexUpdate, tParams.RenameStoryPagePattern, tParams.RenameSpecialPagePattern);

                        OnTaskProgress(new TaskProgressEvent(page, page.Index + 1, Pages.Count));
                        OnArchiveStatusChanged(new CBZArchiveStatusEvent(this, CBZArchiveStatusEvent.ARCHIVE_FILE_RENAMED));

                        Thread.Sleep(5);
                    }
                }
            }

            if (tParams.ContinuePipeline)
            {
                OnPipelineNextTask(new PipelineEvent(this, PipelineEvent.PIPELINE_RUN_RENAMING, null, tParams.Stack, null));
            }

            OnApplicationStateChanged(new ApplicationStatusEvent(this, ApplicationStatusEvent.STATE_READY));
        }

        public bool PageNameEqualsIndex(Page page)
        {
            try
            {
                String name = page.Name.Replace(page.FileExtension, "");
                int pageNumber = 0;

                var isNummeric = int.TryParse(name, out pageNumber);
                return pageNumber.Equals(page.Number);
            } catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>DEPRECATED</remarks>
        /// <param name="page"></param>
        /// <param name="path"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void ExtractSingleFile(Page page, String path = null)
        {
            try
            {
                ZipArchiveEntry fileEntry = Archive.GetEntry(page.EntryName);
                if (fileEntry != null)
                {
                    FileInfo NewTemporaryFileName = MakeNewTempFileName();
                    fileEntry.ExtractToFile(NewTemporaryFileName.FullName);
                    //page.TempPath = NewTemporaryFileName.FullName;
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

        public FileInfo MakeNewTempFileName(String extension = "")
        {
            return new FileInfo(Path.Combine(PathHelper.ResolvePath(WorkingDir), ProjectGUID, RandomId.getInstance().make() + extension + ".tmp"));
        }

        protected DirectoryInfo MakeTempDirectory(String name = "_tmp")
        {
            return Directory.CreateDirectory(Path.Combine(PathHelper.ResolvePath(WorkingDir), ProjectGUID, name, RandomId.getInstance().make()));

            //return new FileInfo(PathHelper.ResolvePath(WorkingDir) + ProjectGUID + "\\" + MakeNewRandomId() + extension + ".tmp");
        }

        protected bool RemoveDirectoryReadOnlyAttribute(ref DirectoryInfo directoryInfo)
        {
            FileAttributes fileAttributes = directoryInfo.Attributes & ~FileAttributes.ReadOnly;
            //Directory.SetAttributes(fileAttributes);
            directoryInfo.Attributes = fileAttributes;

            return !directoryInfo.Attributes.HasFlag(FileAttributes.ReadOnly);
        }

        protected void SetDirectoryAccessControl(ref DirectoryInfo directoryInfo)
        {
            DirectorySecurity dSecurity = directoryInfo.GetAccessControl();
            dSecurity.AddAccessRule(new FileSystemAccessRule(new SecurityIdentifier(WellKnownSidType.AuthenticatedUserSid, null), FileSystemRights.FullControl,
                                                             InheritanceFlags.ObjectInherit | InheritanceFlags.ContainerInherit,
                                                             PropagationFlags.InheritOnly, AccessControlType.Allow));
            directoryInfo.SetAccessControl(dSecurity);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void CopyFile(string inputFilePath, string outputFilePath, bool propagateEvents = false)
        {
            int bufferSize = 1024 * 1024;

            using (FileStream fileStream = new FileStream(outputFilePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite))
            {
                FileStream fs = new FileStream(inputFilePath, FileMode.Open, FileAccess.Read);
                fileStream.SetLength(fs.Length);
                int bytesRead = -1;
                long byesTotal = 0;
                byte[] bytes = new byte[bufferSize];

                while ((bytesRead = fs.Read(bytes, 0, bufferSize)) > 0)
                {
                    fileStream.Write(bytes, 0, bytesRead);
                    byesTotal += bytesRead;
                    // no progres tracking here atm, since this will fuckup the overall progressbar
                    if (propagateEvents)
                    {
                        OnFileOperation(new FileOperationEvent(FileOperationEvent.OPERATION_COPY, FileOperationEvent.STATUS_RUNNING, byesTotal, fs.Length));
                    }
                }

                fs.Close();
            }

            if (propagateEvents)
            {
                OnFileOperation(new FileOperationEvent(FileOperationEvent.OPERATION_COPY, FileOperationEvent.STATUS_SUCCESS, 0, 100));
            }
        }


        protected void ExtractArchiveProc(object threadParams)
        {
            ExtractArchiveThreadParams tparams = threadParams as ExtractArchiveThreadParams;

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
                    if (tparams.OutputPath != null)
                    {
                        di = new DirectoryInfo(tparams.OutputPath);
                    }

                    ZipArchiveEntry metaDataEntry = Archive.GetEntry("ComicInfo.xml");

                    if (metaDataEntry != null)
                    {
                        MetaData = NewMetaData(metaDataEntry.Open(), metaDataEntry.FullName);
                        count--;

                    }
                }
                catch (Exception)
                { }

                ZipArchiveEntry fileEntry;
                foreach (Page page in tparams.Pages)
                {
                    try
                    {
                        
                        fileEntry = Archive.GetEntry(page.EntryName);  // use original entry name only
                        if (fileEntry != null)
                        {
                            if (di != null)
                            {
                                fileEntry.ExtractToFile(Path.Combine(PathHelper.ResolvePath(di.FullName), fileEntry.Name), true);
                                OnItemExtracted(new ItemExtractedEvent(index, Pages.Count, Path.Combine(PathHelper.ResolvePath(di.FullName), fileEntry.Name)));
                            }
                            else
                            {
                                fileEntry.ExtractToFile(Path.Combine(PathHelper.ResolvePath(WorkingDir), ProjectGUID, fileEntry.Name), true);
                                page.TemporaryFile = new LocalFile(Path.Combine(PathHelper.ResolvePath(WorkingDir), ProjectGUID, fileEntry.Name));
                                OnItemExtracted(new ItemExtractedEvent(index, Pages.Count, Path.Combine(PathHelper.ResolvePath(WorkingDir), ProjectGUID, fileEntry.Name)));
                            } 
                            
                            
                        } else
                        {
                            MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_ERROR, "Error extracting File from Archive [" + page.Name + "]. File no longer present in Archive!");
                        }
                    } catch (Exception efile)
                    {
                        MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_ERROR, "Error extracting File from Archive [" + efile.Message + "]");
                    } finally
                    {
                        OnTaskProgress(new TaskProgressEvent(page, index, Pages.Count));
                        index++;
                        Thread.Sleep(5);
                    }

                }
 
                /*  Bulk Extract??!?
                foreach (ZipArchiveEntry entry in Archive.Entries)
                {
                    if (!entry.FullName.ToLower().Contains("comicinfo.xml"))
                    {
                        entry.ExtractToFile(Path.Combine(PathHelper.ResolvePath(WorkingDir), ProjectGUID, entry.Name));
                        OnItemExtracted(new ItemExtractedEvent(index, count, Path.Combine(PathHelper.ResolvePath(WorkingDir), ProjectGUID, entry.Name)));
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

        public Thread Close()
        {
            if (LoadArchiveThread != null)
            {
                if (LoadArchiveThread.IsAlive)
                {
                    LoadArchiveThread.Join();
                }
            }

            if (SaveArchiveThread != null)
            {
                if (SaveArchiveThread.IsAlive)
                {
                    SaveArchiveThread.Join();
                }
            }

            if (CloseArchiveThread != null)
            {
                if (CloseArchiveThread.IsAlive)
                {
                    return CloseArchiveThread;
                }
            }

            CloseArchiveThread = new Thread(new ThreadStart(CloseArchiveProc));
            CloseArchiveThread.Start();

            return CloseArchiveThread;
        }

        protected void CloseArchiveProc()
        {
            OnArchiveStatusChanged(new CBZArchiveStatusEvent(this, CBZArchiveStatusEvent.ARCHIVE_CLOSING));

            MetaData.Free();

            FileSize = 0;
            FileName = "";
            MaxFileIndex = 0;
            Thread.BeginCriticalRegion();

            lock (Pages)
            {
                try
                {
                    foreach (Page page in Pages)
                    {
                        try
                        {
                            page.Close(!Win_CBZSettings.Default.AutoDeleteTempFiles);
                        }
                        catch (Exception e)
                        {
                            MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_ERROR, "Error freeing page [" + page.Name + "] with message [" + e.Message + "]");
                        }
                        finally
                        {
                            OnPageChanged(new PageChangedEvent(page, null, PageChangedEvent.IMAGE_STATUS_CLOSED));
                            OnArchiveStatusChanged(new CBZArchiveStatusEvent(this, CBZArchiveStatusEvent.ARCHIVE_CLOSING));
                            OnTaskProgress(new TaskProgressEvent(page, page.Index, Pages.Count));
                            Thread.Sleep(5);
                        }
                    }
                }
                catch (Exception e)
                {
                    MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_ERROR, "Error closing Archive [" + e.Message + "]");
                }               
            }

            Thread.EndCriticalRegion();

            Name = "";
            FileName = "";
            IsSaved = false;
            IsNew = true;
            IsChanged = false;
            IsClosed = false;

            Archive?.Dispose();

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

        public string SizeFormat(long value)
        {
            double size = value;
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

        public void DeleteTempFolderItems()
        {
            List<string> filesToDelete = new List<string>();
            List<string> foldersToDelete = new List<string>();
            String path = PathHelper.ResolvePath(WorkingDir);
            int fileIndex = 0;
            int filesDeletedCount = 0;
            int filesFailed = 0;
            long totalSize = 0;

            OnApplicationStateChanged(new ApplicationStatusEvent(this, ApplicationStatusEvent.STATE_PROCESSING));

            List<FileInfo> files = new List<FileInfo>();

            DirectoryInfo dir = new DirectoryInfo(path);

            List<DirectoryInfo> folders = new List<DirectoryInfo>(dir.EnumerateDirectories());


            try
            {
                if (dir.Exists && ProjectGUID != null && ProjectGUID.Length > 0)
                {
                    // fail safe! are we in the right directory? if not we would delete random directories and files!
                    if (dir.Parent.Name.ToString().ToLower().Equals(Win_CBZSettings.Default.AppName.ToLower()) &&
                        dir.Parent.Parent.Name.ToString().ToLower().Equals("roaming"))
                    {

                        foreach (DirectoryInfo folder in folders)
                        {
                            if (folder.Name != ProjectGUID)
                            {
                                files.AddRange(folder.GetFiles());
                                foldersToDelete.Add(folder.FullName);
                            }
                        }

                        

                        foreach (FileInfo file in files)
                        {
                            if (file.Exists)
                            {
                                try
                                {
                                    file.Delete();
                                    totalSize += file.Length;
                                    filesDeletedCount++;
                                }
                                catch
                                {
                                    filesFailed++;
                                }
                            }

                            OnGeneralTaskProgress(new GeneralTaskProgressEvent(GeneralTaskProgressEvent.TASK_DELETE_FILE, GeneralTaskProgressEvent.TASK_STATUS_RUNNING, "Clearing Cache...", fileIndex, files.Count, false));
                            Thread.Sleep(5);
                            fileIndex++;
                        }

                        foreach (DirectoryInfo folder in folders)
                        {
                            if (folder.Name != ProjectGUID)
                            {
                                folder.Delete(false);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_ERROR, "Error deleting cache file [" + e.Message + "]");
            }
            finally
            {
                ApplicationMessage.Show("Applicaiton cache cleared.\r\nFiles deleted: " + filesDeletedCount.ToString() + ",\r\nFiles failed/skipped: " + filesFailed.ToString() + "\r\nDisk space reclaimed: " + SizeFormat(totalSize), "Application Cache cleared", ApplicationMessage.DialogType.MT_INFORMATION, ApplicationMessage.DialogButtons.MB_OK);
            }

            OnGeneralTaskProgress(new GeneralTaskProgressEvent(GeneralTaskProgressEvent.TASK_DELETE_FILE, GeneralTaskProgressEvent.TASK_STATUS_COMPLETED, "", 0, 100, false));
            OnApplicationStateChanged(new ApplicationStatusEvent(this, ApplicationStatusEvent.STATE_READY));
        }

        public void CopyTo(ProjectModel destination)
        {
            if (destination != null)
            {
                Page[] copyPages = new Page[Pages.Count];

                Pages.CopyTo(copyPages, 0);
                destination.Pages = new List<Page>(copyPages);
                destination.MetaData = this.MetaData;
                destination.Name = this.Name;
                destination.ProjectGUID = this.ProjectGUID;   // should be new id!
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

        protected virtual void OnArchiveValidationFinished(ValidationFinishedEvent e)
        {
            CBZValidationEventHandler?.Invoke(this, e);
        }

        protected virtual void OnPipelineNextTask(PipelineEvent e)
        {
            PipelineEventHandler?.Invoke(this, e);
        }

        protected virtual void OnGlobalActionRequired(GlobalActionRequiredEvent e)
        {
            GlobalActionRequired?.Invoke(this, e);
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

        protected virtual void OnMetaDataChanged(MetaDataChangedEvent e)
        {
            MetaDataChanged?.Invoke(this, e);
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

        protected virtual void OnGeneralTaskProgress(GeneralTaskProgressEvent e)
        {
            GeneralTaskProgress?.Invoke(this, e);
        }
    }
}
