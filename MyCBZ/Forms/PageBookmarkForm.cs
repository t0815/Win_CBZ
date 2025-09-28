using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Win_CBZ.Forms
{

    [SupportedOSPlatform("windows")]
    public partial class PageBookmarkForm : Form
    {
        public PageBookmarkForm()
        {
            InitializeComponent();

            Theme.GetInstance().ApplyTheme(BookmarkEditorTableLayout.Controls);
        }

        private void PageBookmarkForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                DialogResult = DialogResult.Cancel;
                Close();
            }
        }
    }
}
