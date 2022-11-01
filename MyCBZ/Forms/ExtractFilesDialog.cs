using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Win_CBZ.Forms
{
    public partial class ExtractFilesDialog : Form
    {

        public String TargetFolder;
        private bool FormShown = false;
        private bool CanClose;

        public ExtractFilesDialog()
        {
            InitializeComponent();

            
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            CanClose = true;
        }

        private void ExtractFilesDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = !CanClose;
        }

        private void RadioButtonExtractSelected_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void ButtonBrowse_Click(object sender, EventArgs e)
        {
            if (FolderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                TextBoxOutputFolder.Text = FolderBrowserDialog.SelectedPath.ToString();
            }
        }

        private void ExtractFilesDialog_Shown(object sender, EventArgs e)
        {
            if (!FormShown)
            {
                TextBoxOutputFolder.Text = TargetFolder;
                FormShown = true;
            }
        }

        private void ButtonOk_Click(object sender, EventArgs e)
        {
            
            try
            {
                DirectoryInfo di = new DirectoryInfo(TextBoxOutputFolder.Text);
                if (!di.Exists)
                {
                    CanClose = false;
                    ApplicationMessage.ShowWarning("Selected Path does not exist! Please choose an other one!", "Extract Files");
                    return;
                }
                TargetFolder = TextBoxOutputFolder.Text;
                CanClose = true;
            }
            catch (Exception mv)
            {
                CanClose = false;
                MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_ERROR, mv.Message);
                ApplicationMessage.ShowException(mv);
            }
        }
    }
}
