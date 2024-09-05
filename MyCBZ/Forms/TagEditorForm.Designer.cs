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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TagEditorForm));
            ItemEditorTableLayout = new System.Windows.Forms.TableLayoutPanel();
            pictureBox2 = new System.Windows.Forms.PictureBox();
            HeaderPanel = new System.Windows.Forms.Panel();
            HeaderLabel = new System.Windows.Forms.Label();
            HeaderPicture = new System.Windows.Forms.PictureBox();
            ItemEditorToolBar = new System.Windows.Forms.ToolStrip();
            ToolStripButtonRemoveSelectedTags = new System.Windows.Forms.ToolStripButton();
            DeleteAllTagsToolButton = new System.Windows.Forms.ToolStripButton();
            ToolButtonSortAscending = new System.Windows.Forms.ToolStripButton();
            flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            ButtonAddTag = new System.Windows.Forms.Button();
            TagIcons = new System.Windows.Forms.ImageList(components);
            TagTextBox = new System.Windows.Forms.TextBox();
            pictureBox1 = new System.Windows.Forms.PictureBox();
            OkButton = new System.Windows.Forms.Button();
            CancelBtn = new System.Windows.Forms.Button();
            TagListView = new System.Windows.Forms.ListView();
            TagsList = new System.Windows.Forms.FlowLayoutPanel();
            TagEditTooltip = new System.Windows.Forms.ToolTip(components);
            Autocomplete = new AutocompleteMenuNS.AutocompleteMenu();
            ItemEditorTableLayout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            HeaderPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)HeaderPicture).BeginInit();
            ItemEditorToolBar.SuspendLayout();
            flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // ItemEditorTableLayout
            // 
            ItemEditorTableLayout.ColumnCount = 2;
            ItemEditorTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            ItemEditorTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 123F));
            ItemEditorTableLayout.Controls.Add(pictureBox2, 0, 3);
            ItemEditorTableLayout.Controls.Add(HeaderPanel, 0, 0);
            ItemEditorTableLayout.Controls.Add(ItemEditorToolBar, 1, 1);
            ItemEditorTableLayout.Controls.Add(flowLayoutPanel1, 0, 1);
            ItemEditorTableLayout.Controls.Add(OkButton, 0, 5);
            ItemEditorTableLayout.Controls.Add(CancelBtn, 1, 5);
            ItemEditorTableLayout.Controls.Add(TagListView, 1, 4);
            ItemEditorTableLayout.Controls.Add(TagsList, 0, 2);
            ItemEditorTableLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            ItemEditorTableLayout.Location = new System.Drawing.Point(0, 0);
            ItemEditorTableLayout.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            ItemEditorTableLayout.Name = "ItemEditorTableLayout";
            ItemEditorTableLayout.RowCount = 6;
            ItemEditorTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            ItemEditorTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 54F));
            ItemEditorTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            ItemEditorTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 62F));
            ItemEditorTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 21F));
            ItemEditorTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 59F));
            ItemEditorTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            ItemEditorTableLayout.Size = new System.Drawing.Size(497, 461);
            ItemEditorTableLayout.TabIndex = 1;
            // 
            // pictureBox2
            // 
            pictureBox2.Image = Properties.Resources.information;
            pictureBox2.InitialImage = Properties.Resources.information;
            pictureBox2.Location = new System.Drawing.Point(6, 319);
            pictureBox2.Margin = new System.Windows.Forms.Padding(6, 0, 0, 0);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new System.Drawing.Size(36, 45);
            pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            pictureBox2.TabIndex = 31;
            pictureBox2.TabStop = false;
            TagEditTooltip.SetToolTip(pictureBox2, "Autocomplete Editor. Start typing and accept an item by pressing enter.");
            // 
            // HeaderPanel
            // 
            HeaderPanel.BackColor = System.Drawing.Color.White;
            ItemEditorTableLayout.SetColumnSpan(HeaderPanel, 2);
            HeaderPanel.Controls.Add(HeaderLabel);
            HeaderPanel.Controls.Add(HeaderPicture);
            HeaderPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            HeaderPanel.Location = new System.Drawing.Point(3, 2);
            HeaderPanel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            HeaderPanel.Name = "HeaderPanel";
            HeaderPanel.Size = new System.Drawing.Size(491, 71);
            HeaderPanel.TabIndex = 2;
            // 
            // HeaderLabel
            // 
            HeaderLabel.AutoSize = true;
            HeaderLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            HeaderLabel.Location = new System.Drawing.Point(95, 9);
            HeaderLabel.Name = "HeaderLabel";
            HeaderLabel.Size = new System.Drawing.Size(50, 28);
            HeaderLabel.TabIndex = 1;
            HeaderLabel.Text = "Tags";
            // 
            // HeaderPicture
            // 
            HeaderPicture.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            HeaderPicture.Image = Properties.Resources.tags;
            HeaderPicture.Location = new System.Drawing.Point(24, 0);
            HeaderPicture.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            HeaderPicture.Name = "HeaderPicture";
            HeaderPicture.Size = new System.Drawing.Size(65, 69);
            HeaderPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            HeaderPicture.TabIndex = 0;
            HeaderPicture.TabStop = false;
            // 
            // ItemEditorToolBar
            // 
            ItemEditorToolBar.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            ItemEditorToolBar.Dock = System.Windows.Forms.DockStyle.None;
            ItemEditorToolBar.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            ItemEditorToolBar.ImageScalingSize = new System.Drawing.Size(20, 20);
            ItemEditorToolBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { ToolStripButtonRemoveSelectedTags, DeleteAllTagsToolButton, ToolButtonSortAscending });
            ItemEditorToolBar.Location = new System.Drawing.Point(399, 100);
            ItemEditorToolBar.Margin = new System.Windows.Forms.Padding(8, 0, 8, 2);
            ItemEditorToolBar.Name = "ItemEditorToolBar";
            ItemEditorToolBar.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            ItemEditorToolBar.Size = new System.Drawing.Size(90, 27);
            ItemEditorToolBar.Stretch = true;
            ItemEditorToolBar.TabIndex = 7;
            ItemEditorToolBar.Text = "toolStrip1";
            // 
            // ToolStripButtonRemoveSelectedTags
            // 
            ToolStripButtonRemoveSelectedTags.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            ToolStripButtonRemoveSelectedTags.Enabled = false;
            ToolStripButtonRemoveSelectedTags.Image = Properties.Resources.delete;
            ToolStripButtonRemoveSelectedTags.ImageTransparentColor = System.Drawing.Color.Magenta;
            ToolStripButtonRemoveSelectedTags.Name = "ToolStripButtonRemoveSelectedTags";
            ToolStripButtonRemoveSelectedTags.Size = new System.Drawing.Size(29, 24);
            ToolStripButtonRemoveSelectedTags.Text = "Remove selected";
            ToolStripButtonRemoveSelectedTags.Click += ToolStripButtonRemoveSelectedTags_Click;
            // 
            // DeleteAllTagsToolButton
            // 
            DeleteAllTagsToolButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            DeleteAllTagsToolButton.Image = Properties.Resources.garbage;
            DeleteAllTagsToolButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            DeleteAllTagsToolButton.Name = "DeleteAllTagsToolButton";
            DeleteAllTagsToolButton.Size = new System.Drawing.Size(29, 24);
            DeleteAllTagsToolButton.Text = "Clear all Tags";
            DeleteAllTagsToolButton.Click += DeleteAllTagsToolButton_Click;
            // 
            // ToolButtonSortAscending
            // 
            ToolButtonSortAscending.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            ToolButtonSortAscending.Image = Properties.Resources.sort_az_ascending2;
            ToolButtonSortAscending.ImageTransparentColor = System.Drawing.Color.Magenta;
            ToolButtonSortAscending.Name = "ToolButtonSortAscending";
            ToolButtonSortAscending.Size = new System.Drawing.Size(29, 24);
            ToolButtonSortAscending.Text = "toolStripButton1";
            ToolButtonSortAscending.ToolTipText = "Sort items ascending";
            ToolButtonSortAscending.Click += ToolButtonSortAscending_Click;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            flowLayoutPanel1.Controls.Add(ButtonAddTag);
            flowLayoutPanel1.Controls.Add(TagTextBox);
            flowLayoutPanel1.Controls.Add(pictureBox1);
            flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            flowLayoutPanel1.Location = new System.Drawing.Point(3, 91);
            flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 0);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new System.Drawing.Size(368, 38);
            flowLayoutPanel1.TabIndex = 8;
            // 
            // ButtonAddTag
            // 
            ButtonAddTag.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            ButtonAddTag.ImageIndex = 1;
            ButtonAddTag.ImageList = TagIcons;
            ButtonAddTag.Location = new System.Drawing.Point(331, 4);
            ButtonAddTag.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            ButtonAddTag.Name = "ButtonAddTag";
            ButtonAddTag.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            ButtonAddTag.Size = new System.Drawing.Size(34, 29);
            ButtonAddTag.TabIndex = 0;
            ButtonAddTag.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            ButtonAddTag.UseVisualStyleBackColor = true;
            ButtonAddTag.Click += ButtonAddTag_Click;
            // 
            // TagIcons
            // 
            TagIcons.ColorDepth = System.Windows.Forms.ColorDepth.Depth24Bit;
            TagIcons.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("TagIcons.ImageStream");
            TagIcons.TransparentColor = System.Drawing.Color.Transparent;
            TagIcons.Images.SetKeyName(0, "tag");
            TagIcons.Images.SetKeyName(1, "plus");
            // 
            // TagTextBox
            // 
            Autocomplete.SetAutocompleteMenu(TagTextBox, Autocomplete);
            TagTextBox.HideSelection = false;
            TagTextBox.Location = new System.Drawing.Point(65, 4);
            TagTextBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 0);
            TagTextBox.Name = "TagTextBox";
            TagTextBox.Size = new System.Drawing.Size(260, 27);
            TagTextBox.TabIndex = 1;
            TagTextBox.KeyDown += TagTextBox_KeyDown;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = Properties.Resources.tag;
            pictureBox1.Location = new System.Drawing.Point(32, 4);
            pictureBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new System.Drawing.Size(27, 29);
            pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 2;
            pictureBox1.TabStop = false;
            // 
            // OkButton
            // 
            OkButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            OkButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            OkButton.Location = new System.Drawing.Point(270, 407);
            OkButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            OkButton.Name = "OkButton";
            OkButton.Size = new System.Drawing.Size(100, 35);
            OkButton.TabIndex = 3;
            OkButton.Text = "Ok";
            OkButton.UseVisualStyleBackColor = true;
            OkButton.Click += OkButton_Click;
            // 
            // CancelBtn
            // 
            CancelBtn.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            CancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            CancelBtn.Location = new System.Drawing.Point(394, 407);
            CancelBtn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            CancelBtn.Name = "CancelBtn";
            CancelBtn.Size = new System.Drawing.Size(99, 35);
            CancelBtn.TabIndex = 4;
            CancelBtn.Text = "Cancel";
            CancelBtn.UseVisualStyleBackColor = true;
            CancelBtn.Click += CancelBtn_Click;
            // 
            // TagListView
            // 
            TagListView.Location = new System.Drawing.Point(377, 385);
            TagListView.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            TagListView.Name = "TagListView";
            TagListView.OwnerDraw = true;
            TagListView.Size = new System.Drawing.Size(117, 13);
            TagListView.TabIndex = 32;
            TagListView.UseCompatibleStateImageBehavior = false;
            TagListView.View = System.Windows.Forms.View.SmallIcon;
            TagListView.Visible = false;
            TagListView.DrawItem += TagListView_DrawItem;
            TagListView.SelectedIndexChanged += TagListView_SelectedIndexChanged;
            // 
            // TagsList
            // 
            TagsList.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            TagsList.AutoScroll = true;
            TagsList.BackColor = System.Drawing.SystemColors.Window;
            TagsList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            ItemEditorTableLayout.SetColumnSpan(TagsList, 2);
            TagsList.Location = new System.Drawing.Point(8, 133);
            TagsList.Margin = new System.Windows.Forms.Padding(8, 4, 8, 4);
            TagsList.Name = "TagsList";
            TagsList.Size = new System.Drawing.Size(481, 182);
            TagsList.TabIndex = 9;
            TagsList.MouseDown += TagsList_MouseDown;
            TagsList.MouseUp += TagsList_MouseUp;
            // 
            // TagEditTooltip
            // 
            TagEditTooltip.IsBalloon = true;
            TagEditTooltip.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            TagEditTooltip.ToolTipTitle = "Win_CBZ";
            // 
            // Autocomplete
            // 
            Autocomplete.Colors = (AutocompleteMenuNS.Colors)resources.GetObject("Autocomplete.Colors");
            Autocomplete.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            Autocomplete.ImageList = TagIcons;
            Autocomplete.MinFragmentLength = 1;
            Autocomplete.SearchPattern = "[\\w+( +\\w+)*$]";
            Autocomplete.TargetControlWrapper = null;
            // 
            // TagEditorForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(497, 461);
            Controls.Add(ItemEditorTableLayout);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            Name = "TagEditorForm";
            SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Tag Editor";
            FormClosing += TagEditor_FormClosing;
            Shown += TagEditorForm_Shown;
            ItemEditorTableLayout.ResumeLayout(false);
            ItemEditorTableLayout.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            HeaderPanel.ResumeLayout(false);
            HeaderPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)HeaderPicture).EndInit();
            ItemEditorToolBar.ResumeLayout(false);
            ItemEditorToolBar.PerformLayout();
            flowLayoutPanel1.ResumeLayout(false);
            flowLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
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
        private System.Windows.Forms.ToolStripButton DeleteAllTagsToolButton;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ToolTip TagEditTooltip;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.ListView TagListView;
        private System.Windows.Forms.ToolStripButton ToolStripButtonRemoveSelectedTags;
        private System.Windows.Forms.ImageList TagIcons;
        private AutocompleteMenuNS.AutocompleteMenu Autocomplete;
    }
}