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
using Win_CBZ.Models;
using System.Windows.Input;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using SharpCompress;
using Win_CBZ.Exceptions;

namespace Win_CBZ
{
    public partial class MainForm : Form
    {

        private Thread ClosingTask;

        //private Thread OpeningTask;

        //private Thread SavingTask;

        private bool WindowClosed = false;

        private bool WindowShown = false;

        private String LastOutputDirectory = "";

        private Thread ThumbnailThread;

        private Thread RequestThumbnailThread;

        private Thread RequestImageInfoThread;

        private Thread UpdatePageViewThread;

        private Thread MovePagesThread;

        private Thread MoveItemsThread;

        private List<Page> ThumbnailPagesSlice;

        private List<Page> ImageInfoPagesSlice;

        private GlobalAction CurrentGlobalAction;

        private List<GlobalAction> CurrentGlobalActions;

        private ImageTask selectedImageTask;

        private DebugForm df;

        public MainForm()
        {
            InitializeComponent();

            Program.ProjectModel = NewProjectModel();

            MessageLogger.Instance.SetHandler(MessageLogged);

            ThumbnailPagesSlice = new List<Page>();
            ImageInfoPagesSlice = new List<Page>();
            CurrentGlobalActions = new List<GlobalAction>();

            Width = Win_CBZSettings.Default.WindowW;
            Height = Win_CBZSettings.Default.WindowH;
            MainSplitBox.SplitterDistance = Win_CBZSettings.Default.Splitter1;
            SplitBoxPageView.SplitterDistance = Win_CBZSettings.Default.Splitter2;
            SplitBoxItemsList.SplitterDistance = Win_CBZSettings.Default.Splitter3;
            PrimarySplitBox.SplitterDistance = Win_CBZSettings.Default.Splitter4;

            df = new DebugForm(PageView);
        }

        private ProjectModel NewProjectModel()
        {
            ProjectModel newProjectModel = new ProjectModel(Win_CBZSettings.Default.TempFolderPath, Win_CBZSettings.Default.MetaDataFilename);
            
            newProjectModel.ArchiveStatusChanged += ArchiveStateChanged;
            newProjectModel.TaskProgress += TaskProgress;
            newProjectModel.PageChanged += PageChanged;
            newProjectModel.CBZValidationEventHandler += ValidationFinished;
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
            newProjectModel.CompatibilityMode = Win_CBZSettings.Default.CompatMode;


            Text = Win_CBZSettings.Default.AppName + " (c) Trash_s0Ft";

            return newProjectModel;
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            if (!WindowShown)
            {
                MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_INFO, Win_CBZSettings.Default.AppName + " v" + Win_CBZSettings.Default.Version + "  - Welcome!");

                FileSettingsTablePanel.Width = MainSplitBox.SplitterDistance - 24;

                TextboxStoryPageRenamingPattern.Text = Win_CBZSettings.Default.StoryPageRenamePattern;
                TextboxSpecialPageRenamingPattern.Text = Win_CBZSettings.Default.SpecialPageRenamePattern;

                TogglePagePreviewToolbutton.Checked = Win_CBZSettings.Default.PagePreviewEnabled;
                SplitBoxPageView.Panel1Collapsed = !Win_CBZSettings.Default.PagePreviewEnabled;
                Program.ProjectModel.PreloadPageImages = Win_CBZSettings.Default.PagePreviewEnabled;
                CheckBoxIgnoreErrorsOnSave.Checked = Win_CBZSettings.Default.IgnoreErrorsOnSave;

                CheckBoxCompatibilityMode.Checked = Win_CBZSettings.Default.CompatMode;

                ComboBoxCompressionLevel.SelectedIndex = 0;

                DebugToolsToolStripMenuItem.Visible = Win_CBZSettings.Default.DebugMode == "3ab980acc9ab16b";

                TextBoxMetaDataFilename.Text = Win_CBZSettings.Default.MetaDataFilename;


                Label placeholderLabel;
                foreach (String placeholder in Win_CBZSettings.Default.RenamerPlaceholders)
                {
                    placeholderLabel = new Label
                    {
                        Name = "Label" + placeholder,
                        Text = placeholder,
                        AutoSize = true
                    };
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
                try
                {
                    Program.ProjectModel.New();
                    ClearLog();
                } catch (ConcurrentOperationException c)
                {
                    if (c.ShowErrorDialog)
                    {
                        ApplicationMessage.ShowWarning(c.Message, "Concurrency Exception", ApplicationMessage.DialogType.MT_WARNING, ApplicationMessage.DialogButtons.MB_OK);
                    }
                }              
            }
        }

        private void ClearLog()
        {
            MessageLogListView.Items.Clear();
            MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_INFO, Win_CBZSettings.Default.AppName + " v" + Win_CBZSettings.Default.Version + "  - Welcome!");
        }


        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {

            DialogResult openCBFResult = OpenCBFDialog.ShowDialog();

            if (openCBFResult == DialogResult.OK)
            {
                ClosingTask = Program.ProjectModel.Close();

                ClearLog();

                Task.Factory.StartNew(() =>
                {
                    if (ClosingTask != null)
                    {
                        if (ClosingTask.IsAlive)
                        {
                            ClosingTask.Join();
                        }
                    }

                    Program.ProjectModel.Open(OpenCBFDialog.FileName, ZipArchiveMode.Read);
                });
            }
        }

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult saveDialogResult = SaveArchiveDialog.ShowDialog();

