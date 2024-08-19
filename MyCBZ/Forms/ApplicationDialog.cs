using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Win_CBZ.ApplicationMessage;

namespace Win_CBZ.Forms
{
    [SupportedOSPlatform("windows")]
    public partial class ApplicationDialog : Form
    {
        private DialogType _type;
        private String _message;
        private DialogButtons _buttons;
        private String _title;

        private DialogButtons _existingButtons;
        private DialogResult _cancelResult;
        private DialogResult _okResult;

        public DialogType DialogType
        {
            set
            {
                switch (value)
                {
                    case DialogType.MT_INFORMATION:
                        Text = "Information";
                        DialogIconPictureBox.Image = global::Win_CBZ.Properties.Resources.info_dialog;
                        break;

                    case DialogType.MT_WARNING:
                        Text = "WARNING!";
                        DialogIconPictureBox.Image = global::Win_CBZ.Properties.Resources.warning_dialog;
                        break;

                    case DialogType.MT_CONFIRMATION:
                        Text = "Please confirm";
                        DialogIconPictureBox.Image = global::Win_CBZ.Properties.Resources.question_dialog;
                        break;

                    case DialogType.MT_CHECK:
                        Text = "Operation Successful!";
                        DialogIconPictureBox.Image = global::Win_CBZ.Properties.Resources.checks;
                        break;

                    default:
                        Text = "Application Error";
                        DialogIconPictureBox.Image = global::Win_CBZ.Properties.Resources.error_dialog;
                        break;
                }
                _type = value;
            }
        }

        public DialogButtons Buttons
        {
            set
            {
                int index = 4;
                if (value.HasFlag(DialogButtons.MB_ABORT))
                {
                    MakeBtn("Abort", DialogButtons.MB_ABORT, DialogResult.Abort, index, AcceptButtonType.CANCEL_BUTTON);
                    _cancelResult = DialogResult.Abort;
                    index--;
                }

                if (value.HasFlag(DialogButtons.MB_IGNORE))
                {
                    MakeBtn("Ignore", DialogButtons.MB_IGNORE, DialogResult.Ignore, index, AcceptButtonType.CANCEL_BUTTON);
                    index--;
                }

                if (value.HasFlag(DialogButtons.MB_CANCEL) && !_existingButtons.HasFlag(DialogButtons.MB_ABORT))
                {

                    MakeBtn("Cancel", DialogButtons.MB_CANCEL, DialogResult.Cancel, index, AcceptButtonType.CANCEL_BUTTON);
                    _cancelResult = DialogResult.Cancel;
                    index--;
                }

                if (value.HasFlag(DialogButtons.MB_NO) && !_existingButtons.HasFlag(DialogButtons.MB_CANCEL | DialogButtons.MB_ABORT))
                {
                    MakeBtn("No", DialogButtons.MB_NO, DialogResult.No, index, AcceptButtonType.CANCEL_BUTTON);
                    _cancelResult = DialogResult.No;
                    index--;
                }

                if (value.HasFlag(DialogButtons.MB_OK))
                {
                    MakeBtn("Ok", DialogButtons.MB_OK, DialogResult.OK, index, AcceptButtonType.ACCEPT_BUTTON);
                    _okResult = DialogResult.OK;
                    index--;
                }

                if (value.HasFlag(DialogButtons.MB_YES) && !_existingButtons.HasFlag(DialogButtons.MB_OK))
                {
                    MakeBtn("Yes", DialogButtons.MB_YES, DialogResult.Yes, index, AcceptButtonType.ACCEPT_BUTTON);
                    _okResult = DialogResult.Yes;
                    index--;
                }

                if (value.HasFlag(DialogButtons.MB_QUIT) && !_existingButtons.HasFlag(DialogButtons.MB_OK))
                {
                    MakeBtn("Quit", DialogButtons.MB_QUIT, DialogResult.Yes, index, AcceptButtonType.ACCEPT_BUTTON);
                    index--;
                }

                _buttons = value;
            }
        }

        public String DialogTitle
        {
            set
            {
                ErrorMessageTitle.Text = value;
                _title = value;
            }
        }

        public String Message
        {
            set
            {
                TextBoxMessage.Text = value;
                TextBoxMessage.Select(0, 0);
                _message = value;
            }
        }

        protected Button MakeBtn(String caption, DialogButtons btn, DialogResult result, int index, AcceptButtonType acceptBtnType)
        {
            Button dialogButton = new Button();
            dialogButton.Text = caption;
            dialogButton.DialogResult = result;
            dialogButton.TabIndex = index;
            dialogButton.Height = 34;
            dialogButton.Anchor = AnchorStyles.Right | AnchorStyles.Left;
            ErrorDialogTablePanel.Controls.Add(dialogButton, index, 2);
            _existingButtons |= btn;

            switch (acceptBtnType)
            {
                case AcceptButtonType.ACCEPT_BUTTON:
                    AcceptButton = dialogButton;
                    break;
                case AcceptButtonType.CANCEL_BUTTON:
                    CancelButton = dialogButton;
                    break;
            }

            return dialogButton;
        }

        public void ShowMessageScrollbars(ScrollBars show)
        {
            TextBoxMessage.ScrollBars = show;
        }

        public ApplicationDialog()
        {
            InitializeComponent();
        }

        private void ApplicationDialog_KeyUp(object sender, KeyEventArgs e)
        {
            if (sender == this)
            {
                if (e.KeyCode == Keys.Escape)
                {
                    DialogResult = _cancelResult;
                    Close();
                }

                if (e.KeyCode == Keys.Return)
                {
                    DialogResult = _okResult;
                    Close();
                }
            }
        }

        private void ApplicationDialog_KeyPress(object sender, KeyPressEventArgs e)
        {
            
        }
    }
}
