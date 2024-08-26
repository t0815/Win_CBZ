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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PageSettingsForm));
            SettingsTablePanel = new System.Windows.Forms.TableLayoutPanel();
            ImagePreviewTableLayout = new System.Windows.Forms.TableLayoutPanel();
            ImagePreviewButton = new System.Windows.Forms.Button();
            flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            InfoIconTooltip = new System.Windows.Forms.PictureBox();
            label12 = new System.Windows.Forms.Label();
            ProgressBarReload = new System.Windows.Forms.ProgressBar();
            HeaderPanel = new System.Windows.Forms.Panel();
            HeaderLabel = new System.Windows.Forms.Label();
            pictureBox1 = new System.Windows.Forms.PictureBox();
            ButtonOk = new System.Windows.Forms.Button();
            ButtonCancel = new System.Windows.Forms.Button();
            PreviewThumbPictureBox = new System.Windows.Forms.PictureBox();
            TabControlPageProperties = new System.Windows.Forms.TabControl();
            tabPage1 = new System.Windows.Forms.TabPage();
            panel1 = new System.Windows.Forms.Panel();
            LoadingIndicator = new LoadingIndicator.WinForms.BoxIndicatorControl();
            tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            label3 = new System.Windows.Forms.Label();
            label5 = new System.Windows.Forms.Label();
            LabelSize = new System.Windows.Forms.Label();
            label6 = new System.Windows.Forms.Label();
            LabelDimensions = new System.Windows.Forms.Label();
            LabelFormat = new System.Windows.Forms.Label();
            LabelImageFormat = new System.Windows.Forms.Label();
            PageNameTextBox = new System.Windows.Forms.TextBox();
            label2 = new System.Windows.Forms.Label();
            label9 = new System.Windows.Forms.Label();
            LabelDpi = new System.Windows.Forms.Label();
            label8 = new System.Windows.Forms.Label();
            LabelBits = new System.Windows.Forms.Label();
            label7 = new System.Windows.Forms.Label();
            LabelImageColors = new System.Windows.Forms.Label();
            label10 = new System.Windows.Forms.Label();
            KeyEditorPanel = new System.Windows.Forms.Panel();
            textBoxKey = new System.Windows.Forms.TextBox();
            ButtonNewKey = new System.Windows.Forms.Button();
            label11 = new System.Windows.Forms.Label();
            CheckBoxDoublePage = new System.Windows.Forms.CheckBox();
            label4 = new System.Windows.Forms.Label();
            CheckBoxPageDeleted = new System.Windows.Forms.CheckBox();
            label1 = new System.Windows.Forms.Label();
            PageIndexTextbox = new System.Windows.Forms.TextBox();
            label13 = new System.Windows.Forms.Label();
            ComboBoxPageType = new System.Windows.Forms.ComboBox();
            label14 = new System.Windows.Forms.Label();
            IsCompressedLabel = new System.Windows.Forms.Label();
            pictureBox2 = new System.Windows.Forms.PictureBox();
            ButtonReloadImage = new System.Windows.Forms.Button();
            TextBoxFileLocation = new System.Windows.Forms.TextBox();
            tabPage2 = new System.Windows.Forms.TabPage();
            metaDataView = new System.Windows.Forms.WebBrowser();
            Tooltip = new System.Windows.Forms.ToolTip(components);
            SettingsTablePanel.SuspendLayout();
            ImagePreviewTableLayout.SuspendLayout();
            flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)InfoIconTooltip).BeginInit();
            HeaderPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)PreviewThumbPictureBox).BeginInit();
            TabControlPageProperties.SuspendLayout();
            tabPage1.SuspendLayout();
            panel1.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            KeyEditorPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            tabPage2.SuspendLayout();
            SuspendLayout();
            // 
            // SettingsTablePanel
            // 
            SettingsTablePanel.ColumnCount = 3;
            SettingsTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 250F));
            SettingsTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            SettingsTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 133F));
            SettingsTablePanel.Controls.Add(ImagePreviewTableLayout, 0, 1);
            SettingsTablePanel.Controls.Add(HeaderPanel, 0, 0);
            SettingsTablePanel.Controls.Add(ButtonOk, 1, 2);
            SettingsTablePanel.Controls.Add(ButtonCancel, 2, 2);
            SettingsTablePanel.Controls.Add(PreviewThumbPictureBox, 0, 2);
            SettingsTablePanel.Controls.Add(TabControlPageProperties, 1, 1);
            SettingsTablePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            SettingsTablePanel.Location = new System.Drawing.Point(0, 0);
            SettingsTablePanel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            SettingsTablePanel.Name = "SettingsTablePanel";
            SettingsTablePanel.RowCount = 3;
            SettingsTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            SettingsTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            SettingsTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 74F));
            SettingsTablePanel.Size = new System.Drawing.Size(744, 694);
            SettingsTablePanel.TabIndex = 1;
            // 
            // ImagePreviewTableLayout
            // 
            ImagePreviewTableLayout.ColumnCount = 1;
            ImagePreviewTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            ImagePreviewTableLayout.Controls.Add(ImagePreviewButton, 0, 0);
            ImagePreviewTableLayout.Controls.Add(flowLayoutPanel1, 0, 1);
            ImagePreviewTableLayout.Controls.Add(ProgressBarReload, 0, 2);
            ImagePreviewTableLayout.Location = new System.Drawing.Point(15, 102);
            ImagePreviewTableLayout.Margin = new System.Windows.Forms.Padding(15, 2, 3, 2);
            ImagePreviewTableLayout.Name = "ImagePreviewTableLayout";
            ImagePreviewTableLayout.RowCount = 3;
            ImagePreviewTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 74.59016F));
            ImagePreviewTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.40984F));
            ImagePreviewTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            ImagePreviewTableLayout.Size = new System.Drawing.Size(231, 458);
            ImagePreviewTableLayout.TabIndex = 2;
            // 
            // ImagePreviewButton
            // 
            ImagePreviewButton.BackgroundImage = Properties.Resources.placeholder_image;
            ImagePreviewButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            ImagePreviewButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            ImagePreviewButton.Location = new System.Drawing.Point(3, 2);
            ImagePreviewButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            ImagePreviewButton.Name = "ImagePreviewButton";
            ImagePreviewButton.Size = new System.Drawing.Size(225, 318);
            ImagePreviewButton.TabIndex = 7;
            ImagePreviewButton.UseVisualStyleBackColor = true;
            ImagePreviewButton.Click += ImagePreviewButton_Click;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Controls.Add(InfoIconTooltip);
            flowLayoutPanel1.Controls.Add(label12);
            flowLayoutPanel1.Location = new System.Drawing.Point(3, 324);
            flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new System.Drawing.Size(225, 104);
            flowLayoutPanel1.TabIndex = 8;
            // 
            // InfoIconTooltip
            // 
            InfoIconTooltip.Image = Properties.Resources.information;
            InfoIconTooltip.InitialImage = Properties.Resources.information;
            InfoIconTooltip.Location = new System.Drawing.Point(0, 0);
            InfoIconTooltip.Margin = new System.Windows.Forms.Padding(0);
            InfoIconTooltip.Name = "InfoIconTooltip";
            InfoIconTooltip.Size = new System.Drawing.Size(36, 45);
            InfoIconTooltip.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            InfoIconTooltip.TabIndex = 7;
            InfoIconTooltip.TabStop = false;
            // 
            // label12
            // 
            label12.Location = new System.Drawing.Point(39, 0);
            label12.Name = "label12";
            label12.Size = new System.Drawing.Size(149, 40);
            label12.TabIndex = 0;
            label12.Text = "Click on the Image for enlarged Preview";
            // 
            // ProgressBarReload
            // 
            ProgressBarReload.Location = new System.Drawing.Point(3, 436);
            ProgressBarReload.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            ProgressBarReload.Name = "ProgressBarReload";
            ProgressBarReload.Size = new System.Drawing.Size(225, 18);
            ProgressBarReload.TabIndex = 9;
            ProgressBarReload.Visible = false;
            // 
            // HeaderPanel
            // 
            HeaderPanel.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            HeaderPanel.BackColor = System.Drawing.Color.White;
            SettingsTablePanel.SetColumnSpan(HeaderPanel, 3);
            HeaderPanel.Controls.Add(HeaderLabel);
            HeaderPanel.Controls.Add(pictureBox1);
            HeaderPanel.Location = new System.Drawing.Point(3, 2);
            HeaderPanel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            HeaderPanel.Name = "HeaderPanel";
            HeaderPanel.Size = new System.Drawing.Size(738, 95);
            HeaderPanel.TabIndex = 0;
            // 
            // HeaderLabel
            // 
            HeaderLabel.AutoSize = true;
            HeaderLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            HeaderLabel.Location = new System.Drawing.Point(112, 29);
            HeaderLabel.Name = "HeaderLabel";
            HeaderLabel.Size = new System.Drawing.Size(148, 28);
            HeaderLabel.TabIndex = 1;
            HeaderLabel.Text = "Page Properties";
            // 
            // pictureBox1
            // 
            pictureBox1.Image = Properties.Resources.edit_large;
            pictureBox1.Location = new System.Drawing.Point(24, 9);
            pictureBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new System.Drawing.Size(61, 69);
            pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // ButtonOk
            // 
            ButtonOk.Anchor = System.Windows.Forms.AnchorStyles.Right;
            ButtonOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            ButtonOk.Enabled = false;
            ButtonOk.Location = new System.Drawing.Point(497, 636);
            ButtonOk.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            ButtonOk.Name = "ButtonOk";
            ButtonOk.Size = new System.Drawing.Size(111, 41);
            ButtonOk.TabIndex = 2;
            ButtonOk.Text = "Ok";
            ButtonOk.UseVisualStyleBackColor = true;
            ButtonOk.Click += ButtonOk_Click;
            // 
            // ButtonCancel
            // 
            ButtonCancel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            ButtonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            ButtonCancel.Location = new System.Drawing.Point(614, 636);
            ButtonCancel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            ButtonCancel.Name = "ButtonCancel";
            ButtonCancel.Size = new System.Drawing.Size(119, 41);
            ButtonCancel.TabIndex = 3;
            ButtonCancel.Text = "Cancel";
            ButtonCancel.UseVisualStyleBackColor = true;
            ButtonCancel.Click += ButtonCancel_Click;
            // 
            // PreviewThumbPictureBox
            // 
            PreviewThumbPictureBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            PreviewThumbPictureBox.ErrorImage = Properties.Resources.placeholder_image;
            PreviewThumbPictureBox.InitialImage = Properties.Resources.placeholder_image;
            PreviewThumbPictureBox.Location = new System.Drawing.Point(3, 622);
            PreviewThumbPictureBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            PreviewThumbPictureBox.Name = "PreviewThumbPictureBox";
            PreviewThumbPictureBox.Size = new System.Drawing.Size(244, 55);
            PreviewThumbPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            PreviewThumbPictureBox.TabIndex = 5;
            PreviewThumbPictureBox.TabStop = false;
            PreviewThumbPictureBox.Visible = false;
            // 
            // TabControlPageProperties
            // 
            SettingsTablePanel.SetColumnSpan(TabControlPageProperties, 2);
            TabControlPageProperties.Controls.Add(tabPage1);
            TabControlPageProperties.Controls.Add(tabPage2);
            TabControlPageProperties.Dock = System.Windows.Forms.DockStyle.Fill;
            TabControlPageProperties.Location = new System.Drawing.Point(253, 102);
            TabControlPageProperties.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            TabControlPageProperties.Name = "TabControlPageProperties";
            TabControlPageProperties.SelectedIndex = 0;
            TabControlPageProperties.Size = new System.Drawing.Size(488, 516);
            TabControlPageProperties.TabIndex = 6;
            TabControlPageProperties.SelectedIndexChanged += TabControlPageProperties_SelectedIndexChanged;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(panel1);
            tabPage1.Location = new System.Drawing.Point(4, 29);
            tabPage1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            tabPage1.Size = new System.Drawing.Size(480, 483);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Properties";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            panel1.AutoScroll = true;
            panel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            panel1.Controls.Add(LoadingIndicator);
            panel1.Controls.Add(tableLayoutPanel1);
            panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            panel1.Location = new System.Drawing.Point(3, 2);
            panel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            panel1.Name = "panel1";
            panel1.Padding = new System.Windows.Forms.Padding(4);
            panel1.Size = new System.Drawing.Size(474, 479);
            panel1.TabIndex = 0;
            // 
            // LoadingIndicator
            // 
            LoadingIndicator.AnimationInterval = System.TimeSpan.Parse("00:00:00.2000000");
            LoadingIndicator.BoxColor = System.Drawing.Color.FromArgb(162, 199, 214);
            LoadingIndicator.BoxSize = 12;
            LoadingIndicator.HighlightedBoxColor = System.Drawing.Color.FromArgb(67, 143, 174);
            LoadingIndicator.Location = new System.Drawing.Point(132, 71);
            LoadingIndicator.Margin = new System.Windows.Forms.Padding(2);
            LoadingIndicator.Name = "LoadingIndicator";
            LoadingIndicator.NumberOfBoxes = 3;
            LoadingIndicator.RoundCornerRadius = 3;
            LoadingIndicator.Size = new System.Drawing.Size(66, 32);
            LoadingIndicator.TabIndex = 9;
            LoadingIndicator.Text = "boxIndicatorControl1";
            LoadingIndicator.Visible = false;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.AutoScroll = true;
            tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            tableLayoutPanel1.ColumnCount = 4;
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 41F));
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 65F));
            tableLayoutPanel1.Controls.Add(label3, 0, 0);
            tableLayoutPanel1.Controls.Add(label5, 0, 2);
            tableLayoutPanel1.Controls.Add(LabelSize, 1, 2);
            tableLayoutPanel1.Controls.Add(label6, 0, 3);
            tableLayoutPanel1.Controls.Add(LabelDimensions, 1, 3);
            tableLayoutPanel1.Controls.Add(LabelFormat, 0, 1);
            tableLayoutPanel1.Controls.Add(LabelImageFormat, 1, 1);
            tableLayoutPanel1.Controls.Add(PageNameTextBox, 1, 5);
            tableLayoutPanel1.Controls.Add(label2, 0, 5);
            tableLayoutPanel1.Controls.Add(label9, 0, 4);
            tableLayoutPanel1.Controls.Add(LabelDpi, 1, 4);
            tableLayoutPanel1.Controls.Add(label8, 2, 1);
            tableLayoutPanel1.Controls.Add(LabelBits, 3, 1);
            tableLayoutPanel1.Controls.Add(label7, 2, 2);
            tableLayoutPanel1.Controls.Add(LabelImageColors, 3, 2);
            tableLayoutPanel1.Controls.Add(label10, 0, 6);
            tableLayoutPanel1.Controls.Add(KeyEditorPanel, 1, 6);
            tableLayoutPanel1.Controls.Add(label11, 0, 10);
            tableLayoutPanel1.Controls.Add(CheckBoxDoublePage, 1, 10);
            tableLayoutPanel1.Controls.Add(label4, 0, 9);
            tableLayoutPanel1.Controls.Add(CheckBoxPageDeleted, 1, 9);
            tableLayoutPanel1.Controls.Add(label1, 0, 8);
            tableLayoutPanel1.Controls.Add(PageIndexTextbox, 1, 8);
            tableLayoutPanel1.Controls.Add(label13, 0, 7);
            tableLayoutPanel1.Controls.Add(ComboBoxPageType, 1, 7);
            tableLayoutPanel1.Controls.Add(label14, 0, 11);
            tableLayoutPanel1.Controls.Add(IsCompressedLabel, 1, 11);
            tableLayoutPanel1.Controls.Add(pictureBox2, 2, 6);
            tableLayoutPanel1.Controls.Add(ButtonReloadImage, 3, 0);
            tableLayoutPanel1.Controls.Add(TextBoxFileLocation, 1, 0);
            tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanel1.Location = new System.Drawing.Point(4, 4);
            tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.Padding = new System.Windows.Forms.Padding(0, 8, 10, 0);
            tableLayoutPanel1.RowCount = 12;
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            tableLayoutPanel1.Size = new System.Drawing.Size(466, 471);
            tableLayoutPanel1.TabIndex = 2;
            // 
            // label3
            // 
            label3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            label3.AutoSize = true;
            label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            label3.Location = new System.Drawing.Point(47, 8);
            label3.Name = "label3";
            label3.Padding = new System.Windows.Forms.Padding(0, 8, 0, 0);
            label3.Size = new System.Drawing.Size(70, 25);
            label3.TabIndex = 4;
            label3.Text = "Location";
            label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label5
            // 
            label5.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            label5.AutoSize = true;
            label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            label5.Location = new System.Drawing.Point(78, 104);
            label5.Name = "label5";
            label5.Padding = new System.Windows.Forms.Padding(0, 9, 0, 0);
            label5.Size = new System.Drawing.Size(39, 26);
            label5.TabIndex = 8;
            label5.Text = "Size";
            label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // LabelSize
            // 
            LabelSize.AutoSize = true;
            LabelSize.Location = new System.Drawing.Point(123, 104);
            LabelSize.Name = "LabelSize";
            LabelSize.Padding = new System.Windows.Forms.Padding(0, 9, 0, 0);
            LabelSize.Size = new System.Drawing.Size(56, 29);
            LabelSize.TabIndex = 9;
            LabelSize.Text = "0 Bytes";
            LabelSize.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label6
            // 
            label6.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            label6.AutoSize = true;
            label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            label6.Location = new System.Drawing.Point(25, 152);
            label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label6.Name = "label6";
            label6.Padding = new System.Windows.Forms.Padding(0, 9, 0, 0);
            label6.Size = new System.Drawing.Size(91, 26);
            label6.TabIndex = 10;
            label6.Text = "Dimensions";
            label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // LabelDimensions
            // 
            LabelDimensions.AutoSize = true;
            LabelDimensions.Location = new System.Drawing.Point(124, 152);
            LabelDimensions.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            LabelDimensions.Name = "LabelDimensions";
            LabelDimensions.Padding = new System.Windows.Forms.Padding(0, 9, 0, 0);
            LabelDimensions.Size = new System.Drawing.Size(75, 29);
            LabelDimensions.TabIndex = 11;
            LabelDimensions.Text = "0 x 0 Pixel";
            // 
            // LabelFormat
            // 
            LabelFormat.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            LabelFormat.AutoSize = true;
            LabelFormat.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            LabelFormat.Location = new System.Drawing.Point(58, 56);
            LabelFormat.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            LabelFormat.Name = "LabelFormat";
            LabelFormat.Padding = new System.Windows.Forms.Padding(0, 9, 0, 0);
            LabelFormat.Size = new System.Drawing.Size(58, 26);
            LabelFormat.TabIndex = 12;
            LabelFormat.Text = "Format";
            LabelFormat.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // LabelImageFormat
            // 
            LabelImageFormat.AutoSize = true;
            LabelImageFormat.Location = new System.Drawing.Point(124, 56);
            LabelImageFormat.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            LabelImageFormat.Name = "LabelImageFormat";
            LabelImageFormat.Padding = new System.Windows.Forms.Padding(0, 9, 0, 0);
            LabelImageFormat.Size = new System.Drawing.Size(68, 29);
            LabelImageFormat.TabIndex = 15;
            LabelImageFormat.Text = "unknown";
            LabelImageFormat.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // PageNameTextBox
            // 
            PageNameTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            PageNameTextBox.Location = new System.Drawing.Point(125, 253);
            PageNameTextBox.Margin = new System.Windows.Forms.Padding(5, 5, 3, 2);
            PageNameTextBox.Name = "PageNameTextBox";
            PageNameTextBox.Size = new System.Drawing.Size(201, 27);
            PageNameTextBox.TabIndex = 2;
            PageNameTextBox.TextChanged += PageNameTextBox_TextChanged;
            // 
            // label2
            // 
            label2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            label2.AutoSize = true;
            label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            label2.Location = new System.Drawing.Point(68, 248);
            label2.Name = "label2";
            label2.Padding = new System.Windows.Forms.Padding(0, 8, 0, 0);
            label2.Size = new System.Drawing.Size(49, 25);
            label2.TabIndex = 3;
            label2.Text = "Name";
            label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label9
            // 
            label9.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            label9.AutoSize = true;
            label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            label9.Location = new System.Drawing.Point(83, 200);
            label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label9.Name = "label9";
            label9.Padding = new System.Windows.Forms.Padding(0, 9, 0, 0);
            label9.Size = new System.Drawing.Size(33, 26);
            label9.TabIndex = 14;
            label9.Text = "DPI";
            label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // LabelDpi
            // 
            LabelDpi.AutoSize = true;
            LabelDpi.Location = new System.Drawing.Point(124, 200);
            LabelDpi.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            LabelDpi.Name = "LabelDpi";
            LabelDpi.Padding = new System.Windows.Forms.Padding(0, 9, 0, 0);
            LabelDpi.Size = new System.Drawing.Size(17, 29);
            LabelDpi.TabIndex = 17;
            LabelDpi.Text = "0";
            LabelDpi.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label8
            // 
            label8.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            label8.AutoSize = true;
            label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            label8.Location = new System.Drawing.Point(339, 56);
            label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label8.Name = "label8";
            label8.Padding = new System.Windows.Forms.Padding(0, 9, 0, 0);
            label8.Size = new System.Drawing.Size(27, 43);
            label8.TabIndex = 13;
            label8.Text = "Bits";
            label8.Visible = false;
            // 
            // LabelBits
            // 
            LabelBits.AutoSize = true;
            LabelBits.Location = new System.Drawing.Point(374, 56);
            LabelBits.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            LabelBits.Name = "LabelBits";
            LabelBits.Padding = new System.Windows.Forms.Padding(0, 9, 0, 0);
            LabelBits.Size = new System.Drawing.Size(17, 29);
            LabelBits.TabIndex = 16;
            LabelBits.Text = "0";
            LabelBits.Visible = false;
            // 
            // label7
            // 
            label7.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            label7.AutoSize = true;
            label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            label7.Location = new System.Drawing.Point(335, 104);
            label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label7.Name = "label7";
            label7.Padding = new System.Windows.Forms.Padding(0, 9, 0, 0);
            label7.Size = new System.Drawing.Size(31, 43);
            label7.TabIndex = 18;
            label7.Text = "Colors";
            label7.Visible = false;
            // 
            // LabelImageColors
            // 
            LabelImageColors.AutoSize = true;
            LabelImageColors.Location = new System.Drawing.Point(374, 104);
            LabelImageColors.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            LabelImageColors.Name = "LabelImageColors";
            LabelImageColors.Padding = new System.Windows.Forms.Padding(0, 9, 0, 0);
            LabelImageColors.Size = new System.Drawing.Size(17, 29);
            LabelImageColors.TabIndex = 19;
            LabelImageColors.Text = "0";
            LabelImageColors.Visible = false;
            // 
            // label10
            // 
            label10.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            label10.AutoSize = true;
            label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            label10.Location = new System.Drawing.Point(84, 296);
            label10.Name = "label10";
            label10.Padding = new System.Windows.Forms.Padding(0, 8, 0, 0);
            label10.Size = new System.Drawing.Size(33, 24);
            label10.TabIndex = 20;
            label10.Text = "Key";
            label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // KeyEditorPanel
            // 
            KeyEditorPanel.BackColor = System.Drawing.SystemColors.ControlLightLight;
            KeyEditorPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            KeyEditorPanel.Controls.Add(textBoxKey);
            KeyEditorPanel.Controls.Add(ButtonNewKey);
            KeyEditorPanel.Dock = System.Windows.Forms.DockStyle.Top;
            KeyEditorPanel.Location = new System.Drawing.Point(125, 301);
            KeyEditorPanel.Margin = new System.Windows.Forms.Padding(5, 5, 3, 2);
            KeyEditorPanel.Name = "KeyEditorPanel";
            KeyEditorPanel.Size = new System.Drawing.Size(201, 27);
            KeyEditorPanel.TabIndex = 25;
            // 
            // textBoxKey
            // 
            textBoxKey.BorderStyle = System.Windows.Forms.BorderStyle.None;
            textBoxKey.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxKey.Location = new System.Drawing.Point(0, 0);
            textBoxKey.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            textBoxKey.Name = "textBoxKey";
            textBoxKey.Size = new System.Drawing.Size(174, 20);
            textBoxKey.TabIndex = 22;
            textBoxKey.TextChanged += textBoxKey_TextChanged;
            // 
            // ButtonNewKey
            // 
            ButtonNewKey.Dock = System.Windows.Forms.DockStyle.Right;
            ButtonNewKey.Enabled = false;
            ButtonNewKey.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            ButtonNewKey.Image = Properties.Resources.arrow_circle2;
            ButtonNewKey.Location = new System.Drawing.Point(174, 0);
            ButtonNewKey.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            ButtonNewKey.Name = "ButtonNewKey";
            ButtonNewKey.Size = new System.Drawing.Size(25, 25);
            ButtonNewKey.TabIndex = 22;
            ButtonNewKey.Text = "...";
            ButtonNewKey.UseVisualStyleBackColor = true;
            ButtonNewKey.Click += ButtonNewKey_Click;
            // 
            // label11
            // 
            label11.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            label11.AutoSize = true;
            label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            label11.Location = new System.Drawing.Point(14, 488);
            label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label11.Name = "label11";
            label11.Padding = new System.Windows.Forms.Padding(0, 8, 0, 0);
            label11.Size = new System.Drawing.Size(102, 25);
            label11.TabIndex = 23;
            label11.Text = "Double-Page";
            label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // CheckBoxDoublePage
            // 
            CheckBoxDoublePage.AutoSize = true;
            CheckBoxDoublePage.Location = new System.Drawing.Point(124, 493);
            CheckBoxDoublePage.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            CheckBoxDoublePage.Name = "CheckBoxDoublePage";
            CheckBoxDoublePage.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            CheckBoxDoublePage.Size = new System.Drawing.Size(18, 22);
            CheckBoxDoublePage.TabIndex = 24;
            CheckBoxDoublePage.UseVisualStyleBackColor = true;
            CheckBoxDoublePage.CheckedChanged += CheckBoxDoublePage_CheckedChanged;
            // 
            // label4
            // 
            label4.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            label4.AutoSize = true;
            label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            label4.Location = new System.Drawing.Point(52, 440);
            label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Padding = new System.Windows.Forms.Padding(0, 8, 0, 0);
            label4.Size = new System.Drawing.Size(64, 25);
            label4.TabIndex = 6;
            label4.Text = "Deleted";
            label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // CheckBoxPageDeleted
            // 
            CheckBoxPageDeleted.AutoSize = true;
            CheckBoxPageDeleted.Location = new System.Drawing.Point(124, 445);
            CheckBoxPageDeleted.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            CheckBoxPageDeleted.Name = "CheckBoxPageDeleted";
            CheckBoxPageDeleted.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            CheckBoxPageDeleted.Size = new System.Drawing.Size(18, 22);
            CheckBoxPageDeleted.TabIndex = 7;
            CheckBoxPageDeleted.UseVisualStyleBackColor = true;
            CheckBoxPageDeleted.CheckedChanged += CheckBoxPageDeleted_CheckedChanged;
            // 
            // label1
            // 
            label1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            label1.AutoSize = true;
            label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            label1.Location = new System.Drawing.Point(71, 392);
            label1.Name = "label1";
            label1.Padding = new System.Windows.Forms.Padding(0, 8, 0, 0);
            label1.Size = new System.Drawing.Size(46, 25);
            label1.TabIndex = 1;
            label1.Text = "Index";
            label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PageIndexTextbox
            // 
            PageIndexTextbox.Location = new System.Drawing.Point(125, 397);
            PageIndexTextbox.Margin = new System.Windows.Forms.Padding(5, 5, 3, 2);
            PageIndexTextbox.Name = "PageIndexTextbox";
            PageIndexTextbox.Size = new System.Drawing.Size(71, 27);
            PageIndexTextbox.TabIndex = 0;
            PageIndexTextbox.TextChanged += PageIndexTextbox_TextChanged;
            // 
            // label13
            // 
            label13.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            label13.AutoSize = true;
            label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            label13.Location = new System.Drawing.Point(73, 344);
            label13.Name = "label13";
            label13.Padding = new System.Windows.Forms.Padding(0, 8, 0, 0);
            label13.Size = new System.Drawing.Size(44, 25);
            label13.TabIndex = 26;
            label13.Text = "Type";
            label13.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ComboBoxPageType
            // 
            ComboBoxPageType.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            ComboBoxPageType.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            ComboBoxPageType.Dock = System.Windows.Forms.DockStyle.Top;
            ComboBoxPageType.FormattingEnabled = true;
            ComboBoxPageType.Location = new System.Drawing.Point(123, 349);
            ComboBoxPageType.Margin = new System.Windows.Forms.Padding(3, 5, 3, 4);
            ComboBoxPageType.Name = "ComboBoxPageType";
            ComboBoxPageType.Size = new System.Drawing.Size(203, 28);
            ComboBoxPageType.TabIndex = 27;
            ComboBoxPageType.TextUpdate += ComboBoxPageType_TextUpdate;
            ComboBoxPageType.TextChanged += ComboBoxPageType_TextChanged;
            // 
            // label14
            // 
            label14.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            label14.AutoSize = true;
            label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            label14.Location = new System.Drawing.Point(19, 536);
            label14.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label14.Name = "label14";
            label14.Padding = new System.Windows.Forms.Padding(0, 8, 0, 0);
            label14.Size = new System.Drawing.Size(97, 25);
            label14.TabIndex = 28;
            label14.Text = "Compressed";
            label14.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // IsCompressedLabel
            // 
            IsCompressedLabel.AutoSize = true;
            IsCompressedLabel.Location = new System.Drawing.Point(123, 536);
            IsCompressedLabel.Name = "IsCompressedLabel";
            IsCompressedLabel.Padding = new System.Windows.Forms.Padding(0, 8, 0, 0);
            IsCompressedLabel.Size = new System.Drawing.Size(29, 28);
            IsCompressedLabel.TabIndex = 29;
            IsCompressedLabel.Text = "No";
            IsCompressedLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pictureBox2
            // 
            pictureBox2.Image = Properties.Resources.information;
            pictureBox2.InitialImage = Properties.Resources.information;
            pictureBox2.Location = new System.Drawing.Point(333, 296);
            pictureBox2.Margin = new System.Windows.Forms.Padding(4, 0, 0, 0);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            pictureBox2.Size = new System.Drawing.Size(23, 30);
            pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            pictureBox2.TabIndex = 30;
            pictureBox2.TabStop = false;
            Tooltip.SetToolTip(pictureBox2, "This property is only available in Meta- Format VERSION_2");
            // 
            // ButtonReloadImage
            // 
            ButtonReloadImage.Image = Properties.Resources.arrow_circle2;
            ButtonReloadImage.Location = new System.Drawing.Point(373, 12);
            ButtonReloadImage.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            ButtonReloadImage.Name = "ButtonReloadImage";
            ButtonReloadImage.Size = new System.Drawing.Size(32, 29);
            ButtonReloadImage.TabIndex = 31;
            Tooltip.SetToolTip(ButtonReloadImage, "Reload image from disk");
            ButtonReloadImage.UseVisualStyleBackColor = true;
            ButtonReloadImage.Click += ButtonReloadImage_Click;
            // 
            // TextBoxFileLocation
            // 
            TextBoxFileLocation.BackColor = System.Drawing.SystemColors.Control;
            tableLayoutPanel1.SetColumnSpan(TextBoxFileLocation, 2);
            TextBoxFileLocation.Dock = System.Windows.Forms.DockStyle.Top;
            TextBoxFileLocation.Location = new System.Drawing.Point(123, 13);
            TextBoxFileLocation.Margin = new System.Windows.Forms.Padding(3, 5, 3, 2);
            TextBoxFileLocation.Name = "TextBoxFileLocation";
            TextBoxFileLocation.ReadOnly = true;
            TextBoxFileLocation.Size = new System.Drawing.Size(244, 27);
            TextBoxFileLocation.TabIndex = 5;
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(metaDataView);
            tabPage2.Location = new System.Drawing.Point(4, 29);
            tabPage2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            tabPage2.Size = new System.Drawing.Size(480, 483);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "Page XML";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // metaDataView
            // 
            metaDataView.AllowNavigation = false;
            metaDataView.AllowWebBrowserDrop = false;
            metaDataView.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            metaDataView.Location = new System.Drawing.Point(3, 2);
            metaDataView.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            metaDataView.MinimumSize = new System.Drawing.Size(20, 25);
            metaDataView.Name = "metaDataView";
            metaDataView.ScriptErrorsSuppressed = true;
            metaDataView.Size = new System.Drawing.Size(451, 444);
            metaDataView.TabIndex = 4;
            metaDataView.WebBrowserShortcutsEnabled = false;
            // 
            // Tooltip
            // 
            Tooltip.AutoPopDelay = 30000;
            Tooltip.InitialDelay = 200;
            Tooltip.IsBalloon = true;
            Tooltip.ReshowDelay = 100;
            Tooltip.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            Tooltip.ToolTipTitle = "Win_CBZ";
            // 
            // PageSettingsForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            CancelButton = ButtonCancel;
            ClientSize = new System.Drawing.Size(744, 694);
            Controls.Add(SettingsTablePanel);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            KeyPreview = true;
            Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            Name = "PageSettingsForm";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Page Properties";
            FormClosing += PageSettingsForm_FormClosing;
            Load += PageSettingsForm_Load_1;
            Shown += PageSettingsForm_Shown;
            KeyUp += PageSettingsForm_KeyUp;
            SettingsTablePanel.ResumeLayout(false);
            ImagePreviewTableLayout.ResumeLayout(false);
            flowLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)InfoIconTooltip).EndInit();
            HeaderPanel.ResumeLayout(false);
            HeaderPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)PreviewThumbPictureBox).EndInit();
            TabControlPageProperties.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            panel1.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            KeyEditorPanel.ResumeLayout(false);
            KeyEditorPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            tabPage2.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel SettingsTablePanel;
        private System.Windows.Forms.Panel HeaderPanel;
        private System.Windows.Forms.Label HeaderLabel;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button ButtonOk;
        private System.Windows.Forms.Button ButtonCancel;
        private System.Windows.Forms.PictureBox PreviewThumbPictureBox;
        private System.Windows.Forms.TableLayoutPanel ImagePreviewTableLayout;
        private System.Windows.Forms.Button ImagePreviewButton;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.PictureBox InfoIconTooltip;
        private System.Windows.Forms.TabControl TabControlPageProperties;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox TextBoxFileLocation;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label LabelSize;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label LabelDimensions;
        private System.Windows.Forms.Label LabelFormat;
        private System.Windows.Forms.Label LabelImageFormat;
        private System.Windows.Forms.TextBox PageNameTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label LabelDpi;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label LabelBits;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label LabelImageColors;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox CheckBoxPageDeleted;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox PageIndexTextbox;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.CheckBox CheckBoxDoublePage;
        private System.Windows.Forms.Panel KeyEditorPanel;
        private System.Windows.Forms.TextBox textBoxKey;
        private System.Windows.Forms.Button ButtonNewKey;
        private System.Windows.Forms.WebBrowser metaDataView;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.ComboBox ComboBoxPageType;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label IsCompressedLabel;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.ToolTip Tooltip;
        private System.Windows.Forms.Button ButtonReloadImage;
        private System.Windows.Forms.ProgressBar ProgressBarReload;
        private LoadingIndicator.WinForms.BoxIndicatorControl LoadingIndicator;
    }
}