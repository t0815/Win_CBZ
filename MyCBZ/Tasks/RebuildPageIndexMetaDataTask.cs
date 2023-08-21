using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Win_CBZ.Data;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Win_CBZ.Tasks
{
    internal class RebuildPageIndexMetaDataTask
    {

        public static Task<TaskResult> UpdatePageIndexMetadata(List<Page> pages, MetaData metaData, EventHandler<GeneralTaskProgressEvent> handler, EventHandler<PageChangedEvent> pageChangedHandler)
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
                        page.Number = newIndex + 1;
                        newIndex++;
                    }

                    if (pageChangedHandler != null && isUpdated)
                    {
                        pageChangedHandler.Invoke(null, new PageChangedEvent(page, PageChangedEvent.IMAGE_STATUS_CHANGED));
                    }


                    if (handler != null)
                    {
                        handler.Invoke(null, new GeneralTaskProgressEvent(
                            GeneralTaskProgressEvent.TASK_UPDATE_PAGE_INDEX, 
                            GeneralTaskProgressEvent.TASK_STATUS_RUNNING, 
                            "Rebuilding index...",
                            current, 
                            total));
                    }
                    current++;
                    System.Threading.Thread.Sleep(10);
                }

                if (handler != null)
                {
                    handler.Invoke(null, new GeneralTaskProgressEvent(
                        GeneralTaskProgressEvent.TASK_UPDATE_PAGE_INDEX,
                        GeneralTaskProgressEvent.TASK_STATUS_COMPLETED,
                        "Ready.",
                        current,
                        total));
                }

                return result;
            });
        }
    }
}
