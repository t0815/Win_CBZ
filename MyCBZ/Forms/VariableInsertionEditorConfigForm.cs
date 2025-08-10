using AutocompleteMenuNS;
using System;
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
    public partial class VariableInsertionEditorConfigForm : Form
    {
        public EditorTypeConfig Config;

        private DataValidation Validation;

        public VariableInsertionEditorConfigForm(EditorTypeConfig editorTypeConfig)
        {
            InitializeComponent();

            Config = editorTypeConfig;

            if (Config != null)
            {

                TextBoxOutput.Text = Config.Value;


                // 
                if (Config.AutoCompleteItems != null)
                {
                    //AutoCompleteStringCollection autoCompleteStringCollection = new AutoCompleteStringCollection();
                    //autoCompleteStringCollection.AddRange(config.AutoCompleteItems);

                    var items = new List<AutocompleteItem>();
                    foreach (var item in Config.AutoCompleteItems)
                        items.Add(new AutocompleteItem(item) { ImageIndex = AutocompleteIcons.Images.IndexOfKey(Config.AutoCompleteImageKey) });

                    if (Config.AutoCompleteImageKey == "" || Config.AutoCompleteImageKey == null)
                        AutoCompleteVariables.LeftPadding = 2;
                    else AutoCompleteVariables.LeftPadding = 18;

                    AutoCompleteVariables.SetAutocompleteItems(items);

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

        private void VariableInsertionEditorConfigForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            object result = null;

            if (DialogResult == DialogResult.OK)
            {
                if (Config != null)
                {


                    if (TextBoxOutput.Text.Contains("|") || TextBoxOutput.Text.Contains(","))
                    {
                        ApplicationMessage.ShowError("Invalid Value! The following characters are not allowed:\r\n\r\n" + String.Join("\r\n", new string[] { "\",\"", "\"|\"" }), "Invalid Value", ApplicationMessage.DialogType.MT_ERROR, ApplicationMessage.DialogButtons.MB_OK);

                        e.Cancel = true;
                    }

                    if (Config.ResultType == EditorTypeConfig.RESULT_TYPE_STRING)
                    {
                        result = TextBoxOutput.Text;
                    }

                    if (Config.ResultType == EditorTypeConfig.RESULT_TYPE_STRINGS)
                    {
                        throw new ApplicationException("Output type Strings[] not supported by this editor!", true);
                    }

                    Config.Result = result;


                }
            }
        }

        private void VariableInsertionEditorConfigForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                DialogResult = DialogResult.Cancel;
                Close();
            }
        }
    }
}
