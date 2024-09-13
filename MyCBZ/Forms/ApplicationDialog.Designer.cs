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
            ErrorDialogTablePanel = new System.Windows.Forms.TableLayoutPanel();
            DialogIconPictureBox = new System.Windows.Forms.PictureBox();
            MessageContainer = new System.Windows.Forms.Panel();
            TextBoxMessage = new System.Windows.Forms.TextBox();
            ErrorMessageTitle = new System.Windows.Forms.Label();
            ErrorDialogTablePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)DialogIconPictureBox).BeginInit();
            MessageContainer.SuspendLayout();
            SuspendLayout();
            // 
            // ErrorDialogTablePanel
            // 
            ErrorDialogTablePanel.AutoSize = true;
            ErrorDialogTablePanel.BackColor = System.Drawing.Color.White;
            ErrorDialogTablePanel.ColumnCount = 5;
            ErrorDialogTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            ErrorDialogTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            ErrorDialogTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            ErrorDialogTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            ErrorDialogTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            ErrorDialogTablePanel.Controls.Add(DialogIconPictureBox, 0, 0);
            ErrorDialogTablePanel.Controls.Add(MessageContainer, 0, 1);
            ErrorDialogTablePanel.Controls.Add(ErrorMessageTitle, 1, 0);
            ErrorDialogTablePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            ErrorDialogTablePanel.Location = new System.Drawing.Point(0, 0);
            ErrorDialogTablePanel.Margin = new System.Windows.Forms.Padding(0);
            ErrorDialogTablePanel.Name = "ErrorDialogTablePanel";
            ErrorDialogTablePanel.Padding = new System.Windows.Forms.Padding(0, 0, 0, 4);
            ErrorDialogTablePanel.RowCount = 3;
            ErrorDialogTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 78F));
            ErrorDialogTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            ErrorDialogTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 56F));
            ErrorDialogTablePanel.Size = new System.Drawing.Size(567, 425);
            ErrorDialogTablePanel.TabIndex = 0;
            // 
            // DialogIconPictureBox
            // 
            DialogIconPictureBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            DialogIconPictureBox.Location = new System.Drawing.Point(8, 2);
            DialogIconPictureBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            DialogIconPictureBox.Name = "DialogIconPictureBox";
            DialogIconPictureBox.Size = new System.Drawing.Size(64, 74);
            DialogIconPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            DialogIconPictureBox.TabIndex = 0;
            DialogIconPictureBox.TabStop = false;
            // 
            // MessageContainer
            // 
            MessageContainer.BackColor = System.Drawing.SystemColors.Control;
            MessageContainer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            ErrorDialogTablePanel.SetColumnSpan(MessageContainer, 5);
            MessageContainer.Controls.Add(TextBoxMessage);
            MessageContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            MessageContainer.Location = new System.Drawing.Point(3, 80);
            MessageContainer.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            MessageContainer.Name = "MessageContainer";
            MessageContainer.Padding = new System.Windows.Forms.Padding(16, 20, 16, 20);
            MessageContainer.Size = new System.Drawing.Size(561, 283);
            MessageContainer.TabIndex = 3;
            // 
            // TextBoxMessage
            // 
            TextBoxMessage.BackColor = System.Drawing.SystemColors.Control;
            TextBoxMessage.BorderStyle = System.Windows.Forms.BorderStyle.None;
            TextBoxMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            TextBoxMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            TextBoxMessage.Location = new System.Drawing.Point(16, 20);
            TextBoxMessage.Margin = new System.Windows.Forms.Padding(16, 20, 16, 20);
            TextBoxMessage.Multiline = true;
            TextBoxMessage.Name = "TextBoxMessage";
            TextBoxMessage.ReadOnly = true;
            TextBoxMessage.Size = new System.Drawing.Size(527, 241);
            TextBoxMessage.TabIndex = 99;
            TextBoxMessage.Text = "[Error Message]";
            // 
            // ErrorMessageTitle
            // 
            ErrorMessageTitle.Anchor = System.Windows.Forms.AnchorStyles.Left;
            ErrorMessageTitle.AutoSize = true;
            ErrorDialogTablePanel.SetColumnSpan(ErrorMessageTitle, 4);
            ErrorMessageTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            ErrorMessageTitle.Location = new System.Drawing.Point(83, 25);
            ErrorMessageTitle.Name = "ErrorMessageTitle";
            ErrorMessageTitle.Size = new System.Drawing.Size(151, 28);
            ErrorMessageTitle.TabIndex = 4;
            ErrorMessageTitle.Text = "[MessageTitle]";
            ErrorMessageTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ApplicationDialog
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(567, 425);
            Controls.Add(ErrorDialogTablePanel);
            Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            Name = "ApplicationDialog";
            ShowIcon = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Application Error";
            TopMost = true;
            Load += ApplicationDialog_Load;
            KeyPress += ApplicationDialog_KeyPress;
            KeyUp += ApplicationDialog_KeyUp;
            ErrorDialogTablePanel.ResumeLayout(false);
            ErrorDialogTablePanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)DialogIconPictureBox).EndInit();
            MessageContainer.ResumeLayout(false);
            MessageContainer.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel ErrorDialogTablePanel;
        private System.Windows.Forms.PictureBox DialogIconPictureBox;
        private System.Windows.Forms.Panel MessageContainer;
        private System.Windows.Forms.TextBox TextBoxMessage;
        private System.Windows.Forms.Label ErrorMessageTitle;
    }
}