namespace Win_CBZ.Forms
{
    partial class ExtractFilesDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExtractFilesDialog));
            ExtractToTablePanel = new System.Windows.Forms.TableLayoutPanel();
            HeaderPanel = new System.Windows.Forms.Panel();
            HeaderLabel = new System.Windows.Forms.Label();
            HeaderPicture = new System.Windows.Forms.PictureBox();
            ButtonOk = new System.Windows.Forms.Button();
            ButtonCancel = new System.Windows.Forms.Button();
            SettingsGroup1Panel = new System.Windows.Forms.Panel();
            label3 = new System.Windows.Forms.Label();
            tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            DirectoryPathTextBox = new System.Windows.Forms.Panel();
            TextBoxOutputFolder = new System.Windows.Forms.TextBox();
            ButtonBrowse = new System.Windows.Forms.Button();
            RadioButtonExtractAll = new System.Windows.Forms.RadioButton();
            RadioButtonExtractSelected = new System.Windows.Forms.RadioButton();
            label2 = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            OpenTargetDirectory = new System.Windows.Forms.OpenFileDialog();
            FolderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            ExtractToTablePanel.SuspendLayout();
            HeaderPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)HeaderPicture).BeginInit();
            SettingsGroup1Panel.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            DirectoryPathTextBox.SuspendLayout();
            SuspendLayout();
            // 
            // ExtractToTablePanel
            // 
            ExtractToTablePanel.ColumnCount = 3;
            ExtractToTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30.40752F));
            ExtractToTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 69.59248F));
            ExtractToTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 132F));
            ExtractToTablePanel.Controls.Add(HeaderPanel, 0, 0);
            ExtractToTablePanel.Controls.Add(ButtonOk, 1, 2);
            ExtractToTablePanel.Controls.Add(ButtonCancel, 2, 2);
            ExtractToTablePanel.Controls.Add(SettingsGroup1Panel, 0, 1);
            ExtractToTablePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            ExtractToTablePanel.Location = new System.Drawing.Point(0, 0);
            ExtractToTablePanel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            ExtractToTablePanel.Name = "ExtractToTablePanel";
            ExtractToTablePanel.RowCount = 3;
            ExtractToTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            ExtractToTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            ExtractToTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 78F));
            ExtractToTablePanel.Size = new System.Drawing.Size(576, 535);
            ExtractToTablePanel.TabIndex = 0;
            // 
            // HeaderPanel
            // 
            HeaderPanel.BackColor = System.Drawing.Color.White;
            ExtractToTablePanel.SetColumnSpan(HeaderPanel, 3);
            HeaderPanel.Controls.Add(HeaderLabel);
            HeaderPanel.Controls.Add(HeaderPicture);
            HeaderPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            HeaderPanel.Location = new System.Drawing.Point(3, 0);
            HeaderPanel.Margin = new System.Windows.Forms.Padding(3, 0, 3, 2);
            HeaderPanel.Name = "HeaderPanel";
            HeaderPanel.Size = new System.Drawing.Size(570, 98);
            HeaderPanel.TabIndex = 0;
            // 
            // HeaderLabel
            // 
            HeaderLabel.AutoSize = true;
            HeaderLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            HeaderLabel.Location = new System.Drawing.Point(111, 22);
            HeaderLabel.Name = "HeaderLabel";
            HeaderLabel.Size = new System.Drawing.Size(321, 28);
            HeaderLabel.TabIndex = 1;
            HeaderLabel.Text = "Extract selected pages from Archive";
            // 
            // HeaderPicture
            // 
            HeaderPicture.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            HeaderPicture.Image = Properties.Resources.box_out_large;
            HeaderPicture.Location = new System.Drawing.Point(24, 0);
            HeaderPicture.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            HeaderPicture.Name = "HeaderPicture";
            HeaderPicture.Size = new System.Drawing.Size(65, 95);
            HeaderPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            HeaderPicture.TabIndex = 0;
            HeaderPicture.TabStop = false;
            // 
            // ButtonOk
            // 
            ButtonOk.Anchor = System.Windows.Forms.AnchorStyles.Right;
            ButtonOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            ButtonOk.Location = new System.Drawing.Point(329, 475);
            ButtonOk.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            ButtonOk.Name = "ButtonOk";
            ButtonOk.Size = new System.Drawing.Size(111, 41);
            ButtonOk.TabIndex = 2;
            ButtonOk.Text = "Ok";
            ButtonOk.UseVisualStyleBackColor = true;
            ButtonOk.Click += ButtonOk_Click;
            // 
            // ButtonCancel
            // 
            ButtonCancel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            ButtonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            ButtonCancel.Location = new System.Drawing.Point(454, 475);
            ButtonCancel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            ButtonCancel.Name = "ButtonCancel";
            ButtonCancel.Size = new System.Drawing.Size(119, 41);
            ButtonCancel.TabIndex = 3;
            ButtonCancel.Text = "Cancel";
            ButtonCancel.UseVisualStyleBackColor = true;
            ButtonCancel.Click += ButtonCancel_Click;
            // 
            // SettingsGroup1Panel
            // 
            ExtractToTablePanel.SetColumnSpan(SettingsGroup1Panel, 3);
            SettingsGroup1Panel.Controls.Add(label3);
            SettingsGroup1Panel.Controls.Add(tableLayoutPanel1);
            SettingsGroup1Panel.Controls.Add(RadioButtonExtractAll);
            SettingsGroup1Panel.Controls.Add(RadioButtonExtractSelected);
            SettingsGroup1Panel.Controls.Add(label2);
            SettingsGroup1Panel.Controls.Add(label1);
            SettingsGroup1Panel.Location = new System.Drawing.Point(3, 102);
            SettingsGroup1Panel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            SettingsGroup1Panel.Name = "SettingsGroup1Panel";
            SettingsGroup1Panel.Size = new System.Drawing.Size(570, 353);
            SettingsGroup1Panel.TabIndex = 4;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(68, 191);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(85, 20);
            label3.TabIndex = 6;
            label3.Text = "Destination";
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 89.11175F));
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10.88825F));
            tableLayoutPanel1.Controls.Add(DirectoryPathTextBox, 0, 0);
            tableLayoutPanel1.Location = new System.Drawing.Point(68, 215);
            tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanel1.Size = new System.Drawing.Size(427, 33);
            tableLayoutPanel1.TabIndex = 5;
            // 
            // DirectoryPathTextBox
            // 
            DirectoryPathTextBox.BackColor = System.Drawing.SystemColors.ControlLightLight;
            DirectoryPathTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            DirectoryPathTextBox.Controls.Add(TextBoxOutputFolder);
            DirectoryPathTextBox.Controls.Add(ButtonBrowse);
            DirectoryPathTextBox.Location = new System.Drawing.Point(5, 2);
            DirectoryPathTextBox.Margin = new System.Windows.Forms.Padding(5, 2, 3, 2);
            DirectoryPathTextBox.Name = "DirectoryPathTextBox";
            DirectoryPathTextBox.Padding = new System.Windows.Forms.Padding(1);
            DirectoryPathTextBox.Size = new System.Drawing.Size(372, 29);
            DirectoryPathTextBox.TabIndex = 27;
            // 
            // TextBoxOutputFolder
            // 
            TextBoxOutputFolder.BorderStyle = System.Windows.Forms.BorderStyle.None;
            TextBoxOutputFolder.Dock = System.Windows.Forms.DockStyle.Fill;
            TextBoxOutputFolder.Location = new System.Drawing.Point(1, 1);
            TextBoxOutputFolder.Margin = new System.Windows.Forms.Padding(4, 3, 3, 2);
            TextBoxOutputFolder.Name = "TextBoxOutputFolder";
            TextBoxOutputFolder.Size = new System.Drawing.Size(343, 20);
            TextBoxOutputFolder.TabIndex = 22;
            // 
            // ButtonBrowse
            // 
            ButtonBrowse.Dock = System.Windows.Forms.DockStyle.Right;
            ButtonBrowse.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            ButtonBrowse.Image = Properties.Resources.folder_small;
            ButtonBrowse.Location = new System.Drawing.Point(344, 1);
            ButtonBrowse.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            ButtonBrowse.Name = "ButtonBrowse";
            ButtonBrowse.Size = new System.Drawing.Size(25, 25);
            ButtonBrowse.TabIndex = 22;
            ButtonBrowse.Text = "...";
            ButtonBrowse.UseVisualStyleBackColor = true;
            ButtonBrowse.Click += ButtonBrowse_Click;
            // 
            // RadioButtonExtractAll
            // 
            RadioButtonExtractAll.AutoSize = true;
            RadioButtonExtractAll.Checked = true;
            RadioButtonExtractAll.Location = new System.Drawing.Point(71, 138);
            RadioButtonExtractAll.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            RadioButtonExtractAll.Name = "RadioButtonExtractAll";
            RadioButtonExtractAll.Size = new System.Drawing.Size(90, 24);
            RadioButtonExtractAll.TabIndex = 4;
            RadioButtonExtractAll.TabStop = true;
            RadioButtonExtractAll.Text = "All Pages";
            RadioButtonExtractAll.UseVisualStyleBackColor = true;
            RadioButtonExtractAll.CheckedChanged += RadioButtonExtractAll_CheckedChanged;
            // 
            // RadioButtonExtractSelected
            // 
            RadioButtonExtractSelected.AutoSize = true;
            RadioButtonExtractSelected.Enabled = false;
            RadioButtonExtractSelected.Location = new System.Drawing.Point(71, 82);
            RadioButtonExtractSelected.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            RadioButtonExtractSelected.Name = "RadioButtonExtractSelected";
            RadioButtonExtractSelected.Size = new System.Drawing.Size(129, 24);
            RadioButtonExtractSelected.TabIndex = 3;
            RadioButtonExtractSelected.Text = "Selected Pages";
            RadioButtonExtractSelected.UseVisualStyleBackColor = true;
            RadioButtonExtractSelected.CheckedChanged += RadioButtonExtractSelected_CheckedChanged_1;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(24, 309);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(141, 20);
            label2.TabIndex = 2;
            label2.Text = "Click \"Ok\" to extract";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(21, 39);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(152, 20);
            label1.TabIndex = 1;
            label1.Text = "Select what to extract";
            // 
            // OpenTargetDirectory
            // 
            OpenTargetDirectory.AddExtension = false;
            OpenTargetDirectory.CheckFileExists = false;
            OpenTargetDirectory.Filter = "All Files (*.*)|*.*";
            // 
            // FolderBrowserDialog
            // 
            FolderBrowserDialog.RootFolder = System.Environment.SpecialFolder.MyComputer;
            // 
            // ExtractFilesDialog
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(576, 535);
            Controls.Add(ExtractToTablePanel);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            Name = "ExtractFilesDialog";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Extract Pages...";
            FormClosing += ExtractFilesDialog_FormClosing;
            Shown += ExtractFilesDialog_Shown;
            ExtractToTablePanel.ResumeLayout(false);
            HeaderPanel.ResumeLayout(false);
            HeaderPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)HeaderPicture).EndInit();
            SettingsGroup1Panel.ResumeLayout(false);
            SettingsGroup1Panel.PerformLayout();
            tableLayoutPanel1.ResumeLayout(false);
            DirectoryPathTextBox.ResumeLayout(false);
            DirectoryPathTextBox.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel ExtractToTablePanel;
        private System.Windows.Forms.Panel HeaderPanel;
        private System.Windows.Forms.PictureBox HeaderPicture;
        private System.Windows.Forms.Button ButtonOk;
        private System.Windows.Forms.Button ButtonCancel;
        private System.Windows.Forms.Label HeaderLabel;
        private System.Windows.Forms.OpenFileDialog OpenTargetDirectory;
        private System.Windows.Forms.FolderBrowserDialog FolderBrowserDialog;
        private System.Windows.Forms.Panel SettingsGroup1Panel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel DirectoryPathTextBox;
        private System.Windows.Forms.TextBox TextBoxOutputFolder;
        private System.Windows.Forms.Button ButtonBrowse;
        private System.Windows.Forms.RadioButton RadioButtonExtractAll;
        private System.Windows.Forms.RadioButton RadioButtonExtractSelected;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
    }
}