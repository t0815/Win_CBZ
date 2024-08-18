using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Win_CBZ.Data;
using Win_CBZ.Handler;
using Win_CBZ.Models;
using Win_CBZ.Result;

namespace Win_CBZ.Tasks
{
    [SupportedOSPlatform("windows")]
    internal class ProcessImagesTask
    {
        public static Task<ImageTaskResult> ProcessImages(List<Page> pages, ImageTask globalTask, String[] skipPages, AppEventHandler.GeneralTaskProgressDelegate handler, CancellationToken? cancellationToken = null)
        {
            return new Task<ImageTaskResult>(() =>
            {
                int current = 1;
                int total = pages.Count;
                List<ImageTask> collectedTasks = new List<ImageTask>();
                ImageTaskResult result = new ImageTaskResult();
                Page taskPage = null;
                Page secondPage = null;

                foreach (Page page in pages)
                {
                    if (Array.IndexOf(skipPages, page.Name) > -1)
                    {
                        continue;
                    }

                    if (cancellationToken.HasValue && cancellationToken.Value.IsCancellationRequested)
                    {
                        break;
                    }

                    if (page.ImageTask.ImageAdjustments.ResizeMode > 0 && 
                        (page.Format.H != page.ImageTask.ImageAdjustments.ResizeTo.Y ||
                         page.Format.W != page.ImageTask.ImageAdjustments.ResizeTo.X)
                        )
                    {
                        page.ImageTask.SetTaskResize();
                    }

                    if (page.ImageTask.ImageAdjustments.SplitPage)
                    {
                        page.ImageTask.SetTaskSplit();
                    }

                    
                    if (page.ImageTask.TaskCount() == 0)
                    {
                        page.ImageTask = new ImageTask(page.Id);
                        //page.ImageTask.ImageFormat = new PageImageFormat(page.ImageTask.ImageFormat);
                        page.ImageTask.ImageAdjustments = globalTask.ImageAdjustments;

                        if (page.ImageTask.ImageAdjustments.ResizeMode > 0 &&
                        (page.Format.H != page.ImageTask.ImageAdjustments.ResizeTo.Y ||
                         page.Format.W != page.ImageTask.ImageAdjustments.ResizeTo.X)
                        )
                        {
                            page.ImageTask.SetTaskResize();
                        }

                        if (page.ImageTask.ImageAdjustments.SplitPage)
                        {
                            page.ImageTask.SetTaskSplit();
                        }
                    }
                    
                    if (page.ImageTask.TaskCount() > 0)
                    {
                        taskPage = new Page(page, false, true);
                        taskPage.Id = page.Id;  // Important! Keep original Id here


                        Stream[] results = taskPage.ImageTask
                            .SetupTasks(taskPage)
                            .Apply()
                            .CleanUp()
                            .GetResultStream();

                        if (taskPage.ImageTask.Success)
                        {
                            taskPage.UpdateImage(results[0]);
                            taskPage.UpdateTemporaryFile(taskPage.ImageTask.ResultFileName[0]);
                            result.AddFinishedPage(taskPage);

                            if (taskPage.ImageTask.ResultFileName[1].Exists())
                            {
                                secondPage = new Page(taskPage.ImageTask.ResultFileName[1], taskPage.WorkingDir);
                                secondPage.Index = taskPage.Index + 1;
                                secondPage.Number = taskPage.Number + 1;
                                secondPage.Compressed = false;

                                result.AddFinishedPage(secondPage);
                            }

                            //page.Copy(page.ImageTask.ResultFileName, page.TempPath);                         
                        } else
                        {

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
            }, cancellationToken.Value);
        }
    }
}
