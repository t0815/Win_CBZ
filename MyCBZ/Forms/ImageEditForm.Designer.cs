namespace MyCBZ
{
    partial class ImageEditForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.MetadataPanel = new System.Windows.Forms.Panel();
            this.MetaDataHeaderPanel = new System.Windows.Forms.TableLayoutPanel();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.MetaDataTableHeaderActionButtonsPanel = new System.Windows.Forms.TableLayoutPanel();
            this.btnRemoveMetaData = new System.Windows.Forms.Button();
            this.btnAddMetaData = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.MetaDataTableActionsPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.AddMetaDataRowBtn = new System.Windows.Forms.Button();
            this.RemoveMetadataRowBtn = new System.Windows.Forms.Button();
            this.metaDataGrid = new System.Windows.Forms.DataGridView();
            this.MetadataPanel.SuspendLayout();
            this.MetaDataHeaderPanel.SuspendLayout();
            this.MetaDataTableHeaderActionButtonsPanel.SuspendLayout();
            this.MetaDataTableActionsPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.metaDataGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // MetadataPanel
            // 
            this.MetadataPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MetadataPanel.AutoScroll = true;
            this.MetadataPanel.Controls.Add(this.MetaDataHeaderPanel);
            this.MetadataPanel.Controls.Add(this.MetaDataTableActionsPanel);
            this.MetadataPanel.Controls.Add(this.metaDataGrid);
            this.MetadataPanel.Location = new System.Drawing.Point(197, 210);
            this.MetadataPanel.Name = "MetadataPanel";
            this.MetadataPanel.Size = new System.Drawing.Size(709, 348);
            this.MetadataPanel.TabIndex = 10;
            // 
            // MetaDataHeaderPanel
            // 
            this.MetaDataHeaderPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MetaDataHeaderPanel.AutoSize = true;
            this.MetaDataHeaderPanel.ColumnCount = 3;
            this.MetaDataHeaderPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 13.35227F));
            this.MetaDataHeaderPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 86.64773F));
            this.MetaDataHeaderPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 111F));
            this.MetaDataHeaderPanel.Controls.Add(this.comboBox1, 1, 0);
            this.MetaDataHeaderPanel.Controls.Add(this.MetaDataTableHeaderActionButtonsPanel, 2, 0);
            this.MetaDataHeaderPanel.Controls.Add(this.label1, 0, 0);
            this.MetaDataHeaderPanel.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.FixedSize;
            this.MetaDataHeaderPanel.Location = new System.Drawing.Point(6, 9);
            this.MetaDataHeaderPanel.MaximumSize = new System.Drawing.Size(0, 35);
            this.MetaDataHeaderPanel.Name = "MetaDataHeaderPanel";
            this.MetaDataHeaderPanel.RowCount = 1;
            this.MetaDataHeaderPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.MetaDataHeaderPanel.Size = new System.Drawing.Size(635, 35);
            this.MetaDataHeaderPanel.TabIndex = 13;
            // 
            // comboBox1
            // 
            this.comboBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "ComicInfo.xml"});
            this.comboBox1.Location = new System.Drawing.Point(72, 3);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(448, 24);
            this.comboBox1.TabIndex = 2;
            this.comboBox1.Text = "ComicInfo.xml";
            // 
            // MetaDataTableHeaderActionButtonsPanel
            // 
            this.MetaDataTableHeaderActionButtonsPanel.ColumnCount = 2;
            this.MetaDataTableHeaderActionButtonsPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.MetaDataTableHeaderActionButtonsPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.MetaDataTableHeaderActionButtonsPanel.Controls.Add(this.btnRemoveMetaData, 0, 0);
            this.MetaDataTableHeaderActionButtonsPanel.Controls.Add(this.btnAddMetaData, 0, 0);
            this.MetaDataTableHeaderActionButtonsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MetaDataTableHeaderActionButtonsPanel.Location = new System.Drawing.Point(523, 0);
            this.MetaDataTableHeaderActionButtonsPanel.Margin = new System.Windows.Forms.Padding(0);
            this.MetaDataTableHeaderActionButtonsPanel.Name = "MetaDataTableHeaderActionButtonsPanel";
            this.MetaDataTableHeaderActionButtonsPanel.RowCount = 1;
            this.MetaDataTableHeaderActionButtonsPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.MetaDataTableHeaderActionButtonsPanel.Size = new System.Drawing.Size(112, 35);
            this.MetaDataTableHeaderActionButtonsPanel.TabIndex = 8;
            // 
            // btnRemoveMetaData
            // 
            this.btnRemoveMetaData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnRemoveMetaData.Enabled = false;
            this.btnRemoveMetaData.Image = global::MyCBZ.Properties.Resources.delete2;
            this.btnRemoveMetaData.Location = new System.Drawing.Point(59, 3);
            this.btnRemoveMetaData.Name = "btnRemoveMetaData";
            this.btnRemoveMetaData.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnRemoveMetaData.Size = new System.Drawing.Size(50, 29);
            this.btnRemoveMetaData.TabIndex = 8;
            this.btnRemoveMetaData.UseVisualStyleBackColor = true;
            // 
            // btnAddMetaData
            // 
            this.btnAddMetaData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnAddMetaData.Image = global::MyCBZ.Properties.Resources.add2;
            this.btnAddMetaData.Location = new System.Drawing.Point(3, 3);
            this.btnAddMetaData.Name = "btnAddMetaData";
            this.btnAddMetaData.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnAddMetaData.Size = new System.Drawing.Size(50, 29);
            this.btnAddMetaData.TabIndex = 7;
            this.btnAddMetaData.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 1);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 32);
            this.label1.TabIndex = 1;
            this.label1.Text = "Metadata";
            // 
            // MetaDataTableActionsPanel
            // 
            this.MetaDataTableActionsPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MetaDataTableActionsPanel.AutoSize = true;
            this.MetaDataTableActionsPanel.Controls.Add(this.AddMetaDataRowBtn);
            this.MetaDataTableActionsPanel.Controls.Add(this.RemoveMetadataRowBtn);
            this.MetaDataTableActionsPanel.Location = new System.Drawing.Point(0, 197);
            this.MetaDataTableActionsPanel.Name = "MetaDataTableActionsPanel";
            this.MetaDataTableActionsPanel.Size = new System.Drawing.Size(446, 50);
            this.MetaDataTableActionsPanel.TabIndex = 12;
            // 
            // AddMetaDataRowBtn
            // 
            this.AddMetaDataRowBtn.Enabled = false;
            this.AddMetaDataRowBtn.Image = global::MyCBZ.Properties.Resources.add2;
            this.AddMetaDataRowBtn.Location = new System.Drawing.Point(3, 3);
            this.AddMetaDataRowBtn.Name = "AddMetaDataRowBtn";
            this.AddMetaDataRowBtn.Size = new System.Drawing.Size(46, 44);
            this.AddMetaDataRowBtn.TabIndex = 0;
            this.AddMetaDataRowBtn.UseVisualStyleBackColor = true;
            // 
            // RemoveMetadataRowBtn
            // 
            this.RemoveMetadataRowBtn.Enabled = false;
            this.RemoveMetadataRowBtn.Image = global::MyCBZ.Properties.Resources.delete2;
            this.RemoveMetadataRowBtn.Location = new System.Drawing.Point(55, 3);
            this.RemoveMetadataRowBtn.Name = "RemoveMetadataRowBtn";
            this.RemoveMetadataRowBtn.Size = new System.Drawing.Size(47, 44);
            this.RemoveMetadataRowBtn.TabIndex = 1;
            this.RemoveMetadataRowBtn.UseVisualStyleBackColor = true;
            // 
            // metaDataGrid
            // 
            this.metaDataGrid.AllowUserToOrderColumns = true;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.metaDataGrid.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.metaDataGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.metaDataGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.metaDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.metaDataGrid.DefaultCellStyle = dataGridViewCellStyle3;
            this.metaDataGrid.Location = new System.Drawing.Point(3, 50);
            this.metaDataGrid.Name = "metaDataGrid";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.metaDataGrid.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.metaDataGrid.RowHeadersWidth = 51;
            this.metaDataGrid.RowTemplate.Height = 24;
            this.metaDataGrid.Size = new System.Drawing.Size(443, 141);
            this.metaDataGrid.TabIndex = 11;
            // 
            // ImageEditForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1036, 706);
            this.Controls.Add(this.MetadataPanel);
            this.Name = "ImageEditForm";
            this.Text = "Edit Page";
            this.MetadataPanel.ResumeLayout(false);
            this.MetadataPanel.PerformLayout();
            this.MetaDataHeaderPanel.ResumeLayout(false);
            this.MetaDataHeaderPanel.PerformLayout();
            this.MetaDataTableHeaderActionButtonsPanel.ResumeLayout(false);
            this.MetaDataTableActionsPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.metaDataGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel MetadataPanel;
        private System.Windows.Forms.TableLayoutPanel MetaDataHeaderPanel;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.TableLayoutPanel MetaDataTableHeaderActionButtonsPanel;
        private System.Windows.Forms.Button btnRemoveMetaData;
        private System.Windows.Forms.Button btnAddMetaData;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.FlowLayoutPanel MetaDataTableActionsPanel;
        private System.Windows.Forms.Button AddMetaDataRowBtn;
        private System.Windows.Forms.Button RemoveMetadataRowBtn;
        private System.Windows.Forms.DataGridView metaDataGrid;
    }
}