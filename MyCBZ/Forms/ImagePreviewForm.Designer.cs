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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImagePreviewForm));
            this.ImagePreviewPanel = new System.Windows.Forms.Panel();
            this.PageImagePreview = new System.Windows.Forms.PictureBox();
            this.PreviewToolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.ExportImageDialog = new System.Windows.Forms.SaveFileDialog();
            this.ImagePreviewPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PageImagePreview)).BeginInit();
            this.PreviewToolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // ImagePreviewPanel
            // 
            this.ImagePreviewPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ImagePreviewPanel.AutoScroll = true;
            this.ImagePreviewPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ImagePreviewPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ImagePreviewPanel.Controls.Add(this.PageImagePreview);
            this.ImagePreviewPanel.Location = new System.Drawing.Point(0, 33);
            this.ImagePreviewPanel.Margin = new System.Windows.Forms.Padding(2);
            this.ImagePreviewPanel.Name = "ImagePreviewPanel";
            this.ImagePreviewPanel.Size = new System.Drawing.Size(777, 703);
            this.ImagePreviewPanel.TabIndex = 0;
            this.ImagePreviewPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.ImagePreviewPanel_Paint);
            // 
            // PageImagePreview
            // 
            this.PageImagePreview.Location = new System.Drawing.Point(3, 3);
            this.PageImagePreview.Name = "PageImagePreview";
            this.PageImagePreview.Size = new System.Drawing.Size(157, 94);
            this.PageImagePreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.PageImagePreview.TabIndex = 0;
            this.PageImagePreview.TabStop = false;
            this.PageImagePreview.WaitOnLoad = true;
            this.PageImagePreview.LoadCompleted += new System.ComponentModel.AsyncCompletedEventHandler(this.PageImagePreview_LoadCompleted);
            this.PageImagePreview.BindingContextChanged += new System.EventHandler(this.PageImagePreview_BindingContextChanged);
            this.PageImagePreview.Paint += new System.Windows.Forms.PaintEventHandler(this.PageImagePreview_Paint);
            // 
            // PreviewToolStrip
            // 
            this.PreviewToolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.PreviewToolStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.PreviewToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripSeparator1,
            this.toolStripButton3,
            this.toolStripButton4,
            this.toolStripSeparator2,
            this.toolStripLabel1});
            this.PreviewToolStrip.Location = new System.Drawing.Point(0, 0);
            this.PreviewToolStrip.Name = "PreviewToolStrip";
            this.PreviewToolStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.PreviewToolStrip.Size = new System.Drawing.Size(361, 31);
            this.PreviewToolStrip.TabIndex = 1;
            this.PreviewToolStrip.Text = "PreviewToolStrip";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = global::Win_CBZ.Properties.Resources.save_as;
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(28, 28);
            this.toolStripButton1.Text = "toolStripButton1";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 31);
            // 
            // toolStripButton3
            // 
            this.toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton3.Image = global::Win_CBZ.Properties.Resources.nav_left;
            this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton3.Name = "toolStripButton3";
            this.toolStripButton3.Size = new System.Drawing.Size(28, 28);
            this.toolStripButton3.Text = "toolStripButton3";
            // 
            // toolStripButton4
            // 
            this.toolStripButton4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton4.Image = global::Win_CBZ.Properties.Resources.nav_right;
            this.toolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton4.Name = "toolStripButton4";
            this.toolStripButton4.Size = new System.Drawing.Size(28, 28);
            this.toolStripButton4.Text = "toolStripButton4";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 31);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(253, 28);
            this.toolStripLabel1.Text = "Preview Mode (Arrow Left/- Right to Navigate)";
            // 
            // ExportImageDialog
            // 
            this.ExportImageDialog.Filter = "Bitmap|*.bmp|Jpeg|*.jpg|Png|*png";
            // 
            // ImagePreviewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(777, 736);
            this.Controls.Add(this.PreviewToolStrip);
            this.Controls.Add(this.ImagePreviewPanel);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "ImagePreviewForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Preview";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ImagePreviewForm_FormClosed);
            this.Load += new System.EventHandler(this.ImagePreviewForm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ImagePreviewForm_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ImagePreviewForm_KeyUp);
            this.ImagePreviewPanel.ResumeLayout(false);
            this.ImagePreviewPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PageImagePreview)).EndInit();
            this.PreviewToolStrip.ResumeLayout(false);
            this.PreviewToolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel ImagePreviewPanel;
        private System.Windows.Forms.ToolStrip PreviewToolStrip;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.PictureBox PageImagePreview;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripButton3;
        private System.Windows.Forms.ToolStripButton toolStripButton4;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.SaveFileDialog ExportImageDialog;
    }
}