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
using Win_CBZ.Models;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using ComboBox = System.Windows.Forms.ComboBox;

namespace Win_CBZ.Forms
{

    [SupportedOSPlatform("windows")]
    public partial class UserFilerKeysForm : Form
    {

        public string[] FilterKeys { get; set; }

        private string[] _defaultKeys;

        public int BaseContitionType { get; set; } = 0;

        private List<string> Lines = new List<string>();

        public UserFilerKeysForm()
        {
            InitializeComponent();

            Theme.GetInstance().ApplyThemeDataGid(DatagridUserKeyFilter);
            Theme.GetInstance().ApplyTheme(ItemEditorTableLayout.Controls);

            FilterKeys = Win_CBZSettings.Default.KeyFilter?.OfType<string>().ToArray();
            BaseContitionType = Win_CBZSettings.Default.KeyFilterBaseContitionType;

            _defaultKeys = Win_CBZSettings.Default.CustomDefaultProperties?.OfType<string>().ToArray();

            DatagridUserKeyFilter.Columns.Add(
                new DataGridViewTextBoxColumn
                {
                    CellTemplate = new DataGridViewTextBoxCell(),
                    Width = 250,
                    HeaderText = "Key Name",
                }
            );

            DatagridUserKeyFilter.Rows.Clear();

            ComboBoxCondition.SelectedIndex = BaseContitionType;
        }

        private void OkButton_Click(object sender, EventArgs e)
        {

        }

        private void ButtonAddKey_Click(object sender, EventArgs e)
        {
            if (TextBoxKey.Text.Length > 0)
            {
                DatagridUserKeyFilter.Rows.Add(TextBoxKey.Text);

                TextBoxKey.Text = string.Empty;

            }
        }

        private void TextBoxKey_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                if (TextBoxKey.Text.Length > 0)
                {
                    DatagridUserKeyFilter.Rows.Add(TextBoxKey.Text);

                    TextBoxKey.Text = string.Empty;

                }
                e.SuppressKeyPress = true;
                e.Handled = true;
            }
        }

        private void UserFilerKeysForm_Shown(object sender, EventArgs e)
        {
            TextBoxKey.Focus();

            Task.Factory.StartNew(() =>
            {
                if (_defaultKeys != null)
                {
                    foreach (string line in _defaultKeys)
                    {
                        string[] r = line.Split("=");
                        Lines.Add(r[0].TrimStart().TrimEnd());
                    }

                    if (Lines.Count > 0)
                    {
                        //AutoCompleteStringCollection autoCompleteStringCollection = new AutoCompleteStringCollection();
                        //autoCompleteStringCollection.AddRange(config.AutoCompleteItems);

                        var items = new List<AutocompleteItem>();
                        foreach (var item in Lines)
                            items.Add(new AutocompleteItem(item) { ImageIndex = 0 });

                        AutoCompleteDefaultKeys.SetAutocompleteItems(items);

                        //TagTextBox.AutoCompleteCustomSource = autoCompleteStringCollection;
                        //TagTextBox.AutoCompleteSource = AutoCompleteSource.CustomSource;
                        //TagTextBox.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

                    }
                }

                if (FilterKeys != null)
                {
                    foreach (var key in FilterKeys)
                    {


                        Invoke(new Action(() =>
                        {
                            DatagridUserKeyFilter.Rows.Add(key);
                        }));

                        Thread.Sleep(2);
                    }
                }
            });
        }

        private void UserFilerKeysForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (DialogResult == DialogResult.OK)
            {
                FilterKeys = new string[DatagridUserKeyFilter.Rows.Count];

                for (int i = 0; i < DatagridUserKeyFilter.Rows.Count; i++)
                {
                    FilterKeys[i] = DatagridUserKeyFilter.Rows[i].Cells[0].Value.ToString();
                }

                BaseContitionType = ComboBoxCondition.SelectedIndex;
            }
        }

        private void DeleteAllTagsToolButton_Click(object sender, EventArgs e)
        {
            DatagridUserKeyFilter.Rows.Clear();
        }

        private void ToolStripButtonRemoveSelectedKeys_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewCell cell in DatagridUserKeyFilter.SelectedCells)
            {
                DatagridUserKeyFilter.Rows.Remove(cell.OwningRow);
            }
        }

        private void ComboBoxCondition_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0)
            {
                return;
            }

            if (sender as ComboBox == null)
            {
                return;
            }

            Pen pen = new Pen(Color.Black, 1);
            Font font = new Font("Verdana", 9f, FontStyle.Regular);

            if (e.State.HasFlag(DrawItemState.Selected))
            {
                e.Graphics.FillRectangle(new SolidBrush(Theme.GetInstance().AccentColor), e.Bounds);
            }
            else
            {
                e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(255, 255, 255)), e.Bounds);
            }


            if (ComboIcons.Images.ContainsKey("hash"))
            {
                Image img = ComboIcons.Images["hash"];
                e.Graphics.DrawImage(img, new Point(e.Bounds.X, e.Bounds.Y));
                e.Graphics.DrawString(((ComboBox)sender).Items[e.Index].ToString(), font, new SolidBrush(Color.Black), new PointF(e.Bounds.X + 18, e.Bounds.Y + 1));
            }
            else
            {
                e.Graphics.DrawString(((ComboBox)sender).Items[e.Index].ToString(), font, new SolidBrush(Color.Black), new PointF(e.Bounds.X + 1, e.Bounds.Y + 1));
            }
        }
    }
}
