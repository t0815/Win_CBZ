using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using Win_CBZ.Data;
using Win_CBZ.Helper;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Win_CBZ.Tasks
{
    [SupportedOSPlatform("windows")]
    internal class RebuildPageIndexMetaDataTask
    {

        public static Task<TaskResult> UpdatePageIndexMetadata(List<Page> pages, MetaData metaData, MetaData.PageIndexVersion pageIndexVersion, EventHandler<GeneralTaskProgressEvent> handler, EventHandler<PageChangedEvent> pageChangedHandler)
        {
            return new Task<TaskResult>(() =>
            {
                bool isUpdated = false;
                int newIndex = 0;
                int current = 1;
                int total = pages.Count;
                TaskResult result = new TaskResult();

                foreach (Page page in pages)
                {
                    if (page.Key == null)
                    {
                        page.Key = RandomId.GetInstance().Make();
                        isUpdated = true;
                    }


                    if (page.Deleted)
                    {
                        page.Index = -1;
                        page.Number = -1;
                        isUpdated = true;
                    }
                    else
                    {
                        if (page.Index != newIndex)
                        {
                            isUpdated = true;
                        }
                        page.Index = newIndex;
                        //page.OriginalIndex = NewIndex;
                        page.Number = newIndex + 1;

                        newIndex++;
                    }

                    if (pageChangedHandler != null)
                    {
                        if (isUpdated)
                        {
                            pageChangedHandler.Invoke(null, new PageChangedEvent(page, null, PageChangedEvent.IMAGE_STATUS_CHANGED));
                        }
                    }

                    if (handler != null)
                    {
                        handler.Invoke(null, new GeneralTaskProgressEvent(
                            GeneralTaskProgressEvent.TASK_UPDATE_PAGE_INDEX, 
                            GeneralTaskProgressEvent.TASK_STATUS_RUNNING, 
                            "Rebuilding index...",
                            current, 
                            total,
                            true));
                    }
                    current++;
                    isUpdated = false;
                    System.Threading.Thread.Sleep(5);
                }

                metaData.RebuildPageMetaData(pages, pageIndexVersion);

                if (handler != null)
                {
                    handler.Invoke(null, new GeneralTaskProgressEvent(
                        GeneralTaskProgressEvent.TASK_UPDATE_PAGE_INDEX,
                        GeneralTaskProgressEvent.TASK_STATUS_COMPLETED,
                        "Ready.",
                        current,
                        total,
                        true));
                }

                return result;
            });
        }
    }
}
