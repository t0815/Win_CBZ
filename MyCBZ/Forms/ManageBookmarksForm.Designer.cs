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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ManageBookmarksForm));
            BookmarkEditorTableLayout = new System.Windows.Forms.TableLayoutPanel();
            HeaderPanel = new System.Windows.Forms.Panel();
            HeaderLabel = new System.Windows.Forms.Label();
            HeaderPicture = new System.Windows.Forms.PictureBox();
            CancelBtn = new System.Windows.Forms.Button();
            OkButton = new System.Windows.Forms.Button();
            flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            toolStrip1 = new System.Windows.Forms.ToolStrip();
            toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            toolStrip2 = new System.Windows.Forms.ToolStrip();
            toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            BookmarksTree = new System.Windows.Forms.TreeView();
            PagesList = new System.Windows.Forms.ListView();
            ColName = new System.Windows.Forms.ColumnHeader();
            ColIndex = new System.Windows.Forms.ColumnHeader();
            columnHeader3 = new System.Windows.Forms.ColumnHeader();
            BookmarkEditorTableLayout.SuspendLayout();
            HeaderPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)HeaderPicture).BeginInit();
            flowLayoutPanel1.SuspendLayout();
            toolStrip1.SuspendLayout();
            flowLayoutPanel2.SuspendLayout();
            toolStrip2.SuspendLayout();
            SuspendLayout();
            // 
            // BookmarkEditorTableLayout
            // 
            BookmarkEditorTableLayout.ColumnCount = 4;
            BookmarkEditorTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            BookmarkEditorTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 337F));
            BookmarkEditorTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 330F));
            BookmarkEditorTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 107F));
            BookmarkEditorTableLayout.Controls.Add(HeaderPanel, 0, 0);
            BookmarkEditorTableLayout.Controls.Add(CancelBtn, 3, 5);
            BookmarkEditorTableLayout.Controls.Add(OkButton, 2, 5);
            BookmarkEditorTableLayout.Controls.Add(flowLayoutPanel1, 1, 2);
            BookmarkEditorTableLayout.Controls.Add(flowLayoutPanel2, 2, 2);
            BookmarkEditorTableLayout.Controls.Add(BookmarksTree, 2, 3);
            BookmarkEditorTableLayout.Controls.Add(PagesList, 1, 3);
            BookmarkEditorTableLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            BookmarkEditorTableLayout.Location = new System.Drawing.Point(0, 0);
            BookmarkEditorTableLayout.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            BookmarkEditorTableLayout.Name = "BookmarkEditorTableLayout";
            BookmarkEditorTableLayout.RowCount = 6;
            BookmarkEditorTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            BookmarkEditorTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 46F));
            BookmarkEditorTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38F));
            BookmarkEditorTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            BookmarkEditorTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 43F));
            BookmarkEditorTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 46F));
            BookmarkEditorTableLayout.Size = new System.Drawing.Size(805, 526);
            BookmarkEditorTableLayout.TabIndex = 2;
            // 
            // HeaderPanel
            // 
            HeaderPanel.BackColor = System.Drawing.Color.White;
            BookmarkEditorTableLayout.SetColumnSpan(HeaderPanel, 4);
            HeaderPanel.Controls.Add(HeaderLabel);
            HeaderPanel.Controls.Add(HeaderPicture);
            HeaderPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            HeaderPanel.Location = new System.Drawing.Point(3, 0);
            HeaderPanel.Margin = new System.Windows.Forms.Padding(3, 0, 3, 2);
            HeaderPanel.Name = "HeaderPanel";
            HeaderPanel.Size = new System.Drawing.Size(799, 73);
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
            HeaderPicture.Image = Properties.Resources.edit_large;
            HeaderPicture.Location = new System.Drawing.Point(24, 0);
            HeaderPicture.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            HeaderPicture.Name = "HeaderPicture";
            HeaderPicture.Size = new System.Drawing.Size(65, 71);
            HeaderPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            HeaderPicture.TabIndex = 0;
            HeaderPicture.TabStop = false;
            // 
            // CancelBtn
            // 
            CancelBtn.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            CancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            CancelBtn.Location = new System.Drawing.Point(702, 485);
            CancelBtn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            CancelBtn.Name = "CancelBtn";
            CancelBtn.Size = new System.Drawing.Size(99, 35);
            CancelBtn.TabIndex = 4;
            CancelBtn.Text = "Cancel";
            CancelBtn.UseVisualStyleBackColor = true;
            // 
            // OkButton
            // 
            OkButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            OkButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            OkButton.Location = new System.Drawing.Point(594, 485);
            OkButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            OkButton.Name = "OkButton";
            OkButton.Size = new System.Drawing.Size(100, 35);
            OkButton.TabIndex = 3;
            OkButton.Text = "Ok";
            OkButton.UseVisualStyleBackColor = true;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Controls.Add(toolStrip1);
            flowLayoutPanel1.Location = new System.Drawing.Point(34, 124);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new System.Drawing.Size(331, 32);
            flowLayoutPanel1.TabIndex = 5;
            // 
            // toolStrip1
            // 
            toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripButton1, toolStripButton2 });
            toolStrip1.Location = new System.Drawing.Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new System.Drawing.Size(71, 27);
            toolStrip1.TabIndex = 0;
            toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton1
            // 
            toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButton1.Image = (System.Drawing.Image)resources.GetObject("toolStripButton1.Image");
            toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButton1.Name = "toolStripButton1";
            toolStripButton1.Size = new System.Drawing.Size(29, 24);
            toolStripButton1.Text = "toolStripButton1";
            // 
            // toolStripButton2
            // 
            toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButton2.Image = (System.Drawing.Image)resources.GetObject("toolStripButton2.Image");
            toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButton2.Name = "toolStripButton2";
            toolStripButton2.Size = new System.Drawing.Size(29, 24);
            toolStripButton2.Text = "toolStripButton2";
            // 
            // flowLayoutPanel2
            // 
            flowLayoutPanel2.Controls.Add(toolStrip2);
            flowLayoutPanel2.Location = new System.Drawing.Point(371, 124);
            flowLayoutPanel2.Name = "flowLayoutPanel2";
            flowLayoutPanel2.Size = new System.Drawing.Size(319, 32);
            flowLayoutPanel2.TabIndex = 6;
            // 
            // toolStrip2
            // 
            toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
            toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripButton3 });
            toolStrip2.Location = new System.Drawing.Point(0, 0);
            toolStrip2.Name = "toolStrip2";
            toolStrip2.Size = new System.Drawing.Size(42, 27);
            toolStrip2.TabIndex = 0;
            toolStrip2.Text = "toolStrip2";
            // 
            // toolStripButton3
            // 
            toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButton3.Image = (System.Drawing.Image)resources.GetObject("toolStripButton3.Image");
            toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButton3.Name = "toolStripButton3";
            toolStripButton3.Size = new System.Drawing.Size(29, 24);
            toolStripButton3.Text = "toolStripButton3";
            // 
            // BookmarksTree
            // 
            BookmarksTree.Location = new System.Drawing.Point(371, 162);
            BookmarksTree.Name = "BookmarksTree";
            BookmarksTree.Size = new System.Drawing.Size(319, 272);
            BookmarksTree.TabIndex = 7;
            // 
            // PagesList
            // 
            PagesList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { ColName, ColIndex, columnHeader3 });
            PagesList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            PagesList.Location = new System.Drawing.Point(34, 162);
            PagesList.Name = "PagesList";
            PagesList.Size = new System.Drawing.Size(331, 272);
            PagesList.TabIndex = 8;
            PagesList.UseCompatibleStateImageBehavior = false;
            PagesList.View = System.Windows.Forms.View.Details;
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
            // ManageBookmarksForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(805, 526);
            Controls.Add(BookmarkEditorTableLayout);
            Name = "ManageBookmarksForm";
            Text = "ManageBookmarksForm";
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
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripButton toolStripButton3;
        private System.Windows.Forms.ListView PagesList;
        private System.Windows.Forms.ColumnHeader ColName;
        private System.Windows.Forms.ColumnHeader ColIndex;
        private System.Windows.Forms.ColumnHeader columnHeader3;
    }
}