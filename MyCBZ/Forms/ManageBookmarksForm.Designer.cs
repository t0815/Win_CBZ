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
            flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            toolStrip2 = new System.Windows.Forms.ToolStrip();
            toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            TextBoxBookmarkName = new System.Windows.Forms.ToolStripTextBox();
            ToolButtonCreateBookmark = new System.Windows.Forms.ToolStripButton();
            toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            toolStripButton5 = new System.Windows.Forms.ToolStripButton();
            BookmarksTree = new System.Windows.Forms.TreeView();
            PagesList = new System.Windows.Forms.ListView();
            ColName = new System.Windows.Forms.ColumnHeader();
            ColIndex = new System.Windows.Forms.ColumnHeader();
            columnHeader3 = new System.Windows.Forms.ColumnHeader();
            OkButton = new System.Windows.Forms.Button();
            CancelBtn = new System.Windows.Forms.Button();
            pictureBox2 = new System.Windows.Forms.PictureBox();
            panel1 = new System.Windows.Forms.Panel();
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
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)PreviewPictureBox).BeginInit();
            SuspendLayout();
            // 
            // BookmarkEditorTableLayout
            // 
            BookmarkEditorTableLayout.ColumnCount = 5;
            BookmarkEditorTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            BookmarkEditorTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 389F));
            BookmarkEditorTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 260F));
            BookmarkEditorTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 106F));
            BookmarkEditorTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            BookmarkEditorTableLayout.Controls.Add(HeaderPanel, 0, 0);
            BookmarkEditorTableLayout.Controls.Add(flowLayoutPanel1, 1, 2);
            BookmarkEditorTableLayout.Controls.Add(flowLayoutPanel2, 2, 2);
            BookmarkEditorTableLayout.Controls.Add(BookmarksTree, 2, 3);
            BookmarkEditorTableLayout.Controls.Add(PagesList, 1, 3);
            BookmarkEditorTableLayout.Controls.Add(OkButton, 2, 6);
            BookmarkEditorTableLayout.Controls.Add(CancelBtn, 3, 6);
            BookmarkEditorTableLayout.Controls.Add(pictureBox2, 1, 6);
            BookmarkEditorTableLayout.Controls.Add(panel1, 1, 4);
            BookmarkEditorTableLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            BookmarkEditorTableLayout.Location = new System.Drawing.Point(0, 0);
            BookmarkEditorTableLayout.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            BookmarkEditorTableLayout.Name = "BookmarkEditorTableLayout";
            BookmarkEditorTableLayout.RowCount = 7;
            BookmarkEditorTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            BookmarkEditorTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            BookmarkEditorTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            BookmarkEditorTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            BookmarkEditorTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 227F));
            BookmarkEditorTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            BookmarkEditorTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 57F));
            BookmarkEditorTableLayout.Size = new System.Drawing.Size(808, 743);
            BookmarkEditorTableLayout.TabIndex = 2;
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
            HeaderPanel.Size = new System.Drawing.Size(802, 73);
            HeaderPanel.TabIndex = 2;
            // 
            // HeaderLabel
            // 
            HeaderLabel.AutoSize = true;
            HeaderLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            HeaderLabel.Location = new System.Drawing.Point(95, 16);
            HeaderLabel.Name = "HeaderLabel";
            HeaderLabel.Size = new System.Drawing.Size(185, 28);
            HeaderLabel.TabIndex = 1;
            HeaderLabel.Text = "Manage Bookmarks";
            // 
            // HeaderPicture
            // 
            HeaderPicture.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            HeaderPicture.Image = Properties.Resources.book_bookmark_large;
            HeaderPicture.Location = new System.Drawing.Point(24, 0);
            HeaderPicture.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            HeaderPicture.Name = "HeaderPicture";
            HeaderPicture.Size = new System.Drawing.Size(65, 71);
            HeaderPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            HeaderPicture.TabIndex = 0;
            HeaderPicture.TabStop = false;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Controls.Add(toolStrip1);
            flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            flowLayoutPanel1.Location = new System.Drawing.Point(29, 100);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new System.Drawing.Size(383, 34);
            flowLayoutPanel1.TabIndex = 5;
            // 
            // toolStrip1
            // 
            toolStrip1.Dock = System.Windows.Forms.DockStyle.Fill;
            toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { ToolButtonAddPages, toolStripSeparator1, toolStripButton4, toolStripButton1, toolStripSeparator2 });
            toolStrip1.Location = new System.Drawing.Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            toolStrip1.Size = new System.Drawing.Size(112, 27);
            toolStrip1.TabIndex = 0;
            toolStrip1.Text = "toolStrip1";
            // 
            // ToolButtonAddPages
            // 
            ToolButtonAddPages.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            ToolButtonAddPages.Image = Properties.Resources.navigate_plus;
            ToolButtonAddPages.ImageTransparentColor = System.Drawing.Color.Magenta;
            ToolButtonAddPages.Name = "ToolButtonAddPages";
            ToolButtonAddPages.Size = new System.Drawing.Size(29, 24);
            ToolButtonAddPages.Text = "Add selected Page(s) to Bookmark";
            ToolButtonAddPages.Click += ToolButtonAddPages_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
            // 
            // toolStripButton4
            // 
            toolStripButton4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButton4.Enabled = false;
            toolStripButton4.Image = Properties.Resources.delete;
            toolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButton4.Name = "toolStripButton4";
            toolStripButton4.Size = new System.Drawing.Size(29, 24);
            toolStripButton4.Text = "Remove selected Bookmarks";
            toolStripButton4.Click += ToolStripButton4_Click;
            // 
            // toolStripButton1
            // 
            toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButton1.Image = Properties.Resources.garbage;
            toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButton1.Name = "toolStripButton1";
            toolStripButton1.Size = new System.Drawing.Size(29, 24);
            toolStripButton1.Text = "Remove all Bookmarks";
            toolStripButton1.Click += ToolStripButton1_Click;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new System.Drawing.Size(6, 27);
            // 
            // flowLayoutPanel2
            // 
            BookmarkEditorTableLayout.SetColumnSpan(flowLayoutPanel2, 2);
            flowLayoutPanel2.Controls.Add(toolStrip2);
            flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            flowLayoutPanel2.Location = new System.Drawing.Point(418, 100);
            flowLayoutPanel2.Name = "flowLayoutPanel2";
            flowLayoutPanel2.Size = new System.Drawing.Size(360, 34);
            flowLayoutPanel2.TabIndex = 6;
            // 
            // toolStrip2
            // 
            toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
            toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripLabel1, TextBoxBookmarkName, ToolButtonCreateBookmark, toolStripButton2, toolStripSeparator3, toolStripButton5 });
            toolStrip2.Location = new System.Drawing.Point(0, 0);
            toolStrip2.Name = "toolStrip2";
            toolStrip2.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            toolStrip2.Size = new System.Drawing.Size(297, 27);
            toolStrip2.TabIndex = 0;
            toolStrip2.Text = "toolStrip2";
            // 
            // toolStripLabel1
            // 
            toolStripLabel1.Name = "toolStripLabel1";
            toolStripLabel1.Size = new System.Drawing.Size(49, 24);
            toolStripLabel1.Text = "Name";
            // 
            // TextBoxBookmarkName
            // 
            TextBoxBookmarkName.Name = "TextBoxBookmarkName";
            TextBoxBookmarkName.Size = new System.Drawing.Size(140, 27);
            // 
            // ToolButtonCreateBookmark
            // 
            ToolButtonCreateBookmark.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            ToolButtonCreateBookmark.Image = Properties.Resources.book_bookmark;
            ToolButtonCreateBookmark.ImageTransparentColor = System.Drawing.Color.Magenta;
            ToolButtonCreateBookmark.Name = "ToolButtonCreateBookmark";
            ToolButtonCreateBookmark.Size = new System.Drawing.Size(29, 24);
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
            toolStripButton2.Size = new System.Drawing.Size(29, 24);
            toolStripButton2.Text = "Rename Bookmark";
            toolStripButton2.Click += ToolStripButton2_Click;
            // 
            // toolStripSeparator3
            // 
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new System.Drawing.Size(6, 27);
            // 
            // toolStripButton5
            // 
            toolStripButton5.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButton5.Enabled = false;
            toolStripButton5.Image = Properties.Resources.delete;
            toolStripButton5.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButton5.Name = "toolStripButton5";
            toolStripButton5.Size = new System.Drawing.Size(29, 24);
            toolStripButton5.Text = "Remove Bookmark";
            toolStripButton5.Click += ToolStripButton5_Click;
            // 
            // BookmarksTree
            // 
            BookmarkEditorTableLayout.SetColumnSpan(BookmarksTree, 2);
            BookmarksTree.HideSelection = false;
            BookmarksTree.Location = new System.Drawing.Point(418, 140);
            BookmarksTree.Name = "BookmarksTree";
            BookmarkEditorTableLayout.SetRowSpan(BookmarksTree, 2);
            BookmarksTree.Size = new System.Drawing.Size(360, 509);
            BookmarksTree.TabIndex = 7;
            BookmarksTree.AfterSelect += BookmarksTree_AfterSelect;
            // 
            // PagesList
            // 
            PagesList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { ColName, ColIndex, columnHeader3 });
            PagesList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            PagesList.Location = new System.Drawing.Point(29, 140);
            PagesList.Name = "PagesList";
            PagesList.Size = new System.Drawing.Size(383, 290);
            PagesList.TabIndex = 8;
            PagesList.UseCompatibleStateImageBehavior = false;
            PagesList.View = System.Windows.Forms.View.Details;
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
            // OkButton
            // 
            OkButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            OkButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            OkButton.Location = new System.Drawing.Point(571, 691);
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
            CancelBtn.Location = new System.Drawing.Point(684, 691);
            CancelBtn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            CancelBtn.Name = "CancelBtn";
            CancelBtn.Size = new System.Drawing.Size(93, 35);
            CancelBtn.TabIndex = 4;
            CancelBtn.Text = "Cancel";
            CancelBtn.UseVisualStyleBackColor = true;
            // 
            // pictureBox2
            // 
            pictureBox2.Image = Properties.Resources.information;
            pictureBox2.InitialImage = Properties.Resources.information;
            pictureBox2.Location = new System.Drawing.Point(32, 686);
            pictureBox2.Margin = new System.Windows.Forms.Padding(6, 0, 0, 0);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new System.Drawing.Size(36, 43);
            pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            pictureBox2.TabIndex = 32;
            pictureBox2.TabStop = false;
            BookmarkEditorTooltip.SetToolTip(pictureBox2, "Use the right Panel, to create a new Bookmark.\r\nOn the left side, assign selected pages to a selected Bookmark.\r\n\r\nDouble-click on a Page to view it in preview-window.");
            // 
            // panel1
            // 
            panel1.AutoScroll = true;
            panel1.Controls.Add(PreviewPictureBox);
            panel1.Location = new System.Drawing.Point(29, 436);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(383, 213);
            panel1.TabIndex = 34;
            // 
            // PreviewPictureBox
            // 
            PreviewPictureBox.Location = new System.Drawing.Point(12, 14);
            PreviewPictureBox.Name = "PreviewPictureBox";
            PreviewPictureBox.Size = new System.Drawing.Size(200, 221);
            PreviewPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            PreviewPictureBox.TabIndex = 33;
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
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(808, 743);
            Controls.Add(BookmarkEditorTableLayout);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            KeyPreview = true;
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
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
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
        private System.Windows.Forms.ListView PagesList;
        private System.Windows.Forms.ColumnHeader ColName;
        private System.Windows.Forms.ColumnHeader ColIndex;
        private System.Windows.Forms.ColumnHeader columnHeader3;
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
        private System.Windows.Forms.PictureBox PreviewPictureBox;
        private System.Windows.Forms.Panel panel1;
    }
}