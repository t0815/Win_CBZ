
using SharpCompress.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Win_CBZ.Data;
using Win_CBZ.Events;
using Win_CBZ.Helper;

namespace Win_CBZ.Forms
{
    [SupportedOSPlatform("windows")]
    internal partial class SettingsDialog : Form
    {

        public String[] NewDefaults;

        public String[] NewValidTagList;

        public String[] CustomFieldTypesCollection;

        public List<MetaDataFieldType> CustomFieldTypesSettings;


        public String MetaDataFilename;

        public bool ValidateTagsSetting;

        public bool TagValidationIgnoreCase;

        public int ConversionQualityValue;

        public bool OmitEmptyXMLTags;

        public int ConversionModeValue;

        public int MetaPageIndexWriteVersion;

        public bool DeleteTempFilesImediately;

        public bool SkipIndexCheck;

        public bool CalculateCrc32;

        public string InterpolationMode;

        public string TempPath;

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

            OmitEmptyXMLTags = Win_CBZSettings.Default.OmitEmptyXMLTags;

            DeleteTempFilesImediately = Win_CBZSettings.Default.AutoDeleteTempFiles;

            SkipIndexCheck = Win_CBZSettings.Default.SkipIndexCheck;

            CalculateCrc32 = Win_CBZSettings.Default.CalculateHash;

            InterpolationMode = Win_CBZSettings.Default.InterpolationMode;

            TempPath = Win_CBZSettings.Default.TempFolderPath;

            //CustomFieldTypesCollection = Win_CBZSettings.Default.CustomMetadataFields.OfType<String>().ToArray();

            CustomFieldTypesSettings = MetaDataFieldConfig.GetInstance().GetAllTypes();


            // ----------------------------------------


            ValidTags.Lines = NewValidTagList;
            CustomDefaultKeys.Lines = NewDefaults;

            CheckBoxValidateTags.Checked = ValidateTagsSetting;
            CheckBoxTagValidationIgnoreCase.Checked = !TagValidationIgnoreCase;

            ComboBoxPageIndexVersionWrite.SelectedIndex = MetaPageIndexWriteVersion - 1;
            ComboBoxConvertPages.SelectedIndex = ConversionModeValue;
            ImageQualityTrackBar.Value = ConversionQualityValue;

            CheckBoxPruneEmplyTags.Checked = OmitEmptyXMLTags;
            CheckBoxSkipIndexCheck.Checked = SkipIndexCheck;
            CheckBoxCalculateCrc.Checked = CalculateCrc32;

            ComboBoxFileName.Text = MetaDataFilename;

            MetaDataConfigTabControl.Dock = DockStyle.Fill;
            ImageProcessingTabControl.Dock = DockStyle.Fill;
            AppSettingsTabControl.Dock = DockStyle.Fill;
            CBZSettingsTabControl.Dock = DockStyle.Fill;

            MetaDataConfigTabControl.Visible = true;
            ImageProcessingTabControl.Visible = false;
            AppSettingsTabControl.Visible = false;
            CBZSettingsTabControl.Visible = false;

            CheckBoxDeleteTempFiles.Checked = DeleteTempFilesImediately;
            if (ComboBoxInterpolationModes.Items.IndexOf(InterpolationMode) > -1)
            {
                ComboBoxInterpolationModes.SelectedIndex = ComboBoxInterpolationModes.Items.IndexOf(InterpolationMode);
            } else
            {
                try {
                    ComboBoxInterpolationModes.SelectedIndex = int.Parse(InterpolationMode);
                } catch
                {
                    ComboBoxInterpolationModes.SelectedIndex = 0;
                }
                
            }
            

            TextBoxTempPath.Text = TempPath;

            CustomFieldsDataGrid.Columns.Add(new DataGridViewColumn()
            {
                DataPropertyName = "Name",
                HeaderText = "Key Name",
                CellTemplate = new DataGridViewTextBoxCell(),
                Width = 100,
                Frozen = true,
                SortMode = DataGridViewColumnSortMode.Automatic,
            });

            CustomFieldsDataGrid.Columns.Add(new DataGridViewColumn()
            {
                DataPropertyName = "Type",
                HeaderText = "Field Type",
                CellTemplate = new DataGridViewTextBoxCell(),
                Width = 120,
                SortMode = DataGridViewColumnSortMode.Automatic,
            });

            CustomFieldsDataGrid.Columns.Add(new DataGridViewColumn()
            {
                DataPropertyName = "Editor",
                HeaderText = "Editor",
                CellTemplate = new DataGridViewTextBoxCell(),
                Width = 120,
                SortMode = DataGridViewColumnSortMode.Automatic,
            });

            CustomFieldsDataGrid.Columns.Add(new DataGridViewColumn()
            {
                DataPropertyName = "Items",
                HeaderText = "Items/Autocomplete",
                CellTemplate = new DataGridViewTextBoxCell(),
                Width = 140,
                SortMode = DataGridViewColumnSortMode.Automatic,
            });

