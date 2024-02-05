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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsDialog));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
            this.SettingsTablePanel = new System.Windows.Forms.TableLayoutPanel();
            this.ButtonOk = new System.Windows.Forms.Button();
            this.HeaderPanel = new System.Windows.Forms.Panel();
            this.HeaderLabel = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.SettingsSectionList = new System.Windows.Forms.ListBox();
            this.ButtonCancel = new System.Windows.Forms.Button();
            this.SettingsContentPanel = new System.Windows.Forms.Panel();
            this.MetaDataConfigTabControl = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.SettingsGroup1Panel = new System.Windows.Forms.Panel();
            this.PictureBoxToolTipMetaFileName = new System.Windows.Forms.PictureBox();
            this.label4 = new System.Windows.Forms.Label();
            this.ComboBoxFileName = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.CustomDefaultKeys = new System.Windows.Forms.TextBox();
            this.MetaDataTabPageTags = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.CheckBoxValidateTags = new System.Windows.Forms.CheckBox();
            this.InfoIconTooltip = new System.Windows.Forms.PictureBox();
            this.CheckBoxTagValidationIgnoreCase = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.ValidTags = new System.Windows.Forms.TextBox();
            this.ItemEditorToolBar = new System.Windows.Forms.ToolStrip();
            this.ToolButtonSortAscending = new System.Windows.Forms.ToolStripButton();
            this.CBZSettingsTabControl = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.label5 = new System.Windows.Forms.Label();
            this.ComboBoxPageIndexVersionWrite = new System.Windows.Forms.ComboBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.AppSettingsTabControl = new System.Windows.Forms.TabControl();
            this.TabPageAppSettings = new System.Windows.Forms.TabPage();
            this.CustomFieldTypesTablePanel = new System.Windows.Forms.TableLayoutPanel();
            this.CustomFieldsDataGrid = new System.Windows.Forms.DataGridView();
            this.AddFieldTypeButton = new System.Windows.Forms.Button();
            this.RemoveFieldTypeButton = new System.Windows.Forms.Button();
            this.RestoreFieldTypesButton = new System.Windows.Forms.Button();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.ImageProcessingTabControl = new System.Windows.Forms.TabControl();
            this.ImageConversionTabPage = new System.Windows.Forms.TabPage();
            this.LabelConvertImages = new System.Windows.Forms.Label();
            this.ComboBoxConvertPages = new System.Windows.Forms.ComboBox();
            this.GroupBoxImageQuality = new System.Windows.Forms.GroupBox();
            this.ImageQualitySliderMaxLabel = new System.Windows.Forms.Label();
            this.ImageQualitySliderMinLabel = new System.Windows.Forms.Label();
            this.ImageQualityTrackBar = new System.Windows.Forms.TrackBar();
            this.ImageProcessingTabPage = new System.Windows.Forms.TabPage();
            this.TagValidationTooltip = new System.Windows.Forms.ToolTip(this.components);
            this.SettingsValidationErrorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.SettingsTablePanel.SuspendLayout();
            this.HeaderPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SettingsContentPanel.SuspendLayout();
            this.MetaDataConfigTabControl.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SettingsGroup1Panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBoxToolTipMetaFileName)).BeginInit();
            this.MetaDataTabPageTags.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.InfoIconTooltip)).BeginInit();
            this.ItemEditorToolBar.SuspendLayout();
            this.CBZSettingsTabControl.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.AppSettingsTabControl.SuspendLayout();
            this.TabPageAppSettings.SuspendLayout();
            this.CustomFieldTypesTablePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CustomFieldsDataGrid)).BeginInit();
            this.ImageProcessingTabControl.SuspendLayout();
            this.ImageConversionTabPage.SuspendLayout();
            this.GroupBoxImageQuality.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ImageQualityTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SettingsValidationErrorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // SettingsTablePanel
            // 
            this.SettingsTablePanel.ColumnCount = 3;
            this.SettingsTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.SettingsTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.SettingsTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 132F));
            this.SettingsTablePanel.Controls.Add(this.ButtonOk, 1, 2);
            this.SettingsTablePanel.Controls.Add(this.HeaderPanel, 0, 0);
            this.SettingsTablePanel.Controls.Add(this.SettingsSectionList, 0, 1);
            this.SettingsTablePanel.Controls.Add(this.ButtonCancel, 2, 2);
            this.SettingsTablePanel.Controls.Add(this.SettingsContentPanel, 1, 1);
            this.SettingsTablePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SettingsTablePanel.Location = new System.Drawing.Point(0, 0);
            this.SettingsTablePanel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.SettingsTablePanel.Name = "SettingsTablePanel";
            this.SettingsTablePanel.RowCount = 3;
            this.SettingsTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.SettingsTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.SettingsTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 53F));
            this.SettingsTablePanel.Size = new System.Drawing.Size(800, 519);
            this.SettingsTablePanel.TabIndex = 0;
            // 
            // ButtonOk
            // 
            this.ButtonOk.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.ButtonOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.ButtonOk.Location = new System.Drawing.Point(555, 475);
            this.ButtonOk.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ButtonOk.Name = "ButtonOk";
            this.ButtonOk.Size = new System.Drawing.Size(110, 34);
            this.ButtonOk.TabIndex = 2;
            this.ButtonOk.Text = "Ok";
            this.ButtonOk.UseVisualStyleBackColor = true;
            this.ButtonOk.Click += new System.EventHandler(this.ButtonOk_Click);
            // 
            // HeaderPanel
            // 
            this.HeaderPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.HeaderPanel.BackColor = System.Drawing.Color.White;
            this.SettingsTablePanel.SetColumnSpan(this.HeaderPanel, 3);
            this.HeaderPanel.Controls.Add(this.HeaderLabel);
            this.HeaderPanel.Controls.Add(this.pictureBox1);
            this.HeaderPanel.Location = new System.Drawing.Point(3, 2);
            this.HeaderPanel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.HeaderPanel.Name = "HeaderPanel";
            this.HeaderPanel.Size = new System.Drawing.Size(794, 76);
            this.HeaderPanel.TabIndex = 0;
            // 
            // HeaderLabel
            // 
            this.HeaderLabel.AutoSize = true;
            this.HeaderLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HeaderLabel.Location = new System.Drawing.Point(116, 27);
            this.HeaderLabel.Name = "HeaderLabel";
            this.HeaderLabel.Size = new System.Drawing.Size(111, 28);
            this.HeaderLabel.TabIndex = 1;
            this.HeaderLabel.Text = "Preferences";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Win_CBZ.Properties.Resources.window_gear_large;
            this.pictureBox1.Location = new System.Drawing.Point(24, 14);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(59, 55);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // SettingsSectionList
            // 
            this.SettingsSectionList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SettingsSectionList.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SettingsSectionList.FormattingEnabled = true;
            this.SettingsSectionList.ItemHeight = 21;
            this.SettingsSectionList.Items.AddRange(new object[] {
            "Meta Data",
            "Application",
            "CBZ",
            "Image Processing"});
            this.SettingsSectionList.Location = new System.Drawing.Point(3, 82);
            this.SettingsSectionList.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.SettingsSectionList.Name = "SettingsSectionList";
            this.SettingsSectionList.Size = new System.Drawing.Size(194, 382);
            this.SettingsSectionList.TabIndex = 1;
            this.SettingsSectionList.SelectedIndexChanged += new System.EventHandler(this.SettingsSectionList_SelectedIndexChanged);
            // 
            // ButtonCancel
            // 
            this.ButtonCancel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.ButtonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ButtonCancel.Location = new System.Drawing.Point(678, 475);
            this.ButtonCancel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ButtonCancel.Name = "ButtonCancel";
            this.ButtonCancel.Size = new System.Drawing.Size(119, 34);
            this.ButtonCancel.TabIndex = 3;
            this.ButtonCancel.Text = "Cancel";
            this.ButtonCancel.UseVisualStyleBackColor = true;
            this.ButtonCancel.Click += new System.EventHandler(this.ButtonCancel_Click);
            // 
            // SettingsContentPanel
            // 
            this.SettingsContentPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SettingsTablePanel.SetColumnSpan(this.SettingsContentPanel, 2);
            this.SettingsContentPanel.Controls.Add(this.MetaDataConfigTabControl);
            this.SettingsContentPanel.Controls.Add(this.CBZSettingsTabControl);
            this.SettingsContentPanel.Controls.Add(this.AppSettingsTabControl);
            this.SettingsContentPanel.Controls.Add(this.ImageProcessingTabControl);
            this.SettingsContentPanel.Location = new System.Drawing.Point(204, 84);
            this.SettingsContentPanel.Margin = new System.Windows.Forms.Padding(4);
            this.SettingsContentPanel.Name = "SettingsContentPanel";
            this.SettingsContentPanel.Size = new System.Drawing.Size(592, 378);
            this.SettingsContentPanel.TabIndex = 2;
            // 
            // MetaDataConfigTabControl
            // 
            this.MetaDataConfigTabControl.Controls.Add(this.tabPage1);
            this.MetaDataConfigTabControl.Controls.Add(this.MetaDataTabPageTags);
            this.MetaDataConfigTabControl.Location = new System.Drawing.Point(3, 4);
            this.MetaDataConfigTabControl.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MetaDataConfigTabControl.Name = "MetaDataConfigTabControl";
            this.MetaDataConfigTabControl.SelectedIndex = 0;
            this.MetaDataConfigTabControl.Size = new System.Drawing.Size(308, 372);
            this.MetaDataConfigTabControl.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.SettingsGroup1Panel);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage1.Size = new System.Drawing.Size(300, 343);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Default";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // SettingsGroup1Panel
            // 
            this.SettingsGroup1Panel.Controls.Add(this.PictureBoxToolTipMetaFileName);
            this.SettingsGroup1Panel.Controls.Add(this.label4);
            this.SettingsGroup1Panel.Controls.Add(this.ComboBoxFileName);
            this.SettingsGroup1Panel.Controls.Add(this.button1);
            this.SettingsGroup1Panel.Controls.Add(this.label2);
            this.SettingsGroup1Panel.Controls.Add(this.label1);
            this.SettingsGroup1Panel.Controls.Add(this.CustomDefaultKeys);
            this.SettingsGroup1Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SettingsGroup1Panel.Location = new System.Drawing.Point(3, 2);
            this.SettingsGroup1Panel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.SettingsGroup1Panel.Name = "SettingsGroup1Panel";
            this.SettingsGroup1Panel.Size = new System.Drawing.Size(294, 339);
            this.SettingsGroup1Panel.TabIndex = 4;
            // 
            // PictureBoxToolTipMetaFileName
            // 
            this.PictureBoxToolTipMetaFileName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.PictureBoxToolTipMetaFileName.Image = global::Win_CBZ.Properties.Resources.information;
            this.PictureBoxToolTipMetaFileName.InitialImage = global::Win_CBZ.Properties.Resources.information;
            this.PictureBoxToolTipMetaFileName.Location = new System.Drawing.Point(238, 8);
            this.PictureBoxToolTipMetaFileName.Margin = new System.Windows.Forms.Padding(0);
            this.PictureBoxToolTipMetaFileName.Name = "PictureBoxToolTipMetaFileName";
            this.PictureBoxToolTipMetaFileName.Padding = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.PictureBoxToolTipMetaFileName.Size = new System.Drawing.Size(38, 36);
            this.PictureBoxToolTipMetaFileName.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.PictureBoxToolTipMetaFileName.TabIndex = 6;
            this.PictureBoxToolTipMetaFileName.TabStop = false;
            this.TagValidationTooltip.SetToolTip(this.PictureBoxToolTipMetaFileName, "Should always be \"ComicInfo.xml\". \r\nThis option sets the name of the Metadata- Fi" +
        "le within the Archive and can be changed here for more flexibility.");
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(15, 18);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(66, 16);
            this.label4.TabIndex = 5;
            this.label4.Text = "Filename:";
            // 
            // ComboBoxFileName
            // 
            this.ComboBoxFileName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ComboBoxFileName.Items.AddRange(new object[] {
            "ComicInfo.xml"});
            this.ComboBoxFileName.Location = new System.Drawing.Point(87, 14);
            this.ComboBoxFileName.Margin = new System.Windows.Forms.Padding(3, 4, 20, 4);
            this.ComboBoxFileName.Name = "ComboBoxFileName";
            this.ComboBoxFileName.Size = new System.Drawing.Size(108, 24);
            this.ComboBoxFileName.TabIndex = 4;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(164, 244);
            this.button1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(115, 38);
            this.button1.TabIndex = 3;
            this.button1.Text = "Fill Predifined";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.Location = new System.Drawing.Point(8, 297);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(313, 32);
            this.label2.TabIndex = 2;
            this.label2.Text = "One Key per Line\r\nTo set a default value for a given key use <key>=<value> format" +
    "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 50);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(144, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "Default MetaData Keys";
            // 
            // CustomDefaultKeys
            // 
            this.CustomDefaultKeys.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CustomDefaultKeys.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CustomDefaultKeys.Location = new System.Drawing.Point(18, 71);
            this.CustomDefaultKeys.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.CustomDefaultKeys.Multiline = true;
            this.CustomDefaultKeys.Name = "CustomDefaultKeys";
            this.CustomDefaultKeys.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.CustomDefaultKeys.Size = new System.Drawing.Size(259, 153);
            this.CustomDefaultKeys.TabIndex = 0;
            // 
            // MetaDataTabPageTags
            // 
            this.MetaDataTabPageTags.Controls.Add(this.tableLayoutPanel1);
            this.MetaDataTabPageTags.Location = new System.Drawing.Point(4, 25);
            this.MetaDataTabPageTags.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MetaDataTabPageTags.Name = "MetaDataTabPageTags";
            this.MetaDataTabPageTags.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MetaDataTabPageTags.Size = new System.Drawing.Size(419, 343);
            this.MetaDataTabPageTags.TabIndex = 1;
            this.MetaDataTabPageTags.Text = "Tags";
            this.MetaDataTabPageTags.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel1.Controls.Add(this.CheckBoxValidateTags, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.InfoIconTooltip, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.CheckBoxTagValidationIgnoreCase, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.ValidTags, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.ItemEditorToolBar, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 2);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(413, 339);
            this.tableLayoutPanel1.TabIndex = 0;
            this.tableLayoutPanel1.Paint += new System.Windows.Forms.PaintEventHandler(this.tableLayoutPanel1_Paint);
            // 
            // CheckBoxValidateTags
            // 
            this.CheckBoxValidateTags.AutoSize = true;
            this.CheckBoxValidateTags.Location = new System.Drawing.Point(3, 2);
            this.CheckBoxValidateTags.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.CheckBoxValidateTags.Name = "CheckBoxValidateTags";
            this.CheckBoxValidateTags.Padding = new System.Windows.Forms.Padding(5, 7, 0, 0);
            this.CheckBoxValidateTags.Size = new System.Drawing.Size(286, 27);
            this.CheckBoxValidateTags.TabIndex = 0;
            this.CheckBoxValidateTags.Text = "Validate Tags against a list of known Tags";
            this.CheckBoxValidateTags.UseVisualStyleBackColor = true;
            this.CheckBoxValidateTags.CheckStateChanged += new System.EventHandler(this.CheckBoxValidateTags_CheckStateChanged);
            // 
            // InfoIconTooltip
            // 
            this.InfoIconTooltip.Image = global::Win_CBZ.Properties.Resources.information;
            this.InfoIconTooltip.InitialImage = global::Win_CBZ.Properties.Resources.information;
            this.InfoIconTooltip.Location = new System.Drawing.Point(353, 0);
            this.InfoIconTooltip.Margin = new System.Windows.Forms.Padding(0);
            this.InfoIconTooltip.Name = "InfoIconTooltip";
            this.InfoIconTooltip.Padding = new System.Windows.Forms.Padding(7, 7, 7, 6);
            this.tableLayoutPanel1.SetRowSpan(this.InfoIconTooltip, 2);
            this.InfoIconTooltip.Size = new System.Drawing.Size(38, 37);
            this.InfoIconTooltip.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.InfoIconTooltip.TabIndex = 5;
            this.InfoIconTooltip.TabStop = false;
            this.TagValidationTooltip.SetToolTip(this.InfoIconTooltip, "This options allow you, to validate matadata tags against your own list of valid " +
        "tags,\r\npreventing typos, duplicate- and invalid tags, from being generated/shown" +
        " within applications.\r\n");
            // 
            // CheckBoxTagValidationIgnoreCase
            // 
            this.CheckBoxTagValidationIgnoreCase.AutoSize = true;
            this.CheckBoxTagValidationIgnoreCase.Enabled = false;
            this.CheckBoxTagValidationIgnoreCase.Location = new System.Drawing.Point(4, 44);
            this.CheckBoxTagValidationIgnoreCase.Margin = new System.Windows.Forms.Padding(4);
            this.CheckBoxTagValidationIgnoreCase.Name = "CheckBoxTagValidationIgnoreCase";
            this.CheckBoxTagValidationIgnoreCase.Padding = new System.Windows.Forms.Padding(24, 5, 0, 0);
            this.CheckBoxTagValidationIgnoreCase.Size = new System.Drawing.Size(143, 25);
            this.CheckBoxTagValidationIgnoreCase.TabIndex = 6;
            this.CheckBoxTagValidationIgnoreCase.Text = "Case Sensitive";
            this.CheckBoxTagValidationIgnoreCase.UseVisualStyleBackColor = true;
            this.CheckBoxTagValidationIgnoreCase.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 303);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(111, 16);
            this.label3.TabIndex = 4;
            this.label3.Text = "One Tag per Line";
            // 
            // ValidTags
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.ValidTags, 2);
            this.ValidTags.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ValidTags.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ValidTags.Location = new System.Drawing.Point(3, 92);
            this.ValidTags.Margin = new System.Windows.Forms.Padding(3, 2, 3, 8);
            this.ValidTags.Multiline = true;
            this.ValidTags.Name = "ValidTags";
            this.ValidTags.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.ValidTags.Size = new System.Drawing.Size(407, 203);
            this.ValidTags.TabIndex = 3;
            // 
            // ItemEditorToolBar
            // 
            this.ItemEditorToolBar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ItemEditorToolBar.AutoSize = false;
            this.ItemEditorToolBar.BackColor = System.Drawing.Color.White;
            this.tableLayoutPanel1.SetColumnSpan(this.ItemEditorToolBar, 2);
            this.ItemEditorToolBar.Dock = System.Windows.Forms.DockStyle.None;
            this.ItemEditorToolBar.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.ItemEditorToolBar.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.ItemEditorToolBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolButtonSortAscending});
            this.ItemEditorToolBar.Location = new System.Drawing.Point(362, 80);
            this.ItemEditorToolBar.Margin = new System.Windows.Forms.Padding(8, 0, 0, 2);
            this.ItemEditorToolBar.Name = "ItemEditorToolBar";
            this.ItemEditorToolBar.Padding = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.ItemEditorToolBar.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.ItemEditorToolBar.Size = new System.Drawing.Size(51, 8);
            this.ItemEditorToolBar.Stretch = true;
            this.ItemEditorToolBar.TabIndex = 8;
            // 
            // ToolButtonSortAscending
            // 
            this.ToolButtonSortAscending.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ToolButtonSortAscending.Image = global::Win_CBZ.Properties.Resources.sort_az_ascending2;
            this.ToolButtonSortAscending.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ToolButtonSortAscending.Name = "ToolButtonSortAscending";
            this.ToolButtonSortAscending.Size = new System.Drawing.Size(29, 29);
            this.ToolButtonSortAscending.ToolTipText = "Sort items ascending";
            this.ToolButtonSortAscending.Click += new System.EventHandler(this.ToolButtonSortAscending_Click);
            // 
            // CBZSettingsTabControl
            // 
            this.CBZSettingsTabControl.Controls.Add(this.tabPage2);
            this.CBZSettingsTabControl.Location = new System.Drawing.Point(130, 4);
            this.CBZSettingsTabControl.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.CBZSettingsTabControl.Name = "CBZSettingsTabControl";
            this.CBZSettingsTabControl.SelectedIndex = 0;
            this.CBZSettingsTabControl.Size = new System.Drawing.Size(151, 318);
            this.CBZSettingsTabControl.TabIndex = 4;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.tableLayoutPanel2);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Size = new System.Drawing.Size(143, 289);
            this.tabPage2.TabIndex = 0;
            this.tabPage2.Text = "Compatibility";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 180F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 57F));
            this.tableLayoutPanel2.Controls.Add(this.label5, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.ComboBoxPageIndexVersionWrite, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.pictureBox2, 2, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 22.55435F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 77.44566F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(143, 289);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(18, 16);
            this.label5.Margin = new System.Windows.Forms.Padding(18, 16, 18, 16);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(135, 16);
            this.label5.TabIndex = 0;
            this.label5.Text = "Meta Format -Version";
            // 
            // ComboBoxPageIndexVersionWrite
            // 
            this.ComboBoxPageIndexVersionWrite.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComboBoxPageIndexVersionWrite.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComboBoxPageIndexVersionWrite.FormattingEnabled = true;
            this.ComboBoxPageIndexVersionWrite.Items.AddRange(new object[] {
            "VERSION_1",
            "VERSION_2"});
            this.ComboBoxPageIndexVersionWrite.Location = new System.Drawing.Point(189, 16);
            this.ComboBoxPageIndexVersionWrite.Margin = new System.Windows.Forms.Padding(9, 16, 18, 16);
            this.ComboBoxPageIndexVersionWrite.Name = "ComboBoxPageIndexVersionWrite";
            this.ComboBoxPageIndexVersionWrite.Size = new System.Drawing.Size(1, 24);
            this.ComboBoxPageIndexVersionWrite.TabIndex = 1;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::Win_CBZ.Properties.Resources.information;
            this.pictureBox2.InitialImage = global::Win_CBZ.Properties.Resources.information;
            this.pictureBox2.Location = new System.Drawing.Point(86, 0);
            this.pictureBox2.Margin = new System.Windows.Forms.Padding(0);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Padding = new System.Windows.Forms.Padding(7, 16, 7, 6);
            this.pictureBox2.Size = new System.Drawing.Size(38, 46);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox2.TabIndex = 7;
            this.pictureBox2.TabStop = false;
            this.TagValidationTooltip.SetToolTip(this.pictureBox2, resources.GetString("pictureBox2.ToolTip"));
            // 
            // AppSettingsTabControl
            // 
            this.AppSettingsTabControl.Controls.Add(this.TabPageAppSettings);
            this.AppSettingsTabControl.Controls.Add(this.tabPage3);
            this.AppSettingsTabControl.Location = new System.Drawing.Point(273, 4);
            this.AppSettingsTabControl.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.AppSettingsTabControl.Name = "AppSettingsTabControl";
            this.AppSettingsTabControl.SelectedIndex = 0;
            this.AppSettingsTabControl.Size = new System.Drawing.Size(267, 320);
            this.AppSettingsTabControl.TabIndex = 3;
            // 
            // TabPageAppSettings
            // 
            this.TabPageAppSettings.Controls.Add(this.CustomFieldTypesTablePanel);
            this.TabPageAppSettings.Location = new System.Drawing.Point(4, 25);
            this.TabPageAppSettings.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.TabPageAppSettings.Name = "TabPageAppSettings";
            this.TabPageAppSettings.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.TabPageAppSettings.Size = new System.Drawing.Size(259, 291);
            this.TabPageAppSettings.TabIndex = 0;
            this.TabPageAppSettings.Text = "MetaData Editor";
            this.TabPageAppSettings.UseVisualStyleBackColor = true;
            // 
            // CustomFieldTypesTablePanel
            // 
            this.CustomFieldTypesTablePanel.ColumnCount = 3;
            this.CustomFieldTypesTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 28.57143F));
            this.CustomFieldTypesTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 28.57143F));
            this.CustomFieldTypesTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 42.85714F));
            this.CustomFieldTypesTablePanel.Controls.Add(this.CustomFieldsDataGrid, 0, 1);
            this.CustomFieldTypesTablePanel.Controls.Add(this.AddFieldTypeButton, 0, 2);
            this.CustomFieldTypesTablePanel.Controls.Add(this.RemoveFieldTypeButton, 1, 2);
            this.CustomFieldTypesTablePanel.Controls.Add(this.RestoreFieldTypesButton, 2, 2);
            this.CustomFieldTypesTablePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CustomFieldTypesTablePanel.Location = new System.Drawing.Point(3, 4);
            this.CustomFieldTypesTablePanel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.CustomFieldTypesTablePanel.Name = "CustomFieldTypesTablePanel";
            this.CustomFieldTypesTablePanel.RowCount = 3;
            this.CustomFieldTypesTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 39F));
            this.CustomFieldTypesTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.CustomFieldTypesTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 43F));
            this.CustomFieldTypesTablePanel.Size = new System.Drawing.Size(253, 283);
            this.CustomFieldTypesTablePanel.TabIndex = 1;
            // 
            // CustomFieldsDataGrid
            // 
            this.CustomFieldsDataGrid.AllowUserToAddRows = false;
            this.CustomFieldsDataGrid.AllowUserToDeleteRows = false;
            this.CustomFieldsDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.CustomFieldTypesTablePanel.SetColumnSpan(this.CustomFieldsDataGrid, 3);
            dataGridViewCellStyle15.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle15.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle15.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle15.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle15.SelectionBackColor = System.Drawing.Color.Gold;
            dataGridViewCellStyle15.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle15.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.CustomFieldsDataGrid.DefaultCellStyle = dataGridViewCellStyle15;
            this.CustomFieldsDataGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CustomFieldsDataGrid.Location = new System.Drawing.Point(3, 43);
            this.CustomFieldsDataGrid.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.CustomFieldsDataGrid.Name = "CustomFieldsDataGrid";
            this.CustomFieldsDataGrid.RowHeadersWidth = 51;
            this.CustomFieldsDataGrid.RowTemplate.Height = 24;
            this.CustomFieldsDataGrid.Size = new System.Drawing.Size(247, 193);
            this.CustomFieldsDataGrid.TabIndex = 0;
            this.CustomFieldsDataGrid.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.CustomFieldsDataGrid_CellContentClick);
            this.CustomFieldsDataGrid.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.CustomFieldsDataGrid_CellValueChanged);
            this.CustomFieldsDataGrid.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.CustomFieldsDataGrid_DataError);
            this.CustomFieldsDataGrid.SelectionChanged += new System.EventHandler(this.CustomFieldsDataGrid_SelectionChanged);
            // 
            // AddFieldTypeButton
            // 
            this.AddFieldTypeButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AddFieldTypeButton.Image = global::Win_CBZ.Properties.Resources.navigate_plus;
            this.AddFieldTypeButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.AddFieldTypeButton.Location = new System.Drawing.Point(3, 246);
            this.AddFieldTypeButton.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
            this.AddFieldTypeButton.Name = "AddFieldTypeButton";
            this.AddFieldTypeButton.Size = new System.Drawing.Size(66, 34);
            this.AddFieldTypeButton.TabIndex = 1;
            this.AddFieldTypeButton.Text = "Add";
            this.AddFieldTypeButton.UseVisualStyleBackColor = true;
            this.AddFieldTypeButton.Click += new System.EventHandler(this.AddFieldTypeButton_Click);
            // 
            // RemoveFieldTypeButton
            // 
            this.RemoveFieldTypeButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RemoveFieldTypeButton.Enabled = false;
            this.RemoveFieldTypeButton.Location = new System.Drawing.Point(75, 246);
            this.RemoveFieldTypeButton.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
            this.RemoveFieldTypeButton.Name = "RemoveFieldTypeButton";
            this.RemoveFieldTypeButton.Size = new System.Drawing.Size(66, 34);
            this.RemoveFieldTypeButton.TabIndex = 2;
            this.RemoveFieldTypeButton.Text = "Remove";
            this.RemoveFieldTypeButton.UseVisualStyleBackColor = true;
            this.RemoveFieldTypeButton.Click += new System.EventHandler(this.RemoveFieldTypeButton_Click);
            // 
            // RestoreFieldTypesButton
            // 
            this.RestoreFieldTypesButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.RestoreFieldTypesButton.Location = new System.Drawing.Point(147, 246);
            this.RestoreFieldTypesButton.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
            this.RestoreFieldTypesButton.Name = "RestoreFieldTypesButton";
            this.RestoreFieldTypesButton.Size = new System.Drawing.Size(103, 34);
            this.RestoreFieldTypesButton.TabIndex = 3;
            this.RestoreFieldTypesButton.Text = "Restore";
            this.RestoreFieldTypesButton.UseVisualStyleBackColor = true;
            this.RestoreFieldTypesButton.Click += new System.EventHandler(this.RestoreFieldTypesButton_Click);
            // 
            // tabPage3
            // 
            this.tabPage3.Location = new System.Drawing.Point(4, 25);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(259, 291);
            this.tabPage3.TabIndex = 1;
            this.tabPage3.Text = "Deletion";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // ImageProcessingTabControl
            // 
            this.ImageProcessingTabControl.Controls.Add(this.ImageConversionTabPage);
            this.ImageProcessingTabControl.Controls.Add(this.ImageProcessingTabPage);
            this.ImageProcessingTabControl.Location = new System.Drawing.Point(408, 4);
            this.ImageProcessingTabControl.Margin = new System.Windows.Forms.Padding(4);
            this.ImageProcessingTabControl.Name = "ImageProcessingTabControl";
            this.ImageProcessingTabControl.SelectedIndex = 0;
            this.ImageProcessingTabControl.Size = new System.Drawing.Size(156, 324);
            this.ImageProcessingTabControl.TabIndex = 2;
            // 
            // ImageConversionTabPage
            // 
            this.ImageConversionTabPage.Controls.Add(this.LabelConvertImages);
            this.ImageConversionTabPage.Controls.Add(this.ComboBoxConvertPages);
            this.ImageConversionTabPage.Controls.Add(this.GroupBoxImageQuality);
            this.ImageConversionTabPage.Location = new System.Drawing.Point(4, 25);
            this.ImageConversionTabPage.Margin = new System.Windows.Forms.Padding(4);
            this.ImageConversionTabPage.Name = "ImageConversionTabPage";
            this.ImageConversionTabPage.Padding = new System.Windows.Forms.Padding(4);
            this.ImageConversionTabPage.Size = new System.Drawing.Size(148, 295);
            this.ImageConversionTabPage.TabIndex = 0;
            this.ImageConversionTabPage.Text = "Image Conversion";
            this.ImageConversionTabPage.UseVisualStyleBackColor = true;
            // 
            // LabelConvertImages
            // 
            this.LabelConvertImages.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.LabelConvertImages.AutoSize = true;
            this.LabelConvertImages.Location = new System.Drawing.Point(8, 23);
            this.LabelConvertImages.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LabelConvertImages.Name = "LabelConvertImages";
            this.LabelConvertImages.Size = new System.Drawing.Size(101, 16);
            this.LabelConvertImages.TabIndex = 22;
            this.LabelConvertImages.Text = "Convert Images";
            // 
            // ComboBoxConvertPages
            // 
            this.ComboBoxConvertPages.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ComboBoxConvertPages.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComboBoxConvertPages.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.ComboBoxConvertPages.FormattingEnabled = true;
            this.ComboBoxConvertPages.Items.AddRange(new object[] {
            "Dont Convert, keep original Format",
            "Bitmap",
            "Jpeg",
            "PNG",
            "Tiff"});
            this.ComboBoxConvertPages.Location = new System.Drawing.Point(11, 39);
            this.ComboBoxConvertPages.Margin = new System.Windows.Forms.Padding(4);
            this.ComboBoxConvertPages.Name = "ComboBoxConvertPages";
            this.ComboBoxConvertPages.Size = new System.Drawing.Size(112, 24);
            this.ComboBoxConvertPages.TabIndex = 23;
            // 
            // GroupBoxImageQuality
            // 
            this.GroupBoxImageQuality.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GroupBoxImageQuality.Controls.Add(this.ImageQualitySliderMaxLabel);
            this.GroupBoxImageQuality.Controls.Add(this.ImageQualitySliderMinLabel);
            this.GroupBoxImageQuality.Controls.Add(this.ImageQualityTrackBar);
            this.GroupBoxImageQuality.Location = new System.Drawing.Point(11, 82);
            this.GroupBoxImageQuality.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.GroupBoxImageQuality.Name = "GroupBoxImageQuality";
            this.GroupBoxImageQuality.Padding = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.GroupBoxImageQuality.Size = new System.Drawing.Size(117, 117);
            this.GroupBoxImageQuality.TabIndex = 21;
            this.GroupBoxImageQuality.TabStop = false;
            this.GroupBoxImageQuality.Text = "Image Quality";
            // 
            // ImageQualitySliderMaxLabel
            // 
            this.ImageQualitySliderMaxLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ImageQualitySliderMaxLabel.AutoSize = true;
            this.ImageQualitySliderMaxLabel.Location = new System.Drawing.Point(73, 82);
            this.ImageQualitySliderMaxLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.ImageQualitySliderMaxLabel.Name = "ImageQualitySliderMaxLabel";
            this.ImageQualitySliderMaxLabel.Size = new System.Drawing.Size(35, 16);
            this.ImageQualitySliderMaxLabel.TabIndex = 5;
            this.ImageQualitySliderMaxLabel.Text = "High";
            // 
            // ImageQualitySliderMinLabel
            // 
            this.ImageQualitySliderMinLabel.AutoSize = true;
            this.ImageQualitySliderMinLabel.Location = new System.Drawing.Point(9, 82);
            this.ImageQualitySliderMinLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.ImageQualitySliderMinLabel.Name = "ImageQualitySliderMinLabel";
            this.ImageQualitySliderMinLabel.Size = new System.Drawing.Size(31, 16);
            this.ImageQualitySliderMinLabel.TabIndex = 4;
            this.ImageQualitySliderMinLabel.Text = "Low";
            // 
            // ImageQualityTrackBar
            // 
            this.ImageQualityTrackBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ImageQualityTrackBar.BackColor = System.Drawing.SystemColors.Window;
            this.ImageQualityTrackBar.Enabled = false;
            this.ImageQualityTrackBar.Location = new System.Drawing.Point(14, 22);
            this.ImageQualityTrackBar.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.ImageQualityTrackBar.Maximum = 100;
            this.ImageQualityTrackBar.Minimum = 10;
            this.ImageQualityTrackBar.Name = "ImageQualityTrackBar";
            this.ImageQualityTrackBar.Size = new System.Drawing.Size(98, 56);
            this.ImageQualityTrackBar.TabIndex = 3;
            this.ImageQualityTrackBar.Value = 85;
            // 
            // ImageProcessingTabPage
            // 
            this.ImageProcessingTabPage.Location = new System.Drawing.Point(4, 25);
            this.ImageProcessingTabPage.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ImageProcessingTabPage.Name = "ImageProcessingTabPage";
            this.ImageProcessingTabPage.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ImageProcessingTabPage.Size = new System.Drawing.Size(148, 295);
            this.ImageProcessingTabPage.TabIndex = 1;
            this.ImageProcessingTabPage.Text = "Image Processing";
            this.ImageProcessingTabPage.UseVisualStyleBackColor = true;
            // 
            // TagValidationTooltip
            // 
            this.TagValidationTooltip.AutoPopDelay = 30000;
            this.TagValidationTooltip.InitialDelay = 200;
            this.TagValidationTooltip.IsBalloon = true;
            this.TagValidationTooltip.ReshowDelay = 100;
            this.TagValidationTooltip.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.TagValidationTooltip.ToolTipTitle = "Win_CBZ";
            // 
            // SettingsValidationErrorProvider
            // 
            this.SettingsValidationErrorProvider.ContainerControl = this;
            // 
            // SettingsDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 519);
            this.Controls.Add(this.SettingsTablePanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "SettingsDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Application Configuration";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SettingsDialog_FormClosing);
            this.Load += new System.EventHandler(this.SettingsDialog_Load);
            this.SettingsTablePanel.ResumeLayout(false);
            this.HeaderPanel.ResumeLayout(false);
            this.HeaderPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.SettingsContentPanel.ResumeLayout(false);
            this.MetaDataConfigTabControl.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.SettingsGroup1Panel.ResumeLayout(false);
            this.SettingsGroup1Panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBoxToolTipMetaFileName)).EndInit();
            this.MetaDataTabPageTags.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.InfoIconTooltip)).EndInit();
            this.ItemEditorToolBar.ResumeLayout(false);
            this.ItemEditorToolBar.PerformLayout();
            this.CBZSettingsTabControl.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.AppSettingsTabControl.ResumeLayout(false);
            this.TabPageAppSettings.ResumeLayout(false);
            this.CustomFieldTypesTablePanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.CustomFieldsDataGrid)).EndInit();
            this.ImageProcessingTabControl.ResumeLayout(false);
            this.ImageConversionTabPage.ResumeLayout(false);
            this.ImageConversionTabPage.PerformLayout();
            this.GroupBoxImageQuality.ResumeLayout(false);
            this.GroupBoxImageQuality.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ImageQualityTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SettingsValidationErrorProvider)).EndInit();
            this.ResumeLayout(false);

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
        private System.Windows.Forms.TabPage tabPage3;
    }
}