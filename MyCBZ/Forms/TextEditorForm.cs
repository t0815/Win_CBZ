using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Win_CBZ.Data;

namespace Win_CBZ.Forms
{
    public partial class TextEditorForm : Form
    {

        public List<String> Lines = new List<String>();

        public EditorTypeConfig config;


        public TextEditorForm(EditorTypeConfig editorTypeConfig)
        {
            InitializeComponent();

            config = editorTypeConfig;

            if (config != null ) 
            { 
                if (config.Separator != null)
                {
                    string[] lines = config.Value.Split(config.Separator[0]);

                    foreach ( string line in lines )
                    {
                        Lines.Add(line.TrimStart(' ').TrimEnd(' '));
                    }

                    ItemsText.Lines = Lines.ToArray();
                } else
                {
                    ItemsText.Text = config.Value;
                }
            }

        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void TextEditorForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            object result = null;
            List<String> duplicates = new List<String>();

            if (DialogResult == DialogResult.OK)
            {
                if (ItemsText.Lines.Length > 0)
                {
                    if (config != null)
                    {
                        if (!config.AllowDuplicateValues)
                        {
                            duplicates.AddRange(DataValidation.validateDuplicateStrings(ItemsText.Lines));

                            if (duplicates.Count > 0)
                            {
                                ApplicationMessage.ShowError("Invalid Value! Duplicate entry detected.", "Invalid Value", 3, ApplicationMessage.DialogButtons.MB_OK);

                                DialogResult = DialogResult.None;

                                e.Cancel = true;
                            }
                            else
                            {

                            }
                        }


                        if (config.ResultType == "String")
                        {
                            result = String.Join(config.Separator ?? "" + config.Append ?? "", ItemsText.Lines);
                        }

                        if (config.ResultType == "String[]")
                        {
                            result = ItemsText.Lines;
                        }

                        config.Result = result;

                        DialogResult = DialogResult.OK;
                    }
                }
            }
        }
    }
}
