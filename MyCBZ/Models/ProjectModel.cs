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
using File = System.IO.File;
using Zip = System.IO.Compression.ZipArchive;
using System.Drawing;
using System.Windows.Forms.VisualStyles;
using System.Collections;
using Win_CBZ.Data;
using Win_CBZ.Tasks;
using System.Security.Principal;
using Win_CBZ.Models;
using System.Windows.Input;
using System.Drawing.Imaging;
using Path = System.IO.Path;
using Win_CBZ.Result;
using Win_CBZ.Exceptions;
using Win_CBZ.Helper;
using System.Xml.Linq;
using static Win_CBZ.MetaData;
using System.Runtime.Versioning;
using Win_CBZ.Handler;
using SharpCompress.Compressors.Xz;
using Win_CBZ.Events;
using System.Text.RegularExpressions;
using Win_CBZ.Hash;
using System.Drawing.Drawing2D;
using System.Numerics;
using System.Speech.Recognition;
using static ScintillaNET.Style;

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

        public ArrayList ConversionExcludes { get; set; }

        public List<string> FilteredFileNames { get; set; }

        public bool PreloadPageImages { get; set; }

        public List<Page> Pages { get; set; }

        private int MaxFileIndex = 0;

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

        public ProjectModel(String workingDir, String metaDataFilename)
        {
            WorkingDir = workingDir;
            Pages = new List<Page>();
            RenamerExcludes = new ArrayList();
            FilteredFileNames = new List<string>();
            ConversionExcludes = new ArrayList();
            GlobalImageTask = new ImageTask("");
            CompressionLevel = CompressionLevel.Optimal;
            FileEncoding = Encoding.UTF8;
            Validation = new DataValidation();
            Validation.OnTaskProgress += AppEventHandler.OnTaskProgress;          

            MaxFileIndex = 0;
            Name = "";
            FileName = "";
            IsSaved = false;
            IsNew = true;
            IsChanged = false;
            IsClosed = false;

            NewMetaData(false, metaDataFilename);

            AppEventHandler.PipelineEventHandler += HandlePipeline;

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
            MetaData.MetaDataEntryChanged += AppEventHandler.OnMetaDataEntryChanged;

            AppEventHandler.OnMetaDataChanged(this, new MetaDataChangedEvent(MetaDataChangedEvent.METADATA_NEW, MetaData));

            return MetaData;
        }

        public MetaData NewMetaData(Stream fileInputStream, String metaDataFilename)
        {
            MetaData = new MetaData(fileInputStream, metaDataFilename);
            MetaData.MetaDataEntryChanged += AppEventHandler.OnMetaDataEntryChanged;

            AppEventHandler.OnMetaDataLoaded(this, new MetaDataLoadEvent(MetaData.Values.ToList()));

            return MetaData;
        }
        
        private void HandlePipeline(object sender, PipelineEvent e)
        {
            StackItem nextTask = null;
          
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
                            throw new ConcurrentOperationException("failed to open. thread already running!", true);
                        }
                    }

                    if (ExtractArchiveThread != null)
                    {
                        if (ExtractArchiveThread.IsAlive)
                        {
                            TokenStore.GetInstance().CancellationTokenSourceForName(TokenStore.TOKEN_SOURCE_EXTRACT_ARCHIVE).Cancel();
                        }
                    }

                    if (PageUpdateThread != null)
                    {
                        if (PageUpdateThread.IsAlive)
                        {
                            TokenStore.GetInstance().CancellationTokenSourceForName(TokenStore.TOKEN_SOURCE_UPDATE_PAGE).Cancel();
                        }
                    }

                    if (CloseArchiveThread != null)
                    {
                        while (CloseArchiveThread.IsAlive)
                        {
                            CloseArchiveThread.Join();
                        }
                    }

                    TokenStore.GetInstance().ResetCancellationToken(TokenStore.TOKEN_SOURCE_PROCESS_FILES);

                    AddImagesThreadParams p = nextTask.ThreadParams as AddImagesThreadParams;

                    ProcessAddedFiles = new Thread(AddImagesProc);
                    ProcessAddedFiles.Start(new AddImagesThreadParams()
                    {
                        LocalFiles = new List<LocalFile>((IEnumerable<LocalFile>)e.Data),
                        Stack = remainingStack,
                        InvalidFileNames = FilteredFileNames.ToArray(),
                        CancelToken = TokenStore.GetInstance().CancellationTokenSourceForName(TokenStore.TOKEN_SOURCE_PROCESS_FILES).Token,
                        MaxCountPages = p.MaxCountPages,
                        Interpolation = p.Interpolation,
                        HashFiles = p.HashFiles,
                        ContinuePipeline = true,
                    });
                });

            }

            if (nextTask?.TaskId == PipelineEvent.PIPELINE_UPDATE_INDICES)
            {
                UpdatePageIndicesThreadParams p = nextTask.ThreadParams as UpdatePageIndicesThreadParams;

                Task<TaskResult> indexUpdater = UpdatePageIndexTask.UpdatePageIndex(p.Pages, AppEventHandler.OnGeneralTaskProgress, AppEventHandler.OnPageChanged, p.CancelToken, false, true, remainingStack);

                indexUpdater.ContinueWith((t) =>
                {
                    AppEventHandler.OnGeneralTaskProgress(sender, new GeneralTaskProgressEvent(
                        GeneralTaskProgressEvent.TASK_UPDATE_PAGE_INDEX,
                        GeneralTaskProgressEvent.TASK_STATUS_COMPLETED,
                        "Ready", 0, 0, true));

                    if (MetaData.Exists() && p.UpdateIndexMetadata)
                    {
                        Task<TaskResult> imageMetaDataUpdater = UpdateMetadataTask.UpdatePageMetadata(new List<Page>(p.Pages.ToArray()), Program.ProjectModel.MetaData, p.PageIndexVerToWrite, AppEventHandler.OnGeneralTaskProgress, AppEventHandler.OnPageChanged, TokenStore.GetInstance().CancellationTokenSourceForName(TokenStore.TOKEN_SOURCE_UPDATE_IMAGE_METADATA).Token, false, true, Guid.NewGuid().ToString(), remainingStack);

                        imageMetaDataUpdater.ContinueWith((t) =>
                        {
                            if (t.IsCompletedSuccessfully)
                            {
                                AppEventHandler.OnGeneralTaskProgress(sender, new GeneralTaskProgressEvent(
                                    GeneralTaskProgressEvent.TASK_RELOAD_IMAGE_METADATA,
                                    GeneralTaskProgressEvent.TASK_STATUS_COMPLETED,
                                    "Ready", 0, 0, true));

                                if (p.ContinuePipeline)
                                {                                   
                                    int nextTaskId = p.GetNextTaskId();

                                    if (nextTaskId > 0)
                                    {
                                        AppEventHandler.OnPipelineNextTask(sender, new PipelineEvent(Program.ProjectModel, e.Task, null, remainingStack));
                                    }
                                    else
                                    {
                                        if (t.Result.Stack != null && t.Result.Stack.Count > 0)
                                        {
                                            nextTask = t.Result.Stack[0];
                                            //t.Result.Stack.RemoveAt(0);

                                            if (nextTask.TaskId != e.Task)
                                            {
                                                AppEventHandler.OnPipelineNextTask(sender, new PipelineEvent(Program.ProjectModel, e.Task, null, t.Result.Stack));

                                            }
                                        } else
                                        {
                                            AppEventHandler.OnApplicationStateChanged(sender, new ApplicationStatusEvent(Program.ProjectModel, ApplicationStatusEvent.STATE_READY));

                                        }
                                    }                                   
                                } else
                                {
                                    AppEventHandler.OnArchiveStatusChanged(sender, new ArchiveStatusEvent(Program.ProjectModel, ArchiveStatusEvent.ARCHIVE_ERROR_SAVING));
                                    AppEventHandler.OnApplicationStateChanged(sender, new ApplicationStatusEvent(Program.ProjectModel, ApplicationStatusEvent.STATE_READY));
                                }
                            } else
                            {
                                AppEventHandler.OnArchiveStatusChanged(sender, new ArchiveStatusEvent(Program.ProjectModel, ArchiveStatusEvent.ARCHIVE_ERROR_SAVING));
                                AppEventHandler.OnApplicationStateChanged(sender, new ApplicationStatusEvent(Program.ProjectModel, ApplicationStatusEvent.STATE_READY));
                            }
                        }, p.CancelToken);

                        imageMetaDataUpdater.Start();
                    } else
                    {
                        if (p.ContinuePipeline)
                        {
                            int nextTaskId = p.GetNextTaskId();

                            if (nextTaskId > 0)
                            {
                                AppEventHandler.OnPipelineNextTask(sender, new PipelineEvent(Program.ProjectModel, nextTaskId, null, remainingStack));
                            }
                            else
                            {
                                if (t.Result.Stack.Count > 0)
                                {
                                    nextTask = t.Result.Stack[0];

                                    if (nextTask.TaskId != e.Task)
                                    {
                                        AppEventHandler.OnPipelineNextTask(sender, new PipelineEvent(Program.ProjectModel, e.Task, null, t.Result.Stack));

                                    }
                                }
                                else
                                {
                                    AppEventHandler.OnApplicationStateChanged(sender, new ApplicationStatusEvent(Program.ProjectModel, ApplicationStatusEvent.STATE_READY));

                                }
                            }
                        }
                        else
                        {
                            AppEventHandler.OnArchiveStatusChanged(sender, new ArchiveStatusEvent(Program.ProjectModel, ArchiveStatusEvent.ARCHIVE_ERROR_SAVING));
                            AppEventHandler.OnApplicationStateChanged(sender, new ApplicationStatusEvent(Program.ProjectModel, ApplicationStatusEvent.STATE_READY));
                        }
                    }
                });

                indexUpdater.Start();

                //Task.Factory.StartNew(() =>
                //{
                //    if (MetaData.Exists())
                //    {
                //        MetaData.RebuildPageMetaData(p.Pages, p.PageIndexVerToWrite);
                //    }
                //    UpdatePageIndices(p.Pages, true, true, remainingStack);
                //}, (nextTask.ThreadParams as UpdatePageIndicesThreadParams).CancelToken);

            }

            if (nextTask?.TaskId == PipelineEvent.PIPELINE_UPDATE_IMAGE_METADATA)
            {
                UpdatePageMetadataThreadParams p = nextTask.ThreadParams as UpdatePageMetadataThreadParams;

                if (p.UpdateImageMetadata)
                {
                    if (imageInfoUpdater == null)
                    {
                        imageInfoUpdater = ReadImageMetaDataTask.UpdateImageMetadata(p.Pages, AppEventHandler.OnGeneralTaskProgress, p.CancelToken, false, true, remainingStack);
                        imageInfoUpdater.ContinueWith(t =>
                        {
                            if (t.IsCompletedSuccessfully)
                            {
                                if (t.Result.Stack.Count > 0)
                                {
                                    nextTask = t.Result.Stack[0];

                                    AppEventHandler.OnPipelineNextTask(this, new PipelineEvent(this, e.Task, nextTask, t.Result.Stack));
                                }
                            }
                        });


                        imageInfoUpdater.Start();
                    }
                    else
                    {
                        if (imageInfoUpdater.IsCompleted || imageInfoUpdater.IsCanceled)
                        {
                            imageInfoUpdater = ReadImageMetaDataTask.UpdateImageMetadata(p.Pages, AppEventHandler.OnGeneralTaskProgress, p.CancelToken, false, true, remainingStack);
                            imageInfoUpdater.ContinueWith(t =>
                            {
                                if (t.IsCompletedSuccessfully)
                                {
                                    if (t.Result.Stack.Count > 0)
                                    {
                                        nextTask = t.Result.Stack[0];

                                        AppEventHandler.OnPipelineNextTask(this, new PipelineEvent(this, e.Task, nextTask, t.Result.Stack));
                                    }
                                }
                            });
                            imageInfoUpdater.Start();
                        }
                    }
                } else
                {

                }
            }

            if (nextTask?.TaskId == PipelineEvent.PIPELINE_PROCESS_IMAGES)
            {
                ProcessImagesThreadParams p = nextTask.ThreadParams as ProcessImagesThreadParams;

                if (p.ApplyImageProcessing)
                {
                   
                    imageProcessingTask = ProcessImagesTask.ProcessImages(p.Pages, p.GlobalTask, p.SkipPages, AppEventHandler.OnGeneralTaskProgress, p.CancelToken, remainingStack);
                    Task<ImageTaskResult> imageProcessingFinalTask = imageProcessingTask.ContinueWith(r =>
                    {
                        AppEventHandler.OnApplicationStateChanged(null, new ApplicationStatusEvent(Program.ProjectModel, ApplicationStatusEvent.STATE_PROCESSING));

                        ImageTaskResult imageTaskResult = r.Result;
                        

                        if (r.IsCompletedSuccessfully)
                        {
                            
                            // update pages with results
                            Page page = null;
                            Page previousPage = null;
                            int index = 0;
                            List<Page> thumbUpdates = new List<Page>();

                            foreach (Page resultPage in r.Result.Pages)
                            {
                                try
                                {
                                    page = GetPageById(resultPage.Id);
                                    
                                    //page?.UpdatePage(resultPage);
                                    //page?.UpdateStreams(resultPage);
                                    
                                    //page?.LoadImageInfo(true);


                                    if (page != null)
                                    {
                                        page.UpdatePageAttributes(resultPage);
                                        if (page.LocalFile == null || !page.LocalFile.Equals(resultPage.LocalFile))
                                        {

                                            //page.LocalFile = new LocalFile(resultPage.LocalFile.FullPath);
                                            //page?.UpdateLocalFileWith(resultPage.LocalFile);  // dont overwrite original file!
                                            if (resultPage.TemporaryFile != null && !resultPage.TemporaryFile.Equals(page.TemporaryFile))
                                            {
                                                page.UpdateTemporaryFile(resultPage.TemporaryFile);
                                            }

                                        }

                                        //page.Name = resultPage.Name;
                                        //page.Format = resultPage.Format;

                                        // update page first, to reflect changes in the UI
                                        //    AppEventHandler.OnPageChanged(this, new PageChangedEvent(resultPage, null, PageChangedEvent.IMAGE_STATUS_CHANGED));
                                        // AppEventHandler.OnRedrawThumb(null, new RedrawThumbEvent(page));
                                        //
                                        //      page.Size = resultPage.Size;
                                        page.ImageTask.ImageAdjustments.SplitPage = false;
                                        page.ImageTask.ImageAdjustments.ResizeMode = -1;
                                        page.ImageTask.ImageAdjustments.ConvertType = 0;
                                        page.ImageTask.ImageAdjustments.RotateMode = 0;
                                        page.ThumbnailInvalidated = true;

                                        resultPage.FreeImage();

                                        

                                        // update image adjustments
                                        AppEventHandler.OnPageChanged(this, new PageChangedEvent(page, null, PageChangedEvent.IMAGE_STATUS_CHANGED));
                                        //AppEventHandler.OnRedrawThumb(null, new RedrawThumbEvent(page));
                                        AppEventHandler.OnImageAdjustmentsChanged(null, new ImageAdjustmentsChangedEvent(page.ImageTask.ImageAdjustments, page.Id));

                                        thumbUpdates.Add(page);
                                        previousPage = page;
                                    }
                                    else
                                    {
                                        // todo: check insert position.....
                                        Page newPage = AddPage(resultPage, previousPage.Index, true);
                                        
                                        newPage.ImageTask.ImageAdjustments.SplitPage = false;
                                        newPage.ImageTask.ImageAdjustments.ResizeMode = -1;
                                        newPage.ImageTask.ImageAdjustments.ConvertType = 0;
                                        newPage.ImageTask.ImageAdjustments.RotateMode = 0;

                                        AppEventHandler.OnRedrawThumb(null, new RedrawThumbEvent(newPage));
                                        AppEventHandler.OnImageAdjustmentsChanged(null, new ImageAdjustmentsChangedEvent(resultPage.ImageTask.ImageAdjustments, resultPage.Id));
                                        // AppEventHandler.OnPageChanged(this, new PageChangedEvent(newPage, null, PageChangedEvent.IMAGE_STATUS_NEW));
                                        //

                                        previousPage = newPage;
                                    }

                                    resultPage.FreeStreams();
                                    resultPage.ImageTask.FreeResults();
                                    
                                } catch (Exception e)
                                {
                                    AppEventHandler.OnImageAdjustmentsChanged(null, new ImageAdjustmentsChangedEvent(page.ImageTask.ImageAdjustments, page.Id));


                                    MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_ERROR, "Error updating result images! [" + e.Message + "]");

                                }

                                AppEventHandler.OnGeneralTaskProgress(null, new GeneralTaskProgressEvent(GeneralTaskProgressEvent.TASK_PROCESS_IMAGE, GeneralTaskProgressEvent.TASK_STATUS_RUNNING, "Updating processed pages...", index, r.Result.Pages.Count, false));

                                index++;
                            }

                            AppEventHandler.OnUpdateThumbnails(this, new UpdateThumbnailsEvent(thumbUpdates));
                            AppEventHandler.OnUpdateListViewSorting(this, new UpdatePageListViewSortingEvent(1, SortOrder.Ascending));

                            ImageAdjustments resetImageAdjustments = new ImageAdjustments();
                            AppEventHandler.OnImageAdjustmentsChanged(null, new ImageAdjustmentsChangedEvent(resetImageAdjustments));
                            
                        } else
                        {

                            // todo: continue on error?
                            throw new Exception("");
                        }

                        return imageTaskResult;
                    });

                    imageProcessingFinalTask.ContinueWith(r =>
                    {
                        List<Page> updatePages = new List<Page>();

                        if (r.IsCompletedSuccessfully)
                        {
                            if (r.Result.Stack.Count > 0)
                            {
                                nextTask = r.Result.Stack[0];

                                if (r.Result.Pages.Count > 0)
                                {
                                    r.Result.Pages.Sort((x, y) => x.Index.CompareTo(y.Index));

                                    if (nextTask.ThreadParams is UpdatePageIndicesThreadParams)
                                    {
                                        //UpdatePageIndicesThreadParams np = nextTask.ThreadParams as UpdatePageIndicesThreadParams;

                                        (nextTask.ThreadParams as UpdatePageIndicesThreadParams).Pages = r.Result.Pages;
                                        //updatePages = (nextTask.ThreadParams as UpdatePageIndicesThreadParams).Pages;
                                    }

                                    if (nextTask.ThreadParams is RenamePagesThreadParams)
                                    {

                                        (nextTask.ThreadParams as RenamePagesThreadParams).Pages = r.Result.Pages;
                                        //updatePages = (nextTask.ThreadParams as RenamePagesThreadParams).Pages;

                                    }

                                    foreach (StackItem si in r.Result.Stack)
                                    {
                                        if (si.ThreadParams is UpdatePageIndicesThreadParams)
                                        {
                                            (si.ThreadParams as UpdatePageIndicesThreadParams).Pages = r.Result.Pages;
                                        }

                                        if (si.ThreadParams is RenamePagesThreadParams)
                                        {
                                            (si.ThreadParams as RenamePagesThreadParams).Pages = r.Result.Pages;
                                        }
                                    }



                                    /*
                                    if (updatePages.Count <= r.Result.Pages.Count)
                                    {
                                        for (int i = 0; i < r.Result.Pages.Count; i++)
                                        {
                                            updatePages[i].UpdatePage(r.Result.Pages[i]);
                                        }
                                    }
                                    */
                                    
                                }

                                AppEventHandler.OnPipelineNextTask(this, new PipelineEvent(this, e.Task, null, r.Result.Stack));
                            }
                            else
                            {
                                AppEventHandler.OnApplicationStateChanged(null, new ApplicationStatusEvent(Program.ProjectModel, ApplicationStatusEvent.STATE_READY));
                            }
                        } else
                        {
                            MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_ERROR, "Error processing images! [" + r.Exception.InnerException?.Message + "]");

                            AppEventHandler.OnApplicationStateChanged(null, new ApplicationStatusEvent(Program.ProjectModel, ApplicationStatusEvent.STATE_READY));
                        }
                    });

                    imageProcessingTask.Start();                   
                } else
                {
                    if (p.ContinuePipeline)
                    {
                        AppEventHandler.OnPipelineNextTask(this, new PipelineEvent(this, nextTask.TaskId, null, remainingStack));
                    }
                }
            }

            if (nextTask?.TaskId == PipelineEvent.PIPELINE_RUN_RENAMING)
            {
                RenamePagesThreadParams p = nextTask.ThreadParams as RenamePagesThreadParams;
                p.Stack = remainingStack;   // update stack with current remaining tasks

                // Apply renaming rules
                if (p.ApplyRenaming || p.CompatibilityMode)
                {
                    if (p.CompatibilityMode)
                    {
                        // Force renaming every page to its index in compatibility mode
                        p.RenameStoryPagePattern = "{page}.{ext}";
                        p.RenameSpecialPagePattern = "{page}.{ext}";
                        p.SkipIndexUpdate = true;
                    }

                    try
                    {
                        RenamingThread = new Thread(RunRenameScriptsForPages);
                        RenamingThread.Start(p);
                    }
                    catch (Exception ee)
                    {
                        MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_ERROR, "Error in Renamer-Script  [" + ee.Message + "]");

                    }
                    finally 
                    {
                       
                    }
                } else
                {
                    if (p.Stack != null && p.Stack.Count > 0 && p.ContinuePipeline)
                    {
                        AppEventHandler.OnPipelineNextTask(this, new PipelineEvent(this, e.Task, null, p.Stack));
                    } else
                    {
                        AppEventHandler.OnApplicationStateChanged(null, new ApplicationStatusEvent(Program.ProjectModel, ApplicationStatusEvent.STATE_READY));
                    }
                }
            }

            if (nextTask?.TaskId == PipelineEvent.PIPELINE_SAVE_ARCHIVE)
            {
                if (e.Attributes != null)
                {

                }

                SaveArchiveThreadParams p = nextTask.ThreadParams as SaveArchiveThreadParams;

                if (p.Stack != null && p.Stack.Count > 0)
                {

                }
                else
                {
                   p.Stack = remainingStack;
                }

                SaveArchiveThread = new Thread(SaveArchiveProc);
                SaveArchiveThread.Start(p);
            }

            // end of pipeline
        }

        public void New()
        {
            if (ThreadRunning())
            {
                throw new ConcurrentOperationException("There are still operations running in the Background.\r\nPlease wait until those have completed and try again!", true);
            }

            Task<string> newFollowTask = new Task<string>(() =>
            {
                Pages.Clear();
                MetaData.Free();
                MaxFileIndex = 0;
                IsNew = true;
                IsChanged = false;
                GlobalImageTask = new ImageTask("");

                AppEventHandler.OnApplicationStateChanged(this, new ApplicationStatusEvent(this, ApplicationStatusEvent.STATE_READY));
                AppEventHandler.OnArchiveStatusChanged(this, new ArchiveStatusEvent(this, ArchiveStatusEvent.ARCHIVE_NEW));

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

                return ProjectGUID;
            });

            Close(newFollowTask);           
        }

        public Thread Open(String path, 
            ZipArchiveMode mode, 
            MetaData.PageIndexVersion currentMetaDataVersionWriting, 
            bool skipIndexCheck = false, 
            string interpolationMode = "Default", 
            bool writeIndexSetting = true,
            bool applyKeyUserFilter = false,
            string[] filterKeys = null,
            int filterBaseCondition = 0
            )
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

            TokenStore.GetInstance().ResetCancellationToken(TokenStore.TOKEN_SOURCE_LOAD_ARCHIVE);

            LoadArchiveThread = new Thread(OpenArchiveProc);
            LoadArchiveThread.Start(new OpenArchiveThreadParams()
            {
                FileName = path,
                Mode = mode,
                CurrentPageIndexVer = currentMetaDataVersionWriting,
                SkipIndexCheck = skipIndexCheck,
                WriteIndex = writeIndexSetting,
                Interpolation = interpolationMode,
                ApplyKeyUserFilter = applyKeyUserFilter,
                FilterKeys = filterKeys,
                FilterBaseCondition = filterBaseCondition,
                CancelToken = TokenStore.GetInstance().CancellationTokenSourceForName(TokenStore.TOKEN_SOURCE_LOAD_ARCHIVE).Token
            });

            return LoadArchiveThread;
        }

        protected void OpenArchiveProc(object threadParams)
        {
            OpenArchiveThreadParams tParams = threadParams as OpenArchiveThreadParams;

            ArrayList missingPages = new ArrayList();

            int index = 0;
            long totalSize = 0;
            MetaDataPageIndexMissingData = false;
            MetaDataPageIndexFileMissing = false;
            String IndexUpdateReasonMessage = "";
            bool MetaDataPageIndexFileMissingShown = false;
            bool MetaDataPageIndexDisabledInfoShown = false;

            AppEventHandler.OnArchiveStatusChanged(this, new ArchiveStatusEvent(this, ArchiveStatusEvent.ARCHIVE_OPENING));
            AppEventHandler.OnApplicationStateChanged(this, new ApplicationStatusEvent(this, ApplicationStatusEvent.STATE_OPENING));

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
                        if (metaDataEntry.IsEncrypted)
                        {
                            MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_WARNING, "Warning! Metadata ['" + MetaData.MetaDataFileName + "'] is encrypted! Encryption is not supported.");
                        }

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

                if (MetaData != null && tParams.ApplyKeyUserFilter)
                {
                    MetaData.UserFilterMetaData(tParams.FilterKeys, tParams.FilterBaseCondition);
                }

                MetaDataEntryPage pageIndexEntry;
                foreach (ZipArchiveEntry entry in Archive.Entries)
                {
                    if (!entry.FullName.ToLower().Contains(MetaData.MetaDataFileName.ToLower()))
                    {
                        Page page = new Page(entry, Path.Combine(PathHelper.ResolvePath(WorkingDir), ProjectGUID), RandomId.GetInstance().Make())
                        {
                            Number = index + 1,
                            Index = index,
                            OriginalIndex = index,
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
                                if (!MetaDataPageIndexDisabledInfoShown && !tParams.WriteIndex)
                                {
                                    MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_INFO, "Archive page-index is disabled by user! No index is checked and no image-metadata is being read from index.");
                                    MetaDataPageIndexDisabledInfoShown = true;
                                }
                            }
                        }

                        if ((pageIndexEntry == null || MetaData.IndexVersionSpecification != tParams.CurrentPageIndexVer) && !MetaDataPageIndexFileMissingShown && tParams.WriteIndex)
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
                                    page.Key = RandomId.GetInstance().Make();
                                    MetaDataPageIndexMissingData = true;
                                }
                                page.DoublePage = Boolean.Parse(pageIndexEntry.GetAttribute(MetaDataEntryPage.COMIC_PAGE_ATTRIBUTE_DOUBLE_PAGE) ?? "False");
                                page.Bookmark = pageIndexEntry.GetAttribute(MetaDataEntryPage.COMIC_PAGE_ATTRIBUTE_BOOKMARK) ?? string.Empty;
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
                            if (tParams.WriteIndex)
                            {
                                MetaDataPageIndexFileMissing = true;
                                MetaDataPageIndexMissingData = true;
                                if (!MetaDataPageIndexFileMissingShown)
                                {
                                    IndexUpdateReasonMessage = "Image metadata missing from pageindex! Reload image metadata and rebuild pageindex now?";
                                }
                                MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_WARNING, "Warning! Archive page metadata missing for page [" + page.Name + "]!");
                            }
                        }

                        try
                        {
                            page.ImageTask.ImageAdjustments.Interpolation = Enum.Parse<InterpolationMode>(tParams.Interpolation);
                        }
                        catch (Exception ex)
                        {
                            MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_WARNING, "Failed to set interpolation mode ['" + tParams.Interpolation + "'] for page ['" + page.Name + "']! [" + ex.Message + "]");
                        }

                        Pages.Add(page);

                        AppEventHandler.OnPageChanged(this, new PageChangedEvent(page, null, PageChangedEvent.IMAGE_STATUS_NEW));
                        
                        AppEventHandler.OnTaskProgress(this, new TaskProgressEvent(page, index, countEntries));

                        totalSize += page.Size;
                        index++;
                    }

                    Thread.Sleep(5);
                 
                    tParams.CancelToken.ThrowIfCancellationRequested();
                }
               
                String pageIndexName = "";
                Page pageCheck = null;
                index = 0;

                if (!tParams.SkipIndexCheck && tParams.WriteIndex)
                {

                    // check index and compare with files
                    AppEventHandler.OnApplicationStateChanged(this, new ApplicationStatusEvent(this, ApplicationStatusEvent.STATE_CHECKING_INDEX));
                    AppEventHandler.OnTaskProgress(this, new TaskProgressEvent(null, 0, 100));

                    foreach (MetaDataEntryPage entry in MetaData.PageIndex)
                    {
                        if (MetaData.IndexVersionSpecification == PageIndexVersion.VERSION_2)
                        {
                            pageIndexName = entry.GetAttribute(MetaDataEntryPage.COMIC_PAGE_ATTRIBUTE_IMAGE);
                        }
                        else if (MetaData.IndexVersionSpecification == PageIndexVersion.VERSION_1)
                        {
                            pageIndexName = entry.GetAttribute(MetaDataEntryPage.COMIC_PAGE_ATTRIBUTE_KEY);
                        }

                        pageCheck = GetPageByName(pageIndexName);

                        if (pageCheck == null)
                        {
                            missingPages.Add(pageIndexName);
                        }

                        pageCheck = null;
                        AppEventHandler.OnTaskProgress(this, new TaskProgressEvent(null, index, MetaData.PageIndex.Count));
                        Thread.Sleep(5);
                        index++;

                        tParams.CancelToken.ThrowIfCancellationRequested();
                    }
                }

                IsChanged = false;
                IsNew = false;
            }
            catch (OperationCanceledException)
            {
                //Archive.Dispose();

                //OnArchiveStatusChanged(new ArchiveStatusEvent(this, ArchiveStatusEvent.ARCHIVE_CLOSED));
            }
            catch (Exception e)
            {
                MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_ERROR, "Error opening Archive\n" + e.Message);
            }


            if (!tParams.CancelToken.IsCancellationRequested)
            {
                FileSize = totalSize;
                MaxFileIndex = index;

                if (MetaDataPageIndexMissingData && tParams.WriteIndex)
                {
                    String gid = Guid.NewGuid().ToString();
                    //TokenStore.GetInstance().ResetCancellationToken(TokenStore.TASK_TYPE_UPDATE_IMAGE_METADATA);

                    AppEventHandler.OnGlobalActionRequired(this, 
                        new GlobalActionRequiredEvent(this, 
                            0, 
                            "Image metadata missing from pageindex! Reload image metadata and rebuild pageindex now?", 
                            "Rebuild", 
                            GlobalActionRequiredEvent.TASK_TYPE_UPDATE_IMAGE_METADATA, 
                            ReadImageMetaDataTask.UpdateImageMetadata(Pages, 
                                AppEventHandler.OnGeneralTaskProgress,
                                tParams.CancelToken,
                                false,
                                true,
                                tParams.Stack
                            ),
                            gid
                        )
                    );
                }

                if (MetaDataPageIndexFileMissing && tParams.WriteIndex)
                {
                    String gid = Guid.NewGuid().ToString();

                    //TokenStore.GetInstance().ResetCancellationToken(TokenStore.TASK_TYPE_UPDATE_IMAGE_METADATA);

                    AppEventHandler.OnGlobalActionRequired(this, 
                        new GlobalActionRequiredEvent(this, 
                            0, 
                            IndexUpdateReasonMessage, 
                            "Rebuild", 
                            GlobalActionRequiredEvent.TASK_TYPE_INDEX_REBUILD, 
                            UpdateMetadataTask.UpdatePageMetadata(Pages, 
                                MetaData, 
                                PageIndexVersionWriter, 
                                AppEventHandler.OnGeneralTaskProgress, 
                                AppEventHandler.OnPageChanged,
                                tParams.CancelToken, 
                                false, 
                                true,
                                gid,
                                tParams.Stack
                            ),
                            gid
                        ));
                }

                if (missingPages.Count > 0)
                {
                    MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_WARNING, "Page(s) missing from archive but are present in page-index!");
                }


                AppEventHandler.OnArchiveStatusChanged(this, new ArchiveStatusEvent(this, ArchiveStatusEvent.ARCHIVE_OPENED));
            }

            AppEventHandler.OnApplicationStateChanged(this, new ApplicationStatusEvent(this, ApplicationStatusEvent.STATE_READY));
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

            bool tagValidationFailed = false;
            bool applyImageProcessing = false;
            bool metaDataValidationFailed = false;
            if (Win_CBZSettings.Default.ValidateTags)
            {
                tagValidationFailed = Validation.ValidateTags();

                if (tagValidationFailed)
                {
                    AppEventHandler.OnApplicationStateChanged(this, new ApplicationStatusEvent(this, ApplicationStatusEvent.STATE_READY));

                    return false;
                }
            }

            /*
            metaDataValidationFailed = Validation.ValidateMetaDataDuplicateKeys(ref invalidKeys);
            if (metaDataValidationFailed)
            {
                AppEventHandler.OnApplicationStateChanged(this, new ApplicationStatusEvent(this, ApplicationStatusEvent.STATE_READY));

                return false;
            }
            */

            metaDataValidationFailed = Validation.ValidateMetaDataInvalidKeys(ref invalidKeys);
            if (metaDataValidationFailed)
            {
                AppEventHandler.OnApplicationStateChanged(this, new ApplicationStatusEvent(this, ApplicationStatusEvent.STATE_READY));

                return false;
            }

            if (Program.ProjectModel.MetaData.Exists())
            {
                /*
                string defaultKeys = String.Join("", Program.ProjectModel.MetaData.Values.Select(k => k.Key).ToArray());
                if (!Regex.IsMatch(defaultKeys, @"^[a-z]+$", RegexOptions.IgnoreCase))
                {
                    MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_WARNING, "Validateion Error! Metadata-Keys must contain only values between ['a-zA-Z']");

                    ApplicationMessage.ShowWarning("Validateion Error! Metadata-Keys must contain only values between ['a-zA-Z']!", "Invalid Metadata", ApplicationMessage.DialogType.MT_WARNING, ApplicationMessage.DialogButtons.MB_OK);

                    AppEventHandler.OnApplicationStateChanged(this, new ApplicationStatusEvent(this, ApplicationStatusEvent.STATE_READY));

                    return false;
                }
                */

                foreach (MetaDataEntry entry in MetaData.Values)
                {
                    MetaDataFieldConfig.GetInstance().UpdateAutoCompleteOptions(entry.Key, entry.ValueAsList());
                }
            }

            Task<SaveArchivePreChecksResult> checkImageProcessing = new Task<SaveArchivePreChecksResult>(() =>
            {
                SaveArchivePreChecksResult result = new SaveArchivePreChecksResult();
                bool applyImageProcessing = false;
                bool updateIndexMetadata = false;
                bool writeMetadataOnly = false;
                int checkIndex = 0;

                AppEventHandler.OnArchiveStatusChanged(this, new ArchiveStatusEvent(this, ArchiveStatusEvent.ARCHIVE_SAVING));

                if (GlobalImageTask != null && 
                    (GlobalImageTask.ImageAdjustments.ConvertType > 0 ||
                     GlobalImageTask.ImageAdjustments.SplitPage ||
                     GlobalImageTask.ImageAdjustments.RotateMode > 0 ||
                     GlobalImageTask.ImageAdjustments.ResizeMode > 0
                     ))
                {
                    applyImageProcessing = true;
                    updateIndexMetadata = true;
                }

                if (!applyImageProcessing)
                {
                    foreach (Page page in Pages)
                    {
                        if (page.ImageTask != null &&
                            (page.ImageTask.ImageAdjustments.ConvertType > 0 ||
                             page.ImageTask.ImageAdjustments.SplitPage ||
                             page.ImageTask.ImageAdjustments.RotateMode > 0 ||
                             page.ImageTask.ImageAdjustments.ResizeMode > 0
                             ))
                        {
                            applyImageProcessing = true;
                        }

                        if (MetaData.FindIndexEntryForPage(page)?.GetAttribute(MetaDataEntryPage.COMIC_PAGE_ATTRIBUTE_IMAGE_WIDTH) != page.Format.W.ToString() ||
                            MetaData.FindIndexEntryForPage(page)?.GetAttribute(MetaDataEntryPage.COMIC_PAGE_ATTRIBUTE_IMAGE_HEIGHT) != page.Format.H.ToString() ||
                            MetaData.FindIndexEntryForPage(page)?.GetAttribute(MetaDataEntryPage.COMIC_PAGE_ATTRIBUTE_IMAGE_SIZE) != page.Size.ToString())
                        {
                            updateIndexMetadata = true;
                        }

                        AppEventHandler.OnTaskProgress(this, new TaskProgressEvent(page, checkIndex, Pages.Count, "Initializing..."));

                        checkIndex++;

                        Thread.Sleep(1);
                    }
                }

                result.ApplyImageProcessing = applyImageProcessing;
                result.UpdatePageIndexMetadata = updateIndexMetadata;

                return result;
            });

            checkImageProcessing.ContinueWith((t) =>
            {
                if (t.IsCompletedSuccessfully)
                {
                    
                    PageIndexVersionWriter = metaDataVersionWriting;

                    TokenStore.GetInstance().ResetCancellationToken(TokenStore.TOKEN_SOURCE_SAVE_ARCHIVE);

                    AppEventHandler.OnPipelineNextTask(this, new PipelineEvent(
                        this,
                        PipelineEvent.PIPELINE_PROCESS_IMAGES,
                        null,
                        new List<StackItem>()
                        {
                            new StackItem()
                            {
                                TaskId = PipelineEvent.PIPELINE_PROCESS_IMAGES,
                                ThreadParams = new ProcessImagesThreadParams()
                                {
                                    ApplyImageProcessing = t.Result.ApplyImageProcessing,
                                    ContinuePipeline = true,
                                    CancelToken = TokenStore.GetInstance().CancellationTokenSourceForName(TokenStore.TOKEN_SOURCE_SAVE_ARCHIVE).Token,
                                    Pages = Pages,
                                    SkipPages = ConversionExcludes.Cast<String>().ToArray(),
                                    GlobalTask = GlobalImageTask
                                }
                            },
                            new StackItem()
                            {
                                TaskId = PipelineEvent.PIPELINE_RUN_RENAMING,
                                ThreadParams = new RenamePagesThreadParams()
                                {
                                    Pages = Pages,
                                    ApplyRenaming = ApplyRenaming,
                                    CompatibilityMode = CompatibilityMode,
                                    IgnorePageNameDuplicates = CompatibilityMode,
                                    RenameStoryPagePattern = CompatibilityMode ? "" : RenameStoryPagePattern,
                                    RenameSpecialPagePattern = CompatibilityMode ? "" : RenameSpecialPagePattern,
                                    ContinuePipeline = true,
                                    CancelToken = TokenStore.GetInstance().CancellationTokenSourceForName(TokenStore.TOKEN_SOURCE_SAVE_ARCHIVE).Token
                                }
                            },
                            new StackItem()
                            {
                                TaskId = PipelineEvent.PIPELINE_UPDATE_INDICES,
                                ThreadParams = new UpdatePageIndicesThreadParams()
                                {
                                    Pages = Pages,
                                    ContinuePipeline = true,
                                    InitialIndexRebuild = false,
                                    UpdateIndexMetadata = t.Result.UpdatePageIndexMetadata || CompatibilityMode,
                                    Stack = new List<StackItem>(),
                                    PageIndexVerToWrite = metaDataVersionWriting,
                                    CancelToken = TokenStore.GetInstance().CancellationTokenSourceForName(TokenStore.TOKEN_SOURCE_SAVE_ARCHIVE).Token
                                }
                            },
                            new StackItem()
                            {
                                TaskId = PipelineEvent.PIPELINE_SAVE_ARCHIVE, //999, //   PipelineEvent.PIPELINE_SAVE_ARCHIVE
                                ThreadParams = new SaveArchiveThreadParams()
                                {
                                    Pages = Pages,
                                    FileName = path,
                                    Mode = mode,
                                    ContinuePipeline = true,
                                    ContinueOnError = continueOnError,
                                    CompressionLevel = CompressionLevel,
                                    PageIndexVerToWrite = metaDataVersionWriting,
                                    WriteMetadataOnly = t.Result.WriteMetadataOnly,
                                    WriteIndex = Win_CBZSettings.Default.WriteXmlPageIndex,
                                    CancelToken = TokenStore.GetInstance().CancellationTokenSourceForName(TokenStore.TOKEN_SOURCE_SAVE_ARCHIVE).Token
                                }
                            }
                        }
                    ));
                }
            });

            checkImageProcessing.Start();
         
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

            AppEventHandler.OnArchiveStatusChanged(this, new ArchiveStatusEvent(this, ArchiveStatusEvent.ARCHIVE_SAVING));

            // Rebuild ComicInfo.xml's PageIndex... dont do that here. needs to happen beforehand
            //MetaData.RebuildPageMetaData(tParams?.Pages?.ToList<Page>(), tParams.PageIndexVerToWrite);

            try
            {
                BuildingArchive = ZipFile.Open(TemporaryFileName, ZipArchiveMode.Create);

                // Write files to new temporary archive
                Thread.BeginCriticalRegion();
                foreach (Page page in tParams?.Pages)
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
                            page.FreeStreams();

                            if (page.Changed || page.Compressed)
                            {
                                if (page.TemporaryFile != null)
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
                            } else
                                {
                                    sourceFileName = page.LocalFile.FullPath;
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
                                page.UpdateImageEntry(updatedEntry, RandomId.GetInstance().Make());
                                page.Compressed = true;
                            }
                            page.Changed = false;
                            page.ImageChanged = false;
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

                                AppEventHandler.OnPageChanged(this, new PageChangedEvent(page, null, PageChangedEvent.IMAGE_STATUS_COMPRESSED));
                        }
                        else
                        {
                            // collect all deleted Items
                            deletedPages.Add(page);
                        }

                        tParams.CancelToken.ThrowIfCancellationRequested();
                    }
                    catch (OperationCanceledException)
                    {
                        errorSavingArchive = true;
                    }
                    catch (Exception efile)
                    {
                        MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_ERROR, "Error compressing File [" + fileToCompress.FileName + "] to Archive [" + efile.Message + "]");

                        if (!Win_CBZSettings.Default.IgnoreErrorsOnSave)
                        {
                            AppEventHandler.OnArchiveStatusChanged(this, new ArchiveStatusEvent(this, ArchiveStatusEvent.ARCHIVE_ERROR_SAVING));
                            AppEventHandler.OnApplicationStateChanged(this, new ApplicationStatusEvent(this, ApplicationStatusEvent.STATE_READY));
                            errorSavingArchive = true;
                            return;
                        }
                    }
                    finally
                    {
                        Thread.Sleep(5);
                    }

                    AppEventHandler.OnArchiveOperation(this, new ArchiveOperationEvent(ArchiveOperationEvent.OPERATION_COMPRESS, ArchiveOperationEvent.STATUS_SUCCESS, index, Pages.Count + 1, page));

                    index++;
                }
                Thread.EndCriticalRegion();
                
                // Create Metadata
                try
                {
                    if (MetaData.Values.Count > 0 || MetaData.PageIndex.Count > 0)
                    {
                        MemoryStream ms = MetaData.BuildComicInfoXMLStream(false, tParams.WriteIndex);
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

                        Task<TaskResult> copyFile = CopyFileTask.CopyFile(new LocalFile(TemporaryFileName), new LocalFile(tParams.FileName), AppEventHandler.OnFileOperation, tParams.CancelToken);

                        copyFile.Start();
                        copyFile.Wait(tParams.CancelToken); // run synchronously and wait for completion
                        
                        //CopyFile(TemporaryFileName, tParams.FileName, true);

                        int deletedIndex = 0;
                        foreach (Page deletedPage in deletedPages)
                        {
                            Pages.Remove(deletedPage);

                            AppEventHandler.OnPageChanged(this, new PageChangedEvent(deletedPage, null, PageChangedEvent.IMAGE_STATUS_CLOSED));
                            AppEventHandler.OnTaskProgress(this, new TaskProgressEvent(deletedPage, deletedIndex, deletedPages.Count));
                            
                            tParams.CancelToken.ThrowIfCancellationRequested(); 
                            
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

                                AppEventHandler.OnTaskProgress(this, new TaskProgressEvent(page, deletedIndex, Pages.Count));
                                deletedIndex++;
                            }
                                
                        }

                    }
                    catch (OperationCanceledException)
                    {
                        errorSavingArchive = true;
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
                                    page?.UpdateImageEntry(entry, RandomId.GetInstance().Make());
                                }

                                if (Win_CBZ.Win_CBZSettings.Default.AutoDeleteTempFiles)
                                {
                                    foreach (Page page in tParams.Pages)
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
                AppEventHandler.OnArchiveStatusChanged(this, new ArchiveStatusEvent(this, ArchiveStatusEvent.ARCHIVE_SAVED));
            } else
            {
                AppEventHandler.OnArchiveStatusChanged(this, new ArchiveStatusEvent(this, ArchiveStatusEvent.ARCHIVE_ERROR_SAVING));
            }

            AppEventHandler.OnApplicationStateChanged(this, new ApplicationStatusEvent(this, ApplicationStatusEvent.STATE_READY));
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

            TokenStore.GetInstance().ResetCancellationToken(TokenStore.TOKEN_SOURCE_EXTRACT_ARCHIVE);

            ExtractArchiveThread = new Thread(ExtractArchiveProc);
            ExtractArchiveThread.Start(new ExtractArchiveThreadParams() 
            { 
                OutputPath = outputPath,
                Pages = Pages,
                CancelToken = TokenStore.GetInstance().CancellationTokenSourceForName(TokenStore.TOKEN_SOURCE_EXTRACT_ARCHIVE).Token,
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

            TokenStore.GetInstance().ResetCancellationToken(TokenStore.TOKEN_SOURCE_CBZ_VALIDATION);

            ArchiveValidationThread = new Thread(ValidateProc);
            ArchiveValidationThread.Start(new CBZValidationThreadParams() 
            { 
                ShowDialog = showErrorsDialog, 
                PageIndexVersion = pageIndexVersion,
                Pages = Pages,
                CancelToken = TokenStore.GetInstance().CancellationTokenSourceForName(TokenStore.TOKEN_SOURCE_CBZ_VALIDATION).Token,
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
            AppEventHandler.OnApplicationStateChanged(this, new ApplicationStatusEvent(this, ApplicationStatusEvent.STATE_PROCESSING));

            totalItemsToProcess = tParams.Pages.Count;
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
                foreach (Page page in tParams?.Pages)
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

                    AppEventHandler.OnTaskProgress(this, new TaskProgressEvent(page, progressIndex, totalItemsToProcess));
                    
                    if (tParams.CancelToken.IsCancellationRequested)
                    {
                        break;
                    }
                    
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
                        problems.Add("Metadata->Values: " + key + "'");
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

                String series = Program.ProjectModel.MetaData.ValueForKey("Series");
                if (series != null)
                {
                    if (series.Length == 0)
                    {
                        problems.Add("Metadata->Values->Series: Value missing! Some Comic-Viewers require this field to be filled (i.e. Komga)");
                    }
                }
                else
                {
                    problems.Add("Metadata->Values->Series: Value missing! Some Comic-Viewers require this field to be filled (i.e. Komga)");
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
                        tagValidationFailed = Validation.ValidateTags(ref unknownTags, false, true, progressIndex, totalItemsToProcess, tParams.CancelToken);
                        if (tagValidationFailed)
                        {
                            foreach (String tag in unknownTags)
                            {
                                problems.Add("[Metadata->Values->Tags: Unknown Tag '" + tag + "']");
                            }
                        }
                    }

                    string[] tagList = tags.Split(',');
                    duplicateTags = Validation.ValidateDuplicateStrings(tagList, tParams.CancelToken);

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

            AppEventHandler.OnTaskProgress(this, new TaskProgressEvent(null, 0, 0));

            AppEventHandler.OnApplicationStateChanged(this, new ApplicationStatusEvent(this, ApplicationStatusEvent.STATE_READY));
            AppEventHandler.OnArchiveValidationFinished(this, new ValidationFinishedEvent(problems.ToArray(), tParams.ShowDialog));
        }

        public bool ThreadRunning()
        {
            var a = PageUpdateThread?.IsAlive ?? false;
            var b = LoadArchiveThread?.IsAlive ?? false;
            var c = ExtractArchiveThread?.IsAlive ?? false;
            var d = RenamingThread?.IsAlive ?? false;
            var e = CloseArchiveThread?.IsAlive ?? false;
            var f = SaveArchiveThread?.IsAlive ?? false;
            var g = PageUpdateThread?.IsAlive ?? false;
            var h = ProcessAddedFiles?.IsAlive ?? false;
            var i = ParseAddedFileNames?.IsAlive ?? false;
            var j = RestoreRenamingThread?.IsAlive ?? false;
            var k = AutoRenameThread?.IsAlive ?? false;
            var l = ArchiveValidationThread?.IsAlive ?? false;
            
            return a || b || c || d || e || f || g || h || i || j || k || l;
        }

        public void CancelAllThreads()
        {
            TokenStore.GetInstance().CancellationTokenSourceForName(TokenStore.TOKEN_SOURCE_GLOBAL).Cancel();

        }


        public Page AddPageFromFile(LocalFile localFile, PageIndexVersion version, int insertAt = -1)
        {
            int pageStatus = 0;

            FileInfo targetPath;

            int realNewIndex = Pages.Count;
            Page page;
            Page insertPage = null;

            if (insertAt >= 0 && insertAt < Pages.Count)
            {
                realNewIndex = insertAt;
                insertPage = GetPageByIndex(realNewIndex);
            }

            targetPath = MakeNewTempFileName();

            //CopyFile(fileObject.FullPath, targetPath.FullName);

            page = GetPageByName(localFile.Name);

            if (page == null)
            {
                page = new Page(localFile, targetPath.Directory.FullName, FileAccess.ReadWrite)
                {
                    Number = realNewIndex + 1,
                    Index = realNewIndex,
                    OriginalIndex = realNewIndex,
                    Key = version == PageIndexVersion.VERSION_1 ? localFile.Name : RandomId.GetInstance().Make(),
                };
                pageStatus = PageChangedEvent.IMAGE_STATUS_NEW;
            }
            else
            {
                throw new PageException(page, "Page already exists!", true);
            }

            try
            {
                page.LoadImage(true);    // dont load full image here!
            }
            catch (PageException pe)
            {
                pageStatus = PageChangedEvent.IMAGE_STATUS_ERROR;

                MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_WARNING, "Failed to load image metadata for page ['" + page.Name + "']! [" + pe.Message + "]");
            }
            finally
            {
                page.FreeImage();
            }

            if (insertAt > -1 && insertAt < Pages.Count)
            {
                Pages.Insert(realNewIndex, page);
                
            } else
            {
                Pages.Add(page);
            }

            AppEventHandler.OnPageChanged(this, new PageChangedEvent(page, insertPage, pageStatus));
            
            return page;
        }

        public Page AddPage(Page page, int insertAt = -1, bool insertAfterIndex = false)
        {
            int pageStatus = 0;
            int realNewIndex = Pages.Count;
            Page insertPage = null;

            if (insertAt >= 0 && insertAt < Pages.Count)
            {
                realNewIndex = insertAt;
                insertPage = GetPageByIndex(realNewIndex);
            }

            if (page != null)
            {
                pageStatus = PageChangedEvent.IMAGE_STATUS_NEW;

                try
                {
                    page.LoadImage(true);    // dont load full image here!
                }
                catch (PageException pe)
                {
                    pageStatus = PageChangedEvent.IMAGE_STATUS_ERROR;

                    MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_WARNING, "Failed to load image metadata for page ['" + page.Name + "']! [" + pe.Message + "]");
                }
                finally
                {
                    page.FreeImage();
                }

                page.Index = insertAfterIndex ? realNewIndex + 1 : realNewIndex;
                page.Number = insertAfterIndex ?  realNewIndex + 2 : realNewIndex + 1;

                if (insertAt > -1 && insertAt < Pages.Count)
                {
                    Pages.Insert(insertAfterIndex ? realNewIndex + 1 : realNewIndex, page);

                }
                else
                {
                    Pages.Add(page);
                }

                AppEventHandler.OnPageChanged(this, new PageChangedEvent(page, insertPage, pageStatus));
            }
            else
            {
                throw new PageException(page, "Page already exists!", true);
            }

            
            return page;
        }

        public void AddImagesProc(object threadParams)
        {
            AddImagesThreadParams tParams = threadParams as AddImagesThreadParams;

            int index = tParams.MaxCountPages;
            int realNewIndex = tParams.MaxCountPages;
            int total = tParams?.LocalFiles.Count ?? 0;
            int progressIndex = 0;
            int pageStatus = 0;
            bool pageError = false;
            FileInfo targetPath;
            FileInfo localPath;
            Page page;

            foreach (LocalFile fileObject in tParams?.LocalFiles)
            {
                try
                {
                    localPath = new FileInfo(fileObject.FileName);

                    if (tParams.InvalidFileNames.Contains(localPath.Name.ToLower()))
                    {
                        MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_WARNING, "Skipping file ['" + localPath.Name + "'] because of non-allowed filename!");

                        continue;
                    }

                    if (tParams.FilterExtensions.Contains(localPath.Extension.ToLower()))
                    {
                        MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_WARNING, "Skipping file ['" + localPath.Name + "'] because of filtered file-extension ['" + localPath.Extension.ToLower() + "']!");

                        continue;
                    }

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
                            Key = tParams.PageIndexVerToWrite == PageIndexVersion.VERSION_1 ? fileObject.FileName : RandomId.GetInstance().Make(),
                        };
                        pageStatus = PageChangedEvent.IMAGE_STATUS_NEW;

                        realNewIndex++;
                    }
                    else
                    {
                        if (page.LastModified != fileObject.LastModified || page.Size != fileObject.FileSize)
                        {
                            pageStatus = PageChangedEvent.IMAGE_STATUS_CHANGED;
                            page.Deleted = false;
                            page.FreeImage();
                            page.Invalidated = true;
                            page.Index = page.OriginalIndex;
                            page.ThumbnailInvalidated = true;
                        }
                        page.UpdateLocalWorkingCopy(fileObject, targetPath);
                        page.Key = tParams.PageIndexVerToWrite == PageIndexVersion.VERSION_1 ? fileObject.Name : RandomId.GetInstance().Make();
                        page.Changed = true;
                        page.ImageChanged = true;
                    }

                    try
                    {
                        if (tParams.HashFiles)
                        {
                            HashCrc32.Calculate(ref page);
                        }
                    }
                    catch (Exception pe)
                    {
                        pageStatus = PageChangedEvent.IMAGE_STATUS_ERROR;

                        MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_WARNING, "Failed calculate crc32 for page ['" + page.Name + "']! [" + pe.Message + "]");
                    }
                    finally
                    {
                        //
                    }

                    try
                    {
                        page.ImageTask.ImageAdjustments.Interpolation = Enum.Parse<InterpolationMode>(tParams.Interpolation);
                    }
                    catch (Exception ex)
                    {
                        MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_WARNING, "Failed to set interpolation mode ['" + tParams.Interpolation + "'] for page ['" + page.Name + "']! [" + ex.Message + "]");
                    }

                    try
                    {
                        page.LoadImage(true);    // dont load full image here!

                    }
                    catch (PageException pe)
                    {
                        pageStatus = PageChangedEvent.IMAGE_STATUS_ERROR;

                        MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_WARNING, "Failed to load image metadata for page ['" + page.Name + "']! [" + pe.Message + "]");
                    }
                    finally
                    {
                        page.FreeImage();
                    }

                    if (!page.Changed)
                    {
                        Pages.Add(page);
                    }

                    tParams.CancelToken.ThrowIfCancellationRequested();

                    AppEventHandler.OnPageChanged(this, new PageChangedEvent(page, null, pageStatus));
                    AppEventHandler.OnTaskProgress(this, new TaskProgressEvent(page, progressIndex, total, "Adding image..."));

                    index++;
                    progressIndex++;
                    pageError = false;
                    Thread.Sleep(5);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception ef)
                {
                    MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_ERROR, ef.Message);
                }
                finally
                {
                    if (realNewIndex > MaxFileIndex)
                    {
                        AppEventHandler.OnArchiveStatusChanged(this, new ArchiveStatusEvent(this, ArchiveStatusEvent.ARCHIVE_FILE_ADDED));

                        IsChanged = true;
                        MaxFileIndex = realNewIndex;
                    }
                }
            }

            AppEventHandler.OnOperationFinished(this, new OperationFinishedEvent(progressIndex, Pages.Count));
            AppEventHandler.OnApplicationStateChanged(this, new ApplicationStatusEvent(this, ApplicationStatusEvent.STATE_READY));

            if (tParams.ContinuePipeline && !tParams.CancelToken.IsCancellationRequested)
            {
                AppEventHandler.OnPipelineNextTask(this, new PipelineEvent(this, tParams.GetNextTaskId(), null, tParams.Stack));

            }
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

        public void ParseFiles(List<String> files, 
            bool hashFiles = false, 
            string interpolationMode = "Default",
            bool filterExtensions = false,
            string filterExtensionList = ""
            )
        {

            if (ParseAddedFileNames != null)
            {
                if (ParseAddedFileNames.IsAlive)
                {
                    throw new ApplicationException("File Analyzer- Thread already running!", true);
                }
            }

            TokenStore.GetInstance().ResetCancellationToken(TokenStore.TOKEN_SOURCE_PARSE_FILES);

            ParseAddedFileNames = new Thread(ParseFilesProc);
            ParseAddedFileNames.Start(new ParseFilesThreadParams() 
            { 
                FileNamesToAdd = files,
                HasMetaData = MetaData.Exists(),
                PageIndexVerToWrite = PageIndexVersionWriter,
                CancelToken = TokenStore.GetInstance().CancellationTokenSourceForName(TokenStore.TOKEN_SOURCE_PARSE_FILES).Token,
                Pages = Pages,
                ContinuePipeline = true,
                MaxCountPages = Pages.Count,
                HashFiles = hashFiles,
                Interpolation = interpolationMode,
                FilterExtensions = filterExtensions,
                AllowedExtensions = filterExtensionList.Split('|').ToArray<String>(),
            });
        }

        public void ParseFilesProc(object threadParams)
        {
            ParseFilesThreadParams tParams = threadParams as ParseFilesThreadParams;

            if (tParams?.FileNamesToAdd == null)
            {
                AppEventHandler.OnMessageLogged(this, new LogMessageEvent(LogMessageEvent.LOGMESSAGE_TYPE_WARNING, "Unable to parse files! [ProjectModel::ParseFilesProc(), parameter FileNamesToAdd was NULL] "));

                return; 
            }
                      
            AppEventHandler.OnApplicationStateChanged(this, new ApplicationStatusEvent(this, ApplicationStatusEvent.STATE_ANALYZING));

            List<LocalFile> files = new List<LocalFile>();
            LocalFile fileToAdd = null;
            int index = 0;
            foreach (String fname in tParams?.FileNamesToAdd)
            {

                try
                {
                    fileToAdd = new LocalFile(fname);

                    if (tParams.FilterExtensions)
                    {
                        if (!tParams.AllowedExtensions.Contains(fileToAdd.FileExtension.Trim('.').ToLower()))
                        {
                            MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_INFO, "Skipping file [" + fileToAdd.FileName + "] because of extension-filter! ");
                            index++;

                            AppEventHandler.OnTaskProgress(this, new TaskProgressEvent(null, index, tParams.FileNamesToAdd.Count));

                            Thread.Sleep(5);

                            continue;
                        }
                    }

                    files.Add(new LocalFile(fname));
                    index++;

                    tParams.CancelToken.ThrowIfCancellationRequested();

                    AppEventHandler.OnTaskProgress(this, new TaskProgressEvent(null, index, tParams.FileNamesToAdd.Count));

                    Thread.Sleep(5);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_ERROR, ex.Message);
                }
                finally
                {
                    
                }
            }

            if (!tParams.CancelToken.IsCancellationRequested && tParams.ContinuePipeline)
            {
                // if no stack is provided, create the default one
                if (tParams.Stack == null)
                {
                    tParams.Stack = new List<StackItem>()
                    {
                        new StackItem
                        {
                            TaskId = PipelineEvent.PIPELINE_MAKE_PAGES,
                            ThreadParams = new AddImagesThreadParams
                            {
                                LocalFiles = files.ToList(),
                                PageIndexVerToWrite = tParams.PageIndexVerToWrite,
                                MaxCountPages = tParams.MaxCountPages,
                                CancelToken = tParams.CancelToken,
                                ContinuePipeline = true,
                                HashFiles = tParams.HashFiles,
                                Interpolation = tParams.Interpolation,
                            }
                        },
                        new StackItem
                        {
                            TaskId = tParams.HasMetaData ? PipelineEvent.PIPELINE_UPDATE_INDICES : -1,
                            ThreadParams = new UpdatePageIndicesThreadParams()
                            {
                                ContinuePipeline = false,
                                InitialIndexRebuild = false,
                                Pages = tParams.Pages,
                                CancelToken = tParams.CancelToken,
                            }
                        }
                    };
                }
            }

            AppEventHandler.OnTaskProgress(this, new TaskProgressEvent(null, 0, tParams.FileNamesToAdd.Count));
            AppEventHandler.OnApplicationStateChanged(this, new ApplicationStatusEvent(this, ApplicationStatusEvent.STATE_READY));

            if (!tParams.CancelToken.IsCancellationRequested && tParams.ContinuePipeline)
            {
                AppEventHandler.OnPipelineNextTask(this, new PipelineEvent(this, tParams.GetNextTaskId(), files, tParams.Stack));
            }                     
        }

        // <deprecated></deprecated>
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

        // <deprecated></deprecated>
        public void UpdatePageIndices(List<Page> pages, bool initialIndexBulid, bool continuePipeline = false, List<StackItem> stack = null)
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
                Pages = pages,
                ContinuePipeline = continuePipeline,
                InitialIndexRebuild = initialIndexBulid,
                Stack = stack ?? new List<StackItem> { new StackItem() { TaskId = 0, ThreadParams = null } }
            });
        }

        // <deprecated></deprecated>
        protected void UpdatePageIndicesProc(object threadParams)
        {
            UpdatePageIndicesThreadParams tParams = threadParams as UpdatePageIndicesThreadParams;

            if (tParams?.Pages == null)
            {
                AppEventHandler.OnMessageLogged(this, new LogMessageEvent(LogMessageEvent.LOGMESSAGE_TYPE_WARNING, "Unable to update page indices! [ProjectModel::UpdatePageIndicesProc(), parameter PAGES was NULL] "));

                return;
            }

            int newIndex = 0;
            int updated = 1;
            bool isUpdated = false;

            AppEventHandler.OnApplicationStateChanged(this, new ApplicationStatusEvent(this, ApplicationStatusEvent.STATE_UPDATING_INDEX));

            foreach (Page page in tParams?.Pages)
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

                if (!tParams.InitialIndexRebuild && isUpdated)
                {
                    AppEventHandler.OnPageChanged(this, new PageChangedEvent(page, null, PageChangedEvent.IMAGE_STATUS_CHANGED));
                }

                AppEventHandler.OnGeneralTaskProgress(this, new GeneralTaskProgressEvent(GeneralTaskProgressEvent.TASK_UPDATE_PAGE_INDEX, GeneralTaskProgressEvent.TASK_STATUS_RUNNING, "Rebuilding index...", updated, Pages.Count, true));
                //AppEventHandler.OnArchiveStatusChanged(this, new ArchiveStatusEvent(this, ArchiveStatusEvent.ARCHIVE_FILE_UPDATED));

                IsChanged = true;
                isUpdated = false;
                updated++;

                Thread.Sleep(5);
            }

            MaxFileIndex = newIndex;

            AppEventHandler.OnGeneralTaskProgress(this, new GeneralTaskProgressEvent(GeneralTaskProgressEvent.TASK_UPDATE_PAGE_INDEX, GeneralTaskProgressEvent.TASK_STATUS_COMPLETED, "Ready", 0, Pages.Count, true));
            AppEventHandler.OnApplicationStateChanged(this, new ApplicationStatusEvent(this, ApplicationStatusEvent.STATE_READY));

            Thread.Sleep(50);

            if (tParams.ContinuePipeline || tParams.Stack.Count == 0)
            {
                AppEventHandler.OnArchiveStatusChanged(this, new ArchiveStatusEvent(this, ArchiveStatusEvent.ARCHIVE_READY));
            }

            if (tParams.ContinuePipeline)
            {
                AppEventHandler.OnPipelineNextTask(this, new PipelineEvent(this, PipelineEvent.PIPELINE_UPDATE_INDICES, null, tParams.Stack));
            }         
        }

        public int GetPageCount()
        {
            var res = Pages.Where(p => !p.Deleted).ToList();

            return res.Count;
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

        public Page GetPageByHash(String hash)
        {
            foreach (Page page1 in Pages)
            {
                if (page1.Hash == hash)
                {
                    return page1;
                }
            }

            return null;
        }

        public List<Page> GetPagesByHash(String hash)
        {
            List<Page> pages = new List<Page>();

            foreach (Page page1 in Pages)
            {
                if (page1.Hash == hash)
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

            TokenStore.GetInstance().ResetCancellationToken(TokenStore.TOKEN_SOURCE_AUTO_RENAME);

            AutoRenameThread = new Thread(AutoRenameAllPagesProc);
            AutoRenameThread.Start(new RenamePagesThreadParams()
            {
                Pages = Pages,
                ApplyRenaming = true,
                CompatibilityMode = false,
                IgnorePageNameDuplicates = false,
                RenameStoryPagePattern = RenameStoryPagePattern,
                RenameSpecialPagePattern = RenameSpecialPagePattern,
                ContinuePipeline = false,
                Stack = null,
                CancelToken = TokenStore.GetInstance().CancellationTokenSourceForName(TokenStore.TOKEN_SOURCE_AUTO_RENAME).Token
            });
        }

        public void AutoRenameAllPagesProc(object threadParams)
        {
            RenamePagesThreadParams tParams = threadParams as RenamePagesThreadParams;

            if (tParams?.Pages == null)
            {
                AppEventHandler.OnMessageLogged(this, new LogMessageEvent(LogMessageEvent.LOGMESSAGE_TYPE_WARNING, "Unable to apply renaming! [ProjectModel::AutoRenameAllPagesProc(), parameter PAGES was NULL] "));

                return;
            }

            AppEventHandler.OnApplicationStateChanged(this, new ApplicationStatusEvent(this, ApplicationStatusEvent.STATE_RENAMING));

            foreach (Page page in tParams?.Pages)
            {
                if (RenamerExcludes.IndexOf(page.Name) == -1 && !page.Deleted)
                {
                    PageScriptRename(page, tParams.IgnorePageNameDuplicates, tParams.RenameStoryPagePattern, tParams.RenameSpecialPagePattern);

                    AppEventHandler.OnTaskProgress(this, new TaskProgressEvent(page, page.Index + 1, Pages.Count));
                }
            }

            AppEventHandler.OnOperationFinished(this, new OperationFinishedEvent(0, Pages.Count));
            AppEventHandler.OnApplicationStateChanged(this, new ApplicationStatusEvent(this, ApplicationStatusEvent.STATE_READY));
        
            if (tParams.ContinuePipeline && tParams.Stack != null)
            {
                AppEventHandler.OnPipelineNextTask(this, new PipelineEvent(this, PipelineEvent.PIPELINE_RUN_RENAMING, null, tParams.Stack));
            }
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

            TokenStore.GetInstance().ResetCancellationToken(TokenStore.TOKEN_SOURCE_RESTORE_RENAMING);

            RestoreRenamingThread = new Thread(RestoreOriginalNamesProc);
            RestoreRenamingThread.Start(new RestoreRenamedThreadParams()
            {
                Pages = Pages,
                CancelToken = TokenStore.GetInstance().CancellationTokenSourceForName(TokenStore.TOKEN_SOURCE_RESTORE_RENAMING).Token
            });
        }

        public void RestoreOriginalNamesProc(object threadParams)
        {
            RestoreRenamedThreadParams tParams = threadParams as RestoreRenamedThreadParams;

            if (tParams?.Pages == null)
            {
                AppEventHandler.OnMessageLogged(this, new LogMessageEvent(LogMessageEvent.LOGMESSAGE_TYPE_WARNING, "Unable to restore renaming! [ProjectModel::RestoreOriginalNamesProc(), parameter PAGES was NULL] "));

                return;
            }

            AppEventHandler.OnApplicationStateChanged(this, new ApplicationStatusEvent(this, ApplicationStatusEvent.STATE_RENAMING));

            foreach (Page page in tParams.Pages)
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

                        AppEventHandler.OnTaskProgress(this, new TaskProgressEvent(page, page.Index + 1, Pages.Count));
                    }
                }
            }

            AppEventHandler.OnOperationFinished(this, new OperationFinishedEvent(0, Pages.Count));
            AppEventHandler.OnApplicationStateChanged(this, new ApplicationStatusEvent(this, ApplicationStatusEvent.STATE_READY));
        }
        
        public void RenamePage(Page page, String name, bool ignoreDuplicates = false, bool showErrors = false)
        {
            Page originalPage = new Page();
            originalPage.UpdatePageAttributes(page);

            if (name == null || name == "")
            {
                throw new PageException(page, "Failed to rename page ['" + page.Name + "'] with ID [" + page.Id + "]! The new name must not be NULL.", showErrors);
            }

            if (FilteredFileNames.Contains(name.ToLower()))
            {
                throw new ApplicationException("Failed to rename page ['" + page.Name + "'] with ID [" + page.Id + "]! The new name [" + name + "] is not permitted.", showErrors);
            }

            if (!ignoreDuplicates)
            {
                List<Page> existing = Pages.FindAll(p => p.Name.ToLower() == name.ToLower());

                foreach (Page existingPage in existing)
                {
                    if (existingPage.Id != page.Id)
                    {              
                        throw new PageDuplicateNameException(page, "Failed to rename page ['" + page.Name + "'] with ID [" + page.Id + "]! A different page with the same name already exists at Index " + existingPage.Index + ".", showErrors);
                    }
                }
            }

            //Page oldPage = new Page(page);  // dont create new page obj - file will get locked

            if (!page.Name.Equals(name))
            {
                page.OriginalName = page.Name;
                page.Name = name;
                if (MetaDataVersionFlavorHandler.GetInstance().HandlePageIndexVersion() == PageIndexVersion.VERSION_2)
                {
                    page.Key = name;
                }

                IsChanged = true;

                AppEventHandler.OnPageChanged(this, new PageChangedEvent(page, originalPage, PageChangedEvent.IMAGE_STATUS_RENAMED));
                AppEventHandler.OnArchiveStatusChanged(this, new ArchiveStatusEvent(this, ArchiveStatusEvent.ARCHIVE_FILE_RENAMED));
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

                case "{title}":
                    return MetaData.ValueForKey("Title");

                case "{ext}":
                    return page.FileExtension.TrimStart('.');

                case "{index}":
                    return page.Index.ToString();

                case "{number}":
                    return page.Number.ToString();

                case "{page}":
                    return FormatLeadingZeros(page.Index + 1, Pages.Count).ToString();

                case "{pages}":
                    return Pages.Count.ToString();

                case "{size}":
                    return page.SizeFormat().Replace(" ", "_");

                case "{type}":
                    return TransformImageType(page.ImageType.ToString());

                case "{year}":
                    return MetaData.ValueForKey("Year");

                case "{month}":
                    return MetaData.ValueForKey("Month");

                case "{day}":
                    return MetaData.ValueForKey("Day");

                case "{hash}":
                    return page.Hash;

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

        /// <deprecated></deprecated>
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>DEPRECATED</remarks>
        /// <param name="page"></param>
        /// 
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

            if (tParams?.Pages == null)
            {
                AppEventHandler.OnMessageLogged(this, new LogMessageEvent(LogMessageEvent.LOGMESSAGE_TYPE_WARNING, "Unable to run renaming-script! [ProjectModel::RunRenameScriptsForPages(), parameter PAGES was NULL] "));

                return;
            }

            AppEventHandler.OnApplicationStateChanged(this, new ApplicationStatusEvent(this, ApplicationStatusEvent.STATE_RENAMING));

            foreach (Page page in tParams?.Pages)
            {
                if (!page.Deleted)
                {
                    // todo: evaluate if renaming should exclude files for compat-mode
                    if (tParams.CompatibilityMode && !Page.NameEqualsIndex(page) && RenamerExcludes.IndexOf(page.Name) == -1)
                    {
                        RenamePageScript(page, tParams.IgnorePageNameDuplicates, tParams.SkipIndexUpdate, tParams.RenameStoryPagePattern, tParams.RenameSpecialPagePattern);

                        AppEventHandler.OnTaskProgress(this, new TaskProgressEvent(page, page.Index + 1, Pages.Count));
                        AppEventHandler.OnArchiveStatusChanged(this, new ArchiveStatusEvent(this, ArchiveStatusEvent.ARCHIVE_FILE_RENAMED));

                        Thread.Sleep(5);
                    }
                }
            }

            if (tParams.ContinuePipeline && !tParams.CancelToken.IsCancellationRequested)
            {
                foreach (StackItem si in tParams.Stack)
                {
                    if (si.ThreadParams is UpdatePageIndicesThreadParams)
                    {
                        (si.ThreadParams as UpdatePageIndicesThreadParams).Pages = tParams.Pages;
                    }

                    if (si.ThreadParams is SaveArchiveThreadParams)
                    {
                        (si.ThreadParams as SaveArchiveThreadParams).Pages = tParams.Pages;
                    }
                }

                AppEventHandler.OnPipelineNextTask(this, new PipelineEvent(this, tParams.GetNextTaskId(), null, tParams.Stack, null));
            }

            AppEventHandler.OnApplicationStateChanged(this, new ApplicationStatusEvent(this, ApplicationStatusEvent.STATE_READY));
        }

        /// <deprecated></deprecated>
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
                    AppEventHandler.OnItemExtracted(this, new ItemExtractedEvent(1, 1, NewTemporaryFileName.FullName));
                } else
                {
                    MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_WARNING, "No Entry with name [" + page.EntryName + "] exists in archive!");
                }
            }
            catch (Exception efile)
            {
                MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_ERROR, "Error extracting File [" + page.EntryName + "] from Archive [" + efile.Message + "]");
            }
        }

        public FileInfo MakeNewTempFileName(String extension = "")
        {
            return new FileInfo(Path.Combine(PathHelper.ResolvePath(WorkingDir), ProjectGUID, RandomId.GetInstance().Make() + extension + ".tmp"));
        }

        protected DirectoryInfo MakeTempDirectory(String name = "_tmp")
        {
            return Directory.CreateDirectory(Path.Combine(PathHelper.ResolvePath(WorkingDir), ProjectGUID, name, RandomId.GetInstance().Make()));

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


        /// <deprecated>use copyfile task</deprecated>
        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputFilePath"></param>
        /// <param name="outputFilePath"></param>
        /// <param name="propagateEvents"></param>
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
                        AppEventHandler.OnFileOperation(this, new FileOperationEvent(FileOperationEvent.OPERATION_COPY, FileOperationEvent.STATUS_RUNNING, byesTotal, fs.Length));
                    }
                    Thread.Sleep(1);
                }

                fs.Close();
            }

            if (propagateEvents)
            {
                AppEventHandler.OnFileOperation(this, new FileOperationEvent(FileOperationEvent.OPERATION_COPY, FileOperationEvent.STATUS_SUCCESS, 0, 100));
            }
        }


        protected void ExtractArchiveProc(object threadParams)
        {
            ExtractArchiveThreadParams tparams = threadParams as ExtractArchiveThreadParams;

            AppEventHandler.OnArchiveStatusChanged(this, new ArchiveStatusEvent(this, ArchiveStatusEvent.ARCHIVE_EXTRACTING));

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
                                AppEventHandler.OnItemExtracted(this, new ItemExtractedEvent(index, Pages.Count, Path.Combine(PathHelper.ResolvePath(di.FullName), fileEntry.Name)));
                            }
                            else
                            {
                                fileEntry.ExtractToFile(Path.Combine(PathHelper.ResolvePath(WorkingDir), ProjectGUID, fileEntry.Name), true);
                                page.TemporaryFile = new LocalFile(Path.Combine(PathHelper.ResolvePath(WorkingDir), ProjectGUID, fileEntry.Name));
                                AppEventHandler.OnItemExtracted(this, new ItemExtractedEvent(index, Pages.Count, Path.Combine(PathHelper.ResolvePath(WorkingDir), ProjectGUID, fileEntry.Name)));
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
                        AppEventHandler.OnTaskProgress(this, new TaskProgressEvent(page, index, Pages.Count));
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

            AppEventHandler.OnArchiveStatusChanged(this, new ArchiveStatusEvent(this, ArchiveStatusEvent.ARCHIVE_EXTRACTED));
        }
        
        public void Close(Task followUpTask = null, Task finalTask = null)
        {
            List<Thread> threads = new List<Thread>();

            if (LoadArchiveThread != null)
            {
                if (LoadArchiveThread.IsAlive)
                {
                    threads.Add(LoadArchiveThread); 
                }
            }

            if (SaveArchiveThread != null)
            {
                if (SaveArchiveThread.IsAlive)
                {
                    threads.Add(SaveArchiveThread);
                }
            }

            if (CloseArchiveThread != null)
            {
                if (CloseArchiveThread.IsAlive)
                {
                    threads.Add(CloseArchiveThread);
                }
            }

            TokenStore.GetInstance().CancellationTokenSourceForName(TokenStore.TOKEN_SOURCE_THUMBNAIL_SLICE).Cancel();
            TokenStore.GetInstance().ResetCancellationToken(TokenStore.TOKEN_SOURCE_AWAIT_THREADS);

            Task<TaskResult> awaitClosingArchive = AwaitOperationsTask.AwaitOperations(threads, 
                AppEventHandler.OnGeneralTaskProgress,
                TokenStore.GetInstance().RequestCancellationToken(TokenStore.TOKEN_SOURCE_AWAIT_THREADS),
                true
                );

            awaitClosingArchive.ContinueWith(t => {
                if (t.IsCompleted && t.Result.Status == 0)
                {
                    CloseArchiveThread = new Thread(new ThreadStart(CloseArchiveProc));
                    CloseArchiveThread.Start();

                    if (followUpTask != null)
                    {
                        Task<TaskResult> follow = AwaitOperationsTask.AwaitOperations(new List<Thread>() { CloseArchiveThread },
                            AppEventHandler.OnGeneralTaskProgress,
                            TokenStore.GetInstance().RequestCancellationToken(TokenStore.TOKEN_SOURCE_AWAIT_THREADS),
                            true
                            );

                        follow.ContinueWith(t => {

                            if (t.IsCompletedSuccessfully)
                            {
                                followUpTask.ContinueWith(t =>
                                {
                                    finalTask?.Start();
                                });

                                followUpTask.Start();
                            }
                        });

                        follow.Start();
                    }
                }
            });

            awaitClosingArchive.Start();
        }

        protected void CloseArchiveProc()
        {
            AppEventHandler.OnArchiveStatusChanged(this, new ArchiveStatusEvent(this, ArchiveStatusEvent.ARCHIVE_CLOSING));

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
                            AppEventHandler.OnPageChanged(this, new PageChangedEvent(page, null, PageChangedEvent.IMAGE_STATUS_CLOSED));
                            AppEventHandler.OnArchiveStatusChanged(this, new ArchiveStatusEvent(this, ArchiveStatusEvent.ARCHIVE_CLOSING));
                            AppEventHandler.OnTaskProgress(this, new TaskProgressEvent(page, page.Index, Pages.Count));
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

            //AppEventHandler.OnArchiveStatusChanged(this, new ArchiveStatusEvent(this, ArchiveStatusEvent.ARCHIVE_CLOSED));
        }

        public void ClearTempFolder(String path)
        {

            if (LoadArchiveThread != null)
            {
                if (LoadArchiveThread.IsAlive)
                {
                    throw new ConcurrentOperationException("There are still operations running in the Background.\r\nPlease wait until those have completed and try again!", true);
                    //TokenStore.GetInstance().CancellationTokenSourceForName(TokenStore.TOKEN_SOURCE_LOAD_ARCHIVE).Cancel();
                }
            }

            if (CloseArchiveThread != null)
            {
                if (CloseArchiveThread.IsAlive)
                {
                    throw new ConcurrentOperationException("There are still operations running in the Background.\r\nPlease wait until those have completed and try again!", true);
                    //TokenStore.GetInstance().CancellationTokenSourceForName(TokenStore.TOKEN_SOURCE_CLOSE_ARCHIVE).Cancel();
                }
            }

            if (ExtractArchiveThread != null)
            {
                if (ExtractArchiveThread.IsAlive)
                {
                    throw new ConcurrentOperationException("There are still operations running in the Background.\r\nPlease wait until those have completed and try again!", true);
                    //TokenStore.GetInstance().CancellationTokenSourceForName(TokenStore.TOKEN_SOURCE_EXTRACT_ARCHIVE).Cancel();
                }
            }

            if (SaveArchiveThread != null)
            {
                if (SaveArchiveThread.IsAlive)
                {
                    throw new ConcurrentOperationException("There are still operations running in the Background.\r\nPlease wait until those have completed and try again!", true);
                    //TokenStore.GetInstance().CancellationTokenSourceForName(TokenStore.TOKEN_SOURCE_SAVE_ARCHIVE).Cancel();
                }
            }

            if (DeleteFileThread != null)
            {
                if (DeleteFileThread.IsAlive)
                {
                    throw new ConcurrentOperationException("There are still operations running in the Background.\r\nPlease wait until those have completed and try again!", true);
                    //TokenStore.GetInstance().CancellationTokenSourceForName(TokenStore.TOKEN_SOURCE_DELETE_FILE).Cancel();
                }
            }

            TokenStore.GetInstance().ResetCancellationToken(TokenStore.TOKEN_SOURCE_DELETE_FILE);

            DeleteFileThread = new Thread(DeleteTempFolderItems);
            DeleteFileThread.Start(new DeleteTemporaryFilesThreadParams() 
            { 
                Path = path,
                CancelToken = TokenStore.GetInstance().CancellationTokenSourceForName(TokenStore.TOKEN_SOURCE_DELETE_FILE).Token
            });
        }      

        public void DeleteTempFolderItems(object threadParams)
        {
            DeleteTemporaryFilesThreadParams tParams = threadParams as DeleteTemporaryFilesThreadParams;

            List<string> filesToDelete = new List<string>();
            List<string> foldersToDelete = new List<string>();
            String path = PathHelper.ResolvePath(tParams.Path);
            int fileIndex = 0;
            int filesDeletedCount = 0;
            int filesFailed = 0;
            long totalSize = 0;

            AppEventHandler.OnApplicationStateChanged(this, new ApplicationStatusEvent(this, ApplicationStatusEvent.STATE_PROCESSING));

            List<FileInfo> files = new List<FileInfo>();

            DirectoryInfo dir = new DirectoryInfo(path);

            List<DirectoryInfo> folders = new List<DirectoryInfo>(dir.EnumerateDirectories());


            try
            {
                if (dir.Exists && ProjectGUID != null && ProjectGUID.Length > 0)
                {
                    // fail safe! are we in the right directory? if not we would delete random directories and files!
                    if (dir.Parent.Name.ToString().ToLower().Equals(Assembly.GetExecutingAssembly().GetName().Name.ToLower()) &&
                        dir.Parent.Parent.Name.ToString().ToLower().Equals("roaming"))
                    {

                        foreach (DirectoryInfo folder in folders)
                        {
                            if (folder.Name != ProjectGUID)
                            {
                                files.AddRange(folder.GetFiles());
                                foldersToDelete.Add(folder.FullName);

                                tParams.CancelToken.ThrowIfCancellationRequested();
                            }
                        }

                        long fileLen = 0;

                        foreach (FileInfo file in files)
                        {
                            if (file.Exists)
                            {
                                try
                                {
                                    fileLen = file.Length;
                                    file.Delete();
                                    totalSize += fileLen;
                                    filesDeletedCount++;
                                }
                                catch
                                {
                                    filesFailed++;
                                }
                            }

                            tParams.CancelToken.ThrowIfCancellationRequested();

                            AppEventHandler.OnGeneralTaskProgress(this, new GeneralTaskProgressEvent(GeneralTaskProgressEvent.TASK_DELETE_FILE, GeneralTaskProgressEvent.TASK_STATUS_RUNNING, "Clearing Cache...", fileIndex, files.Count, false));
                            Thread.Sleep(2);
                            fileIndex++;
                        }

                        foreach (DirectoryInfo folder in folders)
                        {
                            if (folder.Name != ProjectGUID)
                            {
                                folder.Delete(false);

                                tParams.CancelToken.ThrowIfCancellationRequested();
                            }
                        }
                    }
                }
            }
            catch (OperationCanceledException oce)
            {                 
                MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_WARNING, "Operation canceled [" + oce.Message + "]");
            }
            catch (Exception e)
            {
                MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_ERROR, "Error deleting cache file [" + e.Message + "]");
            }
            finally
            {
                ApplicationMessage.Show("Applicaiton cache cleared.\r\nFiles deleted: " + filesDeletedCount.ToString() + ",\r\nFiles failed/skipped: " + filesFailed.ToString() + "\r\nDisk space reclaimed: " + SizeFormat(totalSize), "Application Cache cleared", ApplicationMessage.DialogType.MT_INFORMATION, ApplicationMessage.DialogButtons.MB_OK);
            }

            AppEventHandler.OnGeneralTaskProgress(this, new GeneralTaskProgressEvent(GeneralTaskProgressEvent.TASK_DELETE_FILE, GeneralTaskProgressEvent.TASK_STATUS_COMPLETED, "", 0, 100, false));
            AppEventHandler.OnApplicationStateChanged(this, new ApplicationStatusEvent(this, ApplicationStatusEvent.STATE_READY));
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
    }
}
