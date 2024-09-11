namespace Win_CBZ.Forms
{
    partial class VariableInsertionEditorConfigForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VariableInsertionEditorConfigForm));
            ItemEditorTableLayout = new System.Windows.Forms.TableLayoutPanel();
            InfoLabel = new System.Windows.Forms.Label();
            CancelBtn = new System.Windows.Forms.Button();
            OkButton = new System.Windows.Forms.Button();
            HeaderPanel = new System.Windows.Forms.Panel();
            HeaderLabel = new System.Windows.Forms.Label();
            HeaderPicture = new System.Windows.Forms.PictureBox();
            TextBoxOutput = new System.Windows.Forms.TextBox();
            label1 = new System.Windows.Forms.Label();
            AutoCompleteVariables = new AutocompleteMenuNS.AutocompleteMenu();
            AutocompleteIcons = new System.Windows.Forms.ImageList(components);
            ItemEditorTableLayout.SuspendLayout();
            HeaderPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)HeaderPicture).BeginInit();
            SuspendLayout();
            // 
            // ItemEditorTableLayout
            // 
            ItemEditorTableLayout.ColumnCount = 4;
            ItemEditorTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            ItemEditorTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            ItemEditorTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 115F));
            ItemEditorTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 107F));
            ItemEditorTableLayout.Controls.Add(InfoLabel, 1, 4);
            ItemEditorTableLayout.Controls.Add(CancelBtn, 3, 5);
            ItemEditorTableLayout.Controls.Add(OkButton, 2, 5);
            ItemEditorTableLayout.Controls.Add(HeaderPanel, 0, 0);
            ItemEditorTableLayout.Controls.Add(TextBoxOutput, 1, 2);
            ItemEditorTableLayout.Controls.Add(label1, 1, 1);
            ItemEditorTableLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            ItemEditorTableLayout.Location = new System.Drawing.Point(0, 0);
            ItemEditorTableLayout.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            ItemEditorTableLayout.Name = "ItemEditorTableLayout";
            ItemEditorTableLayout.RowCount = 6;
            ItemEditorTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            ItemEditorTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 42F));
            ItemEditorTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            ItemEditorTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            ItemEditorTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 95F));
            ItemEditorTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 55F));
            ItemEditorTableLayout.Size = new System.Drawing.Size(501, 344);
            ItemEditorTableLayout.TabIndex = 1;
            // 
            // InfoLabel
            // 
            InfoLabel.AutoSize = true;
            ItemEditorTableLayout.SetColumnSpan(InfoLabel, 2);
            InfoLabel.Location = new System.Drawing.Point(31, 206);
            InfoLabel.Margin = new System.Windows.Forms.Padding(11, 12, 4, 0);
            InfoLabel.Name = "InfoLabel";
            InfoLabel.Size = new System.Drawing.Size(312, 40);
            InfoLabel.TabIndex = 6;
            InfoLabel.Text = "Create output-string by defining one or more variables, which should be inserted";
            // 
            // CancelBtn
            // 
            CancelBtn.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            CancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            CancelBtn.Location = new System.Drawing.Point(398, 294);
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
            OkButton.Location = new System.Drawing.Point(290, 294);
            OkButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            OkButton.Name = "OkButton";
            OkButton.Size = new System.Drawing.Size(100, 35);
            OkButton.TabIndex = 3;
            OkButton.Text = "Ok";
            OkButton.UseVisualStyleBackColor = true;
            OkButton.Click += OkButton_Click;
            // 
            // HeaderPanel
            // 
            HeaderPanel.BackColor = System.Drawing.Color.White;
            ItemEditorTableLayout.SetColumnSpan(HeaderPanel, 4);
            HeaderPanel.Controls.Add(HeaderLabel);
            HeaderPanel.Controls.Add(HeaderPicture);
            HeaderPanel.Location = new System.Drawing.Point(3, 2);
            HeaderPanel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            HeaderPanel.Name = "HeaderPanel";
            HeaderPanel.Size = new System.Drawing.Size(495, 71);
            HeaderPanel.TabIndex = 2;
            // 
            // HeaderLabel
            // 
            HeaderLabel.AutoSize = true;
            HeaderLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            HeaderLabel.Location = new System.Drawing.Point(95, 18);
            HeaderLabel.Name = "HeaderLabel";
            HeaderLabel.Size = new System.Drawing.Size(190, 28);
            HeaderLabel.TabIndex = 1;
            HeaderLabel.Text = "Value  Configuration";
            // 
            // HeaderPicture
            // 
            HeaderPicture.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            HeaderPicture.Image = Properties.Resources.formula;
            HeaderPicture.Location = new System.Drawing.Point(24, 6);
            HeaderPicture.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            HeaderPicture.Name = "HeaderPicture";
            HeaderPicture.Size = new System.Drawing.Size(65, 50);
            HeaderPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            HeaderPicture.TabIndex = 0;
            HeaderPicture.TabStop = false;
            // 
            // TextBoxOutput
            // 
            AutoCompleteVariables.SetAutocompleteMenu(TextBoxOutput, AutoCompleteVariables);
            ItemEditorTableLayout.SetColumnSpan(TextBoxOutput, 2);
            TextBoxOutput.Location = new System.Drawing.Point(23, 120);
            TextBoxOutput.Name = "TextBoxOutput";
            TextBoxOutput.PlaceholderText = "{VARIABLE_NAME}";
            TextBoxOutput.Size = new System.Drawing.Size(368, 27);
            TextBoxOutput.TabIndex = 7;
            // 
            // label1
            // 
            label1.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(23, 97);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(69, 20);
            label1.TabIndex = 8;
            label1.Text = "Variables";
            // 
            // AutoCompleteVariables
            // 
            AutoCompleteVariables.Colors = (AutocompleteMenuNS.Colors)resources.GetObject("AutoCompleteVariables.Colors");
            AutoCompleteVariables.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            AutoCompleteVariables.ImageList = AutocompleteIcons;
            AutoCompleteVariables.MinFragmentLength = 1;
            AutoCompleteVariables.SearchPattern = "[\\{{0,1}\\w\\+]";
            AutoCompleteVariables.TargetControlWrapper = null;
            // 
            // AutocompleteIcons
            // 
            AutocompleteIcons.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            AutocompleteIcons.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("AutocompleteIcons.ImageStream");
            AutocompleteIcons.TransparentColor = System.Drawing.Color.Transparent;
            AutocompleteIcons.Images.SetKeyName(0, "tag");
            AutocompleteIcons.Images.SetKeyName(1, "star");
            AutocompleteIcons.Images.SetKeyName(2, "user");
            AutocompleteIcons.Images.SetKeyName(3, "barcode");
            AutocompleteIcons.Images.SetKeyName(4, "books");
            AutocompleteIcons.Images.SetKeyName(5, "users");
            AutocompleteIcons.Images.SetKeyName(6, "book");
            AutocompleteIcons.Images.SetKeyName(7, "planet");
            AutocompleteIcons.Images.SetKeyName(8, "box");
            AutocompleteIcons.Images.SetKeyName(9, "message");
            AutocompleteIcons.Images.SetKeyName(10, "earth");
            AutocompleteIcons.Images.SetKeyName(11, "clock");
            AutocompleteIcons.Images.SetKeyName(12, "hash");
            // 
            // VariableInsertionEditorConfigForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(501, 344);
            Controls.Add(ItemEditorTableLayout);
            Name = "VariableInsertionEditorConfigForm";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Define AutoFill Variables";
            FormClosing += VariableInsertionEditorConfigForm_FormClosing;
            ItemEditorTableLayout.ResumeLayout(false);
            ItemEditorTableLayout.PerformLayout();
            HeaderPanel.ResumeLayout(false);
            HeaderPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)HeaderPicture).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel ItemEditorTableLayout;
        private System.Windows.Forms.Panel HeaderPanel;
        private System.Windows.Forms.Label HeaderLabel;
        private System.Windows.Forms.PictureBox HeaderPicture;
        private System.Windows.Forms.Label InfoLabel;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button OkButton;
        private System.Windows.Forms.ComboBox comboBox1;
        private AutocompleteMenuNS.AutocompleteMenu AutoCompleteVariables;
        private System.Windows.Forms.TextBox TextBoxOutput;
        private System.Windows.Forms.ImageList AutocompleteIcons;
        private System.Windows.Forms.Label label1;
    }
}