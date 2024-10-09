using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Win_CBZ.Events
{
    internal class UpdatePageListViewSortingEvent
    {
        public int SortColumn { get; set; } = 1;   // sort by index by default

        public SortOrder Order { get; set; } = SortOrder.Ascending;

        public UpdatePageListViewSortingEvent() { }

        public UpdatePageListViewSortingEvent(int column, SortOrder order)
        {
            SortColumn = column;
            Order = order;
        }
    }
}
