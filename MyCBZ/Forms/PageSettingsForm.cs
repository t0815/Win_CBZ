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
using System.Runtime.Versioning;
using System.Threading;
using Win_CBZ.Hash;
using Win_CBZ.Events;
using Win_CBZ.Exceptions;

namespace Win_CBZ.Forms
{
    [SupportedOSPlatform("windows")]
    internal partial class PageSettingsForm : Form
    {

        bool formShown = false;

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
            int maxIndex = Program.ProjectModel.Pages.Count;

            try
            {
                if (DialogResult == DialogResult.OK)
                {
                
                    if (Pages.Count == 1)
                    {
                        int newIndex = -1;

                        try
                        {
                            newIndex = Convert.ToInt32(PageIndexTextbox.Text);
                        }
                        catch (Exception ex)
                        {
                            throw new PageValidationException(Pages[0], "PageIndexTextbox", "Invalid page index! Index must be nummeric [0-9]!", true);
                        }

                        if (newIndex < 1 || newIndex > Program.ProjectModel.Pages.Count)
                        {
                            throw new PageValidationException(Pages[0], "PageIndexTextbox", "Invalid page index! Value must not be < 1 and not > " + maxIndex.ToString() + ".", true);
                        }

                        ErrorProvider.SetError(PageIndexTextbox, null);

                        if (PageNameTextBox.Text.Trim().Length == 0)
                        {
                            throw new PageValidationException(Pages[0], "PageNameTextBox", "Page name must not be empty!", true);
                        }

                        Page existing = Program.ProjectModel.Pages.Find(p => p.Name.ToLower() == PageNameTextBox.Text.ToLower());
                    
                        if (existing != null) 
                        {
                            if (existing.Id != Pages[0].Id)
                            {
                                throw new PageValidationException(Pages[0], "PageNameTextBox", @"Page with the name [" + PageNameTextBox.Text + "] already exists!\r\nA different page with the same name already exists at Index " + existing.Index + ".", true);
                            }
                        }

                        ErrorProvider.SetError(PageNameTextBox, null);
                    }
                


                    // ------------------ validate page settings ------------------

                }

                FirstPage?.FreeImage();
                FirstPage?.FreeStreams();

                FreeResult();
            } catch (PageValidationException pv)
            {
                ErrorProvider.SetError(this.Controls.Find(pv.ControlName, true)[0], pv.Message);

                e.Cancel = true;

                if (Win_CBZSettings.Default.LogValidationErrors)
                {
                    MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_ERROR, pv.Message);
                }
                
                ApplicationMessage.ShowWarning(pv.Message, "Validation Error", ApplicationMessage.DialogType.MT_WARNING, ApplicationMessage.DialogButtons.MB_OK);
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
            Task<bool> freeResultTask = new Task<bool>(() =>
            {
                bool result = true;

                foreach (Page page in Pages)
                {
                    try
                    {
                        page.FreeImage();
                        page.FreeStreams();
                        page.IsMemoryCopy = false;
                    }
                    catch (ApplicationException)
                    {
                        result = false;
                    }
                }

                return result;

            });

            freeResultTask.Start();
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

                    xTrans.Transform(xReader, null, ms);

                    ms.Position = 0;
                    // Set to the document stream

                    TextReader tr = new StreamReader(ms);

                    string xml = tr.ReadToEnd();

