using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Win_CBZ.Data
{
    public class TaskResult
    {
        int Result;

        string Message;

        public TaskResult() { }

        public TaskResult(int result, string message) 
        {
            Result = result;
            Message = message;
        }


    }
}
