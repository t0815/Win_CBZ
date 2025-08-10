using AutocompleteMenuNS;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Win_CBZ.Data;
using Win_CBZ.Models;

namespace Win_CBZ.Forms
{
    [SupportedOSPlatform("windows")]
    public partial class RomajiEditorForm : Form
    {

        public List<String> Lines = new List<String>();

        public EditorTypeConfig Config;

        private DataValidation Validation;

        private string ResultText;

        public RomajiEditorForm(EditorTypeConfig editorTypeConfig)
        {
            InitializeComponent();

            Validation = new DataValidation();

            Config = editorTypeConfig;

            if (Config != null)
            {
                if (Config.Separator != null)
                {
                    if (Config.Value != null && Config.Value.Length > 0)
                    {

                    }

                }
                else
                {
                    ToolStripTextBoxSearch.Text = Config.Value;
                }

                // 
                if (Config.AutoCompleteItems != null)
                {
                    //AutoCompleteStringCollection autoCompleteStringCollection = new AutoCompleteStringCollection();
                    //autoCompleteStringCollection.AddRange(config.AutoCompleteItems);

                    var items = new List<AutocompleteItem>();
                    foreach (var item in Config.AutoCompleteItems)
                        items.Add(new AutocompleteItem(item) { ImageIndex = AutocompleteIcons.Images.IndexOfKey(Config.AutoCompleteImageKey) });

                    if (Config.AutoCompleteImageKey == "" || Config.AutoCompleteImageKey == null)
                        AutoCompleteItems.LeftPadding = 2;
                    else AutoCompleteItems.LeftPadding = 18;

                    AutoCompleteItems.SetAutocompleteItems(items);

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
                        /*
                        duplicates.AddRange(Validation.ValidateDuplicateStrings(ResetText));

                        if (duplicates.Count > 0)
                        {
                            ApplicationMessage.ShowError("Invalid Value! Duplicate entry detected.\r\n\r\n" + String.Join("\r\n", duplicates), "Invalid Value", ApplicationMessage.DialogType.MT_ERROR, ApplicationMessage.DialogButtons.MB_OK);

                            DialogResult = DialogResult.None;

                            e.Cancel = true;
                        }
                        else
                        {

                        }
                        */
                    }

                    if (ResultText.Contains("|") || ResultText.Contains(","))
                    {
                        ApplicationMessage.ShowError("Invalid Value! The following characters are not allowed:\r\n\r\n" + String.Join("\r\n", new string[] { "\",\"", "\"|\"" }), "Invalid Value", ApplicationMessage.DialogType.MT_ERROR, ApplicationMessage.DialogButtons.MB_OK);

                        e.Cancel = true;
                    }

                    if (Config.ResultType == EditorTypeConfig.RESULT_TYPE_STRING)
                    {
                        result = ResultText;
                    }

                    if (Config.ResultType == EditorTypeConfig.RESULT_TYPE_STRINGS)
                    {
                        result = ResultText;
                    }

                    Config.Result = result;

                    DialogResult = DialogResult.OK;
                }
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            //ItemsText.Lines = ItemsText.Lines.OrderBy(s => s).ToArray();
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

        private void ToolStripTextBoxSearch_KeyUp(object sender, KeyEventArgs e)
        {
            //String itemsText = ItemsText.Text;

            //int occurence = itemsText.IndexOf(ToolStripTextBoxSearch.Text, 0, StringComparison.CurrentCultureIgnoreCase);


        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void RomajiEditorForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                DialogResult = DialogResult.Cancel;
                Close();
            }
        }
    }
}
