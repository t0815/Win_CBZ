using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Win_CBZ
{
    internal partial class ImagePreviewForm : Form
    {
        int currentIndex = 0;
        string currentId = null;

        Page displayPage = null;

        public ImagePreviewForm(Page page)
        {
            InitializeComponent();

            try
            {
                displayPage = new Page(page);

                currentIndex = page.Index;
                currentId = page.Id;
                if (displayPage.TemporaryFile == null || !displayPage.TemporaryFile.Exists())
                {
                    displayPage.CreateLocalWorkingCopy();                 
                }

                if (displayPage.TemporaryFile.Exists())
                {
                    PageImagePreview.Image = Image.FromStream(displayPage.GetImageStream());
                    Width = displayPage.W + 40;
                } else
                {
                    PageImagePreview.ImageLocation = null;
                }
                
            }
            catch (Exception e)
            {
                
            }

           
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {

        }

        private void PageImagePreview_LoadCompleted(object sender, AsyncCompletedEventArgs e)
        {
            Width = PageImagePreview.Image.Width + 40;
            ImagePreviewPanel.VerticalScroll.Value = 0;
        }

        private void ImagePreviewForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            PageImagePreview.Image.Dispose();
            PageImagePreview.Dispose();

            displayPage.Close();
        }

        protected void HandlePageNavigation(int direction)
        {
            Page page = Program.ProjectModel.GetPageById(currentId);
            int newIndex = currentIndex;

            if (displayPage != null)
            {
                displayPage.Close();
            }

            if (page != null)
            {
                displayPage = new Page(Program.ProjectModel.GetPageByIndex(newIndex + direction));              
            }

            if (displayPage != null)
            {
                if (displayPage.TemporaryFile == null || !displayPage.TemporaryFile.Exists())
                {
                    displayPage.CreateLocalWorkingCopy();
                }

                if (displayPage.TemporaryFile.Exists())
                {
                    PageImagePreview.Image = Image.FromStream(displayPage.GetImageStream());
                    Width = displayPage.W + 40;
                } else
                {
                    PageImagePreview.ImageLocation = null;
                }

                currentId = displayPage.Id;
                currentIndex = displayPage.Index;

                
            }
        }

        private void ImagePreviewForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                e.SuppressKeyPress = true;
                HandlePageNavigation(-1);
                   
            }

            if (e.KeyCode == Keys.Right)
            {
                e.SuppressKeyPress = true;
                HandlePageNavigation(1);
            } 
        }

        private void ImagePreviewForm_Load(object sender, EventArgs e)
        {

        }

        private void ImagePreviewForm_KeyDown(object sender, KeyEventArgs e)
        {
            /*
            if (e.KeyCode == Keys.Down)
            {
                e.SuppressKeyPress = true;
                ImagePreviewPanel.VerticalScroll.Value = ImagePreviewPanel.VerticalScroll.Value + 50;

            }

            if (e.KeyCode == Keys.Up)
            {
                e.SuppressKeyPress = true;
                if (ImagePreviewPanel.VerticalScroll.Value - 50 < 0)
                {
                    ImagePreviewPanel.VerticalScroll.Value = 0;
                }
                else
                {
                    ImagePreviewPanel.VerticalScroll.Value = ImagePreviewPanel.VerticalScroll.Value - 50;
                }
            }
            */
        }

        private void ImagePreviewPanel_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
