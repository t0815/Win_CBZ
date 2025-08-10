using AutocompleteMenuNS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using System.Xml.Linq;
using Win_CBZ.Data;
using Win_CBZ.Models;
using static System.Net.Mime.MediaTypeNames;
using Brush = System.Drawing.Brush;
using Brushes = System.Drawing.Brushes;
using Color = System.Drawing.Color;
using Font = System.Drawing.Font;
using Label = System.Windows.Forms.Label;
using ListViewItem = System.Windows.Forms.ListViewItem;
using Pen = System.Drawing.Pen;

namespace Win_CBZ.Forms
{
    [SupportedOSPlatform("windows")]
    public partial class TagEditorForm : Form
    {

        public List<String> Lines = new List<String>();

        public EditorTypeConfig Config;

        private DataValidation Validation;

        private List<FlowLayoutPanel> SelectedTags;

        private bool MouseBtnDown;

        private Point SelectionStart;

        public TagEditorForm(EditorTypeConfig editorTypeConfig)
        {
            InitializeComponent();

            Validation = new DataValidation();
            SelectedTags = new List<FlowLayoutPanel>();

            Config = editorTypeConfig;



        }

        public FlowLayoutPanel CreateTag(string tagName)
        {
            if (string.IsNullOrEmpty(tagName))
            {
                return null;
            }

            FlowLayoutPanel tagItem = new FlowLayoutPanel();
            tagItem.Name = "TAG_" + tagName;
            tagItem.Tag = new TagItem()
            {
                Tag = tagName
            };
            tagItem.AutoSize = true;
            //tagItem.Click += TagItemClick;
            tagItem.BackColor = System.Drawing.Color.White;

            tagItem.BorderStyle = BorderStyle.None;
            tagItem.GotFocus += TagFocused;
            tagItem.LostFocus += TagLostFocus;

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
                Margin = new Padding(0, 2, 2, 2),
                Padding = new Padding(0, 2, 1, 1)
            };

            System.Windows.Forms.Label tagLabel = new System.Windows.Forms.Label()
            {
                Name = "TAG_LABEL",
                Text = tagName,
                AutoSize = true,
                Tag = tagItem,
                Padding = new Padding(1, 1, 1, 1),
                Margin = new Padding(0, 2, 0, 1),
                Font = new Font("Segoe UI", 8, FontStyle.Regular)
            };

            closeButton.Click += TagCloseButtonClick;
            tagLabel.Click += TagItemClick;

            tagItem.Controls.Add(new PictureBox()
            {
                Image = global::Win_CBZ.Properties.Resources.tag,
                SizeMode = PictureBoxSizeMode.AutoSize,
                Width = 15,
                Height = 15,
                Margin = new Padding(3, 1, 1, 2)
            });
            tagItem.Controls.Add(tagLabel);

            tagItem.Controls.Add(closeButton);

            tagItem.Parent = TagsList;

            return tagItem;
        }

        public void AddTag(string tagName)
        {
            ListViewItem newItem = TagListView.Items.Add(tagName);
            newItem.ImageKey = "tag";
        }

        public void AddTag(System.Windows.Forms.Control control)
        {
            if (control != null)
            {
                Invoke(new Action(() =>
                {
                    TagsList.Controls.Add(control);
                }));

            }
        }

        public void RemoveTag(string tagName)
        {
            if (string.IsNullOrEmpty(tagName))
            {
                return;
            }

            System.Windows.Forms.Control[] tags = TagsList.Controls.Find("TAG_" + tagName, false);
            if (tags != null && tags.Length > 0)
            {
                FlowLayoutPanel tag = TagsList.Controls.Find("TAG_" + tagName, false)[0] as FlowLayoutPanel;

                if (tag != null)
                {
                    TagsList.Controls.Remove(tag);
                }
            }
        }

        public void ClearTags()
        {
            TagsList.Controls.Clear();
            TagListView.Items.Clear();
        }

        private void TagItemClick(object sender, EventArgs e)
        {
            Label label = sender as Label;
            FlowLayoutPanel container = label.Parent as FlowLayoutPanel;

            if (container != null)
            {
                if (!((TagItem)container.Tag).Selected)
                {
                    //Label contentLabel = container.Controls.Find("TAG_LABEL", false)[0] as Label;
                    //if (contentLabel != null)
                    //{
                    label.BackColor = Color.Gold;
                    //}
                    ((TagItem)container.Tag).Selected = true;
                    SelectedTags.Add(container);
                }
                else
                {

                    label.BackColor = Color.White;
                    //}
                    ((TagItem)container.Tag).Selected = false;
                    SelectedTags.Remove(container);
                }

                ToolStripButtonRemoveSelectedTags.Enabled = SelectedTags.Count > 0;
            }
        }

        private void TagFocused(object sender, EventArgs e)
        {

        }

        private void TagLostFocus(object sender, EventArgs e)
        {

        }

