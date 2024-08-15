using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Win_CBZ.Data
{
    internal class MoveItemsToThreadParams
    {

        public int newIndex { get; set; }

        public System.Windows.Forms.ListView.SelectedListViewItemCollection items { get; set; }

        public MetaData.PageIndexVersion pageIndexVersion { get; set; }

        public CancellationToken CancelToken { get; set; }
    }
}
