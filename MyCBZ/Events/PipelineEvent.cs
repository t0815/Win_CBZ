using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Win_CBZ.Data;

namespace Win_CBZ
{
    internal class PipelineEvent
    {
        public const int PIPELINE_PARSE_FILES = 0;
        public const int PIPELINE_MAKE_PAGES = 1;
        public const int PIPELINE_UPDATE_INDICES = 2;
        public const int PIPELINE_RUN_RENAMING = 3;
        public const int PIPELINE_PROCESS_IMAGES = 4;
        public const int PIPELINE_SAVE_ARCHIVE = 5;
        public const int PIPELINE_UPDATE_IMAGE_METADATA = 6;

        public ProjectModel ArchiveInfo { get; set; }

        public int Task { get; set; }

        public List<StackItem> Stack { get; set; }

        public PipelinePayload Payload { get; set; }

        public object Data { get; set; }

        public delegate void Operation();

        public PipelineEvent.Operation Callback;


        public PipelineEvent(ProjectModel project, int currentTask, object data, List<StackItem> stack)
        {
            ArchiveInfo = project;
            Task = currentTask;
            Data = Data;
            Stack = stack;
        }

        public PipelineEvent(ProjectModel project, int currentTask, object data, List<StackItem> stack, PipelinePayload pipelineConfig)
        {
            ArchiveInfo = project;
            Task = currentTask;
            Data = data;
            Stack = stack;
            Payload = pipelineConfig;
        }

        public PipelineEvent(ProjectModel project, int currentTask, object data, List<StackItem> stack, PipelinePayload pipelineConfig, PipelineEvent.Operation callback)
        {
            ArchiveInfo = project;
            Task = currentTask;
            Data = data;
            Stack = stack;
            Payload = pipelineConfig;
            Callback = callback;
        }   
    }
}
