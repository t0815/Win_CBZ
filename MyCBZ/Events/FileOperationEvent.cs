using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Win_CBZ
{
    public class FileOperationEvent
    {

        public const int STATUS_SUCCESS = 0;
        public const int STATUS_RUNNING = 1;
        public const int STATUS_FAILED = -1;

        public const int OPERATION_DELETE = 1;
        public const int OPERATION_COPY = 2;
        

        public int Operation { get; set; }

        public int Status { get; set; }

        public long Completed { get; set; }

        public long Total { get; set; }
        

        public FileOperationEvent(int operation, int status, long completed, long total)
        {
            Operation = operation;
            Status = status;   
            Completed = completed;
            Total = total;
        }   
    }
}