            CustomFieldsDataGrid.Columns.Add(new DataGridViewColumn()
            {
                DataPropertyName = "AutocompleteIcon",
                HeaderText = "Icontype",
                ToolTipText = "Set an icon to be displayed in autocomplete menus",
                CellTemplate = new DataGridViewTextBoxCell(),
                Width = 70,
                SortMode = DataGridViewColumnSortMode.Automatic,
            });

            CustomFieldsDataGrid.Columns.Add(new DataGridViewColumn()
            {
                DataPropertyName = "AutoAddNew",
                HeaderText = "[+]",
                ToolTipText = "Update this Itemlist with new user-input values automatically",
                CellTemplate = new DataGridViewCheckBoxCell(),
                Width = 30,
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

            PopulateFieldTypeEditor();

            validation = new DataValidation();

            DialogResult = DialogResult.Cancel;
        }

        private void PopulateFieldTypeEditor()
        {
            //CustomFieldTypesSettings.Clear();
            CustomFieldsDataGrid.Rows.Clear();
            foreach (MetaDataFieldType type in CustomFieldTypesSettings)
            {
                //String[] typeParts = line.Split('|');

                //if (typeParts.Length == 5)
                //{
                //CustomFieldTypesSettings.Add(new MetaDataFieldType(type.Name, type.FieldType, type.EditorType, type.Options, type.AutoUpdate));
                CustomFieldsDataGrid.Rows.Add(type.Name, type.FieldType, type.EditorType, type.Options, type.AutoCompleteImageKey, type.AutoUpdate);

                // }
            }


            // DataGridViewCellStyle currentStyle = null;

            for (int i = 0; i < CustomFieldsDataGrid.RowCount; i++)
            {
                CustomFieldsDataGrid.Rows[i].Cells[6].ReadOnly = true;

                foreach (MetaDataFieldType type in CustomFieldTypesSettings)
                {
                    var key = CustomFieldsDataGrid.Rows[i].Cells[0].Value;
                    if (key != null)
                    {
                        if (type.Name == key.ToString())
                        {
                            int selectedIndex = Array.IndexOf(MetaDataFieldType.FieldTypes, type.FieldType);
                            DataGridViewComboBoxCell cc = new DataGridViewComboBoxCell();
                            cc.Items.AddRange(MetaDataFieldType.FieldTypes);
                            cc.Value = type.FieldType; // selectedIndex > -1 ? selectedIndex : 0;
                            cc.Tag = MetaDataFieldType.MakeComboBoxField("", "");    //new EditorTypeConfig("ComboBox", "String", "", " ", false);
                            cc.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                            cc.DisplayStyleForCurrentCellOnly = false;
                            cc.Style = new DataGridViewCellStyle()
                            {
                                SelectionForeColor = Color.Black,
                                SelectionBackColor = Color.Gold,
                                BackColor = Color.White,
                            };

                            CustomFieldsDataGrid.Rows[i].Cells[1] = cc;

                            selectedIndex = Array.IndexOf(EditorTypeConfig.Editors, type.EditorType);
                            cc = new DataGridViewComboBoxCell();
                            cc.Items.AddRange(EditorTypeConfig.Editors);
                            cc.Value = type.EditorType; // selectedIndex > -1 ? selectedIndex : 0;
                            cc.Tag = MetaDataFieldType.MakeComboBoxField("", ""); //new EditorTypeConfig("ComboBox", "String", "", " ", false);
                            cc.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                            cc.DisplayStyleForCurrentCellOnly = false;
                            cc.Style = new DataGridViewCellStyle()
                            {
                                SelectionForeColor = Color.Black,
                                SelectionBackColor = Color.Gold,
                                BackColor = Color.White,
                            };

                            //c.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                            ///c.DisplayStyleForCurrentCellOnly = true;
                            //c.Style = new DataGridViewCellStyle()
                            //{
                            //    SelectionForeColor = Color.Black,
                            //    SelectionBackColor = ((i + 1) % 2 > 0) ? Color.White : Color.FromKnownColor(KnownColor.ControlLight),
                            //};

                            CustomFieldsDataGrid.Rows[i].Cells[2] = cc;

                            if (type.FieldType == MetaDataFieldType.METADATA_FIELD_TYPE_AUTO_COMPLETE)
                            {
                                DataGridViewComboBoxCell ci = new DataGridViewComboBoxCell();
                                int maxIndex = AutocompleteIcons.Images.Keys.Count - 1;
                                ci.Items.Add("");
                                foreach (string imgKey in AutocompleteIcons.Images.Keys)
                                {
                                    ci.Items.Add(imgKey.ToString());
                                }

                                ci.Value = type.AutoCompleteImageKey.ToString(); // selectedIndex > -1 ? selectedIndex : 0;
                                                                                 //ci.Tag = MetaDataFieldType.MakeComboBoxField("", "");    //new EditorTypeConfig("ComboBox", "String", "", " ", false);
                                ci.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                                ci.DisplayStyleForCurrentCellOnly = false;
                                ci.Style = new DataGridViewCellStyle()
                                {
                                    SelectionForeColor = Color.Black,
                                    SelectionBackColor = Color.Gold,
                                    BackColor = Color.White,
                                };

                                CustomFieldsDataGrid.Rows[i].Cells[4].ReadOnly = false;
                                CustomFieldsDataGrid.Rows[i].Cells[4] = ci;
                            }
                            else
                            {
                                CustomFieldsDataGrid.Rows[i].Cells[4].ReadOnly = true;
                                CustomFieldsDataGrid.Rows[i].Cells[4].Value = "";
                            }

                            DataGridViewCheckBoxCell cb = new DataGridViewCheckBoxCell();
                            cb.Value = type.AutoUpdate;
                            cb.Style = new DataGridViewCellStyle()
                            {
                                SelectionForeColor = Color.Black,
                                SelectionBackColor = Color.Gold,
                                BackColor = Color.White,
                            };



                            CustomFieldsDataGrid.Rows[i].Cells[5] = cb;

                            if (type.FieldType == MetaDataFieldType.METADATA_FIELD_TYPE_COMBO_BOX ||
                                type.FieldType == MetaDataFieldType.METADATA_FIELD_TYPE_AUTO_COMPLETE)
                            {
                                DataGridViewButtonCell bc = new DataGridViewButtonCell
                                {
                                    Value = "...",
                                    Tag = new EditorTypeConfig(EditorTypeConfig.EDITOR_TYPE_MULTI_LINE_TEXT_EDITOR, EditorTypeConfig.RESULT_TYPE_STRING, ",", "", false),
                                    Style = new DataGridViewCellStyle()
                                    {
                                        SelectionForeColor = Color.White,
                                        SelectionBackColor = Color.White,
                                    }
                                };

                                CustomFieldsDataGrid.Rows[i].Cells[6] = bc;
                            }
                        }
                    }
                }
            }
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

                    string defaultKeys = String.Join("", Program.ProjectModel.MetaData.GetDefaultEntries().Select(k => k.Key).ToArray());
                    if (!Regex.IsMatch(defaultKeys, @"^[a-z]+$", RegexOptions.IgnoreCase))
                    {
                        MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_WARNING, "Validateion Error! Default Metadata-Keys must contain only values between ['a-zA-Z']");

                        throw new MetaDataValidationException("", defaultKeys, "CustomDefaultKeys", "Validation Error! Default Metadata-Keys must contain only values between ['a-zA-Z']!", true, false);
                    }

                    if (CheckBoxValidateTags.Checked)
                    {
                        List<String> test = new List<String>(ValidTags.Lines);
                        String[] duplicateTags = validation.ValidateDuplicateStrings(test.ToArray());
                        if (duplicateTags.Length > 0)
                        {
                            //ApplicationMessage.ShowWarning("Validateion Error! Duplicate Tags [" + duplicateTags.Select(r => r + ", ") + "] not allowed!", "Validation Error", ApplicationMessage.DialogType.MT_WARNING, ApplicationMessage.DialogButtons.MB_OK);
                            MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_WARNING, "Validateion Error! Duplicate Tags [" + String.Join(",", duplicateTags) + "] not allowed!");

                            throw new MetaDataValidationException("", ValidTags.Text, "ValidTags", "Validation Error! Duplicate Tags [" + String.Join(",", duplicateTags) + "] not allowed!");
                        }
                    }

