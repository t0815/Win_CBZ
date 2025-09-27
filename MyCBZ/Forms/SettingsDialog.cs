
using LoadingIndicator.WinForms;
using SharpCompress.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media;
using Win_CBZ.Data;
using Win_CBZ.Events;
using Win_CBZ.Extensions;
using Win_CBZ.Helper;
using Win_CBZ.Models;
using Color = System.Drawing.Color;
using Pen = System.Drawing.Pen;

namespace Win_CBZ.Forms
{
    [SupportedOSPlatform("windows")]
    internal partial class SettingsDialog : Form
    {

        public String[] NewDefaults;

        public String[] NewValidTagList;

        public String[] CustomFieldTypesCollection;

        public List<String> ImageFileExtensions;

        public List<String> FilteredFileNames;

        public List<MetaDataFieldType> CustomFieldTypesSettings;


        public String MetaDataFilename;

        public bool ValidateTagsSetting;

        public bool TagValidationIgnoreCase;

        public int ConversionQualityValue;

        public bool WriteXMLPageIndex;

        public bool OmitEmptyXMLTags;

        public int ConversionModeValue;

        public int MetaPageIndexWriteVersion;

        public bool DeleteTempFilesImediately;

        public bool SkipIndexCheck;

        public bool DetectDoublePages;

        public bool CalculateCrc32;

        public string InterpolationMode;

        public string TempPath;

        public bool FilterNewPagesByExt;

        public bool FilterNewPagesSpecificName;

        public bool MetadataGridEditMode;

        public bool MetadataGridEditModeValueCol;

        public int CompressionLevel;

        public bool CompatibilityMode;

        public bool IgnoreErrors;

        public bool RestoreWindowPosition;

        public bool JumpToSelectedPage;

        public bool LogValidationErrors;

        public bool SkipArchiveSubfolders;

        public bool AutoUpdateCheck;

        public int AutoUpdateCheckIntervalType;

        // Appearance settings

        public string AccentColor = Colors.COLOR_GOLD;

        public string ButtonColor = "";

        public string TextColor = "";

        public string WindowBackgroundColor = "";

        public string ListBackgroundColor = "";

        // --------------------

        DataValidation validation;

        private int lastSearchOccurence = 0;

        private int occurence = 0;

        private int nextOccurence = 0;

        private int LastSelectedThemeColorIndex = 0;

        private List<string> errorCategories = new List<string>();

        private String DefaultAccentColor = Colors.COLOR_GOLD;

        private String[] ExampleAccentColors = new string[] { "#FFFFFF" };

        private List<ThemeColorMapping> ThemeColorMappings = new List<ThemeColorMapping>()
        {
            new ThemeColorMapping()
            {
                ColorName = Theme.COLOR_NAME_ACCENT,
                Label = "Accent Color",
                ColorValue = Colors.COLOR_GOLD,
                Description = "Main accent color used for buttons, highlights and selection.",
                DefaultValue = Colors.COLOR_GOLD,
                Category = "Main",
                ExampleValues = new List<string>() { Colors.COLOR_CRYOLA, Colors.COLOR_TANGERINE, Colors.COLOR_NEON_GREEN, Colors.COLOR_MANGO, Colors.COLOR_GRAPE_LIGHT_PINK }
            },
            new ThemeColorMapping()
            {
                ColorName = Theme.COLOR_NAME_TEXT,
                Label = "Text Color",
                ColorValue = HTMLColor.ToHexColor(SystemColors.ControlText),
                Description = "Standard text color on all surfaces",
                DefaultValue = HTMLColor.ToHexColor(SystemColors.ControlText),
                Category = "Main",
                ExampleValues = new List<string>() { HTMLColor.ToHexColor(SystemColors.ControlText), Colors.COLOR_DARK_GRAY_TEXT_COLOR }
            },
            new ThemeColorMapping()
            {
                ColorName = "ButtonColor",
                Label = "Button Color",
                ColorValue = HTMLColor.ToHexColor(SystemColors.Control),
                Description = "Main background color used for buttons.",
                DefaultValue = HTMLColor.ToHexColor(SystemColors.Control),
                Category = "Main",
                ExampleValues = new List<string>() { HTMLColor.ToHexColor(SystemColors.Control), Colors.COLOR_DARK_GRAY_BUTTON_FACE }
            },
            new ThemeColorMapping()
            {
                ColorName = "WindowBackgroundColor",
                Label = "Window Background Color",
                ColorValue = HTMLColor.ToHexColor(SystemColors.Control),
                Description = "Background color used for windows.",
                DefaultValue = HTMLColor.ToHexColor(SystemColors.Control),
                Category = "Main",
                ExampleValues = new List<string>() { HTMLColor.ToHexColor(SystemColors.Control), Colors.COLOR_DARK_GRAY_WINDOW_BG }
            },
            new ThemeColorMapping()
            {
                ColorName = Theme.COLOR_NAME_LIST_BACKGROUND,
                Label = "Lists Background Color",
                ColorValue = HTMLColor.ToHexColor(SystemColors.Window),
                Description = "Background color used for Listviews, Listboxes and Comboboxes.",
                DefaultValue = HTMLColor.ToHexColor(SystemColors.Window),
                Category = "Main",
                ExampleValues = new List<string>() { HTMLColor.ToHexColor(SystemColors.Window), Colors.COLOR_DARK_GRAY_LIST_CONTROL_BG }
            },
        };

