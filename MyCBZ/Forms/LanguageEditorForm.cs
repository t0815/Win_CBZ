using System;
using System.Collections;
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
using Win_CBZ.Models;

namespace Win_CBZ.Forms
{
    [SupportedOSPlatform("windows")]
    public partial class LanguageEditorForm : Form
    {
        public EditorTypeConfig config;

        BindingList<LanguageListItem> FilteredList;

        DataGridViewRow selectedRow;

        bool init = false;

        public LanguageEditorForm(EditorTypeConfig editorTypeConfig)
        {
            InitializeComponent();

            config = editorTypeConfig;

            LanguageListDatagrid.DataSource = LanguageList.Languages;

            init = false;

        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void ToolStripTextBoxSearchKeyValue_TextChanged(object sender, EventArgs e)
        {
            String find = ToolStripTextBoxSearchLang.Text;

            FilteredList = new BindingList<LanguageListItem>(LanguageList.Languages.Where<LanguageListItem>((item, x) =>
            {
                return item.Name.ToLower().Contains(find.ToLower());
            }).ToArray());

            LanguageListDatagrid.DataSource = FilteredList;

            this.LanguageEditorForm_Shown(null, e);
        }

        private void LanguageEditorForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            object result = null;


            if (DialogResult == DialogResult.OK)
            {
                if (LanguageListDatagrid.SelectedRows.Count == 1)
                {
                    result = LanguageListDatagrid.SelectedRows[0].Cells[1].Value;
                }
            }

            config.Result = result;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            //ItemsText.Lines = ItemsText.Lines.OrderBy(s => s).ToArray();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {

        }

        private void toolStripTextBox1_Click(object sender, EventArgs e)
        {

        }

        private void ToolStripTextBoxSearchLang_TextChanged(object sender, EventArgs e)
        {
            String find = ToolStripTextBoxSearchLang.Text;

            FilteredList = new BindingList<LanguageListItem>(LanguageList.Languages.Where<LanguageListItem>((item, x) =>
            {
                return item.Name.ToLower().Contains(find.ToLower());
            }).ToArray());

            LanguageListDatagrid.DataSource = FilteredList;

            this.LanguageEditorForm_Shown(null, e);
        }

        private void LanguageEditorForm_Shown(object sender, EventArgs e)
        {
            for (int i = 0; i < LanguageListDatagrid.RowCount; i++)
            {
                var key = LanguageListDatagrid.Rows[i].Cells[1].Value;
                if (selectedRow != null)
                {
                    if (selectedRow.Cells[1].Value == key)
                    {
                        LanguageListDatagrid.Rows[i].Selected = true;
                    }
                }
                else
                {
                    if (config.Value != null)
                    {
                        if (key.ToString() == config.Value.ToString())
                        {

                            LanguageListDatagrid.Rows[i].Selected = true;
                            selectedRow = LanguageListDatagrid.Rows[i];
                        }
                    }
                }
            }

            init = true;
        }

        private void LanguageListDatagrid_SelectionChanged(object sender, EventArgs e)
        {
            if (init)
            {
                if (LanguageListDatagrid.SelectedRows.Count > 0)
                {
                    selectedRow = LanguageListDatagrid.SelectedRows[0];
                }
            }
        }

        private void LanguageEditorForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                DialogResult = DialogResult.Cancel;
                Close();
            }
        }
    }
}
