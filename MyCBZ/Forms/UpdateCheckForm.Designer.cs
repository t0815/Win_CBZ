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
            OkButton = new System.Windows.Forms.Button();
            BookmarkEditorTableLayout.SuspendLayout();
            HeaderPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)HeaderPicture).BeginInit();
            SuspendLayout();
            // 
            // BookmarkEditorTableLayout
            // 
            BookmarkEditorTableLayout.ColumnCount = 5;
            BookmarkEditorTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 52F));
            BookmarkEditorTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            BookmarkEditorTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 158F));
            BookmarkEditorTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 134F));
            BookmarkEditorTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            BookmarkEditorTableLayout.Controls.Add(HeaderPanel, 0, 0);
            BookmarkEditorTableLayout.Controls.Add(OkButton, 3, 8);
            BookmarkEditorTableLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            BookmarkEditorTableLayout.Location = new System.Drawing.Point(0, 0);
            BookmarkEditorTableLayout.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            BookmarkEditorTableLayout.Name = "BookmarkEditorTableLayout";
            BookmarkEditorTableLayout.RowCount = 9;
            BookmarkEditorTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 79F));
            BookmarkEditorTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            BookmarkEditorTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            BookmarkEditorTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            BookmarkEditorTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 33F));
            BookmarkEditorTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
            BookmarkEditorTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 83F));
            BookmarkEditorTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            BookmarkEditorTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 53F));
            BookmarkEditorTableLayout.Size = new System.Drawing.Size(484, 339);
            BookmarkEditorTableLayout.TabIndex = 3;
            // 
            // HeaderPanel
            // 
            HeaderPanel.BackColor = System.Drawing.Color.White;
            BookmarkEditorTableLayout.SetColumnSpan(HeaderPanel, 5);
            HeaderPanel.Controls.Add(HeaderLabel);
            HeaderPanel.Controls.Add(HeaderPicture);
            HeaderPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            HeaderPanel.Location = new System.Drawing.Point(3, 0);
            HeaderPanel.Margin = new System.Windows.Forms.Padding(3, 0, 3, 2);
            HeaderPanel.Name = "HeaderPanel";
            HeaderPanel.Size = new System.Drawing.Size(478, 77);
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
            HeaderPicture.Image = Properties.Resources.box_software;
            HeaderPicture.Location = new System.Drawing.Point(21, 2);
            HeaderPicture.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            HeaderPicture.Name = "HeaderPicture";
            HeaderPicture.Size = new System.Drawing.Size(65, 64);
            HeaderPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            HeaderPicture.TabIndex = 0;
            HeaderPicture.TabStop = false;
            // 
            // OkButton
            // 
            OkButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            OkButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            OkButton.Location = new System.Drawing.Point(360, 291);
            OkButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            OkButton.Name = "OkButton";
            OkButton.Size = new System.Drawing.Size(100, 35);
            OkButton.TabIndex = 3;
            OkButton.Text = "Ok";
            OkButton.UseVisualStyleBackColor = true;
            // 
            // UpdateCheckForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(484, 339);
            Controls.Add(BookmarkEditorTableLayout);
            Name = "UpdateCheckForm";
            Text = "UpdateCheckForm";
            BookmarkEditorTableLayout.ResumeLayout(false);
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
        private System.Windows.Forms.Button OkButton;
    }
}