using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Win_CBZ.Models;

namespace Win_CBZ.Events
{
    internal class ImageAdjustmentsChangedEvent
    {

        public ImageAdjustments Adjustments { get; set; }

        public ListViewItem ListViewItem { get; set; }

        public Page Page { get; set; }

        public ImageAdjustmentsChangedEvent() { }

        public ImageAdjustmentsChangedEvent(ImageAdjustments imageAdjustments, Page page)
        {
            Page = page;
            Adjustments = imageAdjustments;
        }
    }
}
