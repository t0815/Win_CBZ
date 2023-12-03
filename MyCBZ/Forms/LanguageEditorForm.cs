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
                
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            //ItemsText.Lines = ItemsText.Lines.OrderBy(s => s).ToArray();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {

        }
    }
}
