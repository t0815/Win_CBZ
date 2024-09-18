using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Win_CBZ.Data;
using Win_CBZ.Events;
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
                string convertFormatName = null;

                AppEventHandler.OnApplicationStateChanged(null, new ApplicationStatusEvent(Program.ProjectModel, ApplicationStatusEvent.STATE_PROCESSING));

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

                    if (page.ImageTask.ImageAdjustments.ConvertType > 0 &&
                        page.Format.Format != page.ImageTask.ImageAdjustments?.ConvertFormat?.Format
                    )
                    {
                        page.ImageTask.SetTaskConvert();
                    }

                    if (page.ImageTask.ImageAdjustments.ResizeMode > 0 &&
                        (page.Format.H != page.ImageTask.ImageAdjustments.ResizeTo.Y ||
                         page.Format.W != page.ImageTask.ImageAdjustments.ResizeTo.X) ||
                        (page.ImageTask.ImageAdjustments.ResizeMode == 3 && page.ImageTask.ImageAdjustments.ResizeToPercentage > 0)
                        )
                    {
                        page.ImageTask.SetTaskResize();
                    }

                    if (page.ImageTask.ImageAdjustments.RotateMode > 0)
                    {
                        page.ImageTask.SetTaskRotate();
                    }

                    if (page.ImageTask.ImageAdjustments.SplitPage)
                    {
                        page.ImageTask.SetTaskSplit();
                    }


                    if (page.ImageTask.TaskCount() == 0)
                    {
                        //page.ImageTask = new ImageTask(page.Id);
                        //page.ImageTask.ImageFormat = new PageImageFormat(page.ImageTask.ImageFormat);
                        page.ImageTask.ImageAdjustments = new ImageAdjustments(globalTask.ImageAdjustments);

                        if (page.ImageTask.ImageAdjustments.ConvertType > 0 &&
                            page.Format.Format != page.ImageTask.ImageAdjustments.ConvertFormat?.Format
                        )
                        {
                            page.ImageTask.SetTaskConvert();
                        }

                        if (page.ImageTask.ImageAdjustments.ResizeMode > 0 &&
                        (page.Format.H != page.ImageTask.ImageAdjustments.ResizeTo.Y ||
                         page.Format.W != page.ImageTask.ImageAdjustments.ResizeTo.X) ||
                        (page.ImageTask.ImageAdjustments.ResizeMode == 3 && page.ImageTask.ImageAdjustments.ResizeToPercentage > 0)
                        )
                        {
                            page.ImageTask.SetTaskResize();
                        }

                        if (page.ImageTask.ImageAdjustments.RotateMode > 0)
                        {
                            page.ImageTask.SetTaskRotate();
                        }

                        if (page.ImageTask.ImageAdjustments.SplitPage)
                        {
                            page.ImageTask.SetTaskSplit();
                        }
                    }

                    if (page.ImageTask.TaskCount() > 0)
                    {
                        taskPage = new Page(page, false, true);
                        if (!taskPage.ImageInfoRequested)
                        {
                            taskPage.LoadImageInfo();
                        }
                        taskPage.Id = page.Id;  // Important! Keep original Id here
                        taskPage.ImageTask.PageId = page.Id;
                        taskPage.ImageTask.ImageAdjustments.ConvertFormat = new PageImageFormat(page.Format);
                        taskPage.ImageTask.ImageAdjustments.ConvertFormat.FormatFromString(IndexToDataMappings.GetInstance().GetImageFormatNameFromIndex(page.ImageTask.ImageAdjustments.ConvertType));

                        Page[] results = taskPage.ImageTask
                            .SetupTasks(ref taskPage)
                            .Apply()
                            .CleanUp()
                            .GetResultPage();

                        if (taskPage.ImageTask.Success)
                        {
                            try
                            {
                                //taskPage.UpdateImage(results[0]);
                                //taskPage.UpdateTemporaryFile(taskPage.ImageTask.ResultFileName[0]);
                                results[0].LoadImageInfo(true);
                                results[0].FreeImage();
                                result.AddFinishedPage(results[0]);

                                if (results[1] != null && results[1].LocalFile.Exists())
                                {
                                    //secondPage = new Page(taskPage.ImageTask.ResultFileName[1], taskPage.WorkingDir);
                                    results[1].Index = results[0].Index + 1;
                                    results[1].Number = results[0].Number + 1;
                                    results[1].Compressed = false;
                                    results[1].LoadImageInfo(true);
                                    results[1].FreeImage();

                                    result.AddFinishedPage(results[1]);
                                }
                            }
                            catch (PageException pe)
                            {
                                MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_WARNING, pe.Message);
                            }

                            //page.Copy(page.ImageTask.ResultFileName, page.TempPath);                         
                        }
                        else
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
