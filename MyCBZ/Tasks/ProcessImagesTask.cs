﻿using System;
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
                int current = 1;
                int total = pages.Count;
                List<ImageTask> collectedTasks = new List<ImageTask>();
                ImageTaskResult result = new ImageTaskResult();
                foreach (Page page in pages)
                {
                    if (page.ImageTask.TaskCount() > 0)
                    {
                        collectedTasks.Add(page.ImageTask);
                        //page.ImageTask.SetupTasks()

                        //page.ImageTask.PerformCommands();
                        if (page.ImageTask.Success)
                        {
                            //page.Copy(page.ImageTask.ResultFileName, page.TempPath);
                            result.AddFinishedPage(page);
                        }

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
