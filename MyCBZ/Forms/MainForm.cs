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
using System.Net.NetworkInformation;
using System.Collections;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Win_CBZ.Data;
using System.Windows.Forms.VisualStyles;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Status;
using Win_CBZ.Tasks;
using System.Security.Policy;

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

        private Thread RequestImageInfoThread;

        private List<Page> ThumbnailPagesSlice;

        private List<Page> ImageInfoPagesSlice;

        private Task<TaskResult> CurrentGlobalAction;

        public MainForm()
        {
            InitializeComponent();

            Program.ProjectModel = NewProjectModel();

            MessageLogger.Instance.SetHandler(MessageLogged);

            ThumbnailPagesSlice = new List<Page>();
            ImageInfoPagesSlice = new List<Page>();
        }

        private ProjectModel NewProjectModel()
        {
            ProjectModel newProjectModel = new ProjectModel(Win_CBZSettings.Default.TempFolderPath);
            
            newProjectModel.ArchiveStatusChanged += ArchiveStateChanged;
            newProjectModel.TaskProgress += TaskProgress;
            newProjectModel.PageChanged += PageChanged;
            newProjectModel.MetaDataLoaded += MetaDataLoaded;
            newProjectModel.MetaDataChanged += MetaDataChanged;
            newProjectModel.MetaDataEntryChanged += MetaDataEntryChanged;
            // newProjectModel.ItemExtracted += ItemExtracted;
            newProjectModel.OperationFinished += OperationFinished;
            newProjectModel.FileOperation += FileOperationHandler;
            newProjectModel.ArchiveOperation += ArchiveOperationHandler;
            newProjectModel.ApplicationStateChanged += ApplicationStateChanged;
            newProjectModel.GlobalActionRequired += HandleGlobalActionRequired;
            newProjectModel.GeneralTaskProgress += HandleGlobalTaskProgress;

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

                /*
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
                */

                PlaceholdersFlowPanel.FlowDirection = FlowDirection.LeftToRight;
                PlaceholdersFlowPanel.Refresh();

                //FunctionsFlowPanel.FlowDirection = FlowDirection.LeftToRight;
                //FunctionsFlowPanel.Refresh();

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


        private void NewProject()
        {
            if (Program.ProjectModel != null)
            {
                Program.ProjectModel.New();
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
                if (Program.ProjectModel.Exists())
                {
                    SavingTask = Program.ProjectModel.Save();
                } else
                {
                    MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_WARNING, "Warning! Wrong application state. Current File does not exists! [" + Program.ProjectModel.FileName + "]");
                    saveAsToolStripMenuItem.Enabled = true;
                    saveAsToolStripMenuItem_Click(sender, e);
                }
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

                    switch (e.State)
                    {
                        case PageChangedEvent.IMAGE_STATUS_NEW:
                            if (!e.Image.Compressed) { 
                                ImageInfoPagesSlice.Add(e.Image);
                            }
                            break;
                        case PageChangedEvent.IMAGE_STATUS_DELETED:
                            e.Image.Deleted = true; 
                            break;
                        case PageChangedEvent.IMAGE_STATUS_COMPRESSED:
                            e.Image.Compressed = true; 
                            break;
                        case PageChangedEvent.IMAGE_STATUS_CHANGED:
                        case PageChangedEvent.IMAGE_STATUS_RENAMED:
                            e.Image.Changed = true;
                            break;
                    }

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
                } else
                {
                    if (e.State == PageChangedEvent.IMAGE_STATUS_CLOSED)
                    {
                        ListViewItem existingItem = FindListViewItemForPage(PagesList, e.Image);
                        if (existingItem != null)
                        {
                            PagesList.Items.Remove(existingItem);
                        }
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

        private void MetaDataLoaded(object sender, MetaDataLoadEvent e)
        {
            MetaDataGrid.Invoke(new Action(() =>
            {
                //MetaDataGrid.DataSource = e.MetaData;

                BtnAddMetaData.Enabled = false;
                BtnRemoveMetaData.Enabled = true;
                toolStripButtonShowRawMetadata.Enabled = true;
                DataGridViewColumn firstCol = MetaDataGrid.Columns.GetFirstColumn(DataGridViewElementStates.Visible);
                if (firstCol != null)
                {
                    DataGridViewColumn secondCol = MetaDataGrid.Columns.GetNextColumn(firstCol, DataGridViewElementStates.Visible, DataGridViewElementStates.None);
                    if (secondCol != null)
                    {
                        firstCol.Width = 150;
                        secondCol.Width = 350;
                    }
                }
                else
                {
                    MetaDataGrid.Columns.Add(new DataGridViewColumn()
                    {
                        DataPropertyName = "Key",
                        HeaderText = "Key",
                        CellTemplate = new DataGridViewTextBoxCell(),
                        Width = 150,
                        SortMode = DataGridViewColumnSortMode.Automatic,
                    });

                    MetaDataGrid.Columns.Add(new DataGridViewColumn()
                    {
                        DataPropertyName = "Value",
                        HeaderText = "Value",
                        CellTemplate = new DataGridViewTextBoxCell(),
                        Width = 250,
                        SortMode = DataGridViewColumnSortMode.NotSortable,
                    });
                }

                MetaDataGrid.Rows.Clear();
                foreach (MetaDataEntry entry in e.MetaData)
                {
                    MetaDataGrid.Rows.Add(entry.Key, entry.Value);
                }

                for (int i = 0; i < MetaDataGrid.RowCount; i++)
                {
                    foreach (MetaDataEntry entry in e.MetaData)
                    {
                        var key = MetaDataGrid.Rows[i].Cells[0].Value;
                        if (key != null)
                        {
                            if (entry.Key == key.ToString() && entry.Options.Length > 0)
                            {
                                int selectedIndex = Array.IndexOf(entry.Options, entry.Value);
                                DataGridViewComboBoxCell c = new DataGridViewComboBoxCell();
                                c.Items.AddRange(entry.Options);
                                c.Value = entry.Value; // selectedIndex > -1 ? selectedIndex : 0;

                                MetaDataGrid.Rows[i].Cells[1] = c;
                            }
                            else
                            {

                            }
                        }
                    }
                }
            }));
        }

        private void MetaDataChanged(object sender, MetaDataChangedEvent e)
        {
            MetaDataGrid.Invoke(new Action(() =>
            {
                
                    Program.ProjectModel.IsChanged = true;

                    if (Program.ProjectModel.FileName != null)
                    {
                        ToolButtonSave.Enabled = true;
                        saveToolStripMenuItem.Enabled = true;
                    }
                
                    
            }));
        }

        private void MetaDataEntryChanged(object sender, MetaDataEntryChangedEvent e)
        {
            MetaDataGrid.Invoke(new Action(() =>
            {
                Program.ProjectModel.IsChanged = true;

                if (Program.ProjectModel.FileName != null)
                {
                    ToolButtonSave.Enabled = true;
                    saveToolStripMenuItem.Enabled = true;
                }

                if (e.State == MetaDataEntryChangedEvent.ENTRY_NEW)
                {

                    foreach (DataGridViewRow r in  MetaDataGrid.SelectedRows)
                    {
                        r.Selected = false;
                    }

                    MetaDataGrid.Rows.Add(e.Entry.Key, e.Entry.Value);

                    if (e.Entry.Key == "" && e.Entry.Value == null)
                    {
                        //MetaDataGrid.Rows[MetaDataGrid.Rows.Count].Cells[0].Selected
                        MetaDataGrid.CurrentCell = MetaDataGrid.Rows[MetaDataGrid.Rows.Count - 1].Cells[0];
                        MetaDataGrid.BeginEdit(false);
                    }
                }

                if (e.State == MetaDataEntryChangedEvent.ENTRY_DELETED)
                {
                    MetaDataGrid.Rows.RemoveAt(e.Index);
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
                if (e.Status == FileOperationEvent.STATUS_RUNNING)
                {
                    toolStripProgressBar.Maximum = 100;
                    toolStripProgressBar.Value = Convert.ToInt32(100 * e.Completed / e.Total);

                    if (e.Operation == FileOperationEvent.OPERATION_COPY)
                    {
                        applicationStatusLabel.Text = "Copying file...";
                    }

                } else
                {
                    toolStripProgressBar.Value = 0;
                    applicationStatusLabel.Text = "Ready.";

                }
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

        private ListViewItem FindListViewItemForPage(ExtendetListView owner, Page page)
        {
            foreach (ListViewItem item in owner.Items)
            {
                if (((Page) item.Tag).Id.Equals(page.Id))
                {
                    return item;
                }
            }

            return null;
        }

        private void HandleGlobalActionRequired(object sender, GlobalActionRequiredEvent e)
        {
            this.Invoke(new Action(() =>
            {
                LabelGlobalActionStatusMessage.Text = e.Message;
                GlobalAlertTableLayout.Visible = true;
                CurrentGlobalAction = e.Task;
            }));
        }

        private void HandleGlobalTaskProgress(object sender, GeneralTaskProgressEvent e)
        {
            Program.ProjectModel.ApplicationState = e.Status;
            if (e.Type == GeneralTaskProgressEvent.TASK_RELOAD_IMAGE_METADATA)
            {
                switch (e.Status)
                {
                    case GeneralTaskProgressEvent.TASK_STATUS_COMPLETED:
                        Program.ProjectModel.MetaDataPageIndexMissingData = false;
                        Program.ProjectModel.MetaData.RebuildPageMetaData(Program.ProjectModel.Pages);
                        break;
                }
            }

            if (e.Status == GeneralTaskProgressEvent.TASK_STATUS_RUNNING)
            {
                this.Invoke(new Action(() =>
                {
                    saveToolStripMenuItem.Enabled = false;
                    saveAsToolStripMenuItem.Enabled = false;
                    ToolButtonSave.Enabled = false;
                    ToolButtonNew.Enabled = false;
                    ToolButtonAddFiles.Enabled = false;
                    ToolButtonMovePageDown.Enabled = false;
                    ToolButtonMovePageUp.Enabled = false;
                    ToolButtonRemoveFiles.Enabled = false;
                    newToolStripMenuItem.Enabled = false;
                    applicationStatusLabel.Text = e.Message;
                }));

                try
                {
                    if (!WindowClosed)
                    {
                        toolStripProgressBar.Control.Invoke(new Action(() =>
                        {
                            toolStripProgressBar.Maximum = e.Total;
                            if (e.Current > -1 && e.Current <= e.Total)
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

            if (e.Status == GeneralTaskProgressEvent.TASK_STATUS_COMPLETED)
            {
                this.Invoke(new Action(() =>
                {
                    saveToolStripMenuItem.Enabled = true;
                    saveAsToolStripMenuItem.Enabled = true;
                    ToolButtonSave.Enabled = true;
                    ToolButtonNew.Enabled = true;
                    ToolButtonAddFiles.Enabled = true;
                    ToolButtonMovePageDown.Enabled = true;
                    ToolButtonMovePageUp.Enabled = true;
                    ToolButtonRemoveFiles.Enabled = true;
                    newToolStripMenuItem.Enabled = true;
                    applicationStatusLabel.Text = e.Message;
                    Program.ProjectModel.IsChanged = true;
                    Program.ProjectModel.ApplicationState = ApplicationStatusEvent.STATE_READY;
                }));

                try
                {
                    if (!WindowClosed)
                    {
                        toolStripProgressBar.Control.Invoke(new Action(() =>
                        {
                            toolStripProgressBar.Maximum = e.Total;
                            
                            toolStripProgressBar.Value = 0;
                            
                        }));
                    }
                }
                catch (Exception)
                {

                }
            }
        }

        private void ExecuteCurrentGlobalAction_Click_1(object sender, EventArgs e)
        {
            if (CurrentGlobalAction != null)
            {
                if (!CurrentGlobalAction.IsCanceled && !CurrentGlobalAction.IsCompleted && !CurrentGlobalAction.IsFaulted)
                {
                    CurrentGlobalAction.Start();
                    GlobalAlertTableLayout.Visible = false;
                }
            }
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

                if (RequestImageInfoThread != null)
                {
                    if (RequestImageInfoThread.IsAlive)
                    {
                        RequestImageInfoThread.Abort();
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


        private bool ThumbAbort()
        {

            return true;
        }

        public void RequestImageInfoSlice()
        {
            if (!Win_CBZSettings.Default.PagePreviewEnabled)
            {

                if (ThumbnailThread != null)
                {
                    if (ThumbnailThread.IsAlive)
                    {
                        ThumbnailThread.Join();
                    }
                }

                if (RequestThumbnailThread != null)
                {
                    if (RequestThumbnailThread.IsAlive)
                    {
                        RequestThumbnailThread.Join();
                    }
                }

                if (RequestImageInfoThread != null)
                {
                    if (RequestImageInfoThread.IsAlive)
                    {
                        return;
                    }
                }

                RequestImageInfoThread = new Thread(new ThreadStart(LoadImageInfoSlice));
                RequestImageInfoThread.Start();
            }
        }

        public void LoadImageInfoSlice()
        {
            this.Invoke(new Action(() =>
            {

                foreach (Page page in ImageInfoPagesSlice)
                {
                    try
                    {
                        if (!page.ImageInfoRequested)
                        {
                            page.LoadImageInfo();
                        }
                    }
                    catch (Exception e)
                    {
                        MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_WARNING, "Error loading Image-Info for Page '" + page.Name + "' (" + page.Id + ") [" + e.Message + "]");
                    }
                }

                ImageInfoPagesSlice.Clear();
            }));
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
                        if (e.Current > -1 && e.Current <= e.Total)
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


        private void ApplicationStateChanged(object sender, ApplicationStatusEvent e)
        {
            String info = applicationStatusLabel.Text;
            String filename = e.ArchiveInfo.FileName;

            Program.ProjectModel.ApplicationState = e.State;

            switch (e.State)
            {
                case ApplicationStatusEvent.STATE_ANALYZING:
                    info = "Analyzing images...";
                    break;

                default:
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
                    pageCountStatusLabel.Text = e.ArchiveInfo.Pages.Count.ToString() + " Pages";

                    //DisableControllsForApplicationState(e.ArchiveInfo, e.State);
                }));
                //}
            }
            catch (Exception)
            {

            }
        }


        private void DisableControllsForApplicationState(ProjectModel project, int state)
        {
            switch (state)
            {
                case ApplicationStatusEvent.STATE_READY:
                    PagesList.Enabled = true;
                    PageView.Enabled = true;
                    MetaDataGrid.Enabled = true;

                    break;

                case ApplicationStatusEvent.STATE_OPENING:
                    PagesList.Enabled = false;
                    PageView.Enabled = false;
                    MetaDataGrid.Enabled = false;

                    break;

                case ApplicationStatusEvent.STATE_SAVING:
                    PagesList.Enabled = false;
                    PageView.Enabled = false;
                    MetaDataGrid.Enabled = false;

                    break;

                case ApplicationStatusEvent.STATE_CLOSING:
                    PagesList.Enabled = false;
                    PageView.Enabled = false;
                    MetaDataGrid.Enabled = false;

                    break;

                case ApplicationStatusEvent.STATE_ADDING:
                    
                    break;

                case ApplicationStatusEvent.STATE_DELETING:
                    PagesList.Enabled = false;
                    PageView.Enabled = false;

                    break;

                case ApplicationStatusEvent.STATE_RENAMING:
                    
                    break;

                case ApplicationStatusEvent.STATE_ANALYZING:
                    PagesList.Enabled = false;
                    PageView.Enabled = false;
                    MetaDataGrid.Enabled = false;

                    break;

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
                        if (e.State != CBZArchiveStatusEvent.ARCHIVE_FILE_ADDED && e.State != CBZArchiveStatusEvent.ARCHIVE_FILE_RENAMED)
                        {
                            toolStripProgressBar.Value = 0;
                        }
                    }));
                }
            }
            catch (Exception)
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
                    
                    this.Invoke(new Action(() =>
                    {
                        Program.ProjectModel.Pages.Clear();
                        PagesList.Clear();
                        PageView.Clear();
                        PageImages.Images.Clear();
                    }));
                    
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
                    info = "Adding image...";
                    break;

                case CBZArchiveStatusEvent.ARCHIVE_FILE_RENAMED:
                    info = "Renaming page...";
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
                    pageCountStatusLabel.Text = e.ArchiveInfo.Pages.Count.ToString() + " Pages";

                    DisableControllsForArchiveState(e.ArchiveInfo, e.State);
                }));
                //}
            }
            catch (Exception)
            {

            }
        }
     

        private void DisableControllsForArchiveState(ProjectModel project, int state) 
        {
            switch (state)
            {
                case CBZArchiveStatusEvent.ARCHIVE_SAVING:
                case CBZArchiveStatusEvent.ARCHIVE_OPENING:
                    newToolStripMenuItem.Enabled = false;
                    openToolStripMenuItem.Enabled = false;
                    saveAsToolStripMenuItem.Enabled = false;
                    ToolButtonRemoveFiles.Enabled = false;
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
                    toolStripButtonShowRawMetadata.Enabled = false;
                    break;

                case CBZArchiveStatusEvent.ARCHIVE_OPENED:
                    newToolStripMenuItem.Enabled = true;
                    openToolStripMenuItem.Enabled = true;
                    saveAsToolStripMenuItem.Enabled = true; 
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
                    toolStripButtonShowRawMetadata.Enabled = true;
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
                    saveAsToolStripMenuItem.Enabled = true;
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
                    toolStripButtonShowRawMetadata.Enabled = true;
                    ExtractSelectedPages.Enabled = true;
                    Program.ProjectModel.IsNew = false;
                    Program.ProjectModel.IsSaved = true;
                    break;

                case CBZArchiveStatusEvent.ARCHIVE_EXTRACTING:
                    newToolStripMenuItem.Enabled = false;
                    openToolStripMenuItem.Enabled = false;
                    saveAsToolStripMenuItem.Enabled = false;
                    ToolButtonRemoveFiles.Enabled = false;
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
                    saveAsToolStripMenuItem.Enabled = true;
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
                    saveAsToolStripMenuItem.Enabled = false;
                    ToolButtonRemoveFiles.Enabled = false;
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
                    toolStripButtonShowRawMetadata.Enabled = false;
                    RemoveMetaData();
                    break;

                case CBZArchiveStatusEvent.ARCHIVE_CLOSED:
                    newToolStripMenuItem.Enabled = true;
                    openToolStripMenuItem.Enabled = true;
                    saveAsToolStripMenuItem.Enabled = false;
                    ToolButtonNew.Enabled = true;
                    ToolButtonOpen.Enabled = true;
                    addFilesToolStripMenuItem.Enabled = true;
                    ToolButtonAddFiles.Enabled = true;
                    ToolButtonRemoveFiles.Enabled = false;
                    ToolButtonMovePageDown.Enabled = false;
                    ToolButtonMovePageUp.Enabled = false;
                    ToolButtonAddFolder.Enabled = true;
                    BtnAddMetaData.Enabled = Program.ProjectModel.MetaData.Values.Count == 0;
                    AddMetaDataRowBtn.Enabled = Program.ProjectModel.MetaData.Values != null;
                    BtnRemoveMetaData.Enabled = Program.ProjectModel.MetaData.Values.Count > 0;
                    TextboxStoryPageRenamingPattern.Enabled = false;
                    TextboxSpecialPageRenamingPattern.Enabled = false;
                    CheckBoxDoRenamePages.Enabled = false;
                    CheckBoxDoRenamePages.Checked = false;
                    ToolButtonSave.Enabled = false;
                    saveToolStripMenuItem.Enabled = false;
                    toolStripButtonShowRawMetadata.Enabled = false;
                    LabelGlobalActionStatusMessage.Text = "";
                    GlobalAlertTableLayout.Visible = false;
                    CurrentGlobalAction = null;
                    MessageLogListView.Items.Clear();
                    MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_INFO, Win_CBZSettings.Default.AppName + " v" + Win_CBZSettings.Default.Version + "  - Welcome!");
                    RemoveMetaData();
                    break;

                case CBZArchiveStatusEvent.ARCHIVE_FILE_ADDED:
                    CheckBoxDoRenamePages.Enabled = true;
                    CheckBoxDoRenamePages.Checked = false;
                    if (project.FileName != null)
                    {
                        ToolButtonSave.Enabled = true;
                        saveToolStripMenuItem.Enabled = true;
                        saveAsToolStripMenuItem.Enabled = true;
                    }
                    break;

                case CBZArchiveStatusEvent.ARCHIVE_FILE_DELETED:
                case CBZArchiveStatusEvent.ARCHIVE_FILE_RENAMED:
                case CBZArchiveStatusEvent.ARCHIVE_FILE_UPDATED:
                case CBZArchiveStatusEvent.ARCHIVE_METADATA_ADDED:
                case CBZArchiveStatusEvent.ARCHIVE_METADATA_CHANGED:
                case CBZArchiveStatusEvent.ARCHIVE_METADATA_DELETED:
                    if (project.FileName != null)
                    {
                        ToolButtonSave.Enabled = true;
                        saveToolStripMenuItem.Enabled = true;
                        saveAsToolStripMenuItem.Enabled = true;
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
                    if (OpeningTask.IsAlive)
                    {
                        OpeningTask.Join();
                    }
                    //while (OpeningTask.IsAlive)
                    //{
                    //    System.Threading.Thread.Sleep(50);
                    //}
                }

                if (SavingTask != null)
                {
                    if (SavingTask.IsAlive)
                    {
                        SavingTask.Join();
                    }
                    //while (SavingTask.IsAlive)
                    //{
                    //    System.Threading.Thread.Sleep(50);
                    //}
                }

                if (ThumbnailThread != null)
                {
                    if (ThumbnailThread.IsAlive)
                    {
                        ThumbnailThread.Join();
                    }
                    //while (ThumbnailThread.IsAlive)
                    //{
                    //    System.Threading.Thread.Sleep(50);
                    //}
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
                        //PagesList.Items.Clear();
                        //PageView.Clear();

                        //PageImages.Images.Clear();

                        toolStripProgressBar.Value = 0;

                        ClearProject();
                        NewProject();
                    }
                }
                else
                {
                    //PagesList.Items.Clear();
                    //PageView.Clear();

                    //PageImages.Images.Clear();

                    toolStripProgressBar.Value = 0;

                    ClearProject();
                    NewProject();
                }          
            } else
            {

                if (ArchiveProcessing())
                {
                    ApplicationMessage.ShowWarning("Please wait until current operation has finished.", "Still operations in progress", 2, ApplicationMessage.DialogButtons.MB_OK);

                }
            }
        }

        private void PageView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            System.Windows.Forms.ListView.SelectedListViewItemCollection selectedPages = this.PageView.SelectedItems;
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
                e.Cancel = true;
            }
            
        }

        private void AddFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {

            DialogResult openImageResult = OpenImagesDialog.ShowDialog();

            if (openImageResult == DialogResult.OK)
            {
                var maxIndex = PagesList.Items.Count - 1;
                var newIndex = maxIndex < 0 ? 0 : maxIndex;
                Program.ProjectModel.ParseFiles(new List<String>(OpenImagesDialog.FileNames));
                //if (files.Count > 0)
                //{
                //    Program.ProjectModel.AddImages(files, newIndex);
                //}

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
                        ArchiveStateChanged(sender, new CBZArchiveStatusEvent(Program.ProjectModel, CBZArchiveStatusEvent.ARCHIVE_FILE_RENAMED));
                    }
                    catch (PageDuplicateNameException eduplicate)
                    {                      
                        MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_ERROR, eduplicate.Message);
                        e.CancelEdit = true;
                        ApplicationMessage.ShowException(eduplicate, ApplicationMessage.MT_ERROR);
                    }
                    catch (PageException)
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

    
        private void MoveItemsTo(int newIndex, System.Windows.Forms.ListView.SelectedListViewItemCollection items)
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
                if (pageNew != null && pageOriginal != null)
                {
                    PageView.Items.Remove(pageOriginal);
                    PageView.Items.Remove(pageNew);
                }

                Program.ProjectModel.Pages.Remove((Page)originalItem.Tag);
                Program.ProjectModel.Pages.Remove((Page)item.Tag);
                if (direction == 1)
                {
                    PagesList.Items.Insert(newIndex, item);
                    PagesList.Items.Insert(IndexOldItem, originalItem);
                    if (pageNew != null && pageOriginal != null)
                    {
                        PageView.Items.Insert(newIndex, pageNew);
                        PageView.Items.Insert(IndexOldItem, pageOriginal);
                    }
                    Program.ProjectModel.Pages.Insert(newIndex, (Page)item.Tag);
                    Program.ProjectModel.Pages.Insert(IndexOldItem, (Page)originalItem.Tag);
                }
                else
                {
                    PagesList.Items.Insert(IndexOldItem, originalItem);
                    PagesList.Items.Insert(newIndex, item);
                    PageView.Items.Insert(IndexOldItem, pageOriginal);
                    PageView.Items.Insert(newIndex, pageNew);
                    Program.ProjectModel.Pages.Insert(IndexOldItem, (Page)originalItem.Tag);
                    Program.ProjectModel.Pages.Insert(newIndex, (Page)item.Tag);
                }

                PageChanged(this, new PageChangedEvent((Page)pageNew.Tag, PageChangedEvent.IMAGE_STATUS_CHANGED));
                PageChanged(this, new PageChangedEvent((Page)originalItem.Tag, PageChangedEvent.IMAGE_STATUS_CHANGED));
                ArchiveStateChanged(this, new CBZArchiveStatusEvent(Program.ProjectModel, CBZArchiveStatusEvent.ARCHIVE_FILE_UPDATED));

                HandleGlobalActionRequired(null, new GlobalActionRequiredEvent(Program.ProjectModel, 0, "Page order changed. Rebuild pageindex now?", "Rebuild", RebuildPageIndexMetaDataTask.UpdatePageIndexMetadata(Program.ProjectModel.Pages, Program.ProjectModel.MetaData, HandleGlobalTaskProgress, PageChanged)));


                //Program.ProjectModel.Pages.Insert(newIndex, (Page)item.Tag);
                //Program.ProjectModel.Pages.Insert(IndexOldItem, (Page)originalItem.Tag);
                newIndex += direction;
            }

            //Program.ProjectModel.UpdatePageIndices();
            Program.ProjectModel.IsChanged = true;
        }


        private bool ArchiveProcessing()
        {
            return (Program.ProjectModel.ArchiveState == CBZArchiveStatusEvent.ARCHIVE_SAVING ||
               Program.ProjectModel.ArchiveState == CBZArchiveStatusEvent.ARCHIVE_OPENING ||
               Program.ProjectModel.ArchiveState == CBZArchiveStatusEvent.ARCHIVE_EXTRACTING ||
               Program.ProjectModel.ArchiveState == CBZArchiveStatusEvent.ARCHIVE_CLOSING ||
               Program.ProjectModel.ApplicationState == GeneralTaskProgressEvent.TASK_STATUS_RUNNING
               );
        }


        private void AddMetaData()
        {
            //if (Program.ProjectModel.MetaData == null)
            //{
                Program.ProjectModel.NewMetaData(true);
            //}

            Program.ProjectModel.MetaData.FillMissingDefaultProps();
            if (PagesList.Items.Count > 0)
            {
                Program.ProjectModel.MetaData.RebuildPageMetaData(Program.ProjectModel.Pages.ToList<Page>());
            }

            MetaDataLoaded(this, new MetaDataLoadEvent(Program.ProjectModel.MetaData.Values));

            BtnAddMetaData.Enabled = false;
            BtnRemoveMetaData.Enabled = true;
            AddMetaDataRowBtn.Enabled = true;
            RemoveMetadataRowBtn.Enabled = false;
            toolStripButtonShowRawMetadata.Enabled = true;
        }

        private void RemoveMetaData() 
        {
            if (Program.ProjectModel.MetaData != null)
            {
                MetaDataGrid.DataSource = null;
                MetaDataGrid.Rows.Clear();
                MetaDataGrid.Columns.Clear();

                Program.ProjectModel.MetaData.Values.Clear();
                BtnAddMetaData.Enabled = true;
                BtnRemoveMetaData.Enabled = false;
                AddMetaDataRowBtn.Enabled = false;
                RemoveMetadataRowBtn.Enabled = false;
                toolStripButtonShowRawMetadata.Enabled = false;
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
                if (Program.ProjectModel.MetaData.HasValues() && Program.ProjectModel.Exists()) {
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
            Program.ProjectModel.MetaData.Add("");
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
                    if (row.Cells[0].Value != null)
                    {
                        var key = row.Cells[0].Value.ToString();  

                        Program.ProjectModel.MetaData.Remove(key);
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
                    object key = MetaDataGrid.Rows[e.RowIndex].Cells[0].Value;
                    object value = MetaDataGrid.Rows[e.RowIndex].Cells[1].Value;

                    String keyStr = "";
                    String valStr = "";

                    if (key != null)
                    {
                        keyStr = key.ToString();
                    }

                    if (value != null)
                    {
                        valStr = value.ToString();
                    }

                    Program.ProjectModel.MetaData.Validate(new MetaDataEntry(keyStr, valStr), e.FormattedValue.ToString());
                }
            } catch (MetaDataValidationException ve)
            {
                MetaDataGrid.Rows[e.RowIndex].ErrorText = ve.Message;
                //e.Cancel = true;
            }
        }

        private void MetaDataGrid_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            //
            /*
            DataGridViewCellStyle dataGridViewCellStyle = new DataGridViewCellStyle();
            
            dataGridViewCellStyle.ForeColor = e.CellStyle.ForeColor;
            dataGridViewCellStyle.BackColor = e.CellStyle.BackColor;
            dataGridViewCellStyle.SelectionForeColor = Color.White;
            dataGridViewCellStyle.SelectionBackColor = HTMLColor.ToColor(Colors.COLOR_LIGHT_GREEN);

            e.CellStyle = dataGridViewCellStyle;
            */
        }

        private void MetaDataGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                //FileInfo fi = new FileInfo(Program.ProjectModel.FileName);
                Program.ProjectModel.IsChanged = true;

                //if (fi.Exists)
                //{
                    ToolButtonSave.Enabled = true;
                    saveToolStripMenuItem.Enabled = true;
                //}

                string Key = "";
                string Val = "";

                object value = MetaDataGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
               
                if (e.ColumnIndex == 0)
                {
                    if (value == null)
                    {
                        value = "";
                    }

                    Key = value.ToString();
                    value = MetaDataGrid.Rows[e.RowIndex].Cells[e.ColumnIndex + 1].Value;

                    if (value == null)
                    {
                        value = "";
                    }
                    
                    Val = value.ToString();                  
                }

                if (e.ColumnIndex == 1)
                {
                    if (value == null)
                    {
                        value = "";
                    }

                    Val = value.ToString();
                    value = MetaDataGrid.Rows[e.RowIndex].Cells[e.ColumnIndex - 1].Value;

                    if (value == null)
                    {
                        value = "";
                    }

                    Key = value.ToString();
                }

                MetaDataEntry updatedEntry = Program.ProjectModel.MetaData.UpdateEntry(e.RowIndex, new MetaDataEntry(Key, Val));
                MetaDataGrid.Rows[e.RowIndex].ErrorText = null;
                MetaDataGrid.Invalidate();

                if (e.ColumnIndex == 0) { 
                    var key = MetaDataGrid.Rows[e.RowIndex].Cells[0].Value;
                    if (key != null)
                    {
                        if (updatedEntry.Key == key.ToString())
                        {
                            if (updatedEntry.Options.Length > 0)
                            {
                                int selectedIndex = Array.IndexOf(updatedEntry.Options, updatedEntry.Value);
                                DataGridViewComboBoxCell c = new DataGridViewComboBoxCell();
                                c.Items.AddRange(updatedEntry.Options);
                                c.Value = value; //selectedIndex > -1 ? selectedIndex : 0;

                                MetaDataGrid.Rows[e.RowIndex].Cells[1] = c;
                            }
                            else
                            {
                                DataGridViewTextBoxCell c = new DataGridViewTextBoxCell();
                                c.Value = updatedEntry.Value;

                                MetaDataGrid.Rows[e.RowIndex].Cells[1] = c;
                            }
                        }
                    }
                }
                
            }
            catch (Exception) 
            { 
            }
        }

        private void MetaDataGrid_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            if (e.Exception != null) {
                MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_ERROR, e.Exception.Message);
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
            System.Windows.Forms.ListView.SelectedListViewItemCollection selectedPages = this.PagesList.SelectedItems;
            bool buttonState = selectedPages.Count > 0;

            ToolButtonRemoveFiles.Enabled = buttonState;
            ToolButtonMovePageDown.Enabled = buttonState && selectedPages.Count != PagesList.Items.Count;
            ToolButtonMovePageUp.Enabled = buttonState && selectedPages.Count != PagesList.Items.Count;

            ToolButtonSetPageType.Enabled = buttonState;

            if (buttonState)
            {
                if (!((Page)selectedPages[0].Tag).ImageInfoRequested &&  ((Page)selectedPages[0].Tag).W == 0 && ((Page)selectedPages[0].Tag).H == 0)
                {
                  
                    ImageInfoPagesSlice.Add(((Page)selectedPages[0].Tag));
                }
                //((Page)selectedPages[0].Tag).LoadImageInfo();
                LabelW.Text = ((Page)selectedPages[0].Tag).W.ToString();
                LabelH.Text = ((Page)selectedPages[0].Tag).H.ToString();
                //RequestImageInfoSlice();
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
            System.Windows.Forms.ListView.SelectedListViewItemCollection selectedPages = this.PagesList.SelectedItems;

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

                //Program.ProjectModel.UpdatePageIndices();
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
           

            if (CheckBoxDoRenamePages.Checked == false)
            {
                CheckBoxPreview.Enabled = false;
                if (CheckBoxPreview.Checked)
                {
                    Program.ProjectModel.RestoreOriginalNames();
                    
                }
            } else
            {
                CheckBoxPreview.Enabled = true;
            }
           
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
                PageView.Invoke(new Action(() =>
                {
                    foreach (ListViewItem pageItem in PagesList.Items)
                    {
                        CreatePagePreviewFromItem((Page)pageItem.Tag);
                    }
                }));

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
            System.Windows.Forms.ListView owner = sender as System.Windows.Forms.ListView;
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

        private void toolStripButtonShowRawMetadata_Click(object sender, EventArgs e)
        {
            try
            {
                MemoryStream ms = Program.ProjectModel.MetaData.BuildComicInfoXMLStream(true);
                var utf8WithoutBom = new System.Text.UTF8Encoding(false);
                String metaData = utf8WithoutBom.GetString(ms.ToArray());

                MetaDataForm metaDataDialog = new MetaDataForm(metaData);
                metaDataDialog.ShowDialog();
            } catch (Exception ex)
            {
                ApplicationMessage.ShowException(ex);
            }

        }

        private void btnGetExcludesFromSelectedPages_Click(object sender, EventArgs e)
        {
            List<String> excludes = new List<String>();
            foreach (ListViewItem item in PagesList.SelectedItems)
            {
                excludes.Add(((Page)item.Tag).Name);
            }

            RenamerExcludePages.Lines = excludes.ToArray<String>();
        }

        private void RenamerExcludePages_TextChanged(object sender, EventArgs e)
        {
            Program.ProjectModel.RenamerExcludes.Clear();
            Program.ProjectModel.RenamerExcludes.AddRange(RenamerExcludePages.Lines);
        }

        private void PagesList_DrawItem(object sender, DrawListViewItemEventArgs e)
        {

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