                    if (ComboBoxFileName.Text.Length == 0)
                    {
                        throw new MetaDataValidationException("", ComboBoxFileName.Text, "ComboBoxFileName", "Validation Error! Empty Metadata- Filename not allowed!");
                    }
                    else
                    {

                    }

                    int rowIndex = 0;
                    int checkRowÍndex = 0;
                    foreach (MetaDataFieldType t in CustomFieldTypesSettings)
                    {
                        if (t.Name.Length == 0)
                        {
                            throw new MetaDataValidationException("", t.Name, "CustomFieldsDataGrid." + rowIndex.ToString(), "Validation Error! Empty Metadata- Editor Key/Fieldname not allowed!");
                        }

                        if (!Regex.IsMatch(t.Name, @"^[a-z]+$", RegexOptions.IgnoreCase))
                        {
                            MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_WARNING, "Validateion Error! Default Metadata-Keys must contain only values between ['a-zA-Z']");

                            throw new MetaDataValidationException("", t.Name, "CustomFieldsDataGrid." + rowIndex.ToString(), "Validation Error! Metadata- Editor Key/Fieldname must contain only values between ['a-zA-Z']!", true, false);
                        }

                        int checkedRows = 0;
                        foreach (DataGridViewRow row in CustomFieldsDataGrid.Rows)
                        {
                            if (row.Cells[0].Value != null)
                            {
                                if (row.Cells[0].Value.ToString() == t.Name)
                                {
                                    checkedRows++;
                                }
                            }

                            if (checkedRows > 1)
                            {
                                throw new MetaDataValidationException("", t.Name, "CustomFieldsDataGrid." + checkRowÍndex.ToString(), "Validation Error! Duplicate Metadata- Editor Key/Fieldname not allowed!");
                            }

                            checkRowÍndex++;
                        }

