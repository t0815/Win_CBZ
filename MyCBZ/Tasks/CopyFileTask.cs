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


namespace Win_CBZ.Tasks
{
    [SupportedOSPlatform("windows")]
    internal class CopyFileTask
    {

        public static Task<TaskResult> CopyFile(LocalFile source, LocalFile destination, EventHandler<FileOperationEvent> handler, CancellationToken cancellationToken, bool inBackground = false, bool popState = false)
        {
            return new Task<TaskResult>((token) =>
            {
                TaskResult result = new TaskResult();
                long elapsed = 0;
                long start = DateTime.Now.Ticks;
                float bytesPerSecond = 0f;

                int bufferSize = 1024 * 4096;
                int bytesRead = -1;
                long byesWritten = 0;

                using (FileStream targetStream = new FileStream(destination.FullPath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite))
                {
                    FileStream sourceStream = new FileStream(source.FullPath, FileMode.Open, FileAccess.Read);
                    result.Total = sourceStream.Length;
                    
                    targetStream.SetLength(sourceStream.Length);
                    
                    byte[] bytes = new byte[bufferSize];

                    while ((bytesRead = sourceStream.Read(bytes, 0, bufferSize)) > 0)
                    {
                        elapsed = DateTime.Now.Ticks;
                        targetStream.Write(bytes, 0, bytesRead);
                        byesWritten += bytesRead;
                        if (elapsed - start > 0)
                        {
                            bytesPerSecond = (float)(byesWritten / (double)((double)(elapsed - start) / 10000000f));
                        }

                        result.Completed = byesWritten;

                        if (((CancellationToken)token).IsCancellationRequested)
                        {
                            result.Status = -1;
                            
                            break;
                        }

                        // no progres tracking here atm, since this will fuckup the overall progressbar
                        handler?.Invoke(null, new FileOperationEvent(
                            FileOperationEvent.OPERATION_COPY,
                            FileOperationEvent.STATUS_RUNNING,
                            "Copying file...",
                            byesWritten,
                            sourceStream.Length,                           
                            bytesPerSecond,
                            inBackground));

                        Thread.Sleep(1);
                    }

                    sourceStream?.Close();
                    sourceStream?.Dispose();
                }
              
                handler?.Invoke(null, new FileOperationEvent(FileOperationEvent.OPERATION_COPY, FileOperationEvent.STATUS_SUCCESS, 0, 100));

                result.Status = 0;

                return result;
            }, cancellationToken);
        }
    }
}
