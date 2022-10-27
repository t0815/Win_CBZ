using CBZMage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCBZ
{
    internal class ItemFailedEvent
    {

        public int index { get; set; }  

        public String message { get; set; }  

        public Page image { get; set; }


        public ItemFailedEvent(int index, String message, Page image)
        {
            this.index = index;
            this.message = message;
            this.image = image;
        }   
    }
}
