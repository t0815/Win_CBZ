﻿using System;
using System.Collections.Generic;
using System.Drawing;
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
        public static Task<ImageTaskResult> ProcessImages(List<Page> pages, ImageTask globalTask, String[] skipPages, AppEventHandler.GeneralTaskProgressDelegate handler, CancellationToken? cancellationToken = null, List<StackItem> stack = null)
        {
            return new Task<ImageTaskResult>(() =>
            {
                int current = 1;
                int advanceIndexBy = 0;
                int total = pages.Count;
                List<ImageTask> collectedTasks = new List<ImageTask>();
                ImageTaskResult result = new ImageTaskResult()
                {
                    Stack = stack
                };

                Page taskPage = null;

                AppEventHandler.OnApplicationStateChanged(null, new ApplicationStatusEvent(Program.ProjectModel, ApplicationStatusEvent.STATE_PROCESSING));

                foreach (Page page in pages)
                {
                    if (Array.IndexOf(skipPages, page.Name) > -1)
                    {
                        result.AddFinishedPage(page);
                        continue;
                    }

                    if (page.Deleted)
                    {
                        result.AddFinishedPage(page);
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
                        (page.ImageTask.ImageAdjustments.ResizeMode == 3 && page.ImageTask.ImageAdjustments.ResizeToPercentage > 0) ||
                        (page.ImageTask.ImageAdjustments.ResizeMode == 1 && page.ImageTask.ImageAdjustments.ResizeToPageNumber > 0)
                        )
                    {
                        if (page.ImageTask.ImageAdjustments.ResizeToPageNumber > 0)
                        {
                            page.ImageTask.ImageAdjustments.PageToResizeTo = pages.Find(p => p.Number == page.ImageTask.ImageAdjustments.ResizeToPageNumber);
                            if (page.ImageTask.ImageAdjustments.PageToResizeTo != null &&
                                page.ImageTask.ImageAdjustments.PageToResizeTo.Format != null &&
                                (page.ImageTask.ImageAdjustments.PageToResizeTo.Format.W < page.Format.W ||
                                page.ImageTask.ImageAdjustments.PageToResizeTo.Format.H < page.Format.H)
                            )
                            {
                                page.ImageTask.ImageAdjustments.ResizeTo = new Point(page.ImageTask.ImageAdjustments.PageToResizeTo.Format.W, page.ImageTask.ImageAdjustments.PageToResizeTo.Format.H);
                                page.ImageTask.SetTaskResize();
                            }
                        }
                        else
                        {
                            page.ImageTask.SetTaskResize();
                        }
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
                        (page.ImageTask.ImageAdjustments.ResizeMode == 3 && page.ImageTask.ImageAdjustments.ResizeToPercentage > 0) ||
                        (page.ImageTask.ImageAdjustments.ResizeMode == 1 && page.ImageTask.ImageAdjustments.ResizeToPageNumber > 0)
                        )
                        {
                            
                            if (page.ImageTask.ImageAdjustments.ResizeToPageNumber > 0)
                            {
                                page.ImageTask.ImageAdjustments.PageToResizeTo = pages.Find(p => p.Number == page.ImageTask.ImageAdjustments.ResizeToPageNumber);
                                if (page.ImageTask.ImageAdjustments.PageToResizeTo != null &&
                                    page.ImageTask.ImageAdjustments.PageToResizeTo.Format != null &&
                                    (page.ImageTask.ImageAdjustments.PageToResizeTo.Format.W < page.Format.W ||
                                    page.ImageTask.ImageAdjustments.PageToResizeTo.Format.H < page.Format.H)
                                )
                                {
                                    page.ImageTask.ImageAdjustments.ResizeTo = new Point(page.ImageTask.ImageAdjustments.PageToResizeTo.Format.W, page.ImageTask.ImageAdjustments.PageToResizeTo.Format.H);
                                    page.ImageTask.SetTaskResize();
                                }

                            }
                            else
                            {
                                page.ImageTask.SetTaskResize();
                            }
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
                            taskPage.FreeImage();
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
                          
                                results[0].LoadImageInfo(true);
                                results[0].FreeStreams();
                                results[0].FreeImage();
                                results[0].Index = results[0].Index + advanceIndexBy;
                                results[0].Number = results[0].Index + 1;
                                result.AddFinishedPage(results[0]);

                                if (results[1] != null && results[1].LocalFile.Exists())
                                {
                                    
                                    results[1].Index = results[0].Index + 1;
                                    results[1].Number = results[0].Number + 1;
                                    results[1].Compressed = false;
                                    results[1].LoadImageInfo(true);
                                    results[1].FreeStreams();

                                    result.AddFinishedPage(results[1]);

                                    advanceIndexBy++;
                                }
                            }
                            catch (PageException pe)
                            {
                                MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_WARNING, pe.Message);
                            }

                                                   
                        }
                        else
                        {
                            result.AddFinishedPage(page);
                        }

                        taskPage.ImageTask.Tasks.Clear();
                    } else
                    {
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
