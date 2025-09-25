using SharpCompress.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
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
                                        System.Xml.Linq.XDocument doc = System.Xml.Linq.XDocument.Parse(responseText);
                                        var latestVersion = doc.Descendants("latest").FirstOrDefault()?.Value;
                                        var downloadUrl = doc.Descendants("url").FirstOrDefault()?.Value;
                                        var changes = doc.Descendants("changes").FirstOrDefault()?.Value;

                                        StringBuilder message = new StringBuilder();
                                        if (latestVersion != null && downloadUrl != null)
                                        {
                                            Version currentVersion = Assembly.GetExecutingAssembly().GetName().Version;
                                            Version newVersion = new Version(latestVersion);

                                            
                                            if (newVersion > currentVersion)
                                            {
                                                
                                                message.AppendLine($"A new version of Win-CBZ is available!\r\n");
                                                message.AppendLine($"Current version: {currentVersion}");
                                                message.AppendLine($"Latest version: {newVersion}\r\n");
                                                if (changes != null)
                                                {
                                                    message.AppendLine("Changes:\r\n");
                                                    message.AppendLine(changes + "\r\n");
                                                }
                                                
                                            }
                                            else
                                            {
                                                message.Append("You are using the latest version of Win-CBZ.");
                                            }
                                        }

                                        result.Payload.TryAdd(url, message);

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
