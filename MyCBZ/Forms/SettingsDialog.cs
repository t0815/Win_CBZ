using SharpCompress.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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
    internal partial class SettingsDialog : Form
    {

        public String[] NewDefaults;

        public String[] NewValidTagList;

        private String[] FieldTypes;

        public String[] CustomFieldTypesCollection;

        public List<MetaDataFieldType> CustomFieldTypesSettings;


        public String MetaDataFilename;

        public bool ValidateTagsSetting;

        public bool TagValidationIgnoreCase;

        public int ConversionQualityValue;

        public int ConversionModeValue;

        public int MetaPageIndexWriteVersion;

        DataValidation validation;

        public SettingsDialog()
        {
            InitializeComponent();

            CustomFieldTypesSettings = new List<MetaDataFieldType>();

            if (Win_CBZSettings.Default.CustomDefaultProperties != null)
            {
                NewDefaults = Win_CBZSettings.Default.CustomDefaultProperties.OfType<String>().ToArray();
            }

            if (Win_CBZSettings.Default.ValidKnownTags != null)
            {
                NewValidTagList = Win_CBZSettings.Default.ValidKnownTags.OfType<String>().ToArray();
            }

            ConversionQualityValue = Win_CBZSettings.Default.ImageConversionQuality;
            ConversionModeValue = Win_CBZSettings.Default.ImageConversionMode;

            ValidateTagsSetting = Win_CBZSettings.Default.ValidateTags;
            TagValidationIgnoreCase = Win_CBZSettings.Default.TagValidationIgnoreCase;

            MetaPageIndexWriteVersion = Win_CBZSettings.Default.MetaDataPageIndexVersionToWrite;

            MetaDataFilename = Win_CBZSettings.Default.MetaDataFilename;

            FieldTypes = Win_CBZSettings.Default.CustomMetadataFieldTypes.OfType<String>().ToArray();
            CustomFieldTypesCollection = Win_CBZSettings.Default.CustomMetadataFields.OfType<String>().ToArray();

            // ----------------------------------------


            ValidTags.Lines = NewValidTagList;
            CustomDefaultKeys.Lines = NewDefaults;

            CheckBoxValidateTags.Checked = ValidateTagsSetting;
            CheckBoxTagValidationIgnoreCase.Checked = !TagValidationIgnoreCase;

            ComboBoxPageIndexVersionWrite.SelectedIndex = MetaPageIndexWriteVersion - 1;
            ComboBoxConvertPages.SelectedIndex = ConversionModeValue;
            ImageQualityTrackBar.Value = ConversionQualityValue;

            ComboBoxFileName.Text = MetaDataFilename;

            MetaDataConfigTabControl.Dock = DockStyle.Fill;
            ImageProcessingTabControl.Dock = DockStyle.Fill;
            AppSettingsTabControl.Dock = DockStyle.Fill;
            CBZSettingsTabControl.Dock = DockStyle.Fill;    

            MetaDataConfigTabControl.Visible = true;
            ImageProcessingTabControl.Visible = false;
            AppSettingsTabControl.Visible = false;
            CBZSettingsTabControl.Visible = false;

            CustomFieldsDataGrid.Columns.Add(new DataGridViewColumn()
            {
                DataPropertyName = "Name",
                HeaderText = "Key Name",
                CellTemplate = new DataGridViewTextBoxCell(),
                Width = 120,
                SortMode = DataGridViewColumnSortMode.Automatic,
            });

            CustomFieldsDataGrid.Columns.Add(new DataGridViewColumn()
            {
                DataPropertyName = "Type",
                HeaderText = "Field Type",
                CellTemplate = new DataGridViewTextBoxCell(),
                Width = 100,
                SortMode = DataGridViewColumnSortMode.Automatic,
            });

            CustomFieldsDataGrid.Columns.Add(new DataGridViewColumn()
            {
                DataPropertyName = "Editor",
                HeaderText = "Editor",
                CellTemplate = new DataGridViewTextBoxCell(),
                Width = 100,
                SortMode = DataGridViewColumnSortMode.Automatic,
            });

            CustomFieldsDataGrid.Columns.Add(new DataGridViewColumn()
            {
                DataPropertyName = "Items",
                HeaderText = "Items",
                CellTemplate = new DataGridViewTextBoxCell(),
                Width = 100,
                SortMode = DataGridViewColumnSortMode.Automatic,
            });

            CustomFieldsDataGrid.Columns.Add(new DataGridViewColumn()
            {
                DataPropertyName = "",
                HeaderText = "",
                CellTemplate = new DataGridViewTextBoxCell(),
                Width = 30,
                SortMode = DataGridViewColumnSortMode.NotSortable,
                Resizable = DataGridViewTriState.False,

            });

            CustomFieldTypesSettings.Clear();
            CustomFieldsDataGrid.Rows.Clear();
            foreach (String line in CustomFieldTypesCollection)
            {
                String[] typeParts = line.Split('|');

                CustomFieldTypesSettings.Add(new MetaDataFieldType(typeParts[0], typeParts[1], typeParts[2], typeParts[3]));

                CustomFieldsDataGrid.Rows.Add(typeParts[0], typeParts[1], typeParts[2], typeParts[3]);
            }


                // DataGridViewCellStyle currentStyle = null;
            
            for (int i = 0; i < CustomFieldsDataGrid.RowCount; i++)
            {
                CustomFieldsDataGrid.Rows[i].Cells[4].ReadOnly = true;

                foreach (MetaDataFieldType line in CustomFieldTypesSettings)
                {
                    var key = CustomFieldsDataGrid.Rows[i].Cells[0].Value;
                    if (key != null)
                    {
                        if (line.Name == key.ToString())
                        {
                            int selectedIndex = Array.IndexOf(FieldTypes, line.FieldType);
                            DataGridViewComboBoxCell cc = new DataGridViewComboBoxCell();
                            cc.Items.AddRange(FieldTypes);
                            cc.Value = line.FieldType; // selectedIndex > -1 ? selectedIndex : 0;
                            cc.Tag = new EditorTypeConfig("ComboBox", "String", "", " ", false);
                            
                            CustomFieldsDataGrid.Rows[i].Cells[1] = cc;

                            selectedIndex = Array.IndexOf(EditorTypeConfig.Editors, line.EditorType);
                            cc = new DataGridViewComboBoxCell();
                            cc.Items.AddRange(EditorTypeConfig.Editors);
                            cc.Value = line.EditorType; // selectedIndex > -1 ? selectedIndex : 0;
                            cc.Tag = new EditorTypeConfig("ComboBox", "String", "", " ", false);

                            //c.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                            ///c.DisplayStyleForCurrentCellOnly = true;
                            //c.Style = new DataGridViewCellStyle()
                            //{
                            //    SelectionForeColor = Color.Black,
                            //    SelectionBackColor = ((i + 1) % 2 > 0) ? Color.White : Color.FromKnownColor(KnownColor.ControlLight),
                            //};
                            

                            CustomFieldsDataGrid.Rows[i].Cells[2] = cc;


                            if (line.FieldType == EditorFieldMapping.MetaDataFieldTypeComboBox)
                            {
                                DataGridViewButtonCell bc = new DataGridViewButtonCell
                                {
                                    Value = "...",
                                    Tag = new EditorTypeConfig("MultiLineTextEditor", "String", ",", " ", false),
                                    Style = new DataGridViewCellStyle()
                                    {
                                        SelectionForeColor = Color.White,
                                        SelectionBackColor = Color.White,
                                    }
                                };

                                CustomFieldsDataGrid.Rows[i].Cells[4] = bc;
                            }
                        }
                    }
                }
            }

            validation = new DataValidation();

            DialogResult = DialogResult.Cancel;
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

                    if (CheckBoxValidateTags.Checked)
                    {
                        List<String> test = new List<String>(ValidTags.Lines);
                        String[] duplicateTags = validation.ValidateDuplicateStrings(test.ToArray());
                        if (duplicateTags.Length > 0)
                        {
                            //ApplicationMessage.ShowWarning("Validateion Error! Duplicate Tags [" + duplicateTags.Select(r => r + ", ") + "] not allowed!", "Validation Error", ApplicationMessage.DialogType.MT_WARNING, ApplicationMessage.DialogButtons.MB_OK);
                            MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_WARNING, "Validateion Error! Duplicate Tags [" + String.Join(",", duplicateTags) + "] not allowed!");

                            throw new MetaDataValidationException("", "", "Validateion Error! Duplicate Tags [" + String.Join(",", duplicateTags) + "] not allowed!");
                        }
                    }

                    if (ComboBoxFileName.Text.Length == 0)
                    {
                        throw new MetaDataValidationException("", "", "Validateion Error! Empty MetaData- Filename not allowed!");
                    }
                    else
                    {

                    }

                    // -------------- DANGER!  All validation goes above this line --------------------


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
                    ConversionModeValue = ComboBoxConvertPages.SelectedIndex;
                    ConversionQualityValue = ImageQualityTrackBar.Value;
                    MetaDataFilename = ComboBoxFileName.Text;
                    MetaPageIndexWriteVersion = ComboBoxPageIndexVersionWrite.SelectedIndex + 1;
                    List<String> fieldConfigItems = new List<string>();
                    foreach (MetaDataFieldType fieldTypeCnf in CustomFieldTypesSettings)
                    {
                        fieldConfigItems.Add(fieldTypeCnf.ToString());
                        
                    }
                    CustomFieldTypesCollection = fieldConfigItems.ToArray();    
                }
                catch (MetaDataValidationException mv)
                {
                    DialogResult = DialogResult.Cancel;
                    e.Cancel = true;
                    MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_ERROR, mv.Message);
                    ApplicationMessage.ShowWarning(mv.Message, "Validation Error", ApplicationMessage.DialogType.MT_WARNING, ApplicationMessage.DialogButtons.MB_OK);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CustomDefaultKeys.Text = Program.ProjectModel.MetaData.GetDefaultKeys();
        }

        private void CheckBoxValidateTags_CheckStateChanged(object sender, EventArgs e)
        {
            CheckBoxTagValidationIgnoreCase.Enabled = CheckBoxValidateTags.CheckState == CheckState.Checked;
        }

        private void ToolButtonSortAscending_Click(object sender, EventArgs e)
        {
            ValidTags.Lines = ValidTags.Lines.OrderBy(s => s).ToArray();
        }

        private void SettingsSectionList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SettingsSectionList.SelectedIndex == 0)
            {
                MetaDataConfigTabControl.Visible = true;
                ImageProcessingTabControl.Visible = false;
                AppSettingsTabControl.Visible = false;
                CBZSettingsTabControl.Visible = false;
            }

            if (SettingsSectionList.SelectedIndex == 1)
            {
                MetaDataConfigTabControl.Visible = false;
                ImageProcessingTabControl.Visible = false;
                AppSettingsTabControl.Visible = true;
                CBZSettingsTabControl.Visible = false;
            }

            if (SettingsSectionList.SelectedIndex == 2)
            {
                MetaDataConfigTabControl.Visible = false;
                ImageProcessingTabControl.Visible = false;
                AppSettingsTabControl.Visible = false;
                CBZSettingsTabControl.Visible = true;
            }

            if (SettingsSectionList.SelectedIndex == 3)
            {
                MetaDataConfigTabControl.Visible = false;
                ImageProcessingTabControl.Visible = true;
                AppSettingsTabControl.Visible = false;
                CBZSettingsTabControl.Visible = false;
            }
        }

        private void SettingsDialog_Load(object sender, EventArgs e)
        {

        }

        private void CustomFieldsDataGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (e.RowIndex > -1)
            {
                if ((senderGrid.Rows[e.RowIndex].Cells[e.ColumnIndex] is DataGridViewButtonCell ||
                    senderGrid.Rows[e.RowIndex].Cells[e.ColumnIndex] is DataGridViewComboBoxCell
                    ) &&
                    e.RowIndex >= 0)
                {
                    object value = null;
                    String valueText = "";
                    EditorTypeConfig editorConfig = senderGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag as EditorTypeConfig;
                    if (e.ColumnIndex == 4)
                    {
                        value = senderGrid.Rows[e.RowIndex].Cells[e.ColumnIndex - 1].Value;
                        if (value != null)
                        {
                            valueText = value.ToString();
                        }
                    }
                    else
                    {
                        value = senderGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
                        if (value != null)
                        {
                            valueText += value.ToString();
                        }
                    }

                    if (editorConfig != null)
                    {
                        editorConfig.Value = valueText;
                        switch (editorConfig.Type)
                        {
                            case EditorTypeConfig.EditorTypeMultiLineTextEditor:
                                {
                                    TextEditorForm textEditor = new TextEditorForm(editorConfig);
                                    DialogResult r = textEditor.ShowDialog();
                                    if (r == DialogResult.OK)
                                    {
                                        if (textEditor.config.Result != null)
                                        {
                                            senderGrid.Rows[e.RowIndex].Cells[e.ColumnIndex - 1].Value = textEditor.config.Result.ToString();
                                        }
                                    }
                                }
                                break;
                            case EditorTypeConfig.EditorTypeLanguageEditor:
                                {
                                    LanguageEditorForm langEditor = new LanguageEditorForm(editorConfig);
                                    DialogResult r = langEditor.ShowDialog();
                                    if (r == DialogResult.OK)
                                    {
                                        if (langEditor.config.Result != null)
                                        {
                                            senderGrid.Rows[e.RowIndex].Cells[e.ColumnIndex - 1].Value = langEditor.config.Result.ToString();
                                        }
                                    }
                                }
                                break;
                            default:
                                {
                                    DataGridViewComboBoxCell comboCell = senderGrid.Rows[e.RowIndex].Cells[e.ColumnIndex] as DataGridViewComboBoxCell;
                                    comboCell.Style = new DataGridViewCellStyle()
                                    {
                                        SelectionForeColor = Color.Black,
                                        SelectionBackColor = Color.White,
                                    };
                                    CustomFieldsDataGrid.BeginEdit(true);

                                }
                                break;


                        }
                    }
                }
            }
        }

        private void CustomFieldsDataGrid_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            if (e.Exception != null)
            {
                //MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_ERROR, e.Exception.Message);
            }
        }

        private void CustomFieldsDataGrid_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            int newIndex = e.RowIndex;
            //for (int i = 0; i < e.RowCount; i++)
            //{
            //newIndex = newIndex + i;

            //foreach (String line in CustomFieldTypesCollection)
            //{
            //String[] typeParts = line.Split('|');
            //var key = CustomFieldsDataGrid.Rows[i].Cells[0].Value;
            //if (key != null)
            //{

            /*
            if (CustomFieldsDataGrid.Rows[e.RowIndex].Cells.Count == 0)
            {
                
                MetaDataFieldType updatedEntry;
                try
                {
                    object Name = CustomFieldsDataGrid.Rows[e.RowIndex].Cells[0].Value;
                    object FieldType = CustomFieldsDataGrid.Rows[e.RowIndex].Cells[1].Value;
                    object EditorType = CustomFieldsDataGrid.Rows[e.RowIndex].Cells[2].Value;
                    object Options = CustomFieldsDataGrid.Rows[e.RowIndex].Cells[3].Value;

                    if (CustomFieldTypesSettings.Count > e.RowIndex)
                    {
                        updatedEntry = CustomFieldTypesSettings[e.RowIndex];
                        if (updatedEntry == null)
                        {
                            updatedEntry = new MetaDataFieldType(Name?.ToString(), FieldType?.ToString(), EditorType?.ToString(), Options?.ToString());
                            CustomFieldTypesSettings.Add(updatedEntry);
                        }
                    } else
                    {
                        updatedEntry = new MetaDataFieldType(Name?.ToString(), FieldType?.ToString(), EditorType?.ToString(), Options?.ToString());
                        CustomFieldTypesSettings.Add(updatedEntry);
                    }

                    CustomFieldsDataGrid.Rows[e.RowIndex].Cells[4].ReadOnly = true;

                    int selectedIndex = -1;
                    DataGridViewComboBoxCell cc = new DataGridViewComboBoxCell();
                    cc.Items.AddRange(FieldTypes);
                    cc.Value = updatedEntry.FieldType; // selectedIndex > -1 ? selectedIndex : 0;
                    cc.Tag = new EditorTypeConfig("ComboBox", "String", "", " ", false);

                    CustomFieldsDataGrid.Rows[e.RowIndex].Cells[1] = cc;

                    selectedIndex = -1;
                    cc = new DataGridViewComboBoxCell();
                    cc.Items.AddRange(EditorTypeConfig.Editors);
                    cc.Value = updatedEntry.EditorType; // selectedIndex > -1 ? selectedIndex : 0;
                    cc.Tag = new EditorTypeConfig("ComboBox", "String", "", " ", false);

                    CustomFieldsDataGrid.Rows[e.RowIndex].Cells[2] = cc;

                    if (updatedEntry.FieldType == EditorFieldMapping.MetaDataFieldTypeComboBox)
                    {
                        DataGridViewButtonCell bc = new DataGridViewButtonCell
                        {
                            Value = "...",
                            Tag = new EditorTypeConfig("MultiLineTextEditor", "String", ",", " ", false),
                            Style = new DataGridViewCellStyle()
                            {
                                SelectionForeColor = Color.White,
                                SelectionBackColor = Color.White,
                            }
                        };

                        CustomFieldsDataGrid.Rows[e.RowIndex].Cells[4] = bc;
                    }
                }
                catch
                {

                }
            }
            */
            
        }

        private void CustomFieldsDataGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {

                string Key = "";
                string FieldType = "";
                string EditorType = "";
                string Val = "";

                object value = CustomFieldsDataGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;

                if (e.ColumnIndex == 0)
                {
                    if (value == null)
                    {
                        value = "";
                    }

                    Key = value.ToString();

                    value = CustomFieldsDataGrid.Rows[e.RowIndex].Cells[e.ColumnIndex + 1].Value;
                    if (value == null)
                    {
                        value = "";
                    }

                    FieldType = value.ToString();

                    value = CustomFieldsDataGrid.Rows[e.RowIndex].Cells[e.ColumnIndex + 2].Value;
                    if (value == null)
                    {
                        value = "";
                    }

                    EditorType = value.ToString();

                    value = CustomFieldsDataGrid.Rows[e.RowIndex].Cells[e.ColumnIndex + 3].Value;
                    if (value == null)
                    {
                        value = "";
                    }

                    Val = value.ToString();
                }

                if (e.ColumnIndex == 1)
                {
                    if (value == null)
                    {
                        value = "";
                    }

                    FieldType = value.ToString();

                    value = CustomFieldsDataGrid.Rows[e.RowIndex].Cells[e.ColumnIndex - 1].Value;
                    if (value == null)
                    {
                        value = "";
                    }

                    Key = value.ToString();

                    value = CustomFieldsDataGrid.Rows[e.RowIndex].Cells[e.ColumnIndex + 1].Value;
                    if (value == null)
                    {
                        value = "";
                    }

                    EditorType = value.ToString();

                    value = CustomFieldsDataGrid.Rows[e.RowIndex].Cells[e.ColumnIndex + 2].Value;
                    if (value == null)
                    {
                        value = "";
                    }

                    Val = value.ToString();
                }

                if (e.ColumnIndex == 2)
                {
                    if (value == null)
                    {
                        value = "";
                    }

                    EditorType = value.ToString();

                    value = CustomFieldsDataGrid.Rows[e.RowIndex].Cells[e.ColumnIndex - 2].Value;
                    if (value == null)
                    {
                        value = "";
                    }

                    Key = value.ToString();

                    value = CustomFieldsDataGrid.Rows[e.RowIndex].Cells[e.ColumnIndex - 1].Value;
                    if (value == null)
                    {
                        value = "";
                    }

                    FieldType = value.ToString();

                    value = CustomFieldsDataGrid.Rows[e.RowIndex].Cells[e.ColumnIndex + 1].Value;
                    if (value == null)
                    {
                        value = "";
                    }

                    Val = value.ToString();
                }

                if (e.ColumnIndex == 3)
                {
                    if (value == null)
                    {
                        value = "";
                    }

                    Val = value.ToString();

                    value = CustomFieldsDataGrid.Rows[e.RowIndex].Cells[e.ColumnIndex - 3].Value;
                    if (value == null)
                    {
                        value = "";
                    }

                    Key = value.ToString();

                    value = CustomFieldsDataGrid.Rows[e.RowIndex].Cells[e.ColumnIndex - 2].Value;
                    if (value == null)
                    {
                        value = "";
                    }

                    FieldType = value.ToString();

                    value = CustomFieldsDataGrid.Rows[e.RowIndex].Cells[e.ColumnIndex - 1].Value;
                    if (value == null)
                    {
                        value = "";
                    }

                    Key = value.ToString();
                }

                MetaDataFieldType updatedEntry = CustomFieldTypesSettings[e.RowIndex];
                CustomFieldsDataGrid.Rows[e.RowIndex].ErrorText = null;
                CustomFieldsDataGrid.Invalidate();

                //if (e.ColumnIndex == 0)
                //{
                    //var key = CustomFieldsDataGrid.Rows[e.RowIndex].Cells[0].Value;
                    //if (key != null)
                    //{
                        if (updatedEntry != null)
                        {
                            CustomFieldsDataGrid.Rows[e.RowIndex].Cells[4].ReadOnly = true;
                            updatedEntry.Options = Val;
                    updatedEntry.FieldType = FieldType;
                    updatedEntry.Name = Key;
                    updatedEntry.EditorType = EditorType;


                            if (updatedEntry.FieldType == EditorFieldMapping.MetaDataFieldTypeComboBox)
                            {
                                /*
                                if (updatedEntry.Options.Length > 0)
                                {
                                    int selectedIndex = Array.IndexOf(updatedEntry.Options, updatedEntry.);
                                    DataGridViewComboBoxCell c = new DataGridViewComboBoxCell();
                                    c.Items.AddRange(updatedEntry.Options);
                                    c.Value = value; //selectedIndex > -1 ? selectedIndex : 0;
                                    c.Tag = new EditorTypeConfig("ComboBox", "String", "", "", false);

                                    c.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                                    //c.DisplayStyleForCurrentCellOnly = true;

                                    CustomFieldsDataGrid.Rows[e.RowIndex].Cells[1] = c;
                                    //c.ReadOnly = true;
                                }
                                */

                                DataGridViewButtonCell bc = new DataGridViewButtonCell
                                {
                                    Value = "...",
                                    Tag = new EditorTypeConfig(EditorTypeConfig.EditorTypeMultiLineTextEditor, "String", ",", " ", false)
                                };
                                CustomFieldsDataGrid.Rows[e.RowIndex].Cells[4] = bc;
                            }
                            
                            if (updatedEntry.EditorType == EditorTypeConfig.EditorTypeMultiLineTextEditor)
                            {
                                
                            }
                            else if (updatedEntry.EditorType == EditorTypeConfig.EditorTypeLanguageEditor)
                            {
                                DataGridViewButtonCell bc = new DataGridViewButtonCell
                                {
                                    Value = "...",
                                    Tag = new EditorTypeConfig(EditorTypeConfig.EditorTypeLanguageEditor, "String", "", "", false)
                                };
                                CustomFieldsDataGrid.Rows[e.RowIndex].Cells[4] = bc;

                            }
                            else
                            {
                                //DataGridViewTextBoxCell c = new DataGridViewTextBoxCell
                                //{
                                //    Value = updatedEntry.Value
                                //};

                                //CustomFieldsDataGrid.Rows[e.RowIndex].Cells[1] = c;

                            }
                        }
                    //}
               // }
            }
            catch (Exception ex)
            {
                MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_ERROR, ex.Message);
            }
        }

        private void AddFieldTypeButton_Click(object sender, EventArgs e)
        {
            int newIndex = CustomFieldsDataGrid.Rows.Add("", "Text", "", "", "");
            MetaDataFieldType newEntry = new MetaDataFieldType("", "Text", "", "");
            CustomFieldTypesSettings.Add(newEntry);

            CustomFieldsDataGrid.Rows[newIndex].Cells[4].ReadOnly = true;

            int selectedIndex = -1;
            DataGridViewComboBoxCell cc = new DataGridViewComboBoxCell();
            cc.Items.AddRange(FieldTypes);
            cc.Value = newEntry.FieldType; // selectedIndex > -1 ? selectedIndex : 0;
            cc.Tag = new EditorTypeConfig("ComboBox", "String", "", " ", false);

            CustomFieldsDataGrid.Rows[newIndex].Cells[1] = cc;

            selectedIndex = -1;
            cc = new DataGridViewComboBoxCell();
            cc.Items.AddRange(EditorTypeConfig.Editors);
            cc.Value = newEntry.EditorType; // selectedIndex > -1 ? selectedIndex : 0;
            cc.Tag = new EditorTypeConfig("ComboBox", "String", "", " ", false);

            CustomFieldsDataGrid.Rows[newIndex].Cells[2] = cc;

            if (newEntry.FieldType == EditorFieldMapping.MetaDataFieldTypeComboBox)
            {
                DataGridViewButtonCell bc = new DataGridViewButtonCell
                {
                    Value = "...",
                    Tag = new EditorTypeConfig("MultiLineTextEditor", "String", ",", " ", false),
                    Style = new DataGridViewCellStyle()
                    {
                        SelectionForeColor = Color.White,
                        SelectionBackColor = Color.White,
                    }
                };

                CustomFieldsDataGrid.Rows[newIndex].Cells[4] = bc;
            }
        }

        private void CustomFieldsDataGrid_SelectionChanged(object sender, EventArgs e)
        {
            RemoveFieldTypeButton.Enabled = CustomFieldsDataGrid.SelectedRows.Count > 0;
        }

        private void RemoveFieldTypeButton_Click(object sender, EventArgs e)
        {
            List<DataGridViewRow> rowsToRemove = new List<DataGridViewRow>();
            if (CustomFieldsDataGrid.SelectedRows.Count > 0)
            {
                foreach (DataGridViewRow row in CustomFieldsDataGrid.SelectedRows)
                {
                    rowsToRemove.Add(row);  
                    //int index = .Remove(row.Index);

                    /*
                    if (row.Cells[0].Value != null)
                    {
                        var key = row.Cells[0].Value.ToString();  

                        Program.ProjectModel.MetaData.Remove(key);
                    } */
                }

                foreach (DataGridViewRow row in rowsToRemove) 
                {
                    CustomFieldTypesSettings.RemoveAt(row.Index);
                    CustomFieldsDataGrid.Rows.Remove(row);
                }
            }
        }
    }
}
