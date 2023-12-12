using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Xsl;
using System.Xml;
using Win_CBZ.Helper;
using System.Drawing.Imaging;

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
        int countDpiStates = 0;
        int countTypeStates = 0;

        ArrayList typesList = new ArrayList();
        ArrayList dpiList = new ArrayList();
        ArrayList dimensionsList = new ArrayList();

        long totalSize = 0;

        public PageSettingsForm(List<Page> pages)
        {
            InitializeComponent();

            Pages = new List<Page>();

            try {
                foreach (Page p in pages)
                {
                    Pages.Add(p);
                    //p.FreeImage();
                }
            } catch (Exception ex) {
                ApplicationMessage.ShowException(ex);

                return;
            }
            

            bool deletedState = false;
            bool doublePageState = false;

            if (pages != null && pages.Count > 0)
            { 

                try
                {
                    FirstPage = new Page(pages[0]);
                    PreviewThumb = FirstPage.GetThumbnail(ThumbAbort, Handle);
                }
                catch (Exception e)
                {
                    ButtonOk.Enabled = false;
                    ApplicationMessage.ShowException(e);
                }

                ImagePreviewButton.BackgroundImage = PreviewThumb;

                //PreviewThumbPictureBox.Image = PreviewThumb;

                if (pages.Count == 1)
                {
                    if (FirstPage ==  null)
                    {
                        FirstPage= new Page();
                        ButtonOk.Enabled = false;
                    }

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
                        if (!FirstPage.IsMemoryCopy && FirstPage.TemporaryFile != null && FirstPage.TemporaryFile.Exists())
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
                            catch (Exception eentry)
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
                    if (FirstPage.Format?.ColorPalette != null)
                    {
                        LabelImageColors.Text = FirstPage.Format.ColorPalette.Entries.Length.ToString();
                    }
                    
                    textBoxKey.Text = FirstPage.Key;
                    CheckBoxDoublePage.Checked = FirstPage.DoublePage;
                } else
                {
                    PageIndexTextbox.Enabled = false;
                    PageNameTextBox.Enabled = false;
                    textBoxKey.Enabled = false;

                    if (pages.Count > 1)
                    {
                        ImagePreviewButton.Text = "[" + pages.Count.ToString() + " Pages]";
                        ImagePreviewButton.BackgroundImage = null;
                    }

                    textBoxKey.Text = "[" + pages.Count.ToString() + " Pages]";
                    LabelDimensions.Text = "[" + pages.Count.ToString() + " Pages]";
                    LabelDpi.Text = "[" + pages.Count.ToString() + " Pages]";
                    LabelImageFormat.Text = "[" + pages.Count.ToString() + " Pages]";

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

                        if (typesList.IndexOf(page.Format.Name) == -1)
                        {
                            typesList.Add(page.Format.Name);
                        }

                        if (dpiList.IndexOf(page.Format.DPI.ToString()) == -1)
                        {
                            dpiList.Add(page.Format.DPI.ToString());
                        }

                        if (dimensionsList.IndexOf(page.Format.W.ToString() + " x " + page.Format.H.ToString()) == -1)
                        {
                            dimensionsList.Add(page.Format.W.ToString() + " x " + page.Format.H.ToString()); 
                        }

                        totalSize += page.Size;
                    }

                    LabelSize.Text = Program.ProjectModel.SizeFormat(totalSize);
                    TextBoxFileLocation.Text = "[" + pages.Count.ToString() + " Pages selected]";

                    if (typesList.Count == 1)
                    {
                        LabelImageFormat.Text = typesList[0].ToString();
                    }

                    if (dimensionsList.Count == 1)
                    {
                        LabelDimensions.Text = dimensionsList[0].ToString();
                    }

                    if (dpiList.Count == 1) 
                    {
                        LabelDpi.Text = dpiList[0].ToString();
                    }

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

            FirstPage.FreeImage();
           /// LabelBits.Text = Page.Format.
        }

        private void PageSettingsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            FirstPage?.FreeImage();

            foreach (Page page in Pages)
            {
                page.FreeImage();
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
                try
                {
                    page.FreeImage();
                } catch (ApplicationException ex)
                {

                }
                //page.DeleteTemporaryFile();
                
            }
        }

        private void PageNameTextBox_TextChanged(object sender, EventArgs e)
        {

            if (Pages.Count == 1)
            {
                //if (Pages[0].OriginalName == null)
                //{
                //    Pages[0].OriginalName = Pages[0].Name;
                //}

                Pages[0].Name = PageNameTextBox.Text;
            }
        }

        private void PageIndexTextbox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int newIndex = Convert.ToInt32(PageIndexTextbox.Text);

                if (Pages.Count == 1)
                {
                    Pages[0].Index = newIndex - 1;
                    Pages[0].Number = newIndex;
                }
            } catch { }
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
            } else
            {
                DialogResult result = ApplicationMessage.ShowConfirmation("Are you sure you want to regenerate Keys for all Pages?", "Regenerate all Keys", ApplicationMessage.DialogType.MT_CONFIRMATION, ApplicationMessage.DialogButtons.MB_YES | ApplicationMessage.DialogButtons.MB_NO);
                if (result == DialogResult.Yes)
                {
                    foreach (Page page in Pages)
                    {                    
                        page.Key = RandomId.getInstance().make();
                    }
                }
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
            if (Pages.Count > 0)
            {
                Pages[0].FreeImage();
                ImagePreviewForm pagePreviewForm = new ImagePreviewForm(Pages[0]);
                DialogResult dlgResult = pagePreviewForm.ShowDialog();
                pagePreviewForm.Dispose();
            }           
        }

        private void PageSettingsForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Close();
            }
        }

        private void TabControlPageProperties_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (TabControlPageProperties.SelectedIndex == 1 && Pages.Count == 1)
            {
                try
                {
                    StringReader xsltManifest = new StringReader(Properties.Resources.ResourceManager.GetString(Uri.EscapeUriString("defaults").ToLowerInvariant()));
                    XmlReader xslReader = XmlReader.Create(xsltManifest);
                    XslCompiledTransform xTrans = new XslCompiledTransform();
                    xTrans.Load(xslReader);

                    // Read the xml string.
                    XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
                    xmlReaderSettings.ConformanceLevel = ConformanceLevel.Fragment;
                    MemoryStream sr = Pages[0].Serialize(Program.ProjectModel.ProjectGUID, false, true);
                    XmlReader xReader = XmlReader.Create(sr, xmlReaderSettings);

                    // Transform the XML data
                    MemoryStream ms = new MemoryStream();
                    //xTrans.OutputSettings.
                    xTrans.Transform(xReader, null, ms);

                    ms.Position = 0;
                    // Set to the document stream
                    metaDataView.DocumentStream = ms;
                } catch (Exception ex)
                {
                    ApplicationMessage.ShowException(ex);
                }
            }
        }
    }
}
