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

        public EditorTypeConfig Config;

        private DataValidation Validation;

        public TextEditorForm(EditorTypeConfig editorTypeConfig)
        {
            InitializeComponent();
            
            Validation = new DataValidation();

            Config = editorTypeConfig;

            if (Config != null ) 
            { 
                if (Config.Separator != null)
                {
                    if (Config.Value != null && Config.Value.Length > 0)
                    {
                        string[] lines = Config.Value.Split(Config.Separator[0]);

                        foreach (string line in lines)
                        {
                            Lines.Add(line.TrimStart(' ').TrimEnd(' '));
                        }

                        ItemsText.Lines = Lines.ToArray();
                    }
                    
                } else
                {
                    ItemsText.Text = Config.Value;
                }

                // does not work... create a tag editor instead
                if (Config.AutoCompleteItems != null)
                {
                    //AutoCompleteStringCollection autoCompleteStringCollection = new AutoCompleteStringCollection();
                    //autoCompleteStringCollection.AddRange(config.AutoCompleteItems);

                    AutoCompleteItems.Items = Config.AutoCompleteItems;

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
                if (Config != null)
                {
                    if (!Config.AllowDuplicateValues)
                    {
                        duplicates.AddRange(Validation.ValidateDuplicateStrings(ItemsText.Lines));

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

                    if (Config.ResultType == EditorTypeConfig.RESULT_TYPE_STRING)
                    {
                        result = String.Join(Config.Separator ?? "" + Config.Append ?? "", ItemsText.Lines);
                    }

                    if (Config.ResultType == EditorTypeConfig.RESULT_TYPE_STRINGS)
                    {
                        result = ItemsText.Lines;
                    }

                    Config.Result = result;

                    DialogResult = DialogResult.OK;
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
