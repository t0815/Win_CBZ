using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Win_CBZ.Events
{
    internal class RedrawThumbEvent
    {
        public Page Page { get; set; }

        public RedrawThumbEvent() { }

        public RedrawThumbEvent(Page page)
        {
            this.Page = page;
        }
    }
}
