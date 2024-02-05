namespace Win_CBZ.Forms
{
    partial class TagEditorForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TagEditorForm));
            this.ItemEditorTableLayout = new System.Windows.Forms.TableLayoutPanel();
            this.HeaderPanel = new System.Windows.Forms.Panel();
            this.HeaderLabel = new System.Windows.Forms.Label();
            this.HeaderPicture = new System.Windows.Forms.PictureBox();
            this.OkButton = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.ItemEditorToolBar = new System.Windows.Forms.ToolStrip();
            this.DeleteAllTagsToolButton = new System.Windows.Forms.ToolStripButton();
            this.ToolButtonSortAscending = new System.Windows.Forms.ToolStripButton();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.ButtonAddTag = new System.Windows.Forms.Button();
            this.TagTextBox = new System.Windows.Forms.TextBox();
            this.TagsList = new System.Windows.Forms.FlowLayoutPanel();
            this.AutoCompleteItems = new AutocompleteMenuNS.AutocompleteMenu();
            this.ItemEditorTableLayout.SuspendLayout();
            this.HeaderPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.HeaderPicture)).BeginInit();
            this.ItemEditorToolBar.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ItemEditorTableLayout
            // 
            this.ItemEditorTableLayout.ColumnCount = 2;
            this.ItemEditorTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ItemEditorTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 123F));
            this.ItemEditorTableLayout.Controls.Add(this.HeaderPanel, 0, 0);
            this.ItemEditorTableLayout.Controls.Add(this.OkButton, 0, 4);
            this.ItemEditorTableLayout.Controls.Add(this.CancelBtn, 1, 4);
            this.ItemEditorTableLayout.Controls.Add(this.ItemEditorToolBar, 1, 1);
            this.ItemEditorTableLayout.Controls.Add(this.flowLayoutPanel1, 0, 1);
            this.ItemEditorTableLayout.Controls.Add(this.TagsList, 0, 2);
            this.ItemEditorTableLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ItemEditorTableLayout.Location = new System.Drawing.Point(0, 0);
            this.ItemEditorTableLayout.Margin = new System.Windows.Forms.Padding(4);
            this.ItemEditorTableLayout.Name = "ItemEditorTableLayout";
            this.ItemEditorTableLayout.RowCount = 5;
            this.ItemEditorTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.ItemEditorTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 43F));
            this.ItemEditorTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ItemEditorTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.ItemEditorTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 43F));
            this.ItemEditorTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.ItemEditorTableLayout.Size = new System.Drawing.Size(497, 325);
            this.ItemEditorTableLayout.TabIndex = 1;
            // 
            // HeaderPanel
            // 
            this.HeaderPanel.BackColor = System.Drawing.Color.White;
            this.ItemEditorTableLayout.SetColumnSpan(this.HeaderPanel, 2);
            this.HeaderPanel.Controls.Add(this.HeaderLabel);
            this.HeaderPanel.Controls.Add(this.HeaderPicture);
            this.HeaderPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.HeaderPanel.Location = new System.Drawing.Point(3, 2);
            this.HeaderPanel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.HeaderPanel.Name = "HeaderPanel";
            this.HeaderPanel.Size = new System.Drawing.Size(491, 56);
            this.HeaderPanel.TabIndex = 2;
            // 
            // HeaderLabel
            // 
            this.HeaderLabel.AutoSize = true;
            this.HeaderLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HeaderLabel.Location = new System.Drawing.Point(95, 7);
            this.HeaderLabel.Name = "HeaderLabel";
            this.HeaderLabel.Size = new System.Drawing.Size(50, 28);
            this.HeaderLabel.TabIndex = 1;
            this.HeaderLabel.Text = "Tags";
            // 
            // HeaderPicture
            // 
            this.HeaderPicture.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.HeaderPicture.Image = global::Win_CBZ.Properties.Resources.tags;
            this.HeaderPicture.Location = new System.Drawing.Point(24, 0);
            this.HeaderPicture.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.HeaderPicture.Name = "HeaderPicture";
            this.HeaderPicture.Size = new System.Drawing.Size(65, 54);
            this.HeaderPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.HeaderPicture.TabIndex = 0;
            this.HeaderPicture.TabStop = false;
            // 
            // OkButton
            // 
            this.OkButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.OkButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OkButton.Location = new System.Drawing.Point(270, 286);
            this.OkButton.Margin = new System.Windows.Forms.Padding(4);
            this.OkButton.Name = "OkButton";
            this.OkButton.Size = new System.Drawing.Size(100, 28);
            this.OkButton.TabIndex = 3;
            this.OkButton.Text = "Ok";
            this.OkButton.UseVisualStyleBackColor = true;
            this.OkButton.Click += new System.EventHandler(this.OkButton_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelBtn.Location = new System.Drawing.Point(394, 286);
            this.CancelBtn.Margin = new System.Windows.Forms.Padding(4);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(99, 28);
            this.CancelBtn.TabIndex = 4;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // ItemEditorToolBar
            // 
            this.ItemEditorToolBar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ItemEditorToolBar.Dock = System.Windows.Forms.DockStyle.None;
            this.ItemEditorToolBar.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.ItemEditorToolBar.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.ItemEditorToolBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.DeleteAllTagsToolButton,
            this.ToolButtonSortAscending});
            this.ItemEditorToolBar.Location = new System.Drawing.Point(428, 74);
            this.ItemEditorToolBar.Margin = new System.Windows.Forms.Padding(8, 0, 8, 2);
            this.ItemEditorToolBar.Name = "ItemEditorToolBar";
            this.ItemEditorToolBar.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.ItemEditorToolBar.Size = new System.Drawing.Size(61, 27);
            this.ItemEditorToolBar.Stretch = true;
            this.ItemEditorToolBar.TabIndex = 7;
            this.ItemEditorToolBar.Text = "toolStrip1";
            // 
            // DeleteAllTagsToolButton
            // 
            this.DeleteAllTagsToolButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.DeleteAllTagsToolButton.Image = global::Win_CBZ.Properties.Resources.garbage;
            this.DeleteAllTagsToolButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.DeleteAllTagsToolButton.Name = "DeleteAllTagsToolButton";
            this.DeleteAllTagsToolButton.Size = new System.Drawing.Size(29, 24);
            this.DeleteAllTagsToolButton.Text = "toolStripButton1";
            this.DeleteAllTagsToolButton.Click += new System.EventHandler(this.DeleteAllTagsToolButton_Click);
            // 
            // ToolButtonSortAscending
            // 
            this.ToolButtonSortAscending.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ToolButtonSortAscending.Image = global::Win_CBZ.Properties.Resources.sort_az_ascending2;
            this.ToolButtonSortAscending.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ToolButtonSortAscending.Name = "ToolButtonSortAscending";
            this.ToolButtonSortAscending.Size = new System.Drawing.Size(29, 24);
            this.ToolButtonSortAscending.Text = "toolStripButton1";
            this.ToolButtonSortAscending.ToolTipText = "Sort items ascending";
            this.ToolButtonSortAscending.Click += new System.EventHandler(this.ToolButtonSortAscending_Click);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel1.Controls.Add(this.ButtonAddTag);
            this.flowLayoutPanel1.Controls.Add(this.TagTextBox);
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 73);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(368, 30);
            this.flowLayoutPanel1.TabIndex = 8;
            // 
            // ButtonAddTag
            // 
            this.ButtonAddTag.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ButtonAddTag.Image = global::Win_CBZ.Properties.Resources.navigate_plus;
            this.ButtonAddTag.Location = new System.Drawing.Point(331, 3);
            this.ButtonAddTag.Name = "ButtonAddTag";
            this.ButtonAddTag.Padding = new System.Windows.Forms.Padding(3);
            this.ButtonAddTag.Size = new System.Drawing.Size(34, 23);
            this.ButtonAddTag.TabIndex = 0;
            this.ButtonAddTag.UseVisualStyleBackColor = true;
            this.ButtonAddTag.Click += new System.EventHandler(this.ButtonAddTag_Click);
            // 
            // TagTextBox
            // 
            this.AutoCompleteItems.SetAutocompleteMenu(this.TagTextBox, this.AutoCompleteItems);
            this.TagTextBox.Location = new System.Drawing.Point(86, 3);
            this.TagTextBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.TagTextBox.Name = "TagTextBox";
            this.TagTextBox.Size = new System.Drawing.Size(239, 22);
            this.TagTextBox.TabIndex = 1;
            this.TagTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TagTextBox_KeyDown);
            this.TagTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TagTextBox_KeyPress);
            // 
            // TagsList
            // 
            this.TagsList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TagsList.AutoScroll = true;
            this.TagsList.BackColor = System.Drawing.SystemColors.Window;
            this.TagsList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ItemEditorTableLayout.SetColumnSpan(this.TagsList, 2);
            this.TagsList.Location = new System.Drawing.Point(8, 106);
            this.TagsList.Margin = new System.Windows.Forms.Padding(8, 3, 8, 3);
            this.TagsList.Name = "TagsList";
            this.TagsList.Size = new System.Drawing.Size(481, 153);
            this.TagsList.TabIndex = 9;
            // 
            // AutoCompleteItems
            // 
            this.AutoCompleteItems.Colors = ((AutocompleteMenuNS.Colors)(resources.GetObject("AutoCompleteItems.Colors")));
            this.AutoCompleteItems.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.AutoCompleteItems.ImageList = null;
            this.AutoCompleteItems.Items = new string[0];
            this.AutoCompleteItems.MinFragmentLength = 1;
            this.AutoCompleteItems.TargetControlWrapper = null;
            // 
            // TagEditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(497, 325);
            this.Controls.Add(this.ItemEditorTableLayout);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "TagEditorForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Tag Editor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TagEditor_FormClosing);
            this.Shown += new System.EventHandler(this.TagEditorForm_Shown);
            this.ItemEditorTableLayout.ResumeLayout(false);
            this.ItemEditorTableLayout.PerformLayout();
            this.HeaderPanel.ResumeLayout(false);
            this.HeaderPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.HeaderPicture)).EndInit();
            this.ItemEditorToolBar.ResumeLayout(false);
            this.ItemEditorToolBar.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel ItemEditorTableLayout;
        private System.Windows.Forms.Panel HeaderPanel;
        private System.Windows.Forms.Label HeaderLabel;
        private System.Windows.Forms.PictureBox HeaderPicture;
        private System.Windows.Forms.Button OkButton;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.ToolStrip ItemEditorToolBar;
        private System.Windows.Forms.ToolStripButton ToolButtonSortAscending;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button ButtonAddTag;
        private System.Windows.Forms.TextBox TagTextBox;
        private System.Windows.Forms.FlowLayoutPanel TagsList;
        private AutocompleteMenuNS.AutocompleteMenu AutoCompleteItems;
        private System.Windows.Forms.ToolStripButton DeleteAllTagsToolButton;
    }
}