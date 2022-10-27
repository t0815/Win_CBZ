using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Win_CBZ
{
    internal class OperationFinishedEvent
    {

        public int Index { get; set; }

        public int Total { get; set; }


        public OperationFinishedEvent(int index, int total)
        {
            this.Index = index;
            this.Total = total; 
        }   
    }
}
