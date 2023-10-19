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

        public String[] NewDefaults;

        private bool CanClose;

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
            CanClose = true;
        }

        private void ButtonOk_Click(object sender, EventArgs e)
        {
            CanClose = true;
            Program.ProjectModel.MetaData.CustomDefaultProperties = new List<String>(CustomDefaultKeys.Lines.ToArray<String>());
            Program.ProjectModel.MetaData.MakeDefaultKeys(Program.ProjectModel.MetaData.CustomDefaultProperties);
            try
            {
                Program.ProjectModel.MetaData.ValidateDefaults();
                if (Win_CBZSettings.Default.CustomDefaultProperties != null)
                {
                    Win_CBZSettings.Default.CustomDefaultProperties.Clear();
                }
                else
                {
                    Win_CBZSettings.Default.CustomDefaultProperties = new StringCollection();
                }

                NewDefaults = CustomDefaultKeys.Lines.ToArray<String>();
            }
            catch (MetaDataValidationException mv)
            {
                CanClose = false;
                MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_ERROR, mv.Message);
                ApplicationMessage.ShowException(mv);
            }          
        }

        private void SettingsDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = !CanClose;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CustomDefaultKeys.Text = Program.ProjectModel.MetaData.GetDefaultKeys();
        }
    }
}
