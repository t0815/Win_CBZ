namespace Win_CBZ.Forms
{
    partial class TextEditorForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TextEditorForm));
            ItemEditorTableLayout = new System.Windows.Forms.TableLayoutPanel();
            HeaderPanel = new System.Windows.Forms.Panel();
            HeaderLabel = new System.Windows.Forms.Label();
            HeaderPicture = new System.Windows.Forms.PictureBox();
            InfoLabel = new System.Windows.Forms.Label();
            ItemsText = new System.Windows.Forms.TextBox();
            ItemEditorToolBar = new System.Windows.Forms.ToolStrip();
            ToolStripTextBoxSearch = new System.Windows.Forms.ToolStripTextBox();
            ToolButtonSortAscending = new System.Windows.Forms.ToolStripButton();
            CancelBtn = new System.Windows.Forms.Button();
            OkButton = new System.Windows.Forms.Button();
            AutoCompleteItems = new AutocompleteMenuNS.AutocompleteMenu();
            AutocompleteIcons = new System.Windows.Forms.ImageList(components);
            ItemEditorTableLayout.SuspendLayout();
            HeaderPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)HeaderPicture).BeginInit();
            ItemEditorToolBar.SuspendLayout();
            SuspendLayout();
            // 
            // ItemEditorTableLayout
            // 
            ItemEditorTableLayout.ColumnCount = 3;
            ItemEditorTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            ItemEditorTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 115F));
            ItemEditorTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 107F));
            ItemEditorTableLayout.Controls.Add(HeaderPanel, 0, 0);
            ItemEditorTableLayout.Controls.Add(InfoLabel, 0, 3);
            ItemEditorTableLayout.Controls.Add(ItemsText, 0, 2);
            ItemEditorTableLayout.Controls.Add(ItemEditorToolBar, 1, 1);
            ItemEditorTableLayout.Controls.Add(CancelBtn, 2, 4);
            ItemEditorTableLayout.Controls.Add(OkButton, 1, 4);
            ItemEditorTableLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            ItemEditorTableLayout.Location = new System.Drawing.Point(0, 0);
            ItemEditorTableLayout.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            ItemEditorTableLayout.Name = "ItemEditorTableLayout";
            ItemEditorTableLayout.RowCount = 5;
            ItemEditorTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            ItemEditorTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 57F));
            ItemEditorTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
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
            HeaderLabel.Location = new System.Drawing.Point(95, 9);
            HeaderLabel.Name = "HeaderLabel";
            HeaderLabel.Size = new System.Drawing.Size(109, 28);
            HeaderLabel.TabIndex = 1;
            HeaderLabel.Text = "Item Editor";
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
            InfoLabel.Size = new System.Drawing.Size(124, 20);
            InfoLabel.TabIndex = 6;
            InfoLabel.Text = "One item per line";
            // 
            // ItemsText
            // 
            ItemsText.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            AutoCompleteItems.SetAutocompleteMenu(ItemsText, AutoCompleteItems);
            ItemEditorTableLayout.SetColumnSpan(ItemsText, 3);
            ItemsText.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            ItemsText.HideSelection = false;
            ItemsText.Location = new System.Drawing.Point(4, 137);
            ItemsText.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            ItemsText.Multiline = true;
            ItemsText.Name = "ItemsText";
            ItemsText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            ItemsText.Size = new System.Drawing.Size(577, 272);
            ItemsText.TabIndex = 5;
            ItemsText.WordWrap = false;
            // 
            // ItemEditorToolBar
            // 
            ItemEditorToolBar.AllowMerge = false;
            ItemEditorToolBar.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            ItemEditorToolBar.CanOverflow = false;
            ItemEditorTableLayout.SetColumnSpan(ItemEditorToolBar, 2);
            ItemEditorToolBar.Dock = System.Windows.Forms.DockStyle.None;
            ItemEditorToolBar.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            ItemEditorToolBar.ImageScalingSize = new System.Drawing.Size(20, 20);
            ItemEditorToolBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { ToolStripTextBoxSearch, ToolButtonSortAscending });
            ItemEditorToolBar.Location = new System.Drawing.Point(390, 103);
            ItemEditorToolBar.Margin = new System.Windows.Forms.Padding(8, 0, 8, 2);
            ItemEditorToolBar.Name = "ItemEditorToolBar";
            ItemEditorToolBar.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            ItemEditorToolBar.Size = new System.Drawing.Size(168, 27);
            ItemEditorToolBar.Stretch = true;
            ItemEditorToolBar.TabIndex = 7;
            ItemEditorToolBar.Text = "toolStrip1";
            // 
            // ToolStripTextBoxSearch
            // 
            ToolStripTextBoxSearch.Name = "ToolStripTextBoxSearch";
            ToolStripTextBoxSearch.Size = new System.Drawing.Size(134, 27);
            ToolStripTextBoxSearch.ToolTipText = "Type to search, F3 to find next occurence.";
            ToolStripTextBoxSearch.KeyUp += ToolStripTextBoxSearch_KeyUp;
            // 
            // ToolButtonSortAscending
            // 
            ToolButtonSortAscending.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            ToolButtonSortAscending.Image = Properties.Resources.sort_az_ascending2;
            ToolButtonSortAscending.ImageTransparentColor = System.Drawing.Color.Magenta;
            ToolButtonSortAscending.Name = "ToolButtonSortAscending";
            ToolButtonSortAscending.Size = new System.Drawing.Size(29, 24);
            ToolButtonSortAscending.Text = "toolStripButton1";
            ToolButtonSortAscending.ToolTipText = "Sort items ascending";
            ToolButtonSortAscending.Click += toolStripButton1_Click;
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
            AutocompleteIcons.Images.SetKeyName(13, "blackwhite");
            AutocompleteIcons.Images.SetKeyName(14, "family");
            // 
            // TextEditorForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(585, 526);
            Controls.Add(ItemEditorTableLayout);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            MinimizeBox = false;
            Name = "TextEditorForm";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Item Editor";
            FormClosing += TextEditorForm_FormClosing;
            Load += TextEditorForm_Load;
            ItemEditorTableLayout.ResumeLayout(false);
            ItemEditorTableLayout.PerformLayout();
            HeaderPanel.ResumeLayout(false);
            HeaderPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)HeaderPicture).EndInit();
            ItemEditorToolBar.ResumeLayout(false);
            ItemEditorToolBar.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel ItemEditorTableLayout;
        private System.Windows.Forms.Panel HeaderPanel;
        private System.Windows.Forms.Label HeaderLabel;
        private System.Windows.Forms.PictureBox HeaderPicture;
        private System.Windows.Forms.Button OkButton;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.TextBox ItemsText;
        private System.Windows.Forms.Label InfoLabel;
        private System.Windows.Forms.ToolStrip ItemEditorToolBar;
        private System.Windows.Forms.ToolStripButton ToolButtonSortAscending;
        private AutocompleteMenuNS.AutocompleteMenu AutoCompleteItems;
        private System.Windows.Forms.ToolStripTextBox ToolStripTextBoxSearch;
        private System.Windows.Forms.ImageList AutocompleteIcons;
    }
}