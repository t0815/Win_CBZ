using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Win_CBZ.Models;

namespace Win_CBZ.Events
{
    internal class ImageAdjustmentsChangedEvent
    {

        public string PageId { get; set; }

        public ImageAdjustments ImageAdjustments { get; set; }

        public ImageAdjustmentsChangedEvent() { }

        public ImageAdjustmentsChangedEvent(ImageAdjustments imageAdjustments, String pageId = "")
        {
            ImageAdjustments = imageAdjustments;
            PageId = pageId;
        }
    }
}
