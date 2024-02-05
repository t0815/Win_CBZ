using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using Win_CBZ.Data;

namespace Win_CBZ.Forms
{
    public partial class TagEditorForm : Form
    {
        public List<String> Lines = new List<String>();

        public EditorTypeConfig config;

        DataValidation validation;

        public TagEditorForm(EditorTypeConfig editorTypeConfig)
        {
            InitializeComponent();

            validation = new DataValidation();

            config = editorTypeConfig;

            if (config != null)
            {
                if (config.Separator != null)
                {
                    if (config.Value != null)
                    {
                        string[] lines = config.Value.Split(config.Separator[0]);

                        foreach (string line in lines)
                        {
                            Lines.Add(line.TrimStart(' ').TrimEnd(' '));
                        }                     
                    }

                }
                else
                {
                    Lines.AddRange(config.Value.Split('\n'));
                }

                

                if (config.AutoCompleteItems != null)
                {
                    //AutoCompleteStringCollection autoCompleteStringCollection = new AutoCompleteStringCollection();
                    //autoCompleteStringCollection.AddRange(config.AutoCompleteItems);

                    AutoCompleteItems.Items = config.AutoCompleteItems;

                    //TagTextBox.AutoCompleteCustomSource = autoCompleteStringCollection;
                    //TagTextBox.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    //TagTextBox.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

                }
            }

            foreach (String tag in Lines)
            {
                AddTag(CreateTag(tag));
            }
        }

        public FlowLayoutPanel CreateTag(string tagName)
        {
            if (string.IsNullOrEmpty(tagName))
            {
                return null;
            }

            FlowLayoutPanel tagItem = new FlowLayoutPanel();
            tagItem.Name = "TAG_" + tagName;
            tagItem.Tag = tagName;
            tagItem.AutoSize = true;
           
            tagItem.BorderStyle = BorderStyle.None;

            /*
            System.Windows.Forms.Button closeButton = new System.Windows.Forms.Button()
            {
                Text = "",
                Tag = tagItem,
                Image = global::Win_CBZ.Properties.Resources.delete,
                ImageAlign = ContentAlignment.MiddleCenter,
                FlatStyle = FlatStyle.Flat,
                Size = new Size()
                {
                    Width = 15,
                    Height = 15
                },
            };
            */

            PictureBox closeButton = new PictureBox()
            {
                Image = global::Win_CBZ.Properties.Resources.delete,
                SizeMode = PictureBoxSizeMode.StretchImage,
                Width = 15,
                Height = 15,
                Tag = tagItem,
                Cursor = System.Windows.Forms.Cursors.Hand,
                Margin = new Padding(0, 2, 2, 4),
                Padding = new Padding(0, 1, 1, 1)
            };

            closeButton.Click += TagCloseButtonClick;

            tagItem.Controls.Add(new PictureBox()
            {
                Image = global::Win_CBZ.Properties.Resources.tag,
                SizeMode = PictureBoxSizeMode.AutoSize,
                Width = 15,
                Height = 15,
                Margin = new Padding(3, 3, 1, 2)
            });
            tagItem.Controls.Add(new System.Windows.Forms.Label() 
            {
                Text = tagName, 
                AutoSize = true,
                Padding = new Padding(1, 3, 1, 2),
                Margin = new Padding(0, 1, 0, 1)
            });

            tagItem.Controls.Add(closeButton);
            

    

            tagItem.Parent = TagsList;

            return tagItem;
        }

        public void AddTag(System.Windows.Forms.Control control)
        {
            if (control != null)
            {
                TagsList.Controls.Add(control);
            }
        }

        public void RemoveTag(string tagName)
        {
            if (string.IsNullOrEmpty(tagName))
            {
                return;
            }
        }

        public void ClearTags()
        {
            TagsList.Controls.Clear();
            
        }

        private void TagCloseButtonClick(object sender, System.EventArgs e)
        {
            if (((PictureBox)sender).Tag != null) {
                var tag = ((PictureBox)sender).Tag as FlowLayoutPanel;
                Lines.Remove((tag as FlowLayoutPanel).Tag.ToString());
                TagsList.Controls.Remove(tag);
            }
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void TagEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            object result = null;
            List<String> duplicates = new List<String>();

            if (DialogResult == DialogResult.OK)
            {
                
                    if (config != null)
                    {
                        if (!config.AllowDuplicateValues)
                        {
                            duplicates.AddRange(validation.ValidateDuplicateStrings(Lines.ToArray()));

                            if (duplicates.Count > 0)
                            {
                                ApplicationMessage.ShowError("Invalid Value! Duplicate entry detected.\r\n\r\n" + String.Join("\r\n", duplicates), "Invalid Value", ApplicationMessage.DialogType.MT_ERROR, ApplicationMessage.DialogButtons.MB_OK);

                                DialogResult = DialogResult.None;

                                e.Cancel = true;
                            }
                            else
                            {

                            }
                        }


                        if (config.ResultType == "String")
                        {
                            result = String.Join(config.Separator ?? "" + config.Append ?? "", Lines);
                        }

                        if (config.ResultType == "String[]")
                        {
                            result = Lines;
                        }

                        config.Result = result;

                        DialogResult = DialogResult.OK;
                    }
                
            }
        }

        private void ToolButtonSortAscending_Click(object sender, EventArgs e)
        {
            Lines = Lines.OrderBy(s => s).ToList();

            ClearTags();

            foreach (String tag in Lines)
            {
                AddTag(CreateTag(tag));
            }
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void TagTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
        }

        private void TagTextBox_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                if (TagTextBox.Text.Length > 0)
                {
                    Lines.Add(TagTextBox.Text);
                    AddTag(CreateTag(TagTextBox.Text));
                    TagTextBox.Text = string.Empty;
                    e.Handled = true;
                }

            }
        }

        private void TagEditorForm_Shown(object sender, EventArgs e)
        {
                
        }

        private void DeleteAllTagsToolButton_Click(object sender, EventArgs e)
        {
            ClearTags();
            Lines.Clear();
        }

        private void ButtonAddTag_Click(object sender, EventArgs e)
        {
            if (TagTextBox.Text.Length > 0)
            {
                Lines.Add(TagTextBox.Text);
                AddTag(CreateTag(TagTextBox.Text));
                TagTextBox.Text = string.Empty;
            }
        }
    }
}
