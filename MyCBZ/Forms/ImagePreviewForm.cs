using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Win_CBZ.Img;
using Win_CBZ.Models;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Win_CBZ
{
    [SupportedOSPlatform("windows")]
    internal partial class ImagePreviewForm : Form
    {
        int currentIndex = 0;
        string currentId = null;

        Page displayPage = null;

        private int minW = 0;
        private int minH = 0;

        private Task<bool> chapterExtractionTask = null;

        private bool showChapters = false;

        public ImagePreviewForm(Page page)
        {
            InitializeComponent();

            try
            {
                displayPage = new Page(page, true, true);  // todo: memory copy?

                currentIndex = page.Index;
                currentId = page.Id;
                TextBoxJumpPage.Text = displayPage.Number.ToString();
                if (displayPage.TemporaryFile == null || !displayPage.TemporaryFile.Exists())
                {
                    try
                    {
                        displayPage.CreateLocalWorkingCopy();
                    }
                    catch (ApplicationException ae)
                    {

                        if (ae.ShowErrorDialog)
                        {
                            ApplicationMessage.ShowException(ae);
                        }

                        displayPage?.FreeStreams();

                        return;
                    }
                }

                if (displayPage.TemporaryFile.Exists())
                {
                    PageImagePreview.Image = Image.FromStream(displayPage.GetImageStream());
                    HandleWindowSize(displayPage);
                }
                else
                {
                    PageImagePreview.ImageLocation = null;
                }

                chapterExtractionTask = new Task<bool>(() =>
                {
                    try
                    {
                        string currentBookmark = "";
                        int total = Program.ProjectModel.Pages.Count;
                        int currentIndex = 0;

                        Invoke(new Action(() =>
                        {
                            ListboxChapters.Items.Clear();
                            ChapterImagesList.Images.Clear();
                            ExtractionProgressBar.Value = 0;
                            ExtractionProgressBar.Maximum = total;
                            ExtractionProgressBar.Visible = true;
                            toolStripLabel3.Visible = true;
                        }));

                        Program.ProjectModel.Pages.ForEach(p =>
                        {

                            if (currentBookmark != p.Bookmark && p.Bookmark?.Length > 0)
                            {
                                currentBookmark = p.Bookmark;

                                Invoke(new Action(() =>
                                {
                                    ListboxChapters.Items.Add(p);
                                    ChapterImagesList.Images.Add(p.Id, p.GetThumbnail(48, 67));
                                    ExtractionProgressBar.Value = currentIndex;
                                }));

                            }
                            currentIndex++;

                            Thread.Sleep(3); // to avoid UI freeze
                        });

                        return true;
                    }
                    catch (Exception e)
                    {
                        ApplicationMessage.ShowException(e);

                        return false;
                    }
                });

                chapterExtractionTask.ContinueWith(t =>
                {
                    Invoke(new Action(() =>
                    {
                        ExtractionProgressBar.Visible = false;
                        toolStripLabel3.Visible = false;
                       
                    }));
                });

                chapterExtractionTask.Start();
            }
            catch (Exception e2)
            {
                displayPage?.FreeImage();
                displayPage?.FreeStreams();

                ApplicationMessage.ShowException(e2);
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            PageImageFormat targetFormat = null;

            DialogResult r = ExportImageDialog.ShowDialog();
            if (r == DialogResult.OK)
            {
                using (Stream fis = displayPage.GetImageStream())
                {
                    LocalFile localFile = new LocalFile(ExportImageDialog.FileName);
                    targetFormat = new PageImageFormat(localFile.FileExtension);
                    using (Stream fos = localFile.LocalFileInfo.OpenWrite())
                    {
                        ImageOperations.ConvertImage(fis, fos, targetFormat);
                        fos.Close();
                    }
                }
            }
        }

        private void PageImagePreview_LoadCompleted(object sender, AsyncCompletedEventArgs e)
        {
            SplitBoxPageView.Panel1.VerticalScroll.Value = 0;
            HandleWindowSize(displayPage);
        }

        private void ImagePreviewForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (PageImagePreview.Image != null)
            {
                PageImagePreview.Image.Dispose();
            }
            PageImagePreview.Dispose();

            displayPage?.FreeStreams();
            displayPage?.FreeImage();
        }

        protected void HandlePageNavigation(int direction, int index = -1)
        {
            Page page = Program.ProjectModel.GetPageById(currentId);
            int newIndex = currentIndex;
            if (index > -1)
            {
                newIndex = index - 1;
            }

            if (displayPage != null)
            {
                displayPage.FreeImage();
                displayPage.FreeStreams();
            }

            if (page != null)
            {
                try
                {
                    displayPage = new Page(Program.ProjectModel.GetNextAvailablePage(newIndex, direction), true);
                    Task.Factory.StartNew(() =>
                    {
                        try
                        {
                            foreach (Page p in ListboxChapters.Items)
                            {
                                if (p.Bookmark == displayPage.Bookmark)
                                {
                                    Invoke(new Action(() =>
                                    {
                                        ListboxChapters.SelectedIndexChanged -= ListboxChapters_SelectedIndexChanged;
                                        ListboxChapters.SelectedItem = p;
                                        ListboxChapters.TopIndex = ListboxChapters.Items.IndexOf(p);
                                        ListboxChapters.SelectedIndexChanged += ListboxChapters_SelectedIndexChanged;
                                    }));
                                    break;
                                }
                            }

                            return true;
                        }
                        catch (Exception e)
                        {
                            ApplicationMessage.ShowException(e);

                            return false;
                        }
                    });

                }
                catch (ApplicationException ae)
                {
                    if (ae.ShowErrorDialog)
                    {
                        ApplicationMessage.ShowException(ae);
                    }
                }
                catch (Exception e)
                {
                    ApplicationMessage.ShowException(e);
                }

            }

            if (displayPage != null)
            {
                if (!displayPage.IsMemoryCopy && (displayPage.TemporaryFile == null || !displayPage.TemporaryFile.Exists()))
                {
                    try
                    {
                        displayPage.CreateLocalWorkingCopy();
                    }
                    catch (Exception)
                    {
                        if (page != null)
                        {
                            currentId = page.Id;
                            currentIndex = page.Index;
                        }

                        return;
                    }
                }

                if (displayPage.IsMemoryCopy || (displayPage.TemporaryFile != null && displayPage.TemporaryFile.Exists()))
                {
                    try
                    {
                        PageImagePreview.Image = Image.FromStream(displayPage.GetImageStream());
                    }
                    catch (ApplicationException ae)
                    {
                        if (ae.ShowErrorDialog)
                        {
                            ApplicationMessage.ShowException(ae);
                        }
                    }
                    catch (Exception ex)
                    {
                        ApplicationMessage.ShowException(ex);
                    }
                    //PageImagePreview.LoadAsync();
                    HandleWindowSize(displayPage);

                }
                else
                {
                    PageImagePreview.ImageLocation = null;
                }

                currentId = displayPage.Id;
                currentIndex = displayPage.Index;
                TextBoxJumpPage.Text = displayPage.Number.ToString();
            }
        }

        private void HandleWindowSize(Page page)
        {
            if (page != null)
            {
                if (page.Format.W > minW)
                {
                    if (page.Format.W + 40 > Screen.FromHandle(this.Handle).WorkingArea.Width)
                    {
                        Width = Screen.FromHandle(this.Handle).WorkingArea.Width - Location.X;
                    }
                    else
                    {
                        Width = page.Format.W + 40;
                    }
                }

                if (page.Format.H > minH)
                {
                    if (page.Format.H - Location.Y - PreviewToolStrip.Height > Screen.FromHandle(this.Handle).WorkingArea.Height)
                    {
                        Height = Screen.FromHandle(this.Handle).WorkingArea.Height - Location.Y - PreviewToolStrip.Height;
                    }
                    else
                    {
                        Height = page.Format.H - Location.Y - PreviewToolStrip.Height;
                    }
                }
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

            if (e.KeyCode == Keys.Escape)
            {
                Close();
            }
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

        private void PageImagePreview_BindingContextChanged(object sender, EventArgs e)
        {
            SplitBoxPageView.Panel1.VerticalScroll.Value = 0;
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            HandlePageNavigation(-1);
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            HandlePageNavigation(1);
        }

        private void ImagePreviewForm_Load(object sender, EventArgs e)
        {

        }

        private void ToolStripButton2_Click(object sender, EventArgs e)
        {
            SplitBoxPageView.Panel2Collapsed = !toolStripButton2.Checked;
        }

        private void ListboxChapters_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0)
            {
                return;
            }

            Page page = ListboxChapters.Items[e.Index] as Page;

            if (page != null)
            {
                Color backgroundColor = Color.White;
                Color textColor = Color.Black;
                System.Drawing.Pen pen = new System.Drawing.Pen(textColor, 1);

                System.Drawing.SolidBrush tb = new SolidBrush(Color.Black);

                Font f = SystemFonts.CaptionFont;

                if (e.State.HasFlag(DrawItemState.Selected))
                {
                    backgroundColor = Color.Gold;
                }
                else
                {
                    backgroundColor = SystemColors.Window;
                }



                System.Drawing.SolidBrush bg = new SolidBrush(backgroundColor);

                e.Graphics.FillRectangle(bg, new Rectangle(e.Bounds.Left, e.Bounds.Top, e.Bounds.Width, e.Bounds.Height));
                e.Graphics.DrawString(page.Bookmark, f, tb, e.Bounds.X + 56, e.Bounds.Y + 6);
                e.Graphics.DrawString(page.Number.ToString(), f, tb, e.Bounds.X + 56, e.Bounds.Y + 20);
                if (ChapterImagesList.Images.ContainsKey(page.Id))
                {
                    e.Graphics.DrawImage(ChapterImagesList.Images[page.Id], e.Bounds.X + 3, e.Bounds.Y + 3);
                }


            }
        }

        private void ListboxChapters_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ListboxChapters.SelectedIndex > -1)
            {
                Page page = ListboxChapters.SelectedItem as Page;
                if (page != null)
                {
                    currentId = page.Id;
                    TextBoxJumpPage.Text = page.Number.ToString();
                    HandlePageNavigation(1, page.Index);
                }
            }
        }

        private void ImagePreviewForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (chapterExtractionTask != null && chapterExtractionTask.Status == TaskStatus.Running)
            {
                ApplicationMessage.ShowWarning("Please wait until the chapter extraction is finished before closing the preview window.", "Chapter extraction still running!", ApplicationMessage.DialogType.MT_WARNING, ApplicationMessage.DialogButtons.MB_OK);

                e.Cancel = true;
            }
        }
    }
}
