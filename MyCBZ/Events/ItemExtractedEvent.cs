using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Win_CBZ.Events
{
    internal class ItemExtractedEvent
    {

        public int Index { get; set; }  

        public int Total { get; set; }

        public String LocalFile { get; set; }


        public ItemExtractedEvent(int index, int total, String file)
        {
            Index = index;
            Total = total;
            LocalFile = file;
        }   
    }
}
