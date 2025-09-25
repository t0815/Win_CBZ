using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Win_CBZ.Data;
using Win_CBZ.Events;
using static Win_CBZ.Handler.AppEventHandler;

namespace Win_CBZ.Tasks
{
    internal class UpdateCheckTask
    {
        public static Task<TaskResult> CheckForUpdates(List<String> urls, GeneralTaskProgressDelegate handler, CancellationToken cancellationToken, bool inBackground = true)
        {
            return new Task<TaskResult>((token) =>
            {
                TaskResult result = new TaskResult();
                int completed = 0;

                foreach (string url in urls)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        result.Status = -1;
                        result.Message = "Operation Cancelled.";

                        return result;
                    }

                    var client = new HttpClient();

                    client.Timeout = TimeSpan.FromSeconds(15); // 15 seconds timeout
                    client.DefaultRequestHeaders.Add("User-Agent", "WIN_CBZ");
                    client.DefaultRequestHeaders.Add("Accept", "application/xml");
                    client.DefaultRequestHeaders.Add("Cache-Control", "no-cache");

                    var request = new HttpRequestMessage(HttpMethod.Get, url);
                   
                    try
                    {
                        using (var response = client.Send(request))
                        {
                            using (var stream = response.Content)
                            {
                                using (var reader = new System.IO.StreamReader(stream.ReadAsStream()))
                                {
                                    string responseText = reader.ReadToEnd();

                                    if (response.IsSuccessStatusCode)
                                    {


                                        result.Payload.TryAdd(url, responseText);

                                        break;
                                    }
                                    else
                                    {
                                        result.Payload.TryAdd(url, null);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        result.Payload.TryAdd(url, null);
                        handler?.Invoke(null, new GeneralTaskProgressEvent
                        {
                            Type = GeneralTaskProgressEvent.TASK_STATUS_RUNNING,
                            Status = GeneralTaskProgressEvent.TASK_STATUS_RUNNING,
                            Message = $"Error checking for update: {ex.Message} ({url})",
                            Current = completed,
                            Total = urls.Count,
                            InBackground = inBackground,
                            PopGlobalState = false
                        });
                    }

                    completed++;

                    handler?.Invoke(null, new GeneralTaskProgressEvent
                    {
                        Type = GeneralTaskProgressEvent.TASK_STATUS_RUNNING,
                        Status = GeneralTaskProgressEvent.TASK_STATUS_RUNNING,
                        Message = $"Checking for Update... ({url})",
                        Current = completed,
                        Total = urls.Count,
                        InBackground = inBackground,
                        PopGlobalState = false
                    });

                    System.Threading.Thread.Sleep(5);
                }
            
                result.Status = 0;

                handler?.Invoke(null, new GeneralTaskProgressEvent()
                {
                    Type = GeneralTaskProgressEvent.TASK_WAITING_FOR_TASKS,
                    Status = GeneralTaskProgressEvent.TASK_STATUS_COMPLETED,
                    Message = "Ready.",
                    Current = 0,
                    Total = 0,
                    InBackground = inBackground,
                    PopGlobalState = false,
                });

                return result;
            }, cancellationToken);
        }
    }
}