        public SettingsDialog()
        {
            InitializeComponent();

            MetaDataConfigTabControl.Dock = DockStyle.Fill;
            ImageProcessingTabControl.Dock = DockStyle.Fill;
            AppSettingsTabControl.Dock = DockStyle.Fill;
            CBZSettingsTabControl.Dock = DockStyle.Fill;
            UpdatesTabControl.Dock = DockStyle.Fill;
            AppearanceTabControl.Dock = DockStyle.Fill;

            MetaDataConfigTabControl.Visible = true;
            ImageProcessingTabControl.Visible = false;
            AppSettingsTabControl.Visible = false;
            CBZSettingsTabControl.Visible = false;
            UpdatesTabControl.Visible = false;
            AppearanceTabControl.Visible = false;


            

            // Load settings  ------------------------

            CustomFieldTypesSettings = new List<MetaDataFieldType>();
            ImageFileExtensions = new List<String>();
            FilteredFileNames = new List<String>();

            if (Win_CBZSettings.Default.CustomDefaultProperties != null)
            {
                NewDefaults = Win_CBZSettings.Default.CustomDefaultProperties.OfType<String>().ToArray();
            }

            if (Win_CBZSettings.Default.ImageExtenstionList != null)
            {
                string[] exts = Win_CBZSettings.Default.ImageExtenstionList.Split('|').Where(s => s.Length > 0).ToArray<string>();
                ImageFileExtensions.AddRange(exts);
            }

            if (Win_CBZSettings.Default.FilteredFilenamesList != null)
            {
                string[] exts = Win_CBZSettings.Default.FilteredFilenamesList.Split('|').Where(s => s.Length > 0).ToArray<string>();
                FilteredFileNames.AddRange(exts);
            }

            if (Win_CBZSettings.Default.ValidKnownTags != null)
            {
                NewValidTagList = Win_CBZSettings.Default.ValidKnownTags.OfType<String>().ToArray();
            }

            AutoUpdateCheck = Win_CBZSettings.Default.AutoUpdate;
            AutoUpdateCheckIntervalType = Win_CBZSettings.Default.AutoupdateType;

            ConversionQualityValue = Win_CBZSettings.Default.ImageConversionQuality;
            ConversionModeValue = Win_CBZSettings.Default.ImageConversionMode;

            ValidateTagsSetting = Win_CBZSettings.Default.ValidateTags;
            TagValidationIgnoreCase = Win_CBZSettings.Default.TagValidationIgnoreCase;

            MetaPageIndexWriteVersion = Win_CBZSettings.Default.MetaDataPageIndexVersionToWrite;

            MetaDataFilename = Win_CBZSettings.Default.MetaDataFilename;

            OmitEmptyXMLTags = Win_CBZSettings.Default.OmitEmptyXMLTags;

            DetectDoublePages = Win_CBZSettings.Default.DetectDoublePages;

            DeleteTempFilesImediately = Win_CBZSettings.Default.AutoDeleteTempFiles;
            SkipIndexCheck = Win_CBZSettings.Default.SkipIndexCheck;
            CalculateCrc32 = Win_CBZSettings.Default.CalculateHash;

            InterpolationMode = Win_CBZSettings.Default.InterpolationMode;
            WriteXMLPageIndex = Win_CBZSettings.Default.WriteXmlPageIndex;

            MetadataGridEditMode = Win_CBZSettings.Default.MetadataGridInstantEditMode;
            MetadataGridEditModeValueCol = Win_CBZSettings.Default.MetadataGridInstantEditModeValueCol;

            TempPath = Win_CBZSettings.Default.TempFolderPath;
            FilterNewPagesByExt = Win_CBZSettings.Default.FilterByExtension;
            FilterNewPagesSpecificName = Win_CBZSettings.Default.FilterSpecificFilenames;

            CompressionLevel = Win_CBZSettings.Default.CompressionLevel;
            CompatibilityMode = Win_CBZSettings.Default.CompatMode;
            IgnoreErrors = Win_CBZSettings.Default.IgnoreErrorsOnSave;

            SkipArchiveSubfolders = Win_CBZSettings.Default.SkipFilesInSubDirectories;

            JumpToSelectedPage = Win_CBZSettings.Default.JumpToPage;

            RestoreWindowPosition = Win_CBZSettings.Default.RestoreWindowLayout;
            LogValidationErrors = Win_CBZSettings.Default.LogValidationErrors;

            AccentColor = Win_CBZSettings.Default.AccentColor;
            ButtonColor = Win_CBZSettings.Default.ButtonColor;
            ListBackgroundColor = Win_CBZSettings.Default.ListBackgroundColor;
            TextColor = Win_CBZSettings.Default.TextColor;

            //CustomFieldTypesCollection = Win_CBZSettings.Default.CustomMetadataFields.OfType<String>().ToArray();

            CustomFieldTypesSettings = MetaDataFieldConfig.GetInstance().GetAllTypes();

            if (WriteXMLPageIndex == false)
            {
                CompatibilityMode = true;
                CheckBoxCompatibilityMode.Enabled = false;
            }

            //----------------------------------------

            foreach (ThemeColorMapping mapping in ThemeColorMappings)
            {
                switch (mapping.ColorName)
                {
                    case Theme.COLOR_NAME_ACCENT:
                        mapping.ColorValue = Win_CBZSettings.Default.AccentColor;
                        break;

                    case "ButtonColor":
                        mapping.ColorValue = Win_CBZSettings.Default.ButtonColor;
                        break;

                    case Theme.COLOR_NAME_LIST_BACKGROUND:
                        mapping.ColorValue = Win_CBZSettings.Default.ListBackgroundColor;
                        break;

                    case Theme.COLOR_NAME_TEXT:
                        mapping.ColorValue = Win_CBZSettings.Default.TextColor;
                        break;
                }

            }


            ApplyTheme(SettingsTablePanel.Controls);
            ApplyTheme(MetadataDefaultsTable.Controls);
            ApplyTheme(EssentialTableLayoutPanel.Controls);

            ThemeColorsListbox.Items.Clear();

            foreach (ThemeColorMapping mapping in ThemeColorMappings)
            {
                ThemeColorsListbox.Items.Add(mapping);
            }

            // ----------------------------------------

            CheckboxAutoUpdate.Checked = AutoUpdateCheck;
            ComboBoxAutoUpdateInterval.SelectedIndex = AutoUpdateCheckIntervalType;

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
            CheckBoxJumpToPage.Checked = JumpToSelectedPage;

            CheckboxDetectDoublePages.Checked = DetectDoublePages;

            ComboBoxFileName.Text = MetaDataFilename;
            CheckboxAlwaysInEditMode.Checked = MetadataGridEditMode;
            CheckBoxEditModeOnlyValueCol.Checked = MetadataGridEditModeValueCol;
            CheckBoxWriteIndex.Checked = WriteXMLPageIndex;

            ComboBoxCompressionLevel.SelectedIndex = CompressionLevel;
            CheckBoxCompatibilityMode.Checked = CompatibilityMode;
            CheckBoxIgnoreErrorsOnSave.Checked = IgnoreErrors;

            CheckBoxSkipSubfolders.Checked = SkipArchiveSubfolders;

            CheckBoxSaveWindowLayout.Checked = RestoreWindowPosition;
            CheckBoxLogValidationErrors.Checked = LogValidationErrors;



            FilterNewPagesByExtCheckBox.Checked = FilterNewPagesByExt;
            CheckboxFilterFilenames.Checked = FilterNewPagesSpecificName;

            CheckBoxDeleteTempFiles.Checked = DeleteTempFilesImediately;

            if (ComboBoxInterpolationModes.Items.IndexOf(InterpolationMode) > -1)
            {
                ComboBoxInterpolationModes.SelectedIndex = ComboBoxInterpolationModes.Items.IndexOf(InterpolationMode);
            }
            else
            {
                try
                {
                    ComboBoxInterpolationModes.SelectedIndex = int.Parse(InterpolationMode);
                }
                catch
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
                DataPropertyName = "MultiValued",
                HeaderText = "Multi- valued",
                ToolTipText = "Indicates, this field can have multiple values",
                CellTemplate = new DataGridViewCheckBoxCell(),
                Width = 30,
                SortMode = DataGridViewColumnSortMode.Automatic,
            });

