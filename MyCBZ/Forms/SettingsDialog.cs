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

        public String[] NewValidTagList;

        public bool ValidateTagsSetting;

        public bool TagValidationIgnoreCase;

        public SettingsDialog()
        {
            InitializeComponent();

            if (Win_CBZSettings.Default.CustomDefaultProperties != null)
            {
                CustomDefaultKeys.Lines = Win_CBZSettings.Default.CustomDefaultProperties.OfType<String>().ToArray();
            }

            if (Win_CBZSettings.Default.ValidKnownTags != null)
            {
                ValidTags.Lines = Win_CBZSettings.Default.ValidKnownTags.OfType<String>().ToArray();
            }

            CheckBoxValidateTags.Checked = Win_CBZSettings.Default.ValidateTags;
            CheckBoxTagValidationIgnoreCase.Checked = !Win_CBZSettings.Default.TagValidationIgnoreCase;
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void ButtonOk_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;  
        }

        private void SettingsDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (DialogResult == DialogResult.OK)
            {
                Program.ProjectModel.MetaData.CustomDefaultProperties = new List<String>(CustomDefaultKeys.Lines.ToArray<String>());            
                try
                {
                    Program.ProjectModel.MetaData.MakeDefaultKeys(Program.ProjectModel.MetaData.CustomDefaultProperties);

                    Program.ProjectModel.MetaData.ValidateDefaults();
                    if (Win_CBZSettings.Default.CustomDefaultProperties != null)
                    {
                        Win_CBZSettings.Default.CustomDefaultProperties.Clear();
                    }
                    else
                    {
                        Win_CBZSettings.Default.CustomDefaultProperties = new StringCollection();
                    }

                    if (Win_CBZSettings.Default.ValidKnownTags != null)
                    {
                        Win_CBZSettings.Default.ValidKnownTags.Clear();
                    }
                    else
                    {
                        Win_CBZSettings.Default.ValidKnownTags = new StringCollection();
                    }

                    NewDefaults = CustomDefaultKeys.Lines.ToArray<String>();
                    NewValidTagList = ValidTags.Lines.ToArray<String>();
                    ValidateTagsSetting = CheckBoxValidateTags.Checked;
                    TagValidationIgnoreCase = !CheckBoxTagValidationIgnoreCase.Checked;
                }
                catch (MetaDataValidationException mv)
                {
                    e.Cancel = true;
                    MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_ERROR, mv.Message);
                    ApplicationMessage.ShowException(mv);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CustomDefaultKeys.Text = Program.ProjectModel.MetaData.GetDefaultKeys();
        }

        private void SettingsTablePanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void CheckBoxValidateTags_CheckStateChanged(object sender, EventArgs e)
        {
            CheckBoxTagValidationIgnoreCase.Enabled = CheckBoxValidateTags.CheckState == CheckState.Checked;
        }
    }
}
