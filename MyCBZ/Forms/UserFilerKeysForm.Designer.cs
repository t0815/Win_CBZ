using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Win_CBZ.Forms
{
    partial class UserFilerKeysForm
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
            components = new Container();
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            ComponentResourceManager resources = new ComponentResourceManager(typeof(UserFilerKeysForm));
            ItemEditorTableLayout = new TableLayoutPanel();
            panel1 = new Panel();
            TextBoxKey = new TextBox();
            ButtonAddKey = new Button();
            HeaderPanel = new Panel();
            HeaderLabel = new Label();
            HeaderPicture = new PictureBox();
            label1 = new Label();
            flowLayoutPanel1 = new FlowLayoutPanel();
            ItemEditorToolBar = new ToolStrip();
            ToolStripButtonRemoveSelectedKeys = new ToolStripButton();
            DeleteAllTagsToolButton = new ToolStripButton();
            toolStripSeparator1 = new ToolStripSeparator();
            ToolButtonSortAscending = new ToolStripButton();
            DatagridUserKeyFilter = new DataGridView();
            pictureBox2 = new PictureBox();
            ComboBoxCondition = new ComboBox();
            label2 = new Label();
            CancelBtn = new Button();
            OkButton = new Button();
            AutoCompleteDefaultKeys = new AutocompleteMenuNS.AutocompleteMenu();
            ToolTip = new ToolTip(components);
            ComboIcons = new ImageList(components);
            ItemEditorTableLayout.SuspendLayout();
            panel1.SuspendLayout();
            HeaderPanel.SuspendLayout();
            ((ISupportInitialize)HeaderPicture).BeginInit();
            flowLayoutPanel1.SuspendLayout();
            ItemEditorToolBar.SuspendLayout();
            ((ISupportInitialize)DatagridUserKeyFilter).BeginInit();
            ((ISupportInitialize)pictureBox2).BeginInit();
            SuspendLayout();
            // 
            // ItemEditorTableLayout
            // 
            ItemEditorTableLayout.ColumnCount = 5;
            ItemEditorTableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 12F));
            ItemEditorTableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 66.6666641F));
            ItemEditorTableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            ItemEditorTableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 107F));
            ItemEditorTableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 15F));
            ItemEditorTableLayout.Controls.Add(panel1, 1, 2);
            ItemEditorTableLayout.Controls.Add(HeaderPanel, 0, 0);
            ItemEditorTableLayout.Controls.Add(label1, 1, 1);
            ItemEditorTableLayout.Controls.Add(flowLayoutPanel1, 3, 2);
            ItemEditorTableLayout.Controls.Add(DatagridUserKeyFilter, 1, 3);
            ItemEditorTableLayout.Controls.Add(pictureBox2, 1, 6);
            ItemEditorTableLayout.Controls.Add(ComboBoxCondition, 1, 5);
            ItemEditorTableLayout.Controls.Add(label2, 1, 4);
            ItemEditorTableLayout.Controls.Add(CancelBtn, 3, 7);
            ItemEditorTableLayout.Controls.Add(OkButton, 2, 7);
            ItemEditorTableLayout.Dock = DockStyle.Fill;
            ItemEditorTableLayout.Location = new Point(0, 0);
            ItemEditorTableLayout.Margin = new Padding(4, 5, 4, 5);
            ItemEditorTableLayout.Name = "ItemEditorTableLayout";
            ItemEditorTableLayout.RowCount = 8;
            ItemEditorTableLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 64F));
            ItemEditorTableLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 35F));
            ItemEditorTableLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 29F));
            ItemEditorTableLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            ItemEditorTableLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 46F));
            ItemEditorTableLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 39F));
            ItemEditorTableLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 52F));
            ItemEditorTableLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 55F));
            ItemEditorTableLayout.Size = new Size(482, 479);
            ItemEditorTableLayout.TabIndex = 2;
            // 
            // panel1
            // 
            panel1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            panel1.BackColor = SystemColors.ControlLightLight;
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel1.Controls.Add(TextBoxKey);
            panel1.Controls.Add(ButtonAddKey);
            panel1.Location = new Point(12, 99);
            panel1.Margin = new Padding(0, 0, 3, 2);
            panel1.Name = "panel1";
            panel1.Size = new Size(229, 27);
            panel1.TabIndex = 28;
            // 
            // TextBoxKey
            // 
            TextBoxKey.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            AutoCompleteDefaultKeys.SetAutocompleteMenu(TextBoxKey, AutoCompleteDefaultKeys);
            TextBoxKey.BorderStyle = BorderStyle.None;
            TextBoxKey.Location = new Point(4, 3);
            TextBoxKey.Margin = new Padding(3, 2, 3, 2);
            TextBoxKey.Name = "TextBoxKey";
            TextBoxKey.Size = new Size(197, 20);
            TextBoxKey.TabIndex = 22;
            TextBoxKey.KeyDown += TextBoxKey_KeyDown;
            // 
            // ButtonAddKey
            // 
            ButtonAddKey.Dock = DockStyle.Right;
            ButtonAddKey.FlatStyle = FlatStyle.Flat;
            ButtonAddKey.Image = Properties.Resources.navigate_plus;
            ButtonAddKey.Location = new Point(202, 0);
            ButtonAddKey.Margin = new Padding(3, 2, 3, 2);
            ButtonAddKey.Name = "ButtonAddKey";
            ButtonAddKey.Size = new Size(25, 25);
            ButtonAddKey.TabIndex = 22;
            ButtonAddKey.UseVisualStyleBackColor = true;
            ButtonAddKey.Click += ButtonAddKey_Click;
            // 
            // HeaderPanel
            // 
            HeaderPanel.BackColor = Color.White;
            ItemEditorTableLayout.SetColumnSpan(HeaderPanel, 5);
            HeaderPanel.Controls.Add(HeaderLabel);
            HeaderPanel.Controls.Add(HeaderPicture);
            HeaderPanel.Dock = DockStyle.Fill;
            HeaderPanel.Location = new Point(3, 0);
            HeaderPanel.Margin = new Padding(3, 0, 3, 2);
            HeaderPanel.Name = "HeaderPanel";
            HeaderPanel.Size = new Size(476, 62);
            HeaderPanel.TabIndex = 2;
            // 
            // HeaderLabel
            // 
            HeaderLabel.AutoSize = true;
            HeaderLabel.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            HeaderLabel.Location = new Point(95, 18);
            HeaderLabel.Name = "HeaderLabel";
            HeaderLabel.Size = new Size(177, 28);
            HeaderLabel.TabIndex = 1;
            HeaderLabel.Text = "Manage Key Filters";
            // 
            // HeaderPicture
            // 
            HeaderPicture.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            HeaderPicture.BackColor = Color.Transparent;
            HeaderPicture.BackgroundImageLayout = ImageLayout.Center;
            HeaderPicture.Image = Properties.Resources.funnel_gears_;
            HeaderPicture.InitialImage = null;
            HeaderPicture.Location = new Point(24, 5);
            HeaderPicture.Margin = new Padding(3, 2, 3, 2);
            HeaderPicture.Name = "HeaderPicture";
            HeaderPicture.Size = new Size(53, 54);
            HeaderPicture.SizeMode = PictureBoxSizeMode.CenterImage;
            HeaderPicture.TabIndex = 0;
            HeaderPicture.TabStop = false;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            label1.AutoSize = true;
            label1.Location = new Point(15, 79);
            label1.Name = "label1";
            label1.Size = new Size(65, 20);
            label1.TabIndex = 8;
            label1.Text = "Add Key";
            // 
            // flowLayoutPanel1
            // 
            ItemEditorTableLayout.SetColumnSpan(flowLayoutPanel1, 2);
            flowLayoutPanel1.Controls.Add(ItemEditorToolBar);
            flowLayoutPanel1.Location = new Point(360, 99);
            flowLayoutPanel1.Margin = new Padding(0);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(122, 29);
            flowLayoutPanel1.TabIndex = 9;
            // 
            // ItemEditorToolBar
            // 
            ItemEditorToolBar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            ItemEditorToolBar.Dock = DockStyle.None;
            ItemEditorToolBar.GripStyle = ToolStripGripStyle.Hidden;
            ItemEditorToolBar.ImageScalingSize = new Size(20, 20);
            ItemEditorToolBar.Items.AddRange(new ToolStripItem[] { ToolStripButtonRemoveSelectedKeys, DeleteAllTagsToolButton, toolStripSeparator1, ToolButtonSortAscending });
            ItemEditorToolBar.Location = new Point(8, 0);
            ItemEditorToolBar.Margin = new Padding(8, 0, 8, 2);
            ItemEditorToolBar.Name = "ItemEditorToolBar";
            ItemEditorToolBar.RenderMode = ToolStripRenderMode.System;
            ItemEditorToolBar.Size = new Size(96, 27);
            ItemEditorToolBar.Stretch = true;
            ItemEditorToolBar.TabIndex = 8;
            ItemEditorToolBar.Text = "toolStrip1";
            // 
            // ToolStripButtonRemoveSelectedKeys
            // 
            ToolStripButtonRemoveSelectedKeys.DisplayStyle = ToolStripItemDisplayStyle.Image;
            ToolStripButtonRemoveSelectedKeys.Image = Properties.Resources.delete;
            ToolStripButtonRemoveSelectedKeys.ImageTransparentColor = Color.Magenta;
            ToolStripButtonRemoveSelectedKeys.Name = "ToolStripButtonRemoveSelectedKeys";
            ToolStripButtonRemoveSelectedKeys.Size = new Size(29, 24);
            ToolStripButtonRemoveSelectedKeys.Text = "Remove selected";
            ToolStripButtonRemoveSelectedKeys.Click += ToolStripButtonRemoveSelectedKeys_Click;
            // 
            // DeleteAllTagsToolButton
            // 
            DeleteAllTagsToolButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
            DeleteAllTagsToolButton.Image = Properties.Resources.garbage;
            DeleteAllTagsToolButton.ImageTransparentColor = Color.Magenta;
            DeleteAllTagsToolButton.Name = "DeleteAllTagsToolButton";
            DeleteAllTagsToolButton.Size = new Size(29, 24);
            DeleteAllTagsToolButton.Text = "Clear all";
            DeleteAllTagsToolButton.Click += DeleteAllTagsToolButton_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(6, 27);
            // 
            // ToolButtonSortAscending
            // 
            ToolButtonSortAscending.DisplayStyle = ToolStripItemDisplayStyle.Image;
            ToolButtonSortAscending.Enabled = false;
            ToolButtonSortAscending.Image = Properties.Resources.sort_az_ascending2;
            ToolButtonSortAscending.ImageTransparentColor = Color.Magenta;
            ToolButtonSortAscending.Name = "ToolButtonSortAscending";
            ToolButtonSortAscending.Size = new Size(29, 24);
            ToolButtonSortAscending.Text = "toolStripButton1";
            ToolButtonSortAscending.ToolTipText = "Sort items ascending";
            // 
            // DatagridUserKeyFilter
            // 
            DatagridUserKeyFilter.AllowUserToAddRows = false;
            DatagridUserKeyFilter.AllowUserToResizeRows = false;
            DatagridUserKeyFilter.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            DatagridUserKeyFilter.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            ItemEditorTableLayout.SetColumnSpan(DatagridUserKeyFilter, 3);
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Window;
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle1.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = Color.Gold;
            dataGridViewCellStyle1.SelectionForeColor = Color.Black;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.False;
            DatagridUserKeyFilter.DefaultCellStyle = dataGridViewCellStyle1;
            DatagridUserKeyFilter.Location = new Point(15, 131);
            DatagridUserKeyFilter.Name = "DatagridUserKeyFilter";
            DatagridUserKeyFilter.RowHeadersWidth = 51;
            DatagridUserKeyFilter.SelectionMode = DataGridViewSelectionMode.CellSelect;
            DatagridUserKeyFilter.Size = new Size(449, 153);
            DatagridUserKeyFilter.TabIndex = 29;
            // 
            // pictureBox2
            // 
            pictureBox2.Image = Properties.Resources.information;
            pictureBox2.InitialImage = Properties.Resources.information;
            pictureBox2.Location = new Point(16, 372);
            pictureBox2.Margin = new Padding(4, 0, 0, 0);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(30, 43);
            pictureBox2.SizeMode = PictureBoxSizeMode.CenterImage;
            pictureBox2.TabIndex = 32;
            pictureBox2.TabStop = false;
            ToolTip.SetToolTip(pictureBox2, "Autocomplete Editor. Start typing and accept an item by pressing enter.");
            // 
            // ComboBoxCondition
            // 
            ComboBoxCondition.DrawMode = DrawMode.OwnerDrawFixed;
            ComboBoxCondition.DropDownStyle = ComboBoxStyle.DropDownList;
            ComboBoxCondition.FormattingEnabled = true;
            ComboBoxCondition.Items.AddRange(new object[] { "Include All", "Exclude All" });
            ComboBoxCondition.Location = new Point(15, 336);
            ComboBoxCondition.Name = "ComboBoxCondition";
            ComboBoxCondition.Size = new Size(226, 28);
            ComboBoxCondition.TabIndex = 33;
            ComboBoxCondition.DrawItem += ComboBoxCondition_DrawItem;
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            label2.AutoSize = true;
            label2.Location = new Point(15, 313);
            label2.Name = "label2";
            label2.Size = new Size(74, 20);
            label2.TabIndex = 34;
            label2.Text = "Condition";
            // 
            // CancelBtn
            // 
            CancelBtn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            CancelBtn.DialogResult = DialogResult.Cancel;
            CancelBtn.Location = new Point(364, 429);
            CancelBtn.Margin = new Padding(4, 5, 4, 5);
            CancelBtn.Name = "CancelBtn";
            CancelBtn.Size = new Size(99, 35);
            CancelBtn.TabIndex = 4;
            CancelBtn.Text = "Cancel";
            CancelBtn.UseVisualStyleBackColor = true;
            // 
            // OkButton
            // 
            OkButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            OkButton.DialogResult = DialogResult.OK;
            OkButton.Location = new Point(256, 429);
            OkButton.Margin = new Padding(4, 5, 4, 5);
            OkButton.Name = "OkButton";
            OkButton.Size = new Size(100, 35);
            OkButton.TabIndex = 3;
            OkButton.Text = "Ok";
            OkButton.UseVisualStyleBackColor = true;
            OkButton.Click += OkButton_Click;
            // 
            // AutoCompleteDefaultKeys
            // 
            AutoCompleteDefaultKeys.Colors = (AutocompleteMenuNS.Colors)resources.GetObject("AutoCompleteDefaultKeys.Colors");
            AutoCompleteDefaultKeys.Font = new Font("Microsoft Sans Serif", 9F);
            AutoCompleteDefaultKeys.ImageList = null;
            AutoCompleteDefaultKeys.MaximumSize = new Size(319, 200);
            AutoCompleteDefaultKeys.MinFragmentLength = 1;
            AutoCompleteDefaultKeys.TargetControlWrapper = null;
            // 
            // ToolTip
            // 
            ToolTip.IsBalloon = true;
            ToolTip.ToolTipIcon = ToolTipIcon.Info;
            ToolTip.ToolTipTitle = "Win_CBZ";
            // 
            // ComboIcons
            // 
            ComboIcons.ColorDepth = ColorDepth.Depth32Bit;
            ComboIcons.ImageStream = (ImageListStreamer)resources.GetObject("ComboIcons.ImageStream");
            ComboIcons.TransparentColor = Color.Transparent;
            ComboIcons.Images.SetKeyName(0, "hash");
            // 
            // UserFilerKeysForm
            // 
            ClientSize = new Size(482, 479);
            Controls.Add(ItemEditorTableLayout);
            FormBorderStyle = FormBorderStyle.SizableToolWindow;
            Name = "UserFilerKeysForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Key User Filter";
            FormClosing += UserFilerKeysForm_FormClosing;
            Shown += UserFilerKeysForm_Shown;
            ItemEditorTableLayout.ResumeLayout(false);
            ItemEditorTableLayout.PerformLayout();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            HeaderPanel.ResumeLayout(false);
            HeaderPanel.PerformLayout();
            ((ISupportInitialize)HeaderPicture).EndInit();
            flowLayoutPanel1.ResumeLayout(false);
            flowLayoutPanel1.PerformLayout();
            ItemEditorToolBar.ResumeLayout(false);
            ItemEditorToolBar.PerformLayout();
            ((ISupportInitialize)DatagridUserKeyFilter).EndInit();
            ((ISupportInitialize)pictureBox2).EndInit();
            ResumeLayout(false);
        }

        private TableLayoutPanel ItemEditorTableLayout;
        private Button CancelBtn;
        private Button OkButton;
        private Panel HeaderPanel;
        private Label HeaderLabel;
        private PictureBox HeaderPicture;
        private FlowLayoutPanel flowLayoutPanel1;
        private Panel panel1;
        private TextBox TextBoxKey;
        private Button ButtonAddKey;
        private DataGridView DatagridUserKeyFilter;
        private ImageList imageList1;
        private Label label1;

        #endregion

        private AutocompleteMenuNS.AutocompleteMenu AutoCompleteDefaultKeys;
        private PictureBox pictureBox2;
        private ToolStrip ItemEditorToolBar;
        private ToolStripButton ToolStripButtonRemoveSelectedKeys;
        private ToolStripButton DeleteAllTagsToolButton;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripButton ToolButtonSortAscending;
        private ComboBox ComboBoxCondition;
        private Label label2;
        private ToolTip ToolTip;
        private ImageList ComboIcons;
    }
}