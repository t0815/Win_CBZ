namespace Win_CBZ.Forms
{
    partial class AboutDialogForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutDialogForm));
            tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            panel1 = new System.Windows.Forms.Panel();
            AppVersionLabel = new System.Windows.Forms.Label();
            AppNameLabel = new System.Windows.Forms.Label();
            AboutBoxBanner = new System.Windows.Forms.PictureBox();
            flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            ButtonCloseDialog = new System.Windows.Forms.Button();
            panel2 = new System.Windows.Forms.Panel();
            LicenseInfoRichtextBox = new System.Windows.Forms.RichTextBox();
            AboutPictureBox = new System.Windows.Forms.PictureBox();
            tableLayoutPanel1.SuspendLayout();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)AboutBoxBanner).BeginInit();
            flowLayoutPanel1.SuspendLayout();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)AboutPictureBox).BeginInit();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(panel1, 0, 0);
            tableLayoutPanel1.Controls.Add(flowLayoutPanel1, 0, 2);
            tableLayoutPanel1.Controls.Add(panel2, 0, 1);
            tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 105F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 82F));
            tableLayoutPanel1.Size = new System.Drawing.Size(703, 482);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // panel1
            // 
            panel1.BackColor = System.Drawing.SystemColors.ControlLightLight;
            panel1.Controls.Add(AppVersionLabel);
            panel1.Controls.Add(AppNameLabel);
            panel1.Controls.Add(AboutBoxBanner);
            panel1.Dock = System.Windows.Forms.DockStyle.Top;
            panel1.Location = new System.Drawing.Point(0, 0);
            panel1.Margin = new System.Windows.Forms.Padding(0);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(703, 98);
            panel1.TabIndex = 0;
            // 
            // AppVersionLabel
            // 
            AppVersionLabel.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            AppVersionLabel.AutoSize = true;
            AppVersionLabel.BackColor = System.Drawing.Color.Transparent;
            AppVersionLabel.Location = new System.Drawing.Point(598, 61);
            AppVersionLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            AppVersionLabel.Name = "AppVersionLabel";
            AppVersionLabel.Size = new System.Drawing.Size(105, 20);
            AppVersionLabel.TabIndex = 2;
            AppVersionLabel.Text = "<AppVersion>";
            // 
            // AppNameLabel
            // 
            AppNameLabel.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            AppNameLabel.AutoSize = true;
            AppNameLabel.BackColor = System.Drawing.Color.Transparent;
            AppNameLabel.Font = new System.Drawing.Font("Courier New", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            AppNameLabel.Location = new System.Drawing.Point(527, 9);
            AppNameLabel.Name = "AppNameLabel";
            AppNameLabel.Size = new System.Drawing.Size(158, 31);
            AppNameLabel.TabIndex = 1;
            AppNameLabel.Text = "<AppName>";
            // 
            // AboutBoxBanner
            // 
            AboutBoxBanner.Dock = System.Windows.Forms.DockStyle.Left;
            AboutBoxBanner.Image = Properties.Resources.TrashBannerNew;
            AboutBoxBanner.Location = new System.Drawing.Point(0, 0);
            AboutBoxBanner.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            AboutBoxBanner.Name = "AboutBoxBanner";
            AboutBoxBanner.Size = new System.Drawing.Size(521, 98);
            AboutBoxBanner.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            AboutBoxBanner.TabIndex = 0;
            AboutBoxBanner.TabStop = false;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.BackColor = System.Drawing.SystemColors.Control;
            flowLayoutPanel1.Controls.Add(ButtonCloseDialog);
            flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            flowLayoutPanel1.Location = new System.Drawing.Point(3, 402);
            flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Padding = new System.Windows.Forms.Padding(8, 9, 8, 9);
            flowLayoutPanel1.Size = new System.Drawing.Size(697, 78);
            flowLayoutPanel1.TabIndex = 1;
            // 
            // ButtonCloseDialog
            // 
            ButtonCloseDialog.Location = new System.Drawing.Point(554, 11);
            ButtonCloseDialog.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            ButtonCloseDialog.Name = "ButtonCloseDialog";
            ButtonCloseDialog.Size = new System.Drawing.Size(124, 39);
            ButtonCloseDialog.TabIndex = 0;
            ButtonCloseDialog.Text = "Yea, sure...";
            ButtonCloseDialog.UseVisualStyleBackColor = true;
            ButtonCloseDialog.Click += button1_Click;
            // 
            // panel2
            // 
            panel2.Controls.Add(LicenseInfoRichtextBox);
            panel2.Controls.Add(AboutPictureBox);
            panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            panel2.Location = new System.Drawing.Point(3, 107);
            panel2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            panel2.Name = "panel2";
            panel2.Size = new System.Drawing.Size(697, 291);
            panel2.TabIndex = 2;
            // 
            // LicenseInfoRichtextBox
            // 
            LicenseInfoRichtextBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            LicenseInfoRichtextBox.BackColor = System.Drawing.SystemColors.ButtonFace;
            LicenseInfoRichtextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            LicenseInfoRichtextBox.Location = new System.Drawing.Point(153, 28);
            LicenseInfoRichtextBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            LicenseInfoRichtextBox.Name = "LicenseInfoRichtextBox";
            LicenseInfoRichtextBox.ReadOnly = true;
            LicenseInfoRichtextBox.Size = new System.Drawing.Size(530, 251);
            LicenseInfoRichtextBox.TabIndex = 1;
            LicenseInfoRichtextBox.Text = "";
            LicenseInfoRichtextBox.LinkClicked += LicenseInfoRichtextBox_LinkClicked;
            // 
            // AboutPictureBox
            // 
            AboutPictureBox.Image = Properties.Resources.box_surprise_large;
            AboutPictureBox.Location = new System.Drawing.Point(17, 28);
            AboutPictureBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            AboutPictureBox.Name = "AboutPictureBox";
            AboutPictureBox.Size = new System.Drawing.Size(131, 209);
            AboutPictureBox.TabIndex = 0;
            AboutPictureBox.TabStop = false;
            // 
            // AboutDialogForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(703, 482);
            Controls.Add(tableLayoutPanel1);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            Name = "AboutDialogForm";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "About this cool Software";
            KeyUp += AboutDialogForm_KeyUp;
            tableLayoutPanel1.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)AboutBoxBanner).EndInit();
            flowLayoutPanel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)AboutPictureBox).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label AppNameLabel;
        private System.Windows.Forms.PictureBox AboutBoxBanner;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.PictureBox AboutPictureBox;
        private System.Windows.Forms.Button ButtonCloseDialog;
        private System.Windows.Forms.RichTextBox LicenseInfoRichtextBox;
        private System.Windows.Forms.Label AppVersionLabel;
    }
}