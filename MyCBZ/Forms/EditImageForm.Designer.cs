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
            Container = new System.Windows.Forms.ToolStripContainer();
            StatusBar = new System.Windows.Forms.StatusStrip();
            toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            ImageContentPanel = new System.Windows.Forms.Panel();
            EditImageBox = new System.Windows.Forms.PictureBox();
            ToolSelectionToolStrip = new System.Windows.Forms.ToolStrip();
            toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            ToolButtonCrop = new System.Windows.Forms.ToolStripButton();
            toolStripButton4 = new System.Windows.Forms.ToolStripButton();
            MainToolStrip = new System.Windows.Forms.ToolStrip();
            ToolButtonSave = new System.Windows.Forms.ToolStripButton();
            ToolButtonSaveAs = new System.Windows.Forms.ToolStripButton();
            toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            toolStripButton5 = new System.Windows.Forms.ToolStripButton();
            toolStripButton6 = new System.Windows.Forms.ToolStripButton();
            toolStripButton7 = new System.Windows.Forms.ToolStripButton();
            Container.BottomToolStripPanel.SuspendLayout();
            Container.ContentPanel.SuspendLayout();
            Container.LeftToolStripPanel.SuspendLayout();
            Container.TopToolStripPanel.SuspendLayout();
            Container.SuspendLayout();
            StatusBar.SuspendLayout();
            ImageContentPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)EditImageBox).BeginInit();
            ToolSelectionToolStrip.SuspendLayout();
            MainToolStrip.SuspendLayout();
            SuspendLayout();
            // 
            // Container
            // 
            // 
            // Container.BottomToolStripPanel
            // 
            Container.BottomToolStripPanel.Controls.Add(StatusBar);
            // 
            // Container.ContentPanel
            // 
            Container.ContentPanel.Controls.Add(ImageContentPanel);
            Container.ContentPanel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            Container.ContentPanel.Size = new System.Drawing.Size(788, 621);
            Container.Dock = System.Windows.Forms.DockStyle.Fill;
            // 
            // Container.LeftToolStripPanel
            // 
            Container.LeftToolStripPanel.Controls.Add(ToolSelectionToolStrip);
            Container.Location = new System.Drawing.Point(0, 0);
            Container.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            Container.Name = "Container";
            Container.Size = new System.Drawing.Size(825, 678);
            Container.TabIndex = 0;
            Container.Text = "toolStripContainer1";
            // 
            // Container.TopToolStripPanel
            // 
            Container.TopToolStripPanel.Controls.Add(MainToolStrip);
            // 
            // StatusBar
            // 
            StatusBar.Dock = System.Windows.Forms.DockStyle.None;
            StatusBar.ImageScalingSize = new System.Drawing.Size(20, 20);
            StatusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripStatusLabel1, toolStripStatusLabel2 });
            StatusBar.Location = new System.Drawing.Point(0, 0);
            StatusBar.Name = "StatusBar";
            StatusBar.Size = new System.Drawing.Size(825, 26);
            StatusBar.TabIndex = 0;
            // 
            // toolStripStatusLabel1
            // 
            toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            toolStripStatusLabel1.Size = new System.Drawing.Size(43, 20);
            toolStripStatusLabel1.Text = "page";
            // 
            // toolStripStatusLabel2
            // 
            toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            toolStripStatusLabel2.Size = new System.Drawing.Size(32, 20);
            toolStripStatusLabel2.Text = "0x0";
            // 
            // ImageContentPanel
            // 
            ImageContentPanel.AutoScroll = true;
            ImageContentPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            ImageContentPanel.BackColor = System.Drawing.SystemColors.ControlDark;
            ImageContentPanel.Controls.Add(EditImageBox);
            ImageContentPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            ImageContentPanel.Location = new System.Drawing.Point(0, 0);
            ImageContentPanel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            ImageContentPanel.Name = "ImageContentPanel";
            ImageContentPanel.Size = new System.Drawing.Size(788, 621);
            ImageContentPanel.TabIndex = 0;
            // 
            // EditImageBox
            // 
            EditImageBox.Cursor = System.Windows.Forms.Cursors.Cross;
            EditImageBox.Dock = System.Windows.Forms.DockStyle.Fill;
            EditImageBox.Location = new System.Drawing.Point(0, 0);
            EditImageBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            EditImageBox.Name = "EditImageBox";
            EditImageBox.Size = new System.Drawing.Size(788, 621);
            EditImageBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            EditImageBox.TabIndex = 0;
            EditImageBox.TabStop = false;
            // 
            // ToolSelectionToolStrip
            // 
            ToolSelectionToolStrip.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            ToolSelectionToolStrip.AutoSize = false;
            ToolSelectionToolStrip.Dock = System.Windows.Forms.DockStyle.None;
            ToolSelectionToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            ToolSelectionToolStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            ToolSelectionToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripButton1, ToolButtonCrop, toolStripButton4, toolStripSeparator2, toolStripButton2, toolStripButton3, toolStripButton5 });
            ToolSelectionToolStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.VerticalStackWithOverflow;
            ToolSelectionToolStrip.Location = new System.Drawing.Point(0, 4);
            ToolSelectionToolStrip.Name = "ToolSelectionToolStrip";
            ToolSelectionToolStrip.Size = new System.Drawing.Size(37, 481);
            ToolSelectionToolStrip.TabIndex = 0;
            // 
            // toolStripButton1
            // 
            toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButton1.Image = Properties.Resources.pencil;
            toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButton1.Name = "toolStripButton1";
            toolStripButton1.Size = new System.Drawing.Size(35, 24);
            toolStripButton1.Text = "toolStripButton1";
            // 
            // ToolButtonCrop
            // 
            ToolButtonCrop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            ToolButtonCrop.Image = Properties.Resources.selection;
            ToolButtonCrop.ImageTransparentColor = System.Drawing.Color.Magenta;
            ToolButtonCrop.Name = "ToolButtonCrop";
            ToolButtonCrop.Size = new System.Drawing.Size(35, 24);
            ToolButtonCrop.Text = "Crop image";
            ToolButtonCrop.Click += ToolButtonCrop_Click;
            // 
            // toolStripButton4
            // 
            toolStripButton4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButton4.Image = Properties.Resources.paint_bucket;
            toolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButton4.Name = "toolStripButton4";
            toolStripButton4.Size = new System.Drawing.Size(35, 24);
            toolStripButton4.Text = "toolStripButton4";
            // 
            // MainToolStrip
            // 
            MainToolStrip.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            MainToolStrip.AutoSize = false;
            MainToolStrip.Dock = System.Windows.Forms.DockStyle.None;
            MainToolStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            MainToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { ToolButtonSave, ToolButtonSaveAs, toolStripSeparator1, toolStripButton6, toolStripButton7 });
            MainToolStrip.Location = new System.Drawing.Point(4, 0);
            MainToolStrip.Name = "MainToolStrip";
            MainToolStrip.Size = new System.Drawing.Size(812, 31);
            MainToolStrip.TabIndex = 0;
            // 
            // ToolButtonSave
            // 
            ToolButtonSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            ToolButtonSave.Image = Properties.Resources.floppy_disk;
            ToolButtonSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            ToolButtonSave.Name = "ToolButtonSave";
            ToolButtonSave.Size = new System.Drawing.Size(29, 28);
            ToolButtonSave.Text = "Save";
            ToolButtonSave.Click += ToolButtonSave_Click;
            // 
            // ToolButtonSaveAs
            // 
            ToolButtonSaveAs.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            ToolButtonSaveAs.Image = Properties.Resources.save_as;
            ToolButtonSaveAs.ImageTransparentColor = System.Drawing.Color.Magenta;
            ToolButtonSaveAs.Name = "ToolButtonSaveAs";
            ToolButtonSaveAs.Size = new System.Drawing.Size(29, 28);
            ToolButtonSaveAs.Text = "Save as...";
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new System.Drawing.Size(6, 31);
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new System.Drawing.Size(35, 6);
            // 
            // toolStripButton2
            // 
            toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButton2.Image = (System.Drawing.Image)resources.GetObject("toolStripButton2.Image");
            toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButton2.Name = "toolStripButton2";
            toolStripButton2.Size = new System.Drawing.Size(35, 24);
            toolStripButton2.Text = "toolStripButton2";
            // 
            // toolStripButton3
            // 
            toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButton3.Image = (System.Drawing.Image)resources.GetObject("toolStripButton3.Image");
            toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButton3.Name = "toolStripButton3";
            toolStripButton3.Size = new System.Drawing.Size(35, 24);
            toolStripButton3.Text = "toolStripButton3";
            // 
            // toolStripButton5
            // 
            toolStripButton5.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButton5.Image = (System.Drawing.Image)resources.GetObject("toolStripButton5.Image");
            toolStripButton5.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButton5.Name = "toolStripButton5";
            toolStripButton5.Size = new System.Drawing.Size(35, 24);
            toolStripButton5.Text = "toolStripButton5";
            // 
            // toolStripButton6
            // 
            toolStripButton6.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButton6.Image = (System.Drawing.Image)resources.GetObject("toolStripButton6.Image");
            toolStripButton6.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButton6.Name = "toolStripButton6";
            toolStripButton6.Size = new System.Drawing.Size(29, 28);
            toolStripButton6.Text = "toolStripButton6";
            // 
            // toolStripButton7
            // 
            toolStripButton7.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButton7.Image = (System.Drawing.Image)resources.GetObject("toolStripButton7.Image");
            toolStripButton7.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButton7.Name = "toolStripButton7";
            toolStripButton7.Size = new System.Drawing.Size(29, 28);
            toolStripButton7.Text = "toolStripButton7";
            // 
            // EditImageForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(825, 678);
            Controls.Add(Container);
            DoubleBuffered = true;
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            Name = "EditImageForm";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Edit Image";
            FormClosed += EditImageForm_FormClosed;
            Load += EditImageForm_Load;
            Resize += EditImageForm_Resize;
            Container.BottomToolStripPanel.ResumeLayout(false);
            Container.BottomToolStripPanel.PerformLayout();
            Container.ContentPanel.ResumeLayout(false);
            Container.LeftToolStripPanel.ResumeLayout(false);
            Container.TopToolStripPanel.ResumeLayout(false);
            Container.ResumeLayout(false);
            Container.PerformLayout();
            StatusBar.ResumeLayout(false);
            StatusBar.PerformLayout();
            ImageContentPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)EditImageBox).EndInit();
            ToolSelectionToolStrip.ResumeLayout(false);
            ToolSelectionToolStrip.PerformLayout();
            MainToolStrip.ResumeLayout(false);
            MainToolStrip.PerformLayout();
            ResumeLayout(false);
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
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ToolStripButton toolStripButton3;
        private System.Windows.Forms.ToolStripButton toolStripButton5;
        private System.Windows.Forms.ToolStripButton toolStripButton6;
        private System.Windows.Forms.ToolStripButton toolStripButton7;
    }
}