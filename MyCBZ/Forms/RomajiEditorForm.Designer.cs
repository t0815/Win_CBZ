namespace Win_CBZ.Forms
{
    partial class RomajiEditorForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RomajiEditorForm));
            ItemEditorTableLayout = new System.Windows.Forms.TableLayoutPanel();
            HeaderPanel = new System.Windows.Forms.Panel();
            HeaderLabel = new System.Windows.Forms.Label();
            HeaderPicture = new System.Windows.Forms.PictureBox();
            InfoLabel = new System.Windows.Forms.Label();
            ItemEditorToolBar = new System.Windows.Forms.ToolStrip();
            ToolStripTextBoxSearch = new System.Windows.Forms.ToolStripTextBox();
            CancelBtn = new System.Windows.Forms.Button();
            OkButton = new System.Windows.Forms.Button();
            AutoCompleteItems = new AutocompleteMenuNS.AutocompleteMenu();
            AutocompleteIcons = new System.Windows.Forms.ImageList(components);
            ToolStripButtonSearch = new System.Windows.Forms.ToolStripButton();
            dataGridView1 = new System.Windows.Forms.DataGridView();
            ItemEditorTableLayout.SuspendLayout();
            HeaderPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)HeaderPicture).BeginInit();
            ItemEditorToolBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // ItemEditorTableLayout
            // 
            ItemEditorTableLayout.ColumnCount = 3;
            ItemEditorTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            ItemEditorTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 115F));
            ItemEditorTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 107F));
            ItemEditorTableLayout.Controls.Add(HeaderPanel, 0, 0);
            ItemEditorTableLayout.Controls.Add(InfoLabel, 0, 4);
            ItemEditorTableLayout.Controls.Add(CancelBtn, 2, 5);
            ItemEditorTableLayout.Controls.Add(OkButton, 1, 5);
            ItemEditorTableLayout.Controls.Add(ItemEditorToolBar, 0, 1);
            ItemEditorTableLayout.Controls.Add(dataGridView1, 0, 2);
            ItemEditorTableLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            ItemEditorTableLayout.Location = new System.Drawing.Point(0, 0);
            ItemEditorTableLayout.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            ItemEditorTableLayout.Name = "ItemEditorTableLayout";
            ItemEditorTableLayout.RowCount = 6;
            ItemEditorTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            ItemEditorTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 57F));
            ItemEditorTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            ItemEditorTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 61F));
            ItemEditorTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 57F));
            ItemEditorTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 55F));
            ItemEditorTableLayout.Size = new System.Drawing.Size(585, 526);
            ItemEditorTableLayout.TabIndex = 0;
            ItemEditorTableLayout.Paint += ItemEditorTableLayout_Paint;
            // 
            // HeaderPanel
            // 
            HeaderPanel.BackColor = System.Drawing.Color.White;
            ItemEditorTableLayout.SetColumnSpan(HeaderPanel, 3);
            HeaderPanel.Controls.Add(HeaderLabel);
            HeaderPanel.Controls.Add(HeaderPicture);
            HeaderPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            HeaderPanel.Location = new System.Drawing.Point(3, 2);
            HeaderPanel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            HeaderPanel.Name = "HeaderPanel";
            HeaderPanel.Size = new System.Drawing.Size(579, 71);
            HeaderPanel.TabIndex = 2;
            // 
            // HeaderLabel
            // 
            HeaderLabel.AutoSize = true;
            HeaderLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            HeaderLabel.Location = new System.Drawing.Point(95, 18);
            HeaderLabel.Name = "HeaderLabel";
            HeaderLabel.Size = new System.Drawing.Size(130, 28);
            HeaderLabel.TabIndex = 1;
            HeaderLabel.Text = "Romaji Editor";
            // 
            // HeaderPicture
            // 
            HeaderPicture.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            HeaderPicture.Image = Properties.Resources.edit_large;
            HeaderPicture.Location = new System.Drawing.Point(24, 0);
            HeaderPicture.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            HeaderPicture.Name = "HeaderPicture";
            HeaderPicture.Size = new System.Drawing.Size(65, 69);
            HeaderPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            HeaderPicture.TabIndex = 0;
            HeaderPicture.TabStop = false;
            // 
            // InfoLabel
            // 
            InfoLabel.AutoSize = true;
            InfoLabel.Location = new System.Drawing.Point(11, 426);
            InfoLabel.Margin = new System.Windows.Forms.Padding(11, 12, 4, 0);
            InfoLabel.Name = "InfoLabel";
            InfoLabel.Size = new System.Drawing.Size(118, 20);
            InfoLabel.TabIndex = 6;
            InfoLabel.Text = "Select one result";
            // 
            // ItemEditorToolBar
            // 
            ItemEditorToolBar.AllowMerge = false;
            ItemEditorToolBar.CanOverflow = false;
            ItemEditorTableLayout.SetColumnSpan(ItemEditorToolBar, 3);
            ItemEditorToolBar.Dock = System.Windows.Forms.DockStyle.Bottom;
            ItemEditorToolBar.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            ItemEditorToolBar.ImageScalingSize = new System.Drawing.Size(20, 20);
            ItemEditorToolBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { ToolStripTextBoxSearch, ToolStripButtonSearch });
            ItemEditorToolBar.Location = new System.Drawing.Point(8, 103);
            ItemEditorToolBar.Margin = new System.Windows.Forms.Padding(8, 0, 8, 2);
            ItemEditorToolBar.Name = "ItemEditorToolBar";
            ItemEditorToolBar.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            ItemEditorToolBar.Size = new System.Drawing.Size(569, 27);
            ItemEditorToolBar.Stretch = true;
            ItemEditorToolBar.TabIndex = 7;
            ItemEditorToolBar.Text = "toolStrip1";
            // 
            // ToolStripTextBoxSearch
            // 
            ToolStripTextBoxSearch.Name = "ToolStripTextBoxSearch";
            ToolStripTextBoxSearch.Size = new System.Drawing.Size(234, 27);
            ToolStripTextBoxSearch.KeyUp += ToolStripTextBoxSearch_KeyUp;
            // 
            // CancelBtn
            // 
            CancelBtn.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            CancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            CancelBtn.Location = new System.Drawing.Point(482, 476);
            CancelBtn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            CancelBtn.Name = "CancelBtn";
            CancelBtn.Size = new System.Drawing.Size(99, 35);
            CancelBtn.TabIndex = 4;
            CancelBtn.Text = "Cancel";
            CancelBtn.UseVisualStyleBackColor = true;
            CancelBtn.Click += CancelButton_Click;
            // 
            // OkButton
            // 
            OkButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            OkButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            OkButton.Location = new System.Drawing.Point(374, 476);
            OkButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            OkButton.Name = "OkButton";
            OkButton.Size = new System.Drawing.Size(100, 35);
            OkButton.TabIndex = 3;
            OkButton.Text = "Ok";
            OkButton.UseVisualStyleBackColor = true;
            OkButton.Click += OkButton_Click;
            // 
            // AutoCompleteItems
            // 
            AutoCompleteItems.Colors = (AutocompleteMenuNS.Colors)resources.GetObject("AutoCompleteItems.Colors");
            AutoCompleteItems.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            AutoCompleteItems.ImageList = AutocompleteIcons;
            AutoCompleteItems.MinFragmentLength = 1;
            AutoCompleteItems.SearchPattern = "[\\w+( +\\w+)*$]";
            AutoCompleteItems.TargetControlWrapper = null;
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
            // ToolStripButtonSearch
            // 
            ToolStripButtonSearch.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            ToolStripButtonSearch.Image = (System.Drawing.Image)resources.GetObject("ToolStripButtonSearch.Image");
            ToolStripButtonSearch.ImageTransparentColor = System.Drawing.Color.Magenta;
            ToolStripButtonSearch.Name = "ToolStripButtonSearch";
            ToolStripButtonSearch.Size = new System.Drawing.Size(29, 24);
            ToolStripButtonSearch.Text = "toolStripButton1";
            // 
            // dataGridView1
            // 
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            ItemEditorTableLayout.SetColumnSpan(dataGridView1, 3);
            dataGridView1.Location = new System.Drawing.Point(3, 135);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.Size = new System.Drawing.Size(570, 188);
            dataGridView1.TabIndex = 8;
            dataGridView1.CellContentClick += dataGridView1_CellContentClick;
            // 
            // RomajiEditorForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(585, 526);
            Controls.Add(ItemEditorTableLayout);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            MinimizeBox = false;
            Name = "RomajiEditorForm";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Romaji Editor";
            FormClosing += TextEditorForm_FormClosing;
            Load += TextEditorForm_Load;
            ItemEditorTableLayout.ResumeLayout(false);
            ItemEditorTableLayout.PerformLayout();
            HeaderPanel.ResumeLayout(false);
            HeaderPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)HeaderPicture).EndInit();
            ItemEditorToolBar.ResumeLayout(false);
            ItemEditorToolBar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel ItemEditorTableLayout;
        private System.Windows.Forms.Panel HeaderPanel;
        private System.Windows.Forms.Label HeaderLabel;
        private System.Windows.Forms.PictureBox HeaderPicture;
        private System.Windows.Forms.Button OkButton;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Label InfoLabel;
        private System.Windows.Forms.ToolStrip ItemEditorToolBar;
        private AutocompleteMenuNS.AutocompleteMenu AutoCompleteItems;
        private System.Windows.Forms.ToolStripTextBox ToolStripTextBoxSearch;
        private System.Windows.Forms.ImageList AutocompleteIcons;
        private System.Windows.Forms.ToolStripButton ToolStripButtonSearch;
        private System.Windows.Forms.DataGridView dataGridView1;
    }
}