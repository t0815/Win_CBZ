using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Win_CBZ
{
    internal class ItemFailedEvent
    {

        public int Index { get; set; }  

        public String Message { get; set; }  

        public Page Image { get; set; }


        public ItemFailedEvent(int index, String message, Page image)
        {
            Index = index;
            Message = message;
            Image = image;
        }   
    }
}
