using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using Win_CBZ.Data;
using Win_CBZ.Models;
using Win_CBZ.Result;

namespace Win_CBZ.Tasks
{
    [SupportedOSPlatform("windows")]
    internal class ProcessImagesTask
    {
        public static Task<ImageTaskResult> ProcessImages(List<Page> pages, EventHandler<GeneralTaskProgressEvent> handler)
        {
            return new Task<ImageTaskResult>(() =>
            {
                int current = 1;
                int total = pages.Count;
                List<ImageTask> collectedTasks = new List<ImageTask>();
                ImageTaskResult result = new ImageTaskResult();
                Page taskPage = null;

                foreach (Page page in pages)
                {
                    if (page.ImageTask.TaskCount() > 0)
                    {
                        taskPage = new Page(page, false, true);


                        Stream[] results = taskPage.ImageTask.SetupTasks(taskPage)
                            .Apply()
                            .CleanUp()
                            .ResultStream;

                        if (taskPage.ImageTask.Success)
                        {
                            taskPage.UpdateImage(results[0]);
                            //page.Copy(page.ImageTask.ResultFileName, page.TempPath);
                            result.AddFinishedPage(taskPage);
                        }

                        taskPage.ImageTask.Tasks.Clear();
                        page.ImageTask.Tasks.Clear();

                        handler?.Invoke(page.ImageTask, new GeneralTaskProgressEvent(
                                GeneralTaskProgressEvent.TASK_PROCESS_IMAGE,
                                GeneralTaskProgressEvent.TASK_STATUS_RUNNING,
                                "Processing image...",
                                current,
                                total,
                                true));

                        current++;
                        System.Threading.Thread.Sleep(10);
                    }
                }

                handler?.Invoke(collectedTasks, new GeneralTaskProgressEvent(
                        GeneralTaskProgressEvent.TASK_PROCESS_IMAGE,
                        GeneralTaskProgressEvent.TASK_STATUS_COMPLETED,
                        "Ready.",
                        current,
                        total,
                        true));

                return result;
            });
        }
    }
}
