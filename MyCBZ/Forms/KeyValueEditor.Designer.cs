namespace Win_CBZ.Forms
{
    partial class KeyValueEditor
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
            KeyValueDatagrid = new System.Windows.Forms.DataGridView();
            ItemEditorTableLayout = new System.Windows.Forms.TableLayoutPanel();
            HeaderPanel = new System.Windows.Forms.Panel();
            HeaderLabel = new System.Windows.Forms.Label();
            HeaderPicture = new System.Windows.Forms.PictureBox();
            OkButton = new System.Windows.Forms.Button();
            CancelBtn = new System.Windows.Forms.Button();
            ItemEditorToolBar = new System.Windows.Forms.ToolStrip();
            ToolStripTextBoxSearchKeyValue = new System.Windows.Forms.ToolStripTextBox();
            ToolButtonSortAscending = new System.Windows.Forms.ToolStripButton();
            languageListBindingSource = new System.Windows.Forms.BindingSource(components);
            ((System.ComponentModel.ISupportInitialize)KeyValueDatagrid).BeginInit();
            ItemEditorTableLayout.SuspendLayout();
            HeaderPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)HeaderPicture).BeginInit();
            ItemEditorToolBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)languageListBindingSource).BeginInit();
            SuspendLayout();
            // 
            // KeyValueDatagrid
            // 
            KeyValueDatagrid.AllowUserToAddRows = false;
            KeyValueDatagrid.AllowUserToDeleteRows = false;
            KeyValueDatagrid.AllowUserToResizeRows = false;
            KeyValueDatagrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            ItemEditorTableLayout.SetColumnSpan(KeyValueDatagrid, 2);
            KeyValueDatagrid.Dock = System.Windows.Forms.DockStyle.Fill;
            KeyValueDatagrid.Location = new System.Drawing.Point(3, 140);
            KeyValueDatagrid.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            KeyValueDatagrid.MultiSelect = false;
            KeyValueDatagrid.Name = "KeyValueDatagrid";
            KeyValueDatagrid.ReadOnly = true;
            KeyValueDatagrid.RowHeadersVisible = false;
            KeyValueDatagrid.RowHeadersWidth = 51;
            KeyValueDatagrid.RowTemplate.Height = 24;
            KeyValueDatagrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            KeyValueDatagrid.Size = new System.Drawing.Size(794, 318);
            KeyValueDatagrid.TabIndex = 8;
            // 
            // ItemEditorTableLayout
            // 
            ItemEditorTableLayout.ColumnCount = 2;
            ItemEditorTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            ItemEditorTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 107F));
            ItemEditorTableLayout.Controls.Add(HeaderPanel, 0, 0);
            ItemEditorTableLayout.Controls.Add(OkButton, 0, 4);
            ItemEditorTableLayout.Controls.Add(CancelBtn, 1, 4);
            ItemEditorTableLayout.Controls.Add(ItemEditorToolBar, 1, 0);
            ItemEditorTableLayout.Controls.Add(KeyValueDatagrid, 0, 2);
            ItemEditorTableLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            ItemEditorTableLayout.Location = new System.Drawing.Point(0, 0);
            ItemEditorTableLayout.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            ItemEditorTableLayout.Name = "ItemEditorTableLayout";
            ItemEditorTableLayout.RowCount = 5;
            ItemEditorTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            ItemEditorTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 61F));
            ItemEditorTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            ItemEditorTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 46F));
            ItemEditorTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 54F));
            ItemEditorTableLayout.Size = new System.Drawing.Size(800, 562);
            ItemEditorTableLayout.TabIndex = 1;
            // 
            // HeaderPanel
            // 
            HeaderPanel.BackColor = System.Drawing.Color.White;
            ItemEditorTableLayout.SetColumnSpan(HeaderPanel, 2);
            HeaderPanel.Controls.Add(HeaderLabel);
            HeaderPanel.Controls.Add(HeaderPicture);
            HeaderPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            HeaderPanel.Location = new System.Drawing.Point(3, 0);
            HeaderPanel.Margin = new System.Windows.Forms.Padding(3, 0, 3, 2);
            HeaderPanel.Name = "HeaderPanel";
            HeaderPanel.Size = new System.Drawing.Size(794, 73);
            HeaderPanel.TabIndex = 2;
            // 
            // HeaderLabel
            // 
            HeaderLabel.AutoSize = true;
            HeaderLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            HeaderLabel.Location = new System.Drawing.Point(95, 9);
            HeaderLabel.Name = "HeaderLabel";
            HeaderLabel.Size = new System.Drawing.Size(157, 28);
            HeaderLabel.TabIndex = 1;
            HeaderLabel.Text = "Key-Value Editor";
            // 
            // HeaderPicture
            // 
            HeaderPicture.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            HeaderPicture.Image = Properties.Resources.user_earth;
            HeaderPicture.Location = new System.Drawing.Point(24, 0);
            HeaderPicture.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            HeaderPicture.Name = "HeaderPicture";
            HeaderPicture.Size = new System.Drawing.Size(65, 71);
            HeaderPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            HeaderPicture.TabIndex = 0;
            HeaderPicture.TabStop = false;
            // 
            // OkButton
            // 
            OkButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            OkButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            OkButton.Location = new System.Drawing.Point(589, 513);
            OkButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            OkButton.Name = "OkButton";
            OkButton.Size = new System.Drawing.Size(100, 35);
            OkButton.TabIndex = 3;
            OkButton.Text = "Ok";
            OkButton.UseVisualStyleBackColor = true;
            OkButton.Click += OkButton_Click;
            // 
            // CancelBtn
            // 
            CancelBtn.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            CancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            CancelBtn.Location = new System.Drawing.Point(697, 513);
            CancelBtn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            CancelBtn.Name = "CancelBtn";
            CancelBtn.Size = new System.Drawing.Size(99, 35);
            CancelBtn.TabIndex = 4;
            CancelBtn.Text = "Cancel";
            CancelBtn.UseVisualStyleBackColor = true;
            CancelBtn.Click += CancelBtn_Click;
            // 
            // ItemEditorToolBar
            // 
            ItemEditorToolBar.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            ItemEditorTableLayout.SetColumnSpan(ItemEditorToolBar, 2);
            ItemEditorToolBar.Dock = System.Windows.Forms.DockStyle.None;
            ItemEditorToolBar.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            ItemEditorToolBar.ImageScalingSize = new System.Drawing.Size(20, 20);
            ItemEditorToolBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { ToolStripTextBoxSearchKeyValue, ToolButtonSortAscending });
            ItemEditorToolBar.Location = new System.Drawing.Point(566, 107);
            ItemEditorToolBar.Margin = new System.Windows.Forms.Padding(8, 0, 0, 2);
            ItemEditorToolBar.Name = "ItemEditorToolBar";
            ItemEditorToolBar.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            ItemEditorToolBar.Size = new System.Drawing.Size(234, 27);
            ItemEditorToolBar.Stretch = true;
            ItemEditorToolBar.TabIndex = 7;
            ItemEditorToolBar.Text = "toolStrip1";
            // 
            // ToolStripTextBoxSearchKeyValue
            // 
            ToolStripTextBoxSearchKeyValue.Name = "ToolStripTextBoxSearchKeyValue";
            ToolStripTextBoxSearchKeyValue.Size = new System.Drawing.Size(200, 27);
            ToolStripTextBoxSearchKeyValue.TextChanged += ToolStripTextBoxSearchKeyValue_TextChanged;
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
            // 
            // KeyValueEditor
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(800, 562);
            Controls.Add(ItemEditorTableLayout);
            Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            Name = "KeyValueEditor";
            Text = "KeyValueEditor";
            FormClosing += KeyValueEditorForm_FormClosing;
            Shown += KeyValueEditorForm_Shown;
            ((System.ComponentModel.ISupportInitialize)KeyValueDatagrid).EndInit();
            ItemEditorTableLayout.ResumeLayout(false);
            ItemEditorTableLayout.PerformLayout();
            HeaderPanel.ResumeLayout(false);
            HeaderPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)HeaderPicture).EndInit();
            ItemEditorToolBar.ResumeLayout(false);
            ItemEditorToolBar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)languageListBindingSource).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.DataGridView KeyValueDatagrid;
        private System.Windows.Forms.TableLayoutPanel ItemEditorTableLayout;
        private System.Windows.Forms.Panel HeaderPanel;
        private System.Windows.Forms.Label HeaderLabel;
        private System.Windows.Forms.PictureBox HeaderPicture;
        private System.Windows.Forms.Button OkButton;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.ToolStrip ItemEditorToolBar;
        private System.Windows.Forms.ToolStripTextBox ToolStripTextBoxSearchKeyValue;
        private System.Windows.Forms.ToolStripButton ToolButtonSortAscending;
        private System.Windows.Forms.BindingSource languageListBindingSource;
    }
}