                    metaDataView.DocumentText = xml;
                }
                catch (Exception ex)
                {
                    ApplicationMessage.ShowException(ex);
                }
            }
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

        private void ButtonReloadImage_Click(object sender, EventArgs e)
        {
            ProgressBarReload.Maximum = Pages.Count;
            int index = 0;

            ImagePreviewButton.BackgroundImage?.Dispose();

            ImagePreviewButton.BackgroundImage = null;

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
                        return FirstPage.GetThumbnailBitmap();
                    });

                    imageTask.ContinueWith(t =>
                    {
                        try
                        {
                            if (t.IsCompletedSuccessfully)
                            {
                                Invoke(new Action(() =>
                                {
                                    if (Win_CBZ.Win_CBZSettings.Default.CalculateHash)
                                    {
                                        HashCrc32.Calculate(ref FirstPage);
                                    }

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
                            else
                            {
                                Invoke(new Action(() =>
                                {
                                    //ButtonOk.Enabled = false;
                                    if ((t.Exception.InnerException as ApplicationException).ShowErrorDialog)
                                    {
                                        ApplicationMessage.ShowException(t.Exception);
                                    }
                                }));
                            }
                        }
                        catch (Exception ee)
                        {
                            Invoke(new Action(() =>
                            {
                                ApplicationMessage.ShowException(ee);
                            }));
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
                            catch (Exception)
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

            if (SelectedPages.Count > 0)
            {
                LoadingIndicator.BringToFront();
                LoadingIndicator.Enabled = true;
                LoadingIndicator.Visible = true;
            }

            Task<List<Page>> loadingTask = new Task<List<Page>>(() =>
            {
                List<Page> resultPages = new List<Page>();

                foreach (Page p in SelectedPages)
                {
                    //bool realMemoryCopyState = p.IsMemoryCopy;
                    //try
                    //{
                    pageList = new Page(p, true, false);
                    pageList?.UpdatePageAttributes(p);

                    if (pageList != null)
                    {
                        resultPages.Add(pageList);
                    }
                    //}
                    //catch (PageException pe)
                    //{
                    //if (pe.ShowErrorDialog && SelectedPages.Count == 1) 
                    //{
                    //    ApplicationMessage.ShowException(pe);
                    //}


                    //}
                    //catch (Exception ex)
                    //{

                    //}

                    Thread.Sleep(10);
                    //p.FreeImage();
                }

                return resultPages;
            });

            Task<Tuple<Page, Bitmap, List<Page>>> loadFirstPageTask = loadingTask.ContinueWith<Tuple<Page, Bitmap, List<Page>>>(t =>
            {

                if (t.IsCompletedSuccessfully)
                {
                    if (t.Result != null && t.Result.Count > 0)
                    {

                        FirstPage = new Page(SelectedPages[0], true);   // todo: maybe use memory copy here too and just set real memorycopy state

                        if (SelectedPages.Count == 1)
                        {
                            return Tuple.Create<Page, Bitmap, List<Page>>(FirstPage, FirstPage.GetThumbnailBitmap(), t.Result);
                        }
                        else
                        {
                            return Tuple.Create<Page, Bitmap, List<Page>>(FirstPage, null, t.Result);
                        }
                    }
                }
                else
                {

                }

                return Tuple.Create<Page, Bitmap, List<Page>>(null, null, t.Result);
            });

            Task<bool> uddateImageMetadata = loadFirstPageTask.ContinueWith(t =>
            {
                try
                {
                    if (t.IsCompletedSuccessfully)
                    {
                        FirstPage = t.Result.Item1;
                        Pages = t.Result.Item3;

                        if (t.Result.Item2 != null)
                        {
                            Invoke(new Action(() =>
                            {
                                ImagePreviewButton.BackgroundImage = Image.FromHbitmap(t.Result.Item2.GetHbitmap());

                                LabelDimensions.Text = FirstPage.Format.W.ToString() + " x " + FirstPage.Format.H.ToString() + " px";
                                LabelDpi.Text = FirstPage.Format.DPI.ToString();
                                LabelImageFormat.Text = FirstPage.Format.Name;
                                if (FirstPage.Format?.ColorPalette != null)
                                {
                                    LabelImageColors.Text = FirstPage.Format.ColorPalette.Entries.Length.ToString();
                                }
                            }));
                        }

                        bool deletedState = false;
                        bool doublePageState = false;
                        bool compressedState = false;

                        if (t.Result.Item3.Count == 1)
                        {
                            if (t.Result.Item1 == null)
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
                                    catch (Exception)
                                    {
                                        imageLocation = "?\\" + FirstPage.Name;
                                    }
                                }
                            }

                            Invoke(new Action(() =>
                            {

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
                            }));
                        }
                        else
                        {
                            Invoke(new Action(() =>
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
                            }));
                        }
                    }
                    else
                    {
                        //Invoke(new Action(() =>
                        //{
                        //ButtonOk.Enabled = false;
                        if (t.Exception.InnerExceptions.Count > 0)
                        {
                            try
                            {
                                if ((t.Exception.InnerExceptions[0] as ApplicationException).ShowErrorDialog)
                                {
                                    ApplicationMessage.ShowException(t.Exception.InnerExceptions[0]);
                                }
                            }
                            catch (Exception)
                            {
                                ApplicationMessage.ShowException(t.Exception.InnerException);
                            }
                        }


                        //}));

                        return false;
                    }
                }
                catch (Exception ee)
                {
                    //Invoke(new Action(() =>
                    //{
                    ApplicationMessage.ShowException(ee);
                    //}));

                    return false;
                }
                finally
                {
                    FirstPage?.FreeImage();
                }

                return true;
            });

            Task finalTask = uddateImageMetadata.ContinueWith(t =>
            {
                formShown = true;
                if (t.IsCompletedSuccessfully && t.Result)
                {
                    Invoke(new Action(() =>
                    {
                        ButtonOk.Enabled = true;
                        LoadingIndicator.Enabled = false;
                        LoadingIndicator.Visible = false;
                        LoadingIndicator.SendToBack();
                    }));
                }
            });


            loadingTask.Start();

            //////
            ///

            return;

            /*
            if (SelectedPages != null && SelectedPages.Count > 0)
            {

                FirstPage = new Page(SelectedPages[0], true);   // todo: maybe use memory copy here too and just set real memorycopy state

                if (SelectedPages.Count == 1)
                {
                    Task<Bitmap> imageTask = new Task<Bitmap>(() =>
                    {
                        return FirstPage.GetThumbnailBitmap();
                    });

                    imageTask.ContinueWith(t =>
                    {
                        try
                        {
                            if (t.IsCompletedSuccessfully)
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
                                    LoadingIndicator.Visible = false;
                                }));
                            }
                            else
                            {
                                Invoke(new Action(() =>
                                {
                                    //ButtonOk.Enabled = false;
                                    if ((t.Exception.InnerException as ApplicationException).ShowErrorDialog)
                                    {
                                        ApplicationMessage.ShowException(t.Exception);
                                    }
                                }));
                            }
                        }
                        catch (Exception ee)
                        {
                            Invoke(new Action(() =>
                            {
                                ApplicationMessage.ShowException(ee);
                            }));
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


            }
            */

        }

        private void ComboBoxPageType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!formShown)
            {
                return;
            }

            if (ComboBoxPageType.Tag != null && (int)ComboBoxPageType.Tag == 1)
            {
                ComboBoxPageType.Tag = null;
                return;
            }

            if (Win_CBZSettings.Default.WriteXmlPageIndex == false)
            {
                DialogResult res = ApplicationMessage.ShowConfirmation("Currently writing XML- pageindex is disabled!\r\nCBZ needs to contain XML pageindex in order to define individual pagetypes. Please enable it in Application settings under 'CBZ -> Compatibility' first.", "XML pageindex required", ApplicationMessage.DialogType.MT_INFORMATION, ApplicationMessage.DialogButtons.MB_OK);

                ComboBoxPageType.Tag = 1;
                ComboBoxPageType.Text = "Story";


                return;
            }

            if (Win_CBZSettings.Default.WriteXmlPageIndex && !Program.ProjectModel.MetaData.Exists())
            {
                DialogResult res = ApplicationMessage.ShowConfirmation("Currently no metadata available!\r\nCBZ needs to contain XML metadata (" + Win_CBZSettings.Default.MetaDataFilename + ") in order to define individual pagetypes. Add a new set of Metadata first.", "Metadata required", ApplicationMessage.DialogType.MT_CONFIRMATION, ApplicationMessage.DialogButtons.MB_OK);

                ComboBoxPageType.Tag = 1;
                ComboBoxPageType.Text = "Story";
                return;
                
            }
        }
    }
}
