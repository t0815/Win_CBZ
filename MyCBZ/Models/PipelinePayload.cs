using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Win_CBZ
{
    internal class PipelinePayload
    {
        public const String PAYLOAD_EXECUTE_RENAME_SCRIPT = "RenameAllPages()";
        public const String PAYLOAD_EXECUTE_REBUILD_IMAGE_METADATA_INDEX = "RebuildIndex()";
        public const String PAYLOAD_EXECUTE_IMAGE_PROCESSING = "ProcessImages()";

        public Dictionary<String, Task> Tasks;

        public List<String> SuccessfulTasks;

        public List<String> FailedTasks;


        public PipelinePayload()
        {
            Tasks = new Dictionary<String, Task>();
        }

        public PipelinePayload SetAttribute(String name, Task task)
        {
            Tasks.Add(name, task);

            return this;
        }

        public Task GetAttribute(String key)
        {
            if (Tasks.ContainsKey(key))
            {
                return Tasks[key];
            } else
            {
                return null;
            }
        }

        public void Remove(String key)
        {
            Tasks.Remove(key);
        }
    }
}
