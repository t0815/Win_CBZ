namespace Win_CBZ.Forms
{
    partial class UpdateCheckForm
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
            BookmarkEditorTableLayout = new System.Windows.Forms.TableLayoutPanel();
            HeaderPanel = new System.Windows.Forms.Panel();
            HeaderLabel = new System.Windows.Forms.Label();
            HeaderPicture = new System.Windows.Forms.PictureBox();
            LabelUpdateHeadline = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            LabelAppVersion = new System.Windows.Forms.Label();
            LabelNewVersion = new System.Windows.Forms.Label();
            OkButton = new System.Windows.Forms.Button();
            LabelUrl = new System.Windows.Forms.LinkLabel();
            label4 = new System.Windows.Forms.Label();
            TextBoxChanges = new System.Windows.Forms.TextBox();
            BookmarkEditorTableLayout.SuspendLayout();
            HeaderPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)HeaderPicture).BeginInit();
            SuspendLayout();
            // 
            // BookmarkEditorTableLayout
            // 
            BookmarkEditorTableLayout.ColumnCount = 5;
            BookmarkEditorTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 52F));
            BookmarkEditorTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 134F));
            BookmarkEditorTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            BookmarkEditorTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 134F));
            BookmarkEditorTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            BookmarkEditorTableLayout.Controls.Add(HeaderPanel, 0, 0);
            BookmarkEditorTableLayout.Controls.Add(LabelUpdateHeadline, 1, 2);
            BookmarkEditorTableLayout.Controls.Add(label2, 1, 3);
            BookmarkEditorTableLayout.Controls.Add(label3, 1, 4);
            BookmarkEditorTableLayout.Controls.Add(LabelAppVersion, 2, 3);
            BookmarkEditorTableLayout.Controls.Add(LabelNewVersion, 2, 4);
            BookmarkEditorTableLayout.Controls.Add(OkButton, 3, 9);
            BookmarkEditorTableLayout.Controls.Add(LabelUrl, 2, 7);
            BookmarkEditorTableLayout.Controls.Add(label4, 1, 7);
            BookmarkEditorTableLayout.Controls.Add(TextBoxChanges, 1, 6);
            BookmarkEditorTableLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            BookmarkEditorTableLayout.Location = new System.Drawing.Point(0, 0);
            BookmarkEditorTableLayout.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            BookmarkEditorTableLayout.Name = "BookmarkEditorTableLayout";
            BookmarkEditorTableLayout.RowCount = 10;
            BookmarkEditorTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 73F));
            BookmarkEditorTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 21F));
            BookmarkEditorTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            BookmarkEditorTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            BookmarkEditorTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            BookmarkEditorTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            BookmarkEditorTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            BookmarkEditorTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            BookmarkEditorTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 39F));
            BookmarkEditorTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 64F));
            BookmarkEditorTableLayout.Size = new System.Drawing.Size(572, 511);
            BookmarkEditorTableLayout.TabIndex = 3;
            // 
            // HeaderPanel
            // 
            HeaderPanel.BackColor = System.Drawing.Color.White;
            BookmarkEditorTableLayout.SetColumnSpan(HeaderPanel, 5);
            HeaderPanel.Controls.Add(HeaderLabel);
            HeaderPanel.Controls.Add(HeaderPicture);
            HeaderPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            HeaderPanel.Location = new System.Drawing.Point(0, 0);
            HeaderPanel.Margin = new System.Windows.Forms.Padding(0);
            HeaderPanel.Name = "HeaderPanel";
            HeaderPanel.Size = new System.Drawing.Size(572, 73);
            HeaderPanel.TabIndex = 2;
            // 
            // HeaderLabel
            // 
            HeaderLabel.AutoSize = true;
            HeaderLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            HeaderLabel.Location = new System.Drawing.Point(105, 23);
            HeaderLabel.Name = "HeaderLabel";
            HeaderLabel.Size = new System.Drawing.Size(167, 28);
            HeaderLabel.TabIndex = 1;
            HeaderLabel.Text = "Software Updates";
            // 
            // HeaderPicture
            // 
            HeaderPicture.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            HeaderPicture.Image = Properties.Resources.box_software48;
            HeaderPicture.Location = new System.Drawing.Point(21, 2);
            HeaderPicture.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            HeaderPicture.Name = "HeaderPicture";
            HeaderPicture.Size = new System.Drawing.Size(64, 69);
            HeaderPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            HeaderPicture.TabIndex = 0;
            HeaderPicture.TabStop = false;
            // 
            // LabelUpdateHeadline
            // 
            LabelUpdateHeadline.AutoSize = true;
            BookmarkEditorTableLayout.SetColumnSpan(LabelUpdateHeadline, 3);
            LabelUpdateHeadline.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            LabelUpdateHeadline.Location = new System.Drawing.Point(55, 94);
            LabelUpdateHeadline.Name = "LabelUpdateHeadline";
            LabelUpdateHeadline.Size = new System.Drawing.Size(233, 31);
            LabelUpdateHeadline.TabIndex = 4;
            LabelUpdateHeadline.Text = "No Updates available";
            // 
            // label2
            // 
            label2.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(55, 142);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(93, 20);
            label2.TabIndex = 5;
            label2.Text = "Your Version:";
            // 
            // label3
            // 
            label3.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(55, 176);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(100, 20);
            label3.TabIndex = 7;
            label3.Text = "Latest Version";
            // 
            // LabelAppVersion
            // 
            LabelAppVersion.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            LabelAppVersion.AutoSize = true;
            LabelAppVersion.Location = new System.Drawing.Point(189, 142);
            LabelAppVersion.Name = "LabelAppVersion";
            LabelAppVersion.Size = new System.Drawing.Size(17, 20);
            LabelAppVersion.TabIndex = 10;
            LabelAppVersion.Text = "0";
            // 
            // LabelNewVersion
            // 
            LabelNewVersion.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            LabelNewVersion.AutoSize = true;
            LabelNewVersion.Location = new System.Drawing.Point(189, 176);
            LabelNewVersion.Name = "LabelNewVersion";
            LabelNewVersion.Size = new System.Drawing.Size(17, 20);
            LabelNewVersion.TabIndex = 11;
            LabelNewVersion.Text = "0";
            // 
            // OkButton
            // 
            OkButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            OkButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            OkButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            OkButton.Location = new System.Drawing.Point(428, 452);
            OkButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            OkButton.Name = "OkButton";
            OkButton.Size = new System.Drawing.Size(120, 35);
            OkButton.TabIndex = 6;
            OkButton.Text = "Ok";
            OkButton.UseVisualStyleBackColor = true;
            OkButton.Click += OkButton_Click;
            // 
            // LabelUrl
            // 
            LabelUrl.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            LabelUrl.AutoEllipsis = true;
            LabelUrl.AutoSize = true;
            BookmarkEditorTableLayout.SetColumnSpan(LabelUrl, 2);
            LabelUrl.Location = new System.Drawing.Point(189, 388);
            LabelUrl.Name = "LabelUrl";
            LabelUrl.Size = new System.Drawing.Size(360, 20);
            LabelUrl.TabIndex = 8;
            LabelUrl.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            LabelUrl.Click += LabelUrl_Click;
            // 
            // label4
            // 
            label4.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(55, 388);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(78, 20);
            label4.TabIndex = 9;
            label4.Text = "Download";
            // 
            // TextBoxChanges
            // 
            TextBoxChanges.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            TextBoxChanges.BackColor = System.Drawing.SystemColors.Control;
            TextBoxChanges.BorderStyle = System.Windows.Forms.BorderStyle.None;
            BookmarkEditorTableLayout.SetColumnSpan(TextBoxChanges, 3);
            TextBoxChanges.Location = new System.Drawing.Point(55, 233);
            TextBoxChanges.Multiline = true;
            TextBoxChanges.Name = "TextBoxChanges";
            TextBoxChanges.ReadOnly = true;
            TextBoxChanges.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            TextBoxChanges.Size = new System.Drawing.Size(494, 137);
            TextBoxChanges.TabIndex = 12;
            // 
            // UpdateCheckForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(572, 511);
            Controls.Add(BookmarkEditorTableLayout);
            Name = "UpdateCheckForm";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "UpdateCheckForm";
            BookmarkEditorTableLayout.ResumeLayout(false);
            BookmarkEditorTableLayout.PerformLayout();
            HeaderPanel.ResumeLayout(false);
            HeaderPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)HeaderPicture).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel BookmarkEditorTableLayout;
        private System.Windows.Forms.Panel HeaderPanel;
        private System.Windows.Forms.Label HeaderLabel;
        private System.Windows.Forms.PictureBox HeaderPicture;
        private System.Windows.Forms.Label LabelUpdateHeadline;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button OkButton;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.LinkLabel LabelUrl;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label LabelAppVersion;
        private System.Windows.Forms.Label LabelNewVersion;
        private System.Windows.Forms.TextBox TextBoxChanges;
    }
}