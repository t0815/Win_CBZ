namespace Win_CBZ.Forms
{
    partial class EditImageForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditImageForm));
            this.Container = new System.Windows.Forms.ToolStripContainer();
            this.StatusBar = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.ImageContentPanel = new System.Windows.Forms.Panel();
            this.EditImageBox = new System.Windows.Forms.PictureBox();
            this.ToolSelectionToolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.ToolButtonCrop = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
            this.MainToolStrip = new System.Windows.Forms.ToolStrip();
            this.ToolButtonSave = new System.Windows.Forms.ToolStripButton();
            this.ToolButtonSaveAs = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.Container.BottomToolStripPanel.SuspendLayout();
            this.Container.ContentPanel.SuspendLayout();
            this.Container.LeftToolStripPanel.SuspendLayout();
            this.Container.TopToolStripPanel.SuspendLayout();
            this.Container.SuspendLayout();
            this.StatusBar.SuspendLayout();
            this.ImageContentPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.EditImageBox)).BeginInit();
            this.ToolSelectionToolStrip.SuspendLayout();
            this.MainToolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // Container
            // 
            // 
            // Container.BottomToolStripPanel
            // 
            this.Container.BottomToolStripPanel.Controls.Add(this.StatusBar);
            // 
            // Container.ContentPanel
            // 
            this.Container.ContentPanel.Controls.Add(this.ImageContentPanel);
            this.Container.ContentPanel.Size = new System.Drawing.Size(788, 485);
            this.Container.Dock = System.Windows.Forms.DockStyle.Fill;
            // 
            // Container.LeftToolStripPanel
            // 
            this.Container.LeftToolStripPanel.Controls.Add(this.ToolSelectionToolStrip);
            this.Container.Location = new System.Drawing.Point(0, 0);
            this.Container.Name = "Container";
            this.Container.Size = new System.Drawing.Size(825, 542);
            this.Container.TabIndex = 0;
            this.Container.Text = "toolStripContainer1";
            // 
            // Container.TopToolStripPanel
            // 
            this.Container.TopToolStripPanel.Controls.Add(this.MainToolStrip);
            // 
            // StatusBar
            // 
            this.StatusBar.Dock = System.Windows.Forms.DockStyle.None;
            this.StatusBar.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.StatusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2});
            this.StatusBar.Location = new System.Drawing.Point(0, 0);
            this.StatusBar.Name = "StatusBar";
            this.StatusBar.Size = new System.Drawing.Size(825, 26);
            this.StatusBar.TabIndex = 0;
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(43, 20);
            this.toolStripStatusLabel1.Text = "page";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(32, 20);
            this.toolStripStatusLabel2.Text = "0x0";
            // 
            // ImageContentPanel
            // 
            this.ImageContentPanel.AutoScroll = true;
            this.ImageContentPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ImageContentPanel.BackColor = System.Drawing.SystemColors.ControlDark;
            this.ImageContentPanel.Controls.Add(this.EditImageBox);
            this.ImageContentPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ImageContentPanel.Location = new System.Drawing.Point(0, 0);
            this.ImageContentPanel.Name = "ImageContentPanel";
            this.ImageContentPanel.Size = new System.Drawing.Size(788, 485);
            this.ImageContentPanel.TabIndex = 0;
            // 
            // EditImageBox
            // 
            this.EditImageBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EditImageBox.Location = new System.Drawing.Point(0, 0);
            this.EditImageBox.Name = "EditImageBox";
            this.EditImageBox.Size = new System.Drawing.Size(788, 485);
            this.EditImageBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.EditImageBox.TabIndex = 0;
            this.EditImageBox.TabStop = false;
            // 
            // ToolSelectionToolStrip
            // 
            this.ToolSelectionToolStrip.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.ToolSelectionToolStrip.AutoSize = false;
            this.ToolSelectionToolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.ToolSelectionToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.ToolSelectionToolStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.ToolSelectionToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.ToolButtonCrop,
            this.toolStripButton4});
            this.ToolSelectionToolStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.VerticalStackWithOverflow;
            this.ToolSelectionToolStrip.Location = new System.Drawing.Point(0, 4);
            this.ToolSelectionToolStrip.Name = "ToolSelectionToolStrip";
            this.ToolSelectionToolStrip.Size = new System.Drawing.Size(37, 481);
            this.ToolSelectionToolStrip.TabIndex = 0;
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = global::Win_CBZ.Properties.Resources.pencil;
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(35, 24);
            this.toolStripButton1.Text = "toolStripButton1";
            // 
            // ToolButtonCrop
            // 
            this.ToolButtonCrop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ToolButtonCrop.Image = global::Win_CBZ.Properties.Resources.selection;
            this.ToolButtonCrop.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ToolButtonCrop.Name = "ToolButtonCrop";
            this.ToolButtonCrop.Size = new System.Drawing.Size(35, 24);
            this.ToolButtonCrop.Text = "Crop image";
            this.ToolButtonCrop.Click += new System.EventHandler(this.ToolButtonCrop_Click);
            // 
            // toolStripButton4
            // 
            this.toolStripButton4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton4.Image = global::Win_CBZ.Properties.Resources.paint_bucket;
            this.toolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton4.Name = "toolStripButton4";
            this.toolStripButton4.Size = new System.Drawing.Size(35, 24);
            this.toolStripButton4.Text = "toolStripButton4";
            // 
            // MainToolStrip
            // 
            this.MainToolStrip.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MainToolStrip.AutoSize = false;
            this.MainToolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.MainToolStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.MainToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolButtonSave,
            this.ToolButtonSaveAs,
            this.toolStripSeparator1});
            this.MainToolStrip.Location = new System.Drawing.Point(4, 0);
            this.MainToolStrip.Name = "MainToolStrip";
            this.MainToolStrip.Size = new System.Drawing.Size(146, 31);
            this.MainToolStrip.TabIndex = 0;
            // 
            // ToolButtonSave
            // 
            this.ToolButtonSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ToolButtonSave.Image = global::Win_CBZ.Properties.Resources.floppy_disk;
            this.ToolButtonSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ToolButtonSave.Name = "ToolButtonSave";
            this.ToolButtonSave.Size = new System.Drawing.Size(29, 28);
            this.ToolButtonSave.Text = "Save";
            this.ToolButtonSave.Click += new System.EventHandler(this.ToolButtonSave_Click);
            // 
            // ToolButtonSaveAs
            // 
            this.ToolButtonSaveAs.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ToolButtonSaveAs.Image = global::Win_CBZ.Properties.Resources.save_as;
            this.ToolButtonSaveAs.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ToolButtonSaveAs.Name = "ToolButtonSaveAs";
            this.ToolButtonSaveAs.Size = new System.Drawing.Size(29, 28);
            this.ToolButtonSaveAs.Text = "Save as...";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 31);
            // 
            // EditImageForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(825, 542);
            this.Controls.Add(this.Container);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "EditImageForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Edit Image";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.EditImageForm_FormClosed);
            this.Load += new System.EventHandler(this.EditImageForm_Load);
            this.Resize += new System.EventHandler(this.EditImageForm_Resize);
            this.Container.BottomToolStripPanel.ResumeLayout(false);
            this.Container.BottomToolStripPanel.PerformLayout();
            this.Container.ContentPanel.ResumeLayout(false);
            this.Container.LeftToolStripPanel.ResumeLayout(false);
            this.Container.TopToolStripPanel.ResumeLayout(false);
            this.Container.ResumeLayout(false);
            this.Container.PerformLayout();
            this.StatusBar.ResumeLayout(false);
            this.StatusBar.PerformLayout();
            this.ImageContentPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.EditImageBox)).EndInit();
            this.ToolSelectionToolStrip.ResumeLayout(false);
            this.ToolSelectionToolStrip.PerformLayout();
            this.MainToolStrip.ResumeLayout(false);
            this.MainToolStrip.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStripContainer Container;
        private System.Windows.Forms.StatusStrip StatusBar;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStrip ToolSelectionToolStrip;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripButton ToolButtonCrop;
        private System.Windows.Forms.ToolStripButton toolStripButton4;
        private System.Windows.Forms.ToolStrip MainToolStrip;
        private System.Windows.Forms.ToolStripButton ToolButtonSaveAs;
        private System.Windows.Forms.ToolStripButton ToolButtonSave;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.Panel ImageContentPanel;
        private System.Windows.Forms.PictureBox EditImageBox;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
    }
}