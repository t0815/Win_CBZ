﻿using AutocompleteMenuNS;
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
using System.Windows.Media;
using Win_CBZ.Data;
using static System.Net.Mime.MediaTypeNames;

namespace Win_CBZ.Forms
{
    public partial class TagEditorForm : Form
    {
        public List<String> Lines = new List<String>();

        public EditorTypeConfig Config;

        private DataValidation Validation;

        public TagEditorForm(EditorTypeConfig editorTypeConfig)
        {
            InitializeComponent();

            Validation = new DataValidation();

            Config = editorTypeConfig;

            if (Config != null)
            {
                if (Config.Separator != null)
                {
                    if (Config.Value != null && Config.Value.Length > 0)
                    {
                        string[] lines = Config.Value.Split(Config.Separator[0]);

                        foreach (string line in lines)
                        {
                            Lines.Add(line.TrimStart(' ').TrimEnd(' '));
                        }                     
                    }

                }
                else
                {
                    if (Config.Value != null && Config.Value.Length > 0)
                    {
                        Lines.AddRange(Config.Value.Split('\n'));
                    }                       
                }

                if (Config.AutoCompleteItems != null)
                {
                    //AutoCompleteStringCollection autoCompleteStringCollection = new AutoCompleteStringCollection();
                    //autoCompleteStringCollection.AddRange(config.AutoCompleteItems);

                    var items = new List<AutocompleteItem>();
                    foreach (var item in Config.AutoCompleteItems)
                        items.Add(new SnippetAutocompleteItem(item) { ImageIndex = 0 });

                    AutoCompleteItems.SetAutocompleteItems(items);

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
            tagItem.Click += TagItemClick;
            tagItem.BackColor = System.Drawing.Color.White;
            
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
                Padding = new Padding(0, 2, 1, 1)
            };

            closeButton.Click += TagCloseButtonClick;

            tagItem.Controls.Add(new PictureBox()
            {
                Image = global::Win_CBZ.Properties.Resources.tag,
                SizeMode = PictureBoxSizeMode.AutoSize,
                Width = 15,
                Height = 15,
                Margin = new Padding(3, 2, 1, 2)
            });
            tagItem.Controls.Add(new System.Windows.Forms.Label() 
            {
                Text = tagName, 
                AutoSize = true,
                Padding = new Padding(1, 2, 1, 1),
                Margin = new Padding(0, 1, 0, 1),
                Font = new Font("Segoe UI", 8, FontStyle.Regular)
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

        private void TagItemClick(object sender, EventArgs e)
        {

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
                if (Config != null)
                {
                    if (!Config.AllowDuplicateValues)
                    {
                        duplicates.AddRange(Validation.ValidateDuplicateStrings(Lines.ToArray()));

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

                    var check = String.Join("", Lines);
                    if (check.Contains("|") || check.Contains(","))
                    {
                        ApplicationMessage.ShowError("Invalid Value! The following characters are not allowed:\r\n\r\n" + String.Join("\r\n", new string[] { "\",\"", "\"|\"" }), "Invalid Value", ApplicationMessage.DialogType.MT_ERROR, ApplicationMessage.DialogButtons.MB_OK);

                        e.Cancel = true;
                    }


                    if (Config.ResultType == EditorTypeConfig.RESULT_TYPE_STRING)
                    {
                        result = String.Join(Config.Separator ?? "" + Config.Append ?? "", Lines);
                    }

                    if (Config.ResultType == EditorTypeConfig.RESULT_TYPE_STRINGS)
                    {
                        result = Lines;
                    }

                    Config.Result = result;

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

        private void TagTextBox_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                if (TagTextBox.Text.Length > 0)
                {
                    Lines.Add(TagTextBox.Text);
                    AddTag(CreateTag(TagTextBox.Text));
                    TagTextBox.Text = string.Empty;
                    
                }
                e.SuppressKeyPress = true;
                e.Handled = true;
            }
        }

        private void TagEditorForm_Shown(object sender, EventArgs e)
        {
            TagTextBox.Focus();
        }

        private void DeleteAllTagsToolButton_Click(object sender, EventArgs e)
        {
            ClearTags();
            Lines.Clear();
            TagTextBox.Focus();
        }

        private void ButtonAddTag_Click(object sender, EventArgs e)
        {
            if (TagTextBox.Text.Length > 0)
            {
                Lines.Add(TagTextBox.Text);
                AddTag(CreateTag(TagTextBox.Text));
                TagTextBox.Text = string.Empty;
            }
            TagTextBox.Focus();
        }
    }
}
