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

        DataValidation validation;

        public TextEditorForm(EditorTypeConfig editorTypeConfig)
        {
            InitializeComponent();
            
            validation = new DataValidation();

            config = editorTypeConfig;

            if (config != null ) 
            { 
                if (config.Separator != null)
                {
                    if (config.Value != null && config.Value.Length > 0)
                    {
                        string[] lines = config.Value.Split(config.Separator[0]);

                        foreach (string line in lines)
                        {
                            Lines.Add(line.TrimStart(' ').TrimEnd(' '));
                        }

                        ItemsText.Lines = Lines.ToArray();
                    }
                    
                } else
                {
                    ItemsText.Text = config.Value;
                }

                // does not work... create a tag editor instead
                if (config.AutoCompleteItems != null)
                {
                    //AutoCompleteStringCollection autoCompleteStringCollection = new AutoCompleteStringCollection();
                    //autoCompleteStringCollection.AddRange(config.AutoCompleteItems);

                    AutoCompleteItems.Items = config.AutoCompleteItems;

                    //ItemsText.AutoCompleteCustomSource = autoCompleteStringCollection;
                    //ItemsText.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    //ItemsText.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

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
                            duplicates.AddRange(validation.ValidateDuplicateStrings(ItemsText.Lines));

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

                        if (ItemsText.Text.Contains("|") || ItemsText.Text.Contains(","))
                        {
                            ApplicationMessage.ShowError("Invalid Value! The following characters are not allowed:\r\n\r\n" + String.Join("\r\n", new string[] { "\",\"", "\"|\"" }), "Invalid Value", ApplicationMessage.DialogType.MT_ERROR, ApplicationMessage.DialogButtons.MB_OK);

                            e.Cancel = true;
                        }

                        if (config.ResultType == EditorTypeConfig.RESULT_TYPE_STRING)
                        {
                            result = String.Join(config.Separator ?? "" + config.Append ?? "", ItemsText.Lines);
                        }

                        if (config.ResultType == EditorTypeConfig.RESULT_TYPE_STRINGS)
                        {
                            result = ItemsText.Lines;
                        }

                        config.Result = result;

                        DialogResult = DialogResult.OK;
                    }
                }
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            ItemsText.Lines = ItemsText.Lines.OrderBy(s => s).ToArray();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {

        }

        private void ItemEditorTableLayout_Paint(object sender, PaintEventArgs e)
        {

        }

        private void TextEditorForm_Load(object sender, EventArgs e)
        {

        }
    }
}
