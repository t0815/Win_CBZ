

namespace Win_CBZ.Data
{
    internal class TaskResult
    {
        public int Result { get; set; } = -1;

        public string Message { get; set; }

        public object[] Payload { get; set; }

        public int Total { get; set; } 

        public int Completed { get; set; }

        public TaskResult() { }

        public TaskResult(int result, string message) 
        {
            Result = result;
            Message = message;
        }

        public TaskResult(int result, string message, object[] payload)
        {
            Result = result;
            Message = message;
            Payload = payload;
        }
    }
}
