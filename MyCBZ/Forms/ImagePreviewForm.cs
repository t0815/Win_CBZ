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
        private Random RandomProvider;

        public ImagePreviewForm(Page page)
        {
            InitializeComponent();

            try
            {
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
            this.Width = PageImagePreview.Width + 40;
        }
    }
}