            CustomFieldsDataGrid.Columns.Add(new DataGridViewColumn()
            {
                DataPropertyName = "ValueSeparator",
                HeaderText = "Value Separator",
                ToolTipText = "Set separator for multivalued field",
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

        private void ApplyTheme(TableLayoutControlCollection container)
        {
            //Theme theme = Theme.GetInstance();

            CustomFieldsDataGrid.DefaultCellStyle.SelectionBackColor = HTMLColor.ToColor(AccentColor);
            CustomFieldsDataGrid.RowsDefaultCellStyle.SelectionBackColor = HTMLColor.ToColor(AccentColor);

            container.OfType<Button>().ToList().ForEach(b =>
            {
                b.BackColor = HTMLColor.ToColor(ButtonColor);
                //b.FlatAppearance.MouseOverBackColor = HTMLColor.AdjustBrightness(HTMLColor.ToColor(ButtonColor), 0.8f);
                //b.FlatAppearance.MouseDownBackColor = HTMLColor.AdjustBrightness(HTMLColor.ToColor(ButtonColor), 0.6f);
            });

            container.OfType<Label>().Each<Label>(l =>
            {
                l.ForeColor = HTMLColor.ToColor(TextColor);
            });

            //Control.ControlCollection controls = container.OfType<Control>().SelectMany(c => c.Controls.OfType<Control>());

            ThemeColorsListbox.BackColor = HTMLColor.ToColor(ListBackgroundColor);
            SettingsSectionList.BackColor = HTMLColor.ToColor(ListBackgroundColor);

            SettingsSectionList.Invalidate();
        }

        private void ApplyThemeContainer(Container container)
        {
            container.Components.OfType<Button>().ToList().ForEach(b =>
            {
                b.BackColor = HTMLColor.ToColor(ButtonColor);
                //b.FlatAppearance.MouseOverBackColor = HTMLColor.AdjustBrightness(HTMLColor.ToColor(ButtonColor), 0.8f);
                //b.FlatAppearance.MouseDownBackColor = HTMLColor.AdjustBrightness(HTMLColor.ToColor(ButtonColor), 0.6f);
            });

            container.Components.OfType<Label>().Each<Label>(l =>
            {
                l.ForeColor = HTMLColor.ToColor(TextColor);
            });
        }

        private void PopulateFieldTypeEditor()
        {

            CustomFieldsDataGrid.Rows.Clear();
            foreach (MetaDataFieldType type in CustomFieldTypesSettings)
            {

                CustomFieldsDataGrid.Rows.Add(type.Name, type.FieldType, type.EditorType, type.Options, type.AutoCompleteImageKey, type.MultiValued, type.MultiValueSeparator, type.AutoUpdate);
            }


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
                                SelectionBackColor = Theme.GetInstance().AccentColor,
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
                                SelectionBackColor = Theme.GetInstance().AccentColor,
                                BackColor = Color.White,
                            };

                            CustomFieldsDataGrid.Rows[i].Cells[2] = cc;

                            if (type.FieldType == MetaDataFieldType.METADATA_FIELD_TYPE_AUTO_COMPLETE ||
                                type.FieldType == MetaDataFieldType.METADATA_FIELD_TYPE_COMBO_BOX)
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
                                    SelectionBackColor = Theme.GetInstance().AccentColor,
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
                            cb.Value = type.MultiValued;

                            if (type.FieldType == MetaDataFieldType.METADATA_FIELD_TYPE_COMBO_BOX)
                            {
                                cb.Value = false;

                            }

                            cb.Style = new DataGridViewCellStyle()
                            {
                                SelectionForeColor = Color.Black,
                                SelectionBackColor = Theme.GetInstance().AccentColor,
                                BackColor = Color.White,
                            };

                            CustomFieldsDataGrid.Rows[i].Cells[5] = cb;

                            bool disable =
                                type.EditorType == EditorTypeConfig.EDITOR_TYPE_VARIABLE_EDITOR ||
                                type.EditorType == EditorTypeConfig.EDITOR_TYPE_TAG_EDITOR ||
                                type.EditorType == EditorTypeConfig.EDITOR_TYPE_MULTI_LINE_TEXT_EDITOR ||
                                type.FieldType == MetaDataFieldType.METADATA_FIELD_TYPE_COMBO_BOX;

                            if (type.MultiValued)
                            {
                                if (type.FieldType == MetaDataFieldType.METADATA_FIELD_TYPE_COMBO_BOX)
                                {
                                    CustomFieldsDataGrid.Rows[i].Cells[5].Value = false;
                                }
                            }

                            CustomFieldsDataGrid.Rows[i].Cells[5].ReadOnly = disable;

                            DataGridViewTextBoxCell tb = new DataGridViewTextBoxCell();
                            tb.Value = type.MultiValueSeparator;

                            tb.Style = new DataGridViewCellStyle()
                            {
                                SelectionForeColor = Color.Black,
                                SelectionBackColor = Theme.GetInstance().AccentColor,
                                BackColor = Color.White,
                            };

                            CustomFieldsDataGrid.Rows[i].Cells[6] = tb;

                            CustomFieldsDataGrid.Rows[i].Cells[6].ReadOnly = !type.MultiValued;

                            cb = new DataGridViewCheckBoxCell();
                            cb.Value = type.AutoUpdate;
                            cb.Style = new DataGridViewCellStyle()
                            {
                                SelectionForeColor = Color.Black,
                                SelectionBackColor = Theme.GetInstance().AccentColor,
                                BackColor = Color.White,
                            };

                            CustomFieldsDataGrid.Rows[i].Cells[7] = cb;

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

                                CustomFieldsDataGrid.Rows[i].Cells[8] = bc;
                            }
                            else
                            {
                                CustomFieldsDataGrid.Rows[i].Cells[8].ReadOnly = true;
                            }

                            if (type.EditorType == EditorTypeConfig.EDITOR_TYPE_VARIABLE_EDITOR)
                            {
                                string[] variables = Win_CBZSettings.Default.RenamerPlaceholders.OfType<String>().ToArray();

                                DataGridViewButtonCell bc = new DataGridViewButtonCell
                                {
                                    Value = "...",
                                    Tag = new EditorTypeConfig(EditorTypeConfig.EDITOR_TYPE_VARIABLE_EDITOR, EditorTypeConfig.RESULT_TYPE_STRING, "", "", false, variables)
                                };
                                CustomFieldsDataGrid.Rows[i].Cells[8] = bc;

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
                errorCategories.Clear();

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

                    if (FilterNewPagesByExtCheckBox.Checked && ImageFileExtensions.Count == 0)
                    {
                        throw new MetaDataValidationException("", "", "FilterNewPagesByExtCheckBox", "Validation Error! No Image-Extensions defined for filtering new pages!");
                    }

                    ImageFileExtensions.ForEach(ext =>
                    {
                        if (ext.Length == 0)
                        {
                            throw new MetaDataValidationException("", ext, "ExtensionList", "Validation Error! Empty Image-Extension not allowed!");
                        }

                        if (!Regex.IsMatch(ext, @"^[a-z0-9]+$", RegexOptions.IgnoreCase))
                        {
                            //MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_WARNING, "Validateion Error! Image-Extensions must contain only values between ['a-zA-Z0-9']");
                            throw new MetaDataValidationException("", ext, "ExtensionList", "Validation Error! Image-Extensions must contain only values between ['a-zA-Z0-9']!", true, false);
                        }
                    });

                    FilteredFileNames.ForEach(name =>
                    {
                        if (name.Length == 0)
                        {
                            throw new MetaDataValidationException("", name, "FilenameList", "Validation Error! Empty Filtered-Filename not allowed!");
                        }

                        if (Regex.IsMatch(name, @"^[+|;\\]+", RegexOptions.IgnoreCase))
                        {
                            //MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_WARNING, "Validateion Error! Filtered-Filenames must not contain ['|\\+;']");
                            throw new MetaDataValidationException("", name, "FilenameList", "Validation Error! Filtered-Filenames must not contain ['|\\+;']!", true, false);
                        }
                    });

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

                    DetectDoublePages = CheckboxDetectDoublePages.Checked;

                    DeleteTempFilesImediately = CheckBoxDeleteTempFiles.Checked;
                    SkipIndexCheck = CheckBoxSkipIndexCheck.Checked;
                    CalculateCrc32 = CheckBoxCalculateCrc.Checked;
                    JumpToSelectedPage = CheckBoxJumpToPage.Checked;

                    InterpolationMode = ComboBoxInterpolationModes.SelectedItem.ToString();// ComboBoxInterpolationModes.SelectedIndex;
                    TempPath = TextBoxTempPath.Text;

                    FilterNewPagesByExt = FilterNewPagesByExtCheckBox.Checked;
                    FilterNewPagesSpecificName = CheckboxFilterFilenames.Checked;

                    MetadataGridEditMode = CheckboxAlwaysInEditMode.Checked;
                    MetadataGridEditModeValueCol = CheckBoxEditModeOnlyValueCol.Checked;
                    WriteXMLPageIndex = CheckBoxWriteIndex.Checked;

                    CompressionLevel = ComboBoxCompressionLevel.SelectedIndex;
                    CompatibilityMode = CheckBoxCompatibilityMode.Checked;
                    IgnoreErrors = CheckBoxIgnoreErrorsOnSave.Checked;

                    SkipArchiveSubfolders = CheckBoxSkipSubfolders.Checked;

                    RestoreWindowPosition = CheckBoxSaveWindowLayout.Checked;
                    LogValidationErrors = CheckBoxLogValidationErrors.Checked;

                    AutoUpdateCheck = CheckboxAutoUpdate.Checked;
                    AutoUpdateCheckIntervalType = ComboBoxAutoUpdateInterval.SelectedIndex;

                    List<String> fieldConfigItems = new List<string>();
                    foreach (MetaDataFieldType fieldTypeCnf in CustomFieldTypesSettings)
                    {
                        fieldConfigItems.Add(fieldTypeCnf.ToString());

                    }
                    CustomFieldTypesCollection = fieldConfigItems.ToArray();

                    //
                    foreach (ThemeColorMapping mapping in ThemeColorMappings)
                    {
                        switch (mapping.ColorName)
                        {
                            case Theme.COLOR_NAME_ACCENT:
                                AccentColor = mapping.ColorValue;
                                break;

                            case Theme.COLOR_NAME_BUTTON:
                                ButtonColor = mapping.ColorValue;
                                break;

                            case Theme.COLOR_NAME_LIST_BACKGROUND:
                                ListBackgroundColor = mapping.ColorValue;
                                break;

                            case Theme.COLOR_NAME_TEXT:
                                TextColor = mapping.ColorValue;
                                break;
                        }

                    }

                    if (!Path.EndsInDirectorySeparator(TempPath))
                    {
                        TempPath += Path.DirectorySeparatorChar;
                    }
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

                    string errorSection = "";

                    if (controlName == "CustomDefaultKeys" ||
                        controlName == "ValidTags" ||
                        controlName == "ComboBoxFileName")
                    {
                        errorSection = "metadata";
                    }
                    else if (controlName == "CustomFieldsDataGrid" ||
                        controlName == "TextBoxTempPath" ||
                        controlName == "FilterNewPagesByExtCheckBox" ||
                        controlName == "CheckboxFilterFilenames" ||
                        controlName == "ExtensionList" ||
                        controlName == "FilenameList"
                        )
                    {
                        errorSection = "application";
                    }

                    if (errorSection.Length > 0)
                    {
                        if (errorCategories.Contains(errorSection) == false)
                        {
                            errorCategories.Add(errorSection);
                        }


                        SettingsSectionList.Refresh();
                    }

                    try
                    {
                        SettingsValidationErrorProvider.SetError(this.Controls.Find(controlName, true)[0], mv.Message);
                    }
                    catch
                    {
                        ApplicationMessage.ShowError("Failed to assign error to control! Control '${controlName}' not found!", "Error", ApplicationMessage.DialogType.MT_ERROR, ApplicationMessage.DialogButtons.MB_OK);

                    }

                    if (row != null && row.Length > 0)
                    {
                        CustomFieldsDataGrid.Rows[int.Parse(row)].ErrorText = mv.Message;
                    }

                    DialogResult = DialogResult.Cancel;
                    e.Cancel = true;

                    if (Win_CBZSettings.Default.LogValidationErrors)
                    {
                        MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_ERROR, mv.Message);
                    }

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
            if (SettingsSectionList.SelectedIndex == -1)
            {
                SettingsSectionList.SelectedIndex = 0;
            }

            string section = SettingsSectionList.SelectedItem.ToString().ToLower();

            if (section == "application")
            {
                MetaDataConfigTabControl.Visible = false;
                ImageProcessingTabControl.Visible = false;
                AppSettingsTabControl.Visible = true;
                CBZSettingsTabControl.Visible = false;
                UpdatesTabControl.Visible = false;
                AppearanceTabControl.Visible = false;
            }

            if (section == "metadata")
            {
                MetaDataConfigTabControl.Visible = true;
                ImageProcessingTabControl.Visible = false;
                AppSettingsTabControl.Visible = false;
                CBZSettingsTabControl.Visible = false;
                UpdatesTabControl.Visible = false;
                AppearanceTabControl.Visible = false;
            }

            if (section == "appearance")
            {
                MetaDataConfigTabControl.Visible = false;
                ImageProcessingTabControl.Visible = false;
                AppSettingsTabControl.Visible = false;
                CBZSettingsTabControl.Visible = false;
                UpdatesTabControl.Visible = false;
                AppearanceTabControl.Visible = true;

                ThemeColorsListbox.SelectedIndex = -1;
                ThemeColorsListbox.SelectedIndex = LastSelectedThemeColorIndex;
                ThemeColorsListbox.Invalidate();
            }

            if (section == "cbz")
            {
                MetaDataConfigTabControl.Visible = false;
                ImageProcessingTabControl.Visible = false;
                AppSettingsTabControl.Visible = false;
                CBZSettingsTabControl.Visible = true;
                UpdatesTabControl.Visible = false;
                AppearanceTabControl.Visible = false;
            }

            if (section == "image processing")
            {
                MetaDataConfigTabControl.Visible = false;
                ImageProcessingTabControl.Visible = true;
                AppSettingsTabControl.Visible = false;
                CBZSettingsTabControl.Visible = false;
                UpdatesTabControl.Visible = false;
                AppearanceTabControl.Visible = false;
            }

            if (section == "updates")
            {
                MetaDataConfigTabControl.Visible = false;
                ImageProcessingTabControl.Visible = false;
                AppSettingsTabControl.Visible = false;
                CBZSettingsTabControl.Visible = false;
                UpdatesTabControl.Visible = true;
                AppearanceTabControl.Visible = false;
            }
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
                    if (e.ColumnIndex == 8)
                    {
                        value = senderGrid.Rows[e.RowIndex].Cells[e.ColumnIndex - 5].Value;
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

                    if (e.ColumnIndex == 1 || e.ColumnIndex == 2 || e.ColumnIndex == 4)
                    {
                        if (senderGrid.Rows[e.RowIndex].Cells[e.ColumnIndex] is DataGridViewComboBoxCell)
                        {
                            // && fieldType.FieldType == MetaDataFieldType.METADATA_FIELD_TYPE_COMBO_BOX) {
                            DataGridViewComboBoxCell comboCell = senderGrid.Rows[e.RowIndex].Cells[e.ColumnIndex] as DataGridViewComboBoxCell;
                            senderGrid.BeginEdit(true);
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
                                            senderGrid.Rows[e.RowIndex].Cells[e.ColumnIndex - 5].Value = textEditor.Config.Result.ToString();
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
                                            senderGrid.Rows[e.RowIndex].Cells[e.ColumnIndex - 5].Value = langEditor.config.Result.ToString();
                                        }
                                    }
                                }
                                break;
                            case EditorTypeConfig.EDITOR_TYPE_VARIABLE_EDITOR:
                                {
                                    VariableInsertionEditorConfigForm varEditor = new VariableInsertionEditorConfigForm(editorConfig);
                                    DialogResult r = varEditor.ShowDialog();
                                    if (r == DialogResult.OK)
                                    {
                                        if (varEditor.Config.Result != null)
                                        {
                                            senderGrid.Rows[e.RowIndex].Cells[e.ColumnIndex - 5].Value = varEditor.Config.Result.ToString();
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
                                    CustomFieldsDataGrid.BeginEdit(false);

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
                bool Multivalued = false;
                string MultivalueSeparator = "";

                object value = CustomFieldsDataGrid.Rows[e.RowIndex].Cells[0].Value;
                if (value == null)
                {
                    value = "";
                }

                Key = value.ToString();

                value = CustomFieldsDataGrid.Rows[e.RowIndex].Cells[1].Value;
                if (value == null)
                {
                    value = "";
                }

                FieldType = value.ToString();

                value = CustomFieldsDataGrid.Rows[e.RowIndex].Cells[2].Value;
                if (value == null)
                {
                    value = "";
                }

                EditorType = value.ToString();

                value = CustomFieldsDataGrid.Rows[e.RowIndex].Cells[3].Value;
                if (value == null)
                {
                    value = "";
                }

                Val = value.ToString();

                value = CustomFieldsDataGrid.Rows[e.RowIndex].Cells[4].Value;
                if (value == null || (string)value == "")
                {
                    value = "";
                }

                AutoCompleteImageKey = value.ToString();

                value = CustomFieldsDataGrid.Rows[e.RowIndex].Cells[7].Value;
                if (value == null)
                {
                    value = "False";
                }

                AutoUpdate = Boolean.Parse(value.ToString());

                value = CustomFieldsDataGrid.Rows[e.RowIndex].Cells[5].Value;
                if (value == null)
                {
                    value = "False";
                }

                Multivalued = Boolean.Parse(value.ToString());

                value = CustomFieldsDataGrid.Rows[e.RowIndex].Cells[6].Value;
                if (value == null)
                {
                    value = "";
                }

                MultivalueSeparator = value.ToString();

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
                    updatedEntry.MultiValued = Multivalued;
                    updatedEntry.MultiValueSeparator = MultivalueSeparator;

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
                        SelectionBackColor = Theme.GetInstance().AccentColor,
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
                        SelectionBackColor = Theme.GetInstance().AccentColor,
                        BackColor = Color.White,
                    };

                    CustomFieldsDataGrid.Rows[e.RowIndex].Cells[2] = cc;

                    if (updatedEntry.FieldType == MetaDataFieldType.METADATA_FIELD_TYPE_AUTO_COMPLETE ||
                        updatedEntry.FieldType == MetaDataFieldType.METADATA_FIELD_TYPE_COMBO_BOX
                      )
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
                            SelectionBackColor = Theme.GetInstance().AccentColor,
                            BackColor = Color.White,
                        };

                        CustomFieldsDataGrid.Rows[e.RowIndex].Cells[4].ReadOnly = false;
                        CustomFieldsDataGrid.Rows[e.RowIndex].Cells[4] = ci;
                    }
                    else
                    {
                        DataGridViewTextBoxCell tc = new DataGridViewTextBoxCell();
                        tc.Value = "";
                        CustomFieldsDataGrid.Rows[e.RowIndex].Cells[4] = tc;

                        CustomFieldsDataGrid.Rows[e.RowIndex].Cells[4].ReadOnly = true;

                    }

                    if (updatedEntry.EditorType == EditorTypeConfig.EDITOR_TYPE_MULTI_LINE_TEXT_EDITOR)
                    {
                        CustomFieldsDataGrid.Rows[e.RowIndex].Cells[5].Value = true;
                        CustomFieldsDataGrid.Rows[e.RowIndex].Cells[6].ReadOnly = false;
                        updatedEntry.MultiValued = true;

                    }
                    else if (updatedEntry.EditorType == EditorTypeConfig.EDITOR_TYPE_TAG_EDITOR)
                    {
                        CustomFieldsDataGrid.Rows[e.RowIndex].Cells[5].Value = true;
                        CustomFieldsDataGrid.Rows[e.RowIndex].Cells[6].ReadOnly = false;
                        updatedEntry.MultiValued = true;

                        if (updatedEntry.FieldType == MetaDataFieldType.METADATA_FIELD_TYPE_COMBO_BOX)
                        {
                            updatedEntry.FieldType = MetaDataFieldType.METADATA_FIELD_TYPE_TEXT_BOX;
                            if (CustomFieldsDataGrid.Rows[e.RowIndex].Cells[1] is DataGridViewComboBoxCell)
                            {
                                CustomFieldsDataGrid.Rows[e.RowIndex].Cells[1].Value = updatedEntry.FieldType;

                            }
                        }
                    }

                    if (updatedEntry.FieldType == MetaDataFieldType.METADATA_FIELD_TYPE_COMBO_BOX ||
                        updatedEntry.FieldType == MetaDataFieldType.METADATA_FIELD_TYPE_AUTO_COMPLETE)
                    {
                        DataGridViewButtonCell bc = new DataGridViewButtonCell
                        {
                            Value = "...",
                            Tag = new EditorTypeConfig(EditorTypeConfig.EDITOR_TYPE_MULTI_LINE_TEXT_EDITOR, EditorTypeConfig.RESULT_TYPE_STRING, ",", " ", false)
                        };
                        CustomFieldsDataGrid.Rows[e.RowIndex].Cells[8] = bc;

                        if (updatedEntry.MultiValued)
                        {
                            if (updatedEntry.FieldType == MetaDataFieldType.METADATA_FIELD_TYPE_COMBO_BOX)
                            {
                                updatedEntry.MultiValued = false;
                                CustomFieldsDataGrid.Rows[e.RowIndex].Cells[5].Value = false;
                            }

                        }
                        else
                        {

                        }

                        //CustomFieldsDataGrid.Rows[e.RowIndex].Cells[5].Value = !(updatedEntry.FieldType == MetaDataFieldType.METADATA_FIELD_TYPE_COMBO_BOX);
                        CustomFieldsDataGrid.Rows[e.RowIndex].Cells[6].ReadOnly = !updatedEntry.MultiValued;
                    }
                    else
                    {
                        DataGridViewTextBoxCell tc = new DataGridViewTextBoxCell();

                        CustomFieldsDataGrid.Rows[e.RowIndex].Cells[8] = tc;
                        CustomFieldsDataGrid.Rows[e.RowIndex].Cells[6].ReadOnly = !updatedEntry.MultiValued;
                    }

                }

