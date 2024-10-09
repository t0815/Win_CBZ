using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Win_CBZ.Events;

namespace Win_CBZ.Handler
{
    internal static class AppEventHandler
    {
        // events
        public static event EventHandler<LogMessageEvent> MessageLogged;

        public static event EventHandler<TaskProgressEvent> TaskProgress;

        public static event EventHandler<PageChangedEvent> PageChanged;

        public static event EventHandler<ApplicationStatusEvent> ApplicationStateChanged;

        public static event EventHandler<ArchiveStatusEvent> ArchiveStatusChanged;

        public static event EventHandler<MetaDataLoadEvent> MetaDataLoaded;

        public static event EventHandler<MetaDataChangedEvent> MetaDataChanged;

        public static event EventHandler<MetaDataEntryChangedEvent> MetaDataEntryChanged;

        public static event EventHandler<OperationFinishedEvent> OperationFinished;

        public static event EventHandler<ItemExtractedEvent> ItemExtracted;

        public static event EventHandler<FileOperationEvent> FileOperation;

        public static event EventHandler<ArchiveOperationEvent> ArchiveOperation;

        public static event EventHandler<GlobalActionRequiredEvent> GlobalActionRequired;

        public static event EventHandler<GeneralTaskProgressEvent> GeneralTaskProgress;

        public static event EventHandler<PipelineEvent> PipelineEventHandler;

        public static event EventHandler<ValidationFinishedEvent> CBZValidationEventHandler;

        public static event EventHandler<RedrawThumbEvent> RedrawThumbnail;

        public static event EventHandler<UpdateThumbnailsEvent> UpdateThumbnails;

        public static event EventHandler<UpdatePageListViewSortingEvent> UpdateListViewSorting;

        public static event EventHandler<ImageAdjustmentsChangedEvent> ImageAdjustmentsChanged;

        // delegates definitions
        public delegate void GeneralTaskProgressDelegate(object sender, GeneralTaskProgressEvent e);

        public delegate void TaskProgressDelegate(object sender, TaskProgressEvent e);

        public delegate void PipelineNextTaskDelegate(object sender, PipelineEvent e);

        public delegate void ArchiveValidationFinishedDelegate(object sender, ValidationFinishedEvent e);

        public delegate void MetaDataEntryChangedDelegate(object sender, MetaDataEntryChangedEvent e);

        // handler
        public static void OnApplicationStateChanged(object sender, ApplicationStatusEvent e)
        {
            ApplicationStateChanged?.Invoke(sender, e);
        }

        public static void OnGeneralTaskProgress(object sender, GeneralTaskProgressEvent e)
        {
            GeneralTaskProgress?.Invoke(sender, e);
        }

        public static void OnTaskProgress(object sender, TaskProgressEvent e)
        {
            TaskProgress?.Invoke(sender, e);
        }

        public static void OnGlobalActionRequired(object sender, GlobalActionRequiredEvent e)
        {
            GlobalActionRequired?.Invoke(sender, e);
        }

        public static void OnRedrawThumb(object sender, RedrawThumbEvent e)
        {
            RedrawThumbnail?.Invoke(sender, e);
        }

        public static void OnUpdateThumbnails(object sender, UpdateThumbnailsEvent e)
        {
            UpdateThumbnails?.Invoke(sender, e);
        }

        public static void OnUpdateListViewSorting(object sender, UpdatePageListViewSortingEvent e)
        {
            UpdateListViewSorting?.Invoke(sender, e);
        }

        public static void OnArchiveValidationFinished(object sender, ValidationFinishedEvent e)
        {
            CBZValidationEventHandler?.Invoke(sender, e);
        }

        public static void OnPipelineNextTask(object sender, PipelineEvent e)
        {
            PipelineEventHandler?.Invoke(sender, e);
        }

        public static void OnPageChanged(object sender, PageChangedEvent e)
        {
            PageChanged?.Invoke(sender, e);
        }

        public static void OnMetaDataEntryChanged(object sender, MetaDataEntryChangedEvent e)
        {
            MetaDataEntryChanged?.Invoke(sender, e);
        }

        public static void OnMetaDataLoaded(object sender, MetaDataLoadEvent e)
        {
            MetaDataLoaded?.Invoke(sender, e);
        }

        public static void OnMetaDataChanged(object sender, MetaDataChangedEvent e)
        {
            MetaDataChanged?.Invoke(sender, e);
        }

        public static void OnArchiveStatusChanged(object sender, ArchiveStatusEvent e)
        {
            ArchiveStatusChanged?.Invoke(sender, e);
        }

        public static void OnOperationFinished(object sender, OperationFinishedEvent e)
        {
            OperationFinished?.Invoke(sender, e);
        }

        public static void OnItemExtracted(object sender, ItemExtractedEvent e)
        {
            ItemExtracted?.Invoke(sender, e);
        }

        public static void OnFileOperation(object sender, FileOperationEvent e)
        {
            FileOperation?.Invoke(sender, e);
        }

        public static void OnArchiveOperation(object sender, ArchiveOperationEvent e)
        {
            ArchiveOperation?.Invoke(sender, e);
        }

        public static void OnMessageLogged(object sender, LogMessageEvent e)
        {
            MessageLogged?.Invoke(sender, e);
        }

        public static void OnImageAdjustmentsChanged(object sender, ImageAdjustmentsChangedEvent e)
        {
            ImageAdjustmentsChanged?.Invoke(sender, e);
        }
    }
}
