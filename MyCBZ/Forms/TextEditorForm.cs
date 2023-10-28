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
    public partial class TextEditorForm : Form
    {

        public List<String> Lines = new List<String>();

        public EditorTypeConfig config;


        public TextEditorForm(EditorTypeConfig editorTypeConfig)
        {
            InitializeComponent();

            config = editorTypeConfig;

            if (config != null ) 
            { 
                if (config.Separator != null)
                {
                    string[] lines = config.Value.Split(config.Separator[0]);

                    foreach ( string line in lines )
                    {
                        Lines.Add(line.TrimStart(' ').TrimEnd(' '));
                    }

                    ItemsText.Lines = Lines.ToArray();
                } else
                {
                    ItemsText.Text = config.Value;
                }
            }

        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            String result = "";

            if (ItemsText.Lines.Length > 0) 
            {
                if (config != null)
                {
                    if (config.ResultType == "String")
                    {
                        result = String.Join(config.Separator + config.Append, ItemsText.Lines);
                    }

                    config.Result = result;
                }
            }
        }
    }
}
