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

        public ImagePreviewForm(Page page)
        {
            InitializeComponent();

            try
            {
                currentIndex = page.Index;
                currentId = page.Id;
                if (page.TemporaryFile == null || !page.TemporaryFile.Exists())
                {
                    page.CreateLocalWorkingCopy();                 
                }

                if (page.TemporaryFile.Exists())
                {
                    PageImagePreview.ImageLocation = page.TemporaryFile.FullPath;
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
            Width = PageImagePreview.Width + 40;
            ImagePreviewPanel.VerticalScroll.Value = 0;
        }

        private void ImagePreviewForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            PageImagePreview.Image.Dispose();
            PageImagePreview.Dispose();
        }

        protected void HandlePageNavigation(int direction)
        {
            Page page = Program.ProjectModel.GetPageById(currentId);
            Page nextPage = null;
            int newIndex = currentIndex;

            if (page != null)
            {               
               nextPage = Program.ProjectModel.GetPageByIndex(newIndex + direction);              
            }

            if (nextPage != null)
            {
                if (nextPage.TemporaryFile == null || !page.TemporaryFile.Exists())
                {
                    nextPage.CreateLocalWorkingCopy();
                }

                if (nextPage.TemporaryFile.Exists())
                {
                    PageImagePreview.ImageLocation = nextPage.TemporaryFile.FullPath;
                } else
                {
                    PageImagePreview.ImageLocation = null;
                }

                currentId = nextPage.Id;
                currentIndex = nextPage.Index;

                
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
