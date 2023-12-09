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

        List<Page> Pages;
        Page FirstPage;
        Image PreviewThumb;
        String imageLocation;

        int countDeletedStates = 0;
        int countDoublePageStates = 0;

        long totalSize = 0;

        public PageSettingsForm(List<Page> pages)
        {
            InitializeComponent();

            Pages = new List<Page>();

            foreach (Page p in pages)
            {
                Pages.Add(new Page(p, RandomId.getInstance().make()));
            }

            bool deletedState = false;
            bool doublePageState = false;

            if (pages != null && pages.Count > 0)
            {
                FirstPage = new Page(pages[0], RandomId.getInstance().make());

                try
                {
                    PreviewThumb = FirstPage.GetThumbnail(ThumbAbort, Handle);
                }
                catch (Exception e)
                {
                    ApplicationMessage.ShowException(e);
                }

                ImagePreviewButton.BackgroundImage = PreviewThumb;

                //PreviewThumbPictureBox.Image = PreviewThumb;

                if (pages.Count == 1)
                {
                    if (FirstPage.LocalFile != null && FirstPage.LocalFile.Exists())
                    {
                        if (!FirstPage.Compressed)
                        {
                            imageLocation = FirstPage.LocalFile.FilePath;
                        }
                        else
                        {
                            try
                            {
                                ZipArchiveEntry entry = FirstPage.GetCompressedEntry();
                                imageLocation = "\\\\" + entry.Name;
                            }
                            catch
                            {
                                imageLocation = "?\\" + FirstPage.Name;
                            }
                        }
                    }
                    else
                    {
                        if (FirstPage.TemporaryFile != null && FirstPage.TemporaryFile.Exists())
                        {
                            imageLocation = FirstPage.TemporaryFile.FilePath;
                        }
                        else
                        {
                            try
                            {
                                ZipArchiveEntry entry = FirstPage.GetCompressedEntry();
                                imageLocation = "\\\\" + entry.Name;
                            }
                            catch
                            {
                                imageLocation = "?\\" + FirstPage.Name;
                            }
                        }
                    }

                    TextBoxFileLocation.Text = imageLocation;
                    PageNameTextBox.Text = FirstPage.Name;
                    LabelSize.Text = FirstPage.SizeFormat();
                    PageIndexTextbox.Text = (FirstPage.Index + 1).ToString();
                    CheckBoxPageDeleted.Checked = FirstPage.Deleted;
                    LabelDimensions.Text = FirstPage.Format.W.ToString() + " x " + FirstPage.Format.H.ToString() + " px";
                    LabelDpi.Text = FirstPage.Format.DPI.ToString();
                    LabelImageFormat.Text = FirstPage.Format.Name;
                    LabelImageColors.Text = FirstPage.Format.ColorPalette.Entries.Length.ToString();
                    textBoxKey.Text = FirstPage.Key;
                    CheckBoxDoublePage.Checked = FirstPage.DoublePage;
                } else
                {
                    PageIndexTextbox.Enabled = false;
                    PageNameTextBox.Enabled = false;
                    textBoxKey.Enabled = false;

                    if (pages.Count > 1)
                    {
                        ImagePreviewButton.Text = pages.Count.ToString() + " Pages";
                        ImagePreviewButton.BackgroundImage = null;
                    }

                    LabelDimensions.Text = "--";
                    LabelDpi.Text = "--";

                    foreach (Page page in pages)
                    {
                        if (deletedState != page.Deleted)
                        {
                            countDeletedStates++;
                            deletedState = page.Deleted;
                        }

                        if (doublePageState != page.DoublePage)
                        {
                            countDoublePageStates++;
                            doublePageState = page.DoublePage;
                        }

                        totalSize += page.Size;
                    }

                    LabelSize.Text = Program.ProjectModel.SizeFormat(totalSize);
                    TextBoxFileLocation.Text = pages.Count.ToString() + " Pages selected";

                    if (countDeletedStates < 2)
                    {
                        CheckBoxPageDeleted.Checked = deletedState;
                    } else
                    {
                        CheckBoxPageDeleted.CheckState = CheckState.Indeterminate;
                    }

                    if (countDoublePageStates < 2)
                    {
                        CheckBoxDoublePage.Checked = doublePageState;
                    } else
                    {
                        CheckBoxDoublePage.CheckState = CheckState.Indeterminate;
                    }
                }
            }

            
           /// LabelBits.Text = Page.Format.
        }

        private void PageSettingsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            foreach (Page page in Pages)
            {
                page.Close();
            }
            
        }

        private bool ThumbAbort()
        {
            return true;
        }

        public List<Page> GetResult()
        {
            return Pages;
        }

        public void FreeResult()
        {
            foreach (Page page in Pages)
            {
                page.DeleteTemporaryFile();
                page.FreeImage();
            }
        }

        private void PageNameTextBox_TextChanged(object sender, EventArgs e)
        {

            if (Pages.Count == 1)
            {
                Pages[0].Name = PageNameTextBox.Text;
            }
        }

        private void PageIndexTextbox_TextChanged(object sender, EventArgs e)
        {
            int newIndex = Convert.ToInt32(PageIndexTextbox.Text);

            if (Pages.Count == 1)
            {
                Pages[0].Index = newIndex - 1;
                Pages[0].Number = newIndex;
            }
        }

        private void ButtonOk_Click(object sender, EventArgs e)
        {

        }

        private void CheckBoxPageDeleted_CheckedChanged(object sender, EventArgs e)
        {

            foreach (Page page in Pages)
            {
                if (CheckBoxPageDeleted.CheckState != CheckState.Indeterminate)
                {
                    page.Deleted = CheckBoxPageDeleted.CheckState == CheckState.Checked;
                }

                if (page.Deleted)
                {
                    page.Index = -1;
                    if (Pages.Count == 1)
                    {
                        PageIndexTextbox.Text = "-1";
                    }

                }
                else
                {
                    page.Index = page.OriginalIndex;
                    if (Pages.Count == 1)
                    {
                        PageIndexTextbox.Text = (page.OriginalIndex + 1).ToString();
                    }
                }

                if (Pages.Count == 1)
                {
                    PageIndexTextbox.Enabled = !page.Deleted;
                }

            }          
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {

        }

        private void ButtonNewKey_Click(object sender, EventArgs e)
        {
            if (Pages.Count == 1)
            {
                textBoxKey.Text = RandomId.getInstance().make();
                Pages[0].Key = textBoxKey.Text;
            }
        }

        private void textBoxKey_TextChanged(object sender, EventArgs e)
        {
            if (Pages.Count == 1)
            {
                Pages[0].Key = textBoxKey.Text.Trim();
            }
        }

        private void CheckBoxDoublePage_CheckedChanged(object sender, EventArgs e)
        {
            foreach (Page page in Pages)
            {
                if (CheckBoxDoublePage.CheckState != CheckState.Indeterminate)
                {
                    page.DoublePage = CheckBoxDoublePage.CheckState == CheckState.Checked;
                }
            }
        }

        private void ImagePreviewButton_Click(object sender, EventArgs e)
        {
            ImagePreviewForm pagePreviewForm = new ImagePreviewForm(Pages[0]);
            DialogResult dlgResult = pagePreviewForm.ShowDialog();
            pagePreviewForm.Dispose();
        }

        private void PageSettingsForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Close();
            }
        }
    }
}
