

using System.Collections.Generic;

namespace Win_CBZ.Data
{
    internal class TaskResult
    {
        public int Result { get; set; } = -1;

        public string Message { get; set; }

        public Dictionary<string, object> Payload { get; set; } = new Dictionary<string, object>(); 

        public int Total { get; set; } 

        public int Completed { get; set; }

        public TaskResult() { }

        public TaskResult(int result, string message) 
        {
            Result = result;
            Message = message;
        }

        public bool AddPayload(string key, object v) 
        { 
            return Payload.TryAdd(key, v);
        }

        public bool RemovePayload(string key)
        {
            return true;
        }

        public object GetPayload(string key)
        {
            object v = Payload[key];

            return v == null ? null : v;
        }
    }
}
