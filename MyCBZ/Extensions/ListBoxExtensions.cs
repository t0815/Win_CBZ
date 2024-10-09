using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Forms;

namespace Win_CBZ.Extensions
{
    [SupportedOSPlatform("windows")]
    public static class ListBoxExtensions
    {
        public static void Sort(this ListBox lb, SortOrder order = SortOrder.Ascending)
        {
            var list = lb.Items.Cast<Page>().ToArray();
            list = order == SortOrder.Descending
                        ? list.OrderByDescending(x => x.Index).ToArray()
                        : list.OrderBy(x => x.Index).ToArray();
            lb.Items.Clear();
            lb.Items.AddRange(list);
        }
    }
}
