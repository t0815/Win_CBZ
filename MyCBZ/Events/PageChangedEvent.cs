using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Win_CBZ
{
    internal class PageChangedEvent
    {

        public const int IMAGE_STATUS_NEW = 0;
        public const int IMAGE_STATUS_COMPRESSED = 1;
        public const int IMAGE_STATUS_DELETED = 2;
        public const int IMAGE_STATUS_CHANGED = 3;
        public const int IMAGE_STATUS_RENAMED = 4;
        public const int IMAGE_STATUS_CLOSED = 5;
        public const int IMAGE_STATUS_ERROR = 6;

        public int State { get; set; }

        public Page Page { get; set; }

        public object OldValue { get; set; }

        public bool NoThumbRefresh { get; set; }


        public PageChangedEvent(Page page, object old, int state = PageChangedEvent.IMAGE_STATUS_NEW)
        {
            State = state; 
            Page = page;
            OldValue = old;
        }

        public PageChangedEvent(Page page, object old, int state = PageChangedEvent.IMAGE_STATUS_NEW, bool noThumbRefresh = false)
        {
            State = state;
            Page = page;
            OldValue = old;
            NoThumbRefresh = noThumbRefresh;
        }
    }
}
