using System;
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
    public partial class KeyValueEditor : Form
    {
        public EditorTypeConfig config;

        BindingList<LanguageListItem> FilteredList;

        DataGridViewRow selectedRow;

        bool init = false;

        public KeyValueEditor(EditorTypeConfig editorTypeConfig)
        {
            InitializeComponent();

            config = editorTypeConfig;

            KeyValueDatagrid.DataSource = config.Value;

            init = false;
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void ToolStripTextBoxSearchKeyValue_TextChanged(object sender, EventArgs e)
        {
            String find = ToolStripTextBoxSearchKeyValue.Text;

            //FilteredList = new BindingList<LanguageListItem>(LanguageList.Languages.Where<LanguageListItem>((item, x) => {
            //    return item.Name.ToLower().Contains(find.ToLower());
            //}).ToArray());

            //KeyValueDatagrid.DataSource = FilteredList;

            KeyValueEditorForm_Shown(null, e);
        }

        private void KeyValueEditorForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            object result = null;


            if (DialogResult == DialogResult.OK)
            {
                if (KeyValueDatagrid.SelectedRows.Count == 1)
                {
                    result = KeyValueDatagrid.SelectedRows[0].Cells[1].Value;
                }
            }

            config.Result = result;
        }

        private void KeyValueEditorForm_Shown(object sender, EventArgs e)
        {
            for (int i = 0; i < KeyValueDatagrid.RowCount; i++)
            {
                var key = KeyValueDatagrid.Rows[i].Cells[1].Value;
                if (selectedRow != null)
                {
                    if (selectedRow.Cells[1].Value == key)
                    {
                        KeyValueDatagrid.Rows[i].Selected = true;
                    }
                }
                else
                {
                    if (key.ToString() == config.Value.ToString())
                    {

                        KeyValueDatagrid.Rows[i].Selected = true;
                        selectedRow = KeyValueDatagrid.Rows[i];
                    }
                }
            }

            init = true;
        }

        private void KeyValueDatagrid_SelectionChanged(object sender, EventArgs e)
        {
            if (init)
            {
                if (KeyValueDatagrid.SelectedRows.Count > 0)
                {
                    selectedRow = KeyValueDatagrid.SelectedRows[0];
                }
            }
        }

        
    }
}