                if (updatedEntry.EditorType == EditorTypeConfig.EDITOR_TYPE_MULTI_LINE_TEXT_EDITOR)
                {


                }
                else if (updatedEntry.EditorType == EditorTypeConfig.EDITOR_TYPE_TAG_EDITOR)
                {


                }
                else if (updatedEntry.EditorType == EditorTypeConfig.EDITOR_TYPE_VARIABLE_EDITOR)
                {
                    string[] variables = Win_CBZSettings.Default.RenamerPlaceholders.OfType<String>().ToArray();

                    DataGridViewButtonCell bc = new DataGridViewButtonCell
                    {
                        Value = "...",
                        Tag = new EditorTypeConfig(EditorTypeConfig.EDITOR_TYPE_VARIABLE_EDITOR, EditorTypeConfig.RESULT_TYPE_STRING, "", "", false, variables)
                    };
                    CustomFieldsDataGrid.Rows[e.RowIndex].Cells[8] = bc;
                    CustomFieldsDataGrid.Rows[e.RowIndex].Cells[5].Value = false;

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
                SelectionBackColor = Theme.GetInstance().AccentColor,
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
                SelectionBackColor = Theme.GetInstance().AccentColor,
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
                    SelectionBackColor = Theme.GetInstance().AccentColor,
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
            cb.Value = newEntry.MultiValued;
            cb.Style = new DataGridViewCellStyle()
            {
                SelectionForeColor = Color.Black,
                SelectionBackColor = Theme.GetInstance().AccentColor,
                BackColor = Color.White,
            };

            CustomFieldsDataGrid.Rows[newIndex].Cells[5] = cb;

            CustomFieldsDataGrid.Rows[newIndex].Cells[5].ReadOnly = newEntry.EditorType == EditorTypeConfig.EDITOR_TYPE_VARIABLE_EDITOR ||
                newEntry.EditorType == EditorTypeConfig.EDITOR_TYPE_TAG_EDITOR ||
                newEntry.EditorType == EditorTypeConfig.EDITOR_TYPE_MULTI_LINE_TEXT_EDITOR;

            DataGridViewTextBoxCell tb = new DataGridViewTextBoxCell();
            tb.Value = newEntry.MultiValueSeparator;
            tb.Style = new DataGridViewCellStyle()
            {
                SelectionForeColor = Color.Black,
                SelectionBackColor = Theme.GetInstance().AccentColor,
                BackColor = Color.White,
            };

            CustomFieldsDataGrid.Rows[newIndex].Cells[6] = tb;


            cb = new DataGridViewCheckBoxCell();
            cb.Value = newEntry.AutoUpdate;
            cb.Style = new DataGridViewCellStyle()
            {
                SelectionForeColor = Color.Black,
                SelectionBackColor = Theme.GetInstance().AccentColor,
                BackColor = Color.White,
            };

            CustomFieldsDataGrid.Rows[newIndex].Cells[7] = cb;

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

                CustomFieldsDataGrid.Rows[newIndex].Cells[8] = bc;
            }

