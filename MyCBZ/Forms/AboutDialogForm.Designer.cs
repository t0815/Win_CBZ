namespace MyCBZ.Forms
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.AppNameLabel = new System.Windows.Forms.Label();
            this.AboutBoxBanner = new System.Windows.Forms.PictureBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.ButtonCloseDialog = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.LicenseInfoRichtextBox = new System.Windows.Forms.RichTextBox();
            this.AboutPictureBox = new System.Windows.Forms.PictureBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AboutBoxBanner)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AboutPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 84F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 67F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(529, 387);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.panel1.Controls.Add(this.AppNameLabel);
            this.panel1.Controls.Add(this.AboutBoxBanner);
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(521, 78);
            this.panel1.TabIndex = 0;
            // 
            // AppNameLabel
            // 
            this.AppNameLabel.AutoSize = true;
            this.AppNameLabel.BackColor = System.Drawing.Color.Transparent;
            this.AppNameLabel.Font = new System.Drawing.Font("Courier New", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AppNameLabel.Location = new System.Drawing.Point(351, 25);
            this.AppNameLabel.Name = "AppNameLabel";
            this.AppNameLabel.Size = new System.Drawing.Size(167, 31);
            this.AppNameLabel.TabIndex = 1;
            this.AppNameLabel.Text = "<AppName>";
            // 
            // AboutBoxBanner
            // 
            this.AboutBoxBanner.Dock = System.Windows.Forms.DockStyle.Left;
            this.AboutBoxBanner.Image = global::MyCBZ.Properties.Resources.Trash_About;
            this.AboutBoxBanner.Location = new System.Drawing.Point(0, 0);
            this.AboutBoxBanner.Name = "AboutBoxBanner";
            this.AboutBoxBanner.Size = new System.Drawing.Size(289, 78);
            this.AboutBoxBanner.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.AboutBoxBanner.TabIndex = 0;
            this.AboutBoxBanner.TabStop = false;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.BackColor = System.Drawing.SystemColors.Control;
            this.flowLayoutPanel1.Controls.Add(this.ButtonCloseDialog);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 323);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Padding = new System.Windows.Forms.Padding(8);
            this.flowLayoutPanel1.Size = new System.Drawing.Size(523, 61);
            this.flowLayoutPanel1.TabIndex = 1;
            // 
            // ButtonCloseDialog
            // 
            this.ButtonCloseDialog.Location = new System.Drawing.Point(380, 11);
            this.ButtonCloseDialog.Name = "ButtonCloseDialog";
            this.ButtonCloseDialog.Size = new System.Drawing.Size(124, 34);
            this.ButtonCloseDialog.TabIndex = 0;
            this.ButtonCloseDialog.Text = "Yea, sure...";
            this.ButtonCloseDialog.UseVisualStyleBackColor = true;
            this.ButtonCloseDialog.Click += new System.EventHandler(this.button1_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.LicenseInfoRichtextBox);
            this.panel2.Controls.Add(this.AboutPictureBox);
            this.panel2.Location = new System.Drawing.Point(3, 87);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(521, 203);
            this.panel2.TabIndex = 2;
            // 
            // LicenseInfoRichtextBox
            // 
            this.LicenseInfoRichtextBox.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.LicenseInfoRichtextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LicenseInfoRichtextBox.Location = new System.Drawing.Point(153, 24);
            this.LicenseInfoRichtextBox.Name = "LicenseInfoRichtextBox";
            this.LicenseInfoRichtextBox.ReadOnly = true;
            this.LicenseInfoRichtextBox.Size = new System.Drawing.Size(348, 165);
            this.LicenseInfoRichtextBox.TabIndex = 1;
            this.LicenseInfoRichtextBox.Text = "";
            // 
            // AboutPictureBox
            // 
            this.AboutPictureBox.Image = global::MyCBZ.Properties.Resources.box_surprise_large;
            this.AboutPictureBox.Location = new System.Drawing.Point(17, 22);
            this.AboutPictureBox.Name = "AboutPictureBox";
            this.AboutPictureBox.Size = new System.Drawing.Size(130, 167);
            this.AboutPictureBox.TabIndex = 0;
            this.AboutPictureBox.TabStop = false;
            // 
            // AboutDialogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(529, 387);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "AboutDialogForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "About this cool Software";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AboutBoxBanner)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.AboutPictureBox)).EndInit();
            this.ResumeLayout(false);

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
    }
}