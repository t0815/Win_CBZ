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
    public partial class PasteBookmarksOffsetForm : Form
    {

        public int offset = 0;

        public PasteBookmarksOffsetForm()
        {
            InitializeComponent();

            Theme.GetInstance().ApplyTheme(BookmarkEditorTableLayout.Controls);
        }

        private void OkButton_Click(object sender, EventArgs e)
        {

        }

        private void TextboxOffset_TextChanged(object sender, EventArgs e)
        {
            try
            {
                offset = int.Parse(TextboxOffset.Text);
            }
            catch
            {
                offset = 0;
            }
        }
    }
}
