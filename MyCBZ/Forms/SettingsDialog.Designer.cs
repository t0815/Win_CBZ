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
            this.SettingsTablePanel = new System.Windows.Forms.TableLayoutPanel();
            this.ButtonOk = new System.Windows.Forms.Button();
            this.HeaderPanel = new System.Windows.Forms.Panel();
            this.HeaderLabel = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.SettingsSectionList = new System.Windows.Forms.ListBox();
            this.ButtonCancel = new System.Windows.Forms.Button();
            this.MetaDataConfigTabControl = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.SettingsGroup1Panel = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.CustomDefaultKeys = new System.Windows.Forms.TextBox();
            this.MetaDataTabPageTags = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.CheckBoxValidateTags = new System.Windows.Forms.CheckBox();
            this.InfoIconTooltip = new System.Windows.Forms.PictureBox();
            this.label3 = new System.Windows.Forms.Label();
            this.ValidTags = new System.Windows.Forms.TextBox();
            this.CheckBoxTagValidationIgnoreCase = new System.Windows.Forms.CheckBox();
            this.TagValidationTooltip = new System.Windows.Forms.ToolTip(this.components);
            this.ItemEditorToolBar = new System.Windows.Forms.ToolStrip();
            this.ToolButtonSortAscending = new System.Windows.Forms.ToolStripButton();
            this.SettingsTablePanel.SuspendLayout();
            this.HeaderPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.MetaDataConfigTabControl.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SettingsGroup1Panel.SuspendLayout();
            this.MetaDataTabPageTags.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.InfoIconTooltip)).BeginInit();
            this.ItemEditorToolBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // SettingsTablePanel
            // 
            this.SettingsTablePanel.ColumnCount = 3;
            this.SettingsTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30.40752F));
            this.SettingsTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 69.59248F));
            this.SettingsTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 99F));
            this.SettingsTablePanel.Controls.Add(this.ButtonOk, 1, 2);
            this.SettingsTablePanel.Controls.Add(this.HeaderPanel, 0, 0);
            this.SettingsTablePanel.Controls.Add(this.SettingsSectionList, 0, 1);
            this.SettingsTablePanel.Controls.Add(this.ButtonCancel, 2, 2);
            this.SettingsTablePanel.Controls.Add(this.MetaDataConfigTabControl, 1, 1);
            this.SettingsTablePanel.Location = new System.Drawing.Point(2, 2);
            this.SettingsTablePanel.Margin = new System.Windows.Forms.Padding(2);
            this.SettingsTablePanel.Name = "SettingsTablePanel";
            this.SettingsTablePanel.RowCount = 3;
            this.SettingsTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 24.02235F));
            this.SettingsTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 75.97765F));
            this.SettingsTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.SettingsTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16F));
            this.SettingsTablePanel.Size = new System.Drawing.Size(590, 418);
            this.SettingsTablePanel.TabIndex = 0;
            this.SettingsTablePanel.Paint += new System.Windows.Forms.PaintEventHandler(this.SettingsTablePanel_Paint);
            // 
            // ButtonOk
            // 
            this.ButtonOk.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.ButtonOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.ButtonOk.Location = new System.Drawing.Point(405, 374);
            this.ButtonOk.Margin = new System.Windows.Forms.Padding(2);
            this.ButtonOk.Name = "ButtonOk";
            this.ButtonOk.Size = new System.Drawing.Size(83, 27);
            this.ButtonOk.TabIndex = 2;
            this.ButtonOk.Text = "Ok";
            this.ButtonOk.UseVisualStyleBackColor = true;
            this.ButtonOk.Click += new System.EventHandler(this.ButtonOk_Click);
            // 
            // HeaderPanel
            // 
            this.HeaderPanel.BackColor = System.Drawing.Color.White;
            this.SettingsTablePanel.SetColumnSpan(this.HeaderPanel, 3);
            this.HeaderPanel.Controls.Add(this.HeaderLabel);
            this.HeaderPanel.Controls.Add(this.pictureBox1);
            this.HeaderPanel.Location = new System.Drawing.Point(2, 2);
            this.HeaderPanel.Margin = new System.Windows.Forms.Padding(2);
            this.HeaderPanel.Name = "HeaderPanel";
            this.HeaderPanel.Size = new System.Drawing.Size(585, 67);
            this.HeaderPanel.TabIndex = 0;
            // 
            // HeaderLabel
            // 
            this.HeaderLabel.AutoSize = true;
            this.HeaderLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HeaderLabel.Location = new System.Drawing.Point(87, 22);
            this.HeaderLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.HeaderLabel.Name = "HeaderLabel";
            this.HeaderLabel.Size = new System.Drawing.Size(91, 21);
            this.HeaderLabel.TabIndex = 1;
            this.HeaderLabel.Text = "Preferences";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Win_CBZ.Properties.Resources.window_gear_large;
            this.pictureBox1.Location = new System.Drawing.Point(18, 11);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(44, 45);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // SettingsSectionList
            // 
            this.SettingsSectionList.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SettingsSectionList.FormattingEnabled = true;
            this.SettingsSectionList.ItemHeight = 17;
            this.SettingsSectionList.Items.AddRange(new object[] {
            "Meta Data"});
            this.SettingsSectionList.Location = new System.Drawing.Point(2, 88);
            this.SettingsSectionList.Margin = new System.Windows.Forms.Padding(2);
            this.SettingsSectionList.Name = "SettingsSectionList";
            this.SettingsSectionList.Size = new System.Drawing.Size(142, 259);
            this.SettingsSectionList.TabIndex = 1;
            // 
            // ButtonCancel
            // 
            this.ButtonCancel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.ButtonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ButtonCancel.Location = new System.Drawing.Point(499, 374);
            this.ButtonCancel.Margin = new System.Windows.Forms.Padding(2);
            this.ButtonCancel.Name = "ButtonCancel";
            this.ButtonCancel.Size = new System.Drawing.Size(89, 27);
            this.ButtonCancel.TabIndex = 3;
            this.ButtonCancel.Text = "Cancel";
            this.ButtonCancel.UseVisualStyleBackColor = true;
            this.ButtonCancel.Click += new System.EventHandler(this.ButtonCancel_Click);
            // 
            // MetaDataConfigTabControl
            // 
            this.SettingsTablePanel.SetColumnSpan(this.MetaDataConfigTabControl, 2);
            this.MetaDataConfigTabControl.Controls.Add(this.tabPage1);
            this.MetaDataConfigTabControl.Controls.Add(this.MetaDataTabPageTags);
            this.MetaDataConfigTabControl.Location = new System.Drawing.Point(151, 88);
            this.MetaDataConfigTabControl.Margin = new System.Windows.Forms.Padding(2);
            this.MetaDataConfigTabControl.Name = "MetaDataConfigTabControl";
            this.MetaDataConfigTabControl.SelectedIndex = 0;
            this.MetaDataConfigTabControl.Size = new System.Drawing.Size(436, 266);
            this.MetaDataConfigTabControl.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.SettingsGroup1Panel);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(2);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(2);
            this.tabPage1.Size = new System.Drawing.Size(428, 240);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Default";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // SettingsGroup1Panel
            // 
            this.SettingsGroup1Panel.Controls.Add(this.button1);
            this.SettingsGroup1Panel.Controls.Add(this.label2);
            this.SettingsGroup1Panel.Controls.Add(this.label1);
            this.SettingsGroup1Panel.Controls.Add(this.CustomDefaultKeys);
            this.SettingsGroup1Panel.Location = new System.Drawing.Point(4, 5);
            this.SettingsGroup1Panel.Margin = new System.Windows.Forms.Padding(2);
            this.SettingsGroup1Panel.Name = "SettingsGroup1Panel";
            this.SettingsGroup1Panel.Size = new System.Drawing.Size(422, 219);
            this.SettingsGroup1Panel.TabIndex = 4;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(327, 185);
            this.button1.Margin = new System.Windows.Forms.Padding(2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(86, 31);
            this.button1.TabIndex = 3;
            this.button1.Text = "Fill Predifined";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(8, 178);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(302, 26);
            this.label2.TabIndex = 2;
            this.label2.Text = "One Key per Line\r\nTo set a default value for a given key use <key>=<value> format" +
    "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(2, 6);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(117, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Default MetaData Keys";
            // 
            // CustomDefaultKeys
            // 
            this.CustomDefaultKeys.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CustomDefaultKeys.Location = new System.Drawing.Point(2, 21);
            this.CustomDefaultKeys.Margin = new System.Windows.Forms.Padding(2);
            this.CustomDefaultKeys.Multiline = true;
            this.CustomDefaultKeys.Name = "CustomDefaultKeys";
            this.CustomDefaultKeys.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.CustomDefaultKeys.Size = new System.Drawing.Size(411, 138);
            this.CustomDefaultKeys.TabIndex = 0;
            // 
            // MetaDataTabPageTags
            // 
            this.MetaDataTabPageTags.Controls.Add(this.tableLayoutPanel1);
            this.MetaDataTabPageTags.Location = new System.Drawing.Point(4, 22);
            this.MetaDataTabPageTags.Margin = new System.Windows.Forms.Padding(2);
            this.MetaDataTabPageTags.Name = "MetaDataTabPageTags";
            this.MetaDataTabPageTags.Padding = new System.Windows.Forms.Padding(2);
            this.MetaDataTabPageTags.Size = new System.Drawing.Size(428, 240);
            this.MetaDataTabPageTags.TabIndex = 1;
            this.MetaDataTabPageTags.Text = "Tags";
            this.MetaDataTabPageTags.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 57.54717F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 42.45283F));
            this.tableLayoutPanel1.Controls.Add(this.CheckBoxValidateTags, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.InfoIconTooltip, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.CheckBoxTagValidationIgnoreCase, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.ValidTags, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.ItemEditorToolBar, 1, 1);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(4, 5);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 62.16216F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 37.83784F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 135F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(424, 236);
            this.tableLayoutPanel1.TabIndex = 0;
            this.tableLayoutPanel1.Paint += new System.Windows.Forms.PaintEventHandler(this.tableLayoutPanel1_Paint);
            // 
            // CheckBoxValidateTags
            // 
            this.CheckBoxValidateTags.AutoSize = true;
            this.CheckBoxValidateTags.Location = new System.Drawing.Point(2, 2);
            this.CheckBoxValidateTags.Margin = new System.Windows.Forms.Padding(2);
            this.CheckBoxValidateTags.Name = "CheckBoxValidateTags";
            this.CheckBoxValidateTags.Padding = new System.Windows.Forms.Padding(4, 4, 0, 0);
            this.CheckBoxValidateTags.Size = new System.Drawing.Size(230, 21);
            this.CheckBoxValidateTags.TabIndex = 0;
            this.CheckBoxValidateTags.Text = "Validate Tags against a list of known Tags";
            this.CheckBoxValidateTags.UseVisualStyleBackColor = true;
            this.CheckBoxValidateTags.CheckStateChanged += new System.EventHandler(this.CheckBoxValidateTags_CheckStateChanged);
            // 
            // InfoIconTooltip
            // 
            this.InfoIconTooltip.Image = global::Win_CBZ.Properties.Resources.information;
            this.InfoIconTooltip.InitialImage = global::Win_CBZ.Properties.Resources.information;
            this.InfoIconTooltip.Location = new System.Drawing.Point(244, 0);
            this.InfoIconTooltip.Margin = new System.Windows.Forms.Padding(0);
            this.InfoIconTooltip.Name = "InfoIconTooltip";
            this.InfoIconTooltip.Padding = new System.Windows.Forms.Padding(5);
            this.tableLayoutPanel1.SetRowSpan(this.InfoIconTooltip, 2);
            this.InfoIconTooltip.Size = new System.Drawing.Size(33, 37);
            this.InfoIconTooltip.TabIndex = 5;
            this.InfoIconTooltip.TabStop = false;
            this.TagValidationTooltip.SetToolTip(this.InfoIconTooltip, "This options allows you, to validate matadata tags against your own list of valid" +
        " tags, \r\npreventing typos and duplicate, invalid tags being generated/shown with" +
        "in applications.\r\n");
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(2, 211);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(90, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "One Tag per Line";
            // 
            // ValidTags
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.ValidTags, 2);
            this.ValidTags.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ValidTags.Location = new System.Drawing.Point(2, 78);
            this.ValidTags.Margin = new System.Windows.Forms.Padding(2);
            this.ValidTags.Multiline = true;
            this.ValidTags.Name = "ValidTags";
            this.ValidTags.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.ValidTags.Size = new System.Drawing.Size(403, 126);
            this.ValidTags.TabIndex = 3;
            // 
            // CheckBoxTagValidationIgnoreCase
            // 
            this.CheckBoxTagValidationIgnoreCase.AutoSize = true;
            this.CheckBoxTagValidationIgnoreCase.Enabled = false;
            this.CheckBoxTagValidationIgnoreCase.Location = new System.Drawing.Point(3, 32);
            this.CheckBoxTagValidationIgnoreCase.Name = "CheckBoxTagValidationIgnoreCase";
            this.CheckBoxTagValidationIgnoreCase.Padding = new System.Windows.Forms.Padding(18, 4, 0, 0);
            this.CheckBoxTagValidationIgnoreCase.Size = new System.Drawing.Size(114, 11);
            this.CheckBoxTagValidationIgnoreCase.TabIndex = 6;
            this.CheckBoxTagValidationIgnoreCase.Text = "Case Sensitive";
            this.CheckBoxTagValidationIgnoreCase.UseVisualStyleBackColor = true;
            this.CheckBoxTagValidationIgnoreCase.Visible = false;
            // 
            // TagValidationTooltip
            // 
            this.TagValidationTooltip.AutoPopDelay = 30000;
            this.TagValidationTooltip.InitialDelay = 200;
            this.TagValidationTooltip.IsBalloon = true;
            this.TagValidationTooltip.ReshowDelay = 100;
            this.TagValidationTooltip.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.TagValidationTooltip.ToolTipTitle = "Custom Tag Validation";
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
            this.ItemEditorToolBar.Location = new System.Drawing.Point(374, 47);
            this.ItemEditorToolBar.Margin = new System.Windows.Forms.Padding(6, 0, 0, 2);
            this.ItemEditorToolBar.Name = "ItemEditorToolBar";
            this.ItemEditorToolBar.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.ItemEditorToolBar.Size = new System.Drawing.Size(50, 27);
            this.ItemEditorToolBar.Stretch = true;
            this.ItemEditorToolBar.TabIndex = 8;
            // 
            // ToolButtonSortAscending
            // 
            this.ToolButtonSortAscending.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ToolButtonSortAscending.Image = global::Win_CBZ.Properties.Resources.sort_az_ascending2;
            this.ToolButtonSortAscending.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ToolButtonSortAscending.Name = "ToolButtonSortAscending";
            this.ToolButtonSortAscending.Size = new System.Drawing.Size(24, 24);
            this.ToolButtonSortAscending.Text = "toolStripButton1";
            this.ToolButtonSortAscending.ToolTipText = "Sort items ascending";
            this.ToolButtonSortAscending.Click += new System.EventHandler(this.ToolButtonSortAscending_Click);
            // 
            // SettingsDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(600, 422);
            this.Controls.Add(this.SettingsTablePanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "SettingsDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Application Configuration";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SettingsDialog_FormClosing);
            this.SettingsTablePanel.ResumeLayout(false);
            this.HeaderPanel.ResumeLayout(false);
            this.HeaderPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.MetaDataConfigTabControl.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.SettingsGroup1Panel.ResumeLayout(false);
            this.SettingsGroup1Panel.PerformLayout();
            this.MetaDataTabPageTags.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.InfoIconTooltip)).EndInit();
            this.ItemEditorToolBar.ResumeLayout(false);
            this.ItemEditorToolBar.PerformLayout();
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
    }
}