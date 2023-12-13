using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Win_CBZ.Img;

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
                //EditPage.ImageTask.SetupTasks(editPage, commands);
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

            EditImageForm_Resize(null, e);


            //OverlayPictureBox.Image = new Bitmap(300, 200);
            //OverlayPictureBox.Image.
        }

        private void ToolButtonSave_Click(object sender, EventArgs e)
        {
            if (EditPage != null)
            {
                EditImageBox.Image?.Dispose();
                EditImageBox.Image = null;

                EditPage.FreeImage();

                //OriginalPage.UpdateImage();
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

        private void ToolButtonCrop_Click(object sender, EventArgs e)
        {
            ToolButtonCrop.Checked = true;
        }

        private void EditImageForm_Resize(object sender, EventArgs e)
        {
            //EditImageBox.Location = new Point((ImageContentPanel.Width / 2) - (EditImageBox.Width / 2), (ImageContentPanel.Height / 2) - (EditImageBox.Height / 2));
        }
    }
}