            if (saveDialogResult == DialogResult.OK)
            {
                if (Program.ProjectModel.SaveAs(SaveArchiveDialog.FileName, ZipArchiveMode.Update))
                {

                }
            }
        }

        private void ToolButtonSave_Click(object sender, EventArgs e)
        {
            if (Program.ProjectModel.IsNew && !Program.ProjectModel.IsSaved)
            {
                SaveAsToolStripMenuItem_Click(sender, e);
            } else
            {
                if (Program.ProjectModel.Exists())
                {
                    if (Program.ProjectModel.Save())
                    {

                    }
                } else
                {
                    MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_WARNING, "Warning! Wrong application state. Current File does not exists! [" + Program.ProjectModel.FileName + "]");
                    SaveAsToolStripMenuItem.Enabled = true;
                    SaveAsToolStripMenuItem_Click(sender, e);
                }
            }
        }

        private void AddFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult openImageResult = AddPagesDirDialog.ShowDialog();

            if (openImageResult == DialogResult.OK)
            {

                var maxIndex = PagesList.Items.Count - 1;
                var newIndex = maxIndex < 0 ? 0 : maxIndex;
                var files = Program.ProjectModel.LoadDirectory(AddPagesDirDialog.SelectedPath);
                Program.ProjectModel.ParseFiles(files);
                //if (files.Count > 0)
                //{
                //    Program.ProjectModel.AddImages(files, newIndex);
                //}

            }
        }

        private void ValidationFinished(object sender, ValidationFinishedEvent e)
        {
            if (e.ShowErrorsDialog)
            {
                if (e.ValidationErrors.Length > 0)
                {
                    ApplicationMessage.ShowCustom("Validation finished with Errors:\r\n\r\n" + e.ValidationErrors.Select(s => s + "\r\n").Aggregate((a, b) => a + b), "CBZ Archive validation failed!", ApplicationMessage.DialogType.MT_INFORMATION, ApplicationMessage.DialogButtons.MB_OK, ScrollBars.Both, 560);
                }
                else
                {
                    ApplicationMessage.Show("Validation Successfull! CBZ Archive is valid, no problems detected.", "CBZ Archive validation successfull!", ApplicationMessage.DialogType.MT_CHECK, ApplicationMessage.DialogButtons.MB_OK);
                }
            }
        }

        private void PageChanged(object sender, PageChangedEvent e)
        {
            if (!WindowClosed)
            {
                PagesList.Invoke(new Action(() =>
                {
                    if (e.State != PageChangedEvent.IMAGE_STATUS_CLOSED)
                    {
                        ListViewItem item;
                        ListViewItem existingItem = FindListViewItemForPage(PagesList, e.Page);

                        if (existingItem == null)
                        {
                            item = PagesList.Items.Add(e.Page.Name);
                            item.ImageKey = e.Page.Id;
                            item.SubItems.Add(e.Page.Number.ToString());
                            item.SubItems.Add(e.Page.ImageType.ToString());
                            item.SubItems.Add(e.Page.LastModified.ToString());
                            item.SubItems.Add(e.Page.SizeFormat());
                        }
                        else
                        {
                            item = existingItem;
                            item.Text = e.Page.Name;
                            item.SubItems[1] = new ListViewItem.ListViewSubItem(item, !e.Page.Deleted ? e.Page.Number.ToString() : "-");
                            item.SubItems[2] = new ListViewItem.ListViewSubItem(item, e.Page.ImageType.ToString());
                            item.SubItems[3] = new ListViewItem.ListViewSubItem(item, e.Page.LastModified.ToString());
                            item.SubItems[4] = new ListViewItem.ListViewSubItem(item, e.Page.SizeFormat());
                        }

                        item.Tag = e.Page;
                        item.BackColor = Color.White;
                        item.ForeColor = Color.Black;

                        switch (e.State)
                        {
                            case PageChangedEvent.IMAGE_STATUS_NEW:
                                if (!e.Page.Compressed)
                                {
                                    ImageInfoPagesSlice.Add(e.Page);
                                }
                                break;
                            case PageChangedEvent.IMAGE_STATUS_DELETED:
                                e.Page.Deleted = true;
                                break;
                            case PageChangedEvent.IMAGE_STATUS_COMPRESSED:
                                e.Page.Compressed = true;
                                break;
                            case PageChangedEvent.IMAGE_STATUS_CHANGED:
                            case PageChangedEvent.IMAGE_STATUS_RENAMED:
                                e.Page.Changed = true;
                                e.Page.Invalidated = true;
                                e.Page.ThumbnailInvalidated = true;
                                break;
                        }

                        if (!e.Page.Compressed)
                        {
                            item.BackColor = HTMLColor.ToColor(Colors.COLOR_LIGHT_ORANGE);
                        }

                        if (e.Page.Changed)
                        {
                            if (e.Page.Compressed)
                            {
                                item.BackColor = HTMLColor.ToColor(Colors.COLOR_LIGHT_GREEN);
                            } else
                            {
                                item.BackColor = HTMLColor.ToColor(Colors.COLOR_LIGHT_PURPLE);
                            }
                        }

                        if (e.Page.Deleted)
                        {
                            item.ForeColor = Color.Silver;
                            item.BackColor = Color.Transparent;
                        }

                    }
                    else
                    {
                        /*
                        if (e.State == PageChangedEvent.IMAGE_STATUS_CLOSED)
                        {
                            ListViewItem existingItem = FindListViewItemForPage(PagesList, e.Image);

                            if (existingItem != null)
                            {
                                PagesList.Items.Remove(existingItem);
                            }

                            existingItem = FindListViewItemForPage(PageView, e.Image);

                            if (existingItem != null)
                            {
                                PageView.Items.Remove(existingItem);
                            }
                        }
                        */
                    }
                }));

                if (TogglePagePreviewToolbutton.Checked)
                {

                    if (!e.Page.Closed && !e.Page.Deleted)
                    {
                        PageThumbsListBox.Invoke(new Action(() =>
                        {
                            CreatePagePreviewFromItem(e.Page);
                        }));
                    }
                }
            }
        }

        private void OperationFinished(object sender, OperationFinishedEvent e)
        {
            MainToolStripProgressBar.Control.Invoke(new Action(() =>
            {
                MainToolStripProgressBar.Maximum = 100;
                MainToolStripProgressBar.Value = 0;
            }));
        }

        private void FileOperationHandler(object sender, FileOperationEvent e)
        {
            MainToolStripProgressBar.Control.Invoke(new Action(() =>
            {
                if (e.Status == FileOperationEvent.STATUS_RUNNING)
                {
                    MainToolStripProgressBar.Maximum = 100;
                    MainToolStripProgressBar.Value = Convert.ToInt32(100 * e.Completed / e.Total);

                    if (e.Operation == FileOperationEvent.OPERATION_COPY)
                    {
                        ApplicationStatusLabel.Text = "Copying file...";
                    }

                } else
                {
                    MainToolStripProgressBar.Value = 0;
                    ApplicationStatusLabel.Text = "Ready.";

                }
            }));
        }

        private void ArchiveOperationHandler(object sender, ArchiveOperationEvent e)
        {
            MainToolStripProgressBar.Control.Invoke(new Action(() =>
            {
                MainToolStripProgressBar.Maximum = e.Total;
                MainToolStripProgressBar.Value = e.Completed;
            }));
        }

        private void PageOperationHandler(object sender, ArchiveOperationEvent e)
        {
            MainToolStripProgressBar.Control.Invoke(new Action(() =>
            {
                MainToolStripProgressBar.Maximum = e.Total;
                MainToolStripProgressBar.Value = e.Completed;
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

        private Page FindThumbImageForPage(object owner, Page page)
        {
          
            foreach (object item in PageThumbsListBox.Items)
            {
                if (((Page)item).Id.Equals(page.Id))
                {
                    return item as Page;
                }
            }

            return null;
           
        }

        private void HandleGlobalActionRequired(object sender, GlobalActionRequiredEvent e)
        {
            this.Invoke(new Action(() =>
            {
                GlobalAction action = new GlobalAction
                {
                    Message = e.Message,
                    Action = e.Task,
                    Key = e.TaskType
                };

                if (GetGlobalActionByKey(action.Key) == null)
                {
                    CurrentGlobalActions.Add(action);
                }

                if (CurrentGlobalAction == null)
                {
                    LabelGlobalActionStatusMessage.Text = e.Message;
                    GlobalAlertTableLayout.Visible = true;
                    CurrentGlobalAction = action;
                }
            }));
        }

        private GlobalAction GetGlobalActionByKey(string key)
        {
            foreach (GlobalAction globalAction in CurrentGlobalActions)
            {
                if (globalAction.Key == key)
                {
                    return globalAction;
                }
            }
            return null;
        }

        private void HandleGlobalTaskProgress(object sender, GeneralTaskProgressEvent e)
        {
            Program.ProjectModel.ApplicationState = e.Status;

            if (e.Status == GeneralTaskProgressEvent.TASK_STATUS_RUNNING)
            {
                if (!WindowClosed)
                {
                    Invoke(new Action(() =>
                    {
                        SaveToolStripMenuItem.Enabled = false;
                        SaveAsToolStripMenuItem.Enabled = false;
                        ToolButtonSave.Enabled = false;
                        ToolButtonNew.Enabled = false;
                        ToolButtonAddFiles.Enabled = false;
                        ToolButtonMovePageDown.Enabled = false;
                        ToolButtonMovePageUp.Enabled = false;
                        ToolButtonRemoveFiles.Enabled = false;
                        NewToolStripMenuItem.Enabled = false;
                        ApplicationStatusLabel.Text = e.Message;
                    }));
                }

                try
                {
                    if (!WindowClosed)
                    {
                        MainToolStripProgressBar.Control.Invoke(new Action(() =>
                        {
                            MainToolStripProgressBar.Maximum = e.Total;
                            if (e.Current > -1 && e.Current <= e.Total)
                            {
                                MainToolStripProgressBar.Value = e.Current;
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
                if (!WindowClosed)
                {
                    Invoke(new Action(() =>
                    {
                        SaveToolStripMenuItem.Enabled = true;
                        SaveAsToolStripMenuItem.Enabled = true;
                        ToolButtonSave.Enabled = true;
                        ToolButtonNew.Enabled = true;
                        ToolButtonAddFiles.Enabled = true;
                        ToolButtonMovePageDown.Enabled = true;
                        ToolButtonMovePageUp.Enabled = true;
                        ToolButtonRemoveFiles.Enabled = true;
                        NewToolStripMenuItem.Enabled = true;
                        ApplicationStatusLabel.Text = e.Message;
                        Program.ProjectModel.IsChanged = true;
                        Program.ProjectModel.ApplicationState = ApplicationStatusEvent.STATE_READY;

                        if (e.Type == GeneralTaskProgressEvent.TASK_RELOAD_IMAGE_METADATA)
                        {
                            Program.ProjectModel.MetaDataPageIndexMissingData = false;
                            Program.ProjectModel.MetaData.RebuildPageMetaData(Program.ProjectModel.Pages);
                        }

                        if (CurrentGlobalActions.Count > 1)
                        {
                            CurrentGlobalActions.Remove(CurrentGlobalAction);

                            CurrentGlobalAction = CurrentGlobalActions[0];

                            Invoke(new Action(() =>
                            {
                                LabelGlobalActionStatusMessage.Text = CurrentGlobalAction.Message;
                                GlobalAlertTableLayout.Visible = true;
                            }));
                        } else
                        {
                            if (CurrentGlobalActions.Count > 0) 
                                CurrentGlobalActions.Remove(CurrentGlobalAction);
                            CurrentGlobalAction = null;
                        }
                    }));

                    try
                    {
                        if (!WindowClosed)
                        {
                            MainToolStripProgressBar.Control.Invoke(new Action(() =>
                            {
                                MainToolStripProgressBar.Maximum = e.Total;

                                MainToolStripProgressBar.Value = 0;

                            }));
                        }
                    }
                    catch (Exception)
                    {

                    }
                }
            }
        }

        private void ExecuteCurrentGlobalAction_Click_1(object sender, EventArgs e)
        {
            if (CurrentGlobalAction != null)
            {
                if (!CurrentGlobalAction.Action.IsCanceled && !CurrentGlobalAction.Action.IsCompleted && !CurrentGlobalAction.Action.IsFaulted)
                {
                    CurrentGlobalAction.Action.Start();
                    GlobalAlertTableLayout.Visible = false;
                }
            }
        }

        public void ReloadPreviewThumbs()
        {
            Invoke(new Action(() =>
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
                        RequestThumbnailThread.Join();
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
            if (Program.ProjectModel.ArchiveState != CBZArchiveStatusEvent.ARCHIVE_CLOSING) { 
                Invoke(new Action(() =>
                {
                    //PageImages.Images.Clear();
                    try
                    {

                        foreach (Page page in ThumbnailPagesSlice)
                        {
                            try
                            {
                                if (!page.Closed)
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
                                else
                                {
                                    page.ThumbnailInvalidated = false;
                                }
                            }
                            catch (Exception e)
                            {
                                MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_WARNING, "Error generating Thumbnail for Page '" + page.Name + "' (" + page.Id + ") [" + e.Message + "]");
                            }
                            finally
                            {

                            }
                        }

                    } catch (Exception eo) 
                    {
                        MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_WARNING, "Error in thumb-generation thread! [" + eo.Message + "]");
                    }

                    ThumbnailPagesSlice.Clear();

                    if (TogglePagePreviewToolbutton.Checked && PageThumbsListBox.Items.Count > 0) //PageView.Items.Count > 0)
                    {
                        //PageView.RedrawItems(0, PageView.Items.Count - 1, false);
                        PageThumbsListBox.Refresh();
                    }
                }));
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private Page CreatePagePreviewFromItem(Page page)
        {
            //ListViewItem itemPage;
            Page existingItem = FindThumbImageForPage(PageView, page);

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
                PageThumbsListBox.Items.Add(page);
            } else
            {
                PageThumbsListBox.Items[PageThumbsListBox.Items.IndexOf(existingItem)] = page;
            }

            /*
            if (existingItem == null)
            {
                itemPage = PageView.Items.Add(page.Index.ToString());
                itemPage.Name = page.Index.ToString();
                itemPage.ImageKey = page.Id;
                itemPage.SubItems.Add(page.Name);
                itemPage.SubItems.Add(page.Id.ToString());
            }
            else
            {
                itemPage = existingItem;
                itemPage.Text = page.Index.ToString(); 
                itemPage.Name = page.Index.ToString();
                itemPage.ImageKey = page.Id;
                itemPage.SubItems[1] = new ListViewItem.ListViewSubItem(itemPage, page.Name);
                itemPage.SubItems[2] = new ListViewItem.ListViewSubItem(itemPage, page.Id.ToString());
            }
            */

            //itemPage.Tag = page;

            return page;
        }


        private bool ThumbAbort()
        {

            return true;
        }

        public void RequestImageInfoSlice()
        {
            if (Win_CBZSettings.Default.PagePreviewEnabled)
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
                        while (RequestImageInfoThread.IsAlive)
                        {
                            Thread.Sleep(5);
                        }
                    }
                }

                RequestImageInfoThread = new Thread(new ThreadStart(LoadImageInfoSlice));
                RequestImageInfoThread.Start();
            }
        }

        public void LoadImageInfoSlice()
        {
            Invoke(new Action(() =>
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

        public void UpdatePageView()
        {
            if (Win_CBZSettings.Default.PagePreviewEnabled)
            {
                if (MovePagesThread != null)
                {
                    if (MovePagesThread.IsAlive)
                    {
                        
                    }
                }

                if (MoveItemsThread != null)
                {
                    if (MoveItemsThread.IsAlive)
                    {
                        
                    }
                }

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
                        RequestImageInfoThread.Abort();
                    }
                }

                if (RequestImageInfoThread != null)
                {
                    if (RequestImageInfoThread.IsAlive)
                    {
                        RequestImageInfoThread.Abort();
                    }
                }

                if (UpdatePageViewThread != null)
                {
                    if (UpdatePageViewThread.IsAlive)
                    {
                        UpdatePageViewThread.Join();
                    }
                }

                UpdatePageViewThread = new Thread(new ThreadStart(UpdatePageViewProc));
                UpdatePageViewThread.Start();
            }
        }

        public void UpdatePageViewProc()
        {
            PageView.Invoke(new Action(() =>
            {
                PageView.Items.Clear();
                PageImages.Images.Clear();

                foreach (Page page in Program.ProjectModel.Pages)
                {
                    try
                    {
                        CreatePagePreviewFromItem(page);
                    }
                    catch (Exception e)
                    {
                        MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_WARNING, "Error loading Image-Info for Page '" + page.Name + "' (" + page.Id + ") [" + e.Message + "]");
                    }

                    Thread.Sleep(1);
                }

               
            }));
        }

        private void TaskProgress(object sender, TaskProgressEvent e)
        {
            try
            {
                if (!WindowClosed)
                {
                    MainToolStripProgressBar.Control.Invoke(new Action(() =>
                    {
                        MainToolStripProgressBar.Maximum = e.Total;
                        if (e.Current > -1 && e.Current <= e.Total)
                        {
                            MainToolStripProgressBar.Value = e.Current;
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
            String info = ApplicationStatusLabel.Text;
            String filename = e.ArchiveInfo.FileName;

            Program.ProjectModel.ApplicationState = e.State;

            switch (e.State)
            {
                case ApplicationStatusEvent.STATE_UPDATING_INDEX:
                    this.Invoke(new Action(() =>
                    {
                        LabelGlobalActionStatusMessage.Text = "";
                        GlobalAlertTableLayout.Visible = false;

                        CurrentGlobalAction = null;
                        CurrentGlobalActions.Clear();
                    }));
                    break;

                case ApplicationStatusEvent.STATE_ANALYZING:
                    info = "Analyzing images...";
                    break;

                case ApplicationStatusEvent.STATE_READY:
                    info = "Ready.";
                    break;

                default:
                    break;
            }

            try
            {
                //if (this.InvokeRequired)
                //{
                Invoke(new Action(() =>
                {
                    FileNameLabel.Text = filename;
                    ApplicationStatusLabel.Text = info;
                    Program.ProjectModel.ArchiveState = e.State;
                    PageCountStatusLabel.Text = e.ArchiveInfo.Pages.Count.ToString() + " Pages";

                    DisableControllsForApplicationState(e.State);
                }));
                //}
            }
            catch (Exception)
            {

            }
        }


        private void DisableControllsForApplicationState(int state)
        {
            switch (state)
            {
                case ApplicationStatusEvent.STATE_READY:
                    PagesList.Enabled = true;
                    PageView.Enabled = true;
                    PageThumbsListBox.Enabled = true;
                    MetaDataGrid.Enabled = true;

                    break;

                case ApplicationStatusEvent.STATE_OPENING:
                    PagesList.Enabled = false;
                    PageView.Enabled = false;
                    PageThumbsListBox.Enabled = false;
                    MetaDataGrid.Enabled = false;

                    break;

                case ApplicationStatusEvent.STATE_SAVING:
                    PagesList.Enabled = false;
                    PageView.Enabled = false;
                    PageThumbsListBox.Enabled = false;
                    MetaDataGrid.Enabled = false;

                    break;

                case ApplicationStatusEvent.STATE_CLOSING:
                    PagesList.Enabled = false;
                    PageView.Enabled = false;
                    PageThumbsListBox.Enabled = false;
                    MetaDataGrid.Enabled = false;

                    break;

                case ApplicationStatusEvent.STATE_ADDING:
                    
                    break;

                case ApplicationStatusEvent.STATE_DELETING:
                    PagesList.Enabled = false;
                    PageView.Enabled = false;
                    PageThumbsListBox.Enabled = false;

                    break;

                case ApplicationStatusEvent.STATE_RENAMING:
                    
                    break;

                case ApplicationStatusEvent.STATE_ANALYZING:
                    PagesList.Enabled = false;
                    PageView.Enabled = false;
                    MetaDataGrid.Enabled = false;
                    PageThumbsListBox.Enabled = false;

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
                    MainToolStripProgressBar.Control.Invoke(new Action(() =>
                    {
                        if (e.State != CBZArchiveStatusEvent.ARCHIVE_FILE_ADDED && e.State != CBZArchiveStatusEvent.ARCHIVE_FILE_RENAMED && e.State != CBZArchiveStatusEvent.ARCHIVE_CLOSING)
                        {
                            MainToolStripProgressBar.Value = 0;
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
                        PagesList.Items.Clear();
                        PageView.Items.Clear();
                        PageThumbsListBox.Items.Clear();
                        PageImages.Images.Clear();
                        filename = "";
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

                case CBZArchiveStatusEvent.ARCHIVE_ERROR_SAVING:
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
                case CBZArchiveStatusEvent.ARCHIVE_FILE_UPDATED:
                    Program.ProjectModel.IsChanged = true;
                    break;
                case CBZArchiveStatusEvent.ARCHIVE_EXTRACTING:
                    info = "Extracting file...";
                    break;
            }

            try
            {
                //if (this.InvokeRequired)
                //{
                this.Invoke(new Action(() =>
                {
                    if (TogglePagePreviewToolbutton.Checked && PageThumbsListBox.Items.Count > 0) // PageView.Items.Count > 0)
                    {
                        //PageView.RedrawItems(0, PageView.Items.Count - 1, false);

                    }
                    

                    FileNameLabel.Text = filename;
                    ApplicationStatusLabel.Text = info;
                    Program.ProjectModel.ArchiveState = e.State;
                    PageCountStatusLabel.Text = e.ArchiveInfo.Pages.Count.ToString() + " Pages";

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
            if (!WindowClosed)
            {
                switch (state)
                {
                    case CBZArchiveStatusEvent.ARCHIVE_SAVING:
                    case CBZArchiveStatusEvent.ARCHIVE_OPENING:
                        NewToolStripMenuItem.Enabled = false;
                        OpenToolStripMenuItem.Enabled = false;
                        SaveAsToolStripMenuItem.Enabled = false;
                        ToolButtonRemoveFiles.Enabled = false;
                        ToolButtonNew.Enabled = false;
                        ToolButtonOpen.Enabled = false;
                        AddFilesToolStripMenuItem.Enabled = false;
                        ToolButtonAddFiles.Enabled = false;
                        BtnAddMetaData.Enabled = false;
                        BtnRemoveMetaData.Enabled = false;
                        ToolButtonExtractArchive.Enabled = false;
                        ToolButtonAddFolder.Enabled = false;
                        ExtractSelectedPages.Enabled = false;
                        BtnRemoveMetaData.Enabled = false;
                        ToolButtonSave.Enabled = false;
                        SaveToolStripMenuItem.Enabled = false;
                        ToolStripButtonShowRawMetadata.Enabled = false;
                        PagesList.Enabled = false;
                        PageView.Enabled = false;
                        PageThumbsListBox.Enabled = false;
                        MetaDataGrid.Enabled = false;
                        AddMetaDataRowBtn.Enabled = false;
                        ToolButtonEditImageProps.Enabled = false;
                        break;

                    case CBZArchiveStatusEvent.ARCHIVE_OPENED:
                        NewToolStripMenuItem.Enabled = true;
                        OpenToolStripMenuItem.Enabled = true;
                        SaveAsToolStripMenuItem.Enabled = true;
                        ToolButtonNew.Enabled = true;
                        ToolButtonOpen.Enabled = true;
                        AddFilesToolStripMenuItem.Enabled = true;
                        ToolButtonAddFiles.Enabled = true;
                        ToolButtonExtractArchive.Enabled = true;
                        ExtractSelectedPages.Enabled = true;
                        ToolButtonAddFolder.Enabled = true;
                        BtnAddMetaData.Enabled = Program.ProjectModel.MetaData.Values.Count == 0;
                        BtnRemoveMetaData.Enabled = Program.ProjectModel.MetaData.Values.Count > 0;
                        AddMetaDataRowBtn.Enabled = Program.ProjectModel.MetaData.Values != null;
                        ToolStripButtonShowRawMetadata.Enabled = true;
                        //TextboxStoryPageRenamingPattern.Enabled = true;
                        //TextboxSpecialPageRenamingPattern.Enabled = true;
                        CheckBoxDoRenamePages.Enabled = true;
                        CheckBoxDoRenamePages.Checked = false;
                        ToolButtonSave.Enabled = false;
                        SaveToolStripMenuItem.Enabled = false;
                        Program.ProjectModel.IsNew = false;
                        PagesList.Enabled = true;
                        PageView.Enabled = true;
                        PageThumbsListBox.Enabled = true;
                        MetaDataGrid.Enabled = true;
                        break;

                    case CBZArchiveStatusEvent.ARCHIVE_SAVED:
                        NewToolStripMenuItem.Enabled = true;
                        OpenToolStripMenuItem.Enabled = true;
                        SaveAsToolStripMenuItem.Enabled = true;
                        ToolButtonNew.Enabled = true;
                        ToolButtonOpen.Enabled = true;
                        AddFilesToolStripMenuItem.Enabled = true;
                        ToolButtonAddFiles.Enabled = true;
                        ToolButtonExtractArchive.Enabled = true;
                        ExtractSelectedPages.Enabled = true;
                        CheckBoxDoRenamePages.Enabled = true;
                        ToolButtonAddFolder.Enabled = true;
                        TextboxStoryPageRenamingPattern.Enabled = true;
                        TextboxSpecialPageRenamingPattern.Enabled = true;
                        ToolButtonSave.Enabled = false;
                        SaveToolStripMenuItem.Enabled = false;
                        ToolStripButtonShowRawMetadata.Enabled = true;
                        ExtractSelectedPages.Enabled = true;
                        Program.ProjectModel.IsNew = false;
                        Program.ProjectModel.IsSaved = true;
                        PagesList.Enabled = true;
                        PageView.Enabled = true;
                        PageThumbsListBox.Enabled = true;
                        MetaDataGrid.Enabled = true;
                        BtnAddMetaData.Enabled = Program.ProjectModel.MetaData.Values.Count == 0;
                        BtnRemoveMetaData.Enabled = Program.ProjectModel.MetaData.Values.Count > 0;
                        AddMetaDataRowBtn.Enabled = Program.ProjectModel.MetaData.Values != null;
                        PageView.Refresh();
                        PageView.Invalidate();
                        break;

                    case CBZArchiveStatusEvent.ARCHIVE_ERROR_SAVING:
                        NewToolStripMenuItem.Enabled = true;
                        OpenToolStripMenuItem.Enabled = true;
                        SaveToolStripMenuItem.Enabled = true;
                        SaveAsToolStripMenuItem.Enabled = true;
                        ToolButtonNew.Enabled = true;
                        ToolButtonOpen.Enabled = true;
                        AddFilesToolStripMenuItem.Enabled = true;
                        ToolButtonAddFiles.Enabled = true;
                        ToolButtonExtractArchive.Enabled = true;
                        ExtractSelectedPages.Enabled = true;
                        CheckBoxDoRenamePages.Enabled = true;
                        ToolButtonAddFolder.Enabled = true;
                        TextboxStoryPageRenamingPattern.Enabled = true;
                        TextboxSpecialPageRenamingPattern.Enabled = true;
                        ToolButtonSave.Enabled = true;
                        ToolStripButtonShowRawMetadata.Enabled = true;
                        ExtractSelectedPages.Enabled = true;
                        Program.ProjectModel.IsSaved = false;
                        PagesList.Enabled = true;
                        PageView.Enabled = true;
                        PageThumbsListBox.Enabled = true;
                        MetaDataGrid.Enabled = true;
                        BtnAddMetaData.Enabled = Program.ProjectModel.MetaData.Values.Count == 0;
                        BtnRemoveMetaData.Enabled = Program.ProjectModel.MetaData.Values.Count > 0;
                        AddMetaDataRowBtn.Enabled = Program.ProjectModel.MetaData.Values != null;
                        PageView.Refresh();
                        PageView.Invalidate();
                        break;

                    case CBZArchiveStatusEvent.ARCHIVE_EXTRACTING:
                        NewToolStripMenuItem.Enabled = false;
                        OpenToolStripMenuItem.Enabled = false;
                        SaveAsToolStripMenuItem.Enabled = false;
                        ToolButtonRemoveFiles.Enabled = false;
                        ToolButtonNew.Enabled = false;
                        ToolButtonOpen.Enabled = false;
                        AddFilesToolStripMenuItem.Enabled = false;
                        ToolButtonAddFiles.Enabled = false;
                        ToolButtonAddFolder.Enabled = false;
                        BtnRemoveMetaData.Enabled = false;
                        ToolButtonExtractArchive.Enabled = false;
                        ExtractSelectedPages.Enabled = false;

                        break;

                    case CBZArchiveStatusEvent.ARCHIVE_EXTRACTED:
                        NewToolStripMenuItem.Enabled = true;
                        OpenToolStripMenuItem.Enabled = true;
                        SaveAsToolStripMenuItem.Enabled = true;
                        ToolButtonNew.Enabled = true;
                        ToolButtonOpen.Enabled = true;
                        AddFilesToolStripMenuItem.Enabled = true;
                        ToolButtonAddFolder.Enabled = true;
                        ToolButtonAddFiles.Enabled = true;
                        ToolButtonExtractArchive.Enabled = true;
                        ExtractSelectedPages.Enabled = true;
                        break;

                    case CBZArchiveStatusEvent.ARCHIVE_CLOSING:
                        NewToolStripMenuItem.Enabled = false;
                        OpenToolStripMenuItem.Enabled = false;
                        SaveAsToolStripMenuItem.Enabled = false;
                        ToolButtonRemoveFiles.Enabled = false;
                        ToolButtonNew.Enabled = false;
                        ToolButtonOpen.Enabled = false;
                        AddFilesToolStripMenuItem.Enabled = false;
                        ToolButtonAddFiles.Enabled = false;
                        BtnAddMetaData.Enabled = false;
                        BtnRemoveMetaData.Enabled = false;
                        ToolButtonAddFolder.Enabled = false;
                        ToolButtonExtractArchive.Enabled = false;
                        ExtractSelectedPages.Enabled = false;
                        ToolButtonSave.Enabled = false;
                        SaveToolStripMenuItem.Enabled = false;
                        ToolStripButtonShowRawMetadata.Enabled = false;
                        PagesList.Enabled = false;
                        PageView.Enabled = false;
                        PageThumbsListBox.Enabled = true;
                        MetaDataGrid.Enabled = false;
                        ToolButtonEditImageProps.Enabled = false;
                        AddMetaDataRowBtn.Enabled = false;
                        RemoveMetaData();
                        break;

                    case CBZArchiveStatusEvent.ARCHIVE_CLOSED:
                        NewToolStripMenuItem.Enabled = true;
                        OpenToolStripMenuItem.Enabled = true;
                        SaveAsToolStripMenuItem.Enabled = false;
                        ToolButtonNew.Enabled = true;
                        ToolButtonOpen.Enabled = true;
                        AddFilesToolStripMenuItem.Enabled = true;
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
                        SaveToolStripMenuItem.Enabled = false;
                        ToolStripButtonShowRawMetadata.Enabled = false;
                        LabelGlobalActionStatusMessage.Text = "";
                        GlobalAlertTableLayout.Visible = false;
                        PagesList.Enabled = true;
                        PageView.Enabled = true;
                        PageThumbsListBox.Enabled = true;
                        MetaDataGrid.Enabled = true;
                        AddMetaDataRowBtn.Enabled = false;
                        ToolButtonEditImageProps.Enabled = false;
                        CurrentGlobalAction = null;
                        LabelW.Text = "0";
                        LabelH.Text = "0";
                        RadioApplyAdjustmentsPage.Text = "(no Page selected)";
                        RadioApplyAdjustmentsPage.Enabled = false;
                        CurrentGlobalActions.Clear();
                        //MessageLogListView.Items.Clear();
                        //MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_INFO, "Archive [" + project.FileName + "] closed");
                        //MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_INFO, "--- **** ---");
                        RemoveMetaData();
                        break;

                    case CBZArchiveStatusEvent.ARCHIVE_FILE_ADDED:
                        CheckBoxDoRenamePages.Enabled = true;
                        //CheckBoxDoRenamePages.Checked = false;
                        if (project.FileName != null)
                        {
                            ToolButtonSave.Enabled = true;
                            SaveToolStripMenuItem.Enabled = true;
                            SaveAsToolStripMenuItem.Enabled = true;
                        }
                        break;

                    case CBZArchiveStatusEvent.ARCHIVE_FILE_DELETED:
                    case CBZArchiveStatusEvent.ARCHIVE_FILE_RENAMED:
                    case CBZArchiveStatusEvent.ARCHIVE_FILE_UPDATED:
                        if (project.FileName != null)
                        {
                            ToolButtonSave.Enabled = true;
                            SaveToolStripMenuItem.Enabled = true;
                            SaveAsToolStripMenuItem.Enabled = true;
                        }
                        break;
                    case CBZArchiveStatusEvent.ARCHIVE_METADATA_ADDED:
                        AddMetaDataRowBtn.Enabled = Program.ProjectModel.MetaData.HasValues();
                        if (project.FileName != null)
                        {
                            ToolButtonSave.Enabled = true;
                            SaveToolStripMenuItem.Enabled = true;
                            SaveAsToolStripMenuItem.Enabled = true;
                        }
                        break;
                    case CBZArchiveStatusEvent.ARCHIVE_METADATA_CHANGED:
                    case CBZArchiveStatusEvent.ARCHIVE_METADATA_DELETED:
                        if (project.FileName != null)
                        {
                            ToolButtonSave.Enabled = true;
                            SaveToolStripMenuItem.Enabled = true;
                            SaveAsToolStripMenuItem.Enabled = true;
                        }
                        break;
                }
            }
        }

        private void ClearProject()
        {
            Task.Factory.StartNew(() =>
            {
                /*
                if (OpeningTask != null)
                {
                    if (OpeningTask.IsAlive)
                    {
                        OpeningTask.Join();
                    }
                }

                if (SavingTask != null)
                {
                    if (SavingTask.IsAlive)
                    {
                        SavingTask.Join();
                    }
                }
                */

                if (ThumbnailThread != null)
                {
                    if (ThumbnailThread.IsAlive)
                    {
                        ThumbnailThread.Join();
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
                        //PagesList.Items.Clear();
                        //PageView.Clear();

                        //PageImages.Images.Clear();

                        MainToolStripProgressBar.Value = 0;

                        //ClearProject();
                        NewProject();
                    }
                }
                else
                {
                    //PagesList.Items.Clear();
                    //PageView.Clear();

                    //PageImages.Images.Clear();

                    MainToolStripProgressBar.Value = 0;

                    //ClearProject();
                    NewProject();
                }          
            } else
            {

                if (ArchiveProcessing())
                {
                    ApplicationMessage.ShowWarning("Please wait until current operation has finished.", "Still operations in progress", ApplicationMessage.DialogType.MT_WARNING, ApplicationMessage.DialogButtons.MB_OK);

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
            ToolButtonImagePreview.Enabled = selectedPages.Count == 1;

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
            if (ArchiveProcessing())
            {
                DialogResult res = ApplicationMessage.ShowWarning("Warning, there are currently still Tasks running in the Background.\nIt is advised to wait until current operation has finished.", "Still operations in progress", ApplicationMessage.DialogType.MT_WARNING, ApplicationMessage.DialogButtons.MB_QUIT | ApplicationMessage.DialogButtons.MB_CANCEL);
                if (res == DialogResult.Yes)
                {
                    if (Program.ProjectModel.IsChanged && !Program.ProjectModel.IsSaved)
                    {
                        res = ApplicationMessage.ShowConfirmation("There are unsaved changes to the current CBZ-Archive.\nAre you sure you want to quit anyway?", "Unsaved changes...");
                        if (res == DialogResult.Yes)
                        {
                            e.Cancel = false || ArchiveProcessing();
                            if (!e.Cancel)
                            {
                                QuitApplication();
                            }
                        }
                        else
                        {
                            e.Cancel = true;
                            return;
                        }
                    }

                    QuitApplication();
                }
                else
                {
                    e.Cancel = true;
                    return;
                }
            }


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
                            QuitApplication();
                        }
                    } else
                    {
                        e.Cancel = true;
                    }
                }
                else
                {
                    QuitApplication();
                }
            } 
            /* else
            {
                DialogResult res = ApplicationMessage.ShowWarning("Warning, there are currently still Tasks running in the Background. It is advised to wait until current operation has finished.", "Still operations in progress", 2, ApplicationMessage.DialogButtons.MB_QUIT | ApplicationMessage.DialogButtons.MB_CANCEL);
                if (res == DialogResult.Yes)
                {
                    WindowClosed = true;
                    Win_CBZSettings.Default.Save();
                    CancelAllThreads();
                    Program.ProjectModel.CancelAllThreads();
                    Application.ExitThread();
                }
                else
                {
                    e.Cancel = true;
                }
            }   */       
        }


        private void CancelAllThreads()
        {
            Task.Factory.StartNew(() =>
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
                        RequestThumbnailThread.Abort();
                    }
                }

                if (RequestImageInfoThread != null)
                {
                    if (RequestImageInfoThread.IsAlive)
                    {
                        RequestImageInfoThread.Abort();
                    }
                }

                if (UpdatePageViewThread != null)
                {
                    if (UpdatePageViewThread.IsAlive)
                    {
                        UpdatePageViewThread.Abort();
                    }
                }
            });
        }

        private void QuitApplication()
        {
            Win_CBZSettings.Default.WindowW = Width;
            Win_CBZSettings.Default.WindowH = Height;
            Win_CBZSettings.Default.Splitter1 = MainSplitBox.SplitterDistance;
            Win_CBZSettings.Default.Splitter2 = SplitBoxPageView.SplitterDistance;
            Win_CBZSettings.Default.Splitter3 = SplitBoxItemsList.SplitterDistance;
            Win_CBZSettings.Default.Splitter4 = PrimarySplitBox.SplitterDistance;
            Win_CBZSettings.Default.IgnoreErrorsOnSave = CheckBoxIgnoreErrorsOnSave.Checked;
            Win_CBZSettings.Default.Save();

            WindowClosed = true;
            CancelAllThreads();
            Program.ProjectModel.CancelAllThreads();
            Application.ExitThread();
        }


        private void AddFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {

            DialogResult openImageResult = OpenImagesDialog.ShowDialog();

            if (openImageResult == DialogResult.OK)
            {
                var maxIndex = PagesList.Items.Count - 1;
                var newIndex = maxIndex < 0 ? 0 : maxIndex;

                Program.ProjectModel.ParseFiles(new List<String>(OpenImagesDialog.FileNames));
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
                        Page oldPage = new Page(((Page)changedItem.Tag));
                        Program.ProjectModel.RenamePage((Page)changedItem.Tag, e.Label);
                        PageChanged(sender, new PageChangedEvent(((Page)changedItem.Tag), oldPage, PageChangedEvent.IMAGE_STATUS_RENAMED));
                        ArchiveStateChanged(sender, new CBZArchiveStatusEvent(Program.ProjectModel, CBZArchiveStatusEvent.ARCHIVE_FILE_RENAMED));
                    }
                    catch (PageDuplicateNameException eduplicate)
                    {                      
                        MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_ERROR, eduplicate.Message);
                        e.CancelEdit = true;
                        ApplicationMessage.ShowException(eduplicate, ApplicationMessage.DialogType.MT_ERROR);
                    }
                    catch (PageException pe)
                    {
                        e.CancelEdit = true;
                        if (pe.ShowErrorDialog)
                        {
                            ApplicationMessage.ShowException(pe, ApplicationMessage.DialogType.MT_ERROR);
                        }
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
                    RequestThumbnailThread.Abort();
                }
            }

            if (RequestImageInfoThread != null)
            {
                if (RequestImageInfoThread.IsAlive)
                {
                    RequestImageInfoThread.Abort();
                }
            }

            if (MovePagesThread != null)
            {
                if (MovePagesThread.IsAlive)
                {
                    MovePagesThread.Abort();
                }
            }

            if (UpdatePageViewThread != null)
            {
                if (UpdatePageViewThread.IsAlive)
                {
                    UpdatePageViewThread.Abort();
                }
            }

            MoveItemsThread = new Thread(MoveItemsToProc);
            MoveItemsThread.Start(new MoveItemsToThreadParams() 
            { 
                newIndex = newIndex, 
                items = items 
            });

        }


        private void MoveItemsToProc(object threadParams)
        {
            MoveItemsToThreadParams tparams = threadParams as MoveItemsToThreadParams;
            
            Page pageOriginal;
            int newIndex = tparams.newIndex;
             
            PagesList.Invoke(new Action(() =>
            {
                List<ListViewItem> ItemsSliced = new List<ListViewItem>();
                List<Page> PageViewItemsSliced = new List<Page>();
                List<Page> PagesSliced = new List<Page>();
                if (tparams.newIndex < 0 || tparams.newIndex > PagesList.Items.Count - 1)
                {
                    return;
                }

                foreach (ListViewItem item in tparams.items)
                {
                    ItemsSliced.Add(item);
                    PagesSliced.Add((Page)item.Tag);
                    PagesList.Items.Remove(item);
                    Program.ProjectModel.Pages.Remove((Page)item.Tag);
                    pageOriginal = FindThumbImageForPage(PageThumbsListBox, (Page)item.Tag);
                    if (pageOriginal != null)
                    {
                        PageViewItemsSliced.Add(pageOriginal);
                        PageThumbsListBox.Items.Remove(pageOriginal);
                        //PageView.Items.Remove(pageOriginal);
                    }                  
                }

                foreach (ListViewItem item in ItemsSliced)
                {
                    PagesList.Items.Insert(newIndex, item);
                    newIndex++;
                }

                newIndex = tparams.newIndex;
                foreach (Page p in PagesSliced)
                {
                    p.Index = newIndex;
                    p.Number = newIndex + 1;
                    Program.ProjectModel.Pages.Insert(newIndex, p);

                    newIndex++;
                }

                newIndex = tparams.newIndex;
                foreach (Page pageItem in PageViewItemsSliced)
                {
                    //pageItem.Text = newIndex.ToString();
                    PageThumbsListBox.Items.Insert(newIndex, pageItem);

                    PageChanged(this, new PageChangedEvent(pageItem, null, PageChangedEvent.IMAGE_STATUS_CHANGED));
                    ArchiveStateChanged(this, new CBZArchiveStatusEvent(Program.ProjectModel, CBZArchiveStatusEvent.ARCHIVE_FILE_UPDATED));

                    HandleGlobalActionRequired(null, new GlobalActionRequiredEvent(Program.ProjectModel, 0, "Page order changed. Rebuild pageindex now?", "Rebuild", GlobalActionRequiredEvent.TASK_TYPE_INDEX_REBUILD, RebuildPageIndexMetaDataTask.UpdatePageIndexMetadata(Program.ProjectModel.Pages, Program.ProjectModel.MetaData, HandleGlobalTaskProgress, PageChanged)));

                    newIndex++;
                }

                //Program.ProjectModel.UpdatePageIndices();
                Program.ProjectModel.IsChanged = true;
            }));
        }

        private void MovePageTo(Page page, int newIndex)
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
                    RequestThumbnailThread.Abort();
                }
            }

            if (RequestImageInfoThread != null)
            {
                if (RequestImageInfoThread.IsAlive)
                {
                    RequestImageInfoThread.Abort();
                }
            }

            if (MovePagesThread != null)
            {
                if (MovePagesThread.IsAlive)
                {
                    MovePagesThread.Join();
                }
            }

            if (UpdatePageViewThread != null)
            {
                if (UpdatePageViewThread.IsAlive)
                {
                    UpdatePageViewThread.Abort();
                }
            }

            MovePagesThread = new Thread(MovePageProc);
            MovePagesThread.Start(new MovePageThreadParams() 
            { 
                newIndex = newIndex, 
                page = page 
            });         
        }

        private void MovePageProc(object movePagesThreadParams)
        {
            MovePageThreadParams tparams = movePagesThreadParams as MovePageThreadParams;
            
            Invoke(new Action(() =>
            {
                Page originalPage;
                ListViewItem originalItem;
                Page updatePage;
                ListViewItem updateItem;
                //Image originalImage;
                //Image updateImage;

                updateItem = FindListViewItemForPage(PagesList, tparams.page);
                updatePage = FindThumbImageForPage(PageThumbsListBox, tparams.page);
                //updateImage = PageImages.Images[PageImages.Images.IndexOfKey(page.Id)];


                originalItem = PagesList.Items[tparams.newIndex];
                //originalPage = FindListViewItemForPage(PageView, (Page)originalItem.Tag);
                originalPage = FindThumbImageForPage(PageThumbsListBox, (Page)originalItem.Tag);
                //originalImage = PageImages.Images[PageImages.Images.IndexOfKey(((Page)originalItem.Tag).Id)];

                int IndexItemToMove = updateItem.Index;

                //if (newIndex < 0 || newIndex > PagesList.Items.Count - 1)
                // {
                //    return;
                //}

                if (tparams.newIndex == IndexItemToMove)
                {
                    //ApplicationMessage.ShowWarning("Unable to move Page! Page already at index " + IndexItemToMove.ToString() + "!", "Unable to move Page", 2, ApplicationMessage.DialogButtons.MB_OK);
                    return;
                }

                PagesList.Items.Remove(originalItem);
                PagesList.Items.Remove(updateItem);
                if (updatePage != null && originalPage != null)
                {
                    //PageView.Items.Remove(originalPage);
                    //PageView.Items.Remove(updatePage);

                }

                Program.ProjectModel.Pages.Remove((Page)originalItem.Tag);
                Program.ProjectModel.Pages.Remove((Page)updateItem.Tag);

                PagesList.Items.Insert(tparams.newIndex, updateItem);
                PagesList.Items.Insert(tparams.newIndex + 1, originalItem);

                Program.ProjectModel.Pages.Insert(tparams.newIndex, (Page)updateItem.Tag);
                Program.ProjectModel.Pages.Insert(tparams.newIndex + 1, (Page)originalItem.Tag);

                if (updatePage != null && originalPage != null)
                {
                    //UpdatePageView();
                    
                    //CreatePagePreviewFromItem(page);
                    //CreatePagePreviewFromItem((Page)originalItem.Tag);
                    //PageView.Items.Clear();

                    //foreach (ListViewItem pageItem in PagesList.Items)
                    //{
                    //CreatePagePreviewFromItem((Page)pageItem.Tag);

                    //MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_DEBUG, i.Text + "_index="+i.Index.ToString()+"_page="+((Page)i.Tag).Name+"_pageId="+ ((Page)i.Tag).Id);
                    //}
                }

                PageChanged(this, new PageChangedEvent(tparams.page, originalPage, PageChangedEvent.IMAGE_STATUS_CHANGED));
                if (originalPage != null)
                {
                    PageChanged(this, new PageChangedEvent(originalPage, null, PageChangedEvent.IMAGE_STATUS_CHANGED));
                }
                ArchiveStateChanged(this, new CBZArchiveStatusEvent(Program.ProjectModel, CBZArchiveStatusEvent.ARCHIVE_FILE_UPDATED));

                HandleGlobalActionRequired(null, new GlobalActionRequiredEvent(Program.ProjectModel, 0, "Page order changed. Rebuild pageindex now?", "Rebuild", GlobalActionRequiredEvent.TASK_TYPE_INDEX_REBUILD, RebuildPageIndexMetaDataTask.UpdatePageIndexMetadata(Program.ProjectModel.Pages, Program.ProjectModel.MetaData, HandleGlobalTaskProgress, PageChanged)));

                Program.ProjectModel.IsChanged = true;
            }));          
        }


        private bool ArchiveProcessing()
        {
            return (Program.ProjectModel.ArchiveState == CBZArchiveStatusEvent.ARCHIVE_SAVING ||
               Program.ProjectModel.ArchiveState == CBZArchiveStatusEvent.ARCHIVE_OPENING ||
               Program.ProjectModel.ArchiveState == CBZArchiveStatusEvent.ARCHIVE_EXTRACTING ||
               Program.ProjectModel.ArchiveState == CBZArchiveStatusEvent.ARCHIVE_CLOSING ||
               Program.ProjectModel.ApplicationState == GeneralTaskProgressEvent.TASK_STATUS_RUNNING ||
               Program.ProjectModel.ThreadRunning()
               );
        }


        private void AddMetaData()
        {
            //if (Program.ProjectModel.MetaData == null)
            //{
                Program.ProjectModel.NewMetaData(true, Win_CBZSettings.Default.MetaDataFilename);
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
            ToolStripButtonShowRawMetadata.Enabled = true;
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
                ToolStripButtonShowRawMetadata.Enabled = false;
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
                if ((Program.ProjectModel.MetaData.HasValues() || Program.ProjectModel.MetaData.HasRemovedValues()) && Program.ProjectModel.Exists()) {
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
                    int index = Program.ProjectModel.MetaData.Remove(row.Index);
                    
                    /*
                    if (row.Cells[0].Value != null)
                    {
                        var key = row.Cells[0].Value.ToString();  

                        Program.ProjectModel.MetaData.Remove(key);
                    } */
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
                DialogResult dlgResult = DialogResult.OK;

                if (ve.ShowErrorDialog)
                {
                    dlgResult = ApplicationMessage.ShowWarning(ve.Message, "Metadata validation", ApplicationMessage.DialogType.MT_WARNING, ApplicationMessage.DialogButtons.MB_OK | ApplicationMessage.DialogButtons.MB_IGNORE );
                }

                if (ve.RemoveEntry && dlgResult != DialogResult.Ignore)
                {
                    e.Cancel = true;
                }
            }
        }

        private void MetaDataGrid_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            DataGridViewCellStyle dataGridViewCellStyle = new DataGridViewCellStyle
            {
                ForeColor = e.CellStyle.ForeColor,
                BackColor = Color.White, //HTMLColor.ToColor(Colors.COLOR_LIGHT_BLUE);
                SelectionForeColor = Color.Black,
                SelectionBackColor = e.CellStyle.SelectionBackColor
            };

            e.CellStyle = dataGridViewCellStyle;            
        }

        private void MetaDataLoaded(object sender, MetaDataLoadEvent e)
        {
            if (!WindowClosed)
            {
                MetaDataGrid.Invoke(new Action(() =>
                {
                    //MetaDataGrid.DataSource = e.MetaData;

                    BtnAddMetaData.Enabled = false;
                    BtnRemoveMetaData.Enabled = true;
                    ToolStripButtonShowRawMetadata.Enabled = true;
                    DataGridViewColumn firstCol = MetaDataGrid.Columns.GetFirstColumn(DataGridViewElementStates.Visible);
                    if (firstCol != null)
                    {
                        DataGridViewColumn secondCol = MetaDataGrid.Columns.GetNextColumn(firstCol, DataGridViewElementStates.Visible, DataGridViewElementStates.None);
                        if (secondCol != null)
                        {
                            firstCol.Width = 150;
                            secondCol.Width = 380;
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
                            SortMode = DataGridViewColumnSortMode.Automatic,
                        });

                        MetaDataGrid.Columns.Add(new DataGridViewColumn()
                        {
                            DataPropertyName = "",
                            HeaderText = "",
                            CellTemplate = new DataGridViewTextBoxCell(),
                            Width = 30,
                            SortMode = DataGridViewColumnSortMode.NotSortable,
                            Resizable = DataGridViewTriState.False,

                        });
                    }

                    MetaDataGrid.Rows.Clear();
                    foreach (MetaDataEntry entry in e.MetaData)
                    {
                        MetaDataGrid.Rows.Add(entry.Key, entry.Value);
                    }

                   // DataGridViewCellStyle currentStyle = null;
                    for (int i = 0; i < MetaDataGrid.RowCount; i++)
                    {
                        
                        //currentStyle = new DataGridViewCellStyle(MetaDataGrid.Rows[i].Cells[2].Style);
                        MetaDataGrid.Rows[i].Cells[2].ReadOnly = true;
                        /*
                        MetaDataGrid.Rows[i].Cells[2].Style = new DataGridViewCellStyle()
                        {
                            SelectionForeColor = Color.Black,
                            SelectionBackColor = ((i+1) % 2 > 0) ? Color.White : Color.FromKnownColor(KnownColor.ControlLight),
                        }; */

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
                                    c.Tag = new EditorTypeConfig("ComboBox", "String", "", " ", false);
                                    
                                    /*
                                    c.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                                    c.DisplayStyleForCurrentCellOnly = true;
                                    c.Style = new DataGridViewCellStyle()
                                    {
                                        SelectionForeColor = Color.Black,
                                        SelectionBackColor = ((i + 1) % 2 > 0) ? Color.White : Color.FromKnownColor(KnownColor.ControlLight),
                                    };
                                    */

                                    MetaDataGrid.Rows[i].Cells[1] = c;
                                    //c.ReadOnly = true;
                                }
                                else if (key.ToString() == "Tags")
                                {
                                    DataGridViewButtonCell bc = new DataGridViewButtonCell
                                    {
                                        Value = "...",
                                        Tag = new EditorTypeConfig("MultiLineTextEditor", "String", ",", " ", false),
                                        Style = new DataGridViewCellStyle()
                                        {
                                            SelectionForeColor = Color.White,
                                            SelectionBackColor = Color.White,
                                        }
                                    };
                                    MetaDataGrid.Rows[i].Cells[2] = bc;
                                }
                                else
                                {

                                }
                            }
                        }
                    }
                }));
            }
        }

        private void MetaDataChanged(object sender, MetaDataChangedEvent e)
        {
            MetaDataGrid.Invoke(new Action(() =>
            {
                Program.ProjectModel.IsChanged = true;

                if (Program.ProjectModel.FileName != null)
                {
                    ToolButtonSave.Enabled = true;
                    SaveToolStripMenuItem.Enabled = true;
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
                    SaveToolStripMenuItem.Enabled = true;
                }

                if (e.State == MetaDataEntryChangedEvent.ENTRY_NEW)
                {

                    foreach (DataGridViewRow r in MetaDataGrid.SelectedRows)
                    {
                        r.Selected = false;
                    }

                    MetaDataGrid.Rows.Add(e.Entry.Key, e.Entry.Value, "");

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

        private void MetaDataGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if ((senderGrid.Rows[e.RowIndex].Cells[e.ColumnIndex] is DataGridViewButtonCell ||
                senderGrid.Rows[e.RowIndex].Cells[e.ColumnIndex] is DataGridViewComboBoxCell
                ) &&
                e.RowIndex >= 0)
            {
                String value = "";
                EditorTypeConfig editorConfig = senderGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag as EditorTypeConfig;
                if (e.ColumnIndex == 2)
                {
                    value = senderGrid.Rows[e.RowIndex].Cells[e.ColumnIndex - 1].Value.ToString();
                }
                else
                {
                    value = senderGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                }

                if (editorConfig != null)
                {
                    editorConfig.Value = value;
                    switch (editorConfig.Type)
                    {
                        case "MultiLineTextEditor":
                            {
                                TextEditorForm textEditor = new TextEditorForm(editorConfig);
                                DialogResult r = textEditor.ShowDialog();
                                if (r == DialogResult.OK)
                                {
                                    if (textEditor.config.Result != null)
                                    {
                                        senderGrid.Rows[e.RowIndex].Cells[e.ColumnIndex - 1].Value = textEditor.config.Result.ToString();
                                    }
                                }
                            }
                            break;
                        case "ComboBox":
                            {
                                DataGridViewComboBoxCell comboCell = senderGrid.Rows[e.RowIndex].Cells[e.ColumnIndex] as DataGridViewComboBoxCell;
                                comboCell.Style = new DataGridViewCellStyle() { 
                                    SelectionForeColor = Color.Black,
                                    SelectionBackColor = Color.White,
                                };
                                MetaDataGrid.BeginEdit(true);

                            }
                            break;


                    }
                }
            }
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
                    SaveToolStripMenuItem.Enabled = true;
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
                            MetaDataGrid.Rows[e.RowIndex].Cells[2].ReadOnly = true;
                            if (updatedEntry.Options.Length > 0)
                            {
                                int selectedIndex = Array.IndexOf(updatedEntry.Options, updatedEntry.Value);
                                DataGridViewComboBoxCell c = new DataGridViewComboBoxCell();
                                c.Items.AddRange(updatedEntry.Options);
                                c.Value = value; //selectedIndex > -1 ? selectedIndex : 0;
                                c.Tag = new EditorTypeConfig("ComboBox", "String", "", "", false);
                                
                                c.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                                //c.DisplayStyleForCurrentCellOnly = true;

                                MetaDataGrid.Rows[e.RowIndex].Cells[1] = c;
                                //c.ReadOnly = true;
                            }
                            else
                            {
                                if (key.ToString() == "Tags")
                                {
                                    DataGridViewButtonCell bc = new DataGridViewButtonCell
                                    {
                                        Value = "...",
                                        Tag = new EditorTypeConfig("MultiLineTextEditor", "String", ",", " ", false)
                                    };
                                    MetaDataGrid.Rows[e.RowIndex].Cells[2] = bc;
                                }

                                DataGridViewTextBoxCell c = new DataGridViewTextBoxCell
                                {
                                    Value = updatedEntry.Value
                                };

                                MetaDataGrid.Rows[e.RowIndex].Cells[1] = c;
                                
                            }
                        }
                    }
                }
                
            }
            catch (Exception ex) 
            {
                MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_ERROR, ex.Message);
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
                ExtractFilesDialog dlg = new ExtractFilesDialog
                {
                    TargetFolder = LastOutputDirectory
                };

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
            System.Windows.Forms.ListView.SelectedListViewItemCollection selectedPages = PagesList.SelectedItems;
            bool buttonState = selectedPages.Count > 0;
            bool propsButtonAvailable = selectedPages.Count == 1;

            ToolButtonRemoveFiles.Enabled = buttonState;
            ToolButtonMovePageDown.Enabled = buttonState && selectedPages.Count != PagesList.Items.Count;
            ToolButtonMovePageUp.Enabled = buttonState && selectedPages.Count != PagesList.Items.Count;
            ToolButtonEditImageProps.Enabled = propsButtonAvailable;
            ToolButtonImagePreview.Enabled = selectedPages.Count == 1;

            ToolButtonSetPageType.Enabled = selectedPages.Count == 1;

            if (buttonState)
            {
                if (!((Page)selectedPages[0].Tag).ImageInfoRequested &&  ((Page)selectedPages[0].Tag).Format.W == 0 && ((Page)selectedPages[0].Tag).Format.H == 0)
                {
                  
                    ImageInfoPagesSlice.Add(((Page)selectedPages[0].Tag));
                }
                //((Page)selectedPages[0].Tag).LoadImageInfo();
                LabelW.Text = ((Page)selectedPages[0].Tag).Format.W.ToString();
                LabelH.Text = ((Page)selectedPages[0].Tag).Format.H.ToString();

                RadioApplyAdjustmentsPage.Text = ((Page)selectedPages[0].Tag).Name;
                RadioApplyAdjustmentsPage.Enabled = true;
                //RequestImageInfoSlice();
            } else
            {
                RadioApplyAdjustmentsPage.Text = "(no page selected)";
                RadioApplyAdjustmentsPage.Enabled = false;
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

                    PageChanged(this, new PageChangedEvent((Page)img.Tag, null, PageChangedEvent.IMAGE_STATUS_DELETED));
                    ArchiveStateChanged(this, new CBZArchiveStatusEvent(Program.ProjectModel, CBZArchiveStatusEvent.ARCHIVE_FILE_DELETED));
                }

                HandleGlobalActionRequired(null, new GlobalActionRequiredEvent(Program.ProjectModel, 0, "Page order changed. Rebuild pageindex now?", "Rebuild", GlobalActionRequiredEvent.TASK_TYPE_INDEX_REBUILD, RebuildPageIndexMetaDataTask.UpdatePageIndexMetadata(Program.ProjectModel.Pages, Program.ProjectModel.MetaData, HandleGlobalTaskProgress, PageChanged)));

                //Program.ProjectModel.UpdatePageIndices();
            }
        }      

        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
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

                    HandleGlobalActionRequired(null, new GlobalActionRequiredEvent(Program.ProjectModel, 0, "Page type changed. Rebuild pageindex now?", "Rebuild", GlobalActionRequiredEvent.TASK_TYPE_INDEX_REBUILD, RebuildPageIndexMetaDataTask.UpdatePageIndexMetadata(Program.ProjectModel.Pages, Program.ProjectModel.MetaData, HandleGlobalTaskProgress, PageChanged)));

                    PageChanged(null, new PageChangedEvent((Page)item.Tag, null, PageChangedEvent.IMAGE_STATUS_CHANGED));
                    ArchiveStateChanged(null, new CBZArchiveStatusEvent(Program.ProjectModel, CBZArchiveStatusEvent.ARCHIVE_METADATA_CHANGED));

                }
                else
                {
                    if (((ToolStripSplitButton)sender).HasDropDown && !((ToolStripSplitButton)sender).DropDown.Visible)
                    {
                        ((Page)item.Tag).ImageType = (String)((ToolStripSplitButton)sender).Tag;
                    }
                }

                if (item.SubItems.Count > 0)
                {
                    item.SubItems[2].Text = ((Page)item.Tag).ImageType;
                }
            }
        }

        private void ToolButtonEditImageProps_Click(object sender, EventArgs e)
        {
            if (PagesList.SelectedItem != null)
            {
                Page page = (Page)PagesList.SelectedItem.Tag;
                Page editPage = new Page(page);

                PageSettingsForm pageSettingsForm = new PageSettingsForm(editPage);
                DialogResult dlgResult = pageSettingsForm.ShowDialog();

                if (dlgResult == DialogResult.OK)
                {
                    Page pageResult = pageSettingsForm.GetResult();
                    Page pageToUpdate = Program.ProjectModel.GetPageById(pageResult.Id);
                    if (pageToUpdate != null)
                    {
                        try
                        {
                            Program.ProjectModel.MetaData.UpdatePageIndexMetaDataEntry(pageResult, page.Key);
                        }
                        catch (MetaDataPageEntryException em)
                        {
                            if (em.ShowErrorDialog)
                            {
                                ApplicationMessage.ShowWarning(em.Message, em.GetType().Name, ApplicationMessage.DialogType.MT_WARNING, ApplicationMessage.DialogButtons.MB_OK);
                            }
                        }
                        
                        pageToUpdate.UpdatePage(pageResult, false, true);  // dont update name without rename checks!
                        if (!pageResult.Deleted)
                        {
                            if (editPage.Name != pageResult.Name)
                            {
                                try
                                {
                                    Program.ProjectModel.RenamePage(pageToUpdate, pageResult.Name);

                                    HandleGlobalActionRequired(null, new GlobalActionRequiredEvent(Program.ProjectModel, 0, "Page name changed. Rebuild pageindex now?", "Rebuild", GlobalActionRequiredEvent.TASK_TYPE_INDEX_REBUILD, RebuildPageIndexMetaDataTask.UpdatePageIndexMetadata(Program.ProjectModel.Pages, Program.ProjectModel.MetaData, HandleGlobalTaskProgress, PageChanged)));

                                    PageChanged(null, new PageChangedEvent(pageResult, editPage, PageChangedEvent.IMAGE_STATUS_RENAMED));
                                    ArchiveStateChanged(null, new CBZArchiveStatusEvent(Program.ProjectModel, CBZArchiveStatusEvent.ARCHIVE_FILE_UPDATED));


                                } catch (Exception ex3)
                                {
                                    MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_ERROR, ex3.Message);
                                }
                            }

                            try
                            {
                                MovePageTo(pageToUpdate, pageResult.Index);
                            }
                            catch (Exception ex)
                            {
                                RestoreIndex(pageToUpdate, editPage);
                                RestoreIndex(page, editPage);

                                ApplicationMessage.ShowException(ex);
                            }

                        }

                        if (pageResult.Deleted != editPage.Deleted)
                        {
                            HandleGlobalActionRequired(null, new GlobalActionRequiredEvent(Program.ProjectModel, 0, "Page order changed. Rebuild pageindex now?", "Rebuild", GlobalActionRequiredEvent.TASK_TYPE_INDEX_REBUILD, RebuildPageIndexMetaDataTask.UpdatePageIndexMetadata(Program.ProjectModel.Pages, Program.ProjectModel.MetaData, HandleGlobalTaskProgress, PageChanged)));
                        }

                        if (pageToUpdate.Deleted)
                        {
                            try
                            {
                                pageToUpdate.DeleteTemporaryFile();
                            }
                            catch (Exception ex)
                            {
                                MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_ERROR, ex.Message);
                            }

                            HandleGlobalActionRequired(null, new GlobalActionRequiredEvent(Program.ProjectModel, 0, "Page order changed. Rebuild pageindex now?", "Rebuild", GlobalActionRequiredEvent.TASK_TYPE_INDEX_REBUILD, RebuildPageIndexMetaDataTask.UpdatePageIndexMetadata(Program.ProjectModel.Pages, Program.ProjectModel.MetaData, HandleGlobalTaskProgress, PageChanged)));
                        }

                        PageChanged(this, new PageChangedEvent(pageToUpdate, editPage, PageChangedEvent.IMAGE_STATUS_CHANGED));
                    }
                }

                pageSettingsForm.FreeResult();
                pageSettingsForm.Dispose();
            }
        }

        private void RestoreIndex(Page updatedPage, Page originalPage)
        {
            updatedPage.Index = originalPage.Index;
            updatedPage.Number = originalPage.Index + 1;
        }

        private void SelectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in PagesList.Items)
            {
                item.Selected = true;
            }
        }

        private void CheckBoxDoRenamePages_CheckedChanged(object sender, EventArgs e)
        {
            if (sender != null)
            {
                if (Win_CBZSettings.Default.CompatMode)
                {
                    if (CheckBoxDoRenamePages.CheckState == CheckState.Checked)
                    {
                        DialogResult res = ApplicationMessage.Show("Remaming is not supported in compatibility mode!\r\nPages are renamed automatically according to their respective indices.", "Not supported", ApplicationMessage.DialogType.MT_INFORMATION, ApplicationMessage.DialogButtons.MB_OK);
                        Program.ProjectModel.ApplyRenaming = false;
                        CheckBoxDoRenamePages.CheckState = CheckState.Unchecked;
                    }
                }
                else
                {
                    Program.ProjectModel.ApplyRenaming = CheckBoxDoRenamePages.CheckState == CheckState.Checked;
                    Program.ProjectModel.IsChanged = true;
                    TextboxStoryPageRenamingPattern.Enabled = CheckBoxDoRenamePages.CheckState == CheckState.Checked;
                    TextboxSpecialPageRenamingPattern.Enabled = CheckBoxDoRenamePages.CheckState == CheckState.Checked;
                    ToolButtonSave.Enabled = true;
                    SaveToolStripMenuItem.Enabled = true;
                    CheckBoxPreview.Enabled = CheckBoxDoRenamePages.CheckState == CheckState.Checked;

                    if (CheckBoxDoRenamePages.CheckState == CheckState.Unchecked)
                    {
                        CheckBoxPreview.Enabled = false;
                        if (CheckBoxPreview.Checked)
                        {
                            try
                            {
                                Program.ProjectModel.RestoreOriginalNames();
                            }
                            catch (ConcurrentOperationException c)
                            { 
                                if (c.ShowErrorDialog)
                                {

                                }
                            }
                        }
                    }
                    else
                    {
                        if (CheckBoxPreview.Checked == true)
                        {
                            try
                            {
                                Program.ProjectModel.AutoRenameAllPages();
                            }
                            catch (ConcurrentOperationException c)
                            {
                                if (c.ShowErrorDialog)
                                {
                                    ApplicationMessage.ShowWarning(c.Message, "ConcurrentOperationException", ApplicationMessage.DialogType.MT_WARNING, ApplicationMessage.DialogButtons.MB_OK);
                                }
                            }
                        }
                    }
                }
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
            DialogResult result = ApplicationMessage.ShowConfirmation("This will clear the application cache folder. Continue?", "Confirm Delete", ApplicationMessage.DialogType.MT_CONFIRMATION, ApplicationMessage.DialogButtons.MB_YES | ApplicationMessage.DialogButtons.MB_NO);
        
            if (result == DialogResult.Yes)
            {
                Program.ProjectModel.DeleteTempFolderItems();
            }
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
            if (!Program.ProjectModel.MetaData.Exists())
            {
                DialogResult res = ApplicationMessage.ShowConfirmation("Currently no metadata available!\r\nCBZ needs to contain XML metadata (comicinfo.xml) in order to define individual pagetypes. Add a new set of Metadata now?", "Metadata required", ApplicationMessage.DialogType.MT_CONFIRMATION, ApplicationMessage.DialogButtons.MB_YES | ApplicationMessage.DialogButtons.MB_NO);
                if (res == DialogResult.Yes)
                {
                    BtnAddMetaData_Click(sender, null);
                }
                else
                {
                    return;
                }
            }

            foreach (ListViewItem item in PagesList.SelectedItems)
            {
                ((Page)item.Tag).ImageType = (String)((ToolStripSplitButton)sender).Tag;

                if (item.SubItems.Count > 0)
                {
                    item.SubItems[2].Text = (String)((ToolStripSplitButton)sender).Tag;
                }

                HandleGlobalActionRequired(null, new GlobalActionRequiredEvent(Program.ProjectModel, 0, "Page type changed. Rebuild pageindex now?", "Rebuild", GlobalActionRequiredEvent.TASK_TYPE_INDEX_REBUILD, RebuildPageIndexMetaDataTask.UpdatePageIndexMetadata(Program.ProjectModel.Pages, Program.ProjectModel.MetaData, HandleGlobalTaskProgress, PageChanged)));

                PageChanged(null, new PageChangedEvent((Page)item.Tag, null, PageChangedEvent.IMAGE_STATUS_CHANGED));
                ArchiveStateChanged(null, new CBZArchiveStatusEvent(Program.ProjectModel, CBZArchiveStatusEvent.ARCHIVE_METADATA_CHANGED));
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

                if (settingsDialog.NewValidTagList != null)
                {
                    foreach (String line in settingsDialog.NewValidTagList)
                    {
                        if (line != null && line != "")
                        {
                            Win_CBZSettings.Default.ValidKnownTags.Add(line);
                        }
                    }
                }

                Win_CBZSettings.Default.ValidateTags = settingsDialog.ValidateTagsSetting;
                Win_CBZSettings.Default.TagValidationIgnoreCase = settingsDialog.TagValidationIgnoreCase;
                Win_CBZSettings.Default.ImageConversionMode = settingsDialog.ConversionModeValue;
                Win_CBZSettings.Default.ImageConversionQuality = settingsDialog.ConversionQualityValue;
                Win_CBZSettings.Default.MetaDataFilename = settingsDialog.MetaDataFilename;

                TextBoxMetaDataFilename.Text = settingsDialog.MetaDataFilename;
                Program.ProjectModel.MetaData.MetaDataFileName = settingsDialog.MetaDataFilename;
            } else
            {
               //
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
            Pen captionPen = new Pen(Color.Black, 1);
            Brush textBrush = Brushes.Black;
            Brush textBGBrush = Brushes.White;
            Font textFont = SystemFonts.CaptionFont;
            if (e.Item.Selected)
            {
                borderPen = new Pen(Color.DodgerBlue, 2);
            }
            else
            {
                borderPen = new Pen(Color.LightGray, 2);
            }

            if (owner.LargeImageList != null)
            {
                int center = ((e.Bounds.Width + 4) / 2) - ((owner.LargeImageList.ImageSize.Width + 4) / 2);

                Rectangle rectangle = new Rectangle(center, e.Bounds.Y + 2, owner.LargeImageList.ImageSize.Width + 4, owner.LargeImageList.ImageSize.Height + 4);


                int customItemBoundsW = owner.LargeImageList.ImageSize.Width;
                int customItemBoundsH = 16; //owner.LargeImageList.ImageSize.Height;

                Rectangle textBox = new Rectangle(center, rectangle.Height - 16, customItemBoundsW, customItemBoundsH);

                // e.Bounds.Width = customItemBoundsW + 4;


                if (page != null)
                {
                    e.Graphics.DrawRectangle(borderPen, rectangle);
                    if (owner.LargeImageList.Images.IndexOfKey(page.Id) > -1)
                    {
                        e.Graphics.DrawImage(owner.LargeImageList.Images[owner.LargeImageList.Images.IndexOfKey(page.Id)], new Point(center + 2, e.Bounds.Y + 4));
                    }
                    else
                    {
                        e.Graphics.DrawImage(global::Win_CBZ.Properties.Resources.placeholder_image, new Point(center + 2, e.Bounds.Y + 4));

                        if (!page.Closed)
                        {
                            ThumbnailPagesSlice.Add(page);
                            if (RequestThumbnailThread != null)
                            {
                                if (!RequestThumbnailThread.IsAlive)
                                {
                                    
                                    RequestThumbnailSlice();
                                }
                            }
                            else
                            {
                                RequestThumbnailSlice();
                            }
                        }
                    }

                    //e.Graphics.DrawRectangle(captionPen, textBox);
                    //e.Graphics.FillRectangle(textBGBrush, textBox);
                    //e.Graphics.DrawString(page.Name + "-> " + item.Index.ToString(), textFont, textBrush, new Point(center + 20, e.Bounds.Y + rectangle.Height - 14));
                }
                else
                {

                }
            }           
        }

        private void CheckBoxPreview_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBoxPreview.Checked)
            {
                try
                {
                    Program.ProjectModel.AutoRenameAllPages();
                }
                catch (ConcurrentOperationException c)
                {
                    if (c.ShowErrorDialog)
                    {
                        ApplicationMessage.ShowWarning(c.Message, "ConcurrentOperationException", ApplicationMessage.DialogType.MT_WARNING, ApplicationMessage.DialogButtons.MB_OK);
                    }
                }
                
            } else
            {
                try
                {
                    Program.ProjectModel.RestoreOriginalNames();
                }
                catch (ConcurrentOperationException c)
                {
                    if (c.ShowErrorDialog)
                    {
                        ApplicationMessage.ShowWarning(c.Message, "ConcurrentOperationException", ApplicationMessage.DialogType.MT_WARNING, ApplicationMessage.DialogButtons.MB_OK);
                    }
                }
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

        private void ToolStripButtonShowRawMetadata_Click(object sender, EventArgs e)
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

        private void BtnGetExcludesFromSelectedPages_Click(object sender, EventArgs e)
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

        private void PagesList_DoubleClick(object sender, EventArgs e)
        {
            ToolButtonEditImageProps_Click(this, e);
        }

        private void PageView_DoubleClick(object sender, EventArgs e)
        {
            if (PageView.SelectedItem != null)
            {
                Page page = (Page)PageView.SelectedItem.Tag;

                ImagePreviewForm pagePreviewForm = new ImagePreviewForm(page);
                DialogResult dlgResult = pagePreviewForm.ShowDialog();
                pagePreviewForm.Dispose();
            }
        }

        private void ComboBoxCompressionLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = ComboBoxCompressionLevel.SelectedIndex;

            switch (index)
            {
                case 0:
                    Program.ProjectModel.CompressionLevel = CompressionLevel.Optimal;
                    break;
                case 1:
                    Program.ProjectModel.CompressionLevel = CompressionLevel.Fastest;
                    break;
                case 2:
                    Program.ProjectModel.CompressionLevel = CompressionLevel.NoCompression;
                    break;

            }
        }

        private void ToolButtonImagePreview_Click(object sender, EventArgs e)
        {
            if (PagesList.SelectedItem != null)
            {
                Page page = (Page)PagesList.SelectedItem.Tag;

                ImagePreviewForm pagePreviewForm = new ImagePreviewForm(page);
                DialogResult dlgResult = pagePreviewForm.ShowDialog();
                pagePreviewForm.Dispose();
            }
        }

        private void ToolButtonValidateCBZ_Click(object sender, EventArgs e)
        {
            try
            { 
                Program.ProjectModel.Validate(true);
            } catch (ConcurrentOperationException c) 
            {
                if (c.ShowErrorDialog)
                {
                    ApplicationMessage.ShowWarning(c.Message, "Concurrency Exception", ApplicationMessage.DialogType.MT_WARNING, ApplicationMessage.DialogButtons.MB_OK);
                }
            }
        }

        private void ComboBoxApplyPageAdjustmentsTo_SelectionChangeCommitted(object sender, EventArgs e)
        {
            System.Windows.Forms.ComboBox cb = (System.Windows.Forms.ComboBox)sender;
            String selected = cb.SelectedItem as String;
            ImageTask selectedTask = null;
            Page page;

            if (selected != null)
            {
                if (selected == "<Global>")
                {
                    selectedTask = Program.ProjectModel.GlobalImageTask;
                    
                } else
                {
                    page = Program.ProjectModel.GetPageByName(selected);

                    if (page != null)
                    {
                        selectedTask = page.ImageTask;
                    }
                }

                if (selectedTask != null)
                {
                    //ImageQualityTrackBar.Value = selectedTask.ImageAdjustments.Quality;
                    switch (selectedTask.ImageAdjustments.ResizeMode)
                    {
                        case 0:
                            RadioButtonResizeNever.Checked = true;
                            break;
                        case 1:
                            RadioButtonResizeIfLarger.Checked = true;
                            break;
                        case 2:
                            RadioButtonResizeTo.Checked = true;
                            break;

                    }

                    CheckBoxSplitDoublePages.Checked = selectedTask.ImageAdjustments.SplitPage;
                    TextBoxSplitPageAt.Text = selectedTask.ImageAdjustments.SplitPageAt.ToString();
                    ComboBoxSplitAtType.SelectedIndex = selectedTask.ImageAdjustments.SplitType;

                    selectedImageTask = selectedTask;
                }
            }
        }

        private void ImageQualityTrackBar_ValueChanged(object sender, EventArgs e)
        {
            if (selectedImageTask != null)
            {
                //selectedImageTask.ImageAdjustments.Quality = ImageQualityTrackBar.Value;
            }
            
        }

        private void ImageResizeRadioChanged(object sender, EventArgs e) 
        { 
            RadioButton radio = (RadioButton)sender;

            if (selectedImageTask != null)
            {
                switch (radio.Name)
                {
                    case "RadioButtonResizeNever":
                        selectedImageTask.ImageAdjustments.ResizeMode = 0;
                        break;
                    case "RadioButtonResizeIfLarger":
                        selectedImageTask.ImageAdjustments.ResizeMode = 1;
                        break;
                    case "RadioButtonResizeTo":
                        selectedImageTask.ImageAdjustments.ResizeMode = 2;
                        break;
                }
            }
        }

        private void CheckBoxCompatibilityMode_CheckedChanged(object sender, EventArgs e)
        {
            Win_CBZSettings.Default.CompatMode = CheckBoxCompatibilityMode.Checked;

            Program.ProjectModel.CompatibilityMode = CheckBoxCompatibilityMode.Checked;

            if (CheckBoxDoRenamePages.CheckState == CheckState.Checked)
            {
                CheckBoxDoRenamePages.CheckState = CheckState.Unchecked;
                Program.ProjectModel.ApplyRenaming = CheckBoxDoRenamePages.CheckState == CheckState.Checked;
                Program.ProjectModel.IsChanged = true;
                TextboxStoryPageRenamingPattern.Enabled = CheckBoxDoRenamePages.CheckState == CheckState.Checked;
                TextboxSpecialPageRenamingPattern.Enabled = CheckBoxDoRenamePages.CheckState == CheckState.Checked;
                ToolButtonSave.Enabled = true;
                CheckBoxPreview.Enabled = false;
                SaveToolStripMenuItem.Enabled = true;

                try
                {
                    Program.ProjectModel.RestoreOriginalNames();
                }
                catch (ConcurrentOperationException c)
                {
                    if (c.ShowErrorDialog)
                    {
                        ApplicationMessage.ShowWarning(c.Message, "ConcurrentOperationException", ApplicationMessage.DialogType.MT_WARNING, ApplicationMessage.DialogButtons.MB_OK);
                    }
                }
            }
        }

        private void RadioApplyAdjustments_CheckedChanged(object sender, EventArgs e)
        {
            System.Windows.Forms.RadioButton rb = (System.Windows.Forms.RadioButton)sender;
            String selected = rb.Tag as String;
            ImageTask selectedTask = null;
            Page page = null;

            if (selected != null)
            {
                if (selected == "<Global>")
                {
                    selectedTask = Program.ProjectModel.GlobalImageTask;

                }
                else
                {
                    page = Program.ProjectModel.GetPageById(selected);

                    if (page != null)
                    {
                        selectedTask = page.ImageTask;
                    }
                }

                if (selectedTask != null)
                {
                    //ImageQualityTrackBar.Value = selectedTask.ImageAdjustments.Quality;
                    switch (selectedTask.ImageAdjustments.ResizeMode)
                    {
                        case 0:
                            RadioButtonResizeNever.Checked = true;
                            break;
                        case 1:
                            RadioButtonResizeIfLarger.Checked = true;
                            break;
                        case 2:
                            RadioButtonResizeTo.Checked = true;
                            break;

                    }

                    CheckBoxSplitDoublePages.Checked = selectedTask.ImageAdjustments.SplitPage;
                    TextBoxSplitPageAt.Text = selectedTask.ImageAdjustments.SplitPageAt.ToString();
                    ComboBoxSplitAtType.SelectedIndex = selectedTask.ImageAdjustments.SplitType;

                    selectedImageTask = selectedTask;
                }
            }
        }

        private void DebugToolsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            df.Show();
        }

        private void ComboBoxConvertPages_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void PageView_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void CheckBoxIgnoreErrorsOnSave_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void ToolButtonAddFolder_Click(object sender, EventArgs e)
        {
            AddFolderToolStripMenuItem_Click(sender, e);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PagesList_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(ListViewItem)))
            { 
                var movedItem = (ListViewItem)e.Data.GetData(typeof(ListViewItem));

                //ListViewItem targetItem = PagesList.GetItemAt(e.X, e.Y);

                if (movedItem != null)
                {
                    MoveItemsTo(movedItem.Index, PagesList.SelectedItems);
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
                //if (!items.Contains(lvi))
                //{
                    items.Add(lvi);
                //}
            }
            DataObject data = new DataObject();
            data.SetData(typeof(System.Windows.Forms.ListView.SelectedListViewItemCollection), PagesList.SelectedItems);
            // pass the items to move...
            PagesList.DoDragDrop(data, DragDropEffects.Move);            
        }

        private void PageThumbsListBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            Page page = null;
            System.Windows.Forms.ListBox owner = sender as System.Windows.Forms.ListBox;
            if (e.Index > -1)
            {
                page = owner.Items[e.Index] as Page;
            }
       
            Pen borderPen;
            Pen captionPen = new Pen(Color.Black, 1);
            Brush textBrush = Brushes.Black;
            Brush textBGBrush = Brushes.White;
            Font textFont = SystemFonts.CaptionFont;
            if (e.State.HasFlag(DrawItemState.Selected))
            {
                borderPen = new Pen(Color.DodgerBlue, 2);
            }
            else
            {
                borderPen = new Pen(Color.LightGray, 2);
            }

            if (PageImages != null)
            {
                int center = ((e.Bounds.Width + 4) / 2) - ((PageImages.ImageSize.Width + 4) / 2);

                Rectangle rectangle = new Rectangle(center, e.Bounds.Y + 2, PageImages.ImageSize.Width + 4, PageImages.ImageSize.Height + 4);


                int customItemBoundsW = PageImages.ImageSize.Width;
                int customItemBoundsH = 16; //owner.LargeImageList.ImageSize.Height;

                Rectangle textBox = new Rectangle(center, rectangle.Height - 16, customItemBoundsW, customItemBoundsH);

                // e.Bounds.Width = customItemBoundsW + 4;


                if (page != null)
                {
                    e.Graphics.DrawRectangle(borderPen, rectangle);
                    if (PageImages.Images.IndexOfKey(page.Id) > -1)
                    {
                        if (page.Deleted)
                        {
                            e.Graphics.DrawImage(global::Win_CBZ.Properties.Resources.placeholder_image, new Point(center + 2, e.Bounds.Y + 4));
                        }
                        else
                        {
                            e.Graphics.DrawImage(PageImages.Images[PageImages.Images.IndexOfKey(page.Id)], new Point(center + 2, e.Bounds.Y + 4));
                        }
                    }
                    else
                    {
                        e.Graphics.DrawImage(global::Win_CBZ.Properties.Resources.placeholder_image, new Point(center + 2, e.Bounds.Y + 4));

                        if (!page.Closed)
                        {
                            ThumbnailPagesSlice.Add(page);
                            if (RequestThumbnailThread != null)
                            {
                                if (!RequestThumbnailThread.IsAlive)
                                {

                                    RequestThumbnailSlice();
                                }
                            }
                            else
                            {
                                RequestThumbnailSlice();
                            }
                        }
                    }

                    //e.Graphics.DrawRectangle(captionPen, textBox);
                    //e.Graphics.FillRectangle(textBGBrush, textBox);
                    //e.Graphics.DrawString(page.Name + "-> " + item.Index.ToString(), textFont, textBrush, new Point(center + 20, e.Bounds.Y + rectangle.Height - 14));
                }
                else
                {

                }
            }
        }

        private void PageThumbsListBox_DoubleClick(object sender, EventArgs e)
        {
            if (PageThumbsListBox.SelectedIndex > -1)
            {
                Page page = PageThumbsListBox.Items[PageThumbsListBox.SelectedIndex] as Page;

                ImagePreviewForm pagePreviewForm = new ImagePreviewForm(page);
                DialogResult dlgResult = pagePreviewForm.ShowDialog();
                pagePreviewForm.Dispose();
            }
        }

        private void PageThumbsListBox_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            e.ItemWidth = 244;
            e.ItemWidth = 212;
        }

        private void PageThumbsListBox_SelectedValueChanged(object sender, EventArgs e)
        {
            //System.Windows.Forms.ListView.SelectedListViewItemCollection selectedPages = this.PageView.SelectedItems;
            System.Windows.Forms.ListBox.SelectedObjectCollection selectedPages = PageThumbsListBox.SelectedItems;
            bool buttonState = selectedPages.Count > 0;

            ToolButtonRemoveFiles.Enabled = buttonState;
            ToolButtonMovePageDown.Enabled = buttonState;
            ToolButtonMovePageUp.Enabled = buttonState;

            ToolButtonSetPageType.Enabled = selectedPages.Count == 1;
            ToolButtonImagePreview.Enabled = selectedPages.Count == 1;

            foreach (ListViewItem itempage in PagesList.Items)
            {
                itempage.Selected = false;
            }

            foreach (Page item in selectedPages)
            {
                if (item.Index > -1)
                {
                    PagesList.Items[item.Index].Selected = true;
                }              
            }
        }
    }
}
