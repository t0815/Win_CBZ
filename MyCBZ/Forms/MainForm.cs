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
using MyCBZ;
using System.Threading;
using MyCBZ.Forms;
using System.IO;

namespace CBZMage
{
    public partial class MainForm : Form
    {

        private ProjectModel ProjectModel;

        private Thread ClosingTask;

        private Thread OpeningTask;

        private bool WindowClosed = false;

        private bool WindowShown = false;

        private Thread ThumbnailThread;

        public MainForm()
        {
            InitializeComponent();

            ProjectModel = NewProjectModel();

            MessageLogger.Instance.SetHandler(MessageLogged);
        }

        private ProjectModel NewProjectModel()
        {
            ProjectModel newProjectModel = new ProjectModel(Win_CBZSettings.Default.TempFolderPath);
            newProjectModel.ImageProgress += FileLoaded;
            newProjectModel.ArchiveStatusChanged += ArchiveStateChanged;
            newProjectModel.ItemChanged += ItemChanged;
            newProjectModel.MetaDataLoaded += MetaDataLoaded;
            newProjectModel.ItemExtracted += ItemExtracted;
            newProjectModel.OperationFinished += OperationFinished;
            newProjectModel.FileOperation += FileOperationHandler;
            newProjectModel.ArchiveOperation += ArchiveOperationHandler;

            this.Text = Win_CBZSettings.Default.AppName + " (c) Trash_s0Ft";

            return newProjectModel;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {

            DialogResult openCBFResult = OpenCBFDialog.ShowDialog();

            if (openCBFResult == DialogResult.OK)
            {
                ClosingTask = ProjectModel.Close();

                Task.Factory.StartNew(() =>
                {
                    if (ClosingTask != null)
                    {
                        while (ClosingTask.IsAlive)
                        {
                            System.Threading.Thread.Sleep(100);
                        }
                    }

                    OpeningTask = ProjectModel.Open(OpenCBFDialog.FileName, ZipArchiveMode.Read);
                });
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult saveDialogResult = SaveArchiveDialog.ShowDialog();

            if (saveDialogResult == DialogResult.OK)
            {
                ProjectModel.SaveAs(SaveArchiveDialog.FileName, ZipArchiveMode.Update);
            }
        }

        private void ToolButtonSave_Click(object sender, EventArgs e)
        {    
            ProjectModel.Save();
        }

        private void FileLoaded(object sender, ItemLoadProgressEvent e)
        {
            toolStripProgressBar.Control.Invoke(new Action(() =>
            {
                toolStripProgressBar.Maximum = e.Total;
                toolStripProgressBar.Value = e.Index;
            }));

            PagesList.Invoke(new Action(() =>
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
                } else
                {
                    item = existingItem;
                    item.SubItems[1] = new ListViewItem.ListViewSubItem(item, !e.Image.Deleted ? e.Image.Number.ToString() : "-");
                    item.SubItems[2] = new ListViewItem.ListViewSubItem(item, e.Image.ImageType.ToString());
                    item.SubItems[3] = new ListViewItem.ListViewSubItem(item, e.Image.LastModified.ToString());
                    item.SubItems[4] = new ListViewItem.ListViewSubItem(item, e.Image.Size.ToString());
                }
               
                item.Tag = e.Image;

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
            }));

            if (TogglePagePreviewToolbutton.Checked)
            {
                PageView.Invoke(new Action(() =>
                {
                    CreatePagePreviewFromItem(e.Image);
                }));
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

                foreach (Page page in ProjectModel.Pages)
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

        private ListViewItem CreatePagePreviewFromItem(Page page)
        {
            ListViewItem itemPage;
            ListViewItem existingItem = FindListViewItemForPage(PageView, page);

            if (!PageImages.Images.ContainsKey(page.Name))
            {
                PageImages.Images.Add(page.Id, page.GetThumbnail(ThumbAbort, Handle));
            }
            else
            {
                //PageImages.Images.RemoveByKey(page.Name);
                //PageImages.Images.Add(page.Id, page.GetThumbnail(ThumbAbort, Handle));
            }

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


        private bool ThumbAbort()
        {

            return true;
        }

        private void ItemChanged(object sender, PageChangedEvent e)
        {
            try
            {
                if (!WindowClosed)
                {
                    toolStripProgressBar.Control.Invoke(new Action(() =>
                    {
                        toolStripProgressBar.Maximum = e.Total;
                        toolStripProgressBar.Value = e.Image.Index;
                    }));
                }
            } catch (Exception)
            {

            }
        }

        private void ItemExtracted(object sender, ItemExtractedEvent e)
        {
            try
            {
                if (!WindowClosed)
                {
                    toolStripProgressBar.Control.Invoke(new Action(() =>
                    {
                        toolStripProgressBar.Maximum = e.Total;
                        toolStripProgressBar.Value = e.Index;
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
                    filename = "<NO FILE>";
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
                        ProjectModel.ArchiveState = e.State;

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
            catch (Exception)
            {

            }
        }


        private void NewProject()
        {
            if (ProjectModel != null)
            {
                ProjectModel.New();
            }
        }

        private void DisableControllsForArchiveState(int state) 
        {
            switch (state)
            {
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
                    extractAllToolStripMenuItem.Enabled = false;
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
                    extractAllToolStripMenuItem.Enabled = true;
                    BtnAddMetaData.Enabled = ProjectModel.MetaData.Values.Count == 0;
                    BtnRemoveMetaData.Enabled = ProjectModel.MetaData.Values.Count > 0;
                    AddMetaDataRowBtn.Enabled = ProjectModel.MetaData.Values != null;
                    TextboxStoryPageRenamingPattern.Enabled = true;
                    TextboxSpecialPageRenamingPattern.Enabled = true;
                    ToolButtonSave.Enabled = false;
                    saveToolStripMenuItem.Enabled = false;
                    ProjectModel.IsNew = false; 
                    break;

                case CBZArchiveStatusEvent.ARCHIVE_SAVING:
                    newToolStripMenuItem.Enabled = false;
                    openToolStripMenuItem.Enabled = false;
                    ToolButtonNew.Enabled = false;
                    ToolButtonOpen.Enabled = false;
                    addFilesToolStripMenuItem.Enabled = false;
                    ToolButtonAddFiles.Enabled = false;
                    BtnAddMetaData.Enabled = false;
                    BtnRemoveMetaData.Enabled = false;
                    ToolButtonExtractArchive.Enabled = false;
                    extractAllToolStripMenuItem.Enabled = false;
                    BtnAddMetaData.Enabled = false;
                    BtnRemoveMetaData.Enabled = false;
                    CheckBoxDoRenamePages.Enabled = false;
                    TextboxStoryPageRenamingPattern.Enabled = false;
                    TextboxSpecialPageRenamingPattern.Enabled = false;
                    ToolButtonSave.Enabled = false;
                    saveToolStripMenuItem.Enabled = false;
                    break;

                case CBZArchiveStatusEvent.ARCHIVE_SAVED:
                    newToolStripMenuItem.Enabled = true;
                    openToolStripMenuItem.Enabled = true;
                    ToolButtonNew.Enabled = true;
                    ToolButtonOpen.Enabled = true;
                    addFilesToolStripMenuItem.Enabled = true;
                    ToolButtonAddFiles.Enabled = true;
                    ToolButtonExtractArchive.Enabled = true;
                    extractAllToolStripMenuItem.Enabled = true;
                    CheckBoxDoRenamePages.Enabled = true;
                    TextboxStoryPageRenamingPattern.Enabled = true;
                    TextboxSpecialPageRenamingPattern.Enabled = true;
                    ToolButtonSave.Enabled = false;
                    saveToolStripMenuItem.Enabled = false;
                    ProjectModel.IsNew = false;
                    ProjectModel.IsSaved = true;
                    break;

                case CBZArchiveStatusEvent.ARCHIVE_EXTRACTING:
                    newToolStripMenuItem.Enabled = false;
                    openToolStripMenuItem.Enabled = false;
                    ToolButtonNew.Enabled = false;
                    ToolButtonOpen.Enabled = false;
                    addFilesToolStripMenuItem.Enabled = false;
                    ToolButtonAddFiles.Enabled = false;
                    BtnRemoveMetaData.Enabled = false;
                    ToolButtonExtractArchive.Enabled = false;
                    extractAllToolStripMenuItem.Enabled = false;
                    break;

                case CBZArchiveStatusEvent.ARCHIVE_EXTRACTED:
                    newToolStripMenuItem.Enabled = true;
                    openToolStripMenuItem.Enabled = true;
                    ToolButtonNew.Enabled = true;
                    ToolButtonOpen.Enabled = true;
                    addFilesToolStripMenuItem.Enabled = true;
                    ToolButtonAddFiles.Enabled = true;
                    ToolButtonExtractArchive.Enabled = true;
                    extractAllToolStripMenuItem.Enabled = true;
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
                    ToolButtonExtractArchive.Enabled = false;
                    extractAllToolStripMenuItem.Enabled = false;
                    ToolButtonSave.Enabled = false;
                    saveToolStripMenuItem.Enabled = false;
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
                    BtnAddMetaData.Enabled = ProjectModel.MetaData.Values.Count == 0;
                    AddMetaDataRowBtn.Enabled = ProjectModel.MetaData.Values != null;
                    BtnRemoveMetaData.Enabled = ProjectModel.MetaData.Values.Count > 0;
                    TextboxStoryPageRenamingPattern.Enabled = true;
                    TextboxSpecialPageRenamingPattern.Enabled = true;
                    ToolButtonSave.Enabled = false;
                    saveToolStripMenuItem.Enabled = false;
                    break;
                case CBZArchiveStatusEvent.ARCHIVE_FILE_ADDED:
                case CBZArchiveStatusEvent.ARCHIVE_FILE_DELETED:
                case CBZArchiveStatusEvent.ARCHIVE_FILE_RENAMED:
                    if (!ProjectModel.IsNew)
                    {
                        ToolButtonSave.Enabled = true;
                        saveToolStripMenuItem.Enabled = true;
                    }
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

                ClosingTask = ProjectModel.Close();
            });
        }

        private void NewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!WindowClosed && !ArchiveProcessing())
            {
                PagesList.Items.Clear();
                PageView.Clear();

                PageImages.Images.Clear();

                toolStripProgressBar.Value = 0;
            }

            ClearProject();
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
            WindowClosed = true;

            if (!ArchiveProcessing())
            {
                Win_CBZSettings.Default.Save();
            }

            e.Cancel = ArchiveProcessing();
        }

        private void AddFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {

            DialogResult openImageResult = OpenImagesDialog.ShowDialog();
            List<LocalFile> files = new List<LocalFile>();

            if (openImageResult == DialogResult.OK)
            {
                var maxIndex = PagesList.Items.Count - 1;
                var newIndex = maxIndex < 0 ? 0 : maxIndex;
                files = ProjectModel.ParseFiles(new List<String>(OpenImagesDialog.FileNames));
                if (files.Count > 0)
                {
                    ProjectModel.AddImages(files, newIndex);
                }

            }
        }

        private void PagesList_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            if (e.Item > -1)
            {
                ListViewItem changedItem = PagesList.Items[e.Item - 1];
                if (changedItem != null)
                {
                    ((Page)changedItem.Tag).Name = e.Label;
                }
            }
        }

        private bool ArchiveProcessing()
        {
            return (ProjectModel.ArchiveState == CBZArchiveStatusEvent.ARCHIVE_SAVING ||
               ProjectModel.ArchiveState == CBZArchiveStatusEvent.ARCHIVE_OPENING ||
               ProjectModel.ArchiveState == CBZArchiveStatusEvent.ARCHIVE_EXTRACTING ||
               ProjectModel.ArchiveState == CBZArchiveStatusEvent.ARCHIVE_CLOSING);
        }


        private void AddMetaData()
        {
            if (ProjectModel.MetaData == null)
            {
                ProjectModel.MetaData = new MetaData(true);
            }

            ProjectModel.MetaData.FillMissingDefaultProps();

            MetaDataLoaded(this, new MetaDataLoadEvent(ProjectModel.MetaData.Values));

            BtnAddMetaData.Enabled = false;
            BtnRemoveMetaData.Enabled = true;
            AddMetaDataRowBtn.Enabled = true;
            RemoveMetadataRowBtn.Enabled = false;
        }

        private void RemoveMetaData() 
        {
            if (ProjectModel.MetaData != null)
            {
                MetaDataGrid.DataSource = null;

                ProjectModel.MetaData.Values.Clear();
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
            RemoveMetaData();
        }

        private void AddMetaDataRowBtn_Click(object sender, EventArgs e)
        {
            ProjectModel.MetaData.Values.Add(new MetaDataEntry(""));
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
                        ProjectModel.MetaData.Values.Remove((MetaDataEntry)row.DataBoundItem);
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
                    ProjectModel.MetaData.Validate((MetaDataEntry)MetaDataGrid.Rows[e.RowIndex].DataBoundItem, e.FormattedValue.ToString());
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
                ProjectModel.Extract();
            } catch (Exception) { }
        }

        private void PagesList_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            ListView.SelectedListViewItemCollection selectedPages = this.PagesList.SelectedItems;
            bool buttonState = selectedPages.Count > 0;

            ToolButtonRemoveFiles.Enabled = buttonState;
            ToolButtonMovePageDown.Enabled = buttonState;
            ToolButtonMovePageUp.Enabled = buttonState;

            ToolButtonSetPageType.Enabled = buttonState;
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
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

                ProjectModel.UpdatePageIndices();
            }   
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
                ProjectModel.PreloadPageImages = Win_CBZSettings.Default.PagePreviewEnabled;

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
                foreach (String pageType in MetaDataEntryPage.PageTypes) {
                    newPageTypeItem = ToolButtonSetPageType.DropDownItems.Add(pageType);
                    newPageTypeItem.Tag = pageType;
                    newPageTypeItem.Click += TypeSelectionToolStripMenuItem_Click;
                }

                WindowShown = true;
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
                ((Page)item.Tag).ImageType = (String)((ToolStripMenuItem)sender).Tag;

                if (item.SubItems.Count > 0)
                {
                    item.SubItems[2].Text = (String)((ToolStripMenuItem)sender).Tag;
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
            ProjectModel.ApplyRenaming = CheckBoxDoRenamePages.Checked;
            ProjectModel.IsChanged = true;
            TextboxStoryPageRenamingPattern.Enabled = CheckBoxDoRenamePages.Checked;
            TextboxSpecialPageRenamingPattern.Enabled = CheckBoxDoRenamePages.Checked;
            ToolButtonSave.Enabled = true;
            saveToolStripMenuItem.Enabled = true;
        }

        private void TextboxStoryPageRenamingPattern_TextChanged(object sender, EventArgs e)
        {
            ProjectModel.RenameStoryPagePattern = TextboxStoryPageRenamingPattern.Text;
            Win_CBZSettings.Default.StoryPageRenamePattern = TextboxStoryPageRenamingPattern.Text;
        }

        private void TextboxSpecialPageRenamingPattern_TextChanged(object sender, EventArgs e)
        {
            ProjectModel.RenameSpecialPagePattern = TextboxSpecialPageRenamingPattern.Text;
            Win_CBZSettings.Default.SpecialPageRenamePattern = TextboxSpecialPageRenamingPattern.Text;
        }

        private void ClearTemporaryFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void PagesList_AfterLabelEdit_1(object sender, LabelEditEventArgs e)
        {
            ListViewItem editedItem = PagesList.Items[e.Item];
            
            foreach (ListViewItem item in PagesList.Items)
            {
                if (item.Tag != editedItem.Tag)
                {
                    if (item.Text == editedItem.Text)
                    {
                        e.CancelEdit = true;
                        return;
                    }
                }
            }

            ((Page)editedItem.Tag).Name = e.Label;
        }

        private void TogglePagePreviewToolbutton_Click(object sender, EventArgs e)
        {
            TogglePagePreviewToolbutton.Checked = !TogglePagePreviewToolbutton.Checked;
            Win_CBZSettings.Default.PagePreviewEnabled = TogglePagePreviewToolbutton.Checked;
            SplitBoxPageView.Panel1Collapsed = !TogglePagePreviewToolbutton.Checked;
            ProjectModel.PreloadPageImages = TogglePagePreviewToolbutton.Checked;

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

                PageView.Invoke(new Action(() =>
                {
                    foreach (ListViewItem pageItem in PagesList.Items)
                    {
                        CreatePagePreviewFromItem((Page)pageItem.Tag);
                    }
                }));
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
