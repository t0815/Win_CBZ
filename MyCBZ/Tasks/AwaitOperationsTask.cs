using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Win_CBZ.Data;
using Win_CBZ.Forms;
using Win_CBZ.Helper;

namespace Win_CBZ.Tasks
{
    internal class AwaitOperationsTask
    {
        public static Task<TaskResult> AwaitOperations(List<Thread> awaitTasks, EventHandler<GeneralTaskProgressEvent> handler, CancellationToken cancellationToken)
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

                        if (handler != null)
                        {
                            handler.Invoke(null, new GeneralTaskProgressEvent(
                                GeneralTaskProgressEvent.TASK_WAITING_FOR_TASKS,
                                GeneralTaskProgressEvent.TASK_STATUS_RUNNING,
                                "Waiting for operations to finish...",
                                finishedThreads.Count,
                                awaitTasks.Count,
                                true));
                        }

                        System.Threading.Thread.Sleep(5);
                    }
                }

                //if (showDialog)
               // {
               //     dlg.Close();
               // }

                result.Result = 0;

                if (handler != null)
                {
                    handler.Invoke(null, new GeneralTaskProgressEvent(
                        GeneralTaskProgressEvent.TASK_WAITING_FOR_TASKS,
                        GeneralTaskProgressEvent.TASK_STATUS_COMPLETED,
                        "Ready.",
                        0,
                        0,
                        true));
                }

                return result;
            }, cancellationToken);
        }
    }
}
