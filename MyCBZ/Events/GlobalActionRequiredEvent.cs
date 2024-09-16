using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Win_CBZ.Data;

namespace Win_CBZ.Events
{
    internal class GlobalActionRequiredEvent
    {
        public const int MESSAGE_TYPE_INFO = 0;
        public const int MESSAGE_TYPE_WARNING = 1;
        public const int MESSAGE_TYPE_ERROR = 2;

        public const string TASK_TYPE_INDEX_REBUILD = "REBUILD_INDEX";
        public const string TASK_TYPE_UPDATE_IMAGE_METADATA = "UPDATE_IMAGE_METADATA";

        public ProjectModel ArchiveInfo { get; set; }

        public int Type { get; set; }

        public string TaskType { get; set; }

        public Task<TaskResult> Task { get; set; }

        public String Message { get; set; }

        public String ButtonText { get; set; }

        public string Id { get; set; }

        public GlobalActionRequiredEvent() { }


        public GlobalActionRequiredEvent(ProjectModel project, int type, String message, String buttonText)
        {
            ArchiveInfo = project;
            Type = type;
            Message = message;
            ButtonText = buttonText;
        }

        public GlobalActionRequiredEvent(ProjectModel project, int type, String message, String buttonText, String taskType, Task<TaskResult> task, String id = null)
        {
            ArchiveInfo = project;
            Type = type;
            Message = message;
            ButtonText = buttonText;
            TaskType = taskType;
            Task = task;
            Id = id;
        }   
    }
}
