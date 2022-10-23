using CBZMage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCBZ
{
    internal class ItemExtractedEvent
    {

        public int Index { get; set; }  

        public int Total { get; set; }

        public String LocalFile { get; set; }


        public ItemExtractedEvent(int index, int total, String file)
        {
            this.Index = index;
            this.Total = total;
            this.LocalFile = file;
        }   
    }
}