        private void TagCloseButtonClick(object sender, System.EventArgs e)
        {
            if (((PictureBox)sender).Tag != null)
            {
                var tag = ((PictureBox)sender).Tag as FlowLayoutPanel;
                Lines.Remove(((TagItem)(tag as FlowLayoutPanel).Tag).Tag);
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
                    if (check.Contains("|") || check.Contains(Config.Separator))
                    {
                        ApplicationMessage.ShowError("Invalid Value! The following characters are not allowed:\r\n\r\n" + String.Join("\r\n", new string[] { "\"" + Config.Separator == " " ? "<SPACE>" : Config.Separator + "\"", "\"|\"" }), "Invalid Value", ApplicationMessage.DialogType.MT_ERROR, ApplicationMessage.DialogButtons.MB_OK);

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
                AddTag(tag);
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
                    AddTag(TagTextBox.Text);
                    TagTextBox.Text = string.Empty;

                }
                e.SuppressKeyPress = true;
                e.Handled = true;
            }
        }

        private void TagEditorForm_Shown(object sender, EventArgs e)
        {
            TagTextBox.Focus();

            Task.Factory.StartNew(() =>
            {
                if (Config != null)
                {
                    if (Config.Separator != null)
                    {
                        if (Config.Value != null && Config.Value.Length > 0)
                        {
                            string[] lines = Config.Value.Split(Config.Separator[0]);

                            foreach (string line in lines)
                            {
                                Lines.Add(line.TrimStart().TrimEnd());
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
                            items.Add(new AutocompleteItem(item) { ImageIndex = 0 });

                        Autocomplete.SetAutocompleteItems(items);

                        //TagTextBox.AutoCompleteCustomSource = autoCompleteStringCollection;
                        //TagTextBox.AutoCompleteSource = AutoCompleteSource.CustomSource;
                        //TagTextBox.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

                    }
                }

                foreach (String tag in Lines)
                {
                    Invoke(new Action(() =>
                    {
                        AddTag(CreateTag(tag));
                        AddTag(tag);
                    }));

                    Thread.Sleep(2);
                }
            });
        }

        private void DeleteAllTagsToolButton_Click(object sender, EventArgs e)
        {
            ClearTags();
            Lines.Clear();
            TagListView.Items.Clear();
            TagTextBox.Focus();
        }

        private void ButtonAddTag_Click(object sender, EventArgs e)
        {
            if (TagTextBox.Text.Length > 0)
            {
                Lines.Add(TagTextBox.Text);
                AddTag(CreateTag(TagTextBox.Text));
                AddTag(TagTextBox.Text);
                TagTextBox.Text = string.Empty;
            }
            TagTextBox.Focus();
        }

        private void TagTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void TagListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            //ToolStripButtonRemoveSelectedTags.Enabled = TagListView.SelectedItems.Count > 0;
            if (TagListView.SelectedItems.Count > 0)
            {

            }

        }

        private void ToolStripButtonRemoveSelectedTags_Click(object sender, EventArgs e)
        {
            foreach (FlowLayoutPanel tag in SelectedTags)
            //foreach (ListViewItem item in TagListView.SelectedItems)
            {
                Lines.Remove(((TagItem)tag.Tag).Tag);
                RemoveTag(((TagItem)tag.Tag).Tag);
                //TagListView.Items.Remove(item);               
            }
        }

        private void TagListView_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            Pen captionPen = new Pen(Color.Black, 1);
            Brush textBrush = Brushes.Black;
            Brush textBGBrush = Brushes.White;
            Font textFont = SystemFonts.CaptionFont;

            e.Graphics.DrawImage(TagListView.SmallImageList.Images["Tag"], new PointF(e.Bounds.X, e.Bounds.Y));
            e.Graphics.DrawString(e.Item.Text, Font, textBrush, new PointF(e.Bounds.X + 17, e.Bounds.Y + 3));
            if (e.State == ListViewItemStates.Selected)
            {
                e.DrawFocusRectangle();
            }
            e.Graphics.DrawImage(TagListView.SmallImageList.Images["delete"], new PointF(e.Bounds.X + e.Bounds.Width - 16, e.Bounds.Y + 1));
        }

        private void TagsList_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (MouseBtnDown == false)
            {
                MouseBtnDown = true;
                SelectionStart = new Point(e.X, e.Y);
            }
        }

        private void TagsList_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            MouseBtnDown = false;
        }

        private void Autocomplete_Opening(object sender, CancelEventArgs e)
        {

        }

        private void Autocomplete_WrapperNeeded(object sender, WrapperNeededEventArgs e)
        {

        }

        private void Autocomplete_Selecting(object sender, SelectingEventArgs e)
        {
            //TagTextBox.Text = e.Item.Text;  
        }

        private void Autocomplete_Selected(object sender, SelectedEventArgs e)
        {
            //TagTextBox.Text = e.Item.Text;
        }

        private void TagEditorForm_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                DialogResult = DialogResult.Cancel;
                Close();
            }
        }
    }
}
