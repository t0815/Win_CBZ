﻿namespace Win_CBZ.Forms
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
            this.MetaDataTablePanel = new System.Windows.Forms.TableLayoutPanel();
            this.HeaderPanel = new System.Windows.Forms.Panel();
            this.HeaderLabel = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.ButtonCancel = new System.Windows.Forms.Button();
            this.SettingsGroup1Panel = new System.Windows.Forms.Panel();
            this.metaDataView = new System.Windows.Forms.WebBrowser();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.MetaDataTablePanel.SuspendLayout();
            this.HeaderPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SettingsGroup1Panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // MetaDataTablePanel
            // 
            this.MetaDataTablePanel.ColumnCount = 3;
            this.MetaDataTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 1.462523F));
            this.MetaDataTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 98.53748F));
            this.MetaDataTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 157F));
            this.MetaDataTablePanel.Controls.Add(this.HeaderPanel, 0, 0);
            this.MetaDataTablePanel.Controls.Add(this.ButtonCancel, 2, 2);
            this.MetaDataTablePanel.Controls.Add(this.SettingsGroup1Panel, 1, 1);
            this.MetaDataTablePanel.Controls.Add(this.button1, 1, 2);
            this.MetaDataTablePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MetaDataTablePanel.Location = new System.Drawing.Point(0, 0);
            this.MetaDataTablePanel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MetaDataTablePanel.Name = "MetaDataTablePanel";
            this.MetaDataTablePanel.RowCount = 3;
            this.MetaDataTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 13.41223F));
            this.MetaDataTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 86.58777F));
            this.MetaDataTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 62F));
            this.MetaDataTablePanel.Size = new System.Drawing.Size(873, 687);
            this.MetaDataTablePanel.TabIndex = 1;
            // 
            // HeaderPanel
            // 
            this.HeaderPanel.BackColor = System.Drawing.Color.White;
            this.MetaDataTablePanel.SetColumnSpan(this.HeaderPanel, 3);
            this.HeaderPanel.Controls.Add(this.HeaderLabel);
            this.HeaderPanel.Controls.Add(this.pictureBox1);
            this.HeaderPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.HeaderPanel.Location = new System.Drawing.Point(3, 2);
            this.HeaderPanel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.HeaderPanel.Name = "HeaderPanel";
            this.HeaderPanel.Size = new System.Drawing.Size(867, 79);
            this.HeaderPanel.TabIndex = 0;
            // 
            // HeaderLabel
            // 
            this.HeaderLabel.AutoSize = true;
            this.HeaderLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HeaderLabel.Location = new System.Drawing.Point(108, 25);
            this.HeaderLabel.Name = "HeaderLabel";
            this.HeaderLabel.Size = new System.Drawing.Size(186, 28);
            this.HeaderLabel.TabIndex = 1;
            this.HeaderLabel.Text = "Metadata XML View";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Win_CBZ.Properties.Resources.info_dialog;
            this.pictureBox1.Location = new System.Drawing.Point(19, 6);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(67, 62);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // ButtonCancel
            // 
            this.ButtonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ButtonCancel.Location = new System.Drawing.Point(726, 644);
            this.ButtonCancel.Margin = new System.Windows.Forms.Padding(11, 20, 3, 2);
            this.ButtonCancel.Name = "ButtonCancel";
            this.ButtonCancel.Padding = new System.Windows.Forms.Padding(5);
            this.ButtonCancel.Size = new System.Drawing.Size(119, 33);
            this.ButtonCancel.TabIndex = 3;
            this.ButtonCancel.Text = "Close";
            this.ButtonCancel.UseVisualStyleBackColor = true;
            this.ButtonCancel.Click += new System.EventHandler(this.ButtonCancel_Click);
            // 
            // SettingsGroup1Panel
            // 
            this.MetaDataTablePanel.SetColumnSpan(this.SettingsGroup1Panel, 2);
            this.SettingsGroup1Panel.Controls.Add(this.metaDataView);
            this.SettingsGroup1Panel.Controls.Add(this.label1);
            this.SettingsGroup1Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SettingsGroup1Panel.Location = new System.Drawing.Point(13, 85);
            this.SettingsGroup1Panel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.SettingsGroup1Panel.Name = "SettingsGroup1Panel";
            this.SettingsGroup1Panel.Size = new System.Drawing.Size(857, 537);
            this.SettingsGroup1Panel.TabIndex = 4;
            // 
            // metaDataView
            // 
            this.metaDataView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.metaDataView.Location = new System.Drawing.Point(0, 0);
            this.metaDataView.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.metaDataView.MinimumSize = new System.Drawing.Size(20, 20);
            this.metaDataView.Name = "metaDataView";
            this.metaDataView.Size = new System.Drawing.Size(857, 537);
            this.metaDataView.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "XML";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(13, 644);
            this.button1.Margin = new System.Windows.Forms.Padding(3, 20, 3, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(102, 33);
            this.button1.TabIndex = 5;
            this.button1.Text = "Copy";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // MetaDataForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(873, 687);
            this.Controls.Add(this.MetaDataTablePanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "MetaDataForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Metadata";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MetaDataForm_FormClosed);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.MetaDataForm_KeyUp);
            this.MetaDataTablePanel.ResumeLayout(false);
            this.HeaderPanel.ResumeLayout(false);
            this.HeaderPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.SettingsGroup1Panel.ResumeLayout(false);
            this.SettingsGroup1Panel.PerformLayout();
            this.ResumeLayout(false);

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