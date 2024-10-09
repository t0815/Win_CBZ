using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Win_CBZ.List
{
    internal class ListViewSorter : IComparer
    {
        private int col;


        public ListViewSorter()
        {
            col = 0;
        }
        public ListViewSorter(int column)
        {
            col = column;
        }
        public int Compare(object x, object y)
        {
            if (((ListViewItem)x).SubItems.Count > col)
            {
                if (int.TryParse(((ListViewItem)x).SubItems[col].Text, out int xInt) && int.TryParse(((ListViewItem)y).SubItems[col].Text, out int yInt))
                {
                    return xInt.CompareTo(yInt);
                }
                else {
                    return String.Compare(((ListViewItem)x).SubItems[col].Text, ((ListViewItem)y).SubItems[col].Text);
                }
            }
            else
            {
                return 0;
            }
        }
    }
}
