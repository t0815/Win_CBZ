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
    public partial class AutoCreateBookmarksForm : Form
    {

        public string BookmarkPrefix { get; set; } = "Chapter";

        public AutoCreateBookmarksForm()
        {
            InitializeComponent();
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            BookmarkPrefix = TextboxPrefix.Text.Trim();
        }
    }
}
