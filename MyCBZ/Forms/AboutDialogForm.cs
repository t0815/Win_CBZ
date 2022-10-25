using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyCBZ.Forms
{
    public partial class AboutDialogForm : Form
    {
        public AboutDialogForm()
        {
            InitializeComponent();
            LicenseInfoRichtextBox.Rtf = global::MyCBZ.Properties.Resources.CBZMageAboutMessage;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
