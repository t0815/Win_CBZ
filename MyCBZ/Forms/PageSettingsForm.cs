﻿using System;
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
using System.Runtime.Versioning;

namespace Win_CBZ.Forms
{
    [SupportedOSPlatform("windows")]
    internal partial class PageSettingsForm : Form
    {

        List<Page> Pages;
        List<Page> SelectedPages;
        Page FirstPage;

        String imageLocation;

        int countDeletedStates = 0;
        int countDoublePageStates = 0;
        int countDpiStates = 0;
        int countTypeStates = 0;
        int countLocations = 0;
        int countCompressedStates = 0;

        ArrayList locationsList = new ArrayList();
        ArrayList typesList = new ArrayList();
        ArrayList dpiList = new ArrayList();
        ArrayList dimensionsList = new ArrayList();
        ArrayList imageTypeList = new ArrayList();

        long totalSize = 0;

        public PageSettingsForm(List<Page> pages)
        {
            InitializeComponent();

            SelectedPages = new List<Page>(pages);

            /// LabelBits.Text = Page.Format.
        }

        private void PageSettingsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            FirstPage?.FreeImage();

            foreach (Page page in Pages)
            {
                page.FreeImage();
                page.IsMemoryCopy = false;

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
                }
                catch (ApplicationException ex)
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
                    if (newIndex > -1)
                    {
                        Pages[0].Index = newIndex - 1;
                    }
                    else
                    {
                        Pages[0].Index = -1;
                    }

