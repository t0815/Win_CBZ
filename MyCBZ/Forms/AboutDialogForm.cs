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
            this.Close();
        }

        private void AboutDialogForm_Load(object sender, EventArgs e)
        {

        }
    }
}
