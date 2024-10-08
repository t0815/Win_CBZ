﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Win_CBZ.Data;
using Win_CBZ.Handler;
using static Win_CBZ.Handler.AppEventHandler;
using Win_CBZ.Events;

namespace Win_CBZ.Tasks
{
    [SupportedOSPlatform("windows")]
    internal class ReadImageMetaDataTask
    {

        public static Task<TaskResult> UpdateImageMetadata(List<Page> pages, GeneralTaskProgressDelegate handler, CancellationToken cancellationToken, bool inBackground = false, bool popState = true, List<StackItem> stack = null)
        {
            return new Task<TaskResult>((token) =>
            {
                int current = 1;
                int total = pages.Count;    
                TaskResult result = new TaskResult()
                { 
                    Stack = stack,
                    Total = total
                };

                lock (pages)
                {
                    foreach (Page p in pages)
                    {
                        try
                        {
                            p.LoadImageInfo(true);
                            //
                        }
                        catch
                        {

                        }
                        finally
                        {
                            p.FreeImage();
                        }

                        handler?.Invoke(p, new GeneralTaskProgressEvent(
                            GeneralTaskProgressEvent.TASK_RELOAD_IMAGE_METADATA,
                            GeneralTaskProgressEvent.TASK_STATUS_RUNNING,
                            "Rebuilding image metadata...",
                            current,
                            total,
                            popState,
                            inBackground,
                            GlobalActionRequiredEvent.TASK_TYPE_UPDATE_IMAGE_METADATA
                            ));

                        if (((CancellationToken)token).IsCancellationRequested)
                        {
                            break;
                        }

                        result.Completed = current;
                        current++;

                        System.Threading.Thread.Sleep(5);
                    }
                }

                handler?.Invoke(pages, new GeneralTaskProgressEvent(
                        GeneralTaskProgressEvent.TASK_RELOAD_IMAGE_METADATA,
                        GeneralTaskProgressEvent.TASK_STATUS_COMPLETED,
                        "Ready.",
                        current,
                        total,
                        popState, 
                        inBackground,
                        GlobalActionRequiredEvent.TASK_TYPE_UPDATE_IMAGE_METADATA
                     ));

                return result;
            }, cancellationToken);
        }
    }
}
