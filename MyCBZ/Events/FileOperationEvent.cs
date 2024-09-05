using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Win_CBZ.Events
{
    internal class FileOperationEvent
    {

        public const int STATUS_SUCCESS = 0;
        public const int STATUS_RUNNING = 1;
        public const int STATUS_FAILED = -1;

        public const int OPERATION_DELETE = 1;
        public const int OPERATION_COPY = 2;
        

        public int Operation { get; set; }

        public int Status { get; set; }

        public string Message { get; set; }

        public long Completed { get; set; }

        public bool InBackground { get; set; }

        public long Total { get; set; }

        public float BytesPerSecond { get; set; }
        

        public FileOperationEvent(int operation, int status, long completed, long total)
        {
            Operation = operation;
            Status = status;   
            Completed = completed;
            Total = total;
        }

        public FileOperationEvent(int operation, int status, long completed, long total, string message, bool inBackground = false)
        {
            Operation = operation;
            Status = status;
            Completed = completed;
            Total = total;
            Message = message;
            InBackground = inBackground;
        }

        public FileOperationEvent(int operation, int status, string message, bool inBackground = false)
        {
            Operation = operation;
            Status = status;
            Message = message;
            InBackground = inBackground;
        }

        public FileOperationEvent(int operation, int status, string message, long completed, long total, float bytesPerSecond, bool inBackground = false)
        {
            Operation = operation;
            Status = status;
            Message = message;
            Completed = completed;
            Total = total;
            BytesPerSecond = bytesPerSecond;
            InBackground = inBackground;
        }
    }
}
