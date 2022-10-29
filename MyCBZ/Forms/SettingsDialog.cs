using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Win_CBZ.Forms
{
    public partial class SettingsDialog : Form
    {

        public SettingsDialog()
        {
            InitializeComponent();

            if (Win_CBZSettings.Default.CustomDefaultProperties != null)
            {
                CustomDefaultKeys.Lines = Win_CBZSettings.Default.CustomDefaultProperties.OfType<String>().ToArray();
            }
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {

        }

        private void ButtonOk_Click(object sender, EventArgs e)
        {
            if (Win_CBZSettings.Default.CustomDefaultProperties != null)
            {
                Win_CBZSettings.Default.CustomDefaultProperties.Clear();
            } else
            {
                Win_CBZSettings.Default.CustomDefaultProperties = new StringCollection();
            }

            foreach (String line in CustomDefaultKeys.Lines)
            {
                if (line != null && line != "")
                {
                    Win_CBZSettings.Default.CustomDefaultProperties.Add(line);
                }
            }
        }
    }
}
