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
                if (page.LocalPath == null)
                {
                   page.LocalPath = Program.ProjectModel.RequestTemporaryFile(page);                  
                }

                PageImagePreview.ImageLocation = page.LocalPath;
                
            }
            catch (Exception e)
            {
                ApplicationMessage.ShowException(e);
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
                if (nextPage.LocalPath == null)
                {
                    nextPage.LocalPath = Program.ProjectModel.RequestTemporaryFile(nextPage);
                }

                PageImagePreview.ImageLocation = nextPage.LocalPath;

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
        }
    }
}
