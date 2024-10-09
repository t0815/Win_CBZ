using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Win_CBZ.Events
{
    class UpdateThumbnailsEvent
    {
            public List<Page> Pages { get; set; }

            public UpdateThumbnailsEvent() { }

            public UpdateThumbnailsEvent(List<Page> pages)
            {
                this.Pages = pages;
            }
    }
}