                    Pages[0].Number = newIndex;
                }
            }
            catch { }
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
            DialogResult = DialogResult.Cancel;
        }

        private void ButtonNewKey_Click(object sender, EventArgs e)
        {
            if (Pages.Count == 1)
            {
                textBoxKey.Text = RandomId.GetInstance().Make();
                Pages[0].Key = textBoxKey.Text;
            }
            else if (Pages.Count > 1)
            {
                DialogResult result = ApplicationMessage.ShowConfirmation("Are you sure you want to regenerate Keys for all Pages?", "Regenerate all Keys", ApplicationMessage.DialogType.MT_CONFIRMATION, ApplicationMessage.DialogButtons.MB_YES | ApplicationMessage.DialogButtons.MB_NO);
                if (result == DialogResult.Yes)
                {
                    foreach (Page page in Pages)
                    {
                        page.Key = RandomId.GetInstance().Make();
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
                ImagePreviewForm pagePreviewForm = new ImagePreviewForm(Pages[0]);
                DialogResult dlgResult = pagePreviewForm.ShowDialog();
                pagePreviewForm.Dispose();
            }
        }

        private void PageSettingsForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                DialogResult = DialogResult.Cancel;
                Close();
            }
        }

        private void TabControlPageProperties_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (TabControlPageProperties.SelectedIndex == 1 && Pages.Count == 1)
            {
                try
                {
                    StringReader xsltManifest = new StringReader(Properties.Resources.ResourceManager.GetString(Uri.EscapeDataString("defaults").ToLowerInvariant()));
                    XmlReader xslReader = XmlReader.Create(xsltManifest);
                    XslCompiledTransform xTrans = new XslCompiledTransform();
                    xTrans.Load(xslReader);

                    // Read the xml string.
                    XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
                    xmlReaderSettings.ConformanceLevel = ConformanceLevel.Fragment;
                    MemoryStream sr = FirstPage.Serialize(Program.ProjectModel.ProjectGUID, false, true);
                    XmlReader xReader = XmlReader.Create(sr, xmlReaderSettings);

                    // Transform the XML data
                    MemoryStream ms = new MemoryStream();
                    //xTrans.OutputSettings.
                    xTrans.Transform(xReader, null, ms);

                    ms.Position = 0;
                    // Set to the document stream
                    //ms.CopyTo(docStream);
                    TextReader tr = new StreamReader(ms);

                    string xml = tr.ReadToEnd();

                    metaDataView.DocumentText = xml;

                    //docStream.Position = 0;
                    //metaDataView.DocumentStream?.Close();
                    //metaDataView.DocumentStream?.Dispose();
                    //metaDataView.DocumentStream = null;
                    //metaDataView.AllowNavigation = false;
                    //metaDataView.Navigate("about:blank");
                    //metaDataView.Update();
                    ///metaDataView.
                    //metaDataView.Document.OpenNew(false);

                    //metaDataView.Refresh();



                }
                catch (Exception ex)
                {
                    ApplicationMessage.ShowException(ex);
                }
            }
        }

        private void PageSettingsForm_Load(object sender, EventArgs e)
        {

        }

        private void ComboBoxPageType_TextUpdate(object sender, EventArgs e)
        {

        }

        private void ComboBoxPageType_TextChanged(object sender, EventArgs e)
        {
            foreach (Page page in Pages)
            {
                page.ImageType = ComboBoxPageType.Text;
            }
        }

        private void PageSettingsForm_Load_1(object sender, EventArgs e)
        {

        }

        private void ButtonReloadImage_Click(object sender, EventArgs e)
        {
            ProgressBarReload.Maximum = Pages.Count;
            int index = 0;

            //PreviewThumb?.Dispose();
            ImagePreviewButton.BackgroundImage?.Dispose();

            ImagePreviewButton.BackgroundImage = null;
            //PreviewThumb = null;

            foreach (Page page in Pages)
            {
                try
                {
                    page.Reload();
                }
                catch (Exception ex)
                {
                    ApplicationMessage.ShowException(ex);
                }
                ProgressBarReload.Value = index;
                index++;
            }

            bool deletedState = false;
            bool doublePageState = false;
            bool compressedState = false;

            if (Pages != null && Pages.Count > 0)
            {

                FirstPage = new Page(Pages[0], true);   // todo: maybe use memory copy here too and just set real memorycopy state

                if (Pages.Count == 1)
                {
                    Task<Bitmap> imageTask = new Task<Bitmap>(() =>
                    {

                        Bitmap thumb = null;

                        try
                        {
                            thumb = FirstPage.GetThumbnailBitmap();
                        }
                        catch (PageMemoryIOException pme)
                        {
                            //ButtonOk.Enabled = false;
                            if (pme.ShowErrorDialog)
                            {
                                //ApplicationMessage.ShowException(pme);
                            }
                        }
                        catch (Exception xe)
                        {
                            //ButtonOk.Enabled = false;
                            //ApplicationMessage.ShowException(xe);
                        }
                        finally
                        {
                            FirstPage.FreeImage();
                        }

                        return thumb;
                    });

                    imageTask.ContinueWith(t =>
                    {
                        try
                        {
                            if (t.IsCompletedSuccessfully && t.Result != null)
                            {
                                Invoke(new Action(() =>
                                {
                                    ImagePreviewButton.BackgroundImage = Image.FromHbitmap(t.Result.GetHbitmap());

                                    LabelDimensions.Text = FirstPage.Format.W.ToString() + " x " + FirstPage.Format.H.ToString() + " px";
                                    LabelDpi.Text = FirstPage.Format.DPI.ToString();
                                    LabelImageFormat.Text = FirstPage.Format.Name;
                                    if (FirstPage.Format?.ColorPalette != null)
                                    {
                                        LabelImageColors.Text = FirstPage.Format.ColorPalette.Entries.Length.ToString();
                                    }
                                }));
                            }
                        }
                        catch (Exception ee)
                        {
                            ApplicationMessage.ShowException(ee);
                        }
                        finally
                        {
                            FirstPage.FreeImage();
                        }
                    });

                    imageTask.Start();
                }


                if (Pages.Count == 1)
                {
                    if (FirstPage == null)
                    {
                        FirstPage = new Page();
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

                    ComboBoxPageType.Text = FirstPage.ImageType;
                    TextBoxFileLocation.Text = imageLocation;
                    PageNameTextBox.Text = FirstPage.Name;
                    LabelSize.Text = FirstPage.SizeFormat();
                    PageIndexTextbox.Text = (FirstPage.Index + 1).ToString();
                    CheckBoxPageDeleted.Checked = FirstPage.Deleted;

                    if (MetaDataVersionFlavorHandler.GetInstance().TargetVersion() == MetaData.PageIndexVersion.VERSION_2)
                    {
                        textBoxKey.Text = FirstPage.Key;
                        textBoxKey.Enabled = true;
                        ButtonNewKey.Enabled = true;
                        label10.Enabled = true;
                    }
                    else
                    {
                        textBoxKey.Text = "";
                        textBoxKey.Enabled = false;
                        ButtonNewKey.Enabled = false;
                        label10.Enabled = false;
                    }

                    CheckBoxDoublePage.Checked = FirstPage.DoublePage;
                    IsCompressedLabel.Text = FirstPage.Compressed ? "Yes" : "No";
                }
                else
                {
                    PageIndexTextbox.Enabled = false;
                    PageNameTextBox.Enabled = false;
                    textBoxKey.Enabled = false;

                    if (Pages.Count > 1)
                    {
                        ImagePreviewButton.Text = "[" + Pages.Count.ToString() + " Pages]";
                        ImagePreviewButton.BackgroundImage = null;
                    }

                    textBoxKey.Text = "";
                    LabelDimensions.Text = "Multiple dimensions";
                    LabelDpi.Text = "Multiple";
                    LabelImageFormat.Text = "Multiple formats";
                    TextBoxFileLocation.Text = "";

                    countDeletedStates = 0;
                    countCompressedStates = 0;
                    countDoublePageStates = 0;

                    totalSize = 0;

                    foreach (Page page in Pages)
                    {

                        if (deletedState != page.Deleted)
                        {
                            countDeletedStates++;
                            deletedState = page.Deleted;
                        }

                        if (compressedState != page.Compressed)
                        {
                            countCompressedStates++;
                            compressedState = page.Compressed;
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

                        if (imageTypeList.IndexOf(page.ImageType) == -1)
                        {
                            imageTypeList.Add(page.ImageType);
                        }

                        totalSize += page.Size;
                    }

                    LabelSize.Text = Program.ProjectModel.SizeFormat(totalSize);


                    if (typesList.Count == 1)
                    {
                        LabelImageFormat.Text = typesList[0].ToString();
                    }

                    if (imageTypeList.Count == 1)
                    {
                        ComboBoxPageType.Text = imageTypeList[0].ToString();
                    }

                    if (dimensionsList.Count == 1)
                    {
                        LabelDimensions.Text = dimensionsList[0].ToString();
                    }

                    if (dpiList.Count == 1)
                    {
                        LabelDpi.Text = dpiList[0].ToString();
                    }

                    if (countCompressedStates < 2)
                    {

                    }
                    else
                    {

                    }

                    if (countDeletedStates < 2)
                    {
                        CheckBoxPageDeleted.Checked = deletedState;
                    }
                    else
                    {
                        CheckBoxPageDeleted.CheckState = CheckState.Indeterminate;
                    }

                    if (countDoublePageStates < 2)
                    {
                        CheckBoxDoublePage.Checked = doublePageState;
                    }
                    else
                    {
                        CheckBoxDoublePage.CheckState = CheckState.Indeterminate;
                    }


                }
            }
        }

        private void PageSettingsForm_Shown(object sender, EventArgs e)
        {

            foreach (String pageType in MetaDataEntryPage.PageTypes)
            {
                ComboBoxPageType.Items.Add(pageType);
            }

            Pages = new List<Page>();
            Page pageList = null;

            try
            {
                foreach (Page p in SelectedPages)
                {
                    bool realMemoryCopyState = p.IsMemoryCopy;
                    pageList = new Page(p, true, false);

                    Pages.Add(pageList);
                    pageList.IsMemoryCopy = realMemoryCopyState;
                    //p.FreeImage();
                }
            }
            catch (Exception ex)
            {
                ApplicationMessage.ShowException(ex);

                ButtonOk.Enabled = false;

                return;
            }


            bool deletedState = false;
            bool doublePageState = false;
            bool compressedState = false;

            if (SelectedPages != null && SelectedPages.Count > 0)
            {

                FirstPage = new Page(SelectedPages[0], true);   // todo: maybe use memory copy here too and just set real memorycopy state

                if (SelectedPages.Count == 1)
                {
                    Task<Bitmap> imageTask = new Task<Bitmap>(() =>
                    {
                        Bitmap thumb = null;

                        try
                        {
                            thumb = FirstPage.GetThumbnailBitmap();
                        }
                        catch (PageMemoryIOException pme)
                        {
                            Invoke(new Action(() =>
                            {
                                ButtonOk.Enabled = false;
                                if (pme.ShowErrorDialog)
                                {
                                    ApplicationMessage.ShowException(pme);
                                }
                            }));


                        }
                        catch (Exception ex)
                        {
                            Invoke(new Action(() =>
                            {
                                ButtonOk.Enabled = false;
                                ApplicationMessage.ShowException(ex);
                            }));


                        }

                        return thumb;
                    });

                    imageTask.ContinueWith(t =>
                    {
                        try
                        {
                            if (t.IsCompletedSuccessfully && t.Result != null)
                            {
                                Invoke(new Action(() =>
                                {
                                    ImagePreviewButton.BackgroundImage = Image.FromHbitmap(t.Result.GetHbitmap());

                                    LabelDimensions.Text = FirstPage.Format.W.ToString() + " x " + FirstPage.Format.H.ToString() + " px";
                                    LabelDpi.Text = FirstPage.Format.DPI.ToString();
                                    LabelImageFormat.Text = FirstPage.Format.Name;
                                    if (FirstPage.Format?.ColorPalette != null)
                                    {
                                        LabelImageColors.Text = FirstPage.Format.ColorPalette.Entries.Length.ToString();
                                    }
                                }));
                            }
                        }
                        catch (Exception ee)
                        {
                            ApplicationMessage.ShowException(ee);
                        }
                        finally
                        {
                            FirstPage.FreeImage();
                        }
                    });

                    imageTask.Start();
                }
            
                // 

                //PreviewThumbPictureBox.Image = PreviewThumb;

                if (SelectedPages.Count == 1)
                {
                    if (FirstPage == null)
                    {
                        FirstPage = new Page();
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

                    ComboBoxPageType.Text = FirstPage.ImageType;
                    TextBoxFileLocation.Text = imageLocation;
                    PageNameTextBox.Text = FirstPage.Name;
                    LabelSize.Text = FirstPage.SizeFormat();
                    PageIndexTextbox.Text = (FirstPage.Index + 1).ToString();
                    CheckBoxPageDeleted.Checked = FirstPage.Deleted;
                    

                    if (MetaDataVersionFlavorHandler.GetInstance().TargetVersion() == MetaData.PageIndexVersion.VERSION_2)
                    {
                        textBoxKey.Text = FirstPage.Key;
                        textBoxKey.Enabled = true;
                        ButtonNewKey.Enabled = true;
                        label10.Enabled = true;
                    }
                    else
                    {
                        textBoxKey.Text = "";
                        textBoxKey.Enabled = false;
                        ButtonNewKey.Enabled = false;
                        label10.Enabled = false;
                    }

                    CheckBoxDoublePage.Checked = FirstPage.DoublePage;
                    IsCompressedLabel.Text = FirstPage.Compressed ? "Yes" : "No";
                }
                else
                {
                    PageIndexTextbox.Enabled = false;
                    PageNameTextBox.Enabled = false;
                    textBoxKey.Enabled = false;

                    if (SelectedPages.Count > 1)
                    {
                        ImagePreviewButton.Text = "[" + SelectedPages.Count.ToString() + " Pages]";
                        ImagePreviewButton.BackgroundImage = null;
                    }

                    textBoxKey.Text = "";
                    LabelDimensions.Text = "Multiple dimensions";
                    LabelDpi.Text = "Multiple";
                    LabelImageFormat.Text = "Multiple formats";
                    TextBoxFileLocation.Text = "";

                    foreach (Page page in SelectedPages)
                    {

                        if (deletedState != page.Deleted)
                        {
                            countDeletedStates++;
                            deletedState = page.Deleted;
                        }

                        if (compressedState != page.Compressed)
                        {
                            countCompressedStates++;
                            compressedState = page.Compressed;
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

                        if (imageTypeList.IndexOf(page.ImageType) == -1)
                        {
                            imageTypeList.Add(page.ImageType);
                        }

                        totalSize += page.Size;
                    }

                    LabelSize.Text = Program.ProjectModel.SizeFormat(totalSize);


                    if (typesList.Count == 1)
                    {
                        LabelImageFormat.Text = typesList[0].ToString();
                    }

                    if (imageTypeList.Count == 1)
                    {
                        ComboBoxPageType.Text = imageTypeList[0].ToString();
                    }

                    if (dimensionsList.Count == 1)
                    {
                        LabelDimensions.Text = dimensionsList[0].ToString();
                    }

                    if (dpiList.Count == 1)
                    {
                        LabelDpi.Text = dpiList[0].ToString();
                    }

                    if (countCompressedStates < 2)
                    {

                    }
                    else
                    {

                    }

                    if (countDeletedStates < 2)
                    {
                        CheckBoxPageDeleted.Checked = deletedState;
                    }
                    else
                    {
                        CheckBoxPageDeleted.CheckState = CheckState.Indeterminate;
                    }

                    if (countDoublePageStates < 2)
                    {
                        CheckBoxDoublePage.Checked = doublePageState;
                    }
                    else
                    {
                        CheckBoxDoublePage.CheckState = CheckState.Indeterminate;
                    }


                }
            }

        }
    }
}
