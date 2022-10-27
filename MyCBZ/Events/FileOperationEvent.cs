using CBZMage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCBZ
{
    internal class FileOperationEvent
    {

        public const int STATUS_SUCCESS = 0;
        public const int STATUS_FAILED = -1;

        public const int OPERATION_DELETE = 1;
        public const int OPERATION_COPY = 2;
        

        public int Operation { get; set; }

        public int Status { get; set; }

        public Page Image { get; set; }

        public long Completed { get; set; }

        public long Total { get; set; }


        public FileOperationEvent(int operation, int status, long completed, long total, Page image)
        {
            this.Operation = operation;
            this.Status = status;   
            this.Image = image;
            this.Completed = completed;
            this.Total = total;
        }   
    }
}
