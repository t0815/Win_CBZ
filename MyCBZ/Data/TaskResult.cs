using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Win_CBZ.Data
{
    internal class TaskResult
    {
        int result;

        string message;

        public TaskResult() { }

        public TaskResult(int result, string message) 
        {
            this.result = result;
            this.message = message;
        }


    }
}
