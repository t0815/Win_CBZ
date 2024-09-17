namespace Win_CBZ.Forms
{
    partial class MetaDataForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MetaDataForm));
            MetaDataTablePanel = new System.Windows.Forms.TableLayoutPanel();
            HeaderPanel = new System.Windows.Forms.Panel();
            HeaderLabel = new System.Windows.Forms.Label();
            pictureBox1 = new System.Windows.Forms.PictureBox();
            ButtonCancel = new System.Windows.Forms.Button();
            SettingsGroup1Panel = new System.Windows.Forms.Panel();
            metaDataView = new System.Windows.Forms.WebBrowser();
            label1 = new System.Windows.Forms.Label();
            button1 = new System.Windows.Forms.Button();
            MetaDataTablePanel.SuspendLayout();
            HeaderPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SettingsGroup1Panel.SuspendLayout();
            SuspendLayout();
            // 
            // MetaDataTablePanel
            // 
            MetaDataTablePanel.ColumnCount = 3;
            MetaDataTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 1.462523F));
            MetaDataTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 98.53748F));
            MetaDataTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 157F));
            MetaDataTablePanel.Controls.Add(HeaderPanel, 0, 0);
            MetaDataTablePanel.Controls.Add(ButtonCancel, 2, 2);
            MetaDataTablePanel.Controls.Add(SettingsGroup1Panel, 1, 1);
            MetaDataTablePanel.Controls.Add(button1, 1, 2);
            MetaDataTablePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            MetaDataTablePanel.Location = new System.Drawing.Point(0, 0);
            MetaDataTablePanel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            MetaDataTablePanel.Name = "MetaDataTablePanel";
            MetaDataTablePanel.RowCount = 3;
            MetaDataTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 13.41223F));
            MetaDataTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 86.58777F));
            MetaDataTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 78F));
            MetaDataTablePanel.Size = new System.Drawing.Size(873, 859);
            MetaDataTablePanel.TabIndex = 1;
            // 
            // HeaderPanel
            // 
            HeaderPanel.BackColor = System.Drawing.Color.White;
            MetaDataTablePanel.SetColumnSpan(HeaderPanel, 3);
            HeaderPanel.Controls.Add(HeaderLabel);
            HeaderPanel.Controls.Add(pictureBox1);
            HeaderPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            HeaderPanel.Location = new System.Drawing.Point(3, 0);
            HeaderPanel.Margin = new System.Windows.Forms.Padding(3, 0, 3, 2);
            HeaderPanel.Name = "HeaderPanel";
            HeaderPanel.Size = new System.Drawing.Size(867, 102);
            HeaderPanel.TabIndex = 0;
            // 
            // HeaderLabel
            // 
            HeaderLabel.AutoSize = true;
            HeaderLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            HeaderLabel.Location = new System.Drawing.Point(108, 31);
            HeaderLabel.Name = "HeaderLabel";
            HeaderLabel.Size = new System.Drawing.Size(186, 28);
            HeaderLabel.TabIndex = 1;
            HeaderLabel.Text = "Metadata XML View";
            // 
            // pictureBox1
            // 
            pictureBox1.Image = Properties.Resources.info_dialog;
            pictureBox1.Location = new System.Drawing.Point(19, 8);
            pictureBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new System.Drawing.Size(67, 78);
            pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // ButtonCancel
            // 
            ButtonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            ButtonCancel.Location = new System.Drawing.Point(726, 805);
            ButtonCancel.Margin = new System.Windows.Forms.Padding(11, 25, 3, 2);
            ButtonCancel.Name = "ButtonCancel";
            ButtonCancel.Padding = new System.Windows.Forms.Padding(5, 6, 5, 6);
            ButtonCancel.Size = new System.Drawing.Size(119, 41);
            ButtonCancel.TabIndex = 3;
            ButtonCancel.Text = "Close";
            ButtonCancel.UseVisualStyleBackColor = true;
            ButtonCancel.Click += ButtonCancel_Click;
            // 
            // SettingsGroup1Panel
            // 
            MetaDataTablePanel.SetColumnSpan(SettingsGroup1Panel, 2);
            SettingsGroup1Panel.Controls.Add(metaDataView);
            SettingsGroup1Panel.Controls.Add(label1);
            SettingsGroup1Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            SettingsGroup1Panel.Location = new System.Drawing.Point(13, 106);
            SettingsGroup1Panel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            SettingsGroup1Panel.Name = "SettingsGroup1Panel";
            SettingsGroup1Panel.Size = new System.Drawing.Size(857, 672);
            SettingsGroup1Panel.TabIndex = 4;
            // 
            // metaDataView
            // 
            metaDataView.Dock = System.Windows.Forms.DockStyle.Fill;
            metaDataView.Location = new System.Drawing.Point(0, 0);
            metaDataView.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            metaDataView.MinimumSize = new System.Drawing.Size(20, 25);
            metaDataView.Name = "metaDataView";
            metaDataView.Size = new System.Drawing.Size(857, 672);
            metaDataView.TabIndex = 3;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(3, 9);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(38, 20);
            label1.TabIndex = 1;
            label1.Text = "XML";
            // 
            // button1
            // 
            button1.Location = new System.Drawing.Point(13, 805);
            button1.Margin = new System.Windows.Forms.Padding(3, 25, 3, 4);
            button1.Name = "button1";
            button1.Size = new System.Drawing.Size(102, 41);
            button1.TabIndex = 5;
            button1.Text = "Copy";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // MetaDataForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(873, 859);
            Controls.Add(MetaDataTablePanel);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            KeyPreview = true;
            Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            Name = "MetaDataForm";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Metadata";
            FormClosed += MetaDataForm_FormClosed;
            KeyUp += MetaDataForm_KeyUp;
            MetaDataTablePanel.ResumeLayout(false);
            HeaderPanel.ResumeLayout(false);
            HeaderPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            SettingsGroup1Panel.ResumeLayout(false);
            SettingsGroup1Panel.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel MetaDataTablePanel;
        private System.Windows.Forms.Panel HeaderPanel;
        private System.Windows.Forms.Label HeaderLabel;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button ButtonCancel;
        private System.Windows.Forms.Panel SettingsGroup1Panel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.WebBrowser metaDataView;
        private System.Windows.Forms.Button button1;
    }
}