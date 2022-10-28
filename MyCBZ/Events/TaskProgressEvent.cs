using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Win_CBZ
{
    internal class TaskProgressEvent
    {

        public int Current { get; set; }

        public int Total { get; set; }

        public Page Page { get; set; }


        public TaskProgressEvent(Page page, int current, int total)
        {
            Page = page;
            Current = current; 
            Total = total;
        }   
    }
}
