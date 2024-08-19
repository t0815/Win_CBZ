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
    public partial class AboutDialogForm : Form
    {
        public AboutDialogForm()
        {
            InitializeComponent();

            AppNameLabel.Text = Win_CBZSettings.Default.AppName;
            AppVersionLabel.Text = "v" + Win_CBZSettings.Default.Version;
            LicenseInfoRichtextBox.Rtf = global::Win_CBZ.Properties.Resources.CBZMageAboutMessage;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void AboutDialogForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Close();
            }
        }
    }
}
