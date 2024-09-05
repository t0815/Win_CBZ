using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Win_CBZ.Events
{
    internal class GeneralTaskProgressEvent
    {

        public const int TASK_RELOAD_IMAGE_METADATA = 0;
        public const int TASK_UPDATE_PAGE_INDEX = 1;
        public const int TASK_PROCESS_IMAGE = 2;
        public const int TASK_DELETE_FILE = 3;
        public const int TASK_WAITING_FOR_TASKS = 4;
        public const int TASK_VALIDATING_COMICINFO = 5;
        public const int TASK_COPYING_FILE = 6;

        public const int TASK_STATUS_IDLE = 10;
        public const int TASK_STATUS_RUNNING = 11;
        public const int TASK_STATUS_COMPLETED = 12;

        public int Type { get; set; }

        public int Status { get; set; }

        public int Current { get; set; }

        public int Total { get; set; }

        public string Message { get; set; }

        public bool PopGlobalState { get; set; }

        public bool InBackground { get; set; }

        public GeneralTaskProgressEvent() { }

        public GeneralTaskProgressEvent(int type, int status, string message, int current, int total, bool popState, bool inBackground = false)
        {
            Current = current; 
            Total = total;
            Type = type;
            Status = status;
            Message = message;
            PopGlobalState = popState;  
            InBackground = inBackground;
        }   
    }
}
