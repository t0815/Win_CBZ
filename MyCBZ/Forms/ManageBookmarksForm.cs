using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Win_CBZ.Forms
{
    internal partial class ManageBookmarksForm : Form
    {
        public ManageBookmarksForm(MetaData metaData, List<Page> pages)
        {
            InitializeComponent();



            pages.ForEach(page =>
            {
                var item = new ListViewItem(page.Name)
                {
                    Tag = page
                };

                item.SubItems.Add(page.Number.ToString());
                item.SubItems.Add(page.Bookmark);
                PagesList.Items.Add(item);
            });
        }
    }
}
