using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Win_CBZ.Data;
using Win_CBZ.Handler;
using Win_CBZ.Helper;

namespace Win_CBZ.Tasks
{
    [SupportedOSPlatform("windows")]
    internal class UpdateMetadataTask
    {
        public static Task<TaskResult> UpdatePageMetadata(List<Page> pages, MetaData metaData, MetaData.PageIndexVersion pageIndexVersion, EventHandler<GeneralTaskProgressEvent> handler, EventHandler<PageChangedEvent> pageChangedHandler, CancellationToken cancellationToken)
        {
            return new Task<TaskResult>((token) =>
            {
                int current = 1;
                int total = pages.Count;
                TaskResult result = new TaskResult();
              
                List<MetaDataEntryPage> originalPageMetaData = metaData.PageIndex.ToList<MetaDataEntryPage>();

                result.Total = total;
                result.Completed = 0;

                if (pages == null)
                {
                    AppEventHandler.OnMessageLogged(null, new LogMessageEvent(LogMessageEvent.LOGMESSAGE_TYPE_WARNING, "Unable to rebuild Index! [MetaData::RebuildPageMetaData(), parameter PAGES was NULL] "));

                    return result;
                }

                metaData.PageIndex.Clear();

                foreach (Page page in pages)
                {
                    try
                    {
                        if (!page.Deleted)
                        {
                            MetaDataEntryPage newPageEntry = MetaDataVersionFlavorHandler.GetInstance().CreateIndexEntry(page);

                            newPageEntry
                                .SetAttribute(MetaDataEntryPage.COMIC_PAGE_ATTRIBUTE_TYPE, page.ImageType)
                                .SetAttribute(MetaDataEntryPage.COMIC_PAGE_ATTRIBUTE_IMAGE_SIZE, page.Size.ToString())
                                .SetAttribute(MetaDataEntryPage.COMIC_PAGE_ATTRIBUTE_DOUBLE_PAGE, page.DoublePage.ToString());

                            if (page.Format.W > 0 && page.Format.H > 0)
                            {
                                newPageEntry.SetAttribute(MetaDataEntryPage.COMIC_PAGE_ATTRIBUTE_IMAGE_WIDTH, page.Format.W.ToString())
                                    .SetAttribute(MetaDataEntryPage.COMIC_PAGE_ATTRIBUTE_IMAGE_HEIGHT, page.Format.H.ToString());
                            }

                            metaData.PageIndex.Add(newPageEntry);

                            handler?.Invoke(null, new GeneralTaskProgressEvent(
                                    GeneralTaskProgressEvent.TASK_UPDATE_PAGE_INDEX,
                                    GeneralTaskProgressEvent.TASK_STATUS_RUNNING,
                                    "Updating index...",
                                    current,
                                    total,
                                    true));

                            result.Completed = current;

                            if (((CancellationToken)token).IsCancellationRequested) 
                            {
                                break;
                            }

                            current++;

                            System.Threading.Thread.Sleep(3);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_WARNING, "Error rebuilding <pages> metadata for pagee->" + page.Name + "! [" + ex.Message + "]");
                        //throw new MetaDataPageEntryException(newPageEntry, "Error rebuilding <pages> metadata for page->" + page.Name + "! [" + ex.Message + "]");
                    }
                }

                metaData.IndexVersionSpecification = pageIndexVersion;

                handler?.Invoke(null, new GeneralTaskProgressEvent(
                        GeneralTaskProgressEvent.TASK_UPDATE_PAGE_INDEX,
                        GeneralTaskProgressEvent.TASK_STATUS_COMPLETED,
                        "Ready.",
                        current,
                        total,
                        true));

                return result;
            }, cancellationToken);
        }

    }
}
