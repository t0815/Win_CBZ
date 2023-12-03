using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Win_CBZ.Data;

namespace Win_CBZ.Forms
{
    public partial class LanguageEditorForm : Form
    {
        public EditorTypeConfig config;

        BindingList<LanguageListItem> FilteredList;

        public LanguageEditorForm(EditorTypeConfig editorTypeConfig)
        {
            InitializeComponent();

            config = editorTypeConfig;

            LanguageListDatagrid.DataSource = LanguageList.Languages;

        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
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

            FilteredList = new BindingList<LanguageListItem>(LanguageList.Languages.Where<LanguageListItem>((item, x) => {
                return item.Name.ToLower().Contains(find.ToLower());
            }).ToArray());

            LanguageListDatagrid.DataSource = FilteredList;
        }
    }
}
