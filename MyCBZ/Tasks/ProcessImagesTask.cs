using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Win_CBZ.Data;
using Win_CBZ.Models;

namespace Win_CBZ.Tasks
{
    internal class ProcessImagesTask
    {

        public static Task<TaskResult> UpdateImageMetadata(List<ImageTask> tasks, EventHandler<GeneralTaskProgressEvent> handler)
        {
            return new Task<TaskResult>(() =>
            {
                int current = 1;
                int total = tasks.Count;    
                TaskResult result = new TaskResult();

                foreach (ImageTask task in tasks)
                {                   
                    task.PerformCommands();
                    if (task.Success)
                    {
                        //page.Copy(page.ImageTask.ResultFileName, page.TempPath);
                    }
                    task.CommandsTodo.Clear();

                    if (handler != null)
                    {
                        handler.Invoke(null, new GeneralTaskProgressEvent(
                            GeneralTaskProgressEvent.TASK_PROCESS_IMAGE, 
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
                        GeneralTaskProgressEvent.TASK_PROCESS_IMAGE,
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
