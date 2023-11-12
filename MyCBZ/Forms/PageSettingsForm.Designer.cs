namespace Win_CBZ.Forms
{
    partial class PageSettingsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PageSettingsForm));
            this.SettingsTablePanel = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label3 = new System.Windows.Forms.Label();
            this.TextBoxFileLocation = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.LabelSize = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.LabelDimensions = new System.Windows.Forms.Label();
            this.LabelFormat = new System.Windows.Forms.Label();
            this.LabelImageFormat = new System.Windows.Forms.Label();
            this.CheckBoxPageDeleted = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.PageIndexTextbox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.PageNameTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.LabelDpi = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.LabelBits = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.LabelImageColors = new System.Windows.Forms.Label();
            this.HeaderPanel = new System.Windows.Forms.Panel();
            this.HeaderLabel = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.ButtonOk = new System.Windows.Forms.Button();
            this.ButtonCancel = new System.Windows.Forms.Button();
            this.PreviewThumbPictureBox = new System.Windows.Forms.PictureBox();
            this.SettingsTablePanel.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.HeaderPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PreviewThumbPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // SettingsTablePanel
            // 
            this.SettingsTablePanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SettingsTablePanel.ColumnCount = 3;
            this.SettingsTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 42.70833F));
            this.SettingsTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 57.29167F));
            this.SettingsTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 133F));
            this.SettingsTablePanel.Controls.Add(this.tableLayoutPanel1, 1, 1);
            this.SettingsTablePanel.Controls.Add(this.HeaderPanel, 0, 0);
            this.SettingsTablePanel.Controls.Add(this.ButtonOk, 1, 2);
            this.SettingsTablePanel.Controls.Add(this.ButtonCancel, 2, 2);
            this.SettingsTablePanel.Controls.Add(this.PreviewThumbPictureBox, 0, 1);
            this.SettingsTablePanel.Location = new System.Drawing.Point(1, 2);
            this.SettingsTablePanel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.SettingsTablePanel.Name = "SettingsTablePanel";
            this.SettingsTablePanel.RowCount = 3;
            this.SettingsTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 21.84211F));
            this.SettingsTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 78.1579F));
            this.SettingsTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 59F));
            this.SettingsTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.SettingsTablePanel.Size = new System.Drawing.Size(710, 462);
            this.SettingsTablePanel.TabIndex = 1;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 4;
            this.SettingsTablePanel.SetColumnSpan(this.tableLayoutPanel1, 2);
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 38.56041F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 61.43959F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 33F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 42F));
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.TextBoxFileLocation, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label5, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.LabelSize, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.label6, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.LabelDimensions, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.LabelFormat, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.LabelImageFormat, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.CheckBoxPageDeleted, 1, 7);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this.PageIndexTextbox, 1, 6);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.PageNameTextBox, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.label9, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.LabelDpi, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.label8, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.LabelBits, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.label7, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.LabelImageColors, 3, 2);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(249, 90);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 9;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 37F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 41F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 39F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 39F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 52F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(458, 310);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.Dock = System.Windows.Forms.DockStyle.Right;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(51, 0);
            this.label3.Name = "label3";
            this.label3.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.label3.Size = new System.Drawing.Size(93, 38);
            this.label3.TabIndex = 4;
            this.label3.Text = "Location";
            // 
            // TextBoxFileLocation
            // 
            this.TextBoxFileLocation.BackColor = System.Drawing.SystemColors.Control;
            this.tableLayoutPanel1.SetColumnSpan(this.TextBoxFileLocation, 3);
            this.TextBoxFileLocation.Location = new System.Drawing.Point(150, 2);
            this.TextBoxFileLocation.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.TextBoxFileLocation.Multiline = true;
            this.TextBoxFileLocation.Name = "TextBoxFileLocation";
            this.TextBoxFileLocation.ReadOnly = true;
            this.TextBoxFileLocation.Size = new System.Drawing.Size(304, 31);
            this.TextBoxFileLocation.TabIndex = 5;
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(105, 78);
            this.label5.Name = "label5";
            this.label5.Padding = new System.Windows.Forms.Padding(0, 7, 0, 0);
            this.label5.Size = new System.Drawing.Size(39, 24);
            this.label5.TabIndex = 8;
            this.label5.Text = "Size";
            // 
            // LabelSize
            // 
            this.LabelSize.AutoSize = true;
            this.LabelSize.Location = new System.Drawing.Point(150, 78);
            this.LabelSize.Name = "LabelSize";
            this.LabelSize.Padding = new System.Windows.Forms.Padding(0, 7, 0, 0);
            this.LabelSize.Size = new System.Drawing.Size(51, 23);
            this.LabelSize.TabIndex = 9;
            this.LabelSize.Text = "0 Bytes";
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(52, 115);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.label6.Size = new System.Drawing.Size(91, 23);
            this.label6.TabIndex = 10;
            this.label6.Text = "Dimensions";
            // 
            // LabelDimensions
            // 
            this.LabelDimensions.AutoSize = true;
            this.LabelDimensions.Location = new System.Drawing.Point(151, 115);
            this.LabelDimensions.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LabelDimensions.Name = "LabelDimensions";
            this.LabelDimensions.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.LabelDimensions.Size = new System.Drawing.Size(65, 22);
            this.LabelDimensions.TabIndex = 11;
            this.LabelDimensions.Text = "0 x 0 Pixel";
            // 
            // LabelFormat
            // 
            this.LabelFormat.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.LabelFormat.AutoSize = true;
            this.LabelFormat.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelFormat.Location = new System.Drawing.Point(85, 38);
            this.LabelFormat.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LabelFormat.Name = "LabelFormat";
            this.LabelFormat.Padding = new System.Windows.Forms.Padding(0, 7, 0, 0);
            this.LabelFormat.Size = new System.Drawing.Size(58, 24);
            this.LabelFormat.TabIndex = 12;
            this.LabelFormat.Text = "Format";
            // 
            // LabelImageFormat
            // 
            this.LabelImageFormat.AutoSize = true;
            this.LabelImageFormat.Location = new System.Drawing.Point(151, 38);
            this.LabelImageFormat.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LabelImageFormat.Name = "LabelImageFormat";
            this.LabelImageFormat.Padding = new System.Windows.Forms.Padding(0, 7, 0, 0);
            this.LabelImageFormat.Size = new System.Drawing.Size(59, 23);
            this.LabelImageFormat.TabIndex = 15;
            this.LabelImageFormat.Text = "unknown";
            // 
            // CheckBoxPageDeleted
            // 
            this.CheckBoxPageDeleted.AutoSize = true;
            this.CheckBoxPageDeleted.Location = new System.Drawing.Point(151, 278);
            this.CheckBoxPageDeleted.Margin = new System.Windows.Forms.Padding(4);
            this.CheckBoxPageDeleted.Name = "CheckBoxPageDeleted";
            this.CheckBoxPageDeleted.Size = new System.Drawing.Size(18, 17);
            this.CheckBoxPageDeleted.TabIndex = 7;
            this.CheckBoxPageDeleted.UseVisualStyleBackColor = true;
            this.CheckBoxPageDeleted.CheckedChanged += new System.EventHandler(this.CheckBoxPageDeleted_CheckedChanged);
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(79, 279);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 5, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(64, 17);
            this.label4.TabIndex = 6;
            this.label4.Text = "Deleted";
            // 
            // PageIndexTextbox
            // 
            this.PageIndexTextbox.Location = new System.Drawing.Point(152, 237);
            this.PageIndexTextbox.Margin = new System.Windows.Forms.Padding(5, 2, 3, 2);
            this.PageIndexTextbox.Name = "PageIndexTextbox";
            this.PageIndexTextbox.Size = new System.Drawing.Size(71, 22);
            this.PageIndexTextbox.TabIndex = 0;
            this.PageIndexTextbox.TextChanged += new System.EventHandler(this.PageIndexTextbox_TextChanged);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(98, 235);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.label1.Size = new System.Drawing.Size(46, 23);
            this.label1.TabIndex = 1;
            this.label1.Text = "Index";
            // 
            // PageNameTextBox
            // 
            this.PageNameTextBox.Location = new System.Drawing.Point(152, 198);
            this.PageNameTextBox.Margin = new System.Windows.Forms.Padding(5, 2, 3, 2);
            this.PageNameTextBox.Name = "PageNameTextBox";
            this.PageNameTextBox.Size = new System.Drawing.Size(199, 22);
            this.PageNameTextBox.TabIndex = 2;
            this.PageNameTextBox.TextChanged += new System.EventHandler(this.PageNameTextBox_TextChanged);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(95, 196);
            this.label2.Name = "label2";
            this.label2.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.label2.Size = new System.Drawing.Size(49, 23);
            this.label2.TabIndex = 3;
            this.label2.Text = "Name";
            // 
            // label9
            // 
            this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(110, 155);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Padding = new System.Windows.Forms.Padding(0, 7, 0, 0);
            this.label9.Size = new System.Drawing.Size(33, 24);
            this.label9.TabIndex = 14;
            this.label9.Text = "DPI";
            // 
            // LabelDpi
            // 
            this.LabelDpi.AutoSize = true;
            this.LabelDpi.Location = new System.Drawing.Point(151, 155);
            this.LabelDpi.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LabelDpi.Name = "LabelDpi";
            this.LabelDpi.Padding = new System.Windows.Forms.Padding(0, 7, 0, 0);
            this.LabelDpi.Size = new System.Drawing.Size(14, 23);
            this.LabelDpi.TabIndex = 17;
            this.LabelDpi.Text = "0";
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(389, 38);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Padding = new System.Windows.Forms.Padding(0, 7, 0, 0);
            this.label8.Size = new System.Drawing.Size(22, 40);
            this.label8.TabIndex = 13;
            this.label8.Text = "Bits";
            this.label8.Visible = false;
            // 
            // LabelBits
            // 
            this.LabelBits.AutoSize = true;
            this.LabelBits.Location = new System.Drawing.Point(419, 38);
            this.LabelBits.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LabelBits.Name = "LabelBits";
            this.LabelBits.Padding = new System.Windows.Forms.Padding(0, 7, 0, 0);
            this.LabelBits.Size = new System.Drawing.Size(14, 23);
            this.LabelBits.TabIndex = 16;
            this.LabelBits.Text = "0";
            this.LabelBits.Visible = false;
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(388, 78);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Padding = new System.Windows.Forms.Padding(0, 7, 0, 0);
            this.label7.Size = new System.Drawing.Size(23, 37);
            this.label7.TabIndex = 18;
            this.label7.Text = "Colors";
            this.label7.Visible = false;
            // 
            // LabelImageColors
            // 
            this.LabelImageColors.AutoSize = true;
            this.LabelImageColors.Location = new System.Drawing.Point(419, 78);
            this.LabelImageColors.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LabelImageColors.Name = "LabelImageColors";
            this.LabelImageColors.Padding = new System.Windows.Forms.Padding(0, 7, 0, 0);
            this.LabelImageColors.Size = new System.Drawing.Size(14, 23);
            this.LabelImageColors.TabIndex = 19;
            this.LabelImageColors.Text = "0";
            this.LabelImageColors.Visible = false;
            // 
            // HeaderPanel
            // 
            this.HeaderPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.HeaderPanel.BackColor = System.Drawing.Color.White;
            this.SettingsTablePanel.SetColumnSpan(this.HeaderPanel, 3);
            this.HeaderPanel.Controls.Add(this.HeaderLabel);
            this.HeaderPanel.Controls.Add(this.pictureBox1);
            this.HeaderPanel.Location = new System.Drawing.Point(3, 2);
            this.HeaderPanel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.HeaderPanel.Name = "HeaderPanel";
            this.HeaderPanel.Size = new System.Drawing.Size(704, 82);
            this.HeaderPanel.TabIndex = 0;
            // 
            // HeaderLabel
            // 
            this.HeaderLabel.AutoSize = true;
            this.HeaderLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HeaderLabel.Location = new System.Drawing.Point(112, 23);
            this.HeaderLabel.Name = "HeaderLabel";
            this.HeaderLabel.Size = new System.Drawing.Size(148, 28);
            this.HeaderLabel.TabIndex = 1;
            this.HeaderLabel.Text = "Page Properties";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Win_CBZ.Properties.Resources.edit_large;
            this.pictureBox1.Location = new System.Drawing.Point(24, 7);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(61, 55);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // ButtonOk
            // 
            this.ButtonOk.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.ButtonOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.ButtonOk.Location = new System.Drawing.Point(462, 415);
            this.ButtonOk.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ButtonOk.Name = "ButtonOk";
            this.ButtonOk.Size = new System.Drawing.Size(111, 33);
            this.ButtonOk.TabIndex = 2;
            this.ButtonOk.Text = "Ok";
            this.ButtonOk.UseVisualStyleBackColor = true;
            this.ButtonOk.Click += new System.EventHandler(this.ButtonOk_Click);
            // 
            // ButtonCancel
            // 
            this.ButtonCancel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.ButtonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ButtonCancel.Location = new System.Drawing.Point(588, 415);
            this.ButtonCancel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ButtonCancel.Name = "ButtonCancel";
            this.ButtonCancel.Size = new System.Drawing.Size(119, 33);
            this.ButtonCancel.TabIndex = 3;
            this.ButtonCancel.Text = "Cancel";
            this.ButtonCancel.UseVisualStyleBackColor = true;
            this.ButtonCancel.Click += new System.EventHandler(this.ButtonCancel_Click);
            // 
            // PreviewThumbPictureBox
            // 
            this.PreviewThumbPictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PreviewThumbPictureBox.Location = new System.Drawing.Point(3, 90);
            this.PreviewThumbPictureBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.PreviewThumbPictureBox.Name = "PreviewThumbPictureBox";
            this.PreviewThumbPictureBox.Size = new System.Drawing.Size(240, 268);
            this.PreviewThumbPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.PreviewThumbPictureBox.TabIndex = 5;
            this.PreviewThumbPictureBox.TabStop = false;
            // 
            // PageSettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(717, 468);
            this.Controls.Add(this.SettingsTablePanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "PageSettingsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Page Properties";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PageSettingsForm_FormClosing);
            this.SettingsTablePanel.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.HeaderPanel.ResumeLayout(false);
            this.HeaderPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PreviewThumbPictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel SettingsTablePanel;
        private System.Windows.Forms.Panel HeaderPanel;
        private System.Windows.Forms.Label HeaderLabel;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button ButtonOk;
        private System.Windows.Forms.Button ButtonCancel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TextBox PageIndexTextbox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox PageNameTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox TextBoxFileLocation;
        private System.Windows.Forms.PictureBox PreviewThumbPictureBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox CheckBoxPageDeleted;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label LabelSize;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label LabelDimensions;
        private System.Windows.Forms.Label LabelFormat;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label LabelImageFormat;
        private System.Windows.Forms.Label LabelBits;
        private System.Windows.Forms.Label LabelDpi;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label LabelImageColors;
    }
}