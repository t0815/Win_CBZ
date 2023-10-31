﻿using System;
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

        private int minW = 0;
        private int minH = 0;

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
                    try
                    {
                        displayPage.CreateLocalWorkingCopy();
                    }
                    catch ( Exception e)
                    {
                        //
                    }               
                }

                if (displayPage.TemporaryFile.Exists())
                {
                    PageImagePreview.Image = Image.FromStream(displayPage.GetImageStream());
                    HandleWindowSize(displayPage);
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

            HandleWindowSize(displayPage);
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
                    try
                    {
                        displayPage.CreateLocalWorkingCopy();
                    } catch (Exception e)
                    {
                        if (page != null)
                        {
                            currentId = page.Id;
                            currentIndex = page.Index;
                        }
                        

                        return;
                    }
                }

                if (displayPage.TemporaryFile != null && displayPage.TemporaryFile.Exists())
                {
                    PageImagePreview.Image = Image.FromStream(displayPage.GetImageStream());
                    HandleWindowSize(displayPage);
                } else
                {
                    PageImagePreview.ImageLocation = null;
                }

                currentId = displayPage.Id;
                currentIndex = displayPage.Index;

                
            }
        }

        private void HandleWindowSize(Page page)
        {
            if (page != null)
            {
                if (page.W > minW)
                {
                    if (page.W + 40 > Screen.FromHandle(this.Handle).WorkingArea.Width)
                    {
                        Width = Screen.FromHandle(this.Handle).WorkingArea.Width  - Location.X;
                    } else
                    {
                        Width = page.W + 40;
                    }
                }

                if (page.H > minH)
                {
                    if (page.H - Location.Y - PreviewToolStrip.Height > Screen.FromHandle(this.Handle).WorkingArea.Height)
                    {
                        Height = Screen.FromHandle(this.Handle).WorkingArea.Height - Location.Y - PreviewToolStrip.Height;
                    }
                    else
                    {
                        Height = page.H - Location.Y - PreviewToolStrip.Height;
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
