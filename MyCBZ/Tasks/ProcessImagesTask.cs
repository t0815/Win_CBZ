using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Win_CBZ.Data;
using Win_CBZ.Models;
using Win_CBZ.Result;

namespace Win_CBZ.Tasks
{
    internal class ProcessImagesTask
    {
        public static Task<ImageTaskResult> ProcessImages(List<Page> pages, EventHandler<GeneralTaskProgressEvent> handler)
        {
            return new Task<ImageTaskResult>(() =>
            {
                List<ImageTask> collectedTasks = new List<ImageTask>();
                foreach (Page page in pages)
                {
                    if (page.ImageTask.TaskCount() > 0)
                    {
                        collectedTasks.Add(page.ImageTask);
                    }
                }

                int current = 1;
                int total = collectedTasks.Count;    
                ImageTaskResult result = new ImageTaskResult();

                foreach (ImageTask task in collectedTasks)
                {             
                    

                    //task.PerformCommands();
                    if (task.Success)
                    {
                        //page.Copy(page.ImageTask.ResultFileName, page.TempPath);
                    }
                    task.CommandsTodo.Clear();

                    if (handler != null)
                    {
                        handler.Invoke(task, new GeneralTaskProgressEvent(
                            GeneralTaskProgressEvent.TASK_PROCESS_IMAGE, 
                            GeneralTaskProgressEvent.TASK_STATUS_RUNNING, 
                            "Processing image...",
                            current, 
                            total));
                    }
                    current++;
                    System.Threading.Thread.Sleep(10);
                }

                if (handler != null)
                {
                    handler.Invoke(collectedTasks, new GeneralTaskProgressEvent(
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
