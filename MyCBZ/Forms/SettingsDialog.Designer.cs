namespace Win_CBZ.Forms
{
    partial class SettingsDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsDialog));
            SettingsTablePanel = new System.Windows.Forms.TableLayoutPanel();
            ButtonOk = new System.Windows.Forms.Button();
            HeaderPanel = new System.Windows.Forms.Panel();
            HeaderLabel = new System.Windows.Forms.Label();
            pictureBox1 = new System.Windows.Forms.PictureBox();
            SettingsSectionList = new System.Windows.Forms.ListBox();
            ButtonCancel = new System.Windows.Forms.Button();
            SettingsContentPanel = new System.Windows.Forms.Panel();
            AppSettingsTabControl = new System.Windows.Forms.TabControl();
            TabPageAppSettings = new System.Windows.Forms.TabPage();
            CustomFieldTypesTablePanel = new System.Windows.Forms.TableLayoutPanel();
            CustomFieldsDataGrid = new System.Windows.Forms.DataGridView();
            AddFieldTypeButton = new System.Windows.Forms.Button();
            DialogImages = new System.Windows.Forms.ImageList(components);
            RemoveFieldTypeButton = new System.Windows.Forms.Button();
            RestoreFieldTypesButton = new System.Windows.Forms.Button();
            tabPage5 = new System.Windows.Forms.TabPage();
            tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            CheckBoxSkipIndexCheck = new System.Windows.Forms.CheckBox();
            label9 = new System.Windows.Forms.Label();
            label7 = new System.Windows.Forms.Label();
            CheckBoxDeleteTempFiles = new System.Windows.Forms.CheckBox();
            CBZSettingsTabControl = new System.Windows.Forms.TabControl();
            tabPage2 = new System.Windows.Forms.TabPage();
            tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            label5 = new System.Windows.Forms.Label();
            ComboBoxPageIndexVersionWrite = new System.Windows.Forms.ComboBox();
            pictureBox2 = new System.Windows.Forms.PictureBox();
            CheckBoxPruneEmplyTags = new System.Windows.Forms.CheckBox();
            label6 = new System.Windows.Forms.Label();
            ImageProcessingTabControl = new System.Windows.Forms.TabControl();
            ImageConversionTabPage = new System.Windows.Forms.TabPage();
            LabelConvertImages = new System.Windows.Forms.Label();
            ComboBoxConvertPages = new System.Windows.Forms.ComboBox();
            GroupBoxImageQuality = new System.Windows.Forms.GroupBox();
            ImageQualitySliderMaxLabel = new System.Windows.Forms.Label();
            ImageQualitySliderMinLabel = new System.Windows.Forms.Label();
            ImageQualityTrackBar = new System.Windows.Forms.TrackBar();
            ImageProcessingTabPage = new System.Windows.Forms.TabPage();
            MetaDataConfigTabControl = new System.Windows.Forms.TabControl();
            tabPage1 = new System.Windows.Forms.TabPage();
            SettingsGroup1Panel = new System.Windows.Forms.Panel();
            MetaDataDefaultKeysTable = new System.Windows.Forms.TableLayoutPanel();
            label4 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            button1 = new System.Windows.Forms.Button();
            PictureBoxToolTipMetaFileName = new System.Windows.Forms.PictureBox();
            ComboBoxFileName = new System.Windows.Forms.ComboBox();
            CustomDefaultKeys = new System.Windows.Forms.TextBox();
            label1 = new System.Windows.Forms.Label();
            MetaDataTabPageTags = new System.Windows.Forms.TabPage();
            tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            CheckBoxValidateTags = new System.Windows.Forms.CheckBox();
            InfoIconTooltip = new System.Windows.Forms.PictureBox();
            CheckBoxTagValidationIgnoreCase = new System.Windows.Forms.CheckBox();
            label3 = new System.Windows.Forms.Label();
            ValidTags = new System.Windows.Forms.TextBox();
            ItemEditorToolBar = new System.Windows.Forms.ToolStrip();
            ToolStripTextBoxSearchTag = new System.Windows.Forms.ToolStripTextBox();
            ToolButtonSortAscending = new System.Windows.Forms.ToolStripButton();
            TagValidationTooltip = new System.Windows.Forms.ToolTip(components);
            SettingsValidationErrorProvider = new System.Windows.Forms.ErrorProvider(components);
            SettingsTablePanel.SuspendLayout();
            HeaderPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SettingsContentPanel.SuspendLayout();
            AppSettingsTabControl.SuspendLayout();
            TabPageAppSettings.SuspendLayout();
            CustomFieldTypesTablePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)CustomFieldsDataGrid).BeginInit();
            tabPage5.SuspendLayout();
            tableLayoutPanel4.SuspendLayout();
            CBZSettingsTabControl.SuspendLayout();
            tabPage2.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            ImageProcessingTabControl.SuspendLayout();
            ImageConversionTabPage.SuspendLayout();
            GroupBoxImageQuality.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)ImageQualityTrackBar).BeginInit();
            MetaDataConfigTabControl.SuspendLayout();
            tabPage1.SuspendLayout();
            SettingsGroup1Panel.SuspendLayout();
            MetaDataDefaultKeysTable.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)PictureBoxToolTipMetaFileName).BeginInit();
            MetaDataTabPageTags.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)InfoIconTooltip).BeginInit();
            ItemEditorToolBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)SettingsValidationErrorProvider).BeginInit();
            SuspendLayout();
            // 
            // SettingsTablePanel
            // 
            SettingsTablePanel.ColumnCount = 3;
            SettingsTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            SettingsTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            SettingsTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 132F));
            SettingsTablePanel.Controls.Add(ButtonOk, 1, 2);
            SettingsTablePanel.Controls.Add(HeaderPanel, 0, 0);
            SettingsTablePanel.Controls.Add(SettingsSectionList, 0, 1);
            SettingsTablePanel.Controls.Add(ButtonCancel, 2, 2);
            SettingsTablePanel.Controls.Add(SettingsContentPanel, 1, 1);
            SettingsTablePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            SettingsTablePanel.Location = new System.Drawing.Point(0, 0);
            SettingsTablePanel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            SettingsTablePanel.Name = "SettingsTablePanel";
            SettingsTablePanel.RowCount = 3;
            SettingsTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            SettingsTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            SettingsTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 66F));
            SettingsTablePanel.Size = new System.Drawing.Size(800, 649);
            SettingsTablePanel.TabIndex = 0;
            // 
            // ButtonOk
            // 
            ButtonOk.Anchor = System.Windows.Forms.AnchorStyles.Right;
            ButtonOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            ButtonOk.Location = new System.Drawing.Point(555, 595);
            ButtonOk.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            ButtonOk.Name = "ButtonOk";
            ButtonOk.Size = new System.Drawing.Size(110, 42);
            ButtonOk.TabIndex = 2;
            ButtonOk.Text = "Ok";
            ButtonOk.UseVisualStyleBackColor = true;
            ButtonOk.Click += ButtonOk_Click;
            // 
            // HeaderPanel
            // 
            HeaderPanel.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            HeaderPanel.BackColor = System.Drawing.Color.White;
            SettingsTablePanel.SetColumnSpan(HeaderPanel, 3);
            HeaderPanel.Controls.Add(HeaderLabel);
            HeaderPanel.Controls.Add(pictureBox1);
            HeaderPanel.Location = new System.Drawing.Point(3, 2);
            HeaderPanel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            HeaderPanel.Name = "HeaderPanel";
            HeaderPanel.Size = new System.Drawing.Size(794, 95);
            HeaderPanel.TabIndex = 0;
            // 
            // HeaderLabel
            // 
            HeaderLabel.AutoSize = true;
            HeaderLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            HeaderLabel.Location = new System.Drawing.Point(116, 34);
            HeaderLabel.Name = "HeaderLabel";
            HeaderLabel.Size = new System.Drawing.Size(111, 28);
            HeaderLabel.TabIndex = 1;
            HeaderLabel.Text = "Preferences";
            // 
            // pictureBox1
            // 
            pictureBox1.Image = Properties.Resources.window_gear_large;
            pictureBox1.Location = new System.Drawing.Point(24, 18);
            pictureBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new System.Drawing.Size(59, 69);
            pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // SettingsSectionList
            // 
            SettingsSectionList.Dock = System.Windows.Forms.DockStyle.Fill;
            SettingsSectionList.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            SettingsSectionList.FormattingEnabled = true;
            SettingsSectionList.ItemHeight = 21;
            SettingsSectionList.Items.AddRange(new object[] { "Meta Data", "Application", "CBZ", "Image Processing" });
            SettingsSectionList.Location = new System.Drawing.Point(3, 102);
            SettingsSectionList.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            SettingsSectionList.Name = "SettingsSectionList";
            SettingsSectionList.Size = new System.Drawing.Size(194, 479);
            SettingsSectionList.TabIndex = 1;
            SettingsSectionList.SelectedIndexChanged += SettingsSectionList_SelectedIndexChanged;
            // 
            // ButtonCancel
            // 
            ButtonCancel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            ButtonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            ButtonCancel.Location = new System.Drawing.Point(675, 595);
            ButtonCancel.Margin = new System.Windows.Forms.Padding(3, 2, 6, 2);
            ButtonCancel.Name = "ButtonCancel";
            ButtonCancel.Size = new System.Drawing.Size(119, 42);
            ButtonCancel.TabIndex = 3;
            ButtonCancel.Text = "Cancel";
            ButtonCancel.UseVisualStyleBackColor = true;
            ButtonCancel.Click += ButtonCancel_Click;
            // 
            // SettingsContentPanel
            // 
            SettingsContentPanel.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            SettingsTablePanel.SetColumnSpan(SettingsContentPanel, 2);
            SettingsContentPanel.Controls.Add(AppSettingsTabControl);
            SettingsContentPanel.Controls.Add(CBZSettingsTabControl);
            SettingsContentPanel.Controls.Add(ImageProcessingTabControl);
            SettingsContentPanel.Controls.Add(MetaDataConfigTabControl);
            SettingsContentPanel.Location = new System.Drawing.Point(204, 105);
            SettingsContentPanel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            SettingsContentPanel.Name = "SettingsContentPanel";
            SettingsContentPanel.Size = new System.Drawing.Size(592, 473);
            SettingsContentPanel.TabIndex = 2;
            // 
            // AppSettingsTabControl
            // 
            AppSettingsTabControl.Controls.Add(TabPageAppSettings);
            AppSettingsTabControl.Controls.Add(tabPage5);
            AppSettingsTabControl.Location = new System.Drawing.Point(314, 2);
            AppSettingsTabControl.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            AppSettingsTabControl.Name = "AppSettingsTabControl";
            AppSettingsTabControl.SelectedIndex = 0;
            AppSettingsTabControl.Size = new System.Drawing.Size(104, 460);
            AppSettingsTabControl.TabIndex = 3;
            // 
            // TabPageAppSettings
            // 
            TabPageAppSettings.Controls.Add(CustomFieldTypesTablePanel);
            TabPageAppSettings.Location = new System.Drawing.Point(4, 29);
            TabPageAppSettings.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            TabPageAppSettings.Name = "TabPageAppSettings";
            TabPageAppSettings.Padding = new System.Windows.Forms.Padding(3, 5, 3, 5);
            TabPageAppSettings.Size = new System.Drawing.Size(96, 427);
            TabPageAppSettings.TabIndex = 0;
            TabPageAppSettings.Text = "MetaData Editor";
            TabPageAppSettings.UseVisualStyleBackColor = true;
            // 
            // CustomFieldTypesTablePanel
            // 
            CustomFieldTypesTablePanel.ColumnCount = 3;
            CustomFieldTypesTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 28.57143F));
            CustomFieldTypesTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 28.57143F));
            CustomFieldTypesTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 42.85714F));
            CustomFieldTypesTablePanel.Controls.Add(CustomFieldsDataGrid, 0, 1);
            CustomFieldTypesTablePanel.Controls.Add(AddFieldTypeButton, 0, 2);
            CustomFieldTypesTablePanel.Controls.Add(RemoveFieldTypeButton, 1, 2);
            CustomFieldTypesTablePanel.Controls.Add(RestoreFieldTypesButton, 2, 2);
            CustomFieldTypesTablePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            CustomFieldTypesTablePanel.Location = new System.Drawing.Point(3, 5);
            CustomFieldTypesTablePanel.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            CustomFieldTypesTablePanel.Name = "CustomFieldTypesTablePanel";
            CustomFieldTypesTablePanel.RowCount = 3;
            CustomFieldTypesTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 49F));
            CustomFieldTypesTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            CustomFieldTypesTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 54F));
            CustomFieldTypesTablePanel.Size = new System.Drawing.Size(90, 417);
            CustomFieldTypesTablePanel.TabIndex = 1;
            // 
            // CustomFieldsDataGrid
            // 
            CustomFieldsDataGrid.AllowUserToAddRows = false;
            CustomFieldsDataGrid.AllowUserToDeleteRows = false;
            CustomFieldsDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            CustomFieldTypesTablePanel.SetColumnSpan(CustomFieldsDataGrid, 3);
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.Gold;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            CustomFieldsDataGrid.DefaultCellStyle = dataGridViewCellStyle1;
            CustomFieldsDataGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            CustomFieldsDataGrid.Location = new System.Drawing.Point(3, 54);
            CustomFieldsDataGrid.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            CustomFieldsDataGrid.MultiSelect = false;
            CustomFieldsDataGrid.Name = "CustomFieldsDataGrid";
            CustomFieldsDataGrid.RowHeadersWidth = 51;
            CustomFieldsDataGrid.RowTemplate.Height = 24;
            CustomFieldsDataGrid.Size = new System.Drawing.Size(84, 304);
            CustomFieldsDataGrid.TabIndex = 0;
            CustomFieldsDataGrid.CellContentClick += CustomFieldsDataGrid_CellContentClick;
            CustomFieldsDataGrid.CellValueChanged += CustomFieldsDataGrid_CellValueChanged;
            CustomFieldsDataGrid.DataError += CustomFieldsDataGrid_DataError;
            CustomFieldsDataGrid.SelectionChanged += CustomFieldsDataGrid_SelectionChanged;
            // 
            // AddFieldTypeButton
            // 
            AddFieldTypeButton.Dock = System.Windows.Forms.DockStyle.Fill;
            AddFieldTypeButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            AddFieldTypeButton.ImageIndex = 0;
            AddFieldTypeButton.ImageList = DialogImages;
            AddFieldTypeButton.Location = new System.Drawing.Point(3, 371);
            AddFieldTypeButton.Margin = new System.Windows.Forms.Padding(3, 8, 3, 4);
            AddFieldTypeButton.Name = "AddFieldTypeButton";
            AddFieldTypeButton.Size = new System.Drawing.Size(19, 42);
            AddFieldTypeButton.TabIndex = 1;
            AddFieldTypeButton.Text = "Add";
            AddFieldTypeButton.UseVisualStyleBackColor = true;
            AddFieldTypeButton.Click += AddFieldTypeButton_Click;
            // 
            // DialogImages
            // 
            DialogImages.ColorDepth = System.Windows.Forms.ColorDepth.Depth24Bit;
            DialogImages.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("DialogImages.ImageStream");
            DialogImages.TransparentColor = System.Drawing.Color.Transparent;
            DialogImages.Images.SetKeyName(0, "navigate_plus.png");
            DialogImages.Images.SetKeyName(1, "delete.png");
            // 
            // RemoveFieldTypeButton
            // 
            RemoveFieldTypeButton.Dock = System.Windows.Forms.DockStyle.Fill;
            RemoveFieldTypeButton.Enabled = false;
            RemoveFieldTypeButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            RemoveFieldTypeButton.ImageIndex = 1;
            RemoveFieldTypeButton.ImageList = DialogImages;
            RemoveFieldTypeButton.Location = new System.Drawing.Point(28, 371);
            RemoveFieldTypeButton.Margin = new System.Windows.Forms.Padding(3, 8, 3, 4);
            RemoveFieldTypeButton.Name = "RemoveFieldTypeButton";
            RemoveFieldTypeButton.Size = new System.Drawing.Size(19, 42);
            RemoveFieldTypeButton.TabIndex = 2;
            RemoveFieldTypeButton.Text = "Remove";
            RemoveFieldTypeButton.UseVisualStyleBackColor = true;
            RemoveFieldTypeButton.Click += RemoveFieldTypeButton_Click;
            // 
            // RestoreFieldTypesButton
            // 
            RestoreFieldTypesButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            RestoreFieldTypesButton.Location = new System.Drawing.Point(53, 371);
            RestoreFieldTypesButton.Margin = new System.Windows.Forms.Padding(3, 8, 3, 4);
            RestoreFieldTypesButton.Name = "RestoreFieldTypesButton";
            RestoreFieldTypesButton.Size = new System.Drawing.Size(34, 42);
            RestoreFieldTypesButton.TabIndex = 3;
            RestoreFieldTypesButton.Text = "Restore";
            RestoreFieldTypesButton.UseVisualStyleBackColor = true;
            RestoreFieldTypesButton.Click += RestoreFieldTypesButton_Click;
            // 
            // tabPage5
            // 
            tabPage5.Controls.Add(tableLayoutPanel4);
            tabPage5.Location = new System.Drawing.Point(4, 29);
            tabPage5.Margin = new System.Windows.Forms.Padding(2);
            tabPage5.Name = "tabPage5";
            tabPage5.Padding = new System.Windows.Forms.Padding(2);
            tabPage5.Size = new System.Drawing.Size(96, 427);
            tabPage5.TabIndex = 2;
            tabPage5.Text = "Behaviour";
            tabPage5.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel4
            // 
            tableLayoutPanel4.ColumnCount = 3;
            tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 153F));
            tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 7F));
            tableLayoutPanel4.Controls.Add(CheckBoxSkipIndexCheck, 1, 0);
            tableLayoutPanel4.Controls.Add(label9, 0, 0);
            tableLayoutPanel4.Controls.Add(label7, 0, 1);
            tableLayoutPanel4.Controls.Add(CheckBoxDeleteTempFiles, 1, 1);
            tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanel4.Location = new System.Drawing.Point(2, 2);
            tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            tableLayoutPanel4.Name = "tableLayoutPanel4";
            tableLayoutPanel4.RowCount = 4;
            tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 60.60606F));
            tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 59F));
            tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 59F));
            tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 248F));
            tableLayoutPanel4.Size = new System.Drawing.Size(92, 423);
            tableLayoutPanel4.TabIndex = 2;
            // 
            // CheckBoxSkipIndexCheck
            // 
            CheckBoxSkipIndexCheck.AutoSize = true;
            CheckBoxSkipIndexCheck.Location = new System.Drawing.Point(162, 20);
            CheckBoxSkipIndexCheck.Margin = new System.Windows.Forms.Padding(9, 20, 3, 2);
            CheckBoxSkipIndexCheck.Name = "CheckBoxSkipIndexCheck";
            CheckBoxSkipIndexCheck.Size = new System.Drawing.Size(1, 24);
            CheckBoxSkipIndexCheck.TabIndex = 8;
            CheckBoxSkipIndexCheck.Text = "Skip index check";
            CheckBoxSkipIndexCheck.UseVisualStyleBackColor = true;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new System.Drawing.Point(18, 20);
            label9.Margin = new System.Windows.Forms.Padding(18, 20, 18, 20);
            label9.Name = "label9";
            label9.Size = new System.Drawing.Size(45, 17);
            label9.TabIndex = 9;
            label9.Text = "Index";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new System.Drawing.Point(18, 77);
            label7.Margin = new System.Windows.Forms.Padding(18, 20, 3, 0);
            label7.Name = "label7";
            label7.Size = new System.Drawing.Size(113, 20);
            label7.TabIndex = 10;
            label7.Text = "Temporary Files";
            // 
            // CheckBoxDeleteTempFiles
            // 
            CheckBoxDeleteTempFiles.AutoSize = true;
            CheckBoxDeleteTempFiles.Location = new System.Drawing.Point(162, 77);
            CheckBoxDeleteTempFiles.Margin = new System.Windows.Forms.Padding(9, 20, 3, 4);
            CheckBoxDeleteTempFiles.Name = "CheckBoxDeleteTempFiles";
            CheckBoxDeleteTempFiles.Size = new System.Drawing.Size(1, 24);
            CheckBoxDeleteTempFiles.TabIndex = 11;
            CheckBoxDeleteTempFiles.Text = "Delete Imediately";
            CheckBoxDeleteTempFiles.UseVisualStyleBackColor = true;
            // 
            // CBZSettingsTabControl
            // 
            CBZSettingsTabControl.Controls.Add(tabPage2);
            CBZSettingsTabControl.Location = new System.Drawing.Point(130, 5);
            CBZSettingsTabControl.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            CBZSettingsTabControl.Name = "CBZSettingsTabControl";
            CBZSettingsTabControl.SelectedIndex = 0;
            CBZSettingsTabControl.Size = new System.Drawing.Size(146, 460);
            CBZSettingsTabControl.TabIndex = 4;
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(tableLayoutPanel2);
            tabPage2.Location = new System.Drawing.Point(4, 29);
            tabPage2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            tabPage2.Name = "tabPage2";
            tabPage2.Size = new System.Drawing.Size(138, 427);
            tabPage2.TabIndex = 0;
            tabPage2.Text = "Compatibility";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 3;
            tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 180F));
            tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 57F));
            tableLayoutPanel2.Controls.Add(label5, 0, 0);
            tableLayoutPanel2.Controls.Add(ComboBoxPageIndexVersionWrite, 1, 0);
            tableLayoutPanel2.Controls.Add(pictureBox2, 2, 0);
            tableLayoutPanel2.Controls.Add(CheckBoxPruneEmplyTags, 1, 1);
            tableLayoutPanel2.Controls.Add(label6, 0, 1);
            tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 4;
            tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 60.60606F));
            tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 59F));
            tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 59F));
            tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 248F));
            tableLayoutPanel2.Size = new System.Drawing.Size(138, 427);
            tableLayoutPanel2.TabIndex = 0;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new System.Drawing.Point(18, 20);
            label5.Margin = new System.Windows.Forms.Padding(18, 20, 18, 20);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(98, 21);
            label5.TabIndex = 0;
            label5.Text = "Meta Format -Version";
            // 
            // ComboBoxPageIndexVersionWrite
            // 
            ComboBoxPageIndexVersionWrite.Dock = System.Windows.Forms.DockStyle.Fill;
            ComboBoxPageIndexVersionWrite.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            ComboBoxPageIndexVersionWrite.Enabled = false;
            ComboBoxPageIndexVersionWrite.FormattingEnabled = true;
            ComboBoxPageIndexVersionWrite.Items.AddRange(new object[] { "VERSION_1", "VERSION_2" });
            ComboBoxPageIndexVersionWrite.Location = new System.Drawing.Point(189, 20);
            ComboBoxPageIndexVersionWrite.Margin = new System.Windows.Forms.Padding(9, 20, 18, 20);
            ComboBoxPageIndexVersionWrite.Name = "ComboBoxPageIndexVersionWrite";
            ComboBoxPageIndexVersionWrite.Size = new System.Drawing.Size(1, 28);
            ComboBoxPageIndexVersionWrite.TabIndex = 1;
            // 
            // pictureBox2
            // 
            pictureBox2.Image = Properties.Resources.information;
            pictureBox2.InitialImage = Properties.Resources.information;
            pictureBox2.Location = new System.Drawing.Point(81, 0);
            pictureBox2.Margin = new System.Windows.Forms.Padding(0);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Padding = new System.Windows.Forms.Padding(7, 20, 7, 8);
            pictureBox2.Size = new System.Drawing.Size(38, 52);
            pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            pictureBox2.TabIndex = 7;
            pictureBox2.TabStop = false;
            TagValidationTooltip.SetToolTip(pictureBox2, resources.GetString("pictureBox2.ToolTip"));
            // 
            // CheckBoxPruneEmplyTags
            // 
            CheckBoxPruneEmplyTags.AutoSize = true;
            CheckBoxPruneEmplyTags.Location = new System.Drawing.Point(189, 81);
            CheckBoxPruneEmplyTags.Margin = new System.Windows.Forms.Padding(9, 20, 3, 2);
            CheckBoxPruneEmplyTags.Name = "CheckBoxPruneEmplyTags";
            CheckBoxPruneEmplyTags.Size = new System.Drawing.Size(1, 24);
            CheckBoxPruneEmplyTags.TabIndex = 8;
            CheckBoxPruneEmplyTags.Text = "Omit empty XML- Tags";
            CheckBoxPruneEmplyTags.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new System.Drawing.Point(18, 81);
            label6.Margin = new System.Windows.Forms.Padding(18, 20, 18, 20);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(84, 19);
            label6.TabIndex = 9;
            label6.Text = "Empty Tags";
            // 
            // ImageProcessingTabControl
            // 
            ImageProcessingTabControl.Controls.Add(ImageConversionTabPage);
            ImageProcessingTabControl.Controls.Add(ImageProcessingTabPage);
            ImageProcessingTabControl.Location = new System.Drawing.Point(432, 5);
            ImageProcessingTabControl.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            ImageProcessingTabControl.Name = "ImageProcessingTabControl";
            ImageProcessingTabControl.SelectedIndex = 0;
            ImageProcessingTabControl.Size = new System.Drawing.Size(151, 458);
            ImageProcessingTabControl.TabIndex = 2;
            // 
            // ImageConversionTabPage
            // 
            ImageConversionTabPage.Controls.Add(LabelConvertImages);
            ImageConversionTabPage.Controls.Add(ComboBoxConvertPages);
            ImageConversionTabPage.Controls.Add(GroupBoxImageQuality);
            ImageConversionTabPage.Location = new System.Drawing.Point(4, 29);
            ImageConversionTabPage.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            ImageConversionTabPage.Name = "ImageConversionTabPage";
            ImageConversionTabPage.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            ImageConversionTabPage.Size = new System.Drawing.Size(143, 425);
            ImageConversionTabPage.TabIndex = 0;
            ImageConversionTabPage.Text = "Image Conversion";
            ImageConversionTabPage.UseVisualStyleBackColor = true;
            // 
            // LabelConvertImages
            // 
            LabelConvertImages.Anchor = System.Windows.Forms.AnchorStyles.Left;
            LabelConvertImages.AutoSize = true;
            LabelConvertImages.Location = new System.Drawing.Point(8, 55);
            LabelConvertImages.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            LabelConvertImages.Name = "LabelConvertImages";
            LabelConvertImages.Size = new System.Drawing.Size(112, 20);
            LabelConvertImages.TabIndex = 22;
            LabelConvertImages.Text = "Convert Images";
            // 
            // ComboBoxConvertPages
            // 
            ComboBoxConvertPages.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            ComboBoxConvertPages.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            ComboBoxConvertPages.FlatStyle = System.Windows.Forms.FlatStyle.System;
            ComboBoxConvertPages.FormattingEnabled = true;
            ComboBoxConvertPages.Items.AddRange(new object[] { "Dont Convert, keep original Format", "Bitmap", "Jpeg", "PNG", "Tiff" });
            ComboBoxConvertPages.Location = new System.Drawing.Point(8, 80);
            ComboBoxConvertPages.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            ComboBoxConvertPages.Name = "ComboBoxConvertPages";
            ComboBoxConvertPages.Size = new System.Drawing.Size(107, 28);
            ComboBoxConvertPages.TabIndex = 23;
            // 
            // GroupBoxImageQuality
            // 
            GroupBoxImageQuality.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            GroupBoxImageQuality.Controls.Add(ImageQualitySliderMaxLabel);
            GroupBoxImageQuality.Controls.Add(ImageQualitySliderMinLabel);
            GroupBoxImageQuality.Controls.Add(ImageQualityTrackBar);
            GroupBoxImageQuality.Location = new System.Drawing.Point(11, 168);
            GroupBoxImageQuality.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            GroupBoxImageQuality.Name = "GroupBoxImageQuality";
            GroupBoxImageQuality.Padding = new System.Windows.Forms.Padding(4, 2, 4, 2);
            GroupBoxImageQuality.Size = new System.Drawing.Size(112, 134);
            GroupBoxImageQuality.TabIndex = 21;
            GroupBoxImageQuality.TabStop = false;
            GroupBoxImageQuality.Text = "Image Quality";
            // 
            // ImageQualitySliderMaxLabel
            // 
            ImageQualitySliderMaxLabel.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            ImageQualitySliderMaxLabel.AutoSize = true;
            ImageQualitySliderMaxLabel.Location = new System.Drawing.Point(-21, 102);
            ImageQualitySliderMaxLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            ImageQualitySliderMaxLabel.Name = "ImageQualitySliderMaxLabel";
            ImageQualitySliderMaxLabel.Size = new System.Drawing.Size(41, 20);
            ImageQualitySliderMaxLabel.TabIndex = 5;
            ImageQualitySliderMaxLabel.Text = "High";
            // 
            // ImageQualitySliderMinLabel
            // 
            ImageQualitySliderMinLabel.AutoSize = true;
            ImageQualitySliderMinLabel.Location = new System.Drawing.Point(9, 102);
            ImageQualitySliderMinLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            ImageQualitySliderMinLabel.Name = "ImageQualitySliderMinLabel";
            ImageQualitySliderMinLabel.Size = new System.Drawing.Size(36, 20);
            ImageQualitySliderMinLabel.TabIndex = 4;
            ImageQualitySliderMinLabel.Text = "Low";
            // 
            // ImageQualityTrackBar
            // 
            ImageQualityTrackBar.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            ImageQualityTrackBar.BackColor = System.Drawing.SystemColors.Window;
            ImageQualityTrackBar.Enabled = false;
            ImageQualityTrackBar.Location = new System.Drawing.Point(14, 28);
            ImageQualityTrackBar.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            ImageQualityTrackBar.Maximum = 100;
            ImageQualityTrackBar.Minimum = 10;
            ImageQualityTrackBar.Name = "ImageQualityTrackBar";
            ImageQualityTrackBar.Size = new System.Drawing.Size(93, 56);
            ImageQualityTrackBar.TabIndex = 3;
            ImageQualityTrackBar.Value = 85;
            // 
            // ImageProcessingTabPage
            // 
            ImageProcessingTabPage.Location = new System.Drawing.Point(4, 29);
            ImageProcessingTabPage.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            ImageProcessingTabPage.Name = "ImageProcessingTabPage";
            ImageProcessingTabPage.Padding = new System.Windows.Forms.Padding(3, 5, 3, 5);
            ImageProcessingTabPage.Size = new System.Drawing.Size(143, 425);
            ImageProcessingTabPage.TabIndex = 1;
            ImageProcessingTabPage.Text = "Image Processing";
            ImageProcessingTabPage.UseVisualStyleBackColor = true;
            // 
            // MetaDataConfigTabControl
            // 
            MetaDataConfigTabControl.Controls.Add(tabPage1);
            MetaDataConfigTabControl.Controls.Add(MetaDataTabPageTags);
            MetaDataConfigTabControl.Location = new System.Drawing.Point(3, 5);
            MetaDataConfigTabControl.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            MetaDataConfigTabControl.Name = "MetaDataConfigTabControl";
            MetaDataConfigTabControl.SelectedIndex = 0;
            MetaDataConfigTabControl.Size = new System.Drawing.Size(122, 465);
            MetaDataConfigTabControl.TabIndex = 1;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(SettingsGroup1Panel);
            tabPage1.Location = new System.Drawing.Point(4, 29);
            tabPage1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            tabPage1.Size = new System.Drawing.Size(114, 432);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Default";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // SettingsGroup1Panel
            // 
            SettingsGroup1Panel.Controls.Add(MetaDataDefaultKeysTable);
            SettingsGroup1Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            SettingsGroup1Panel.Location = new System.Drawing.Point(3, 2);
            SettingsGroup1Panel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            SettingsGroup1Panel.Name = "SettingsGroup1Panel";
            SettingsGroup1Panel.Size = new System.Drawing.Size(108, 428);
            SettingsGroup1Panel.TabIndex = 4;
            // 
            // MetaDataDefaultKeysTable
            // 
            MetaDataDefaultKeysTable.ColumnCount = 3;
            MetaDataDefaultKeysTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 27.52809F));
            MetaDataDefaultKeysTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 72.47191F));
            MetaDataDefaultKeysTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 131F));
            MetaDataDefaultKeysTable.Controls.Add(label4, 0, 0);
            MetaDataDefaultKeysTable.Controls.Add(label2, 0, 4);
            MetaDataDefaultKeysTable.Controls.Add(button1, 2, 3);
            MetaDataDefaultKeysTable.Controls.Add(PictureBoxToolTipMetaFileName, 2, 0);
            MetaDataDefaultKeysTable.Controls.Add(ComboBoxFileName, 1, 0);
            MetaDataDefaultKeysTable.Controls.Add(CustomDefaultKeys, 0, 2);
            MetaDataDefaultKeysTable.Controls.Add(label1, 0, 1);
            MetaDataDefaultKeysTable.Dock = System.Windows.Forms.DockStyle.Fill;
            MetaDataDefaultKeysTable.Location = new System.Drawing.Point(0, 0);
            MetaDataDefaultKeysTable.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            MetaDataDefaultKeysTable.Name = "MetaDataDefaultKeysTable";
            MetaDataDefaultKeysTable.RowCount = 5;
            MetaDataDefaultKeysTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            MetaDataDefaultKeysTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            MetaDataDefaultKeysTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
            MetaDataDefaultKeysTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 68F));
            MetaDataDefaultKeysTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 78F));
            MetaDataDefaultKeysTable.Size = new System.Drawing.Size(108, 428);
            MetaDataDefaultKeysTable.TabIndex = 7;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(3, 0);
            label4.Name = "label4";
            label4.Padding = new System.Windows.Forms.Padding(0, 10, 4, 0);
            label4.Size = new System.Drawing.Size(1, 30);
            label4.TabIndex = 5;
            label4.Text = "Filename:";
            // 
            // label2
            // 
            label2.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            MetaDataDefaultKeysTable.SetColumnSpan(label2, 2);
            label2.Location = new System.Drawing.Point(3, 356);
            label2.Name = "label2";
            label2.Padding = new System.Windows.Forms.Padding(0, 0, 0, 5);
            label2.Size = new System.Drawing.Size(1, 72);
            label2.TabIndex = 2;
            label2.Text = "One Key per Line\r\nTo set a default value for a given key use <key>=<value> format";
            // 
            // button1
            // 
            button1.Dock = System.Windows.Forms.DockStyle.Fill;
            button1.Location = new System.Drawing.Point(-19, 297);
            button1.Margin = new System.Windows.Forms.Padding(3, 15, 6, 2);
            button1.Name = "button1";
            button1.Size = new System.Drawing.Size(122, 51);
            button1.TabIndex = 3;
            button1.Text = "Fill Predifined";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // PictureBoxToolTipMetaFileName
            // 
            PictureBoxToolTipMetaFileName.Image = Properties.Resources.information;
            PictureBoxToolTipMetaFileName.InitialImage = Properties.Resources.information;
            PictureBoxToolTipMetaFileName.Location = new System.Drawing.Point(-22, 0);
            PictureBoxToolTipMetaFileName.Margin = new System.Windows.Forms.Padding(0);
            PictureBoxToolTipMetaFileName.Name = "PictureBoxToolTipMetaFileName";
            PictureBoxToolTipMetaFileName.Padding = new System.Windows.Forms.Padding(7, 8, 7, 8);
            PictureBoxToolTipMetaFileName.Size = new System.Drawing.Size(38, 40);
            PictureBoxToolTipMetaFileName.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            PictureBoxToolTipMetaFileName.TabIndex = 6;
            PictureBoxToolTipMetaFileName.TabStop = false;
            TagValidationTooltip.SetToolTip(PictureBoxToolTipMetaFileName, "Should always be \"ComicInfo.xml\". \r\nThis option sets the name of the Metadata- File within the Archive and can be changed here for more flexibility.");
            // 
            // ComboBoxFileName
            // 
            ComboBoxFileName.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            ComboBoxFileName.Items.AddRange(new object[] { "ComicInfo.xml" });
            ComboBoxFileName.Location = new System.Drawing.Point(-3, 10);
            ComboBoxFileName.Margin = new System.Windows.Forms.Padding(3, 10, 20, 5);
            ComboBoxFileName.Name = "ComboBoxFileName";
            ComboBoxFileName.Size = new System.Drawing.Size(1, 28);
            ComboBoxFileName.TabIndex = 4;
            // 
            // CustomDefaultKeys
            // 
            CustomDefaultKeys.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            MetaDataDefaultKeysTable.SetColumnSpan(CustomDefaultKeys, 3);
            CustomDefaultKeys.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            CustomDefaultKeys.Location = new System.Drawing.Point(6, 108);
            CustomDefaultKeys.Margin = new System.Windows.Forms.Padding(6, 2, 6, 2);
            CustomDefaultKeys.Multiline = true;
            CustomDefaultKeys.Name = "CustomDefaultKeys";
            CustomDefaultKeys.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            CustomDefaultKeys.Size = new System.Drawing.Size(97, 172);
            CustomDefaultKeys.TabIndex = 0;
            // 
            // label1
            // 
            label1.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            label1.AutoSize = true;
            MetaDataDefaultKeysTable.SetColumnSpan(label1, 2);
            label1.Location = new System.Drawing.Point(3, 78);
            label1.Name = "label1";
            label1.Padding = new System.Windows.Forms.Padding(0, 0, 0, 8);
            label1.Size = new System.Drawing.Size(1, 28);
            label1.TabIndex = 1;
            label1.Text = "Default MetaData Keys";
            // 
            // MetaDataTabPageTags
            // 
            MetaDataTabPageTags.Controls.Add(tableLayoutPanel1);
            MetaDataTabPageTags.Location = new System.Drawing.Point(4, 29);
            MetaDataTabPageTags.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            MetaDataTabPageTags.Name = "MetaDataTabPageTags";
            MetaDataTabPageTags.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            MetaDataTabPageTags.Size = new System.Drawing.Size(114, 432);
            MetaDataTabPageTags.TabIndex = 1;
            MetaDataTabPageTags.Text = "Tags";
            MetaDataTabPageTags.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            tableLayoutPanel1.Controls.Add(CheckBoxValidateTags, 0, 0);
            tableLayoutPanel1.Controls.Add(InfoIconTooltip, 1, 0);
            tableLayoutPanel1.Controls.Add(CheckBoxTagValidationIgnoreCase, 0, 1);
            tableLayoutPanel1.Controls.Add(label3, 0, 4);
            tableLayoutPanel1.Controls.Add(ValidTags, 0, 3);
            tableLayoutPanel1.Controls.Add(ItemEditorToolBar, 1, 1);
            tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanel1.Location = new System.Drawing.Point(3, 2);
            tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 5;
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 72F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            tableLayoutPanel1.Size = new System.Drawing.Size(108, 428);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // CheckBoxValidateTags
            // 
            CheckBoxValidateTags.AutoSize = true;
            CheckBoxValidateTags.Location = new System.Drawing.Point(3, 2);
            CheckBoxValidateTags.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            CheckBoxValidateTags.Name = "CheckBoxValidateTags";
            CheckBoxValidateTags.Padding = new System.Windows.Forms.Padding(5, 9, 0, 0);
            CheckBoxValidateTags.Size = new System.Drawing.Size(42, 33);
            CheckBoxValidateTags.TabIndex = 0;
            CheckBoxValidateTags.Text = "Validate Tags against a list of known Tags";
            CheckBoxValidateTags.UseVisualStyleBackColor = true;
            CheckBoxValidateTags.CheckStateChanged += CheckBoxValidateTags_CheckStateChanged;
            // 
            // InfoIconTooltip
            // 
            InfoIconTooltip.Image = Properties.Resources.information;
            InfoIconTooltip.InitialImage = Properties.Resources.information;
            InfoIconTooltip.Location = new System.Drawing.Point(48, 0);
            InfoIconTooltip.Margin = new System.Windows.Forms.Padding(0);
            InfoIconTooltip.Name = "InfoIconTooltip";
            InfoIconTooltip.Padding = new System.Windows.Forms.Padding(7, 9, 7, 8);
            tableLayoutPanel1.SetRowSpan(InfoIconTooltip, 2);
            InfoIconTooltip.Size = new System.Drawing.Size(38, 41);
            InfoIconTooltip.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            InfoIconTooltip.TabIndex = 5;
            InfoIconTooltip.TabStop = false;
            TagValidationTooltip.SetToolTip(InfoIconTooltip, "This options allow you, to validate matadata tags against your own list of valid tags,\r\npreventing typos, duplicate- and invalid tags, from being generated/shown within applications.\r\n");
            // 
            // CheckBoxTagValidationIgnoreCase
            // 
            CheckBoxTagValidationIgnoreCase.AutoSize = true;
            CheckBoxTagValidationIgnoreCase.Enabled = false;
            CheckBoxTagValidationIgnoreCase.Location = new System.Drawing.Point(4, 55);
            CheckBoxTagValidationIgnoreCase.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            CheckBoxTagValidationIgnoreCase.Name = "CheckBoxTagValidationIgnoreCase";
            CheckBoxTagValidationIgnoreCase.Padding = new System.Windows.Forms.Padding(24, 6, 0, 0);
            CheckBoxTagValidationIgnoreCase.Size = new System.Drawing.Size(40, 6);
            CheckBoxTagValidationIgnoreCase.TabIndex = 6;
            CheckBoxTagValidationIgnoreCase.Text = "Case Sensitive";
            CheckBoxTagValidationIgnoreCase.UseVisualStyleBackColor = true;
            CheckBoxTagValidationIgnoreCase.Visible = false;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(3, 383);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(40, 45);
            label3.TabIndex = 4;
            label3.Text = "One Tag per Line";
            // 
            // ValidTags
            // 
            tableLayoutPanel1.SetColumnSpan(ValidTags, 2);
            ValidTags.Dock = System.Windows.Forms.DockStyle.Fill;
            ValidTags.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            ValidTags.HideSelection = false;
            ValidTags.Location = new System.Drawing.Point(3, 140);
            ValidTags.Margin = new System.Windows.Forms.Padding(3, 2, 3, 10);
            ValidTags.Multiline = true;
            ValidTags.Name = "ValidTags";
            ValidTags.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            ValidTags.Size = new System.Drawing.Size(102, 233);
            ValidTags.TabIndex = 3;
            // 
            // ItemEditorToolBar
            // 
            ItemEditorToolBar.AllowMerge = false;
            ItemEditorToolBar.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            ItemEditorToolBar.BackColor = System.Drawing.Color.White;
            tableLayoutPanel1.SetColumnSpan(ItemEditorToolBar, 2);
            ItemEditorToolBar.Dock = System.Windows.Forms.DockStyle.None;
            ItemEditorToolBar.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            ItemEditorToolBar.ImageScalingSize = new System.Drawing.Size(20, 20);
            ItemEditorToolBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { ToolStripTextBoxSearchTag, ToolButtonSortAscending });
            ItemEditorToolBar.Location = new System.Drawing.Point(8, 105);
            ItemEditorToolBar.Margin = new System.Windows.Forms.Padding(8, 0, 0, 4);
            ItemEditorToolBar.Name = "ItemEditorToolBar";
            ItemEditorToolBar.Padding = new System.Windows.Forms.Padding(0, 0, 3, 0);
            ItemEditorToolBar.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            ItemEditorToolBar.Size = new System.Drawing.Size(100, 29);
            ItemEditorToolBar.Stretch = true;
            ItemEditorToolBar.TabIndex = 8;
            // 
            // ToolStripTextBoxSearchTag
            // 
            ToolStripTextBoxSearchTag.Name = "ToolStripTextBoxSearchTag";
            ToolStripTextBoxSearchTag.Size = new System.Drawing.Size(150, 27);
            ToolStripTextBoxSearchTag.KeyUp += ToolStripTextBoxSearchTag_KeyUp;
            // 
            // ToolButtonSortAscending
            // 
            ToolButtonSortAscending.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            ToolButtonSortAscending.Image = Properties.Resources.sort_az_ascending2;
            ToolButtonSortAscending.ImageTransparentColor = System.Drawing.Color.Magenta;
            ToolButtonSortAscending.Name = "ToolButtonSortAscending";
            ToolButtonSortAscending.Size = new System.Drawing.Size(29, 24);
            ToolButtonSortAscending.ToolTipText = "Sort items ascending";
            ToolButtonSortAscending.Click += ToolButtonSortAscending_Click;
            // 
            // TagValidationTooltip
            // 
            TagValidationTooltip.AutoPopDelay = 30000;
            TagValidationTooltip.InitialDelay = 200;
            TagValidationTooltip.IsBalloon = true;
            TagValidationTooltip.ReshowDelay = 100;
            TagValidationTooltip.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            TagValidationTooltip.ToolTipTitle = "Win_CBZ";
            // 
            // SettingsValidationErrorProvider
            // 
            SettingsValidationErrorProvider.ContainerControl = this;
            // 
            // SettingsDialog
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(800, 649);
            Controls.Add(SettingsTablePanel);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            Name = "SettingsDialog";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Application Configuration";
            FormClosing += SettingsDialog_FormClosing;
            Load += SettingsDialog_Load;
            Shown += SettingsDialog_Shown;
            SettingsTablePanel.ResumeLayout(false);
            HeaderPanel.ResumeLayout(false);
            HeaderPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            SettingsContentPanel.ResumeLayout(false);
            AppSettingsTabControl.ResumeLayout(false);
            TabPageAppSettings.ResumeLayout(false);
            CustomFieldTypesTablePanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)CustomFieldsDataGrid).EndInit();
            tabPage5.ResumeLayout(false);
            tableLayoutPanel4.ResumeLayout(false);
            tableLayoutPanel4.PerformLayout();
            CBZSettingsTabControl.ResumeLayout(false);
            tabPage2.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ImageProcessingTabControl.ResumeLayout(false);
            ImageConversionTabPage.ResumeLayout(false);
            ImageConversionTabPage.PerformLayout();
            GroupBoxImageQuality.ResumeLayout(false);
            GroupBoxImageQuality.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)ImageQualityTrackBar).EndInit();
            MetaDataConfigTabControl.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            SettingsGroup1Panel.ResumeLayout(false);
            MetaDataDefaultKeysTable.ResumeLayout(false);
            MetaDataDefaultKeysTable.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)PictureBoxToolTipMetaFileName).EndInit();
            MetaDataTabPageTags.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)InfoIconTooltip).EndInit();
            ItemEditorToolBar.ResumeLayout(false);
            ItemEditorToolBar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)SettingsValidationErrorProvider).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel SettingsTablePanel;
        private System.Windows.Forms.Panel HeaderPanel;
        private System.Windows.Forms.ListBox SettingsSectionList;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button ButtonOk;
        private System.Windows.Forms.Button ButtonCancel;
        private System.Windows.Forms.Label HeaderLabel;
        private System.Windows.Forms.Panel SettingsGroup1Panel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox CustomDefaultKeys;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TabControl MetaDataConfigTabControl;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage MetaDataTabPageTags;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.CheckBox CheckBoxValidateTags;
        private System.Windows.Forms.TextBox ValidTags;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.PictureBox InfoIconTooltip;
        private System.Windows.Forms.ToolTip TagValidationTooltip;
        private System.Windows.Forms.CheckBox CheckBoxTagValidationIgnoreCase;
        private System.Windows.Forms.ToolStrip ItemEditorToolBar;
        private System.Windows.Forms.ToolStripButton ToolButtonSortAscending;
        private System.Windows.Forms.Panel SettingsContentPanel;
        private System.Windows.Forms.TabControl ImageProcessingTabControl;
        private System.Windows.Forms.TabPage ImageConversionTabPage;
        private System.Windows.Forms.Label LabelConvertImages;
        private System.Windows.Forms.ComboBox ComboBoxConvertPages;
        private System.Windows.Forms.GroupBox GroupBoxImageQuality;
        private System.Windows.Forms.Label ImageQualitySliderMaxLabel;
        private System.Windows.Forms.Label ImageQualitySliderMinLabel;
        private System.Windows.Forms.TrackBar ImageQualityTrackBar;
        private System.Windows.Forms.TabPage ImageProcessingTabPage;
        private System.Windows.Forms.ErrorProvider SettingsValidationErrorProvider;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox ComboBoxFileName;
        private System.Windows.Forms.PictureBox PictureBoxToolTipMetaFileName;
        private System.Windows.Forms.TabControl AppSettingsTabControl;
        private System.Windows.Forms.TabPage TabPageAppSettings;
        private System.Windows.Forms.TabControl CBZSettingsTabControl;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox ComboBoxPageIndexVersionWrite;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.TableLayoutPanel CustomFieldTypesTablePanel;
        private System.Windows.Forms.DataGridView CustomFieldsDataGrid;
        private System.Windows.Forms.Button AddFieldTypeButton;
        private System.Windows.Forms.Button RemoveFieldTypeButton;
        private System.Windows.Forms.Button RestoreFieldTypesButton;
        private System.Windows.Forms.TableLayoutPanel MetaDataDefaultKeysTable;
        private System.Windows.Forms.ImageList DialogImages;
        private System.Windows.Forms.ToolStripTextBox ToolStripTextBoxSearchTag;
        private System.Windows.Forms.CheckBox CheckBoxPruneEmplyTags;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.CheckBox CheckBoxSkipIndexCheck;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox CheckBoxDeleteTempFiles;
    }
}