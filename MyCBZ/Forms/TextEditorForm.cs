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
    public partial class TextEditorForm : Form
    {

        public List<String> Lines = new List<String>();

        public EditorTypeConfig Config;

        private DataValidation Validation;

        private int lastSearchOccurence = 0;

        private int occurence = 0;

        private int nextOccurence = 0;

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
                            Lines.Add(line.TrimStart().TrimEnd());
                        }

                        ItemsText.Lines = Lines.ToArray();
                    }
                    
                } else
                {
                    ItemsText.Text = Config.Value;
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

        private void ToolStripTextBoxSearch_KeyUp(object sender, KeyEventArgs e)
        {
            String itemsText = ItemsText.Text;

            if (e.KeyCode == Keys.F3)
            {
                lastSearchOccurence = occurence + ToolStripTextBoxSearch.Text.Length;

                ItemsText.SelectionStart = lastSearchOccurence + ToolStripTextBoxSearch.Text.Length;
                ItemsText.SelectionLength = 0;

                nextOccurence = itemsText.IndexOf(ToolStripTextBoxSearch.Text, lastSearchOccurence, StringComparison.CurrentCultureIgnoreCase);


                if (nextOccurence < 0)
                {
                    ApplicationMessage.Show("Search reached the end of the document. Starting from the beginning.", "Search", ApplicationMessage.DialogType.MT_INFORMATION, ApplicationMessage.DialogButtons.MB_OK);

                    lastSearchOccurence = 0;
                }
            } else
            {
                lastSearchOccurence = 0;
            }

            occurence = itemsText.IndexOf(ToolStripTextBoxSearch.Text, lastSearchOccurence, StringComparison.CurrentCultureIgnoreCase);

            ItemsText.SelectionStart = 0;
            ItemsText.SelectionLength = 0;
            if (occurence > -1)
            {
                ItemsText.SelectionStart = occurence;
                ItemsText.SelectionLength = ToolStripTextBoxSearch.Text.Length;
                ItemsText.ScrollToCaret();

                lastSearchOccurence = occurence;
            }   
            
            
        }
    }
}