            if (newEntry.EditorType == EditorTypeConfig.EDITOR_TYPE_VARIABLE_EDITOR)
            {
                string[] variables = Win_CBZSettings.Default.RenamerPlaceholders.OfType<String>().ToArray();

                DataGridViewButtonCell bc = new DataGridViewButtonCell
                {
                    Value = "...",
                    Tag = new EditorTypeConfig(EditorTypeConfig.EDITOR_TYPE_VARIABLE_EDITOR, EditorTypeConfig.RESULT_TYPE_STRING, "", "", false, variables)
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
                            ApplicationMessage.ShowError("Error restoring defaul Fieldtypes.\r\n" + ex.Message, "Error resoring defaults", ApplicationMessage.DialogType.MT_ERROR, ApplicationMessage.DialogButtons.MB_OK);
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

            ImageFileExtensions.ForEach((String ext) =>
            {
                // todo: create extension list
                if (ext.Trim(' ').Length > 0)
                {
                    AddExt(CreateExt(ext, "EXT", ExtensionList), ExtensionList);
                }

            });

            FilteredFileNames.ForEach((String name) =>
            {
                if (name.Trim(' ').Length > 0)
                {
                    AddExt(CreateExt(name, "FN", FilenameList), FilenameList);
                }
            });
        }

        private void ToolStripTextBoxSearchTag_KeyUp(object sender, KeyEventArgs e)
        {
            String itemsText = ValidTags.Text;

            if (e.KeyCode == Keys.F3)
            {
                lastSearchOccurence = occurence + ToolStripTextBoxSearchTag.Text.Length;

                ValidTags.SelectionStart = lastSearchOccurence + ToolStripTextBoxSearchTag.Text.Length;
                ValidTags.SelectionLength = 0;

                nextOccurence = itemsText.IndexOf(ToolStripTextBoxSearchTag.Text, lastSearchOccurence, StringComparison.CurrentCultureIgnoreCase);


                if (nextOccurence < 0)
                {
                    ApplicationMessage.Show("Search reached the end of the document. Starting from the beginning.", "Search", ApplicationMessage.DialogType.MT_INFORMATION, ApplicationMessage.DialogButtons.MB_OK);

                    lastSearchOccurence = 0;
                }
            }
            else
            {
                lastSearchOccurence = 0;
            }

            occurence = itemsText.IndexOf(ToolStripTextBoxSearchTag.Text, lastSearchOccurence, StringComparison.CurrentCultureIgnoreCase);

            ValidTags.SelectionStart = 0;
            ValidTags.SelectionLength = 0;
            if (occurence > -1)
            {
                ValidTags.SelectionStart = occurence;
                ValidTags.SelectionLength = ToolStripTextBoxSearchTag.Text.Length;
                ValidTags.ScrollToCaret();

                lastSearchOccurence = occurence;
            }
        }

        private void ButtonSelectFolder_Click(object sender, EventArgs e)
        {
            OpenTargetDirectory.InitialDirectory = PathHelper.ResolvePath(TextBoxTempPath.Text);
            if (OpenTargetDirectory.ShowDialog() == DialogResult.OK)
            {
                //LocalFile localFile = new LocalFile(OpenTargetDirectory.SelectedPath);

                TextBoxTempPath.Text = OpenTargetDirectory.SelectedPath;
            }
        }

        private void CustomFieldsDataGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = sender as DataGridView;

            if (e.RowIndex < 0 || e.ColumnIndex < 0)
            {
                return;
            }

            if (e.ColumnIndex == 0 || e.ColumnIndex == 1 || e.ColumnIndex == 2 || e.ColumnIndex == 3 || e.ColumnIndex == 4 || e.ColumnIndex == 6)
            {
                if (senderGrid.Rows[e.RowIndex].Cells[e.ColumnIndex] is DataGridViewComboBoxCell)
                {
                    // && fieldType.FieldType == MetaDataFieldType.METADATA_FIELD_TYPE_COMBO_BOX) {
                    DataGridViewComboBoxCell comboCell = senderGrid.Rows[e.RowIndex].Cells[e.ColumnIndex] as DataGridViewComboBoxCell;
                    senderGrid.BeginEdit(true);
                }

                if (senderGrid.Rows[e.RowIndex].Cells[e.ColumnIndex] is DataGridViewTextBoxCell)
                {
                    // && fieldType.FieldType == MetaDataFieldType.METADATA_FIELD_TYPE_COMBO_BOX) {
                    //DataGridViewComboBoxCell comboCell = senderGrid.Rows[e.RowIndex].Cells[e.ColumnIndex] as DataGridViewComboBoxCell;
                    senderGrid.BeginEdit(false);
                }
            }
        }

