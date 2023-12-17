namespace Win_CBZ.Forms
{
    partial class ApplicationDialog
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
            this.ErrorDialogTablePanel = new System.Windows.Forms.TableLayoutPanel();
            this.DialogIconPictureBox = new System.Windows.Forms.PictureBox();
            this.MessageContainer = new System.Windows.Forms.Panel();
            this.TextBoxMessage = new System.Windows.Forms.TextBox();
            this.ErrorMessageTitle = new System.Windows.Forms.Label();
            this.ErrorDialogTablePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DialogIconPictureBox)).BeginInit();
            this.MessageContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // ErrorDialogTablePanel
            // 
            this.ErrorDialogTablePanel.AutoSize = true;
            this.ErrorDialogTablePanel.BackColor = System.Drawing.Color.White;
            this.ErrorDialogTablePanel.ColumnCount = 5;
            this.ErrorDialogTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.ErrorDialogTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ErrorDialogTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.ErrorDialogTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.ErrorDialogTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.ErrorDialogTablePanel.Controls.Add(this.DialogIconPictureBox, 0, 0);
            this.ErrorDialogTablePanel.Controls.Add(this.MessageContainer, 0, 1);
            this.ErrorDialogTablePanel.Controls.Add(this.ErrorMessageTitle, 1, 0);
            this.ErrorDialogTablePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ErrorDialogTablePanel.Location = new System.Drawing.Point(0, 0);
            this.ErrorDialogTablePanel.Margin = new System.Windows.Forms.Padding(0);
            this.ErrorDialogTablePanel.Name = "ErrorDialogTablePanel";
            this.ErrorDialogTablePanel.RowCount = 3;
            this.ErrorDialogTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.ErrorDialogTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ErrorDialogTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.ErrorDialogTablePanel.Size = new System.Drawing.Size(567, 340);
            this.ErrorDialogTablePanel.TabIndex = 0;
            // 
            // DialogIconPictureBox
            // 
            this.DialogIconPictureBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.DialogIconPictureBox.Location = new System.Drawing.Point(8, 8);
            this.DialogIconPictureBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.DialogIconPictureBox.Name = "DialogIconPictureBox";
            this.DialogIconPictureBox.Size = new System.Drawing.Size(64, 64);
            this.DialogIconPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.DialogIconPictureBox.TabIndex = 0;
            this.DialogIconPictureBox.TabStop = false;
            // 
            // MessageContainer
            // 
            this.MessageContainer.BackColor = System.Drawing.SystemColors.Control;
            this.MessageContainer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ErrorDialogTablePanel.SetColumnSpan(this.MessageContainer, 5);
            this.MessageContainer.Controls.Add(this.TextBoxMessage);
            this.MessageContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MessageContainer.Location = new System.Drawing.Point(3, 82);
            this.MessageContainer.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MessageContainer.Name = "MessageContainer";
            this.MessageContainer.Padding = new System.Windows.Forms.Padding(16);
            this.MessageContainer.Size = new System.Drawing.Size(561, 196);
            this.MessageContainer.TabIndex = 3;
            // 
            // TextBoxMessage
            // 
            this.TextBoxMessage.BackColor = System.Drawing.SystemColors.Control;
            this.TextBoxMessage.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.TextBoxMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TextBoxMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TextBoxMessage.Location = new System.Drawing.Point(16, 16);
            this.TextBoxMessage.Margin = new System.Windows.Forms.Padding(16);
            this.TextBoxMessage.Multiline = true;
            this.TextBoxMessage.Name = "TextBoxMessage";
            this.TextBoxMessage.ReadOnly = true;
            this.TextBoxMessage.Size = new System.Drawing.Size(527, 162);
            this.TextBoxMessage.TabIndex = 99;
            this.TextBoxMessage.Text = "[Error Message]";
            // 
            // ErrorMessageTitle
            // 
            this.ErrorMessageTitle.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.ErrorMessageTitle.AutoSize = true;
            this.ErrorDialogTablePanel.SetColumnSpan(this.ErrorMessageTitle, 4);
            this.ErrorMessageTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ErrorMessageTitle.Location = new System.Drawing.Point(83, 26);
            this.ErrorMessageTitle.Name = "ErrorMessageTitle";
            this.ErrorMessageTitle.Size = new System.Drawing.Size(151, 28);
            this.ErrorMessageTitle.TabIndex = 4;
            this.ErrorMessageTitle.Text = "[MessageTitle]";
            this.ErrorMessageTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ApplicationDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(567, 340);
            this.Controls.Add(this.ErrorDialogTablePanel);
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "ApplicationDialog";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Application Error";
            this.TopMost = true;
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ApplicationDialog_KeyDown);
            this.ErrorDialogTablePanel.ResumeLayout(false);
            this.ErrorDialogTablePanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DialogIconPictureBox)).EndInit();
            this.MessageContainer.ResumeLayout(false);
            this.MessageContainer.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel ErrorDialogTablePanel;
        private System.Windows.Forms.PictureBox DialogIconPictureBox;
        private System.Windows.Forms.Panel MessageContainer;
        private System.Windows.Forms.TextBox TextBoxMessage;
        private System.Windows.Forms.Label ErrorMessageTitle;
    }
}