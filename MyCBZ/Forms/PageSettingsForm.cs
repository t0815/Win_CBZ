using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Win_CBZ.Helper;

namespace Win_CBZ.Forms
{
    internal partial class PageSettingsForm : Form
    {

        Page Page;
        Image PreviewThumb;
        String imageLocation;

        public PageSettingsForm(Page page)
        {
            InitializeComponent();

            Page = new Page(page, RandomId.getInstance().make());

            try
            {
                PreviewThumb = Page.GetThumbnail(ThumbAbort, Handle);
            } catch (Exception e) { 
                ApplicationMessage.ShowException(e);
            }

            //PreviewThumbPictureBox.Image = PreviewThumb;
            ImagePreviewButton.BackgroundImage = PreviewThumb;

            if (page.LocalFile != null && page.LocalFile.Exists())
            {
                imageLocation = page.LocalFile.FilePath;
            } else
            {
                if (page.TemporaryFile != null && page.TemporaryFile.Exists())
                {
                    imageLocation = page.TemporaryFile.FilePath;
                } else
                {
                    try
                    {
                        ZipArchiveEntry entry = page.GetCompressedEntry();
                        imageLocation = "\\\\" +entry.Name;
                    } catch
                    {

                    }
                }                
            }

            TextBoxFileLocation.Text = imageLocation;
            PageNameTextBox.Text = Page.Name;
            LabelSize.Text = Page.SizeFormat();
            PageIndexTextbox.Text = (Page.Index + 1).ToString();
            CheckBoxPageDeleted.Checked = Page.Deleted;
            LabelDimensions.Text = Page.Format.W.ToString() + " x " + Page.Format.H.ToString() + " px";
            LabelDpi.Text = Page.Format.DPI.ToString();
            LabelImageFormat.Text = Page.Format.Name;
            LabelImageColors.Text = Page.Format.ColorPalette.Entries.Length.ToString();
            textBoxKey.Text = Page.Key;
            CheckBoxDoublePage.Checked = Page.DoublePage;
           /// LabelBits.Text = Page.Format.
        }

        private void PageSettingsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Page.Close();
        }

        private bool ThumbAbort()
        {
            return true;
        }

        public Page GetResult()
        {
            return Page;
        }

        public void FreeResult()
        {
            Page.DeleteTemporaryFile();
            Page.FreeImage();
            Page = null;
        }

        private void PageNameTextBox_TextChanged(object sender, EventArgs e)
        {
            Page.Name = PageNameTextBox.Text;
        }

        private void PageIndexTextbox_TextChanged(object sender, EventArgs e)
        {
            int newIndex = Convert.ToInt32(PageIndexTextbox.Text);

            Page.Index = newIndex - 1;
            Page.Number = newIndex;
        }

        private void ButtonOk_Click(object sender, EventArgs e)
        {

        }

        private void CheckBoxPageDeleted_CheckedChanged(object sender, EventArgs e)
        {
            Page.Deleted = CheckBoxPageDeleted.Checked;
            if (Page.Deleted)
            {
                Page.Index = -1;
                PageIndexTextbox.Text = "-1";
               
            } else
            {
                Page.Index = Page.OriginalIndex;
                PageIndexTextbox.Text = (Page.OriginalIndex + 1).ToString();
            }

            PageIndexTextbox.Enabled = !Page.Deleted;
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {

        }

        private void ButtonNewKey_Click(object sender, EventArgs e)
        {
            textBoxKey.Text = RandomId.getInstance().make();
            Page.Key = textBoxKey.Text;
        }

        private void textBoxKey_TextChanged(object sender, EventArgs e)
        {
            Page.Key = textBoxKey.Text.Trim();
        }

        private void CheckBoxDoublePage_CheckedChanged(object sender, EventArgs e)
        {
            Page.DoublePage = CheckBoxDoublePage.Checked;
        }

        private void ImagePreviewButton_Click(object sender, EventArgs e)
        {
            ImagePreviewForm pagePreviewForm = new ImagePreviewForm(Page);
            DialogResult dlgResult = pagePreviewForm.ShowDialog();
            pagePreviewForm.Dispose();
        }
    }
}