        private void SettingsSectionList_DrawItem(object sender, DrawItemEventArgs e)
        {
            Color backgroundColor = Theme.GetInstance().ListBackgroundColor;
            Color textColor = Theme.GetInstance().TextColor;
            System.Drawing.Pen pen = new System.Drawing.Pen(textColor, 1);

            System.Drawing.SolidBrush tb = new SolidBrush(Theme.GetInstance().TextColor);

            Font f = SystemFonts.CaptionFont;

            if (e.State.HasFlag(DrawItemState.Selected))
            {
                backgroundColor = HTMLColor.ToColor(AccentColor);
            }
            else
            {
                backgroundColor = Theme.GetInstance().ListBackgroundColor;
            }

            string name = SettingsSectionList.Items[e.Index] as string;

            System.Drawing.SolidBrush bg = new SolidBrush(backgroundColor);

            e.Graphics.FillRectangle(bg, new Rectangle(e.Bounds.Left, e.Bounds.Top, e.Bounds.Width, e.Bounds.Height));
            e.Graphics.DrawString(name, f, tb, e.Bounds.X + 26, e.Bounds.Y + 6);
            if (CategoryImages.Images.ContainsKey(name.ToLower().Replace(' ', '_')))
            {
                e.Graphics.DrawImage(CategoryImages.Images[name.ToLower().Replace(' ', '_')], e.Bounds.X + 1, e.Bounds.Y + 3);
            }

            if (errorCategories.IndexOf(name.ToLower()) > -1)
            {
                e.Graphics.DrawImage(ErrorImages.Images["error"], e.Bounds.Right - 20, e.Bounds.Y + 8);
            }
        }

