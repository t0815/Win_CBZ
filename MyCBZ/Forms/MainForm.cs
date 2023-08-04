using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Compression;
using System.Reflection;
using Win_CBZ.Forms;
using System.Threading;
using System.IO;
using System.Collections.Specialized;

namespace Win_CBZ
{
    public partial class MainForm : Form
    {

        private Thread ClosingTask;

        private Thread OpeningTask;

        private Thread SavingTask;

        private bool WindowClosed = false;

        private bool WindowShown = false;

        private String LastOutputDirectory = "";

        private Thread ThumbnailThread;

        private Thread RequestThumbnailThread;

        private List<Page> ThumbnailPagesSlice;

        public MainForm()
        {
            InitializeComponent();

            Program.ProjectModel = NewProjectModel();

            MessageLogger.Instance.SetHandler(MessageLogged);

            ThumbnailPagesSlice = new List<Page>();
        }

        private ProjectModel NewProjectModel()
        {
            ProjectModel newProjectModel = new ProjectModel(Win_CBZSettings.Default.TempFolderPath);
            
            newProjectModel.ArchiveStatusChanged += ArchiveStateChanged;
            newProjectModel.TaskProgress += TaskProgress;
            newProjectModel.PageChanged += PageChanged;
            newProjectModel.MetaDataLoaded += MetaDataLoaded;
            // newProjectModel.ItemExtracted += ItemExtracted;
            newProjectModel.OperationFinished += OperationFinished;
            newProjectModel.FileOperation += FileOperationHandler;
            newProjectModel.ArchiveOperation += ArchiveOperationHandler;

            newProjectModel.RenameStoryPagePattern = Win_CBZSettings.Default.StoryPageRenamePattern;
            newProjectModel.RenameSpecialPagePattern = Win_CBZSettings.Default.SpecialPageRenamePattern;


            this.Text = Win_CBZSettings.Default.AppName + " (c) Trash_s0Ft";

            return newProjectModel;
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            if (!WindowShown)
            {
                MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_INFO, Win_CBZSettings.Default.AppName + " v" + Win_CBZSettings.Default.Version + "  - Welcome!");

                TextboxStoryPageRenamingPattern.Text = Win_CBZSettings.Default.StoryPageRenamePattern;
                TextboxSpecialPageRenamingPattern.Text = Win_CBZSettings.Default.SpecialPageRenamePattern;

                TogglePagePreviewToolbutton.Checked = Win_CBZSettings.Default.PagePreviewEnabled;
                SplitBoxPageView.Panel1Collapsed = !Win_CBZSettings.Default.PagePreviewEnabled;
                Program.ProjectModel.PreloadPageImages = Win_CBZSettings.Default.PagePreviewEnabled;

                Label placeholderLabel;
                foreach (String placeholder in Win_CBZSettings.Default.RenamerPlaceholders)
                {
                    placeholderLabel = new Label();
                    placeholderLabel.Name = "Label" + placeholder;
                    placeholderLabel.Text = placeholder;
                    placeholderLabel.AutoSize = true;
                    placeholderLabel.Font = new Font(placeholderLabel.Font.Name, 10, placeholderLabel.Font.Style, placeholderLabel.Font.Unit);
                    PlaceholdersFlowPanel.Controls.Add(placeholderLabel);
                }

                Label fnLabel;
                foreach (String fn in Win_CBZSettings.Default.RenamerFunctions)
                {
                    fnLabel = new Label();
                    fnLabel.Name = "Label" + fn;
                    fnLabel.Text = fn;
                    fnLabel.AutoSize = true;
                    fnLabel.Font = new Font(fnLabel.Font.Name, 10, fnLabel.Font.Style, fnLabel.Font.Unit);
                    FunctionsFlowPanel.Controls.Add(fnLabel);
                }

                PlaceholdersFlowPanel.FlowDirection = FlowDirection.LeftToRight;
                PlaceholdersFlowPanel.Refresh();

                FunctionsFlowPanel.FlowDirection = FlowDirection.LeftToRight;
                FunctionsFlowPanel.Refresh();

                ToolButtonSetPageType.Click += TypeSelectionToolStripMenuItem_Click;
                ToolButtonSetPageType.Tag = "FrontCover";

                ToolStripItem newPageTypeItem;
                foreach (String pageType in MetaDataEntryPage.PageTypes)
                {
                    newPageTypeItem = ToolButtonSetPageType.DropDownItems.Add(pageType);
                    newPageTypeItem.Tag = pageType;
                    newPageTypeItem.Click += TypeSelectionToolStripMenuItem_Click;
                }

                WindowShown = true;
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {

            DialogResult openCBFResult = OpenCBFDialog.ShowDialog();

            if (openCBFResult == DialogResult.OK)
            {
                ClosingTask = Program.ProjectModel.Close();

                Task.Factory.StartNew(() =>
                {
                    if (ClosingTask != null)
                    {
                        while (ClosingTask.IsAlive)
                        {
                            System.Threading.Thread.Sleep(100);
                        }
                    }

                    OpeningTask = Program.ProjectModel.Open(OpenCBFDialog.FileName, ZipArchiveMode.Read);
                });
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult saveDialogResult = SaveArchiveDialog.ShowDialog();

            if (saveDialogResult == DialogResult.OK)
            {
                SavingTask = Program.ProjectModel.SaveAs(SaveArchiveDialog.FileName, ZipArchiveMode.Update);
            }
        }

        private void ToolButtonSave_Click(object sender, EventArgs e)
        {
            if (Program.ProjectModel.IsNew && !Program.ProjectModel.IsSaved)
            {
                saveAsToolStripMenuItem_Click(sender, e);
            } else
            {
                SavingTask = Program.ProjectModel.Save();
            }
        }

        private void PageChanged(object sender, PageChangedEvent e)
        {
            
            PagesList.Invoke(new Action(() =>
            {
                if (e.State != PageChangedEvent.IMAGE_STATUS_CLOSED && e.State != PageChangedEvent.IMAGE_STATUS_DELETED)
                {
                    ListViewItem item;
                    ListViewItem existingItem = FindListViewItemForPage(PagesList, e.Image);

                    if (existingItem == null)
                    {
                        item = PagesList.Items.Add(e.Image.Name);
                        item.ImageKey = e.Image.Id;
                        item.SubItems.Add(e.Image.Number.ToString());
                        item.SubItems.Add(e.Image.ImageType.ToString());
                        item.SubItems.Add(e.Image.LastModified.ToString());
                        item.SubItems.Add(e.Image.Size.ToString());
                    }
                    else
                    {
                        item = existingItem;
                        item.Text = e.Image.Name;
                        item.SubItems[1] = new ListViewItem.ListViewSubItem(item, !e.Image.Deleted ? e.Image.Number.ToString() : "-");
                        item.SubItems[2] = new ListViewItem.ListViewSubItem(item, e.Image.ImageType.ToString());
                        item.SubItems[3] = new ListViewItem.ListViewSubItem(item, e.Image.LastModified.ToString());
                        item.SubItems[4] = new ListViewItem.ListViewSubItem(item, e.Image.Size.ToString());
                    }

                    item.Tag = e.Image;
                    item.BackColor = Color.White;

                    if (!e.Image.Compressed)
                    {
                        item.BackColor = HTMLColor.ToColor(Colors.COLOR_LIGHT_ORANGE);
                    }

                    if (e.Image.Changed)
                    {
                        item.BackColor = HTMLColor.ToColor(Colors.COLOR_LIGHT_GREEN);
                    }

                    if (e.Image.Deleted)
                    {
                        item.ForeColor = Color.Silver;
                        item.BackColor = Color.Transparent;
                    }
                }
            }));

            if (TogglePagePreviewToolbutton.Checked)
            {
                if (e.State != PageChangedEvent.IMAGE_STATUS_CLOSED && e.State != PageChangedEvent.IMAGE_STATUS_DELETED)
                {
                    PageView.Invoke(new Action(() =>
                    {
                        CreatePagePreviewFromItem(e.Image);
                    }));
                }
            }
        }

        private ListViewItem FindListViewItemForPage(ExtendetListView owner, Page page)
        {
            foreach (ListViewItem item in owner.Items)
            {
                if (item.Tag.Equals(page))
                {
                    return item;
                }
            }

            return null;
        }

        public void ReloadPreviewThumbs()
        {
            this.Invoke(new Action(() =>
            {
                PageImages.Images.Clear();

                foreach (Page page in Program.ProjectModel.Pages)
                {
                    try
                    {
                        if (!PageImages.Images.ContainsKey(page.Id))
                        {
                            PageImages.Images.Add(page.Id, page.GetThumbnail(ThumbAbort, Handle));
                        }
                        else
                        {
                            PageImages.Images.RemoveByKey(page.Name);
                            PageImages.Images.Add(page.Id, page.GetThumbnail(ThumbAbort, Handle));
                        }
                    } catch (Exception e)
                    {
                        MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_WARNING, "Error generating Thumbnail for Page '" + page.Name + "' (" + page.Id + ") [" + e.Message + "]");
                    }
                }
            }));
        }


        public void RequestThumbnailSlice()
        {
            if (Win_CBZSettings.Default.PagePreviewEnabled)
            {

                if (ThumbnailThread != null)
                {
                    if (ThumbnailThread.IsAlive)
                    {
                        ThumbnailThread.Abort();
                    }
                }

                if (RequestThumbnailThread != null)
                {
                    if (RequestThumbnailThread.IsAlive)
                    {
                        return;
                    }
                }

                RequestThumbnailThread = new Thread(new ThreadStart(LoadThumbnailSlice));
                RequestThumbnailThread.Start();

                /*
                PageView.Invoke(new Action(() =>
                {
                    foreach (ListViewItem pageItem in PagesList.Items)
                    {
                        CreatePagePreviewFromItem((Page)pageItem.Tag);
                    }
                }));
                */
            }
        }

        public void LoadThumbnailSlice()
        {
            this.Invoke(new Action(() =>
            {
                //PageImages.Images.Clear();

                foreach (Page page in ThumbnailPagesSlice)
                {
                    try
                    {
                        if (!PageImages.Images.ContainsKey(page.Id))
                        {
                            PageImages.Images.Add(page.Id, page.GetThumbnail(ThumbAbort, Handle));
                        }
                        else
                        {
                            if (page.ThumbnailInvalidated && PageImages.Images.IndexOfKey(page.Id) > -1)
                            {
                                PageImages.Images[PageImages.Images.IndexOfKey(page.Id)] = page.GetThumbnail(ThumbAbort, Handle);
                                page.ThumbnailInvalidated = false;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_WARNING, "Error generating Thumbnail for Page '" + page.Name + "' (" + page.Id + ") [" + e.Message + "]");
                    }
                }

                ThumbnailPagesSlice.Clear();
            }));
        }


        private ListViewItem CreatePagePreviewFromItem(Page page)
        {
            ListViewItem itemPage;
            ListViewItem existingItem = FindListViewItemForPage(PageView, page);

            /*
            if (!PageImages.Images.ContainsKey(page.Id))
            {
                PageImages.Images.Add(page.Id, page.GetThumbnail(ThumbAbort, Handle));
            }
            else
            {
                //PageImages.Images.RemoveByKey(page.Name);
                //PageImages.Images.Add(page.Id, page.GetThumbnail(ThumbAbort, Handle));
            }
            */

            if (existingItem == null)
            {
                itemPage = PageView.Items.Add("");
                itemPage.ImageKey = page.Id;
                itemPage.SubItems.Add(page.Name);
                itemPage.SubItems.Add(page.Index.ToString());
            }
            else
            {
                itemPage = existingItem;
                itemPage.ImageKey = page.Id;
                itemPage.SubItems[1] = new ListViewItem.ListViewSubItem(itemPage, page.Name);
                itemPage.SubItems[2] = new ListViewItem.ListViewSubItem(itemPage, page.Index.ToString());
            }

            itemPage.Tag = page;

            return itemPage;
        }

        private void MetaDataLoaded(object sender, MetaDataLoadEvent e)
        {
            MetaDataGrid.Invoke(new Action(() =>
            {
                MetaDataGrid.DataSource = e.MetaData;

                BtnAddMetaData.Enabled = false;
                BtnRemoveMetaData.Enabled = true;
                DataGridViewColumn firstCol = MetaDataGrid.Columns.GetFirstColumn(DataGridViewElementStates.Visible);
                if (firstCol != null)
                {
                    DataGridViewColumn secondCol = MetaDataGrid.Columns.GetNextColumn(firstCol, DataGridViewElementStates.Visible, DataGridViewElementStates.None);
                    if (secondCol != null)
                    {
                        firstCol.Width = 150;
                        secondCol.Width = 250;
                    }
                }         
            }));
        }

        private void OperationFinished(object sender, OperationFinishedEvent e)
        {
            toolStripProgressBar.Control.Invoke(new Action(() =>
            {
                toolStripProgressBar.Maximum = 100;
                toolStripProgressBar.Value = 0;
            }));
        }

        private void FileOperationHandler(object sender, FileOperationEvent e)
        {
            toolStripProgressBar.Control.Invoke(new Action(() =>
            {
                toolStripProgressBar.Maximum = 100;
                toolStripProgressBar.Value = 0;
            }));
        }

        private void ArchiveOperationHandler(object sender, ArchiveOperationEvent e)
        {
            toolStripProgressBar.Control.Invoke(new Action(() =>
            {
                toolStripProgressBar.Maximum = e.Total;
                toolStripProgressBar.Value = e.Completed;
            }));
        }

        private void PageOperationHandler(object sender, ArchiveOperationEvent e)
        {
            toolStripProgressBar.Control.Invoke(new Action(() =>
            {
                toolStripProgressBar.Maximum = e.Total;
                toolStripProgressBar.Value = e.Completed;
            }));
        }


        private bool ThumbAbort()
        {

            return true;
        }

        private void TaskProgress(object sender, TaskProgressEvent e)
        {
            try
            {
                if (!WindowClosed)
                {
                    toolStripProgressBar.Control.Invoke(new Action(() =>
                    {
                        toolStripProgressBar.Maximum = e.Total;
                        if (e.Current > -1)
                        {
                            toolStripProgressBar.Value = e.Current;
                        }
                    }));
                }
            }
            catch (Exception)
            {

            }
        }

        private void ArchiveStateChanged(object sender, CBZArchiveStatusEvent e)
        {
            String info = "Ready.";
            String filename = e.ArchiveInfo.FileName;

            try
            {
                if (!WindowClosed)
                {
                    toolStripProgressBar.Control.Invoke(new Action(() =>
                    {
                        toolStripProgressBar.Value = 0;
                    }));
                }
            } catch (Exception)
            {
                //
            }

            switch (e.State)
            {
                case CBZArchiveStatusEvent.ARCHIVE_OPENED:
                    info = "Ready.";
                    break;

                case CBZArchiveStatusEvent.ARCHIVE_OPENING:
                    info = "Reading archive...";
                    break;

                case CBZArchiveStatusEvent.ARCHIVE_CLOSED:
                    filename = "";
                    info = "Ready.";
                    break;

                case CBZArchiveStatusEvent.ARCHIVE_CLOSING:
                    info = "Closing file...";
                    break;

                case CBZArchiveStatusEvent.ARCHIVE_SAVED:
                    info = "Ready.";
                    break;

                case CBZArchiveStatusEvent.ARCHIVE_SAVING:
                    info = "Writing archive...";
                    break;

                case CBZArchiveStatusEvent.ARCHIVE_FILE_ADDED:
                    break;

                case CBZArchiveStatusEvent.ARCHIVE_FILE_RENAMED:
                    break;
            }
          
            try
            {
                //if (this.InvokeRequired)
                //{
                    this.Invoke(new Action(() =>
                    {
                        fileNameLabel.Text = filename;
                        applicationStatusLabel.Text = info;
                        Program.ProjectModel.ArchiveState = e.State;

                        DisableControllsForArchiveState(e.State);
                    }));
                //}
            } catch (Exception)
            {

            }
        }


        private void MessageLogged(object sender, LogMessageEvent e)
        {
            try
            {
                if (!WindowClosed)
                {
                    MessageLogListView.Invoke(new Action(() =>
                    {
                        ListViewItem logItem = MessageLogListView.Items.Add("");
                        logItem.SubItems.Add(e.MessageTime.LocalDateTime.ToString());
                        logItem.SubItems.Add(e.Message);

                        switch (e.Type)
                        {
                            case LogMessageEvent.LOGMESSAGE_TYPE_INFO:
                                logItem.ImageIndex = 0;
                                break;
                            case LogMessageEvent.LOGMESSAGE_TYPE_WARNING:
                                logItem.ImageIndex = 1;
                                break;
                            case LogMessageEvent.LOGMESSAGE_TYPE_ERROR:
                                logItem.ImageIndex = 2;
                                break;
                            default:
                                logItem.ImageIndex = -1;
                                break;
                        }
                    }));
                }
            }
            catch (Exception) {}
        }


        private void NewProject()
        {
            if (Program.ProjectModel != null)
            {
                Program.ProjectModel.New();
            }
        }

        private void DisableControllsForArchiveState(int state) 
        {
            switch (state)
            {
                case CBZArchiveStatusEvent.ARCHIVE_SAVING:
                case CBZArchiveStatusEvent.ARCHIVE_OPENING:
                    newToolStripMenuItem.Enabled = false;
                    openToolStripMenuItem.Enabled = false;
                    ToolButtonNew.Enabled = false;
                    ToolButtonOpen.Enabled = false;
                    addFilesToolStripMenuItem.Enabled = false;
                    ToolButtonAddFiles.Enabled = false;
                    BtnAddMetaData.Enabled = false;
                    BtnRemoveMetaData.Enabled = false;
                    ToolButtonExtractArchive.Enabled = false;
                    ToolButtonAddFolder.Enabled = false;
                    ExtractSelectedPages.Enabled = false;
                    BtnAddMetaData.Enabled = false;
                    BtnRemoveMetaData.Enabled = false;
                    ToolButtonSave.Enabled = false;
                    saveToolStripMenuItem.Enabled = false;
                    break;

                case CBZArchiveStatusEvent.ARCHIVE_OPENED:
                    newToolStripMenuItem.Enabled = true;
                    openToolStripMenuItem.Enabled = true;
                    ToolButtonNew.Enabled = true;
                    ToolButtonOpen.Enabled = true;
                    addFilesToolStripMenuItem.Enabled = true;
                    ToolButtonAddFiles.Enabled = true;
                    ToolButtonExtractArchive.Enabled = true;
                    ExtractSelectedPages.Enabled = true;
                    ToolButtonAddFolder.Enabled = true;
                    BtnAddMetaData.Enabled = Program.ProjectModel.MetaData.Values.Count == 0;
                    BtnRemoveMetaData.Enabled = Program.ProjectModel.MetaData.Values.Count > 0;
                    AddMetaDataRowBtn.Enabled = Program.ProjectModel.MetaData.Values != null;
                    //TextboxStoryPageRenamingPattern.Enabled = true;
                    //TextboxSpecialPageRenamingPattern.Enabled = true;
                    CheckBoxDoRenamePages.Enabled = true;
                    CheckBoxDoRenamePages.Checked = false;
                    ToolButtonSave.Enabled = false;
                    saveToolStripMenuItem.Enabled = false;
                    Program.ProjectModel.IsNew = false; 
                    break;

                case CBZArchiveStatusEvent.ARCHIVE_SAVED:
                    newToolStripMenuItem.Enabled = true;
                    openToolStripMenuItem.Enabled = true;
                    ToolButtonNew.Enabled = true;
                    ToolButtonOpen.Enabled = true;
                    addFilesToolStripMenuItem.Enabled = true;
                    ToolButtonAddFiles.Enabled = true;
                    ToolButtonExtractArchive.Enabled = true;
                    ExtractSelectedPages.Enabled = true;
                    CheckBoxDoRenamePages.Enabled = true;
                    ToolButtonAddFolder.Enabled = true;
                    TextboxStoryPageRenamingPattern.Enabled = true;
                    TextboxSpecialPageRenamingPattern.Enabled = true;
                    ToolButtonSave.Enabled = false;
                    saveToolStripMenuItem.Enabled = false;
                    ExtractSelectedPages.Enabled = true;
                    Program.ProjectModel.IsNew = false;
                    Program.ProjectModel.IsSaved = true;
                    break;

                case CBZArchiveStatusEvent.ARCHIVE_EXTRACTING:
                    newToolStripMenuItem.Enabled = false;
                    openToolStripMenuItem.Enabled = false;
                    ToolButtonNew.Enabled = false;
                    ToolButtonOpen.Enabled = false;
                    addFilesToolStripMenuItem.Enabled = false;
                    ToolButtonAddFiles.Enabled = false;
                    ToolButtonAddFolder.Enabled = false;
                    BtnRemoveMetaData.Enabled = false;
                    ToolButtonExtractArchive.Enabled = false;
                    ExtractSelectedPages.Enabled = false;
                    break;

                case CBZArchiveStatusEvent.ARCHIVE_EXTRACTED:
                    newToolStripMenuItem.Enabled = true;
                    openToolStripMenuItem.Enabled = true;
                    ToolButtonNew.Enabled = true;
                    ToolButtonOpen.Enabled = true;
                    addFilesToolStripMenuItem.Enabled = true;
                    ToolButtonAddFolder.Enabled = true;
                    ToolButtonAddFiles.Enabled = true;
                    ToolButtonExtractArchive.Enabled = true;
                    ExtractSelectedPages.Enabled = true;
                    break;

                case CBZArchiveStatusEvent.ARCHIVE_CLOSING:
                    newToolStripMenuItem.Enabled = false;
                    openToolStripMenuItem.Enabled = false;
                    ToolButtonNew.Enabled = false;
                    ToolButtonOpen.Enabled = false;
                    addFilesToolStripMenuItem.Enabled = false;
                    ToolButtonAddFiles.Enabled = false;
                    BtnAddMetaData.Enabled = false;
                    BtnRemoveMetaData.Enabled = false;
                    ToolButtonAddFolder.Enabled = false;
                    ToolButtonExtractArchive.Enabled = false;
                    ExtractSelectedPages.Enabled = false;
                    ToolButtonSave.Enabled = false;
                    saveToolStripMenuItem.Enabled = false;
                    RemoveMetaData();
                    break;

                case CBZArchiveStatusEvent.ARCHIVE_CLOSED:
                    newToolStripMenuItem.Enabled = true;
                    openToolStripMenuItem.Enabled = true;
                    ToolButtonNew.Enabled = true;
                    ToolButtonOpen.Enabled = true;
                    addFilesToolStripMenuItem.Enabled = true;
                    ToolButtonAddFiles.Enabled = true;
                    ToolButtonRemoveFiles.Enabled = false;
                    ToolButtonMovePageDown.Enabled = false;
                    ToolButtonMovePageUp.Enabled = false;
                    ToolButtonAddFolder.Enabled = false;
                    BtnAddMetaData.Enabled = Program.ProjectModel.MetaData.Values.Count == 0;
                    AddMetaDataRowBtn.Enabled = Program.ProjectModel.MetaData.Values != null;
                    BtnRemoveMetaData.Enabled = Program.ProjectModel.MetaData.Values.Count > 0;
                    TextboxStoryPageRenamingPattern.Enabled = false;
                    TextboxSpecialPageRenamingPattern.Enabled = false;
                    CheckBoxDoRenamePages.Enabled = false;
                    CheckBoxDoRenamePages.Checked = false;
                    ToolButtonSave.Enabled = false;
                    saveToolStripMenuItem.Enabled = false;
                    RemoveMetaData();
                    break;

                case CBZArchiveStatusEvent.ARCHIVE_FILE_ADDED:
                    CheckBoxDoRenamePages.Enabled = true;
                    CheckBoxDoRenamePages.Checked = false;
                    ToolButtonSave.Enabled = true;
                    saveToolStripMenuItem.Enabled = true;
                    break;

                case CBZArchiveStatusEvent.ARCHIVE_FILE_DELETED:
                case CBZArchiveStatusEvent.ARCHIVE_FILE_RENAMED:
                case CBZArchiveStatusEvent.ARCHIVE_FILE_UPDATED:
                case CBZArchiveStatusEvent.ARCHIVE_METADATA_ADDED:
                case CBZArchiveStatusEvent.ARCHIVE_METADATA_CHANGED:
                case CBZArchiveStatusEvent.ARCHIVE_METADATA_DELETED:
                    ToolButtonSave.Enabled = true;
                    saveToolStripMenuItem.Enabled = true;                   
                    break;

            }
        }

        private void ClearProject()
        {
            Task.Factory.StartNew(() =>
            {
                if (OpeningTask != null)
                {
                    while (OpeningTask.IsAlive)
                    {
                        System.Threading.Thread.Sleep(50);
                    }
                }

                if (SavingTask != null)
                {
                    while (ClosingTask.IsAlive)
                    {
                        System.Threading.Thread.Sleep(50);
                    }
                }

                if (ThumbnailThread != null)
                {
                    while (ThumbnailThread.IsAlive)
                    {
                        System.Threading.Thread.Sleep(50);
                    }
                }

                ClosingTask = Program.ProjectModel.Close();
            });
        }

        private void NewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!WindowClosed && !ArchiveProcessing())
            {
                if (Program.ProjectModel.IsChanged && !Program.ProjectModel.IsSaved)
                {
                    DialogResult res = ApplicationMessage.ShowConfirmation("There are unsaved changes to the current CBZ-Archive.\nAre you sure you want to discard them and create a new file?", "Unsaved changes...");
                    if (res == DialogResult.Yes)
                    {
                        PagesList.Items.Clear();
                        PageView.Clear();

                        PageImages.Images.Clear();

                        toolStripProgressBar.Value = 0;

                        ClearProject();
                        NewProject();
                    }
                }
                else
                {
                    PagesList.Items.Clear();
                    PageView.Clear();

                    PageImages.Images.Clear();

                    toolStripProgressBar.Value = 0;

                    ClearProject();
                    NewProject();
                }

               
            }
        }

