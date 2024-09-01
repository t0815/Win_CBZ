namespace Win_CBZ.Forms
{
    partial class DebugForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DebugForm));
            button1 = new System.Windows.Forms.Button();
            openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            listView1 = new System.Windows.Forms.ListView();
            columnHeader1 = new System.Windows.Forms.ColumnHeader();
            columnHeader2 = new System.Windows.Forms.ColumnHeader();
            columnHeader3 = new System.Windows.Forms.ColumnHeader();
            columnHeader4 = new System.Windows.Forms.ColumnHeader();
            columnHeader5 = new System.Windows.Forms.ColumnHeader();
            listView2 = new System.Windows.Forms.ListView();
            columnHeader6 = new System.Windows.Forms.ColumnHeader();
            columnHeader7 = new System.Windows.Forms.ColumnHeader();
            columnHeader8 = new System.Windows.Forms.ColumnHeader();
            columnHeader9 = new System.Windows.Forms.ColumnHeader();
            button2 = new System.Windows.Forms.Button();
            button3 = new System.Windows.Forms.Button();
            autocompleteMenu1 = new AutocompleteMenuNS.AutocompleteMenu();
            cdg = new Components.CustomDataGridView.CustomDataGridView();
            scintilla1 = new ScintillaNET.Scintilla();
            progressBar1 = new System.Windows.Forms.ProgressBar();
            ((System.ComponentModel.ISupportInitialize)cdg).BeginInit();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new System.Drawing.Point(13, 18);
            button1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            button1.Name = "button1";
            button1.Size = new System.Drawing.Size(100, 35);
            button1.TabIndex = 0;
            button1.Text = "button1";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // openFileDialog1
            // 
            openFileDialog1.FileName = "openFileDialog1";
            // 
            // listView1
            // 
            listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { columnHeader1, columnHeader2, columnHeader3, columnHeader4, columnHeader5 });
            listView1.Location = new System.Drawing.Point(16, 82);
            listView1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            listView1.Name = "listView1";
            listView1.Size = new System.Drawing.Size(566, 392);
            listView1.TabIndex = 1;
            listView1.UseCompatibleStateImageBehavior = false;
            listView1.View = System.Windows.Forms.View.Details;
            listView1.SelectedIndexChanged += listView1_SelectedIndexChanged;
            // 
            // columnHeader1
            // 
            columnHeader1.Width = 138;
            // 
            // columnHeader2
            // 
            columnHeader2.Width = 119;
            // 
            // columnHeader3
            // 
            columnHeader3.Width = 92;
            // 
            // columnHeader4
            // 
            columnHeader4.Width = 94;
            // 
            // columnHeader5
            // 
            columnHeader5.Width = 94;
            // 
            // listView2
            // 
            listView2.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { columnHeader6, columnHeader7, columnHeader8, columnHeader9 });
            listView2.Location = new System.Drawing.Point(615, 84);
            listView2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            listView2.Name = "listView2";
            listView2.Size = new System.Drawing.Size(411, 389);
            listView2.TabIndex = 2;
            listView2.UseCompatibleStateImageBehavior = false;
            listView2.View = System.Windows.Forms.View.Details;
            listView2.SelectedIndexChanged += listView2_SelectedIndexChanged;
            // 
            // columnHeader6
            // 
            columnHeader6.Width = 100;
            // 
            // columnHeader7
            // 
            columnHeader7.Width = 100;
            // 
            // columnHeader8
            // 
            columnHeader8.Width = 86;
            // 
            // columnHeader9
            // 
            columnHeader9.Width = 73;
            // 
            // button2
            // 
            button2.Location = new System.Drawing.Point(140, 18);
            button2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            button2.Name = "button2";
            button2.Size = new System.Drawing.Size(83, 35);
            button2.TabIndex = 3;
            button2.Text = "button2";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button3
            // 
            button3.Location = new System.Drawing.Point(615, 504);
            button3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            button3.Name = "button3";
            button3.Size = new System.Drawing.Size(134, 50);
            button3.TabIndex = 5;
            button3.Text = "add row...";
            button3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // autocompleteMenu1
            // 
            autocompleteMenu1.Colors = (AutocompleteMenuNS.Colors)resources.GetObject("autocompleteMenu1.Colors");
            autocompleteMenu1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            autocompleteMenu1.ImageList = null;
            autocompleteMenu1.MinFragmentLength = 1;
            autocompleteMenu1.SearchPattern = "[^\\w\\+]";
            autocompleteMenu1.TargetControlWrapper = null;
            autocompleteMenu1.Selected += autocompleteMenu1_Selected;
            // 
            // cdg
            // 
            cdg.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            cdg.Location = new System.Drawing.Point(16, 482);
            cdg.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            cdg.Name = "cdg";
            cdg.RowHeadersWidth = 62;
            cdg.RowTemplate.Height = 28;
            cdg.Size = new System.Drawing.Size(565, 181);
            cdg.TabIndex = 4;
            cdg.EditingControlShowing += cdg_EditingControlShowing;
            // 
            // scintilla1
            // 
            scintilla1.AutocompleteListSelectedBackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            scintilla1.LexerName = null;
            scintilla1.Location = new System.Drawing.Point(777, 513);
            scintilla1.Name = "scintilla1";
            scintilla1.ScrollWidth = 57;
            scintilla1.Size = new System.Drawing.Size(250, 125);
            scintilla1.TabIndex = 7;
            scintilla1.Text = "scintilla1";
            // 
            // progressBar1
            // 
            progressBar1.Location = new System.Drawing.Point(352, 31);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new System.Drawing.Size(125, 29);
            progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            progressBar1.TabIndex = 8;
            progressBar1.Value = 50;
            // 
            // DebugForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1040, 692);
            Controls.Add(progressBar1);
            Controls.Add(scintilla1);
            Controls.Add(button3);
            Controls.Add(cdg);
            Controls.Add(button2);
            Controls.Add(listView2);
            Controls.Add(listView1);
            Controls.Add(button1);
            Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            Name = "DebugForm";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)cdg).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        public System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        public System.Windows.Forms.ListView listView2;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.ColumnHeader columnHeader8;
        private System.Windows.Forms.ColumnHeader columnHeader9;
        private System.Windows.Forms.Button button2;
        private Components.CustomDataGridView.CustomDataGridView cdg;
        private System.Windows.Forms.Button button3;
        private AutocompleteMenuNS.AutocompleteMenu autocompleteMenu1;
        private ScintillaNET.Scintilla scintilla1;
        private System.Windows.Forms.ProgressBar progressBar1;
    }
}