        private void ColotList_DrawItem(object sender, DrawItemEventArgs e)
        {
            Color backgroundColor = Theme.GetInstance().ListBackgroundColor;
            Color textColor = Theme.GetInstance().TextColor;
            System.Drawing.Pen pen = new System.Drawing.Pen(textColor, 1);

            ListBox lb = sender as ListBox;

            TextFormatFlags flags = TextFormatFlags.Left | TextFormatFlags.EndEllipsis;

            System.Drawing.SolidBrush tb = new SolidBrush(Theme.GetInstance().TextColor);

            Font f = SystemFonts.CaptionFont;

            if (e.State.HasFlag(DrawItemState.Selected))
            {
                backgroundColor = Theme.GetInstance().AccentColor;
            }
            else
            {
                backgroundColor = Theme.GetInstance().ListBackgroundColor;
            }

            ThemeColorMapping colorConfig = ThemeColorsListbox.Items[e.Index] as ThemeColorMapping;

            System.Drawing.SolidBrush bg = new SolidBrush(backgroundColor);

            // draw item background
            e.Graphics.FillRectangle(bg, new Rectangle(e.Bounds.Left, e.Bounds.Top, e.Bounds.Width, e.Bounds.Height));
            // 
            //e.Graphics.DrawString(colorConfig.ColorName, f, tb, e.Bounds.X + 22, e.Bounds.Y + 2);

            TextRenderer.DrawText(e.Graphics, colorConfig.Label, lb.Font, new Rectangle(e.Bounds.X + 22, e.Bounds.Y + 3, e.Bounds.Width, e.Bounds.Height), textColor, flags);


            e.Graphics.DrawRectangle(pen, new Rectangle(e.Bounds.X + 2, e.Bounds.Y + 2, 17, 17));
            e.Graphics.FillRectangle(new SolidBrush(HTMLColor.ToColor(colorConfig.ColorValue)), new Rectangle(e.Bounds.X + 3, e.Bounds.Y + 3, 16, 16));

        }

        private void Button2_Click(object sender, EventArgs e)
        {
            if (ExtensionTextBox.Text.Trim(' ').Length > 0)
            {
                ImageFileExtensions.Add(ExtensionTextBox.Text.ToLower());
                AddExt(CreateExt(ExtensionTextBox.Text.ToLower(), "EXT", ExtensionList), ExtensionList);
                //AddTag(ExtensionTextBox.Text);
                ExtensionTextBox.Text = string.Empty;
            }
            ExtensionTextBox.Focus();
        }

