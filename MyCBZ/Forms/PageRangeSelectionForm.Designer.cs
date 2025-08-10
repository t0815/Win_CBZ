namespace Win_CBZ.Forms
{
    partial class PageRangeSelectionForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PageRangeSelectionForm));
            ItemEditorTableLayout = new System.Windows.Forms.TableLayoutPanel();
            label2 = new System.Windows.Forms.Label();
            CancelBtn = new System.Windows.Forms.Button();
            OkButton = new System.Windows.Forms.Button();
            HeaderPanel = new System.Windows.Forms.Panel();
            HeaderLabel = new System.Windows.Forms.Label();
            HeaderPicture = new System.Windows.Forms.PictureBox();
            TextBoxSelections = new System.Windows.Forms.TextBox();
            label1 = new System.Windows.Forms.Label();
            TextBoxOffset = new System.Windows.Forms.TextBox();
            CheckBoxUseOffset = new System.Windows.Forms.CheckBox();
            pictureBox2 = new System.Windows.Forms.PictureBox();
            ValidationErrorProvider = new System.Windows.Forms.ErrorProvider(components);
            RangeSelectionTooltip = new System.Windows.Forms.ToolTip(components);
            ItemEditorTableLayout.SuspendLayout();
            HeaderPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)HeaderPicture).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)ValidationErrorProvider).BeginInit();
            SuspendLayout();
            // 
            // ItemEditorTableLayout
            // 
            ItemEditorTableLayout.ColumnCount = 4;
            ItemEditorTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            ItemEditorTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            ItemEditorTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 127F));
            ItemEditorTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 112F));
            ItemEditorTableLayout.Controls.Add(label2, 2, 1);
            ItemEditorTableLayout.Controls.Add(CancelBtn, 3, 6);
            ItemEditorTableLayout.Controls.Add(OkButton, 2, 6);
            ItemEditorTableLayout.Controls.Add(HeaderPanel, 0, 0);
            ItemEditorTableLayout.Controls.Add(TextBoxSelections, 1, 2);
            ItemEditorTableLayout.Controls.Add(label1, 1, 1);
            ItemEditorTableLayout.Controls.Add(TextBoxOffset, 2, 4);
            ItemEditorTableLayout.Controls.Add(CheckBoxUseOffset, 1, 4);
            ItemEditorTableLayout.Controls.Add(pictureBox2, 1, 6);
            ItemEditorTableLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            ItemEditorTableLayout.Location = new System.Drawing.Point(0, 0);
            ItemEditorTableLayout.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            ItemEditorTableLayout.Name = "ItemEditorTableLayout";
            ItemEditorTableLayout.RowCount = 7;
            ItemEditorTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            ItemEditorTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            ItemEditorTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            ItemEditorTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            ItemEditorTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 47F));
            ItemEditorTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            ItemEditorTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 51F));
            ItemEditorTableLayout.Size = new System.Drawing.Size(367, 258);
            ItemEditorTableLayout.TabIndex = 2;
            // 
            // label2
            // 
            label2.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(131, 79);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(0, 20);
            label2.TabIndex = 10;
            // 
            // CancelBtn
            // 
            CancelBtn.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            CancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            CancelBtn.Location = new System.Drawing.Point(264, 212);
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
            OkButton.Location = new System.Drawing.Point(151, 212);
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
            HeaderPanel.Location = new System.Drawing.Point(3, 0);
            HeaderPanel.Margin = new System.Windows.Forms.Padding(3, 0, 3, 2);
            HeaderPanel.Name = "HeaderPanel";
            HeaderPanel.Size = new System.Drawing.Size(361, 61);
            HeaderPanel.TabIndex = 2;
            // 
            // HeaderLabel
            // 
            HeaderLabel.AutoSize = true;
            HeaderLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            HeaderLabel.Location = new System.Drawing.Point(95, 18);
            HeaderLabel.Name = "HeaderLabel";
            HeaderLabel.Size = new System.Drawing.Size(131, 28);
            HeaderLabel.TabIndex = 1;
            HeaderLabel.Text = "Select Pages...";
            // 
            // HeaderPicture
            // 
            HeaderPicture.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            HeaderPicture.Image = Properties.Resources.selection;
            HeaderPicture.Location = new System.Drawing.Point(26, 11);
            HeaderPicture.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            HeaderPicture.Name = "HeaderPicture";
            HeaderPicture.Size = new System.Drawing.Size(65, 30);
            HeaderPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            HeaderPicture.TabIndex = 0;
            HeaderPicture.TabStop = false;
            // 
            // TextBoxSelections
            // 
            ItemEditorTableLayout.SetColumnSpan(TextBoxSelections, 2);
            ValidationErrorProvider.SetIconPadding(TextBoxSelections, -15);
            TextBoxSelections.Location = new System.Drawing.Point(23, 102);
            TextBoxSelections.Name = "TextBoxSelections";
            TextBoxSelections.PlaceholderText = " 3;1-10;1,2,3";
            TextBoxSelections.Size = new System.Drawing.Size(228, 27);
            TextBoxSelections.TabIndex = 7;
            // 
            // label1
            // 
            label1.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(23, 79);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(101, 20);
            label1.TabIndex = 8;
            label1.Text = "Select Page(s)";
            // 
            // TextBoxOffset
            // 
            TextBoxOffset.Location = new System.Drawing.Point(131, 143);
            TextBoxOffset.Name = "TextBoxOffset";
            TextBoxOffset.PlaceholderText = "0";
            TextBoxOffset.Size = new System.Drawing.Size(121, 27);
            TextBoxOffset.TabIndex = 11;
            // 
            // CheckBoxUseOffset
            // 
            CheckBoxUseOffset.AutoSize = true;
            CheckBoxUseOffset.Location = new System.Drawing.Point(23, 145);
            CheckBoxUseOffset.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            CheckBoxUseOffset.Name = "CheckBoxUseOffset";
            CheckBoxUseOffset.Size = new System.Drawing.Size(71, 24);
            CheckBoxUseOffset.TabIndex = 12;
            CheckBoxUseOffset.Text = "Offset";
            CheckBoxUseOffset.UseVisualStyleBackColor = true;
            // 
            // pictureBox2
            // 
            pictureBox2.Image = Properties.Resources.information;
            pictureBox2.InitialImage = Properties.Resources.information;
            pictureBox2.Location = new System.Drawing.Point(26, 207);
            pictureBox2.Margin = new System.Windows.Forms.Padding(6, 0, 0, 0);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new System.Drawing.Size(36, 43);
            pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            pictureBox2.TabIndex = 32;
            pictureBox2.TabStop = false;
            RangeSelectionTooltip.SetToolTip(pictureBox2, resources.GetString("pictureBox2.ToolTip"));
            // 
            // ValidationErrorProvider
            // 
            ValidationErrorProvider.ContainerControl = this;
            // 
            // RangeSelectionTooltip
            // 
            RangeSelectionTooltip.AutoPopDelay = 600000;
            RangeSelectionTooltip.InitialDelay = 500;
            RangeSelectionTooltip.IsBalloon = true;
            RangeSelectionTooltip.ReshowDelay = 100;
            RangeSelectionTooltip.ToolTipTitle = "Win_CBZ";
            // 
            // PageRangeSelectionForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(367, 258);
            Controls.Add(ItemEditorTableLayout);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            Name = "PageRangeSelectionForm";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Select Range";
            FormClosing += PageRangeSelectionForm_FormClosing;
            ItemEditorTableLayout.ResumeLayout(false);
            ItemEditorTableLayout.PerformLayout();
            HeaderPanel.ResumeLayout(false);
            HeaderPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)HeaderPicture).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ((System.ComponentModel.ISupportInitialize)ValidationErrorProvider).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel ItemEditorTableLayout;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button OkButton;
        private System.Windows.Forms.Panel HeaderPanel;
        private System.Windows.Forms.Label HeaderLabel;
        private System.Windows.Forms.PictureBox HeaderPicture;
        private System.Windows.Forms.TextBox TextBoxSelections;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TextBoxOffset;
        private System.Windows.Forms.CheckBox CheckBoxUseOffset;
        private System.Windows.Forms.ErrorProvider ValidationErrorProvider;
        private System.Windows.Forms.ToolTip RangeSelectionTooltip;
        private System.Windows.Forms.PictureBox pictureBox2;
    }
}