using CBZMage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCBZ
{
    internal class PageChangedEvent
    {

        public const int IMAGE_STATUS_NEW = 0;
        public const int IMAGE_STATUS_COMPRESSED = 1;
        public const int IMAGE_STATUS_DELETED = 2;
        public const int IMAGE_STATUS_CHANGED = 3;
        public const int IMAGE_STATUS_RENAMED = 4;
        public const int IMAGE_STATUS_CLOSED = 5;

        public int State { get; set; }

        public int Total { get; set; }

        public Page Image { get; set; }


        public PageChangedEvent(Page image, int state = PageChangedEvent.IMAGE_STATUS_NEW, int total = 1)
        {
            this.State = state;
            this.Total = total; 
            this.Image = image;
        }   
    }
}
