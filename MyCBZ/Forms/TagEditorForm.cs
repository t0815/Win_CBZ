using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using Win_CBZ.Data;

namespace Win_CBZ.Forms
{
    public partial class TagEditorForm : Form
    {
        public List<String> Lines = new List<String>();

        public EditorTypeConfig config;

        DataValidation validation;

        public TagEditorForm(EditorTypeConfig editorTypeConfig)
        {
            InitializeComponent();

            validation = new DataValidation();

            config = editorTypeConfig;

            if (config != null)
            {
                if (config.Separator != null)
                {
                    if (config.Value != null)
                    {
                        string[] lines = config.Value.Split(config.Separator[0]);

                        foreach (string line in lines)
                        {
                            Lines.Add(line.TrimStart(' ').TrimEnd(' '));
                        }                     
                    }

                }
                else
                {
                    Lines.AddRange(config.Value.Split('\n'));
                }

                if (config.AutoCompleteItems != null)
                {
                    AutoCompleteStringCollection autoCompleteStringCollection = new AutoCompleteStringCollection();
                    autoCompleteStringCollection.AddRange(config.AutoCompleteItems);

                    TagTextBox.AutoCompleteCustomSource = autoCompleteStringCollection;
                    TagTextBox.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    TagTextBox.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

                }
            }
        }

        private void OkButton_Click(object sender, EventArgs e)
        {

        }

        private void TagEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            object result = null;
            List<String> duplicates = new List<String>();

            if (DialogResult == DialogResult.OK)
            {
                if (Lines.Count > 0)
                {
                    if (config != null)
                    {
                        if (!config.AllowDuplicateValues)
                        {
                            duplicates.AddRange(validation.ValidateDuplicateStrings(Lines.ToArray()));

                            if (duplicates.Count > 0)
                            {
                                ApplicationMessage.ShowError("Invalid Value! Duplicate entry detected.\r\n\r\n" + String.Join("\r\n", duplicates), "Invalid Value", ApplicationMessage.DialogType.MT_ERROR, ApplicationMessage.DialogButtons.MB_OK);

                                DialogResult = DialogResult.None;

                                e.Cancel = true;
                            }
                            else
                            {

                            }
                        }


                        if (config.ResultType == "String")
                        {
                            result = String.Join(config.Separator ?? "" + config.Append ?? "", Lines);
                        }

                        if (config.ResultType == "String[]")
                        {
                            result = Lines;
                        }

                        config.Result = result;

                        DialogResult = DialogResult.OK;
                    }
                }
            }
        }
    }
}
