using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Win_CBZ
{
    internal class ArchiveOperationEvent
    {

        public const int STATUS_SUCCESS = 0;
        public const int STATUS_FAILED = -1;

        public const int OPERATION_EXTRACT = 1;
        public const int OPERATION_COMPRESS = 2;

        public int Operation { get; set; }

        public int Status { get; set; }

        public Page Page { get; set; }

        public int Completed { get; set; }

        public int Total { get; set; }


        public ArchiveOperationEvent(int operation, int status, int completed, int total, Page page)
        {
            Operation = operation;
            Status = status;   
            Page = page;
            Completed = completed;
            Total = total;
        }   
    }
}
