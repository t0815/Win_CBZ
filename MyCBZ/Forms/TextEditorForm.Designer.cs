namespace Win_CBZ.Forms
{
    partial class TextEditorForm
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
            this.ItemEditorTableLayout = new System.Windows.Forms.TableLayoutPanel();
            this.HeaderPanel = new System.Windows.Forms.Panel();
            this.HeaderLabel = new System.Windows.Forms.Label();
            this.HeaderPicture = new System.Windows.Forms.PictureBox();
            this.OkButton = new System.Windows.Forms.Button();
            this.CancelButton = new System.Windows.Forms.Button();
            this.InfoLabel = new System.Windows.Forms.Label();
            this.ItemsText = new System.Windows.Forms.TextBox();
            this.ItemEditorToolBar = new System.Windows.Forms.ToolStrip();
            this.ToolButtonSortAscending = new System.Windows.Forms.ToolStripButton();
            this.ItemEditorTableLayout.SuspendLayout();
            this.HeaderPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.HeaderPicture)).BeginInit();
            this.ItemEditorToolBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // ItemEditorTableLayout
            // 
            this.ItemEditorTableLayout.ColumnCount = 2;
            this.ItemEditorTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ItemEditorTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.ItemEditorTableLayout.Controls.Add(this.HeaderPanel, 0, 0);
            this.ItemEditorTableLayout.Controls.Add(this.OkButton, 0, 4);
            this.ItemEditorTableLayout.Controls.Add(this.CancelButton, 1, 4);
            this.ItemEditorTableLayout.Controls.Add(this.InfoLabel, 0, 3);
            this.ItemEditorTableLayout.Controls.Add(this.ItemsText, 0, 2);
            this.ItemEditorTableLayout.Controls.Add(this.ItemEditorToolBar, 1, 0);
            this.ItemEditorTableLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ItemEditorTableLayout.Location = new System.Drawing.Point(0, 0);
            this.ItemEditorTableLayout.Name = "ItemEditorTableLayout";
            this.ItemEditorTableLayout.RowCount = 5;
            this.ItemEditorTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 49F));
            this.ItemEditorTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.ItemEditorTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ItemEditorTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.ItemEditorTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.ItemEditorTableLayout.Size = new System.Drawing.Size(347, 274);
            this.ItemEditorTableLayout.TabIndex = 0;
            this.ItemEditorTableLayout.Paint += new System.Windows.Forms.PaintEventHandler(this.ItemEditorTableLayout_Paint);
            // 
            // HeaderPanel
            // 
            this.HeaderPanel.BackColor = System.Drawing.Color.White;
            this.ItemEditorTableLayout.SetColumnSpan(this.HeaderPanel, 2);
            this.HeaderPanel.Controls.Add(this.HeaderLabel);
            this.HeaderPanel.Controls.Add(this.HeaderPicture);
            this.HeaderPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.HeaderPanel.Location = new System.Drawing.Point(2, 2);
            this.HeaderPanel.Margin = new System.Windows.Forms.Padding(2);
            this.HeaderPanel.Name = "HeaderPanel";
            this.HeaderPanel.Size = new System.Drawing.Size(343, 45);
            this.HeaderPanel.TabIndex = 2;
            // 
            // HeaderLabel
            // 
            this.HeaderLabel.AutoSize = true;
            this.HeaderLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HeaderLabel.Location = new System.Drawing.Point(71, 14);
            this.HeaderLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.HeaderLabel.Name = "HeaderLabel";
            this.HeaderLabel.Size = new System.Drawing.Size(86, 21);
            this.HeaderLabel.TabIndex = 1;
            this.HeaderLabel.Text = "Item Editor";
            // 
            // HeaderPicture
            // 
            this.HeaderPicture.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.HeaderPicture.Image = global::Win_CBZ.Properties.Resources.edit_large;
            this.HeaderPicture.Location = new System.Drawing.Point(18, 0);
            this.HeaderPicture.Margin = new System.Windows.Forms.Padding(2);
            this.HeaderPicture.Name = "HeaderPicture";
            this.HeaderPicture.Size = new System.Drawing.Size(49, 43);
            this.HeaderPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.HeaderPicture.TabIndex = 0;
            this.HeaderPicture.TabStop = false;
            // 
            // OkButton
            // 
            this.OkButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.OkButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OkButton.Location = new System.Drawing.Point(189, 242);
            this.OkButton.Name = "OkButton";
            this.OkButton.Size = new System.Drawing.Size(75, 23);
            this.OkButton.TabIndex = 3;
            this.OkButton.Text = "Ok";
            this.OkButton.UseVisualStyleBackColor = true;
            this.OkButton.Click += new System.EventHandler(this.OkButton_Click);
            // 
            // CancelButton
            // 
            this.CancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelButton.Location = new System.Drawing.Point(270, 242);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(74, 23);
            this.CancelButton.TabIndex = 4;
            this.CancelButton.Text = "Cancel";
            this.CancelButton.UseVisualStyleBackColor = true;
            this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // InfoLabel
            // 
            this.InfoLabel.AutoSize = true;
            this.InfoLabel.Location = new System.Drawing.Point(8, 217);
            this.InfoLabel.Margin = new System.Windows.Forms.Padding(8, 8, 3, 0);
            this.InfoLabel.Name = "InfoLabel";
            this.InfoLabel.Size = new System.Drawing.Size(86, 13);
            this.InfoLabel.TabIndex = 6;
            this.InfoLabel.Text = "One item per line";
            // 
            // ItemsText
            // 
            this.ItemsText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ItemEditorTableLayout.SetColumnSpan(this.ItemsText, 2);
            this.ItemsText.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ItemsText.Location = new System.Drawing.Point(3, 92);
            this.ItemsText.Multiline = true;
            this.ItemsText.Name = "ItemsText";
            this.ItemsText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.ItemsText.Size = new System.Drawing.Size(341, 114);
            this.ItemsText.TabIndex = 5;
            this.ItemsText.WordWrap = false;
            // 
            // ItemEditorToolBar
            // 
            this.ItemEditorToolBar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ItemEditorTableLayout.SetColumnSpan(this.ItemEditorToolBar, 2);
            this.ItemEditorToolBar.Dock = System.Windows.Forms.DockStyle.None;
            this.ItemEditorToolBar.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.ItemEditorToolBar.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.ItemEditorToolBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolButtonSortAscending});
            this.ItemEditorToolBar.Location = new System.Drawing.Point(320, 60);
            this.ItemEditorToolBar.Margin = new System.Windows.Forms.Padding(6, 0, 0, 2);
            this.ItemEditorToolBar.Name = "ItemEditorToolBar";
            this.ItemEditorToolBar.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.ItemEditorToolBar.Size = new System.Drawing.Size(27, 27);
            this.ItemEditorToolBar.Stretch = true;
            this.ItemEditorToolBar.TabIndex = 7;
            this.ItemEditorToolBar.Text = "toolStrip1";
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
            this.ToolButtonSortAscending.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // TextEditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(347, 274);
            this.Controls.Add(this.ItemEditorTableLayout);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MinimizeBox = false;
            this.Name = "TextEditorForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Item Editor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TextEditorForm_FormClosing);
            this.ItemEditorTableLayout.ResumeLayout(false);
            this.ItemEditorTableLayout.PerformLayout();
            this.HeaderPanel.ResumeLayout(false);
            this.HeaderPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.HeaderPicture)).EndInit();
            this.ItemEditorToolBar.ResumeLayout(false);
            this.ItemEditorToolBar.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel ItemEditorTableLayout;
        private System.Windows.Forms.Panel HeaderPanel;
        private System.Windows.Forms.Label HeaderLabel;
        private System.Windows.Forms.PictureBox HeaderPicture;
        private System.Windows.Forms.Button OkButton;
        private System.Windows.Forms.Button CancelButton;
        private System.Windows.Forms.TextBox ItemsText;
        private System.Windows.Forms.Label InfoLabel;
        private System.Windows.Forms.ToolStrip ItemEditorToolBar;
        private System.Windows.Forms.ToolStripButton ToolButtonSortAscending;
    }
}