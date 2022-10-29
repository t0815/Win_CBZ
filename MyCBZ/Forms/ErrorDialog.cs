using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using static Win_CBZ.ApplicationError;

namespace Win_CBZ.Forms
{
    public partial class ErrorDialog : Form
    {
        private short _type;
        private String _message;
        private DialogButtons _buttons;
        private String _title;

        public short DialogType 
        { 
            set 
            {
                switch (value)
                {
                    case MT_INFORMATION:
                        Text = "Information";
                        DialogIconPictureBox.Image = global::Win_CBZ.Properties.Resources.info_dialog;
                        break;

                    case MT_WARNING:
                        Text = "WARNING!";
                        DialogIconPictureBox.Image = global::Win_CBZ.Properties.Resources.warning_dialog;
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
                if (value == DialogButtons.MB_OK && value == DialogButtons.MB_CANCEL)
                {
                    ButtonOk.Visible = true;
                    ButtonOk.Text = "Ok";
                    ButtonOk.DialogResult = DialogResult.OK;

                    ButtonCancel.Visible = true;
                    ButtonCancel.Text = "Cancel";
                    ButtonCancel.DialogResult = DialogResult.Cancel;
                } else if (value == DialogButtons.MB_YES && value == DialogButtons.MB_NO)
                {
                    ButtonOk.Visible = true;
                    ButtonOk.Text = "Yes";
                    ButtonOk.DialogResult = DialogResult.Yes;

                    ButtonCancel.Visible = true;
                    ButtonCancel.Text = "No";
                    ButtonCancel.DialogResult = DialogResult.No;

                    if (value == DialogButtons.MB_CANCEL)
                    {

                    }
                } else if (value == DialogButtons.MB_OK)
                {
                    ButtonOk.Visible = false;

                    ButtonCancel.Visible = true;
                    ButtonCancel.Text = "Ok";
                    ButtonCancel.DialogResult = DialogResult.OK;
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
                _message = value;
            }
        }

        public ErrorDialog()
        {
            InitializeComponent();
        }
    }
}
