namespace Win_CBZ
{
    partial class ImagePreviewForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImagePreviewForm));
            PreviewToolStrip = new System.Windows.Forms.ToolStrip();
            toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            toolStripButton4 = new System.Windows.Forms.ToolStripButton();
            toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            TextBoxJumpPage = new System.Windows.Forms.ToolStripTextBox();
            toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            toolStripLabel3 = new System.Windows.Forms.ToolStripLabel();
            ExtractionProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            ExportImageDialog = new System.Windows.Forms.SaveFileDialog();
            SplitBoxPageView = new System.Windows.Forms.SplitContainer();
            PageImagePreview = new System.Windows.Forms.PictureBox();
            ListboxChapters = new System.Windows.Forms.ListBox();
            ChapterImagesList = new System.Windows.Forms.ImageList(components);
            PreviewToolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)SplitBoxPageView).BeginInit();
            SplitBoxPageView.Panel1.SuspendLayout();
            SplitBoxPageView.Panel2.SuspendLayout();
            SplitBoxPageView.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)PageImagePreview).BeginInit();
            SuspendLayout();
            // 
            // PreviewToolStrip
            // 
            PreviewToolStrip.Dock = System.Windows.Forms.DockStyle.None;
            PreviewToolStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            PreviewToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripButton1, toolStripSeparator1, toolStripButton3, toolStripButton4, toolStripSeparator2, toolStripButton2, toolStripLabel2, TextBoxJumpPage, toolStripSeparator3, toolStripLabel1, toolStripSeparator4, toolStripLabel3, ExtractionProgressBar });
            PreviewToolStrip.Location = new System.Drawing.Point(0, 0);
            PreviewToolStrip.Name = "PreviewToolStrip";
            PreviewToolStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            PreviewToolStrip.Size = new System.Drawing.Size(933, 31);
            PreviewToolStrip.TabIndex = 1;
            PreviewToolStrip.Text = "PreviewToolStrip";
            // 
            // toolStripButton1
            // 
            toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButton1.Image = Properties.Resources.save_as;
            toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButton1.Name = "toolStripButton1";
            toolStripButton1.Size = new System.Drawing.Size(29, 28);
            toolStripButton1.Text = "toolStripButton1";
            toolStripButton1.Click += toolStripButton1_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new System.Drawing.Size(6, 31);
            // 
            // toolStripButton3
            // 
            toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButton3.Image = Properties.Resources.nav_left;
            toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButton3.Name = "toolStripButton3";
            toolStripButton3.Size = new System.Drawing.Size(29, 28);
            toolStripButton3.Text = "ToolButtonPreviewNavigateLeft";
            toolStripButton3.Click += toolStripButton3_Click;
            // 
            // toolStripButton4
            // 
            toolStripButton4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButton4.Image = Properties.Resources.nav_right;
            toolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButton4.Name = "toolStripButton4";
            toolStripButton4.Size = new System.Drawing.Size(29, 28);
            toolStripButton4.Text = "ToolButtonPreviewNavigateRight";
            toolStripButton4.Click += toolStripButton4_Click;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new System.Drawing.Size(6, 31);
            // 
            // toolStripButton2
            // 
            toolStripButton2.CheckOnClick = true;
            toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButton2.Image = Properties.Resources.book_bookmark;
            toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButton2.Name = "toolStripButton2";
            toolStripButton2.Size = new System.Drawing.Size(29, 28);
            toolStripButton2.Text = "Show chapters";
            toolStripButton2.Click += ToolStripButton2_Click;
            // 
            // toolStripLabel2
            // 
            toolStripLabel2.Name = "toolStripLabel2";
            toolStripLabel2.Size = new System.Drawing.Size(83, 28);
            toolStripLabel2.Text = "Goto page:";
            // 
            // TextBoxJumpPage
            // 
            TextBoxJumpPage.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            TextBoxJumpPage.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            TextBoxJumpPage.Name = "TextBoxJumpPage";
            TextBoxJumpPage.Size = new System.Drawing.Size(50, 31);
            TextBoxJumpPage.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // toolStripSeparator3
            // 
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new System.Drawing.Size(6, 31);
            // 
            // toolStripLabel1
            // 
            toolStripLabel1.Name = "toolStripLabel1";
            toolStripLabel1.Size = new System.Drawing.Size(319, 28);
            toolStripLabel1.Text = "Preview Mode (Arrow Left/- Right to Navigate)";
            // 
            // toolStripSeparator4
            // 
            toolStripSeparator4.Name = "toolStripSeparator4";
            toolStripSeparator4.Size = new System.Drawing.Size(6, 31);
            // 
            // toolStripLabel3
            // 
            toolStripLabel3.Name = "toolStripLabel3";
            toolStripLabel3.Size = new System.Drawing.Size(185, 28);
            toolStripLabel3.Text = "Extracting Chapter-Images";
            toolStripLabel3.Visible = false;
            // 
            // ExtractionProgressBar
            // 
            ExtractionProgressBar.Name = "ExtractionProgressBar";
            ExtractionProgressBar.Size = new System.Drawing.Size(100, 28);
            ExtractionProgressBar.Visible = false;
            // 
            // ExportImageDialog
            // 
            ExportImageDialog.Filter = "Bitmap|*.bmp|Jpeg|*.jpg|Png|*png";
            // 
            // SplitBoxPageView
            // 
            SplitBoxPageView.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            SplitBoxPageView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            SplitBoxPageView.Location = new System.Drawing.Point(0, 43);
            SplitBoxPageView.Name = "SplitBoxPageView";
            // 
            // SplitBoxPageView.Panel1
            // 
            SplitBoxPageView.Panel1.AutoScroll = true;
            SplitBoxPageView.Panel1.Controls.Add(PageImagePreview);
            // 
            // SplitBoxPageView.Panel2
            // 
            SplitBoxPageView.Panel2.Controls.Add(ListboxChapters);
            SplitBoxPageView.Panel2Collapsed = true;
            SplitBoxPageView.Size = new System.Drawing.Size(1299, 536);
            SplitBoxPageView.SplitterDistance = 1025;
            SplitBoxPageView.TabIndex = 2;
            // 
            // PageImagePreview
            // 
            PageImagePreview.ImageLocation = "";
            PageImagePreview.Location = new System.Drawing.Point(4, 5);
            PageImagePreview.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            PageImagePreview.Name = "PageImagePreview";
            PageImagePreview.Size = new System.Drawing.Size(157, 94);
            PageImagePreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            PageImagePreview.TabIndex = 1;
            PageImagePreview.TabStop = false;
            PageImagePreview.WaitOnLoad = true;
            // 
            // ListboxChapters
            // 
            ListboxChapters.Dock = System.Windows.Forms.DockStyle.Fill;
            ListboxChapters.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            ListboxChapters.FormattingEnabled = true;
            ListboxChapters.ItemHeight = 73;
            ListboxChapters.Location = new System.Drawing.Point(0, 0);
            ListboxChapters.Name = "ListboxChapters";
            ListboxChapters.Size = new System.Drawing.Size(94, 98);
            ListboxChapters.TabIndex = 0;
            ListboxChapters.DrawItem += ListboxChapters_DrawItem;
            ListboxChapters.SelectedIndexChanged += ListboxChapters_SelectedIndexChanged;
            // 
            // ChapterImagesList
            // 
            ChapterImagesList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            ChapterImagesList.ImageSize = new System.Drawing.Size(48, 67);
            ChapterImagesList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // ImagePreviewForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1301, 580);
            Controls.Add(SplitBoxPageView);
            Controls.Add(PreviewToolStrip);
            DoubleBuffered = true;
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            Name = "ImagePreviewForm";
            StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            Text = "Preview";
            FormClosing += ImagePreviewForm_FormClosing;
            FormClosed += ImagePreviewForm_FormClosed;
            Load += ImagePreviewForm_Load;
            KeyDown += ImagePreviewForm_KeyDown;
            KeyUp += ImagePreviewForm_KeyUp;
            PreviewToolStrip.ResumeLayout(false);
            PreviewToolStrip.PerformLayout();
            SplitBoxPageView.Panel1.ResumeLayout(false);
            SplitBoxPageView.Panel1.PerformLayout();
            SplitBoxPageView.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)SplitBoxPageView).EndInit();
            SplitBoxPageView.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)PageImagePreview).EndInit();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolStrip PreviewToolStrip;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripButton3;
        private System.Windows.Forms.ToolStripButton toolStripButton4;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.SaveFileDialog ExportImageDialog;
        private System.Windows.Forms.ToolStripTextBox TextBoxJumpPage;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.SplitContainer SplitBoxPageView;
        private System.Windows.Forms.PictureBox PageImagePreview;
        private System.Windows.Forms.ListBox ListboxChapters;
        private System.Windows.Forms.ImageList ChapterImagesList;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripLabel toolStripLabel3;
        private System.Windows.Forms.ToolStripProgressBar ExtractionProgressBar;
    }
}