                        if (t.Name.Length == 0)
                        {
                            throw new MetaDataValidationException("", t.Name, "CustomFieldsDataGrid." + checkRowÍndex.ToString(), "Validation Error! Empty Metadata- Editor Key/Fieldname not allowed!");
                        }

                        rowIndex++;
                    }

                    if (TextBoxTempPath.Text.Length == 0)
                    {
                        throw new MetaDataValidationException("", TextBoxTempPath.Text, "TextBoxTempPath", "Validation Error! Empty Temporary-Path not allowed!");
                    }

                    if (!System.IO.Directory.Exists(PathHelper.ResolvePath(TextBoxTempPath.Text)))
                    {
                        throw new MetaDataValidationException("", TextBoxTempPath.Text, "TextBoxTempPath", "Validation Error! Temporary-Path ['" + PathHelper.ResolvePath(TextBoxTempPath.Text) + "'] does not exist!");
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
                    OmitEmptyXMLTags = CheckBoxPruneEmplyTags.Checked;
                    DeleteTempFilesImediately = CheckBoxDeleteTempFiles.Checked;
                    SkipIndexCheck = CheckBoxSkipIndexCheck.Checked;
                    CalculateCrc32 = CheckBoxCalculateCrc.Checked;
                    InterpolationMode = ComboBoxInterpolationModes.SelectedItem.ToString();// ComboBoxInterpolationModes.SelectedIndex;
                    TempPath = TextBoxTempPath.Text;

                    List<String> fieldConfigItems = new List<string>();
                    foreach (MetaDataFieldType fieldTypeCnf in CustomFieldTypesSettings)
                    {
                        fieldConfigItems.Add(fieldTypeCnf.ToString());

                    }
                    CustomFieldTypesCollection = fieldConfigItems.ToArray();
                }
                catch (MetaDataValidationException mv)
                {
                    string controlName = mv.ControlName;
                    string row = null;

                    if (controlName.Contains("."))
                    {
                        string[] parts = controlName.Split('.');
                        controlName = parts[0];
                        row = parts[1];
                    }

                    SettingsValidationErrorProvider.SetError(this.Controls.Find(controlName, true)[0], mv.Message);
                    
                    if (row != null && row.Length > 0)
                    {
                        CustomFieldsDataGrid.Rows[int.Parse(row)].ErrorText = mv.Message;
                    }
                    
                    DialogResult = DialogResult.Cancel;
                    e.Cancel = true;
                    MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_ERROR, mv.Message);
                    ApplicationMessage.ShowWarning(mv.Message, "Validation Error", ApplicationMessage.DialogType.MT_WARNING, ApplicationMessage.DialogButtons.MB_OK);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (CustomDefaultKeys.Text.Length > 0)
            {
                DialogResult r = ApplicationMessage.ShowConfirmation("Are you sure you want to reset all Metadata -Keys to default?", "Please confirm", ApplicationMessage.DialogType.MT_CONFIRMATION, ApplicationMessage.DialogButtons.MB_YES | ApplicationMessage.DialogButtons.MB_NO);
                if (r == DialogResult.Yes)
                {
                    CustomDefaultKeys.Text = Program.ProjectModel.MetaData.GetDefaultKeys();
                }
            }
            else
            {
                CustomDefaultKeys.Text = Program.ProjectModel.MetaData.GetDefaultKeys();
            }
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
                    senderGrid.Rows[e.RowIndex].Cells[e.ColumnIndex] is DataGridViewComboBoxCell ||
                    senderGrid.Rows[e.RowIndex].Cells[e.ColumnIndex] is DataGridViewCheckBoxCell
                    ) &&
                    e.RowIndex >= 0)
                {
                    object value = null;
                    String valueText = "";
                    EditorTypeConfig editorConfig = senderGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag as EditorTypeConfig;
                    if (e.ColumnIndex == 6)
                    {
                        value = senderGrid.Rows[e.RowIndex].Cells[e.ColumnIndex - 3].Value;
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

                    if (senderGrid.Rows[e.RowIndex].Cells[e.ColumnIndex] is DataGridViewCheckBoxCell)
                    {
                        senderGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = !Boolean.Parse(valueText);
                    }

                    if (editorConfig != null)
                    {
                        editorConfig.Value = valueText;
                        switch (editorConfig.Type)
                        {
                            case EditorTypeConfig.EDITOR_TYPE_MULTI_LINE_TEXT_EDITOR:
                                {
                                    TextEditorForm textEditor = new TextEditorForm(editorConfig);
                                    DialogResult r = textEditor.ShowDialog();
                                    if (r == DialogResult.OK)
                                    {
                                        if (textEditor.Config.Result != null)
                                        {
                                            senderGrid.Rows[e.RowIndex].Cells[e.ColumnIndex - 3].Value = textEditor.Config.Result.ToString();
                                        }
                                    }
                                }
                                break;
                            case EditorTypeConfig.EDITOR_TYPE_LANGUAGE_EDITOR:
                                {
                                    LanguageEditorForm langEditor = new LanguageEditorForm(editorConfig);
                                    DialogResult r = langEditor.ShowDialog();
                                    if (r == DialogResult.OK)
                                    {
                                        if (langEditor.config.Result != null)
                                        {
                                            senderGrid.Rows[e.RowIndex].Cells[e.ColumnIndex - 3].Value = langEditor.config.Result.ToString();
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
            //NewIndex = NewIndex + i;

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
                    cc.Tag = new EditorConfig("ComboBox", "String", "", " ", false);

                    CustomFieldsDataGrid.Rows[e.RowIndex].Cells[1] = cc;

                    selectedIndex = -1;
                    cc = new DataGridViewComboBoxCell();
                    cc.Items.AddRange(EditorConfig.Editors);
                    cc.Value = updatedEntry.EditorType; // selectedIndex > -1 ? selectedIndex : 0;
                    cc.Tag = new EditorConfig("ComboBox", "String", "", " ", false);

                    CustomFieldsDataGrid.Rows[e.RowIndex].Cells[2] = cc;

                    if (updatedEntry.FieldType == EditorFieldMapping.METADATA_FIELD_TYPE_COMBO_BOX)
                    {
                        DataGridViewButtonCell bc = new DataGridViewButtonCell
                        {
                            Value = "...",
                            Tag = new EditorConfig("MultiLineTextEditor", "String", ",", " ", false),
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
                bool AutoUpdate = false;
                string AutoCompleteImageKey = null;
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

                    value = CustomFieldsDataGrid.Rows[e.RowIndex].Cells[e.ColumnIndex + 4].Value;
                    if (value == null || (string)value == "")
                    {
                        value = "";
                    }

                    AutoCompleteImageKey = value.ToString();

                    value = CustomFieldsDataGrid.Rows[e.RowIndex].Cells[e.ColumnIndex + 5].Value;
                    if (value == null)
                    {
                        value = "False";
                    }

                    AutoUpdate = Boolean.Parse(value.ToString());
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

                    value = CustomFieldsDataGrid.Rows[e.RowIndex].Cells[e.ColumnIndex + 3].Value;
                    if (value == null || (string)value == "")
                    {
                        value = "";
                    }

                    AutoCompleteImageKey = value.ToString();

                    value = CustomFieldsDataGrid.Rows[e.RowIndex].Cells[e.ColumnIndex + 4].Value;
                    if (value == null)
                    {
                        value = "False";
                    }

                    AutoUpdate = Boolean.Parse(value.ToString());
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

                    value = CustomFieldsDataGrid.Rows[e.RowIndex].Cells[e.ColumnIndex + 2].Value;
                    if (value == null || (string)value == "")
                    {
                        value = "";
                    }

                    AutoCompleteImageKey = value.ToString();

                    value = CustomFieldsDataGrid.Rows[e.RowIndex].Cells[e.ColumnIndex + 3].Value;
                    if (value == null)
                    {
                        value = "False";
                    }

                    AutoUpdate = Boolean.Parse(value.ToString());
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

                    EditorType = value.ToString();

                    value = CustomFieldsDataGrid.Rows[e.RowIndex].Cells[e.ColumnIndex + 1].Value;
                    if (value == null || (string)value == "")
                    {
                        value = "";
                    }

                    AutoCompleteImageKey = value.ToString();

                    value = CustomFieldsDataGrid.Rows[e.RowIndex].Cells[e.ColumnIndex + 2].Value;
                    if (value == null)
                    {
                        value = "False";
                    }

                    AutoUpdate = Boolean.Parse(value.ToString());
                }

                if (e.ColumnIndex == 4)
                {
                    if (value == null || (string)value == "")
                    {
                        value = "";
                    }

                    AutoCompleteImageKey = value.ToString();

                    value = CustomFieldsDataGrid.Rows[e.RowIndex].Cells[e.ColumnIndex - 4].Value;
                    if (value == null)
                    {
                        value = "";
                    }

                    Key = value.ToString();

                    value = CustomFieldsDataGrid.Rows[e.RowIndex].Cells[e.ColumnIndex - 3].Value;
                    if (value == null)
                    {
                        value = "";
                    }

                    FieldType = value.ToString();

                    value = CustomFieldsDataGrid.Rows[e.RowIndex].Cells[e.ColumnIndex - 2].Value;
                    if (value == null)
                    {
                        value = "";
                    }

                    EditorType = value.ToString();

                    value = CustomFieldsDataGrid.Rows[e.RowIndex].Cells[e.ColumnIndex - 1].Value;
                    if (value == null)
                    {
                        value = "";
                    }

                    Val = value.ToString();

                    value = CustomFieldsDataGrid.Rows[e.RowIndex].Cells[e.ColumnIndex + 1].Value;
                    if (value == null)
                    {
                        value = "False";
                    }

                    AutoUpdate = Boolean.Parse(value.ToString());
                }

                if (e.ColumnIndex == 5)
                {
                    if (value == null)
                    {
                        value = "False";
                    }

                    AutoUpdate = Boolean.Parse(value.ToString());

                    value = CustomFieldsDataGrid.Rows[e.RowIndex].Cells[e.ColumnIndex - 5].Value;
                    if (value == null)
                    {
                        value = "";
                    }

                    Key = value.ToString();

                    value = CustomFieldsDataGrid.Rows[e.RowIndex].Cells[e.ColumnIndex - 4].Value;
                    if (value == null)
                    {
                        value = "";
                    }

                    FieldType = value.ToString();

                    value = CustomFieldsDataGrid.Rows[e.RowIndex].Cells[e.ColumnIndex - 3].Value;
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

                    Val = value.ToString();

                    value = CustomFieldsDataGrid.Rows[e.RowIndex].Cells[e.ColumnIndex - 1].Value;
                    if (value == null || (string)value == "")
                    {
                        value = "-1";
                    }

                    AutoCompleteImageKey = value.ToString();
                }

                MetaDataFieldType updatedEntry = CustomFieldTypesSettings[e.RowIndex];
                CustomFieldsDataGrid.Rows[e.RowIndex].ErrorText = null;
                CustomFieldsDataGrid.Invalidate();


                if (updatedEntry != null)
                {
                    CustomFieldsDataGrid.Rows[e.RowIndex].Cells[6].ReadOnly = true;
                    updatedEntry.Options = Val;
                    updatedEntry.FieldType = FieldType;
                    updatedEntry.Name = Key;
                    updatedEntry.EditorType = EditorType;
                    updatedEntry.AutoUpdate = AutoUpdate;
                    updatedEntry.AutoCompleteImageKey = AutoCompleteImageKey;

                    int selectedIndex = Array.IndexOf(MetaDataFieldType.FieldTypes, FieldType);
                    DataGridViewComboBoxCell cc = new DataGridViewComboBoxCell();
                    cc.Items.AddRange(MetaDataFieldType.FieldTypes);
                    cc.Value = FieldType; // selectedIndex > -1 ? selectedIndex : 0;
                    cc.Tag = MetaDataFieldType.MakeComboBoxField("", ""); // new EditorTypeConfig("ComboBox", "String", "", " ", false);
                    cc.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                    cc.DisplayStyleForCurrentCellOnly = false;
                    cc.Style = new DataGridViewCellStyle()
                    {
                        SelectionForeColor = Color.Black,
                        SelectionBackColor = Color.Gold,
                        BackColor = Color.White,
                    };

                    CustomFieldsDataGrid.Rows[e.RowIndex].Cells[1] = cc;

                    selectedIndex = Array.IndexOf(EditorTypeConfig.Editors, EditorType);
                    cc = new DataGridViewComboBoxCell();
                    cc.Items.AddRange(EditorTypeConfig.Editors);
                    cc.Value = EditorType; // selectedIndex > -1 ? selectedIndex : 0;
                    cc.Tag = MetaDataFieldType.MakeComboBoxField("", ""); //updatedEntry; // new EditorTypeConfig("ComboBox", "String", "", " ", false);
                    cc.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                    cc.DisplayStyleForCurrentCellOnly = false;
                    cc.Style = new DataGridViewCellStyle()
                    {
                        SelectionForeColor = Color.Black,
                        SelectionBackColor = Color.Gold,
                        BackColor = Color.White,
                    };

                    CustomFieldsDataGrid.Rows[e.RowIndex].Cells[2] = cc;

                    if (updatedEntry.FieldType == MetaDataFieldType.METADATA_FIELD_TYPE_AUTO_COMPLETE)
                    {
                        DataGridViewComboBoxCell ci = new DataGridViewComboBoxCell();
                        int maxIndex = AutocompleteIcons.Images.Keys.Count - 1;
                        ci.Items.Add("");
                        foreach (string key in AutocompleteIcons.Images.Keys)
                        {
                            ci.Items.Add(key.ToString());
                        }
                        ci.Value = updatedEntry.AutoCompleteImageKey; // selectedIndex > -1 ? selectedIndex : 0;
                                                                      //ci.Tag = MetaDataFieldType.MakeComboBoxField("", "");    //new EditorTypeConfig("ComboBox", "String", "", " ", false);
                        ci.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                        ci.DisplayStyleForCurrentCellOnly = false;
                        ci.Style = new DataGridViewCellStyle()
                        {
                            SelectionForeColor = Color.Black,
                            SelectionBackColor = Color.Gold,
                            BackColor = Color.White,
                        };

                        CustomFieldsDataGrid.Rows[e.RowIndex].Cells[4].ReadOnly = false;
                        CustomFieldsDataGrid.Rows[e.RowIndex].Cells[4] = ci;
                    }
                    else
                    {
                        CustomFieldsDataGrid.Rows[e.RowIndex].Cells[4].ReadOnly = true;
                        CustomFieldsDataGrid.Rows[e.RowIndex].Cells[4].Value = "";
                    }

                    if (updatedEntry.FieldType == MetaDataFieldType.METADATA_FIELD_TYPE_COMBO_BOX ||
                        updatedEntry.FieldType == MetaDataFieldType.METADATA_FIELD_TYPE_AUTO_COMPLETE)
                    {
                        DataGridViewButtonCell bc = new DataGridViewButtonCell
                        {
                            Value = "...",
                            Tag = new EditorTypeConfig(EditorTypeConfig.EDITOR_TYPE_MULTI_LINE_TEXT_EDITOR, EditorTypeConfig.RESULT_TYPE_STRING, ",", " ", false)
                        };
                        CustomFieldsDataGrid.Rows[e.RowIndex].Cells[6] = bc;
                    }

                }

                if (updatedEntry.EditorType == EditorTypeConfig.EDITOR_TYPE_MULTI_LINE_TEXT_EDITOR)
                {

                }
                else if (updatedEntry.EditorType == EditorTypeConfig.EDITOR_TYPE_LANGUAGE_EDITOR)
                {
                    //DataGridViewButtonCell bc = new DataGridViewButtonCell
                    //{
                    //    Value = "...",
                    //    Tag = new EditorConfig(EditorConfig.EDITOR_TYPE_LANGUAGE_EDITOR, "String", "", "", false)
                    //};
                    //CustomFieldsDataGrid.Rows[e.RowIndex].Cells[4] = bc;

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
            catch (Exception ex)
            {
                MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_ERROR, ex.Message);
            }
        }

        private void AddFieldTypeButton_Click(object sender, EventArgs e)
        {
            int newIndex = CustomFieldsDataGrid.Rows.Add("", "Text", "", "", -1, false, "");
            MetaDataFieldType newEntry = new MetaDataFieldType("", "Text", "", "", false);
            CustomFieldTypesSettings.Add(newEntry);

            CustomFieldsDataGrid.Rows[newIndex].Cells[6].ReadOnly = true;

            DataGridViewComboBoxCell cc = new DataGridViewComboBoxCell();
            cc.Items.AddRange(MetaDataFieldType.FieldTypes);
            cc.Value = newEntry.FieldType; // selectedIndex > -1 ? selectedIndex : 0;
            cc.Tag = new MetaDataFieldType("", MetaDataFieldType.METADATA_FIELD_TYPE_COMBO_BOX); // EditorTypeConfig(, "String", "", " ", false);
            cc.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
            cc.DisplayStyleForCurrentCellOnly = false;
            cc.Style = new DataGridViewCellStyle()
            {
                SelectionForeColor = Color.Black,
                SelectionBackColor = Color.Gold,
                BackColor = Color.White,
            };

            CustomFieldsDataGrid.Rows[newIndex].Cells[1] = cc;

            cc = new DataGridViewComboBoxCell();
            cc.Items.AddRange(EditorTypeConfig.Editors);
            cc.Value = newEntry.EditorType; // selectedIndex > -1 ? selectedIndex : 0;
            cc.Tag = new EditorTypeConfig("ComboBox", "String", "", " ", false);
            cc.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
            cc.DisplayStyleForCurrentCellOnly = false;
            cc.Style = new DataGridViewCellStyle()
            {
                SelectionForeColor = Color.Black,
                SelectionBackColor = Color.Gold,
                BackColor = Color.White,
            };

            CustomFieldsDataGrid.Rows[newIndex].Cells[2] = cc;

            if (newEntry.FieldType == MetaDataFieldType.METADATA_FIELD_TYPE_AUTO_COMPLETE)
            {
                DataGridViewComboBoxCell ci = new DataGridViewComboBoxCell();
                int maxIndex = AutocompleteIcons.Images.Keys.Count - 1;
                ci.Items.Add("");
                foreach (string key in AutocompleteIcons.Images.Keys)
                {
                    ci.Items.Add(key.ToString());
                }
                ci.Value = newEntry.AutoCompleteImageKey; // selectedIndex > -1 ? selectedIndex : 0;
                                                          //ci.Tag = MetaDataFieldType.MakeComboBoxField("", "");    //new EditorTypeConfig("ComboBox", "String", "", " ", false);
                ci.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                ci.DisplayStyleForCurrentCellOnly = false;
                ci.Style = new DataGridViewCellStyle()
                {
                    SelectionForeColor = Color.Black,
                    SelectionBackColor = Color.Gold,
                    BackColor = Color.White,
                };

                CustomFieldsDataGrid.Rows[newIndex].Cells[4].ReadOnly = false;
                CustomFieldsDataGrid.Rows[newIndex].Cells[4] = ci;
            }
            else
            {
                CustomFieldsDataGrid.Rows[newIndex].Cells[4].ReadOnly = true;
                CustomFieldsDataGrid.Rows[newIndex].Cells[4].Value = "";
            }


            DataGridViewCheckBoxCell cb = new DataGridViewCheckBoxCell();
            cb.Value = newEntry.AutoUpdate;
            cb.Style = new DataGridViewCellStyle()
            {
                SelectionForeColor = Color.Black,
                SelectionBackColor = Color.Gold,
                BackColor = Color.White,
            };

            CustomFieldsDataGrid.Rows[newIndex].Cells[5] = cb;

            if (newEntry.FieldType == MetaDataFieldType.METADATA_FIELD_TYPE_COMBO_BOX ||
                newEntry.FieldType == MetaDataFieldType.METADATA_FIELD_TYPE_AUTO_COMPLETE)
            {
                DataGridViewButtonCell bc = new DataGridViewButtonCell
                {
                    Value = "...",
                    Tag = new EditorTypeConfig(EditorTypeConfig.EDITOR_TYPE_MULTI_LINE_TEXT_EDITOR, EditorTypeConfig.RESULT_TYPE_STRING, ",", " ", false),
                    Style = new DataGridViewCellStyle()
                    {
                        SelectionForeColor = Color.White,
                        SelectionBackColor = Color.White,
                    }
                };

                CustomFieldsDataGrid.Rows[newIndex].Cells[6] = bc;
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

        private void RestoreFieldTypesButton_Click(object sender, EventArgs e)
        {
            DialogResult r = ApplicationMessage.ShowConfirmation("Are you sure you want to reset all MetaData -Editor FieldTypes to their default configuration?", "Please confirm", ApplicationMessage.DialogType.MT_CONFIRMATION, ApplicationMessage.DialogButtons.MB_YES | ApplicationMessage.DialogButtons.MB_NO);
            if (r == DialogResult.Yes)
            {
                CustomFieldTypesSettings.Clear();
                foreach (string row in FactoryDefaults.DefaultMetaDataFieldTypes)
                {

                    String[] typeParts = row.Split('|');

                    if (typeParts.Length >= 5)
                    {
                        try
                        {
                            CustomFieldTypesSettings.Add(new MetaDataFieldType(
                                typeParts[0],
                                typeParts[1],
                                typeParts[2],
                                typeParts[3],
                                bool.Parse(typeParts[4].ToLower()),
                                typeParts.Length > 5 ? typeParts[5] : ""
                            ));
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }


                PopulateFieldTypeEditor();
            }

        }

        private void SettingsDialog_Shown(object sender, EventArgs e)
        {
            RestoreFieldTypesButton.Size = new System.Drawing.Size(80, 30);
            SettingsSectionList.SelectedIndex = 0;
        }

        private void ToolStripTextBoxSearchTag_KeyUp(object sender, KeyEventArgs e)
        {
            String itemsText = ValidTags.Text;

            int occurence = itemsText.IndexOf(ToolStripTextBoxSearchTag.Text, 0, StringComparison.CurrentCultureIgnoreCase);

            ValidTags.SelectionStart = 0;
            ValidTags.SelectionLength = 0;
            if (occurence > -1)
            {
                ValidTags.SelectionStart = occurence;
                ValidTags.SelectionLength = ToolStripTextBoxSearchTag.Text.Length;
                ValidTags.ScrollToCaret();
            }
        }

        private void ButtonSelectFolder_Click(object sender, EventArgs e)
        {
            OpenTargetDirectory.InitialDirectory = PathHelper.ResolvePath(TextBoxTempPath.Text);
            if (OpenTargetDirectory.ShowDialog() == DialogResult.OK)
            {
                LocalFile localFile = new LocalFile(OpenTargetDirectory.FileName);

                TextBoxTempPath.Text = localFile.FilePath;
            }
        }
    }
}
