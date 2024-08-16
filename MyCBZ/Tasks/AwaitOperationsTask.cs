using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Win_CBZ.Data;
using Win_CBZ.Helper;

namespace Win_CBZ.Tasks
{
    internal class AwaitOperationsTask
    {
        public static Task<TaskResult> AwaitOperationsAndRun(List<Thread> awaitTasks, Thread runTask, object taskParams, EventHandler<GeneralTaskProgressEvent> handler)
        {
            return new Task<TaskResult>(() =>
            {
                TaskResult result = new TaskResult();
                int finishedCount = 0;

                foreach (Thread th in awaitTasks)
                {
                    

                    if (handler != null)
                    {
                        handler.Invoke(null, new GeneralTaskProgressEvent(
                            GeneralTaskProgressEvent.TASK_UPDATE_PAGE_INDEX,
                            GeneralTaskProgressEvent.TASK_STATUS_RUNNING,
                            "Waiting for operations to finish...",
                            finishedCount,
                            awaitTasks.Count,
                            true));
                    }
                 
                    
                    System.Threading.Thread.Sleep(5);
                }

                

                if (handler != null)
                {
                    handler.Invoke(null, new GeneralTaskProgressEvent(
                        GeneralTaskProgressEvent.TASK_UPDATE_PAGE_INDEX,
                        GeneralTaskProgressEvent.TASK_STATUS_COMPLETED,
                        "Ready.",
                        0,
                        0,
                        true));
                }

                return result;
            });
        }
    }
}
