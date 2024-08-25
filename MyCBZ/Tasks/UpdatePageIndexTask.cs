using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Win_CBZ.Data;
using Win_CBZ.Events;
using Win_CBZ.Helper;


namespace Win_CBZ.Tasks
{
    [SupportedOSPlatform("windows")]
    internal class UpdatePageIndexTask
    {

        public static Task<TaskResult> UpdatePageIndex(List<Page> pages, EventHandler<GeneralTaskProgressEvent> handler, EventHandler<PageChangedEvent> pageChangedHandler, CancellationToken cancellationToken, bool inBackground = false, bool popState = false)
        {
            return new Task<TaskResult>((token) =>
            {
                bool isUpdated = false;
                int newIndex = 0;
                int current = 1;
                int total = pages.Count;
                TaskResult result = new TaskResult();

                result.Total = total;

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

                    if (((CancellationToken)token).IsCancellationRequested)
                    {
                        
                        break;
                    }
                   
                    handler?.Invoke(null, new GeneralTaskProgressEvent(
                        GeneralTaskProgressEvent.TASK_UPDATE_PAGE_INDEX, 
                        GeneralTaskProgressEvent.TASK_STATUS_RUNNING, 
                        "Rebuilding index...",
                        current, 
                        total,
                        popState,
                        inBackground));
                    
                    current++;
                    isUpdated = false;
                    System.Threading.Thread.Sleep(5);
                }

                result.Completed = current;
               
                handler?.Invoke(null, new GeneralTaskProgressEvent(
                    GeneralTaskProgressEvent.TASK_UPDATE_PAGE_INDEX,
                    GeneralTaskProgressEvent.TASK_STATUS_COMPLETED,
                    "Ready.",
                    current,
                    total,
                    popState,
                    inBackground));
                

                return result;
            }, cancellationToken);
        }
    }
}
