namespace Win_CBZ.Forms
{
    partial class ColorSelect
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ColorSelect));
            ColorEditorTableLayout = new System.Windows.Forms.TableLayoutPanel();
            HeaderPanel = new System.Windows.Forms.Panel();
            HeaderLabel = new System.Windows.Forms.Label();
            HeaderPicture = new System.Windows.Forms.PictureBox();
            CancelBtn = new System.Windows.Forms.Button();
            OkButton = new System.Windows.Forms.Button();
            tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            PalettesTabControl = new System.Windows.Forms.TabControl();
            TabPageDefaultPalette = new System.Windows.Forms.TabPage();
            FlowLayoutDefaultPalette = new System.Windows.Forms.FlowLayoutPanel();
            TabPageSystemPalette = new System.Windows.Forms.TabPage();
            FlowLayoutSystemPalette = new System.Windows.Forms.FlowLayoutPanel();
            TabPageCustomPalette = new System.Windows.Forms.TabPage();
            tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            PictureBoxPalette = new System.Windows.Forms.PictureBox();
            PictureBoxSelectedColor = new System.Windows.Forms.PictureBox();
            PictureBoxHoverColor = new System.Windows.Forms.PictureBox();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            PictureBoxRainbow = new System.Windows.Forms.PictureBox();
            tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            TextBoxHex = new System.Windows.Forms.TextBox();
            label6 = new System.Windows.Forms.Label();
            label7 = new System.Windows.Forms.Label();
            label8 = new System.Windows.Forms.Label();
            label9 = new System.Windows.Forms.Label();
            TextBoxH = new System.Windows.Forms.TextBox();
            TextBoxS = new System.Windows.Forms.TextBox();
            TextBoxV = new System.Windows.Forms.TextBox();
            tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            label3 = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            label5 = new System.Windows.Forms.Label();
            TextBoxR = new System.Windows.Forms.TextBox();
            TextBoxB = new System.Windows.Forms.TextBox();
            TextBoxG = new System.Windows.Forms.TextBox();
            GradientSliderChannelR = new Win_CBZ.Components.GradientSlider.GradientSlider();
            GradientSliderChannelB = new Win_CBZ.Components.GradientSlider.GradientSlider();
            GradientSliderChannelG = new Win_CBZ.Components.GradientSlider.GradientSlider();
            toolStrip1 = new System.Windows.Forms.ToolStrip();
            toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            toolStripButton4 = new System.Windows.Forms.ToolStripButton();
            ColorSelectTooltip = new System.Windows.Forms.ToolTip(components);
            ColorEditorTableLayout.SuspendLayout();
            HeaderPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)HeaderPicture).BeginInit();
            tableLayoutPanel1.SuspendLayout();
            PalettesTabControl.SuspendLayout();
            TabPageDefaultPalette.SuspendLayout();
            TabPageSystemPalette.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)PictureBoxPalette).BeginInit();
            ((System.ComponentModel.ISupportInitialize)PictureBoxSelectedColor).BeginInit();
            ((System.ComponentModel.ISupportInitialize)PictureBoxHoverColor).BeginInit();
            ((System.ComponentModel.ISupportInitialize)PictureBoxRainbow).BeginInit();
            tableLayoutPanel3.SuspendLayout();
            tableLayoutPanel4.SuspendLayout();
            toolStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // ColorEditorTableLayout
            // 
            ColorEditorTableLayout.ColumnCount = 3;
            ColorEditorTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            ColorEditorTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 249F));
            ColorEditorTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 119F));
            ColorEditorTableLayout.Controls.Add(HeaderPanel, 0, 0);
            ColorEditorTableLayout.Controls.Add(CancelBtn, 2, 2);
            ColorEditorTableLayout.Controls.Add(OkButton, 1, 2);
            ColorEditorTableLayout.Controls.Add(tableLayoutPanel1, 0, 1);
            ColorEditorTableLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            ColorEditorTableLayout.Location = new System.Drawing.Point(0, 0);
            ColorEditorTableLayout.Margin = new System.Windows.Forms.Padding(4, 0, 4, 5);
            ColorEditorTableLayout.Name = "ColorEditorTableLayout";
            ColorEditorTableLayout.RowCount = 3;
            ColorEditorTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 63F));
            ColorEditorTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            ColorEditorTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 54F));
            ColorEditorTableLayout.Size = new System.Drawing.Size(855, 513);
            ColorEditorTableLayout.TabIndex = 2;
            // 
            // HeaderPanel
            // 
            HeaderPanel.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            HeaderPanel.BackColor = System.Drawing.Color.White;
            ColorEditorTableLayout.SetColumnSpan(HeaderPanel, 3);
            HeaderPanel.Controls.Add(HeaderLabel);
            HeaderPanel.Controls.Add(HeaderPicture);
            HeaderPanel.Location = new System.Drawing.Point(0, 0);
            HeaderPanel.Margin = new System.Windows.Forms.Padding(0);
            HeaderPanel.Name = "HeaderPanel";
            HeaderPanel.Size = new System.Drawing.Size(855, 63);
            HeaderPanel.TabIndex = 2;
            // 
            // HeaderLabel
            // 
            HeaderLabel.AutoSize = true;
            HeaderLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            HeaderLabel.Location = new System.Drawing.Point(95, 16);
            HeaderLabel.Name = "HeaderLabel";
            HeaderLabel.Size = new System.Drawing.Size(117, 28);
            HeaderLabel.TabIndex = 1;
            HeaderLabel.Text = "Select Color";
            // 
            // HeaderPicture
            // 
            HeaderPicture.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            HeaderPicture.Image = Properties.Resources.painters_palette_brush;
            HeaderPicture.Location = new System.Drawing.Point(24, 0);
            HeaderPicture.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            HeaderPicture.Name = "HeaderPicture";
            HeaderPicture.Size = new System.Drawing.Size(65, 61);
            HeaderPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            HeaderPicture.TabIndex = 0;
            HeaderPicture.TabStop = false;
            // 
            // CancelBtn
            // 
            CancelBtn.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            CancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            CancelBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            CancelBtn.Location = new System.Drawing.Point(752, 464);
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
            OkButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            OkButton.Location = new System.Drawing.Point(632, 464);
            OkButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            OkButton.Name = "OkButton";
            OkButton.Size = new System.Drawing.Size(100, 35);
            OkButton.TabIndex = 3;
            OkButton.Text = "Ok";
            OkButton.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            tableLayoutPanel1.ColumnCount = 2;
            ColorEditorTableLayout.SetColumnSpan(tableLayoutPanel1, 3);
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanel1.Controls.Add(PalettesTabControl, 0, 2);
            tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 1, 1);
            tableLayoutPanel1.Controls.Add(toolStrip1, 0, 1);
            tableLayoutPanel1.Location = new System.Drawing.Point(3, 66);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 17F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 43F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new System.Drawing.Size(849, 390);
            tableLayoutPanel1.TabIndex = 5;
            // 
            // PalettesTabControl
            // 
            PalettesTabControl.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            PalettesTabControl.Controls.Add(TabPageDefaultPalette);
            PalettesTabControl.Controls.Add(TabPageSystemPalette);
            PalettesTabControl.Controls.Add(TabPageCustomPalette);
            PalettesTabControl.Location = new System.Drawing.Point(3, 70);
            PalettesTabControl.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            PalettesTabControl.Name = "PalettesTabControl";
            PalettesTabControl.SelectedIndex = 0;
            PalettesTabControl.Size = new System.Drawing.Size(418, 317);
            PalettesTabControl.TabIndex = 5;
            PalettesTabControl.Selecting += PalettesTabControl_Selecting;
            // 
            // TabPageDefaultPalette
            // 
            TabPageDefaultPalette.Controls.Add(FlowLayoutDefaultPalette);
            TabPageDefaultPalette.Location = new System.Drawing.Point(4, 29);
            TabPageDefaultPalette.Name = "TabPageDefaultPalette";
            TabPageDefaultPalette.Padding = new System.Windows.Forms.Padding(3);
            TabPageDefaultPalette.Size = new System.Drawing.Size(410, 284);
            TabPageDefaultPalette.TabIndex = 0;
            TabPageDefaultPalette.Text = "Default";
            TabPageDefaultPalette.UseVisualStyleBackColor = true;
            // 
            // FlowLayoutDefaultPalette
            // 
            FlowLayoutDefaultPalette.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            FlowLayoutDefaultPalette.AutoScroll = true;
            FlowLayoutDefaultPalette.Location = new System.Drawing.Point(3, 3);
            FlowLayoutDefaultPalette.Name = "FlowLayoutDefaultPalette";
            FlowLayoutDefaultPalette.Padding = new System.Windows.Forms.Padding(8);
            FlowLayoutDefaultPalette.Size = new System.Drawing.Size(405, 280);
            FlowLayoutDefaultPalette.TabIndex = 0;
            // 
            // TabPageSystemPalette
            // 
            TabPageSystemPalette.Controls.Add(FlowLayoutSystemPalette);
            TabPageSystemPalette.Location = new System.Drawing.Point(4, 29);
            TabPageSystemPalette.Name = "TabPageSystemPalette";
            TabPageSystemPalette.Padding = new System.Windows.Forms.Padding(3);
            TabPageSystemPalette.Size = new System.Drawing.Size(410, 284);
            TabPageSystemPalette.TabIndex = 1;
            TabPageSystemPalette.Text = "System";
            TabPageSystemPalette.UseVisualStyleBackColor = true;
            // 
            // FlowLayoutSystemPalette
            // 
            FlowLayoutSystemPalette.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            FlowLayoutSystemPalette.AutoScroll = true;
            FlowLayoutSystemPalette.Location = new System.Drawing.Point(3, 3);
            FlowLayoutSystemPalette.Name = "FlowLayoutSystemPalette";
            FlowLayoutSystemPalette.Padding = new System.Windows.Forms.Padding(8);
            FlowLayoutSystemPalette.Size = new System.Drawing.Size(405, 279);
            FlowLayoutSystemPalette.TabIndex = 1;
            // 
            // TabPageCustomPalette
            // 
            TabPageCustomPalette.Location = new System.Drawing.Point(4, 29);
            TabPageCustomPalette.Name = "TabPageCustomPalette";
            TabPageCustomPalette.Size = new System.Drawing.Size(410, 284);
            TabPageCustomPalette.TabIndex = 3;
            TabPageCustomPalette.Text = "Custom";
            TabPageCustomPalette.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            tableLayoutPanel2.ColumnCount = 3;
            tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 61.32076F));
            tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 38.679245F));
            tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            tableLayoutPanel2.Controls.Add(PictureBoxPalette, 0, 0);
            tableLayoutPanel2.Controls.Add(PictureBoxSelectedColor, 0, 2);
            tableLayoutPanel2.Controls.Add(PictureBoxHoverColor, 1, 2);
            tableLayoutPanel2.Controls.Add(label1, 0, 1);
            tableLayoutPanel2.Controls.Add(label2, 1, 1);
            tableLayoutPanel2.Controls.Add(PictureBoxRainbow, 2, 0);
            tableLayoutPanel2.Controls.Add(tableLayoutPanel3, 0, 4);
            tableLayoutPanel2.Controls.Add(tableLayoutPanel4, 0, 3);
            tableLayoutPanel2.Location = new System.Drawing.Point(427, 20);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 5;
            tableLayoutPanel1.SetRowSpan(tableLayoutPanel2, 2);
            tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 108F));
            tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 71F));
            tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            tableLayoutPanel2.Size = new System.Drawing.Size(419, 367);
            tableLayoutPanel2.TabIndex = 7;
            // 
            // PictureBoxPalette
            // 
            PictureBoxPalette.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            PictureBoxPalette.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            tableLayoutPanel2.SetColumnSpan(PictureBoxPalette, 2);
            PictureBoxPalette.Cursor = System.Windows.Forms.Cursors.Cross;
            PictureBoxPalette.Image = Properties.Resources.palette;
            PictureBoxPalette.Location = new System.Drawing.Point(3, 3);
            PictureBoxPalette.Name = "PictureBoxPalette";
            PictureBoxPalette.Size = new System.Drawing.Size(372, 121);
            PictureBoxPalette.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            PictureBoxPalette.TabIndex = 6;
            PictureBoxPalette.TabStop = false;
            PictureBoxPalette.Click += PictureBoxPalette_Click;
            PictureBoxPalette.MouseMove += PictureBoxPalette_MouseMove;
            PictureBoxPalette.Resize += PictureBoxPalette_Resize;
            // 
            // PictureBoxSelectedColor
            // 
            PictureBoxSelectedColor.Location = new System.Drawing.Point(3, 156);
            PictureBoxSelectedColor.Name = "PictureBoxSelectedColor";
            PictureBoxSelectedColor.Size = new System.Drawing.Size(177, 26);
            PictureBoxSelectedColor.TabIndex = 7;
            PictureBoxSelectedColor.TabStop = false;
            // 
            // PictureBoxHoverColor
            // 
            tableLayoutPanel2.SetColumnSpan(PictureBoxHoverColor, 2);
            PictureBoxHoverColor.Location = new System.Drawing.Point(235, 156);
            PictureBoxHoverColor.Name = "PictureBoxHoverColor";
            PictureBoxHoverColor.Size = new System.Drawing.Size(156, 26);
            PictureBoxHoverColor.TabIndex = 8;
            PictureBoxHoverColor.TabStop = false;
            // 
            // label1
            // 
            label1.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(3, 133);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(66, 20);
            label1.TabIndex = 9;
            label1.Text = "Selected";
            // 
            // label2
            // 
            label2.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(235, 133);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(35, 20);
            label2.TabIndex = 10;
            label2.Text = "Pick";
            // 
            // PictureBoxRainbow
            // 
            PictureBoxRainbow.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            PictureBoxRainbow.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            PictureBoxRainbow.Cursor = System.Windows.Forms.Cursors.Cross;
            PictureBoxRainbow.Location = new System.Drawing.Point(381, 3);
            PictureBoxRainbow.Name = "PictureBoxRainbow";
            PictureBoxRainbow.Size = new System.Drawing.Size(35, 121);
            PictureBoxRainbow.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            PictureBoxRainbow.TabIndex = 12;
            PictureBoxRainbow.TabStop = false;
            PictureBoxRainbow.MouseClick += PictureBoxRainbow_MouseClick;
            PictureBoxRainbow.MouseMove += PictureBoxRainbow_MouseMove;
            PictureBoxRainbow.Resize += PictureBoxRainbow_Resize;
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.ColumnCount = 4;
            tableLayoutPanel2.SetColumnSpan(tableLayoutPanel3, 3);
            tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 161F));
            tableLayoutPanel3.Controls.Add(TextBoxHex, 3, 1);
            tableLayoutPanel3.Controls.Add(label6, 3, 0);
            tableLayoutPanel3.Controls.Add(label7, 0, 0);
            tableLayoutPanel3.Controls.Add(label8, 1, 0);
            tableLayoutPanel3.Controls.Add(label9, 2, 0);
            tableLayoutPanel3.Controls.Add(TextBoxH, 0, 1);
            tableLayoutPanel3.Controls.Add(TextBoxS, 1, 1);
            tableLayoutPanel3.Controls.Add(TextBoxV, 2, 1);
            tableLayoutPanel3.Location = new System.Drawing.Point(3, 299);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 2;
            tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 43.636364F));
            tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 56.363636F));
            tableLayoutPanel3.Size = new System.Drawing.Size(341, 61);
            tableLayoutPanel3.TabIndex = 11;
            // 
            // TextBoxHex
            // 
            TextBoxHex.Location = new System.Drawing.Point(183, 29);
            TextBoxHex.Name = "TextBoxHex";
            TextBoxHex.Size = new System.Drawing.Size(131, 27);
            TextBoxHex.TabIndex = 3;
            TextBoxHex.TextChanged += TextBoxHex_TextChanged;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new System.Drawing.Point(183, 0);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(37, 20);
            label6.TabIndex = 7;
            label6.Text = "HEX";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new System.Drawing.Point(3, 0);
            label7.Name = "label7";
            label7.Size = new System.Drawing.Size(20, 20);
            label7.TabIndex = 8;
            label7.Text = "H";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new System.Drawing.Point(63, 0);
            label8.Name = "label8";
            label8.Size = new System.Drawing.Size(17, 20);
            label8.TabIndex = 9;
            label8.Text = "S";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new System.Drawing.Point(123, 0);
            label9.Name = "label9";
            label9.Size = new System.Drawing.Size(18, 20);
            label9.TabIndex = 10;
            label9.Text = "V";
            // 
            // TextBoxH
            // 
            TextBoxH.Location = new System.Drawing.Point(3, 29);
            TextBoxH.Name = "TextBoxH";
            TextBoxH.Size = new System.Drawing.Size(54, 27);
            TextBoxH.TabIndex = 11;
            // 
            // TextBoxS
            // 
            TextBoxS.Location = new System.Drawing.Point(63, 29);
            TextBoxS.Name = "TextBoxS";
            TextBoxS.Size = new System.Drawing.Size(54, 27);
            TextBoxS.TabIndex = 12;
            // 
            // TextBoxV
            // 
            TextBoxV.Location = new System.Drawing.Point(123, 29);
            TextBoxV.Name = "TextBoxV";
            TextBoxV.Size = new System.Drawing.Size(54, 27);
            TextBoxV.TabIndex = 13;
            // 
            // tableLayoutPanel4
            // 
            tableLayoutPanel4.ColumnCount = 3;
            tableLayoutPanel2.SetColumnSpan(tableLayoutPanel4, 2);
            tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 78.3333359F));
            tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 21.6666641F));
            tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 61F));
            tableLayoutPanel4.Controls.Add(label3, 1, 0);
            tableLayoutPanel4.Controls.Add(label4, 1, 1);
            tableLayoutPanel4.Controls.Add(label5, 1, 2);
            tableLayoutPanel4.Controls.Add(TextBoxR, 2, 0);
            tableLayoutPanel4.Controls.Add(TextBoxB, 2, 2);
            tableLayoutPanel4.Controls.Add(TextBoxG, 2, 1);
            tableLayoutPanel4.Controls.Add(GradientSliderChannelR, 0, 0);
            tableLayoutPanel4.Controls.Add(GradientSliderChannelB, 0, 2);
            tableLayoutPanel4.Controls.Add(GradientSliderChannelG, 0, 1);
            tableLayoutPanel4.Location = new System.Drawing.Point(3, 191);
            tableLayoutPanel4.Name = "tableLayoutPanel4";
            tableLayoutPanel4.RowCount = 3;
            tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            tableLayoutPanel4.Size = new System.Drawing.Size(312, 102);
            tableLayoutPanel4.TabIndex = 13;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(199, 8);
            label3.Margin = new System.Windows.Forms.Padding(3, 8, 3, 0);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(18, 20);
            label3.TabIndex = 4;
            label3.Text = "R";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(199, 42);
            label4.Margin = new System.Windows.Forms.Padding(3, 8, 3, 0);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(19, 20);
            label4.TabIndex = 5;
            label4.Text = "G";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new System.Drawing.Point(199, 76);
            label5.Margin = new System.Windows.Forms.Padding(3, 8, 3, 0);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(18, 20);
            label5.TabIndex = 6;
            label5.Text = "B";
            // 
            // TextBoxR
            // 
            TextBoxR.Location = new System.Drawing.Point(253, 3);
            TextBoxR.Name = "TextBoxR";
            TextBoxR.Size = new System.Drawing.Size(54, 27);
            TextBoxR.TabIndex = 0;
            TextBoxR.TextChanged += TextBoxR_TextChanged;
            // 
            // TextBoxB
            // 
            TextBoxB.Location = new System.Drawing.Point(253, 71);
            TextBoxB.Name = "TextBoxB";
            TextBoxB.Size = new System.Drawing.Size(54, 27);
            TextBoxB.TabIndex = 2;
            TextBoxB.TextChanged += TextBoxB_TextChanged;
            // 
            // TextBoxG
            // 
            TextBoxG.Location = new System.Drawing.Point(253, 37);
            TextBoxG.Name = "TextBoxG";
            TextBoxG.Size = new System.Drawing.Size(53, 27);
            TextBoxG.TabIndex = 1;
            TextBoxG.TextChanged += TextBoxG_TextChanged;
            // 
            // GradientSliderChannelR
            // 
            GradientSliderChannelR.EndColor = System.Drawing.Color.Red;
            GradientSliderChannelR.Location = new System.Drawing.Point(3, 3);
            GradientSliderChannelR.Maximum = 255;
            GradientSliderChannelR.Name = "GradientSliderChannelR";
            GradientSliderChannelR.Size = new System.Drawing.Size(190, 28);
            GradientSliderChannelR.TabIndex = 7;
            GradientSliderChannelR.Text = "gradientSlider2";
            GradientSliderChannelR.ThumbHeight = 28;
            GradientSliderChannelR.ValueChanged += GradientSliderChannelR_ValueChanged;
            // 
            // GradientSliderChannelB
            // 
            GradientSliderChannelB.EndColor = System.Drawing.Color.Blue;
            GradientSliderChannelB.Location = new System.Drawing.Point(3, 71);
            GradientSliderChannelB.Maximum = 255;
            GradientSliderChannelB.Name = "GradientSliderChannelB";
            GradientSliderChannelB.Size = new System.Drawing.Size(190, 28);
            GradientSliderChannelB.TabIndex = 9;
            GradientSliderChannelB.Text = "gradientSlider4";
            GradientSliderChannelB.ThumbHeight = 28;
            GradientSliderChannelB.ValueChanged += GradientSliderChannelB_ValueChanged;
            // 
            // GradientSliderChannelG
            // 
            GradientSliderChannelG.EndColor = System.Drawing.Color.Green;
            GradientSliderChannelG.Location = new System.Drawing.Point(3, 37);
            GradientSliderChannelG.Maximum = 255;
            GradientSliderChannelG.Name = "GradientSliderChannelG";
            GradientSliderChannelG.Size = new System.Drawing.Size(190, 28);
            GradientSliderChannelG.TabIndex = 8;
            GradientSliderChannelG.Text = "gradientSlider3";
            GradientSliderChannelG.ThumbHeight = 28;
            GradientSliderChannelG.Value = 100F;
            GradientSliderChannelG.ValueChanged += GradientSliderChannelG_ValueChanged;
            // 
            // toolStrip1
            // 
            toolStrip1.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripButton1, toolStripSeparator1, toolStripButton2, toolStripButton3, toolStripSeparator2, toolStripDropDownButton1, toolStripButton4 });
            toolStrip1.Location = new System.Drawing.Point(0, 33);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            toolStrip1.Size = new System.Drawing.Size(424, 27);
            toolStrip1.TabIndex = 8;
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
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
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
            // toolStripButton3
            // 
            toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButton3.Image = (System.Drawing.Image)resources.GetObject("toolStripButton3.Image");
            toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButton3.Name = "toolStripButton3";
            toolStripButton3.Size = new System.Drawing.Size(29, 24);
            toolStripButton3.Text = "toolStripButton3";
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new System.Drawing.Size(6, 27);
            // 
            // toolStripDropDownButton1
            // 
            toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripDropDownButton1.Image = (System.Drawing.Image)resources.GetObject("toolStripDropDownButton1.Image");
            toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            toolStripDropDownButton1.Size = new System.Drawing.Size(34, 24);
            toolStripDropDownButton1.Text = "toolStripDropDownButton1";
            // 
            // toolStripButton4
            // 
            toolStripButton4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButton4.Image = (System.Drawing.Image)resources.GetObject("toolStripButton4.Image");
            toolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButton4.Name = "toolStripButton4";
            toolStripButton4.Size = new System.Drawing.Size(29, 24);
            toolStripButton4.Text = "toolStripButton4";
            // 
            // ColorSelectTooltip
            // 
            ColorSelectTooltip.AutoPopDelay = 30000;
            ColorSelectTooltip.InitialDelay = 200;
            ColorSelectTooltip.IsBalloon = true;
            ColorSelectTooltip.ReshowDelay = 100;
            ColorSelectTooltip.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            ColorSelectTooltip.ToolTipTitle = "Win_CBZ";
            // 
            // ColorSelect
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(855, 513);
            Controls.Add(ColorEditorTableLayout);
            DoubleBuffered = true;
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            Name = "ColorSelect";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Select a Color...";
            Load += ColorSelect_Load;
            ColorEditorTableLayout.ResumeLayout(false);
            HeaderPanel.ResumeLayout(false);
            HeaderPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)HeaderPicture).EndInit();
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            PalettesTabControl.ResumeLayout(false);
            TabPageDefaultPalette.ResumeLayout(false);
            TabPageSystemPalette.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)PictureBoxPalette).EndInit();
            ((System.ComponentModel.ISupportInitialize)PictureBoxSelectedColor).EndInit();
            ((System.ComponentModel.ISupportInitialize)PictureBoxHoverColor).EndInit();
            ((System.ComponentModel.ISupportInitialize)PictureBoxRainbow).EndInit();
            tableLayoutPanel3.ResumeLayout(false);
            tableLayoutPanel3.PerformLayout();
            tableLayoutPanel4.ResumeLayout(false);
            tableLayoutPanel4.PerformLayout();
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel ColorEditorTableLayout;
        private System.Windows.Forms.Panel HeaderPanel;
        private System.Windows.Forms.Label HeaderLabel;
        private System.Windows.Forms.PictureBox HeaderPicture;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button OkButton;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TabControl PalettesTabControl;
        private System.Windows.Forms.TabPage TabPageDefaultPalette;
        private System.Windows.Forms.FlowLayoutPanel FlowLayoutDefaultPalette;
        private System.Windows.Forms.TabPage TabPageSystemPalette;
        private System.Windows.Forms.PictureBox PictureBoxPalette;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.PictureBox PictureBoxSelectedColor;
        private System.Windows.Forms.PictureBox PictureBoxHoverColor;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TabPage TabPageCustomPalette;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ToolStripButton toolStripButton3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripButton toolStripButton4;
        private System.Windows.Forms.TextBox TextBoxR;
        private System.Windows.Forms.TextBox TextBoxG;
        private System.Windows.Forms.TextBox TextBoxB;
        private System.Windows.Forms.TextBox TextBoxHex;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.PictureBox PictureBoxRainbow;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox TextBoxH;
        private System.Windows.Forms.TextBox TextBoxS;
        private System.Windows.Forms.TextBox TextBoxV;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.ToolTip ColorSelectTooltip;
        private System.Windows.Forms.FlowLayoutPanel FlowLayoutSystemPalette;
        private Components.GradientSlider.GradientSlider GradientSliderChannelR;
        private Components.GradientSlider.GradientSlider GradientSliderChannelG;
        private Components.GradientSlider.GradientSlider GradientSliderChannelB;
    }
}