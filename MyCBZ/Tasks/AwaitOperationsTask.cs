using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Win_CBZ.Data;
using Win_CBZ.Events;
using Win_CBZ.Forms;
using Win_CBZ.Helper;
using static Win_CBZ.Handler.AppEventHandler;

namespace Win_CBZ.Tasks
{
    internal class AwaitOperationsTask
    {
        public static Task<TaskResult> AwaitOperations(List<Thread> awaitTasks, GeneralTaskProgressDelegate handler, CancellationToken cancellationToken, bool inBackground = false)
        {
            return new Task<TaskResult>((token) =>
            {
                TaskResult result = new TaskResult();
                ApplicationDialog dlg = null;

                Dictionary<int, bool> finishedThreads = new Dictionary<int, bool>();

                //if (showDialog)
               // {
                    //invoke(new Action(() =>
                    //{

                    //}));
                 //   dlg = ApplicationMessage.Create("Waiting for operations to finish. Please wait.", "Please stand by");
                   // dlg.TopMost = true;
                   // dlg.Show();
                //}

                while (finishedThreads.Count < awaitTasks.Count)
                {
                    foreach (Thread th in awaitTasks)
                    {
                        if (!th.IsAlive)
                        {
                            finishedThreads.Add(th.ManagedThreadId, true);
                        }

                        handler?.Invoke(null, new GeneralTaskProgressEvent
                        {
                            Type = GeneralTaskProgressEvent.TASK_WAITING_FOR_TASKS,
                            Status = GeneralTaskProgressEvent.TASK_STATUS_RUNNING,
                            Message = "Waiting for operations to finish...",
                            Current = finishedThreads.Count,
                            Total = awaitTasks.Count,
                            InBackground = inBackground,
                            PopGlobalState = true
                        });
                        
                        System.Threading.Thread.Sleep(5);
                    }
                }

                //if (showDialog)
               // {
               //     dlg.Close();
               // }

                result.Result = 0;

                handler?.Invoke(null, new GeneralTaskProgressEvent()
                {
                    Type = GeneralTaskProgressEvent.TASK_WAITING_FOR_TASKS,
                    Status = GeneralTaskProgressEvent.TASK_STATUS_COMPLETED,
                    Message = "Ready.",
                    Current = 0,
                    Total = 0,
                    InBackground = inBackground,
                });
                
                return result;
            }, cancellationToken);
        }
    }
}
