using ScintillaNET;
using SharpCompress;
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
using Win_CBZ.Data;
using Win_CBZ.Events;
using Win_CBZ.Exceptions;
using Win_CBZ.Extensions;

namespace Win_CBZ.Forms
{
    [SupportedOSPlatform("windows")]
    public partial class PageRangeSelectionForm : Form
    {

        public bool UseOffset { get; private set; } = false;

        public int Offset { get; private set; } = 0;

        public List<PageSelectionRange> Selections { get; private set; } = new List<PageSelectionRange>();

        public PageRangeSelectionForm(bool useOffset = false, int lastOffset = 0)
        {
            InitializeComponent();

            Theme.GetInstance().ApplyTheme(ItemEditorTableLayout.Controls);

            UseOffset = useOffset;
            Offset = lastOffset;

            CheckBoxUseOffset.Checked = UseOffset;
            if (lastOffset > 0)
            {
                TextBoxOffset.Text = lastOffset.ToString();
            }
            else
            {
                TextBoxOffset.Text = "";
            }

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

                if (Selections.Count > 0)
                {
                    Selections.Clear();
                }

                string[] selectionParts = TextBoxSelections.Text.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                if (selectionParts.Length == 0)
                {
                    selectionParts.Append(TextBoxSelections.Text);
                }

                selectionParts.Each(part =>
                {
                    string[] indices;

                    if (part.Contains('-'))
                    {
                        indices = part.Split(new[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
                        if (indices.Length > 0)
                        {

                            if (indices.Length != 2)
                            {
                                throw new ValidationException(part, "TextBoxSelections", "Validation Error! Each selection must be in the format 'start-end'!", true, false);
                            }

                            if (!int.TryParse(indices[0], out int startIndex) || startIndex + Offset < 1)
                            {
                                throw new ValidationException(indices[0], "TextBoxSelections", "Validation Error! Start Index must be nummeric ['0-9'] and greater than 0!", true, false);
                            }

                            if (!int.TryParse(indices[1], out int endIndex) || endIndex + Offset < 1 || startIndex + Offset > endIndex + Offset)
                            {
                                throw new ValidationException(indices[1], "TextBoxSelections", "Validation Error! End Index must be nummeric ['0-9'], greater than 0 and greater than or equal to Start Index!", true, false);
                            }

                            Selections.Add(new PageSelectionRange(startIndex + Offset, endIndex + Offset));
                        }
                    }
                    else if (part.Contains(','))
                    {
                        indices = part.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        if (indices.Length > 0)
                        {
                            indices.ToList().ForEach(indexStr =>
                            {
                                if (!int.TryParse(indexStr, out int index) || index + Offset < 1)
                                {
                                    throw new ValidationException(indexStr, "TextBoxSelections", "Validation Error! Index must be nummeric ['0-9'] and greater than 0!", true, false);
                                }

                                Selections.Add(new PageSelectionRange(index + Offset, index + Offset));
                            });
                        }
                    }
                    else
                    {
                        if (!int.TryParse(part, out int index) || index + Offset < 1)
                        {
                            throw new ValidationException(part, "TextBoxSelections", "Validation Error! Index must be nummeric ['0-9'] and greater than 0!", true, false);
                        }

                        Selections.Add(new PageSelectionRange(index + Offset, index + Offset));
                    }
                });

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

        private void PageRangeSelectionForm_Load(object sender, EventArgs e)
        {

        }

        private void PageRangeSelectionForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                DialogResult = DialogResult.Cancel;
                Close();
            }
        }
    }
}
