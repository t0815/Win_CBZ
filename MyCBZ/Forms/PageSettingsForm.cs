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
    internal partial class PageSettingsForm : Form
    {

        Page Page;
        Image PreviewThumb;
        Random RandomProvider;

        public PageSettingsForm(Page page)
        {
            InitializeComponent();

            RandomProvider = new Random();
            Page = new Page(page, RandomProvider.Next().ToString("X"));
          
            PreviewThumb = Page.GetThumbnail(ThumbAbort, Handle);

            PreviewThumbPictureBox.Image = PreviewThumb;

            TextBoxFileLocation.Text = Page.Compressed ? Page.TempPath : Page.Filename;
            PageNameTextBox.Text = Page.Name;
            PageIndexTextbox.Text = Page.Index.ToString();
        }

        private void PageSettingsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Page.DeleteTemporaryFile();
            Page.FreeImage();
            Page = null;
        }

        private bool ThumbAbort()
        {
            return true;
        }
    }
}
