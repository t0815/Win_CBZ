namespace Win_CBZ.Forms
{
    partial class ManageBookmarksForm
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
            BookmarkEditorTableLayout = new System.Windows.Forms.TableLayoutPanel();
            HeaderPanel = new System.Windows.Forms.Panel();
            HeaderLabel = new System.Windows.Forms.Label();
            HeaderPicture = new System.Windows.Forms.PictureBox();
            flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            toolStrip1 = new System.Windows.Forms.ToolStrip();
            ToolButtonAddPages = new System.Windows.Forms.ToolStripButton();
            toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            toolStripButton4 = new System.Windows.Forms.ToolStripButton();
            toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            ToolButtonSelectRange = new System.Windows.Forms.ToolStripButton();
            flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            toolStrip2 = new System.Windows.Forms.ToolStrip();
            toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            TextBoxBookmarkName = new System.Windows.Forms.ToolStripTextBox();
            ToolButtonCreateBookmark = new System.Windows.Forms.ToolStripButton();
            toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            toolStripButton6 = new System.Windows.Forms.ToolStripButton();
            toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            toolStripButton5 = new System.Windows.Forms.ToolStripButton();
            BookmarksTree = new System.Windows.Forms.TreeView();
            OkButton = new System.Windows.Forms.Button();
            CancelBtn = new System.Windows.Forms.Button();
            pictureBox2 = new System.Windows.Forms.PictureBox();
            splitContainer1 = new System.Windows.Forms.SplitContainer();
            PagesList = new System.Windows.Forms.ListView();
            ColName = new System.Windows.Forms.ColumnHeader();
            ColIndex = new System.Windows.Forms.ColumnHeader();
            columnHeader3 = new System.Windows.Forms.ColumnHeader();
            PreviewPictureBox = new System.Windows.Forms.PictureBox();
            BookmarkEditorTooltip = new System.Windows.Forms.ToolTip(components);
            BookmarkEditorTableLayout.SuspendLayout();
            HeaderPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)HeaderPicture).BeginInit();
            flowLayoutPanel1.SuspendLayout();
            toolStrip1.SuspendLayout();
            flowLayoutPanel2.SuspendLayout();
            toolStrip2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)PreviewPictureBox).BeginInit();
            SuspendLayout();
            // 
            // BookmarkEditorTableLayout
            // 
            BookmarkEditorTableLayout.ColumnCount = 5;
            BookmarkEditorTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            BookmarkEditorTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 486F));
            BookmarkEditorTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 325F));
            BookmarkEditorTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 142F));
            BookmarkEditorTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 99F));
            BookmarkEditorTableLayout.Controls.Add(HeaderPanel, 0, 0);
            BookmarkEditorTableLayout.Controls.Add(flowLayoutPanel1, 1, 2);
            BookmarkEditorTableLayout.Controls.Add(flowLayoutPanel2, 2, 2);
            BookmarkEditorTableLayout.Controls.Add(BookmarksTree, 2, 3);
            BookmarkEditorTableLayout.Controls.Add(OkButton, 2, 6);
            BookmarkEditorTableLayout.Controls.Add(CancelBtn, 3, 6);
            BookmarkEditorTableLayout.Controls.Add(pictureBox2, 1, 6);
            BookmarkEditorTableLayout.Controls.Add(splitContainer1, 1, 3);
            BookmarkEditorTableLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            BookmarkEditorTableLayout.Location = new System.Drawing.Point(0, 0);
            BookmarkEditorTableLayout.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            BookmarkEditorTableLayout.Name = "BookmarkEditorTableLayout";
            BookmarkEditorTableLayout.RowCount = 7;
            BookmarkEditorTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 94F));
            BookmarkEditorTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            BookmarkEditorTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            BookmarkEditorTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            BookmarkEditorTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 284F));
            BookmarkEditorTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            BookmarkEditorTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 71F));
            BookmarkEditorTableLayout.Size = new System.Drawing.Size(1002, 929);
            BookmarkEditorTableLayout.TabIndex = 2;
            // 
            // HeaderPanel
            // 
            HeaderPanel.BackColor = System.Drawing.Color.White;
            BookmarkEditorTableLayout.SetColumnSpan(HeaderPanel, 5);
            HeaderPanel.Controls.Add(HeaderLabel);
            HeaderPanel.Controls.Add(HeaderPicture);
            HeaderPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            HeaderPanel.Location = new System.Drawing.Point(4, 0);
            HeaderPanel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 2);
            HeaderPanel.Name = "HeaderPanel";
            HeaderPanel.Size = new System.Drawing.Size(1076, 92);
            HeaderPanel.TabIndex = 2;
            // 
            // HeaderLabel
            // 
            HeaderLabel.AutoSize = true;
            HeaderLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            HeaderLabel.Location = new System.Drawing.Point(119, 20);
            HeaderLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            HeaderLabel.Name = "HeaderLabel";
            HeaderLabel.Size = new System.Drawing.Size(225, 32);
            HeaderLabel.TabIndex = 1;
            HeaderLabel.Text = "Manage Bookmarks";
            // 
            // HeaderPicture
            // 
            HeaderPicture.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            HeaderPicture.Image = Properties.Resources.book_bookmark_large;
            HeaderPicture.Location = new System.Drawing.Point(30, 0);
            HeaderPicture.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            HeaderPicture.Name = "HeaderPicture";
            HeaderPicture.Size = new System.Drawing.Size(81, 90);
            HeaderPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            HeaderPicture.TabIndex = 0;
            HeaderPicture.TabStop = false;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Controls.Add(toolStrip1);
            flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            flowLayoutPanel1.Location = new System.Drawing.Point(36, 126);
            flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(4);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new System.Drawing.Size(478, 42);
            flowLayoutPanel1.TabIndex = 5;
            // 
            // toolStrip1
            // 
            toolStrip1.Dock = System.Windows.Forms.DockStyle.Fill;
            toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { ToolButtonAddPages, toolStripSeparator1, toolStripButton4, toolStripButton1, toolStripSeparator2, ToolButtonSelectRange });
            toolStrip1.Location = new System.Drawing.Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            toolStrip1.Size = new System.Drawing.Size(166, 29);
            toolStrip1.TabIndex = 0;
            toolStrip1.Text = "toolStrip1";
            // 
            // ToolButtonAddPages
            // 
            ToolButtonAddPages.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            ToolButtonAddPages.Image = Properties.Resources.navigate_plus;
            ToolButtonAddPages.ImageTransparentColor = System.Drawing.Color.Magenta;
            ToolButtonAddPages.Name = "ToolButtonAddPages";
            ToolButtonAddPages.Size = new System.Drawing.Size(34, 24);
            ToolButtonAddPages.Text = "Add selected Page(s) to Bookmark";
            ToolButtonAddPages.Click += ToolButtonAddPages_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new System.Drawing.Size(6, 29);
            // 
            // toolStripButton4
            // 
            toolStripButton4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButton4.Enabled = false;
            toolStripButton4.Image = Properties.Resources.delete;
            toolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButton4.Name = "toolStripButton4";
            toolStripButton4.Size = new System.Drawing.Size(34, 24);
            toolStripButton4.Text = "Remove selected Bookmarks";
            toolStripButton4.Click += ToolStripButton4_Click;
            // 
            // toolStripButton1
            // 
            toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButton1.Image = Properties.Resources.garbage;
            toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButton1.Name = "toolStripButton1";
            toolStripButton1.Size = new System.Drawing.Size(34, 24);
            toolStripButton1.Text = "Remove all Bookmarks";
            toolStripButton1.Click += ToolStripButton1_Click;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new System.Drawing.Size(6, 29);
            // 
            // ToolButtonSelectRange
            // 
            ToolButtonSelectRange.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            ToolButtonSelectRange.Image = Properties.Resources.selection;
            ToolButtonSelectRange.ImageTransparentColor = System.Drawing.Color.Magenta;
            ToolButtonSelectRange.Name = "ToolButtonSelectRange";
            ToolButtonSelectRange.Size = new System.Drawing.Size(34, 24);
            ToolButtonSelectRange.Text = "Select range...";
            ToolButtonSelectRange.Click += ToolButtonSelectRange_Click;
            // 
            // flowLayoutPanel2
            // 
            BookmarkEditorTableLayout.SetColumnSpan(flowLayoutPanel2, 2);
            flowLayoutPanel2.Controls.Add(toolStrip2);
            flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            flowLayoutPanel2.Location = new System.Drawing.Point(522, 126);
            flowLayoutPanel2.Margin = new System.Windows.Forms.Padding(4);
            flowLayoutPanel2.Name = "flowLayoutPanel2";
            flowLayoutPanel2.Size = new System.Drawing.Size(459, 42);
            flowLayoutPanel2.TabIndex = 6;
            // 
            // toolStrip2
            // 
            toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
            toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripLabel1, TextBoxBookmarkName, ToolButtonCreateBookmark, toolStripButton2, toolStripSeparator3, toolStripButton6, toolStripButton3, toolStripSeparator4, toolStripButton5 });
            toolStrip2.Location = new System.Drawing.Point(0, 0);
            toolStrip2.Name = "toolStrip2";
            toolStrip2.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            toolStrip2.Size = new System.Drawing.Size(459, 31);
            toolStrip2.TabIndex = 0;
            toolStrip2.Text = "toolStrip2";
            // 
            // toolStripLabel1
            // 
            toolStripLabel1.Name = "toolStripLabel1";
            toolStripLabel1.Size = new System.Drawing.Size(59, 26);
            toolStripLabel1.Text = "Name";
            // 
            // TextBoxBookmarkName
            // 
            TextBoxBookmarkName.Name = "TextBoxBookmarkName";
            TextBoxBookmarkName.Size = new System.Drawing.Size(174, 31);
            // 
            // ToolButtonCreateBookmark
            // 
            ToolButtonCreateBookmark.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            ToolButtonCreateBookmark.Image = Properties.Resources.book_bookmark;
            ToolButtonCreateBookmark.ImageTransparentColor = System.Drawing.Color.Magenta;
            ToolButtonCreateBookmark.Name = "ToolButtonCreateBookmark";
            ToolButtonCreateBookmark.Size = new System.Drawing.Size(34, 26);
            ToolButtonCreateBookmark.Text = "Create new Bookmark";
            ToolButtonCreateBookmark.Click += ToolButtonCreateBookmark_Click;
            // 
            // toolStripButton2
            // 
            toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButton2.Enabled = false;
            toolStripButton2.Image = Properties.Resources.pencil;
            toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButton2.Name = "toolStripButton2";
            toolStripButton2.Size = new System.Drawing.Size(34, 26);
            toolStripButton2.Text = "Rename Bookmark";
            toolStripButton2.Click += ToolStripButton2_Click;
            // 
            // toolStripSeparator3
            // 
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new System.Drawing.Size(6, 31);
            // 
            // toolStripButton6
            // 
            toolStripButton6.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButton6.Image = Properties.Resources.copy;
            toolStripButton6.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButton6.Name = "toolStripButton6";
            toolStripButton6.Size = new System.Drawing.Size(34, 26);
            toolStripButton6.Text = "Copy Bookmarks to clipboard";
            toolStripButton6.Click += toolStripButton6_Click;
            // 
            // toolStripButton3
            // 
            toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButton3.Image = Properties.Resources.clipboard_paste;
            toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButton3.Name = "toolStripButton3";
            toolStripButton3.Size = new System.Drawing.Size(34, 26);
            toolStripButton3.Text = "Paste bookmarks from clipboard";
            toolStripButton3.Click += toolStripButton3_Click;
            // 
            // toolStripSeparator4
            // 
            toolStripSeparator4.Name = "toolStripSeparator4";
            toolStripSeparator4.Size = new System.Drawing.Size(6, 31);
            // 
            // toolStripButton5
            // 
            toolStripButton5.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButton5.Enabled = false;
            toolStripButton5.Image = Properties.Resources.delete;
            toolStripButton5.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButton5.Name = "toolStripButton5";
            toolStripButton5.Size = new System.Drawing.Size(34, 24);
            toolStripButton5.Text = "Remove Bookmark";
            toolStripButton5.Click += ToolStripButton5_Click;
            // 
            // BookmarksTree
            // 
            BookmarkEditorTableLayout.SetColumnSpan(BookmarksTree, 2);
            BookmarksTree.HideSelection = false;
            BookmarksTree.Location = new System.Drawing.Point(522, 176);
            BookmarksTree.Margin = new System.Windows.Forms.Padding(4);
            BookmarksTree.Name = "BookmarksTree";
            BookmarkEditorTableLayout.SetRowSpan(BookmarksTree, 2);
            BookmarksTree.Size = new System.Drawing.Size(449, 635);
            BookmarksTree.TabIndex = 7;
            BookmarksTree.AfterSelect += BookmarksTree_AfterSelect;
            // 
            // OkButton
            // 
            OkButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            OkButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            OkButton.Location = new System.Drawing.Point(713, 864);
            OkButton.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            OkButton.Name = "OkButton";
            OkButton.Size = new System.Drawing.Size(125, 44);
            OkButton.TabIndex = 3;
            OkButton.Text = "Ok";
            OkButton.UseVisualStyleBackColor = true;
            OkButton.Click += OkButton_Click;
            // 
            // CancelBtn
            // 
            CancelBtn.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            CancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            CancelBtn.Location = new System.Drawing.Point(864, 864);
            CancelBtn.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            CancelBtn.Name = "CancelBtn";
            CancelBtn.Size = new System.Drawing.Size(116, 44);
            CancelBtn.TabIndex = 4;
            CancelBtn.Text = "Cancel";
            CancelBtn.UseVisualStyleBackColor = true;
            // 
            // pictureBox2
            // 
            pictureBox2.Image = Properties.Resources.information;
            pictureBox2.InitialImage = Properties.Resources.information;
            pictureBox2.Location = new System.Drawing.Point(40, 858);
            pictureBox2.Margin = new System.Windows.Forms.Padding(8, 0, 0, 0);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new System.Drawing.Size(45, 54);
            pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            pictureBox2.TabIndex = 32;
            pictureBox2.TabStop = false;
            BookmarkEditorTooltip.SetToolTip(pictureBox2, "Use the right Panel, to create a new Bookmark.\r\nOn the left side, assign selected pages to a selected Bookmark.\r\n\r\nDouble-click on a Page to view it in preview-window.");
            // 
            // splitContainer1
            // 
            splitContainer1.Location = new System.Drawing.Point(36, 176);
            splitContainer1.Margin = new System.Windows.Forms.Padding(4);
            splitContainer1.Name = "splitContainer1";
            splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(PagesList);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.AutoScroll = true;
            splitContainer1.Panel2.Controls.Add(PreviewPictureBox);
            BookmarkEditorTableLayout.SetRowSpan(splitContainer1, 2);
            splitContainer1.Size = new System.Drawing.Size(466, 636);
            splitContainer1.SplitterDistance = 366;
            splitContainer1.SplitterWidth = 5;
            splitContainer1.TabIndex = 35;
            // 
            // PagesList
            // 
            PagesList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { ColName, ColIndex, columnHeader3 });
            PagesList.Dock = System.Windows.Forms.DockStyle.Fill;
            PagesList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            PagesList.Location = new System.Drawing.Point(0, 0);
            PagesList.Margin = new System.Windows.Forms.Padding(4);
            PagesList.Name = "PagesList";
            PagesList.Size = new System.Drawing.Size(466, 366);
            PagesList.TabIndex = 9;
            PagesList.UseCompatibleStateImageBehavior = false;
            PagesList.View = System.Windows.Forms.View.Details;
            PagesList.ItemSelectionChanged += PagesList_ItemSelectionChanged;
            PagesList.SelectedIndexChanged += PagesList_SelectedIndexChanged;
            PagesList.MouseDoubleClick += PagesList_MouseDoubleClick;
            // 
            // ColName
            // 
            ColName.Text = "Name";
            ColName.Width = 150;
            // 
            // ColIndex
            // 
            ColIndex.Text = "Index";
            // 
            // columnHeader3
            // 
            columnHeader3.Text = "Bookmark";
            columnHeader3.Width = 80;
            // 
            // PreviewPictureBox
            // 
            PreviewPictureBox.Location = new System.Drawing.Point(4, 2);
            PreviewPictureBox.Margin = new System.Windows.Forms.Padding(4);
            PreviewPictureBox.Name = "PreviewPictureBox";
            PreviewPictureBox.Size = new System.Drawing.Size(200, 221);
            PreviewPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            PreviewPictureBox.TabIndex = 34;
            PreviewPictureBox.TabStop = false;
            // 
            // BookmarkEditorTooltip
            // 
            BookmarkEditorTooltip.IsBalloon = true;
            BookmarkEditorTooltip.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            BookmarkEditorTooltip.ToolTipTitle = "Win_CBZ";
            // 
            // ManageBookmarksForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1002, 929);
            Controls.Add(BookmarkEditorTableLayout);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            KeyPreview = true;
            Margin = new System.Windows.Forms.Padding(4);
            Name = "ManageBookmarksForm";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Bookmarks";
            KeyUp += ManageBookmarksForm_KeyUp;
            BookmarkEditorTableLayout.ResumeLayout(false);
            HeaderPanel.ResumeLayout(false);
            HeaderPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)HeaderPicture).EndInit();
            flowLayoutPanel1.ResumeLayout(false);
            flowLayoutPanel1.PerformLayout();
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            flowLayoutPanel2.ResumeLayout(false);
            flowLayoutPanel2.PerformLayout();
            toolStrip2.ResumeLayout(false);
            toolStrip2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)PreviewPictureBox).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel BookmarkEditorTableLayout;
        private System.Windows.Forms.Panel HeaderPanel;
        private System.Windows.Forms.Label HeaderLabel;
        private System.Windows.Forms.PictureBox HeaderPicture;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button OkButton;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.TreeView BookmarksTree;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton ToolButtonAddPages;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripButton ToolButtonCreateBookmark;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripButton4;
        private System.Windows.Forms.ToolStripButton toolStripButton5;
        private System.Windows.Forms.ToolStripTextBox TextBoxBookmarkName;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.ToolTip BookmarkEditorTooltip;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ListView PagesList;
        private System.Windows.Forms.ColumnHeader ColName;
        private System.Windows.Forms.ColumnHeader ColIndex;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.PictureBox PreviewPictureBox;
        private System.Windows.Forms.ToolStripButton ToolButtonSelectRange;
        private System.Windows.Forms.ToolStripButton toolStripButton6;
        private System.Windows.Forms.ToolStripButton toolStripButton3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
    }
}