        private void ExtensionTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                if (ExtensionTextBox.Text.Trim(' ').Length > 0)
                {
                    ImageFileExtensions.Add(ExtensionTextBox.Text.ToLower());
                    AddExt(CreateExt(ExtensionTextBox.Text, "EXT", ExtensionList), ExtensionList);
                    ExtensionTextBox.Text = string.Empty;

                }
                e.SuppressKeyPress = true;
                e.Handled = true;
            }
        }

        public FlowLayoutPanel CreateExt(string tagName, string prefix, FlowLayoutPanel parent)
        {
            if (string.IsNullOrEmpty(tagName))
            {
                return null;
            }

            FlowLayoutPanel tagItem = new FlowLayoutPanel();
            tagItem.Name = prefix + "_" + tagName;
            tagItem.Tag = new TagItem()
            {
                Tag = tagName
            };
            tagItem.AutoSize = true;
            //tagItem.Click += TagItemClick;
            tagItem.BackColor = System.Drawing.Color.White;

            tagItem.BorderStyle = BorderStyle.None;
            tagItem.GotFocus += TagFocused;
            tagItem.LostFocus += TagLostFocus;

            /*
            System.Windows.Forms.Button closeButton = new System.Windows.Forms.Button()
            {
                Text = "",
                Tag = tagItem,
                Image = global::Win_CBZ.Properties.Resources.delete,
                ImageAlign = ContentAlignment.MiddleCenter,
                FlatStyle = FlatStyle.Flat,
                Size = new Size()
                {
                    Width = 15,
                    Height = 15
                },
            };
            */

            PictureBox closeButton = new PictureBox()
            {
                Image = global::Win_CBZ.Properties.Resources.delete,
                SizeMode = PictureBoxSizeMode.StretchImage,
                Width = 15,
                Height = 15,
                Tag = tagItem,
                Cursor = System.Windows.Forms.Cursors.Hand,
                Margin = new Padding(0, 2, 2, 2),
                Padding = new Padding(0, 2, 1, 1)
            };

            System.Windows.Forms.Label tagLabel = new System.Windows.Forms.Label()
            {
                Name = prefix + "_LABEL",
                Text = tagName,
                AutoSize = true,
                Tag = tagItem,
                Padding = new Padding(1, 1, 1, 1),
                Margin = new Padding(0, 2, 0, 1),
                Font = new Font("Segoe UI", 8, FontStyle.Regular)
            };

            closeButton.Click += ExtCloseButtonClick;
            tagLabel.Click += TagItemClick;

            /*
            tagItem.Controls.Add(new PictureBox()
            {
                Image = global::Win_CBZ.Properties.Resources.tag,
                SizeMode = PictureBoxSizeMode.AutoSize,
                Width = 15,
                Height = 15,
                Margin = new Padding(3, 1, 1, 2)
            });

            */
            tagItem.Controls.Add(tagLabel);

            tagItem.Controls.Add(closeButton);

            tagItem.Parent = parent;

            return tagItem;
        }

        public void AddExt(System.Windows.Forms.Control control, FlowLayoutPanel container)
        {
            if (control != null)
            {
                Invoke(new Action(() =>
                {
                    container.Controls.Add(control);
                }));

            }
        }

        public void RemoveExt(string tagName, string prefix, FlowLayoutPanel container)
        {
            if (string.IsNullOrEmpty(tagName))
            {
                return;
            }

            System.Windows.Forms.Control[] tags = ExtensionList.Controls.Find(prefix + "_" + tagName, false);
            if (tags != null && tags.Length > 0)
            {
                FlowLayoutPanel tag = container.Controls.Find(prefix + "_" + tagName, false)[0] as FlowLayoutPanel;

                if (tag != null)
                {
                    container.Controls.Remove(tag);
                }
            }
        }

        public void ClearExtensions(FlowLayoutPanel container)
        {
            container.Controls.Clear();
        }

        private void TagItemClick(object sender, EventArgs e)
        {
            Label label = sender as Label;
            FlowLayoutPanel container = label.Parent as FlowLayoutPanel;

            if (container != null)
            {
                if (!((TagItem)container.Tag).Selected)
                {
                    //Label contentLabel = container.Controls.Find("TAG_LABEL", false)[0] as Label;
                    //if (contentLabel != null)
                    //{
                    //label.BackColor = Theme.GetInstance().AccentColor,;
                    //}
                    //((TagItem)container.Tag).Selected = true;
                    //SelectedTags.Add(container);
                }
                else
                {

                    //label.BackColor = Color.White;
                    //}
                    //((TagItem)container.Tag).Selected = false;
                    //SelectedTags.Remove(container);
                }

                //ToolStripButtonRemoveSelectedTags.Enabled = SelectedTags.Count > 0;
            }
        }

        private void TagFocused(object sender, EventArgs e)
        {

        }

        private void TagLostFocus(object sender, EventArgs e)
        {

        }

        private void ExtCloseButtonClick(object sender, System.EventArgs e)
        {
            if (((PictureBox)sender).Tag != null)
            {
                var tag = ((PictureBox)sender).Tag as FlowLayoutPanel;
                if (tag != null && tag.Parent.Name == "ExtensionList")
                {
                    ImageFileExtensions.Remove(((TagItem)(tag as FlowLayoutPanel).Tag).Tag);
                    ExtensionList.Controls.Remove(tag);
                }
                else if (tag != null && tag.Parent.Name == "FilenameList")
                {
                    FilteredFileNames.Remove(((TagItem)(tag as FlowLayoutPanel).Tag).Tag);
                    FilenameList.Controls.Remove(tag);
                }



            }
        }

        private void CustomFieldsDataGrid_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {

        }

        private void CheckBoxWriteIndex_CheckedChanged(object sender, EventArgs e)
        {
            bool enabled = CheckBoxWriteIndex.Checked;

            CheckBoxCompatibilityMode.Enabled = enabled;
            CheckBoxCompatibilityMode.Checked = !enabled;
            if (enabled)
            {
                CheckBoxCompatibilityMode.Checked = CompatibilityMode;
            }

        }

        private void ComboBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0)
            {
                return;
            }

            if (sender as ComboBox == null)
            {
                return;
            }

            Pen pen = new Pen(Theme.GetInstance().TextColor, 1);
            Font font = new Font("Verdana", 9f, FontStyle.Regular);

            if (e.State.HasFlag(DrawItemState.Selected))
            {
                e.Graphics.FillRectangle(new SolidBrush(Theme.GetInstance().AccentColor), e.Bounds);
            }
            else
            {
                e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(255, 255, 255)), e.Bounds);
            }

            String icon = ((ComboBox)sender).Tag as String;

            if (ComboIcons.Images.ContainsKey(icon))
            {
                Image img = ComboIcons.Images[icon];
                e.Graphics.DrawImage(img, new Point(e.Bounds.X, e.Bounds.Y));
                e.Graphics.DrawString(((ComboBox)sender).Items[e.Index].ToString(), font, new SolidBrush(Theme.GetInstance().TextColor), new PointF(e.Bounds.X + 18, e.Bounds.Y + 1));
            }
            else
            {
                e.Graphics.DrawString(((ComboBox)sender).Items[e.Index].ToString(), font, new SolidBrush(Theme.GetInstance().TextColor), new PointF(e.Bounds.X + 1, e.Bounds.Y + 1));
            }
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            if (FilenamesTextbox.Text.Trim(' ').Length > 0)
            {
                FilteredFileNames.Add(FilenamesTextbox.Text.ToLower());
                AddExt(CreateExt(FilenamesTextbox.Text.ToLower(), "FN", FilenameList), FilenameList);
                //AddTag(ExtensionTextBox.Text);
                FilenamesTextbox.Text = string.Empty;
            }
            FilenamesTextbox.Focus();
        }

        private void FilenamesTextbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                if (FilenamesTextbox.Text.Trim(' ').Length > 0)
                {
                    FilteredFileNames.Add(FilenamesTextbox.Text.ToLower());
                    AddExt(CreateExt(FilenamesTextbox.Text.ToLower(), "FN", FilenameList), FilenameList);
                    FilenamesTextbox.Text = string.Empty;

                }
                e.SuppressKeyPress = true;
                e.Handled = true;
            }
        }

        private void SettingsDialog_Load(object sender, EventArgs e)
        {

        }

        private void CheckboxAutoUpdate_CheckedChanged(object sender, EventArgs e)
        {
            ComboBoxAutoUpdateInterval.Enabled = CheckboxAutoUpdate.Checked;
        }

        private void ThemeColorsListbox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ThemeColorMapping colorConfig = null;
                if (ThemeColorsListbox.SelectedItem != null)
                {
                    colorConfig = ThemeColorsListbox.SelectedItem as ThemeColorMapping;
                    ThemeSelectColorDialog.Color = HTMLColor.ToColor(colorConfig.ColorValue);
                    TextboxSelectedThemeColorValue.Text = colorConfig.ColorValue;
                    PictureBoxColorSelect.BackColor = HTMLColor.ToColor(colorConfig.ColorValue);
                    LabelColorDescription.Text = colorConfig.Description;
                    LastSelectedThemeColorIndex = ThemeColorsListbox.SelectedIndex;
                }
                else
                {

                }

                UpdateExampleColors(colorConfig);
            }
            catch (Exception ex)
            {
                MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_ERROR, ex.Message);
            }
        }

        private void PictureBoxColorSelect_Click(object sender, EventArgs e)
        {
            if (ThemeSelectColorDialog.ShowDialog() == DialogResult.OK)
            {
                PictureBoxColorSelect.BackColor = ThemeSelectColorDialog.Color;
                TextboxSelectedThemeColorValue.Text = HTMLColor.ToHexColor(ThemeSelectColorDialog.Color);
                if (ThemeColorsListbox.SelectedItem != null)
                {
                    ThemeColorMapping colorConfig = ThemeColorsListbox.SelectedItem as ThemeColorMapping;
                    colorConfig.ColorValue = TextboxSelectedThemeColorValue.Text;
                    int selectedIndex = ThemeColorsListbox.SelectedIndex;

                    ThemeColorsListbox.Invalidate();
                }
            }
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            if (ThemeColorsListbox.SelectedItem != null)
            {
                ThemeColorMapping colorConfig = ThemeColorsListbox.SelectedItem as ThemeColorMapping;
                colorConfig.ColorValue = colorConfig.DefaultValue;
                PictureBoxColorSelect.BackColor = HTMLColor.ToColor(colorConfig.ColorValue);
                TextboxSelectedThemeColorValue.Text = colorConfig.ColorValue;

                ThemeColorsListbox.Invalidate();
            }
        }

        private void TextboxSelectedThemeColorValue_TextChanged(object sender, EventArgs e)
        {

            if (ThemeColorsListbox.SelectedItem != null)
            {
                ThemeColorMapping colorConfig = ThemeColorsListbox.SelectedItem as ThemeColorMapping;

                try
                {
                    PictureBoxColorSelect.BackColor = HTMLColor.ToColor(TextboxSelectedThemeColorValue.Text);

                    colorConfig.ColorValue = TextboxSelectedThemeColorValue.Text;

                    LastSelectedThemeColorIndex = ThemeColorsListbox.SelectedIndex;
                    ThemeColorsListbox.Invalidate();

                }
                catch (Exception ex)
                {
                    PictureBoxColorSelect.BackColor = HTMLColor.ToColor("#000000");

                }
            }
        }

        private void PictureBoxExampleColor_Click(object sender, EventArgs e)
        {
            PictureBox pictureBox = sender as PictureBox;

            PictureBoxColorSelect.BackColor = pictureBox.BackColor;
            TextboxSelectedThemeColorValue.Text = HTMLColor.ToHexColor(pictureBox.BackColor);
            if (ThemeColorsListbox.SelectedItem != null)
            {
                ThemeColorMapping colorConfig = ThemeColorsListbox.SelectedItem as ThemeColorMapping;
                colorConfig.ColorValue = TextboxSelectedThemeColorValue.Text;
                int selectedIndex = ThemeColorsListbox.SelectedIndex;

                ThemeColorsListbox.Invalidate();
            }
        }

        private void UpdateExampleColors(ThemeColorMapping colorConfig)
        {
            if (colorConfig == null)
            {
                return;
            }

            ExampleColorsFlowLayout.Controls.Clear();

            foreach (string example in colorConfig.ExampleValues)
            {
                if (example != null)
                {
                    PictureBox newExample = new PictureBox();
                    newExample.BorderStyle = BorderStyle.FixedSingle;
                    newExample.BackColor = HTMLColor.ToColor(example);
                    newExample.Width = 20;
                    newExample.Height = 20;
                    newExample.Margin = new Padding(10, 8, 10, 8);
                    newExample.Cursor = Cursors.Hand;
                    newExample.Click += PictureBoxExampleColor_Click;

                    ExampleColorsFlowLayout.Controls.Add(newExample);

                }
            }
        }
    }
}
