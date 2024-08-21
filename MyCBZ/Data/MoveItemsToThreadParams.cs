using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Win_CBZ.Data
{
    internal class MoveItemsToThreadParams : ThreadParam
    {

        public int NewIndex { get; set; }

        public System.Windows.Forms.ListView.SelectedListViewItemCollection Items { get; set; }

        public MetaData.PageIndexVersion PageIndexVersion { get; set; }
    }
}
