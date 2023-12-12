using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Win_CBZ.Forms
{
    internal partial class EditImageForm : Form
    {
        public Page EditPage;
        public Page OriginalPage;

        public EditImageForm(Page editPage)
        {
            InitializeComponent();

            OriginalPage = editPage;

            try
            {
                EditPage = new Page(editPage, false, true);
            } catch (ApplicationException e)
            {
                if (e.ShowErrorDialog)
                {
                    ApplicationMessage.ShowWarning("Failed to create new Page.\r\n" + e.Message, "Page Error", ApplicationMessage.DialogType.MT_WARNING, ApplicationMessage.DialogButtons.MB_OK);
                }
            }
            
        }

        private void EditImageForm_Load(object sender, EventArgs e)
        {
            if (EditPage != null)
            {
                EditImageBox.Image = Image.FromStream(EditPage.GetImageStream());
            }
        }

        private void ToolButtonSave_Click(object sender, EventArgs e)
        {
            if (EditPage != null)
            {
                EditImageBox.Image?.Dispose();
                EditImageBox.Image = null;

                EditPage.FreeImage();

                OriginalPage.UpdateImage();
            }
        }

        private void EditImageForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (EditPage != null)
            {
                EditImageBox.Image?.Dispose();
                EditImageBox.Image = null;

                EditPage.FreeImage();
                EditPage.Close();
            }
        }
    }
}
