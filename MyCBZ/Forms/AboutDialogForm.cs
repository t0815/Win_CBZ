using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Win_CBZ.Forms
{
    [SupportedOSPlatform("windows")]
    public partial class AboutDialogForm : Form
    {
        public AboutDialogForm()
        {
            InitializeComponent();

            AppNameLabel.Text = Assembly.GetExecutingAssembly().GetName().Name;
            AppVersionLabel.Text = "v" + Assembly.GetExecutingAssembly().GetName().Version;
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

        private void LicenseInfoRichtextBox_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            var link = e.LinkText;

            Process.Start(new ProcessStartInfo(link) { UseShellExecute = true });
        }
    }
}
