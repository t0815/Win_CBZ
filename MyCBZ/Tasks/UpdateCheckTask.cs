using SharpCompress.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Win_CBZ.Data;
using Win_CBZ.Events;
using Win_CBZ.Result;
using static Win_CBZ.Handler.AppEventHandler;

namespace Win_CBZ.Tasks
{
    internal class UpdateCheckTask
    {
        public static Task<UpdateCheckTaskResult> CheckForUpdates(List<String> urls, GeneralTaskProgressDelegate handler, CancellationToken cancellationToken, bool inBackground = true)
        {
            return new Task<UpdateCheckTaskResult>((token) =>
            {
                UpdateCheckTaskResult result = new UpdateCheckTaskResult();
                int completed = 0;

                foreach (string url in urls)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        result.Status = -1;
                        result.Message = "Operation Cancelled.";
                        result.LatestVersion = null;

                        return result;
                    }

                    handler?.Invoke(null, new GeneralTaskProgressEvent
                    {
                        Type = GeneralTaskProgressEvent.TASK_UPDATE_CHECK,
                        Status = GeneralTaskProgressEvent.TASK_STATUS_RUNNING,
                        Message = $"Checking for Update... ({url})",
                        Current = completed,
                        Total = urls.Count,
                        InBackground = inBackground,
                        PopGlobalState = false
                    });

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

                                        result.LatestVersion = latestVersion.ToString();
                                        result.DownloadUrl = downloadUrl.ToString();
                                        result.Message = message.ToString();

                                        break;
                                    }
                                    else
                                    {
                                        result.LatestVersion = null;
                                    }

                                    result.Status = ((int)response.StatusCode);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        result.LatestVersion = null;
                        result.Status = 500;

                        handler?.Invoke(null, new GeneralTaskProgressEvent
                        {
                            Type = GeneralTaskProgressEvent.TASK_UPDATE_CHECK,
                            Status = GeneralTaskProgressEvent.TASK_STATUS_RUNNING,
                            Message = $"Error checking for update: {ex.Message} ({url})",
                            Current = completed,
                            Total = urls.Count,
                            InBackground = inBackground,
                            PopGlobalState = false
                        });
                    }

                    completed++;

                    System.Threading.Thread.Sleep(5);
                }

                handler?.Invoke(null, new GeneralTaskProgressEvent
                {
                    Type = GeneralTaskProgressEvent.TASK_UPDATE_CHECK,
                    Status = GeneralTaskProgressEvent.TASK_STATUS_COMPLETED,
                    Message = $"",
                    Current = completed,
                    Total = urls.Count,
                    InBackground = inBackground,
                    PopGlobalState = false
                });

                return result;
            }, cancellationToken);
        }
    }
}
