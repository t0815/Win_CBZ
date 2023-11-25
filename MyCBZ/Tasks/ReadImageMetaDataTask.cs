using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Win_CBZ.Data;

namespace Win_CBZ.Tasks
{
    internal class ReadImageMetaDataTask
    {

        public static Task<TaskResult> UpdateImageMetadata(List<Page> pages, EventHandler<GeneralTaskProgressEvent> handler)
        {
            return new Task<TaskResult>(() =>
            {
                int current = 1;
                int total = pages.Count;    
                TaskResult result = new TaskResult();

                foreach (Page p in pages)
                {
                    p.LoadImageInfo();
                    handler?.Invoke(p, new GeneralTaskProgressEvent(
                            GeneralTaskProgressEvent.TASK_RELOAD_IMAGE_METADATA, 
                            GeneralTaskProgressEvent.TASK_STATUS_RUNNING, 
                            "Rebuilding image metadata...",
                            current, 
                            total));
                    current++;
                    System.Threading.Thread.Sleep(10);
                }

                handler?.Invoke(pages, new GeneralTaskProgressEvent(
                        GeneralTaskProgressEvent.TASK_RELOAD_IMAGE_METADATA,
                        GeneralTaskProgressEvent.TASK_STATUS_COMPLETED,
                        "Ready.",
                        current,
                        total));

                return result;
            });
        }
    }
}