        private void PageView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            ListView.SelectedListViewItemCollection selectedPages = this.PageView.SelectedItems;
            bool buttonState = selectedPages.Count > 0;
           
            ToolButtonRemoveFiles.Enabled = buttonState;
            ToolButtonMovePageDown.Enabled = buttonState;
            ToolButtonMovePageUp.Enabled = buttonState;

            ToolButtonSetPageType.Enabled = selectedPages.Count == 1;

            foreach (ListViewItem itempage in PagesList.Items)
            {
                itempage.Selected = false;
            }

            foreach (ListViewItem item in selectedPages)
            {
                PagesList.Items[item.Index].Selected = true;    
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!ArchiveProcessing())
            {
                if (Program.ProjectModel.IsChanged && !Program.ProjectModel.IsSaved)
                {
                    DialogResult res = ApplicationMessage.ShowConfirmation("There are unsaved changes to the current CBZ-Archive.\nAre you sure you want to quit anyway?", "Unsaved changes...");
                    if (res == DialogResult.Yes)
                    {
                        e.Cancel = false || ArchiveProcessing();
                        if (!e.Cancel)
                        {
                            Win_CBZSettings.Default.Save();
                            WindowClosed = true;
                        }
                    } else
                    {
                        e.Cancel = true;
                    }
                }
                else
                {
                    Win_CBZSettings.Default.Save();
                    WindowClosed = true;
                }
            } else
            {
                ApplicationMessage.ShowWarning("Please wait until current operation has finished.", "Still operations in progress", 2, ApplicationMessage.DialogButtons.MB_OK);
                e.Cancel = false || ArchiveProcessing();
            }
            
        }

