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
using Win_CBZ.Events;
using Win_CBZ.Exceptions;

namespace Win_CBZ.Forms
{
    [SupportedOSPlatform("windows")]
    public partial class PageRangeSelectionForm : Form
    {

        public bool UseOffset { get; private set; } = false;

        public int Offset { get; private set; } = 0;

        public int StartIndex { get; private set; } = 0;

        public int EndIndex { get; private set; } = 0;

        public PageRangeSelectionForm(bool useOffset = false, int lastOffset = 0)
        {
            InitializeComponent();

            UseOffset = useOffset;
            Offset = lastOffset;

            CheckBoxUseOffset.Checked = UseOffset;
            TextBoxOffset.Text = Offset.ToString();
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            
        }

        private void PageRangeSelectionForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (DialogResult == DialogResult.Cancel)
            {
                e.Cancel = false;
                return;
            }

            try
            {
                UseOffset = CheckBoxUseOffset.Checked;
                if (UseOffset)
                {
                    if (!int.TryParse(TextBoxOffset.Text, out int offsetValue))
                    {
                        throw new ValidationException(TextBoxOffset.Text, "TextBoxOffset", "Validation Error! Offset must be nummeric ['0-9']!", true, false);
                    }
                    Offset = offsetValue;
                }
                else
                {
                    Offset = 0;
                }

                if (!int.TryParse(TextBoxStartIndex.Text, out int startIndex) || startIndex < 0)
                {
                    throw new ValidationException(TextBoxStartIndex.Text, "TextBoxStartIndex", "Validation Error! Start Index must be nummeric ['0-9']!", true, false);
                }

                if (!int.TryParse(TextBoxEndIndex.Text, out int endIndex) || startIndex < 0 || endIndex < 0 || startIndex > endIndex)
                {
                    throw new ValidationException(TextBoxEndIndex.Text, "TextBoxEndIndex", "Validation Error! End Index must be nummeric ['0-9']!", true, false);
                }



                StartIndex = startIndex + Offset;

                EndIndex = endIndex + Offset;

                DialogResult = DialogResult.OK;
            }
            catch (ValidationException ve)
            {
                string controlName = ve.ControlName;
                string row = null;

                if (controlName.Contains("."))
                {
                    string[] parts = controlName.Split('.');
                    controlName = parts[0];
                    row = parts[1];
                }

                try
                {
                    ValidationErrorProvider.SetError(this.Controls.Find(controlName, true)[0], ve.Message);
                }
                catch
                {
                    ApplicationMessage.ShowError("Failed to assign error to control! Control '${controlName}' not found!", "Error", ApplicationMessage.DialogType.MT_ERROR, ApplicationMessage.DialogButtons.MB_OK);

                }     

                DialogResult = DialogResult.None;
                e.Cancel = true;

                //if (Win_CBZSettings.Default.LogValidationErrors)
                //{
                //    MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_ERROR, mv.Message);
                //}

                ApplicationMessage.ShowWarning(ve.Message, "Validation Error", ApplicationMessage.DialogType.MT_WARNING, ApplicationMessage.DialogButtons.MB_OK);

            }
        }
    }
}
