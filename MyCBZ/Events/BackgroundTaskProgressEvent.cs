using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Win_CBZ.Events
{
    internal class BackgroundTaskProgressEvent
    {

        public const int TASK_RELOAD_IMAGE_METADATA = 0;
        public const int TASK_UPDATE_PAGE_INDEX = 1;
        public const int TASK_PROCESS_IMAGE = 2;
        public const int TASK_DELETE_FILE = 3;
        public const int TASK_WAITING_FOR_TASKS = 4;

        public const int TASK_STATUS_IDLE = 10;
        public const int TASK_STATUS_RUNNING = 11;
        public const int TASK_STATUS_COMPLETED = 12;


        public int Type { get; set; } 

        public int Status { get; set; }

        public int Current { get; set; }

        public int Total { get; set; }

        public string Message { get; set; }

        public Task Task { get; set; }


        public BackgroundTaskProgressEvent() { }
    }
}
