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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TagEditorForm));
            this.ItemEditorTableLayout = new System.Windows.Forms.TableLayoutPanel();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
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
            this.TagIcons = new System.Windows.Forms.ImageList(this.components);
            this.TagTextBox = new System.Windows.Forms.TextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.TagsList = new System.Windows.Forms.FlowLayoutPanel();
            this.AutoCompleteItems = new AutocompleteMenuNS.AutocompleteMenu();
            this.TagEditTooltip = new System.Windows.Forms.ToolTip(this.components);
            this.TagListView = new System.Windows.Forms.ListView();
            this.ToolStripButtonRemoveSelectedTags = new System.Windows.Forms.ToolStripButton();
            this.ItemEditorTableLayout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.HeaderPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.HeaderPicture)).BeginInit();
            this.ItemEditorToolBar.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // ItemEditorTableLayout
            // 
            this.ItemEditorTableLayout.ColumnCount = 2;
            this.ItemEditorTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ItemEditorTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 123F));
            this.ItemEditorTableLayout.Controls.Add(this.pictureBox2, 0, 3);
            this.ItemEditorTableLayout.Controls.Add(this.HeaderPanel, 0, 0);
            this.ItemEditorTableLayout.Controls.Add(this.ItemEditorToolBar, 1, 1);
            this.ItemEditorTableLayout.Controls.Add(this.flowLayoutPanel1, 0, 1);
            this.ItemEditorTableLayout.Controls.Add(this.OkButton, 0, 5);
            this.ItemEditorTableLayout.Controls.Add(this.CancelBtn, 1, 5);
            this.ItemEditorTableLayout.Controls.Add(this.TagListView, 1, 4);
            this.ItemEditorTableLayout.Controls.Add(this.TagsList, 0, 2);
            this.ItemEditorTableLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ItemEditorTableLayout.Location = new System.Drawing.Point(0, 0);
            this.ItemEditorTableLayout.Margin = new System.Windows.Forms.Padding(4);
            this.ItemEditorTableLayout.Name = "ItemEditorTableLayout";
            this.ItemEditorTableLayout.RowCount = 6;
            this.ItemEditorTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.ItemEditorTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 43F));
            this.ItemEditorTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ItemEditorTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 37F));
            this.ItemEditorTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.ItemEditorTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 47F));
            this.ItemEditorTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.ItemEditorTableLayout.Size = new System.Drawing.Size(497, 369);
            this.ItemEditorTableLayout.TabIndex = 1;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::Win_CBZ.Properties.Resources.information;
            this.pictureBox2.InitialImage = global::Win_CBZ.Properties.Resources.information;
            this.pictureBox2.Location = new System.Drawing.Point(6, 255);
            this.pictureBox2.Margin = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(36, 36);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox2.TabIndex = 31;
            this.pictureBox2.TabStop = false;
            this.TagEditTooltip.SetToolTip(this.pictureBox2, "Autocomplete Editor. Start typing and accept an item by pressing enter.");
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
            this.OkButton.Location = new System.Drawing.Point(270, 326);
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
            this.CancelBtn.Location = new System.Drawing.Point(394, 326);
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
            this.ToolStripButtonRemoveSelectedTags,
            this.DeleteAllTagsToolButton,
            this.ToolButtonSortAscending});
            this.ItemEditorToolBar.Location = new System.Drawing.Point(399, 74);
            this.ItemEditorToolBar.Margin = new System.Windows.Forms.Padding(8, 0, 8, 2);
            this.ItemEditorToolBar.Name = "ItemEditorToolBar";
            this.ItemEditorToolBar.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.ItemEditorToolBar.Size = new System.Drawing.Size(90, 27);
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
            this.DeleteAllTagsToolButton.Text = "Clear all Tags";
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
            this.flowLayoutPanel1.Controls.Add(this.pictureBox1);
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
            this.ButtonAddTag.ImageIndex = 1;
            this.ButtonAddTag.ImageList = this.TagIcons;
            this.ButtonAddTag.Location = new System.Drawing.Point(331, 3);
            this.ButtonAddTag.Name = "ButtonAddTag";
            this.ButtonAddTag.Padding = new System.Windows.Forms.Padding(4);
            this.ButtonAddTag.Size = new System.Drawing.Size(34, 23);
            this.ButtonAddTag.TabIndex = 0;
            this.ButtonAddTag.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.ButtonAddTag.UseVisualStyleBackColor = true;
            this.ButtonAddTag.Click += new System.EventHandler(this.ButtonAddTag_Click);
            // 
            // TagIcons
            // 
            this.TagIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("TagIcons.ImageStream")));
            this.TagIcons.TransparentColor = System.Drawing.Color.Transparent;
            this.TagIcons.Images.SetKeyName(0, "tag");
            this.TagIcons.Images.SetKeyName(1, "plus");
            this.TagIcons.Images.SetKeyName(2, "delete");
            // 
            // TagTextBox
            // 
            this.AutoCompleteItems.SetAutocompleteMenu(this.TagTextBox, this.AutoCompleteItems);
            this.TagTextBox.Location = new System.Drawing.Point(65, 3);
            this.TagTextBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.TagTextBox.Name = "TagTextBox";
            this.TagTextBox.Size = new System.Drawing.Size(260, 22);
            this.TagTextBox.TabIndex = 1;
            this.TagTextBox.TextChanged += new System.EventHandler(this.TagTextBox_TextChanged);
            this.TagTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TagTextBox_KeyDown);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Win_CBZ.Properties.Resources.tag;
            this.pictureBox1.Location = new System.Drawing.Point(32, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(27, 23);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
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
            this.TagsList.Size = new System.Drawing.Size(481, 146);
            this.TagsList.TabIndex = 9;
            this.TagsList.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TagsList_MouseDown);
            this.TagsList.MouseUp += new System.Windows.Forms.MouseEventHandler(this.TagsList_MouseUp);
            // 
            // AutoCompleteItems
            // 
            this.AutoCompleteItems.Colors = ((AutocompleteMenuNS.Colors)(resources.GetObject("AutoCompleteItems.Colors")));
            this.AutoCompleteItems.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AutoCompleteItems.ImageList = this.TagIcons;
            this.AutoCompleteItems.Items = new string[0];
            this.AutoCompleteItems.LeftPadding = 17;
            this.AutoCompleteItems.MinFragmentLength = 1;
            this.AutoCompleteItems.SearchPattern = "[\\w+( +\\w+)*$]";
            this.AutoCompleteItems.TargetControlWrapper = null;
            // 
            // TagEditTooltip
            // 
            this.TagEditTooltip.IsBalloon = true;
            this.TagEditTooltip.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.TagEditTooltip.ToolTipTitle = "Win_CBZ";
            // 
            // TagListView
            // 
            this.TagListView.HideSelection = false;
            this.TagListView.Location = new System.Drawing.Point(377, 295);
            this.TagListView.Name = "TagListView";
            this.TagListView.OwnerDraw = true;
            this.TagListView.Size = new System.Drawing.Size(117, 24);
            this.TagListView.SmallImageList = this.TagIcons;
            this.TagListView.TabIndex = 32;
            this.TagListView.UseCompatibleStateImageBehavior = false;
            this.TagListView.View = System.Windows.Forms.View.SmallIcon;
            this.TagListView.Visible = false;
            this.TagListView.DrawItem += new System.Windows.Forms.DrawListViewItemEventHandler(this.TagListView_DrawItem);
            this.TagListView.SelectedIndexChanged += new System.EventHandler(this.TagListView_SelectedIndexChanged);
            // 
            // ToolStripButtonRemoveSelectedTags
            // 
            this.ToolStripButtonRemoveSelectedTags.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ToolStripButtonRemoveSelectedTags.Enabled = false;
            this.ToolStripButtonRemoveSelectedTags.Image = global::Win_CBZ.Properties.Resources.delete;
            this.ToolStripButtonRemoveSelectedTags.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ToolStripButtonRemoveSelectedTags.Name = "ToolStripButtonRemoveSelectedTags";
            this.ToolStripButtonRemoveSelectedTags.Size = new System.Drawing.Size(29, 24);
            this.ToolStripButtonRemoveSelectedTags.Text = "Remove selected";
            this.ToolStripButtonRemoveSelectedTags.Click += new System.EventHandler(this.ToolStripButtonRemoveSelectedTags_Click);
            // 
            // TagEditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(497, 369);
            this.Controls.Add(this.ItemEditorTableLayout);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.KeyPreview = true;
            this.Name = "TagEditorForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Tag Editor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TagEditor_FormClosing);
            this.Shown += new System.EventHandler(this.TagEditorForm_Shown);
            this.ItemEditorTableLayout.ResumeLayout(false);
            this.ItemEditorTableLayout.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.HeaderPanel.ResumeLayout(false);
            this.HeaderPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.HeaderPicture)).EndInit();
            this.ItemEditorToolBar.ResumeLayout(false);
            this.ItemEditorToolBar.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
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
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ToolTip TagEditTooltip;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.ImageList TagIcons;
        private System.Windows.Forms.ListView TagListView;
        private System.Windows.Forms.ToolStripButton ToolStripButtonRemoveSelectedTags;
    }
}