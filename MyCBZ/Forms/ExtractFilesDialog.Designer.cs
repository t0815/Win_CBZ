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
            this.ExtractToTablePanel = new System.Windows.Forms.TableLayoutPanel();
            this.HeaderPanel = new System.Windows.Forms.Panel();
            this.HeaderLabel = new System.Windows.Forms.Label();
            this.HeaderPicture = new System.Windows.Forms.PictureBox();
            this.ButtonOk = new System.Windows.Forms.Button();
            this.ButtonCancel = new System.Windows.Forms.Button();
            this.SettingsGroup1Panel = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.TextBoxOutputFolder = new System.Windows.Forms.TextBox();
            this.ButtonBrowse = new System.Windows.Forms.Button();
            this.RadioButtonExtractSelected = new System.Windows.Forms.RadioButton();
            this.RadioButtonExtractAll = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.OpenTargetDirectory = new System.Windows.Forms.OpenFileDialog();
            this.FolderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.ExtractToTablePanel.SuspendLayout();
            this.HeaderPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.HeaderPicture)).BeginInit();
            this.SettingsGroup1Panel.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ExtractToTablePanel
            // 
            this.ExtractToTablePanel.ColumnCount = 3;
            this.ExtractToTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30.40752F));
            this.ExtractToTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 69.59248F));
            this.ExtractToTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 99F));
            this.ExtractToTablePanel.Controls.Add(this.HeaderPanel, 0, 0);
            this.ExtractToTablePanel.Controls.Add(this.ButtonOk, 1, 2);
            this.ExtractToTablePanel.Controls.Add(this.ButtonCancel, 2, 2);
            this.ExtractToTablePanel.Controls.Add(this.SettingsGroup1Panel, 0, 1);
            this.ExtractToTablePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ExtractToTablePanel.Location = new System.Drawing.Point(0, 0);
            this.ExtractToTablePanel.Margin = new System.Windows.Forms.Padding(2);
            this.ExtractToTablePanel.Name = "ExtractToTablePanel";
            this.ExtractToTablePanel.RowCount = 3;
            this.ExtractToTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 65F));
            this.ExtractToTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ExtractToTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.ExtractToTablePanel.Size = new System.Drawing.Size(432, 348);
            this.ExtractToTablePanel.TabIndex = 0;
            // 
            // HeaderPanel
            // 
            this.HeaderPanel.BackColor = System.Drawing.Color.White;
            this.ExtractToTablePanel.SetColumnSpan(this.HeaderPanel, 3);
            this.HeaderPanel.Controls.Add(this.HeaderLabel);
            this.HeaderPanel.Controls.Add(this.HeaderPicture);
            this.HeaderPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.HeaderPanel.Location = new System.Drawing.Point(2, 2);
            this.HeaderPanel.Margin = new System.Windows.Forms.Padding(2);
            this.HeaderPanel.Name = "HeaderPanel";
            this.HeaderPanel.Size = new System.Drawing.Size(428, 61);
            this.HeaderPanel.TabIndex = 0;
            // 
            // HeaderLabel
            // 
            this.HeaderLabel.AutoSize = true;
            this.HeaderLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HeaderLabel.Location = new System.Drawing.Point(83, 15);
            this.HeaderLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.HeaderLabel.Name = "HeaderLabel";
            this.HeaderLabel.Size = new System.Drawing.Size(255, 21);
            this.HeaderLabel.TabIndex = 1;
            this.HeaderLabel.Text = "Extract selected pages from Archive";
            // 
            // HeaderPicture
            // 
            this.HeaderPicture.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.HeaderPicture.Image = global::Win_CBZ.Properties.Resources.box_out_large;
            this.HeaderPicture.Location = new System.Drawing.Point(18, 0);
            this.HeaderPicture.Margin = new System.Windows.Forms.Padding(2);
            this.HeaderPicture.Name = "HeaderPicture";
            this.HeaderPicture.Size = new System.Drawing.Size(49, 59);
            this.HeaderPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.HeaderPicture.TabIndex = 0;
            this.HeaderPicture.TabStop = false;
            // 
            // ButtonOk
            // 
            this.ButtonOk.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.ButtonOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.ButtonOk.Location = new System.Drawing.Point(247, 309);
            this.ButtonOk.Margin = new System.Windows.Forms.Padding(2);
            this.ButtonOk.Name = "ButtonOk";
            this.ButtonOk.Size = new System.Drawing.Size(83, 27);
            this.ButtonOk.TabIndex = 2;
            this.ButtonOk.Text = "Ok";
            this.ButtonOk.UseVisualStyleBackColor = true;
            this.ButtonOk.Click += new System.EventHandler(this.ButtonOk_Click);
            // 
            // ButtonCancel
            // 
            this.ButtonCancel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.ButtonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ButtonCancel.Location = new System.Drawing.Point(341, 309);
            this.ButtonCancel.Margin = new System.Windows.Forms.Padding(2);
            this.ButtonCancel.Name = "ButtonCancel";
            this.ButtonCancel.Size = new System.Drawing.Size(89, 27);
            this.ButtonCancel.TabIndex = 3;
            this.ButtonCancel.Text = "Cancel";
            this.ButtonCancel.UseVisualStyleBackColor = true;
            this.ButtonCancel.Click += new System.EventHandler(this.ButtonCancel_Click);
            // 
            // SettingsGroup1Panel
            // 
            this.ExtractToTablePanel.SetColumnSpan(this.SettingsGroup1Panel, 3);
            this.SettingsGroup1Panel.Controls.Add(this.label3);
            this.SettingsGroup1Panel.Controls.Add(this.tableLayoutPanel1);
            this.SettingsGroup1Panel.Controls.Add(this.RadioButtonExtractSelected);
            this.SettingsGroup1Panel.Controls.Add(this.RadioButtonExtractAll);
            this.SettingsGroup1Panel.Controls.Add(this.label2);
            this.SettingsGroup1Panel.Controls.Add(this.label1);
            this.SettingsGroup1Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SettingsGroup1Panel.Location = new System.Drawing.Point(2, 67);
            this.SettingsGroup1Panel.Margin = new System.Windows.Forms.Padding(2);
            this.SettingsGroup1Panel.Name = "SettingsGroup1Panel";
            this.SettingsGroup1Panel.Size = new System.Drawing.Size(428, 229);
            this.SettingsGroup1Panel.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(51, 124);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Destination";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 89.11175F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10.88825F));
            this.tableLayoutPanel1.Controls.Add(this.TextBoxOutputFolder, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.ButtonBrowse, 1, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(51, 140);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(320, 26);
            this.tableLayoutPanel1.TabIndex = 5;
            // 
            // TextBoxOutputFolder
            // 
            this.TextBoxOutputFolder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TextBoxOutputFolder.Location = new System.Drawing.Point(2, 2);
            this.TextBoxOutputFolder.Margin = new System.Windows.Forms.Padding(2);
            this.TextBoxOutputFolder.Name = "TextBoxOutputFolder";
            this.TextBoxOutputFolder.Size = new System.Drawing.Size(281, 20);
            this.TextBoxOutputFolder.TabIndex = 0;
            // 
            // ButtonBrowse
            // 
            this.ButtonBrowse.Location = new System.Drawing.Point(287, 2);
            this.ButtonBrowse.Margin = new System.Windows.Forms.Padding(2);
            this.ButtonBrowse.Name = "ButtonBrowse";
            this.ButtonBrowse.Size = new System.Drawing.Size(24, 19);
            this.ButtonBrowse.TabIndex = 1;
            this.ButtonBrowse.Text = "...";
            this.ButtonBrowse.UseVisualStyleBackColor = true;
            this.ButtonBrowse.Click += new System.EventHandler(this.ButtonBrowse_Click);
            // 
            // RadioButtonExtractSelected
            // 
            this.RadioButtonExtractSelected.AutoSize = true;
            this.RadioButtonExtractSelected.Checked = true;
            this.RadioButtonExtractSelected.Location = new System.Drawing.Point(53, 89);
            this.RadioButtonExtractSelected.Margin = new System.Windows.Forms.Padding(2);
            this.RadioButtonExtractSelected.Name = "RadioButtonExtractSelected";
            this.RadioButtonExtractSelected.Size = new System.Drawing.Size(69, 17);
            this.RadioButtonExtractSelected.TabIndex = 4;
            this.RadioButtonExtractSelected.TabStop = true;
            this.RadioButtonExtractSelected.Text = "All Pages";
            this.RadioButtonExtractSelected.UseVisualStyleBackColor = true;
            this.RadioButtonExtractSelected.CheckedChanged += new System.EventHandler(this.RadioButtonExtractSelected_CheckedChanged);
            // 
            // RadioButtonExtractAll
            // 
            this.RadioButtonExtractAll.AutoSize = true;
            this.RadioButtonExtractAll.Enabled = false;
            this.RadioButtonExtractAll.Location = new System.Drawing.Point(53, 54);
            this.RadioButtonExtractAll.Margin = new System.Windows.Forms.Padding(2);
            this.RadioButtonExtractAll.Name = "RadioButtonExtractAll";
            this.RadioButtonExtractAll.Size = new System.Drawing.Size(100, 17);
            this.RadioButtonExtractAll.TabIndex = 3;
            this.RadioButtonExtractAll.Text = "Selected Pages";
            this.RadioButtonExtractAll.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 197);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(104, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Click \"Ok\" to extract";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 25);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(110, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Select what to extract";
            // 
            // OpenTargetDirectory
            // 
            this.OpenTargetDirectory.AddExtension = false;
            this.OpenTargetDirectory.CheckFileExists = false;
            this.OpenTargetDirectory.Filter = "All Files (*.*)|*.*";
            // 
            // FolderBrowserDialog
            // 
            this.FolderBrowserDialog.SelectedPath = ".\\";
            // 
            // ExtractFilesDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(432, 348);
            this.Controls.Add(this.ExtractToTablePanel);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "ExtractFilesDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Extract Pages...";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ExtractFilesDialog_FormClosing);
            this.Shown += new System.EventHandler(this.ExtractFilesDialog_Shown);
            this.ExtractToTablePanel.ResumeLayout(false);
            this.HeaderPanel.ResumeLayout(false);
            this.HeaderPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.HeaderPicture)).EndInit();
            this.SettingsGroup1Panel.ResumeLayout(false);
            this.SettingsGroup1Panel.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel ExtractToTablePanel;
        private System.Windows.Forms.Panel HeaderPanel;
        private System.Windows.Forms.PictureBox HeaderPicture;
        private System.Windows.Forms.Button ButtonOk;
        private System.Windows.Forms.Button ButtonCancel;
        private System.Windows.Forms.Label HeaderLabel;
        private System.Windows.Forms.Panel SettingsGroup1Panel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton RadioButtonExtractSelected;
        private System.Windows.Forms.RadioButton RadioButtonExtractAll;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TextBox TextBoxOutputFolder;
        private System.Windows.Forms.Button ButtonBrowse;
        private System.Windows.Forms.OpenFileDialog OpenTargetDirectory;
        private System.Windows.Forms.FolderBrowserDialog FolderBrowserDialog;
    }
}