        private void AddFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {

            DialogResult openImageResult = OpenImagesDialog.ShowDialog();
            List<LocalFile> files = new List<LocalFile>();

            if (openImageResult == DialogResult.OK)
            {
                var maxIndex = PagesList.Items.Count - 1;
                var newIndex = maxIndex < 0 ? 0 : maxIndex;
                files = Program.ProjectModel.ParseFiles(new List<String>(OpenImagesDialog.FileNames));
                if (files.Count > 0)
                {
                    Program.ProjectModel.AddImages(files, newIndex);
                }

            }
        }

        private void PagesList_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            if (e.Item > -1)
            {
                ListViewItem changedItem = PagesList.Items[e.Item];
                if (changedItem != null)
                {
                    try
                    {
                        Program.ProjectModel.RenamePage((Page)changedItem.Tag, e.Label);
                        PageChanged(sender, new PageChangedEvent(((Page)changedItem.Tag), PageChangedEvent.IMAGE_STATUS_RENAMED));
                    }
                    catch (PageDuplicateNameException eduplicate)
                    {                      
                        MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_ERROR, eduplicate.Message);
                        e.CancelEdit = true;
                        ApplicationMessage.ShowException(eduplicate, ApplicationMessage.MT_ERROR);
                    }
                    catch (PageException ep)
                    {
                        e.CancelEdit = true;
                    }
                    catch (Exception ex)
                    {
                        MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_ERROR, ex.Message);
                        e.CancelEdit = true;
                    }             
                }
            }
        }


        private void MoveItemsTo(int newIndex, ListView.SelectedListViewItemCollection items)
        {
            ListViewItem originalItem;
            ListViewItem newItem;
            ListViewItem pageOriginal;
            ListViewItem pageNew;
            int IndexOldItem = 0;
            int direction = 0;

            if (newIndex < 0 || newIndex > PagesList.Items.Count - 1)
            {
                return;
            }

            if (newIndex > items[0].Index)
            {
                direction = -1;
            } else
            {
                direction = 1;
            }

            foreach (ListViewItem item in items)
            {
                originalItem = PagesList.Items[newIndex];
                newItem = item;
                pageOriginal = FindListViewItemForPage(PageView, (Page)originalItem.Tag);
                pageNew = FindListViewItemForPage(PageView, (Page)item.Tag);
                IndexOldItem = item.Index;
                PagesList.Items.Remove(originalItem);
                PagesList.Items.Remove(item);
                PageView.Items.Remove(pageOriginal);
                PageView.Items.Remove(pageNew);
                Program.ProjectModel.Pages.Remove((Page)originalItem.Tag);
                Program.ProjectModel.Pages.Remove((Page)item.Tag);
                if (direction == 1)
                {
                    PagesList.Items.Insert(newIndex, item);
                    PagesList.Items.Insert(IndexOldItem, originalItem);
                    PageView.Items.Insert(newIndex, pageNew);
                    PageView.Items.Insert(IndexOldItem, pageOriginal);
                    Program.ProjectModel.Pages.Insert(newIndex, (Page)item.Tag);
                    Program.ProjectModel.Pages.Insert(IndexOldItem, (Page)originalItem.Tag);
                } else
                {
                    PagesList.Items.Insert(IndexOldItem, originalItem);
                    PagesList.Items.Insert(newIndex, item);
                    PageView.Items.Insert(IndexOldItem, pageOriginal);
                    PageView.Items.Insert(newIndex, pageNew);
                    Program.ProjectModel.Pages.Insert(IndexOldItem, (Page)originalItem.Tag);
                    Program.ProjectModel.Pages.Insert(newIndex, (Page)item.Tag);             
                }
              
                //Program.ProjectModel.Pages.Insert(newIndex, (Page)item.Tag);
                //Program.ProjectModel.Pages.Insert(IndexOldItem, (Page)originalItem.Tag);
                newIndex += direction;
            }

            Program.ProjectModel.UpdatePageIndices();
            Program.ProjectModel.IsChanged = true;
        }


        private bool ArchiveProcessing()
        {
            return (Program.ProjectModel.ArchiveState == CBZArchiveStatusEvent.ARCHIVE_SAVING ||
               Program.ProjectModel.ArchiveState == CBZArchiveStatusEvent.ARCHIVE_OPENING ||
               Program.ProjectModel.ArchiveState == CBZArchiveStatusEvent.ARCHIVE_EXTRACTING ||
               Program.ProjectModel.ArchiveState == CBZArchiveStatusEvent.ARCHIVE_CLOSING);
        }


        private void AddMetaData()
        {
            if (Program.ProjectModel.MetaData == null)
            {
                Program.ProjectModel.MetaData = new MetaData(true);
            }

            Program.ProjectModel.MetaData.FillMissingDefaultProps();

            MetaDataLoaded(this, new MetaDataLoadEvent(Program.ProjectModel.MetaData.Values));

            BtnAddMetaData.Enabled = false;
            BtnRemoveMetaData.Enabled = true;
            AddMetaDataRowBtn.Enabled = true;
            RemoveMetadataRowBtn.Enabled = false;
        }

        private void RemoveMetaData() 
        {
            if (Program.ProjectModel.MetaData != null)
            {
                MetaDataGrid.DataSource = null;

                Program.ProjectModel.MetaData.Values.Clear();
                BtnAddMetaData.Enabled = true;
                BtnRemoveMetaData.Enabled = false;
                AddMetaDataRowBtn.Enabled = false;
                RemoveMetadataRowBtn.Enabled = false;
            }
        }

        private void BtnAddMetaData_Click(object sender, EventArgs e)
        {
            AddMetaData();
        }

        private void BtnRemoveMetaData_Click(object sender, EventArgs e)
        {
            if (Program.ProjectModel != null && Program.ProjectModel.MetaData != null)
            {
                if (Program.ProjectModel.MetaData.HasValues()) {
                    if (ApplicationMessage.ShowConfirmation("Are you sure you want to remove existing Metadata from Archive?", "Remove existing Meta-Data") == DialogResult.Yes)
                    {
                        RemoveMetaData();
                    }
                } else
                {
                    RemoveMetaData();
                }
            }          
        }

        private void AddMetaDataRowBtn_Click(object sender, EventArgs e)
        {
            Program.ProjectModel.MetaData.Values.Add(new MetaDataEntry(""));
            MetaDataGrid.Refresh();
        }

        private void MetaDataGrid_SelectionChanged(object sender, EventArgs e)
        {
            RemoveMetadataRowBtn.Enabled = MetaDataGrid.SelectedRows.Count > 0;   
        }

        private void RemoveMetadataRowBtn_Click(object sender, EventArgs e)
        {
            if (MetaDataGrid.SelectedRows.Count > 0)
            {
                foreach (DataGridViewRow row in MetaDataGrid.SelectedRows)
                {
                    if (row.DataBoundItem is MetaDataEntry)
                    {
                        Program.ProjectModel.MetaData.Values.Remove((MetaDataEntry)row.DataBoundItem);
                    }
                }
            }
        }

        private void MetaDataGrid_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 0)
                {
                    Program.ProjectModel.MetaData.Validate((MetaDataEntry)MetaDataGrid.Rows[e.RowIndex].DataBoundItem, e.FormattedValue.ToString());
                }
            } catch (MetaDataValidationException ve)
            {
                MetaDataGrid.Rows[e.RowIndex].ErrorText = ve.Message;
                //e.Cancel = true;
            }
        }

        private void ExtractAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ExtractFilesDialog dlg = new ExtractFilesDialog();
                dlg.TargetFolder = LastOutputDirectory;
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    ExtractSelectedPages.Enabled = false;
                    Program.ProjectModel.Extract(dlg.TargetFolder);
                    LastOutputDirectory = dlg.TargetFolder;
                }
            } catch (Exception) { }
        }

        private void PagesList_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            ListView.SelectedListViewItemCollection selectedPages = this.PagesList.SelectedItems;
            bool buttonState = selectedPages.Count > 0;

            ToolButtonRemoveFiles.Enabled = buttonState;
            ToolButtonMovePageDown.Enabled = buttonState && selectedPages.Count != PagesList.Items.Count;
            ToolButtonMovePageUp.Enabled = buttonState && selectedPages.Count != PagesList.Items.Count;

            ToolButtonSetPageType.Enabled = buttonState;

            if (buttonState)
            {
                LabelW.Text = ((Page)selectedPages[0].Tag).W.ToString();
                LabelH.Text = ((Page)selectedPages[0].Tag).H.ToString();
            }

            ((Page)e.Item.Tag).Selected = e.IsSelected;
            foreach (ListViewItem item in selectedPages)
            {
                if (((Page)item.Tag).Compressed)
                {
                    //
                }
            }
        }

        private void ToolButtonRemoveFiles_Click(object sender, EventArgs e)
        {
            ListView.SelectedListViewItemCollection selectedPages = this.PagesList.SelectedItems;

            if (selectedPages.Count > 0)
            {
                foreach (ListViewItem img in selectedPages)
                {
                    ((Page)img.Tag).Deleted = true;
                    if (!((Page)img.Tag).Compressed)
                    {
                        try
                        {
                            ((Page)img.Tag).DeleteTemporaryFile();
                        } catch (Exception ex)
                        {
                            MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_ERROR, ex.Message);
                        }
                    }
                    img.ForeColor = Color.Silver;
                    img.BackColor = Color.Transparent;
                }

                Program.ProjectModel.UpdatePageIndices();
            }   
        }      

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutDialogForm aboutDialogForm = new AboutDialogForm();
            aboutDialogForm.ShowDialog();
        }

        private void TypeSelectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in PagesList.SelectedItems)
            {
                if (sender is ToolStripMenuItem)
                {
                    ((Page)item.Tag).ImageType = (String)((ToolStripMenuItem)sender).Tag;
                } else
                {
                    ((Page)item.Tag).ImageType = (String)((ToolStripSplitButton)sender).Tag; 
                }

                if (item.SubItems.Count > 0)
                {
                    item.SubItems[2].Text = ((Page)item.Tag).ImageType;
                }
                
            }
        }

        private void ToolButtonEditImageProps_Click(object sender, EventArgs e)
        {

        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in PagesList.Items)
            {
                item.Selected = true;
            }
        }

        private void CheckBoxDoRenamePages_CheckedChanged(object sender, EventArgs e)
        {
            Program.ProjectModel.ApplyRenaming = CheckBoxDoRenamePages.Checked;
            Program.ProjectModel.IsChanged = true;
            TextboxStoryPageRenamingPattern.Enabled = CheckBoxDoRenamePages.Checked;
            TextboxSpecialPageRenamingPattern.Enabled = CheckBoxDoRenamePages.Checked;
            ToolButtonSave.Enabled = true;
            saveToolStripMenuItem.Enabled = true;
        }

        private void TextboxStoryPageRenamingPattern_TextChanged(object sender, EventArgs e)
        {
            Program.ProjectModel.RenameStoryPagePattern = TextboxStoryPageRenamingPattern.Text;
            Win_CBZSettings.Default.StoryPageRenamePattern = TextboxStoryPageRenamingPattern.Text;
        }

        private void TextboxSpecialPageRenamingPattern_TextChanged(object sender, EventArgs e)
        {
            Program.ProjectModel.RenameSpecialPagePattern = TextboxSpecialPageRenamingPattern.Text;
            Win_CBZSettings.Default.SpecialPageRenamePattern = TextboxSpecialPageRenamingPattern.Text;
        }

        private void ClearTemporaryFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void TogglePagePreviewToolbutton_Click(object sender, EventArgs e)
        {
            TogglePagePreviewToolbutton.Checked = !TogglePagePreviewToolbutton.Checked;
            Win_CBZSettings.Default.PagePreviewEnabled = TogglePagePreviewToolbutton.Checked;
            SplitBoxPageView.Panel1Collapsed = !TogglePagePreviewToolbutton.Checked;
            Program.ProjectModel.PreloadPageImages = TogglePagePreviewToolbutton.Checked;

            if (Win_CBZSettings.Default.PagePreviewEnabled && PageView.Items.Count == 0) 
            {
                /*
                if (ThumbnailThread != null)
                {
                    if (ThumbnailThread.IsAlive)
                    {
                        ThumbnailThread.Abort();
                    }
                }

                ThumbnailThread = new Thread(new ThreadStart(ReloadPreviewThumbs));
                ThumbnailThread.Start();
                */

                /*
                PageView.Invoke(new Action(() =>
                {
                    foreach (ListViewItem pageItem in PagesList.Items)
                    {
                        CreatePagePreviewFromItem((Page)pageItem.Tag);
                    }
                }));
                */
            }
        }

        private void ToolButtonSetPageType_ButtonClick(object sender, EventArgs e)
        {
            foreach (ListViewItem item in PagesList.SelectedItems)
            {
                ((Page)item.Tag).ImageType = (String)((ToolStripSplitButton)sender).Tag;

                if (item.SubItems.Count > 0)
                {
                    item.SubItems[2].Text = (String)((ToolStripSplitButton)sender).Tag;
                }

            }
        }

        private void ToolButtonSetPageType_Click(object sender, EventArgs e)
        {
            
        }

        private void SettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SettingsDialog settingsDialog = new SettingsDialog();
            if (settingsDialog.ShowDialog() == DialogResult.OK)
            {
                if (settingsDialog.NewDefaults != null)
                {
                    foreach (String line in settingsDialog.NewDefaults)
                    {
                        if (line != null && line != "")
                        {
                            Win_CBZSettings.Default.CustomDefaultProperties.Add(line);
                        }
                    }
                }
            }
        }

        private void QuitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ToolButtonMovePageUp_Click(object sender, EventArgs e)
        {
            ListViewItem first = PagesList.SelectedItems[0];

            MoveItemsTo(first.Index - 1, PagesList.SelectedItems);
        }

        private void ToolButtonMovePageDown_Click(object sender, EventArgs e)
        {
            ListViewItem last = PagesList.SelectedItems[PagesList.SelectedItems.Count - 1];

            MoveItemsTo(last.Index + 1, PagesList.SelectedItems);
        }

        private void PageView_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            ListView owner = sender as ListView;
            ListViewItem item = e.Item as ListViewItem;
            Page page = (Page)item.Tag;
            Pen borderPen;
            if (e.Item.Selected)
            {
                borderPen = new Pen(Color.DodgerBlue, 2);
            }
            else
            {
                borderPen = new Pen(Color.LightGray, 2);
            }
            int center = ((e.Bounds.Width + 4) / 2) - ((owner.LargeImageList.ImageSize.Width + 4) / 2);

            Rectangle rectangle = new Rectangle(center, e.Bounds.Y + 2, owner.LargeImageList.ImageSize.Width + 4, owner.LargeImageList.ImageSize.Height + 4);


            int customItemBoundsW = owner.LargeImageList.ImageSize.Width;
            int customItemBoundsH = owner.LargeImageList.ImageSize.Height;

            // e.Bounds.Width = customItemBoundsW + 4;


            if (page != null)
            {
                e.Graphics.DrawRectangle(borderPen, rectangle);
                if (owner.LargeImageList.Images.IndexOfKey(page.Id) >= 0)
                {
                    e.Graphics.DrawImage(owner.LargeImageList.Images[owner.LargeImageList.Images.IndexOfKey(page.Id)], new Point(center + 2, e.Bounds.Y + 4));
                } else
                {
                    ThumbnailPagesSlice.Add(page);
                    if (RequestThumbnailThread != null)
                    {
                        if (!RequestThumbnailThread.IsAlive)
                        {
                            RequestThumbnailSlice();
                        }
                    } else
                    {
                        RequestThumbnailSlice();
                    }
                }
            }
            else
            {

            }
        }

        private void CheckBoxPreview_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBoxPreview.Checked)
            {
                Program.ProjectModel.AutoRenameAllPages();
            } else
            {
                Program.ProjectModel.RestoreOriginalNames();
            }
        }

        private void TextboxStoryPageRenamingPattern_TextChanged_1(object sender, EventArgs e)
        {
            Program.ProjectModel.RenameStoryPagePattern = TextboxStoryPageRenamingPattern.Text;
        }

        private void TextboxSpecialPageRenamingPattern_TextChanged_1(object sender, EventArgs e)
        {
            Program.ProjectModel.RenameSpecialPagePattern = TextboxSpecialPageRenamingPattern.Text;

        }

        private void PictureBoxColorSelect_Click(object sender, EventArgs e)
        {
            if (SelectColorDialog.ShowDialog() == DialogResult.OK)
            {
                PictureBoxColorSelect.BackColor = SelectColorDialog.Color;
            }
        }


        /*
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PagesList_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(List<ListViewItem>)))
            {
                var items = (List<ListViewItem>)e.Data.GetData(typeof(List<ListViewItem>));

                ListViewItem targetItem = PagesList.GetItemAt(e.X, e.Y);

                if (targetItem != null)
                {
                    foreach (ListViewItem lvi in items)
                    {


                    }
                }
            }
        }

        private void PagesList_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(List<ListViewItem>)))
            {
                e.Effect = DragDropEffects.Move;

            }

        }

        private void PagesList_ItemDrag(object sender, ItemDragEventArgs e)
        {
            var items = new List<ListViewItem>();
            items.Add((ListViewItem)e.Item);
            foreach (ListViewItem lvi in PagesList.SelectedItems)
            {
                if (!items.Contains(lvi))
                {
                    items.Add(lvi);
                }
            }
            // pass the items to move...
            PagesList.DoDragDrop(items, DragDropEffects.Move);
        }
        **/
    }
}
