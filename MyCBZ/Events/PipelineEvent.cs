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
        public const int PIPELINE_UPDATE_META_DATA = 3;
        public const int PIPELINE_RUN_RENAMING = 4;
        public const int PIPELINE_PROCESS_IMAGES = 5;
        public const int PIPELINE_SAVE_ARCHIVE = 6;
        public const int PIPELINE_UPDATE_IMAGE_METADATA = 7;

        public ProjectModel ArchiveInfo { get; set; }

        public int Task { get; set; }

        public List<StackItem> Stack { get; set; }

        public PipelineAttributes Attributes { get; set; }

        public object Data { get; set; }

        public delegate void Operation();

        public PipelineEvent.Operation Callback;


        public PipelineEvent(ProjectModel project, int currentTask, object data, List<StackItem> stack)
        {
            ArchiveInfo = project;
            Task = currentTask;
            Data = data;
            Stack = stack;
        }

        public PipelineEvent(ProjectModel project, int currentTask, object data, List<StackItem> stack, PipelineAttributes pipelineAttributes)
        {
            ArchiveInfo = project;
            Task = currentTask;
            Data = data;
            Stack = stack;
            Attributes = pipelineAttributes;
        }

        public PipelineEvent(ProjectModel project, int currentTask, object data, List<StackItem> stack, PipelineAttributes pipelineAttributes, PipelineEvent.Operation callback)
        {
            ArchiveInfo = project;
            Task = currentTask;
            Data = data;
            Stack = stack;
            Attributes = pipelineAttributes;
            Callback = callback;
        }   
    }
}
