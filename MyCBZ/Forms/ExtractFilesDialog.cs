using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Win_CBZ.Events;

namespace Win_CBZ.Forms
{
    [SupportedOSPlatform("windows")]
    internal partial class ExtractFilesDialog : Form
    {

        public String TargetFolder;
        public List<Page> SelectedPages;

        public int ExtractType = 0;

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
            ExtractType = 1;
        }

        private void ButtonBrowse_Click(object sender, EventArgs e)
        {
            DialogResult res = FolderBrowserDialog.ShowDialog();

            if (res == DialogResult.OK)
            {
                TextBoxOutputFolder.Text = FolderBrowserDialog.SelectedPath.ToString();
            }
        }

        private void ExtractFilesDialog_Shown(object sender, EventArgs e)
        {
            TextBoxOutputFolder.Text = TargetFolder;
            if (SelectedPages != null && SelectedPages.Count > 0)
            {
                RadioButtonExtractSelected.Enabled = true;
                RadioButtonExtractSelected.Text = "Selected Pages (" + SelectedPages.Count.ToString() + ")";
                RadioButtonExtractSelected.Checked = true;
                ExtractType = 1;
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

        private void RadioButtonExtractAll_CheckedChanged(object sender, EventArgs e)
        {
            ExtractType = 0;
        }

        private void RadioButtonExtractSelected_CheckedChanged_1(object sender, EventArgs e)
        {
            ExtractType = 1;
        }
    }
}
