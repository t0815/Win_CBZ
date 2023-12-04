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
            this.components = new System.ComponentModel.Container();
            this.KeyValueDatagrid = new System.Windows.Forms.DataGridView();
            this.ItemEditorTableLayout = new System.Windows.Forms.TableLayoutPanel();
            this.HeaderPanel = new System.Windows.Forms.Panel();
            this.HeaderLabel = new System.Windows.Forms.Label();
            this.HeaderPicture = new System.Windows.Forms.PictureBox();
            this.OkButton = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.ItemEditorToolBar = new System.Windows.Forms.ToolStrip();
            this.ToolStripTextBoxSearchKeyValue = new System.Windows.Forms.ToolStripTextBox();
            this.ToolButtonSortAscending = new System.Windows.Forms.ToolStripButton();
            this.languageListBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.KeyValueDatagrid)).BeginInit();
            this.ItemEditorTableLayout.SuspendLayout();
            this.HeaderPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.HeaderPicture)).BeginInit();
            this.ItemEditorToolBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.languageListBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // KeyValueDatagrid
            // 
            this.KeyValueDatagrid.AllowUserToAddRows = false;
            this.KeyValueDatagrid.AllowUserToDeleteRows = false;
            this.KeyValueDatagrid.AllowUserToResizeRows = false;
            this.KeyValueDatagrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ItemEditorTableLayout.SetColumnSpan(this.KeyValueDatagrid, 2);
            this.KeyValueDatagrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.KeyValueDatagrid.Location = new System.Drawing.Point(3, 112);
            this.KeyValueDatagrid.MultiSelect = false;
            this.KeyValueDatagrid.Name = "KeyValueDatagrid";
            this.KeyValueDatagrid.ReadOnly = true;
            this.KeyValueDatagrid.RowHeadersVisible = false;
            this.KeyValueDatagrid.RowHeadersWidth = 51;
            this.KeyValueDatagrid.RowTemplate.Height = 24;
            this.KeyValueDatagrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.KeyValueDatagrid.Size = new System.Drawing.Size(794, 255);
            this.KeyValueDatagrid.TabIndex = 8;
            // 
            // ItemEditorTableLayout
            // 
            this.ItemEditorTableLayout.ColumnCount = 2;
            this.ItemEditorTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ItemEditorTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 107F));
            this.ItemEditorTableLayout.Controls.Add(this.HeaderPanel, 0, 0);
            this.ItemEditorTableLayout.Controls.Add(this.OkButton, 0, 4);
            this.ItemEditorTableLayout.Controls.Add(this.CancelBtn, 1, 4);
            this.ItemEditorTableLayout.Controls.Add(this.ItemEditorToolBar, 1, 0);
            this.ItemEditorTableLayout.Controls.Add(this.KeyValueDatagrid, 0, 2);
            this.ItemEditorTableLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ItemEditorTableLayout.Location = new System.Drawing.Point(0, 0);
            this.ItemEditorTableLayout.Margin = new System.Windows.Forms.Padding(4);
            this.ItemEditorTableLayout.Name = "ItemEditorTableLayout";
            this.ItemEditorTableLayout.RowCount = 5;
            this.ItemEditorTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.ItemEditorTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 49F));
            this.ItemEditorTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ItemEditorTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 37F));
            this.ItemEditorTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 43F));
            this.ItemEditorTableLayout.Size = new System.Drawing.Size(800, 450);
            this.ItemEditorTableLayout.TabIndex = 1;
            // 
            // HeaderPanel
            // 
            this.HeaderPanel.BackColor = System.Drawing.Color.White;
            this.ItemEditorTableLayout.SetColumnSpan(this.HeaderPanel, 2);
            this.HeaderPanel.Controls.Add(this.HeaderLabel);
            this.HeaderPanel.Controls.Add(this.HeaderPicture);
            this.HeaderPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.HeaderPanel.Location = new System.Drawing.Point(3, 2);
            this.HeaderPanel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.HeaderPanel.Name = "HeaderPanel";
            this.HeaderPanel.Size = new System.Drawing.Size(794, 56);
            this.HeaderPanel.TabIndex = 2;
            // 
            // HeaderLabel
            // 
            this.HeaderLabel.AutoSize = true;
            this.HeaderLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HeaderLabel.Location = new System.Drawing.Point(95, 7);
            this.HeaderLabel.Name = "HeaderLabel";
            this.HeaderLabel.Size = new System.Drawing.Size(157, 28);
            this.HeaderLabel.TabIndex = 1;
            this.HeaderLabel.Text = "Key-Value Editor";
            // 
            // HeaderPicture
            // 
            this.HeaderPicture.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.HeaderPicture.Image = global::Win_CBZ.Properties.Resources.user_earth;
            this.HeaderPicture.Location = new System.Drawing.Point(24, 0);
            this.HeaderPicture.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.HeaderPicture.Name = "HeaderPicture";
            this.HeaderPicture.Size = new System.Drawing.Size(65, 54);
            this.HeaderPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.HeaderPicture.TabIndex = 0;
            this.HeaderPicture.TabStop = false;
            // 
            // OkButton
            // 
            this.OkButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.OkButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OkButton.Location = new System.Drawing.Point(589, 411);
            this.OkButton.Margin = new System.Windows.Forms.Padding(4);
            this.OkButton.Name = "OkButton";
            this.OkButton.Size = new System.Drawing.Size(100, 28);
            this.OkButton.TabIndex = 3;
            this.OkButton.Text = "Ok";
            this.OkButton.UseVisualStyleBackColor = true;
            this.OkButton.Click += new System.EventHandler(this.OkButton_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelBtn.Location = new System.Drawing.Point(697, 411);
            this.CancelBtn.Margin = new System.Windows.Forms.Padding(4);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(99, 28);
            this.CancelBtn.TabIndex = 4;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // ItemEditorToolBar
            // 
            this.ItemEditorToolBar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ItemEditorTableLayout.SetColumnSpan(this.ItemEditorToolBar, 2);
            this.ItemEditorToolBar.Dock = System.Windows.Forms.DockStyle.None;
            this.ItemEditorToolBar.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.ItemEditorToolBar.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.ItemEditorToolBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripTextBoxSearchKeyValue,
            this.ToolButtonSortAscending});
            this.ItemEditorToolBar.Location = new System.Drawing.Point(566, 80);
            this.ItemEditorToolBar.Margin = new System.Windows.Forms.Padding(8, 0, 0, 2);
            this.ItemEditorToolBar.Name = "ItemEditorToolBar";
            this.ItemEditorToolBar.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.ItemEditorToolBar.Size = new System.Drawing.Size(234, 27);
            this.ItemEditorToolBar.Stretch = true;
            this.ItemEditorToolBar.TabIndex = 7;
            this.ItemEditorToolBar.Text = "toolStrip1";
            // 
            // ToolStripTextBoxSearchKeyValue
            // 
            this.ToolStripTextBoxSearchKeyValue.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.ToolStripTextBoxSearchKeyValue.Name = "ToolStripTextBoxSearchKeyValue";
            this.ToolStripTextBoxSearchKeyValue.Size = new System.Drawing.Size(200, 27);
            this.ToolStripTextBoxSearchKeyValue.TextChanged += new System.EventHandler(this.ToolStripTextBoxSearchKeyValue_TextChanged);
            // 
            // ToolButtonSortAscending
            // 
            this.ToolButtonSortAscending.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ToolButtonSortAscending.Image = global::Win_CBZ.Properties.Resources.sort_az_ascending2;
            this.ToolButtonSortAscending.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ToolButtonSortAscending.Name = "ToolButtonSortAscending";
            this.ToolButtonSortAscending.Size = new System.Drawing.Size(29, 24);
            this.ToolButtonSortAscending.Text = "toolStripButton1";
            this.ToolButtonSortAscending.ToolTipText = "Sort items ascending";
            // 
            // KeyValueEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.ItemEditorTableLayout);
            this.Name = "KeyValueEditor";
            this.Text = "KeyValueEditor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.KeyValueEditorForm_FormClosing);
            this.Shown += new System.EventHandler(this.KeyValueEditorForm_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.KeyValueDatagrid)).EndInit();
            this.ItemEditorTableLayout.ResumeLayout(false);
            this.ItemEditorTableLayout.PerformLayout();
            this.HeaderPanel.ResumeLayout(false);
            this.HeaderPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.HeaderPicture)).EndInit();
            this.ItemEditorToolBar.ResumeLayout(false);
            this.ItemEditorToolBar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.languageListBindingSource)).EndInit();
            this.ResumeLayout(false);

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