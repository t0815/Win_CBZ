using AutocompleteMenuNS;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.Devices;
using SharpCompress;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Versioning;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Shapes;
using System.Xml;
using Win_CBZ.Base;
using Win_CBZ.Data;
using Win_CBZ.Events;
using Win_CBZ.Exceptions;
using Win_CBZ.Extensions;
using Win_CBZ.Forms;
using Win_CBZ.Handler;
using Win_CBZ.Helper;
using Win_CBZ.List;
using Win_CBZ.Models;
using Win_CBZ.Properties;
using Win_CBZ.Result;
using Win_CBZ.Tasks;
using static Win_CBZ.MetaData;
using Cursors = System.Windows.Forms.Cursors;
using Path = System.IO.Path;
using Rectangle = System.Drawing.Rectangle;
using TextBox = System.Windows.Forms.TextBox;

namespace Win_CBZ
{
    [SupportedOSPlatform("windows")]
    public partial class MainForm : Form
    {

        private PageClipboardMonitor pageClipboardMonitor;

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

        private bool ApplyUserKeyFilter = false;

        private bool UseOffset = false;

        private int LastOffset = 0;

        private DebugForm df;

        public MainForm()
        {
            InitializeComponent();

            PagesList.Invalidated += ListView_Invalidated;
            ImageTaskListView.Invalidated += ListView_Invalidated;

            try
            {
                Program.ProjectModel = NewProjectModel();
            }
            catch (MetaDataValidationException ve)
            {
                MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_WARNING, "Failed to initialize ProjectModel.\nFailed to parse default metadata entry ['" + ve.Item.Key + "']!  [" + ve.Message + "]");

                if (ve.ShowErrorDialog)
                {
                    ApplicationMessage.ShowWarning("Failed to initialize ProjectModel.\n" + ve.Message, "Initialization Error", ApplicationMessage.DialogType.MT_WARNING, ApplicationMessage.DialogButtons.MB_OK);
                }
            }
            catch (Exception e)
            {
                MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_WARNING, "Failed to parse default metadata entry! [" + e.Message + "]");
            }


            AppEventHandler.MessageLogged += MessageLogged;

            ThumbnailPagesSlice = new List<Page>();
            ImageInfoPagesSlice = new List<Page>();
            CurrentGlobalActions = new List<GlobalAction>();

            string version = Assembly.GetExecutingAssembly().GetName().Version.ToString();

            // restore old settings
            if (Win_CBZSettings.Default.MigrationRequired)
            {
                Win_CBZSettings.Default.Upgrade();
                Win_CBZSettings.Default.MigrationRequired = false;
                Win_CBZSettings.Default.Save();
            }

            Win_CBZSettings.Default.Version = version;

            Text = Assembly.GetExecutingAssembly().GetName().Name + " Trash_s0Ft";

            Program.DebugMode = Win_CBZSettings.Default.DebugMode == "3ab980acc9ab16b";

            if (Program.DebugMode)
            {
                Text += " [DEBUG MODE]";
            }

            if (Win_CBZSettings.Default.AutoUpdateInterval == 0)
            {
                Win_CBZSettings.Default.AutoUpdateInterval = UpdateCheckHelper.UPDATE_CHECK_INTERVAL_DAILY;
            }

            if (Win_CBZSettings.Default.RestoreWindowLayout)
            {
                if (WindowState == FormWindowState.Normal)
                {

                    if (Win_CBZSettings.Default.WindowW > 0 && Win_CBZSettings.Default.WindowH > 0)
                    {
                        Width = Win_CBZSettings.Default.WindowW;
                        Height = Win_CBZSettings.Default.WindowH;
                    }

                    try
                    {
                        MainSplitBox.SplitterDistance = Win_CBZSettings.Default.Splitter1;
                    }
                    catch (Exception e)
                    {
                        ApplicationMessage.ShowWarning("Failed to load setting for 'Splitter1'.\n" + e.Message, "Initialization Error", ApplicationMessage.DialogType.MT_WARNING, ApplicationMessage.DialogButtons.MB_OK);
                    }

                    try
                    {
                        PrimarySplitBox.SplitterDistance = Win_CBZSettings.Default.Splitter4;
                    }
                    catch (Exception e)
                    {
                        ApplicationMessage.ShowWarning("Failed to load setting for 'Splitter4'.\n" + e.Message, "Initialization Error", ApplicationMessage.DialogType.MT_WARNING, ApplicationMessage.DialogButtons.MB_OK);
                    }

                    try
                    {
                        SplitBoxPageView.SplitterDistance = Win_CBZSettings.Default.Splitter2;
                    }
                    catch (Exception e)
                    {
                        ApplicationMessage.ShowWarning("Failed to load setting for 'Splitter2'.\n" + e.Message, "Initialization Error", ApplicationMessage.DialogType.MT_WARNING, ApplicationMessage.DialogButtons.MB_OK);
                    }

                    try
                    {
                        SplitBoxItemsList.SplitterDistance = Win_CBZSettings.Default.Splitter3;
                    }
                    catch (Exception e)
                    {
                        ApplicationMessage.ShowWarning("Failed to load setting for 'Splitter3'.\n" + e.Message, "Initialization Error", ApplicationMessage.DialogType.MT_WARNING, ApplicationMessage.DialogButtons.MB_OK);
                    }
                }
            }

            ComboBoxConvertPages.SelectedIndex = Win_CBZSettings.Default.ImageConversionMode;

            ApplyUserKeyFilter = Win_CBZSettings.Default.KeyFilterActive;
            ButtonFilter.BackColor = ApplyUserKeyFilter ? Theme.GetInstance().AccentColor : SystemColors.Control;
            ButtonFilter.Image = ApplyUserKeyFilter ? Resources.funnel_error_16 : Resources.funnel;

            Program.ProjectModel.FilteredFileNames.Add(Win_CBZSettings.Default.MetaDataFilename.ToLower());

            // First Run App, initialize settings with defaults here  
            if (Win_CBZSettings.Default.FirstRun)
            {
                Win_CBZSettings.Default.CustomMetadataFields.Clear();
                Win_CBZSettings.Default.CustomMetadataFields.AddRange(FactoryDefaults.DefaultMetaDataFieldTypes);

                Win_CBZSettings.Default.MetaDataFilename = FactoryDefaults.DefaultMetaDataFileName;
                Win_CBZSettings.Default.MetaDataPageIndexVersionToWrite = FactoryDefaults.DefaultMetaDataFileIndexVersion;

                // update to latest version since user already has it now
                Win_CBZSettings.Default.SettingsVersion = FactoryDefaults.GetLastPatchVersion();

                Win_CBZSettings.Default.FirstRun = false;
                Win_CBZSettings.Default.Save();
            }

            int currentMigrationVersion = Win_CBZSettings.Default.SettingsVersion;
            try
            {
                int updatedVersion = FactoryDefaults.PatchUserSettings(currentMigrationVersion);
                if (updatedVersion > currentMigrationVersion)
                {
                    Win_CBZSettings.Default.SettingsVersion = updatedVersion;
                    Win_CBZSettings.Default.Save();

                    ApplicationMessage.Show("Your configuration has been updated to version: " + updatedVersion.ToString() + "\r\nPlease revise your applicaiton configuration.", "User settings updated", ApplicationMessage.DialogType.MT_INFORMATION, ApplicationMessage.DialogButtons.MB_OK);

                }
            }
            catch (SettingsPatchException ex)
            {
                MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_ERROR, ex.Message);

                if (ex.ShowErrorDialog)
                {
                    DialogResult res = ApplicationMessage.ShowWarning("Failed to patch User-Settings to Version: " + ex.CurrentVersion.ToString() + "\r\n" + ex.SourceException.ToString(), "Could not patch UserSettings", ApplicationMessage.DialogType.MT_WARNING, ApplicationMessage.DialogButtons.MB_OK | ApplicationMessage.DialogButtons.MB_IGNORE);
                    if (res == DialogResult.OK)
                    {
                        Win_CBZSettings.Default.SettingsVersion = ex.LastSuccessFulPatchedVersion;
                    }

                    if (res == DialogResult.Ignore)
                    {
                        Win_CBZSettings.Default.SettingsVersion = ex.CurrentVersion;
                    }

                }

                Win_CBZSettings.Default.Save();
            }
            catch (Exception e)
            {
                MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_ERROR, e.Message);

                ApplicationMessage.ShowException(e);
            }
            finally
            {
                MetaDataFieldConfig.GetInstance().UpdateFrom(Win_CBZSettings.Default.CustomMetadataFields.OfType<String>().ToArray());
            }

            Theme.GetInstance().SetColorHex("AccentColor", Win_CBZSettings.Default.AccentColor);

            ApplyTheme();

            
            SetControlsEnabledState("adjustments", false);

            //Win_CBZSettings.Default.SettingsVersion = 0;
            //Win_CBZSettings.Default.Save();

            df = new DebugForm(PageView);


            //pageClipboardMonitor = new PageClipboardMonitor();
            //pageClipboardMonitor.ClipboardChanged += ClipBoardChanged;
        }

        public void ApplyTheme()
        {
            PagesList.SelectionColor = Theme.GetInstance().AccentColor;
            ImageTaskListView.SelectionColor = Theme.GetInstance().AccentColor;
            MetaDataGrid.DefaultCellStyle.SelectionBackColor = Theme.GetInstance().AccentColor;
            //MetaDataGrid.ColumnHeadersDefaultCellStyle.SelectionBackColor = Theme.GetInstance().AccentColor;
            ButtonFilter.BackColor = ApplyUserKeyFilter ? Theme.GetInstance().AccentColor : SystemColors.Control;

            if (MetaDataGrid.Columns.Count > 0)
            {

                MetaDataGrid.Rows.Clear();
                MetaDataGrid.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
                MetaDataGrid.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;

                Program.ProjectModel.MetaData.RemoveSort();

                AppEventHandler.OnMetaDataLoaded(this, new MetaDataLoadEvent(Program.ProjectModel.MetaData.Values.ToList()));
            }
        }

        private ProjectModel NewProjectModel()
        {
            ProjectModel newProjectModel = new ProjectModel(Win_CBZSettings.Default.TempFolderPath, Win_CBZSettings.Default.MetaDataFilename);

            AppEventHandler.ArchiveStatusChanged += ArchiveStateChanged;
            AppEventHandler.TaskProgress += TaskProgress;
            AppEventHandler.PageChanged += PageChanged;
            AppEventHandler.CBZValidationEventHandler += ValidationFinished;
            AppEventHandler.MetaDataLoaded += MetaDataLoaded;
            AppEventHandler.MetaDataChanged += MetaDataChanged;
            AppEventHandler.MetaDataEntryChanged += MetaDataEntryChanged;
            // newProjectModel.ItemExtracted += ItemExtracted;
            AppEventHandler.OperationFinished += OperationFinished;
            AppEventHandler.FileOperation += FileOperationHandler;
            AppEventHandler.ArchiveOperation += ArchiveOperationHandler;
            AppEventHandler.ApplicationStateChanged += ApplicationStateChanged;
            AppEventHandler.GlobalActionRequired += HandleGlobalActionRequired;
            AppEventHandler.GeneralTaskProgress += HandleGlobalTaskProgress;
            AppEventHandler.RedrawThumbnail += HandleRedrawThumbnail;
            AppEventHandler.UpdateThumbnails += HandleUpdateThumbnails;
            AppEventHandler.ImageAdjustmentsChanged += HandleImageAdjustmentsChanged;
            AppEventHandler.UpdateListViewSorting += HandleListviewSorting;

            newProjectModel.RenameStoryPagePattern = Win_CBZSettings.Default.StoryPageRenamePattern;
            newProjectModel.RenameSpecialPagePattern = Win_CBZSettings.Default.SpecialPageRenamePattern;
            newProjectModel.CompatibilityMode = Win_CBZSettings.Default.CompatMode;

            return newProjectModel;
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            if (!WindowShown)
            {

                MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_INFO, Assembly.GetExecutingAssembly().GetName().Name + " v" + Assembly.GetExecutingAssembly().GetName().Version + "  - Welcome!");

                FileSettingsTablePanel.Width = MainSplitBox.SplitterDistance - 24;
                TablePanePageAdjustments.Width = MainSplitBox.SplitterDistance - 32;

                TextboxStoryPageRenamingPattern.Text = Win_CBZSettings.Default.StoryPageRenamePattern;
                TextboxSpecialPageRenamingPattern.Text = Win_CBZSettings.Default.SpecialPageRenamePattern;

                TogglePagePreviewToolbutton.Checked = Win_CBZSettings.Default.PagePreviewEnabled;
                SplitBoxPageView.Panel1Collapsed = !Win_CBZSettings.Default.PagePreviewEnabled;
                Program.ProjectModel.PreloadPageImages = Win_CBZSettings.Default.PagePreviewEnabled;
                CheckBoxIgnoreErrorsOnSave.Checked = Win_CBZSettings.Default.IgnoreErrorsOnSave;

                CheckBoxCompatibilityMode.Checked = Win_CBZSettings.Default.CompatMode;

                ComboBoxCompressionLevel.SelectedIndex = 0;

                bool enabled = Win_CBZSettings.Default.WriteXmlPageIndex;
                bool realCompatModeSetting = Win_CBZSettings.Default.CompatMode;

                CheckBoxCompatibilityMode.Enabled = enabled;
                CheckBoxCompatibilityMode.Checked = !enabled;

                if (enabled)
                {
                    CheckBoxCompatibilityMode.Checked = realCompatModeSetting;
                }

                Program.ProjectModel.CompatibilityMode = CheckBoxCompatibilityMode.Checked;

                // ---------------------------- DEBUG --------------------------------
                DebugToolsToolStripMenuItem.Visible = Program.DebugMode;


                // -------------------------------------------------------------------


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

                backgroundWorker1.RunWorkerAsync();
                UpdateCheckTimer.Enabled = Win_CBZSettings.Default.AutoUpdate;

                WindowShown = true;
            }
        }

        private void NewProject()
        {
            if (Program.ProjectModel != null)
            {
                try
                {
                    List<Thread> threads = new List<Thread>();

                    if (ThumbnailThread != null)
                    {
                        if (ThumbnailThread.IsAlive)
                        {
                            threads.Add(ThumbnailThread);
                        }
                    }

                    if (RequestThumbnailThread != null)
                    {
                        if (RequestThumbnailThread.IsAlive)
                        {
                            threads.Add(RequestThumbnailThread);
                        }
                    }

                    TokenStore.GetInstance().CancellationTokenSourceForName(TokenStore.TOKEN_SOURCE_THUMBNAIL_SLICE).Cancel();

                    Task<TaskResult> awaitClosingArchive = AwaitOperationsTask.AwaitOperations(threads, AppEventHandler.OnGeneralTaskProgress, TokenStore.GetInstance().RequestCancellationToken(TokenStore.TOKEN_SOURCE_AWAIT_THREADS), true);

                    awaitClosingArchive.ContinueWith(t =>
                    {
                        TokenStore.GetInstance().ResetCancellationToken(TokenStore.TOKEN_SOURCE_THUMBNAIL_SLICE);

                        RemoveMetaData();
                        Program.ProjectModel.New();
                        Invoke(new Action(() =>
                        {
                            //Program.ProjectModel.GlobalImageTask = new ImageTask("");

                            GlobalAlertTableLayout.Visible = false;

                            ImageTaskListView.Items.Clear();
                        }));

                        ClearLog();

                        AppEventHandler.OnArchiveStatusChanged(null, new ArchiveStatusEvent(Program.ProjectModel, ArchiveStatusEvent.ARCHIVE_NEW));
                    });

                    awaitClosingArchive.Start();
                }
                catch (ConcurrentOperationException c)
                {
                    MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_WARNING, c.Message);

                    if (c.ShowErrorDialog)
                    {
                        ApplicationMessage.ShowWarning(c.Message, "Concurrency Exception", ApplicationMessage.DialogType.MT_WARNING, ApplicationMessage.DialogButtons.MB_OK);
                    }
                }
            }
        }

        private void ClearLog()
        {
            if (!WindowClosed)
            {
                MessageLogListView.Invoke(new Action(() =>
                {
                    MessageLogListView.Items.Clear();
                    MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_INFO, Assembly.GetExecutingAssembly().GetName().Name + " v" + Assembly.GetExecutingAssembly().GetName().Version + "  - Welcome!");
                }));
            }
        }

        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LocalFile recentPath;

            if (Win_CBZSettings.Default.RecentOpenArchivePath != null && Win_CBZSettings.Default.RecentOpenArchivePath.Length > 0)
            {
                try
                {
                    recentPath = new LocalFile(Win_CBZSettings.Default.RecentOpenArchivePath);

                    OpenCBFDialog.InitialDirectory = Win_CBZSettings.Default.RecentOpenArchivePath;
                }
                catch (ApplicationException ae)
                {
                    Win_CBZSettings.Default.RecentOpenArchivePath = "";
                    MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_WARNING, "" + ae.Message);

                    if (ae.ShowErrorDialog)
                    {
                        //ApplicationMessage.ShowException(ae);
                    }
                }
                catch (ArgumentNullException ane)
                {
                    Win_CBZSettings.Default.RecentOpenArchivePath = "";
                }
            }

            DialogResult openCBFResult = DialogResult.None;

            if (Program.ProjectModel.IsChanged && !Program.ProjectModel.IsSaved)
            {
                DialogResult res = ApplicationMessage.ShowConfirmation("There are unsaved changes to the current CBZ-Archive.\nAre you sure you want to discard them and open a new file?", "Unsaved changes...");
                if (res == DialogResult.Yes)
                {
                    openCBFResult = OpenCBFDialog.ShowDialog();
                }
            }
            else
            {
                openCBFResult = OpenCBFDialog.ShowDialog();
            }

            if (openCBFResult == DialogResult.OK)
            {
                recentPath = new LocalFile(OpenCBFDialog.FileName);
                Win_CBZSettings.Default.RecentOpenArchivePath = recentPath.FilePath;

                Task followTask = new Task(() =>
                {


                    if (!WindowClosed)
                    {
                        Invoke(new Action(() =>
                        {

                            TextBoxExcludePagesImageProcessing.Text = "";
                            RenamerExcludePages.Text = "";
                            PageCountStatusLabel.Text = "0 Pages";
                            ImageTaskListView.Items.Clear();
                            Program.ProjectModel.IsChanged = false;
                            Program.ProjectModel.Pages.Clear();
                        }));
                        ClearLog();
                    }
                });

                Task finalTask = new Task(() =>
                {
                    Program.ProjectModel.Open(OpenCBFDialog.FileName,
                        ZipArchiveMode.Read,
                        MetaDataVersionFlavorHandler.GetInstance().TargetVersion(),
                        Win_CBZSettings.Default.SkipIndexCheck,
                        Win_CBZSettings.Default.InterpolationMode,
                        Win_CBZSettings.Default.WriteXmlPageIndex,
                        Win_CBZSettings.Default.KeyFilterActive,
                        Win_CBZSettings.Default.KeyFilter?.OfType<string>().ToArray(),
                        Win_CBZSettings.Default.KeyFilterBaseContitionType,
                        Win_CBZSettings.Default.SkipFilesInSubDirectories
                        );
                });

                Program.ProjectModel.Close(followTask, finalTask);
            }
        }

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LocalFile recentPath;

            if (Win_CBZSettings.Default.RecentSavedArchivePath != null && Win_CBZSettings.Default.RecentSavedArchivePath.Length > 0)
            {
                try
                {
                    recentPath = new LocalFile(Win_CBZSettings.Default.RecentSavedArchivePath);
                    SaveArchiveDialog.InitialDirectory = Win_CBZSettings.Default.RecentSavedArchivePath;
                }
                catch (ApplicationException ae)
                {
                    Win_CBZSettings.Default.RecentSavedArchivePath = "";
                    if (ae.ShowErrorDialog)
                    {

                    }
                }
                catch (ArgumentNullException ane)
                {

                }
            }

            DialogResult saveDialogResult = SaveArchiveDialog.ShowDialog();

            if (saveDialogResult == DialogResult.OK)
            {
                PagesList.SelectedItem = null;
                if (Program.ProjectModel.SaveAs(SaveArchiveDialog.FileName, ZipArchiveMode.Update, MetaDataVersionFlavorHandler.GetInstance().HandlePageIndexVersion()))
                {
                    try
                    {
                        recentPath = new LocalFile(SaveArchiveDialog.FileName);
                        Win_CBZSettings.Default.RecentSavedArchivePath = recentPath.FilePath;
                    }
                    catch (ApplicationException ae)
                    {
                        if (ae.ShowErrorDialog)
                        {

                        }
                    }
                    catch (ArgumentNullException ane)
                    {

                    }
                }
            }
        }

        private void ToolButtonSave_Click(object sender, EventArgs e)
        {
            if (Program.ProjectModel.IsNew && !Program.ProjectModel.IsSaved)
            {
                SaveAsToolStripMenuItem_Click(sender, e);
            }
            else
            {
                if (Program.ProjectModel.Exists())
                {
                    PagesList.SelectedItem = null;
                    if (Program.ProjectModel.Save())
                    {

                    }
                }
                else
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

                Program.ProjectModel.ParseFiles(files,
                    Win_CBZSettings.Default.CalculateHash,
                    Win_CBZSettings.Default.InterpolationMode,
                    Win_CBZSettings.Default.FilterByExtension,
                    Win_CBZSettings.Default.ImageExtenstionList,
                    Win_CBZSettings.Default.FilterSpecificFilenames,
                    Win_CBZSettings.Default.FilteredFilenamesList,
                    Win_CBZSettings.Default.DetectDoublePages
                    );

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
                    ApplicationMessage.Show("Validation Successful! CBZ Archive is valid, no problems detected.", "CBZ Archive validation successful!", ApplicationMessage.DialogType.MT_CHECK, ApplicationMessage.DialogButtons.MB_OK);
                }
            }
        }

        private void ClipBoardChanged(object sender, ClipboardChangedEvent e)
        {
            //


            if (e.Pages != null)
            {
                PasteToolStripMenuItem.Enabled = true;
            }
            else
            {
                PasteToolStripMenuItem.Enabled = false;
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
                        ListViewItem insertAt = null;
                        ListViewItem existingItem = FindListViewItemForPage(PagesList, e.Page);
                        if (e.OldValue != null && e.State != PageChangedEvent.IMAGE_STATUS_RENAMED)
                        {
                            insertAt = FindListViewItemForPage(PagesList, e.OldValue as Page);
                        }

                        if (e.State == PageChangedEvent.IMAGE_STATUS_RENAMED)
                        {
                            TextBoxExcludePagesImageProcessing.Invoke(new Action(() =>
                            {
                                List<string> lines = TextBoxExcludePagesImageProcessing.Lines.ToList();

                                int oldExcludeNameIndex = Array.IndexOf(TextBoxExcludePagesImageProcessing.Lines, (e.OldValue as Page).Name);
                                if (oldExcludeNameIndex > -1)
                                {
                                    lines[oldExcludeNameIndex] = e.Page.Name;

                                    TextBoxExcludePagesImageProcessing.Lines = lines.ToArray();
                                }
                            }));

                            RenamerExcludePages.Invoke(new Action(() =>
                            {
                                List<string> lines = TextBoxExcludePagesImageProcessing.Lines.ToList();

                                int oldExcludeNameIndex = Array.IndexOf(RenamerExcludePages.Lines, (e.OldValue as Page).Name);
                                if (oldExcludeNameIndex > -1)
                                {
                                    lines[oldExcludeNameIndex] = e.Page.Name;

                                    RenamerExcludePages.Lines = lines.ToArray();
                                }
                            }));
                        }

                        if (existingItem == null)
                        {
                            if (insertAt != null)
                            {
                                item = PagesList.Items.Insert(PagesList.Items.IndexOf(insertAt), e.Page?.Name);
                            }
                            else
                            {
                                item = PagesList.Items.Add(e.Page?.Name);
                            }

                            item.ImageKey = "photo_landscape";
                            item.SubItems.Add(e.Page?.Number.ToString());
                            item.SubItems.Add(e.Page?.ImageType.ToString());
                            item.SubItems.Add(e.Page?.Bookmark);
                            item.SubItems.Add(e.Page?.LastModified.ToString());
                            item.SubItems.Add(e.Page?.SizeFormat());
                        }
                        else
                        {
                            item = existingItem;
                            item.Text = e.Page?.Name;
                            item.SubItems[1] = new ListViewItem.ListViewSubItem(item, !e.Page.Deleted ? e.Page.Number.ToString() : "-");
                            item.SubItems[2] = new ListViewItem.ListViewSubItem(item, e.Page?.ImageType.ToString());
                            item.SubItems[3] = new ListViewItem.ListViewSubItem(item, e.Page?.Bookmark);
                            item.SubItems[4] = new ListViewItem.ListViewSubItem(item, e.Page?.LastModified.ToString());
                            item.SubItems[5] = new ListViewItem.ListViewSubItem(item, e.Page?.SizeFormat());
                        }

                        item.Tag = e.Page;
                        item.BackColor = Color.White;
                        item.ForeColor = Color.Black;

                        if (e.Page != null)
                        {
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
                                    e.Page.Changed = true;
                                    e.Page.Invalidated = true;
                                    e.Page.ThumbnailInvalidated = true;
                                    break;
                                case PageChangedEvent.IMAGE_STATUS_RENAMED:
                                    e.Page.Renamed = true;
                                    break;
                                case PageChangedEvent.IMAGE_STATUS_ERROR:
                                    //
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
                                }
                                else
                                {
                                    item.BackColor = HTMLColor.ToColor(Colors.COLOR_LIGHT_PURPLE);
                                }
                            }

                            if (e.Page.Deleted)
                            {
                                item.ForeColor = Color.Silver;
                                item.BackColor = Color.Transparent;
                            }

                            if (e.State == PageChangedEvent.IMAGE_STATUS_ERROR)
                            {
                                item.ForeColor = Color.White;
                                item.BackColor = HTMLColor.ToColor(Colors.COLOR_ERROR_RED);
                            }
                        }
                        else
                        {
                            item.ForeColor = Color.White;
                            item.BackColor = HTMLColor.ToColor(Colors.COLOR_ERROR_RED);
                        }
                    }
                    else
                    {

                        if (e.State == PageChangedEvent.IMAGE_STATUS_CLOSED)
                        {
                            ListViewItem existingItem = FindListViewItemForPage(PagesList, e.Page);

                            if (existingItem != null)
                            {
                                PagesList.Items.Remove(existingItem);

                                if (PageThumbsListBox.Items.Count > 0)
                                {
                                    PageThumbsListBox.Items.Remove(existingItem.Tag);
                                }
                            }

                            if (existingItem != null)
                            {
                                PageView.Items.Remove(existingItem);
                            }
                        }

                    }
                }));

                if (!WindowClosed)
                {
                    Invoke(new Action(() =>
                    {
                        PageCountStatusLabel.Text = Program.ProjectModel.GetPageCount().ToString() + " Pages";
                    }));
                }

                if (TogglePagePreviewToolbutton.Checked && !e.NoThumbRefresh && !WindowClosed)
                {
                    if (e.Page != null)
                    {
                        if (!e.Page.Closed && !e.Page.Deleted)
                        {
                            CancellationToken cancellationToken = TokenStore.GetInstance().CancellationTokenSourceForName(TokenStore.TOKEN_SOURCE_GLOBAL).Token;

                            Task.Factory.StartNew(() =>
                            {
                                if (!cancellationToken.IsCancellationRequested)
                                {
                                    PageThumbsListBox.Invoke(new Action(() =>
                                    {
                                        CreatePagePreviewFromItem(e.Page, e.OldValue as Page);

                                    }));
                                }

                            }, cancellationToken);
                        }
                    }
                }
            }
        }

        private void OperationFinished(object sender, OperationFinishedEvent e)
        {
            if (!WindowClosed)
            {
                MainToolStripProgressBar.Invoke(new Action(() =>
                {
                    MainToolStripProgressBar.Maximum = 100;
                    MainToolStripProgressBar.Value = 0;
                }));
            }
        }

        private void FileOperationHandler(object sender, FileOperationEvent e)
        {
            if (!WindowClosed)
            {
                Invoke(new Action(() =>
                {
                    if (e.Status == FileOperationEvent.STATUS_RUNNING)
                    {
                        MainToolStripProgressBar.Maximum = 100;
                        MainToolStripProgressBar.Value = Convert.ToInt32(100 * e.Completed / e.Total);

                        if (e.Operation == FileOperationEvent.OPERATION_COPY)
                        {
                            ApplicationStatusLabel.Text = "Copying file...";
                        }

                    }
                    else
                    {
                        MainToolStripProgressBar.Value = 0;
                        ApplicationStatusLabel.Text = "Ready.";

                    }
                }));
            }
        }

        private void ArchiveOperationHandler(object sender, ArchiveOperationEvent e)
        {
            if (!WindowClosed)
            {
                MainToolStripProgressBar.Invoke(new Action(() =>
                {
                    MainToolStripProgressBar.Maximum = e.Total;
                    MainToolStripProgressBar.Value = e.Completed;
                }));
            }
        }

        private void PageOperationHandler(object sender, ArchiveOperationEvent e)
        {
            if (!WindowClosed)
            {
                MainToolStripProgressBar.Invoke(new Action(() =>
                {
                    MainToolStripProgressBar.Maximum = e.Total;
                    MainToolStripProgressBar.Value = e.Completed;
                }));
            }
        }

        private ListViewItem FindListViewItemForPage(ExtendetListView owner, Page page)
        {
            foreach (ListViewItem item in owner.Items)
            {
                if (((Page)item.Tag).Id.Equals(page?.Id))
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
            if (!WindowClosed)
            {
                Invoke(new Action(() =>
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
                    else
                    {
                        GlobalAlertTableLayout.Visible = true;
                    }
                }));
            }
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
                        OpenToolStripMenuItem.Enabled = false;
                        AddFilesToolStripMenuItem.Enabled = false;
                        AddFolderToolStripMenuItem.Enabled = false;

                        SaveAsToolStripMenuItem.Enabled = false;
                        ToolButtonSave.Enabled = false;
                        ToolButtonOpen.Enabled = false;
                        ToolButtonNew.Enabled = false;
                        ToolButtonAddFiles.Enabled = false;
                        ToolButtonAddFolder.Enabled = false;
                        ToolButtonMovePageDown.Enabled = false;
                        ToolButtonMovePageUp.Enabled = false;
                        ToolButtonRemoveFiles.Enabled = false;
                        NewToolStripMenuItem.Enabled = false;

                    }));
                }

                try
                {
                    if (!WindowClosed)
                    {
                        if (!e.InBackground)
                        {
                            Invoke(() => ApplicationStatusLabel.Text = e.Message);

                            MainToolStripProgressBar.Invoke(new Action(() =>
                            {
                                MainToolStripProgressBar.Maximum = e.Total;
                                if (e.Current > -1 && e.Current <= e.Total)
                                {
                                    MainToolStripProgressBar.Value = e.Current;
                                }
                            }));
                        }
                        else
                        {
                            BackgroundTaskStatusPanel.Invoke(new Action(() =>
                            {
                                BackgroundTaskStatusPanel.Visible = true;
                                BackgroundTaskStatusLabel.Text = e.Message;
                            }));
                        }

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
                        if (e.GlobalActionId != null)
                        {
                            SaveToolStripMenuItem.Enabled = true;
                            OpenToolStripMenuItem.Enabled = true;
                            SaveAsToolStripMenuItem.Enabled = true;
                            AddFilesToolStripMenuItem.Enabled = true;
                            AddFolderToolStripMenuItem.Enabled = true;
                            ToolButtonSave.Enabled = true; //Program.ProjectModel.FileName != null && Program.ProjectModel.FileName.Length > 0;
                            ToolButtonNew.Enabled = true;
                            ToolButtonOpen.Enabled = true;
                            ToolButtonAddFiles.Enabled = true;
                            ToolButtonAddFolder.Enabled = true;
                            ToolButtonMovePageDown.Enabled = true;
                            ToolButtonMovePageUp.Enabled = true;
                            ToolButtonRemoveFiles.Enabled = true;
                            NewToolStripMenuItem.Enabled = true;
                            //ApplicationStatusLabel.Text = e.Message;
                            Program.ProjectModel.IsChanged = true;

                            AppEventHandler.OnApplicationStateChanged(
                                sender,
                                new ApplicationStatusEvent
                                {
                                    ArchiveInfo = Program.ProjectModel,
                                    State = ApplicationStatusEvent.STATE_READY
                                }
                            );
                        }



                        if (e.Type == GeneralTaskProgressEvent.TASK_RELOAD_IMAGE_METADATA)
                        {
                            //Program.ProjectModel.MetaDataPageIndexMissingData = false;
                            //Program.ProjectModel.MetaData.RebuildPageMetaData(Program.ProjectModel.Pages, MetaDataVersionFlavorHandler.GetInstance().HandlePageIndexVersion());
                        }

                        if (e.PopGlobalState)
                        {
                            if (CurrentGlobalActions.Count > 1)
                            {
                                CurrentGlobalActions.Remove(CurrentGlobalAction);

                                CurrentGlobalAction = CurrentGlobalActions[0];

                                Invoke(new Action(() =>
                                {
                                    LabelGlobalActionStatusMessage.Text = CurrentGlobalAction.Message;
                                    GlobalAlertTableLayout.Visible = true;
                                }));
                            }
                            else
                            {
                                if (CurrentGlobalActions.Count > 0)
                                    CurrentGlobalActions.Remove(CurrentGlobalAction);
                                CurrentGlobalAction = null;

                                Invoke(new Action(() =>
                                {
                                    GlobalAlertTableLayout.Visible = false;
                                }));
                            }
                        }
                        else
                        {

                            if (CurrentGlobalActions.Count > 0)
                            {
                                if (CurrentGlobalAction.Key == e.GlobalActionId)
                                {
                                    CurrentGlobalActions.Remove(CurrentGlobalAction);
                                    CurrentGlobalAction = null;

                                    Invoke(new Action(() =>
                                    {
                                        GlobalAlertTableLayout.Visible = false;
                                    }));
                                }
                            }
                        }
                    }));

                    try
                    {
                        if (!WindowClosed)
                        {
                            if (!e.InBackground)
                            {
                                Invoke(() => ApplicationStatusLabel.Text = e.Message);
                                MainToolStripProgressBar.Invoke(new Action(() =>
                                {
                                    MainToolStripProgressBar.Maximum = e.Total;

                                    MainToolStripProgressBar.Value = 0;

                                }));
                            }
                            else
                            {
                                BackgroundTaskStatusPanel.Invoke(new Action(() =>
                                {
                                    BackgroundTaskStatusPanel.Visible = false;
                                    BackgroundTaskStatusLabel.Text = "";
                                }));
                            }
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
                else
                {

                }
            }
        }

        private void HandleRedrawThumbnail(object sender, RedrawThumbEvent e)
        {
            if (!WindowClosed)
            {
                Invoke(new Action(() =>
                {
                    e.Page.ThumbnailInvalidated = true;
                    int pageIndex = PageThumbsListBox.Items.IndexOf(Program.ProjectModel.GetPageById(e.Page.Id));
                    if (pageIndex > -1)
                    {
                        if (PageImages.Images.ContainsKey(e.Page.Id))
                        {
                            PageImages.Images.RemoveByKey(e.Page.Id);
                        }
                        PageThumbsListBox.Items[pageIndex] = e.Page;
                    }
                    PageThumbsListBox.Invalidate();
                    PageThumbsListBox.Refresh();
                }));
            }
        }

        private void HandleUpdateThumbnails(object sender, UpdateThumbnailsEvent e)
        {
            if (!WindowClosed)
            {
                Invoke(new Action(() =>
                {
                    if (e.Pages != null && e.Pages.Count > 0)
                    {
                        foreach (Page p in e.Pages)
                        {
                            p.Invalidated = true;
                            int pageIndex = PageThumbsListBox.Items.IndexOf(Program.ProjectModel.GetPageById(p.Id));
                            if (pageIndex > -1)
                            {
                                if (PageImages.Images.ContainsKey(p.Id))
                                {
                                    PageImages.Images.RemoveByKey(p.Id);
                                }
                                PageThumbsListBox.Items[pageIndex] = p;
                            }
                        }
                        PageThumbsListBox.Invalidate();
                        PageThumbsListBox.Refresh();
                    }
                }));
            }
        }

        private void HandleListviewSorting(object sender, UpdatePageListViewSortingEvent e)
        {
            if (!WindowClosed)
            {
                Invoke(() =>
                {
                    Program.ProjectModel.Pages.Sort((x, y) => x.Index.CompareTo(y.Index));

                    PagesList.ListViewItemSorter = new ListViewSorter(e.SortColumn);

                    PagesList.Sorting = e.Order;
                    PagesList.Sort();
                    PagesList.Sorting = SortOrder.None;

                    if (PageThumbsListBox.Items.Count > 0)
                    {
                        PageThumbsListBox.Sort(e.Order);
                    }


                });
            }
        }

        public void ReloadPreviewThumbs()
        {
            if (!WindowClosed)
            {
                PageThumbsListBox.Invoke(new Action(() =>
                {
                    PageImages.Images.Clear();

                    foreach (Page page in Program.ProjectModel.Pages)
                    {
                        try
                        {
                            page.ThumbnailInvalidated = true;

                            if (!PageImages.Images.ContainsKey(page.Id))
                            {
                                PageImages.Images.Add(page.Id, page.GetThumbnail());
                            }
                            else
                            {
                                PageImages.Images.RemoveByKey(page.Name);
                                PageImages.Images.Add(page.Id, page.GetThumbnail());
                            }
                        }
                        catch (Exception e)
                        {
                            if (!page.ThumbnailError)
                            {
                                page.ThumbnailError = true;
                                MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_WARNING, "Error generating Thumbnail for Page '" + page.Name + "' (" + page.Id + ") [" + e.Message + "]");
                            }
                        }
                    }
                }));
            }
        }

        public void RequestThumbnailSlice()
        {
            if (Win_CBZSettings.Default.PagePreviewEnabled)
            {

                if (ThumbnailThread != null)
                {
                    if (ThumbnailThread.IsAlive)
                    {

                        TokenStore.GetInstance().CancellationTokenSourceForName(TokenStore.TOKEN_SOURCE_THUMBNAIL).Cancel();

                    }
                }

                if (RequestImageInfoThread != null)
                {
                    if (RequestImageInfoThread.IsAlive)
                    {
                        TokenStore.GetInstance().CancellationTokenSourceForName(TokenStore.TOKEN_SOURCE_UPDATE_PAGE).Cancel();

                    }
                }

                if (RequestThumbnailThread != null)
                {
                    if (RequestThumbnailThread.IsAlive)
                    {

                        return;
                    }
                }

                TokenStore.GetInstance().ResetCancellationToken(TokenStore.TOKEN_SOURCE_THUMBNAIL_SLICE);

                List<Page> currentSlice = new List<Page>(ThumbnailPagesSlice.ToArray());


                RequestThumbnailThread = new Thread(LoadThumbnailSlice);
                RequestThumbnailThread.Start(new ThumbSliceThreadParams()
                {
                    ThumbnailPagesSlice = currentSlice,
                    CancelToken = TokenStore.GetInstance().CancellationTokenSourceForName(TokenStore.TOKEN_SOURCE_THUMBNAIL_SLICE).Token,
                    ThumbnailQueue = ThumbnailPagesSlice,
                    ArchiveState = Program.ProjectModel.ArchiveState
                });

                //Task awaitTasks = AwaitOperationsTask.AwaitOperations(threads);

                //Task thenTask = awaitTasks.ContinueWith(task => {

                //});

                //thenTask.ContinueWith(task => 
                //{ 
                //ThumbnailPagesSlice.Clear(); 
                //});

                //awaitTasks.Start();
            }
        }

        public void LoadThumbnailSlice(object threadParams)
        {
            ThumbSliceThreadParams tParams = threadParams as ThumbSliceThreadParams;

            bool updateRequired = false;
            int indexToUpdate = -1;

            if (tParams.ArchiveState != ArchiveStatusEvent.ARCHIVE_CLOSING && tParams.ArchiveState != ArchiveStatusEvent.ARCHIVE_CLOSED)
            {

                if (!WindowClosed)
                {

                    //PageImages.Images.Clear();
                    try
                    {
                        foreach (Page page in tParams.ThumbnailPagesSlice)
                        {
                            try
                            {
                                if (!page.Closed && !page.Deleted)
                                {
                                    if (!PageImages.Images.ContainsKey(page.Id))
                                    {
                                        PageImages.Images.Add(page.Id, page.GetThumbnail());
                                        updateRequired = true;
                                    }
                                    else
                                    {
                                        if (page.ThumbnailInvalidated && PageImages.Images.IndexOfKey(page.Id) > -1)
                                        {
                                            updateRequired = true;
                                            PageImages.Images[PageImages.Images.IndexOfKey(page.Id)] = page.GetThumbnail();
                                            page.ThumbnailInvalidated = false;
                                        }
                                    }

                                    if (updateRequired)
                                    {
                                        PageThumbsListBox.Invoke(new Action(() =>
                                        {
                                            indexToUpdate = PageThumbsListBox.Items.IndexOf(page);
                                            if (indexToUpdate > -1)
                                            {
                                                PageThumbsListBox.Items[indexToUpdate] = page;

                                            }
                                        }));
                                    }
                                }
                                else
                                {
                                    page.ThumbnailInvalidated = false;
                                }

                                if (tParams.ThumbnailQueue != null)
                                {
                                    tParams.ThumbnailQueue.Remove(page);
                                }

                                tParams.CancelToken.ThrowIfCancellationRequested();
                            }
                            catch (OperationCanceledException)
                            {
                                break;
                            }
                            catch (Exception e)
                            {
                                if (!page.ThumbnailError)
                                {
                                    page.ThumbnailError = true;
                                    MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_WARNING, "Error generating Thumbnail for Page '" + page.Name + "' (" + page.Id + ") [" + e.Message + "]");
                                }
                            }
                            finally
                            {
                                page.FreeImage();
                            }

                            Thread.Sleep(10);
                        }

                    }
                    catch (Exception eo)
                    {
                        MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_WARNING, "Error in thumb-generation thread! [" + eo.Message + "]");
                    }


                    if (TogglePagePreviewToolbutton.Checked && PageThumbsListBox.Items.Count > 0) //PageView.Items.Count > 0)
                    {
                        //PageView.RedrawItems(0, PageView.Items.Count - 1, false);

                        /*
                        int itemIndex = -1;

                        foreach (Page page in tParams.ThumbnailPagesSlice)
                        {
                            try
                            {
                                if (!page.Closed)
                                {
                                    itemIndex = PageThumbsListBox.Items.IndexOf(page);

                                    if (itemIndex > -1)
                                    {
                                        PageThumbsListBox.Items[itemIndex] = page;
                                    }
                                }

                                tParams.CancelToken.ThrowIfCancellationRequested();
                            }
                            catch (OperationCanceledException)
                            {
                                break;
                            }
                            catch (Exception e)
                            {
                                MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_WARNING, "Error generating Thumbnail for Page '" + page.Name + "' (" + page.Id + ") [" + e.Message + "]");
                            }
                            finally
                            {
                                page.FreeImage();
                            }
                        }
                        */
                    }

                    //tParams.ThumbnailPagesSlice.Clear();

                    PageThumbsListBox.Invoke(new Action(() =>
                    {
                        PageThumbsListBox.Invalidate();
                    }));
                }
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private Page CreatePagePreviewFromItem(Page page, Page insertAt = null)
        {
            //ListViewItem itemPage;
            Page existingItem = FindThumbImageForPage(PageView, page);
            Page insertPageAt = null;

            if (insertAt != null)
            {
                insertPageAt = FindThumbImageForPage(PageView, insertAt);
            }

            if (existingItem == null)
            {
                if (insertPageAt == null)
                {
                    PageThumbsListBox.Items.Add(page);
                }
                else
                {
                    PageThumbsListBox.Items.Insert(PageThumbsListBox.Items.IndexOf(insertPageAt), page);
                }

            }
            else
            {
                PageThumbsListBox.Items[PageThumbsListBox.Items.IndexOf(existingItem)] = page;
            }

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

                RequestImageInfoThread = new Thread(LoadImageInfoSlice);
                RequestImageInfoThread.Start();
            }
        }

        public void LoadImageInfoSlice(object threadParams)
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
                    if (!page.ThumbnailError)
                    {
                        page.ThumbnailError = true;
                        MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_WARNING, "Error loading Image-Info for Page '" + page.Name + "' (" + page.Id + ") [" + e.Message + "]");
                    }
                }
                finally
                {
                    page.FreeImage();
                }
            }

            ImageInfoPagesSlice.Clear();
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
            PageThumbsListBox.Invoke(new Action(() =>
            {
                PageView.Items.Clear();
                PageThumbsListBox.Items.Clear();
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

        public void RefreshPageView()
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

                UpdatePageViewThread = new Thread(new ThreadStart(RefreshPageViewProc));
                UpdatePageViewThread.Start();
            }
        }

        private void RefreshPageViewProc()
        {
            PageThumbsListBox.Invoke(new Action(() =>
            {
                int thumbIndex = -1;
                foreach (Page item in PageThumbsListBox.Items)
                {
                    thumbIndex = PageThumbsListBox.Items.IndexOf(item);

                    PageImages.Images.RemoveByKey(item.Id);
                    PageImages.Images.Add(item.Id, item.GetThumbnailBitmap());
                    item.FreeImage();
                    PageThumbsListBox.Items[thumbIndex] = item;
                }

                PageThumbsListBox.Refresh();
            }));
        }

        private void TaskProgress(object sender, TaskProgressEvent e)
        {
            try
            {
                if (!WindowClosed)
                {
                    StatusToolStripTableLayout.Invoke(new Action(() =>
                    {
                        MainToolStripProgressBar.Maximum = e.Total;
                        if (e.Current > -1 && e.Current <= e.Total)
                        {
                            MainToolStripProgressBar.Value = e.Current;
                        }

                        if (e.Message != null)
                        {
                            ApplicationStatusLabel.Text = e.Message;
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
                                logItem.ImageKey = "info";
                                break;
                            case LogMessageEvent.LOGMESSAGE_TYPE_WARNING:
                                logItem.ImageKey = "warning";
                                break;
                            case LogMessageEvent.LOGMESSAGE_TYPE_ERROR:
                                logItem.ImageKey = "error";
                                break;
                            default:
                                logItem.ImageIndex = -1;
                                break;
                        }
                    }));
                }
            }
            catch (Exception) { }
        }

        private void ApplicationStateChanged(object sender, ApplicationStatusEvent e)
        {
            String info = ApplicationStatusLabel.Text;
            String filename = e?.ArchiveInfo?.FileName;

            Program.ProjectModel.ApplicationState = e.State;

            switch (e.State)
            {
                case ApplicationStatusEvent.STATE_UPDATING_INDEX:
                    if (!WindowClosed)
                    {
                        Invoke(new Action(() =>
                        {
                            LabelGlobalActionStatusMessage.Text = "";
                            GlobalAlertTableLayout.Visible = false;

                            CurrentGlobalAction = null;
                            CurrentGlobalActions.Clear();
                        }));
                    }
                    break;

                case ApplicationStatusEvent.STATE_PROCESSING:
                    info = "Working...";
                    break;

                case ApplicationStatusEvent.STATE_DELETING:
                    info = "Deleting files...";
                    break;

                case ApplicationStatusEvent.STATE_ANALYZING:
                    info = "Analyzing images...";
                    break;

                case ApplicationStatusEvent.STATE_CHECKING_INDEX:
                    info = "Checking index...";
                    break;

                case ApplicationStatusEvent.STATE_READY:
                    ResetUpdateTags();
                    info = "Ready.";
                    break;

                default:
                    break;
            }

            try
            {
                //if (this.InvokeRequired)
                //{
                if (!WindowClosed)
                {
                    Invoke(new Action(() =>
                    {
                        FileNameLabel.Text = filename;
                        ApplicationStatusLabel.Text = info;
                        Program.ProjectModel.ArchiveState = e.State;
                        if (e.ArchiveInfo != null)
                        {
                            PageCountStatusLabel.Text = e.ArchiveInfo.GetPageCount().ToString() + " Pages";
                        }
                        DisableControllsForApplicationState(e.State);
                    }));
                    //}
                }
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
                    NewToolStripMenuItem.Enabled = true;
                    OpenToolStripMenuItem.Enabled = true;
                    SaveAsToolStripMenuItem.Enabled = true;
                    ToolButtonNew.Enabled = true;
                    ToolButtonOpen.Enabled = true;
                    AddFilesToolStripMenuItem.Enabled = true;
                    ToolButtonAddFiles.Enabled = true;
                    ToolButtonRemoveFiles.Enabled = false;
                    ToolButtonMovePageDown.Enabled = false;
                    ToolButtonMovePageUp.Enabled = false;
                    ToolButtonAddFolder.Enabled = true;
                    BtnAddMetaData.Enabled = Program.ProjectModel.MetaData.Values.Count == 0;
                    AddMetaDataRowBtn.Enabled = Program.ProjectModel.MetaData.Exists() && Program.ProjectModel.MetaData.Values != null;
                    BtnRemoveMetaData.Enabled = Program.ProjectModel.MetaData.Values.Count > 0;
                    ToolStripButtonShowRawMetadata.Enabled = Program.ProjectModel.MetaData.Values.Count > 0;
                    ToolButtonExtractArchive.Enabled = !Program.ProjectModel.IsNew;
                    ExtractSelectedPages.Enabled = !Program.ProjectModel.IsNew;
                    TextboxStoryPageRenamingPattern.Enabled = true;
                    TextboxSpecialPageRenamingPattern.Enabled = true;
                    CheckBoxDoRenamePages.Enabled = true;
                    ToolButtonSave.Enabled = Program.ProjectModel.IsChanged; // && Program.ProjectModel.FileName != null && Program.ProjectModel.FileName.Length > 0;
                    SaveToolStripMenuItem.Enabled = Program.ProjectModel.IsChanged; // && Program.ProjectModel.FileName != null;
                    PagesList.Enabled = true;
                    PageView.Enabled = true;
                    PageThumbsListBox.Enabled = true;
                    MetaDataGrid.Enabled = true;
                    ToolButtonEditImageProps.Enabled = false;
                    ToolButtonEditImage.Enabled = false;
                    ToolButtonValidateCBZ.Enabled = true;
                    AddFolderToolStripMenuItem.Enabled = true;
                    ToolBarSearchInput.Enabled = true;
                    ToolBarSearchLabel.Enabled = true;

                    SetControlsEnabledState("renaming,imagetasks", true);

                    break;

                case ApplicationStatusEvent.STATE_OPENING:
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
                    ToolButtonEditImage.Enabled = false;
                    ToolButtonValidateCBZ.Enabled = false;
                    ToolButtonSetPageType.Enabled = false;
                    AddFolderToolStripMenuItem.Enabled = false;
                    ToolBarSearchInput.Enabled = false;
                    ToolBarSearchLabel.Enabled = false;

                    SetControlsEnabledState("adjustments,renaming,imagetasks", false);

                    break;

                case ApplicationStatusEvent.STATE_PROCESSING:
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
                    ToolButtonEditImage.Enabled = false;
                    ToolButtonValidateCBZ.Enabled = false;
                    ToolButtonSetPageType.Enabled = false;
                    AddFolderToolStripMenuItem.Enabled = false;
                    ToolButtonOpen.Enabled = false;
                    ToolButtonNew.Enabled = false;
                    ToolButtonAddFiles.Enabled = false;
                    ToolButtonRemoveFiles.Enabled = false;
                    ToolButtonAddFolder.Enabled = false;
                    ExtractSelectedPages.Enabled = false;

                    SetControlsEnabledState("adjustments,renaming,imagetasks", false);

                    break;

                case ApplicationStatusEvent.STATE_SAVING:
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
                    ToolButtonEditImage.Enabled = false;
                    ToolButtonValidateCBZ.Enabled = false;
                    ToolButtonSetPageType.Enabled = false;
                    AddFolderToolStripMenuItem.Enabled = false;
                    ToolBarSearchInput.Enabled = false;
                    ToolBarSearchLabel.Enabled = false;

                    SetControlsEnabledState("adjustments,renaming,imagetasks", false);

                    break;

                case ApplicationStatusEvent.STATE_CLOSING:
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
                    RemoveMetadataRowBtn.Enabled = false;
                    ToolButtonEditImageProps.Enabled = false;
                    ToolButtonEditImage.Enabled = false;
                    ToolButtonValidateCBZ.Enabled = false;
                    ToolButtonSetPageType.Enabled = false;
                    AddFolderToolStripMenuItem.Enabled = false;
                    ToolBarSearchInput.Enabled = false;
                    ToolBarSearchLabel.Enabled = false;

                    SetControlsEnabledState("adjustments,renaming,imagetasks", false);

                    break;

                case ApplicationStatusEvent.STATE_ADDING:
                    AddFolderToolStripMenuItem.Enabled = false;
                    ToolButtonSave.Enabled = false;
                    SaveToolStripMenuItem.Enabled = false;
                    AddMetaDataRowBtn.Enabled = false;
                    RemoveMetadataRowBtn.Enabled = false;
                    AddFilesToolStripMenuItem.Enabled = false;
                    ToolButtonAddFiles.Enabled = false;
                    BtnAddMetaData.Enabled = false;
                    BtnRemoveMetaData.Enabled = false;
                    break;

                case ApplicationStatusEvent.STATE_DELETING:
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
                    ToolButtonEditImage.Enabled = false;
                    ToolButtonValidateCBZ.Enabled = false;
                    ToolButtonSetPageType.Enabled = false;
                    AddFolderToolStripMenuItem.Enabled = false;

                    break;

                case ApplicationStatusEvent.STATE_RENAMING:
                    ToolButtonSave.Enabled = false;
                    SaveToolStripMenuItem.Enabled = false;
                    ToolButtonRemoveFiles.Enabled = false;
                    break;

                case ApplicationStatusEvent.STATE_ANALYZING:
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
                    ToolButtonEditImage.Enabled = false;
                    ToolButtonValidateCBZ.Enabled = false;

                    break;

            }
        }

        private void ArchiveStateChanged(object sender, ArchiveStatusEvent e)
        {
            String info = "Ready.";
            String filename = e.ArchiveInfo.FileName;

            try
            {
                if (!WindowClosed)
                {
                    MainToolStripProgressBar.Invoke(new Action(() =>
                    {
                        if (e.State != ArchiveStatusEvent.ARCHIVE_FILE_ADDED && e.State != ArchiveStatusEvent.ARCHIVE_FILE_RENAMED && e.State != ArchiveStatusEvent.ARCHIVE_CLOSING)
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

            Program.ProjectModel.ArchiveState = e.State;

            switch (e.State)
            {
                case ArchiveStatusEvent.ARCHIVE_OPENED:
                    info = "Ready.";
                    break;

                case ArchiveStatusEvent.ARCHIVE_OPENING:
                    info = "Reading archive...";
                    break;

                case ArchiveStatusEvent.ARCHIVE_CLOSED:

                    if (!this.WindowClosed)
                    {
                        this.Invoke(new Action(() =>
                        {
                            Program.ProjectModel.Pages.Clear();
                            PagesList.Items.Clear();
                            PageView.Items.Clear();
                            PageThumbsListBox.Items.Clear();
                            PageImages.Images.Clear();
                            filename = "";
                        }));
                    }

                    filename = "";

                    info = "Ready.";
                    break;

                case ArchiveStatusEvent.ARCHIVE_CLOSING:
                    info = "Closing file...";
                    break;

                case ArchiveStatusEvent.ARCHIVE_SAVED:

                    info = "Ready.";
                    break;

                case ArchiveStatusEvent.ARCHIVE_ERROR_SAVING:
                    info = "Ready.";
                    break;

                case ArchiveStatusEvent.ARCHIVE_SAVING:
                    info = "Writing archive...";
                    break;

                case ArchiveStatusEvent.ARCHIVE_FILE_ADDED:
                    Program.ProjectModel.IsChanged = true;
                    info = "Adding image...";
                    break;

                case ArchiveStatusEvent.ARCHIVE_FILE_RENAMED:
                    Program.ProjectModel.IsChanged = true;
                    info = "Renaming page...";
                    break;
                case ArchiveStatusEvent.ARCHIVE_FILE_UPDATED:
                    Program.ProjectModel.IsChanged = true;
                    break;
                case ArchiveStatusEvent.ARCHIVE_EXTRACTING:
                    info = "Extracting file...";
                    break;
                case ArchiveStatusEvent.ARCHIVE_METADATA_CHANGED:
                    Program.ProjectModel.IsChanged = true;
                    break;
            }

            try
            {
                Program.ProjectModel.ArchiveState = e.State;

                Invoke(new Action(() =>
                {
                    if (TogglePagePreviewToolbutton.Checked && PageThumbsListBox.Items.Count > 0) // PageView.Items.Count > 0)
                    {


                    }

                    FileNameLabel.Text = filename;
                    ApplicationStatusLabel.Text = info;
                    PageCountStatusLabel.Text = e.ArchiveInfo.GetPageCount().ToString() + " Pages";

                    DisableControllsForArchiveState(e.ArchiveInfo, e.State);
                }));

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
                    case ArchiveStatusEvent.ARCHIVE_READY:
                        NewToolStripMenuItem.Enabled = true;
                        OpenToolStripMenuItem.Enabled = true;
                        SaveAsToolStripMenuItem.Enabled = true;
                        ToolButtonNew.Enabled = true;
                        ToolButtonOpen.Enabled = true;
                        AddFilesToolStripMenuItem.Enabled = true;
                        ToolButtonAddFiles.Enabled = true;
                        ToolButtonAddFolder.Enabled = true;
                        ToolBarSearchInput.Enabled = true;
                        ToolBarSearchLabel.Enabled = true;

                        ToolButtonRemoveFiles.Enabled = false;
                        ToolButtonMovePageDown.Enabled = false;
                        ToolButtonMovePageUp.Enabled = false;
                        TextboxStoryPageRenamingPattern.Enabled = false;
                        TextboxSpecialPageRenamingPattern.Enabled = false;
                        CheckBoxDoRenamePages.Enabled = false;
                        CheckBoxDoRenamePages.Checked = false;
                        GlobalAlertTableLayout.Visible = false;
                        ToolButtonEditImageProps.Enabled = false;
                        ToolButtonEditImage.Enabled = false;

                        PagesList.Enabled = true;
                        PageView.Enabled = true;
                        PageThumbsListBox.Enabled = true;
                        MetaDataGrid.Enabled = true;
                        AddMetaDataRowBtn.Enabled = false;

                        ToolButtonValidateCBZ.Enabled = true;
                        CurrentGlobalAction = null;

                        ToolButtonSave.Enabled = Program.ProjectModel.IsChanged; // && project.FileName != null && Program.ProjectModel.FileName.Length > 0;
                        SaveToolStripMenuItem.Enabled = Program.ProjectModel.IsChanged; // && project.FileName != null && Program.ProjectModel.FileName.Length > 0;
                        LabelGlobalActionStatusMessage.Text = "";

                        BtnAddMetaData.Enabled = Program.ProjectModel.MetaData.Values.Count == 0;
                        AddMetaDataRowBtn.Enabled = Program.ProjectModel.MetaData.Values != null;
                        BtnRemoveMetaData.Enabled = Program.ProjectModel.MetaData.Values.Count > 0;
                        ToolStripButtonShowRawMetadata.Enabled = Program.ProjectModel.MetaData.Values.Count > 0;

                        LabelW.Text = "0";
                        LabelH.Text = "0";

                        ImageTaskListView.Items.Clear();


                        CurrentGlobalActions.Clear();

                        SetControlsEnabledState("imagetasks,renaming", true);

                        break;

                    case ArchiveStatusEvent.ARCHIVE_SAVING:
                    case ArchiveStatusEvent.ARCHIVE_OPENING:
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
                        ToolButtonEditImage.Enabled = false;
                        ToolButtonValidateCBZ.Enabled = false;
                        ToolBarSearchInput.Enabled = false;
                        ToolBarSearchLabel.Enabled = false;

                        SetControlsEnabledState("adjustments,renaming,imagetasks", false);
                        break;

                    case ArchiveStatusEvent.ARCHIVE_OPENED:
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

                        ToolButtonSave.Enabled = false;
                        SaveToolStripMenuItem.Enabled = false;
                        ToolStripButtonShowRawMetadata.Enabled = true;
                        //TextboxStoryPageRenamingPattern.Enabled = true;
                        //TextboxSpecialPageRenamingPattern.Enabled = true;
                        CheckBoxDoRenamePages.Enabled = true;
                        CheckBoxDoRenamePages.Checked = false;
                        ToolButtonValidateCBZ.Enabled = true;

                        Program.ProjectModel.IsNew = false;
                        PagesList.Enabled = true;
                        PageView.Enabled = true;
                        PageThumbsListBox.Enabled = true;
                        MetaDataGrid.Enabled = true;
                        ToolBarSearchInput.Enabled = true;
                        ToolBarSearchLabel.Enabled = true;

                        BtnAddMetaData.Enabled = Program.ProjectModel.MetaData.Values.Count == 0;
                        BtnRemoveMetaData.Enabled = Program.ProjectModel.MetaData.Values.Count > 0;
                        AddMetaDataRowBtn.Enabled = Program.ProjectModel.MetaData.Values != null;

                        SetControlsEnabledState("renaming,imagetasks", true);
                        break;

                    case ArchiveStatusEvent.ARCHIVE_SAVED:
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

                        ToolStripButtonShowRawMetadata.Enabled = true;
                        ExtractSelectedPages.Enabled = true;
                        ToolButtonValidateCBZ.Enabled = true;

                        PagesList.Enabled = true;
                        PageView.Enabled = true;
                        PageThumbsListBox.Enabled = true;
                        MetaDataGrid.Enabled = true;

                        ToolButtonSave.Enabled = false;
                        SaveToolStripMenuItem.Enabled = false;

                        Program.ProjectModel.IsNew = false;
                        Program.ProjectModel.IsSaved = true;
                        BtnAddMetaData.Enabled = Program.ProjectModel.MetaData.Values.Count == 0;
                        BtnRemoveMetaData.Enabled = Program.ProjectModel.MetaData.Values.Count > 0;
                        AddMetaDataRowBtn.Enabled = Program.ProjectModel.MetaData.Values != null;
                        PageView.Refresh();
                        PageView.Invalidate();

                        SetControlsEnabledState("renaming,imagetasks", true);
                        break;

                    case ArchiveStatusEvent.ARCHIVE_ERROR_SAVING:
                        NewToolStripMenuItem.Enabled = true;
                        OpenToolStripMenuItem.Enabled = true;
                        SaveToolStripMenuItem.Enabled = true;
                        SaveAsToolStripMenuItem.Enabled = true;
                        ToolButtonSave.Enabled = true; // Program.ProjectModel.FileName != null && Program.ProjectModel.FileName.Length > 0;
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
                        ToolStripButtonShowRawMetadata.Enabled = true;
                        ExtractSelectedPages.Enabled = true;
                        Program.ProjectModel.IsSaved = false;
                        PagesList.Enabled = true;
                        PageView.Enabled = true;
                        PageThumbsListBox.Enabled = true;
                        MetaDataGrid.Enabled = true;
                        ToolButtonValidateCBZ.Enabled = true;
                        BtnAddMetaData.Enabled = Program.ProjectModel.MetaData.Values.Count == 0;
                        BtnRemoveMetaData.Enabled = Program.ProjectModel.MetaData.Values.Count > 0;
                        AddMetaDataRowBtn.Enabled = Program.ProjectModel.MetaData.Values != null;
                        PageView.Refresh();
                        PageView.Invalidate();

                        SetControlsEnabledState("adjustments,renaming,imagetasks", false);

                        break;

                    case ArchiveStatusEvent.ARCHIVE_EXTRACTING:
                        NewToolStripMenuItem.Enabled = false;
                        OpenToolStripMenuItem.Enabled = false;
                        SaveAsToolStripMenuItem.Enabled = false;
                        SaveToolStripMenuItem.Enabled = false;
                        ToolButtonSave.Enabled = false;
                        ToolButtonRemoveFiles.Enabled = false;
                        ToolButtonNew.Enabled = false;
                        ToolButtonOpen.Enabled = false;
                        AddFilesToolStripMenuItem.Enabled = false;
                        ToolButtonAddFiles.Enabled = false;
                        ToolButtonAddFolder.Enabled = false;
                        BtnRemoveMetaData.Enabled = false;
                        ToolButtonExtractArchive.Enabled = false;
                        ExtractSelectedPages.Enabled = false;
                        ToolButtonValidateCBZ.Enabled = false;

                        break;

                    case ArchiveStatusEvent.ARCHIVE_EXTRACTED:
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

                    case ArchiveStatusEvent.ARCHIVE_CLOSING:
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
                        ToolButtonEditImage.Enabled = false;
                        ToolButtonValidateCBZ.Enabled = false;
                        ToolBarSearchInput.Enabled = false;
                        ToolBarSearchLabel.Enabled = false;

                        SetControlsEnabledState("adjustments,renaming,imagetasks", false);

                        break;

                    case ArchiveStatusEvent.ARCHIVE_CLOSED:
                        NewToolStripMenuItem.Enabled = true;
                        OpenToolStripMenuItem.Enabled = true;
                        SaveAsToolStripMenuItem.Enabled = true;
                        ToolButtonNew.Enabled = true;
                        ToolButtonOpen.Enabled = true;
                        AddFilesToolStripMenuItem.Enabled = true;
                        ToolButtonAddFiles.Enabled = true;
                        ToolBarSearchInput.Enabled = true;
                        ToolBarSearchLabel.Enabled = true;
                        ToolButtonAddFolder.Enabled = true;
                        ToolButtonValidateCBZ.Enabled = true;

                        ToolButtonRemoveFiles.Enabled = false;
                        ToolButtonMovePageDown.Enabled = false;
                        ToolButtonMovePageUp.Enabled = false;
                        TextboxStoryPageRenamingPattern.Enabled = false;
                        TextboxSpecialPageRenamingPattern.Enabled = false;
                        CheckBoxDoRenamePages.Enabled = false;
                        CheckBoxDoRenamePages.Checked = false;
                        ToolButtonSave.Enabled = false;
                        SaveToolStripMenuItem.Enabled = false;
                        ToolStripButtonShowRawMetadata.Enabled = false;
                        AddMetaDataRowBtn.Enabled = false;
                        ToolButtonEditImageProps.Enabled = false;
                        ToolButtonEditImage.Enabled = false;
                        GlobalAlertTableLayout.Visible = false;

                        PagesList.Enabled = true;
                        PageView.Enabled = true;
                        PageThumbsListBox.Enabled = true;
                        MetaDataGrid.Enabled = true;



                        CurrentGlobalAction = null;

                        BtnAddMetaData.Enabled = Program.ProjectModel.MetaData.Values.Count == 0;
                        AddMetaDataRowBtn.Enabled = Program.ProjectModel.MetaData.Values != null;
                        BtnRemoveMetaData.Enabled = Program.ProjectModel.MetaData.Values.Count > 0;
                        LabelW.Text = "0";
                        LabelH.Text = "0";
                        LabelGlobalActionStatusMessage.Text = "";
                        ImageTaskListView.Items.Clear();
                        CurrentGlobalActions.Clear();

                        SetControlsEnabledState("renaming,imagetasks", true);
                        break;

                    case ArchiveStatusEvent.ARCHIVE_FILE_ADDED:
                        CheckBoxDoRenamePages.Enabled = true;

                        ToolButtonSave.Enabled = true;
                        SaveToolStripMenuItem.Enabled = true;
                        SaveAsToolStripMenuItem.Enabled = true;

                        SetControlsEnabledState("renaming,imagetasks", true);

                        break;

                    case ArchiveStatusEvent.ARCHIVE_FILE_DELETED:
                    case ArchiveStatusEvent.ARCHIVE_FILE_RENAMED:
                    case ArchiveStatusEvent.ARCHIVE_FILE_UPDATED:

                        ToolButtonSave.Enabled = true;
                        SaveToolStripMenuItem.Enabled = true;
                        SaveAsToolStripMenuItem.Enabled = true;

                        break;
                    case ArchiveStatusEvent.ARCHIVE_METADATA_ADDED:
                        AddMetaDataRowBtn.Enabled = Program.ProjectModel.MetaData.HasValues();

                        ToolButtonSave.Enabled = true;
                        SaveToolStripMenuItem.Enabled = true;
                        SaveAsToolStripMenuItem.Enabled = true;
                        ToolBarSearchInput.Enabled = true;
                        ToolBarSearchLabel.Enabled = true;

                        break;
                    case ArchiveStatusEvent.ARCHIVE_METADATA_CHANGED:
                    case ArchiveStatusEvent.ARCHIVE_METADATA_DELETED:

                        ToolButtonSave.Enabled = true;
                        SaveToolStripMenuItem.Enabled = true;
                        SaveAsToolStripMenuItem.Enabled = true;

                        break;
                }
            }
        }

        private void SetControlsEnabledState(string groups, bool enabled)
        {
            groups.Split(',').Select(g => g.ToLower()).ToList().ForEach(group =>
            {
                switch (group)
                {
                    case "all":
                        NewToolStripMenuItem.Enabled = enabled;
                        OpenToolStripMenuItem.Enabled = enabled;
                        SaveAsToolStripMenuItem.Enabled = enabled;
                        ToolButtonNew.Enabled = enabled;
                        ToolButtonOpen.Enabled = enabled;
                        AddFilesToolStripMenuItem.Enabled = enabled;
                        ToolButtonAddFiles.Enabled = enabled;
                        ToolButtonRemoveFiles.Enabled = enabled;
                        ToolButtonMovePageDown.Enabled = enabled;
                        ToolButtonMovePageUp.Enabled = enabled;
                        ToolButtonAddFolder.Enabled = enabled;
                        BtnAddMetaData.Enabled = enabled;
                        BtnRemoveMetaData.Enabled = enabled;
                        ToolButtonExtractArchive.Enabled = enabled;
                        ExtractSelectedPages.Enabled = enabled;
                        ToolButtonSave.Enabled = enabled;
                        SaveToolStripMenuItem.Enabled = enabled;
                        ToolStripButtonShowRawMetadata.Enabled = enabled;
                        PagesList.Enabled = enabled;
                        PageView.Enabled = enabled;
                        PageThumbsListBox.Enabled = enabled;
                        MetaDataGrid.Enabled = enabled;
                        AddMetaDataRowBtn.Enabled = enabled;
                        RemoveMetadataRowBtn.Enabled = enabled;
                        ToolButtonEditImageProps.Enabled = enabled;
                        ToolButtonEditImage.Enabled = enabled;
                        ToolButtonValidateCBZ.Enabled = enabled;
                        ToolButtonSetPageType.Enabled = enabled;
                        ImageTaskListView.Enabled = enabled;
                        ToolbarImageTasks.Enabled = enabled;
                        ComboBoxTaskOrderConversion.Enabled = enabled;
                        ComboBoxTaskOrderResize.Enabled = enabled;
                        ComboBoxTaskOrderRotation.Enabled = enabled;
                        //ComboBoxTaskOrderSplit.Enabled = enabled;
                        ComboBoxConvertPages.Enabled = enabled;
                        RadioButtonResizeNever.Enabled = enabled;
                        RadioButtonResizeIfLarger.Enabled = enabled;
                        RadioButtonResizePercent.Enabled = enabled;
                        RadioButtonRotateNone.Enabled = enabled;
                        RadioButtonRotate90.Enabled = enabled;
                        RadioButtonRotate180.Enabled = enabled;
                        RadioButtonRotate270.Enabled = enabled;
                        CheckBoxSplitDoublePages.Enabled = enabled;
                        ComboBoxSplitAtType.Enabled = enabled;
                        CheckboxKeepAspectratio.Enabled = enabled;
                        CheckBoxDontStretch.Enabled = enabled;
                        CheckboxIgnoreDoublePages.Enabled = enabled;
                        CheckBoxSplitOnlyIfDoubleSize.Enabled = enabled;
                        TextBoxResizePageIndexReference.Enabled = enabled;

                        break;
                    case "save":
                        ToolButtonSave.Enabled = enabled;
                        SaveToolStripMenuItem.Enabled = enabled;
                        SaveAsToolStripMenuItem.Enabled = enabled;
                        break;
                    case "open":
                        OpenToolStripMenuItem.Enabled = enabled;
                        ToolButtonOpen.Enabled = enabled;
                        break;
                    case "file":
                        NewToolStripMenuItem.Enabled = enabled;
                        OpenToolStripMenuItem.Enabled = enabled;
                        SaveAsToolStripMenuItem.Enabled = enabled;
                        ToolButtonNew.Enabled = enabled;
                        ToolButtonOpen.Enabled = enabled;
                        ToolButtonSave.Enabled = enabled;
                        SaveToolStripMenuItem.Enabled = enabled;
                        break;
                    case "add":
                        AddFilesToolStripMenuItem.Enabled = enabled;
                        ToolButtonAddFiles.Enabled = enabled;
                        ToolButtonAddFolder.Enabled = enabled;
                        break;
                    case "remove":
                        ToolButtonRemoveFiles.Enabled = enabled;
                        break;
                    case "metadata":
                        BtnAddMetaData.Enabled = enabled;
                        BtnRemoveMetaData.Enabled = enabled;
                        AddMetaDataRowBtn.Enabled = enabled;
                        RemoveMetadataRowBtn.Enabled = enabled;
                        ToolStripButtonShowRawMetadata.Enabled = enabled;
                        MetaDataGrid.Enabled = enabled;
                        break;
                    case "extract":
                        ToolButtonExtractArchive.Enabled = enabled;
                        ExtractSelectedPages.Enabled = enabled;
                        break;
                    case "pageview":
                        PagesList.Enabled = enabled;
                        PageView.Enabled = enabled;
                        PageThumbsListBox.Enabled = enabled;

                        break;
                    case "imageprops":
                        ToolButtonEditImageProps.Enabled = enabled;
                        ToolButtonEditImage.Enabled = enabled;
                        break;
                    case "validate":
                        ToolButtonValidateCBZ.Enabled = enabled;
                        break;
                    case "search":
                        ToolBarSearchInput.Enabled = enabled;
                        ToolBarSearchLabel.Enabled = enabled;
                        break;
                    case "imagetasks":
                        ImageTaskListView.Enabled = enabled;
                        ToolbarImageTasks.Enabled = enabled;
                        break;
                    case "adjustments":
                        ComboBoxTaskOrderConversion.Enabled = enabled;
                        ComboBoxTaskOrderResize.Enabled = enabled;
                        ComboBoxTaskOrderRotation.Enabled = enabled;
                        //ComboBoxTaskOrderSplit.Enabled = enabled;
                        ComboBoxConvertPages.Enabled = enabled;
                        RadioButtonResizeNever.Enabled = enabled;
                        RadioButtonResizeTo.Enabled = enabled;
                        RadioButtonResizeIfLarger.Enabled = enabled;
                        RadioButtonResizePercent.Enabled = enabled;
                        RadioButtonRotateNone.Enabled = enabled;
                        RadioButtonRotate90.Enabled = enabled;
                        RadioButtonRotate180.Enabled = enabled;
                        RadioButtonRotate270.Enabled = enabled;
                        CheckBoxSplitDoublePages.Enabled = enabled;
                        TextBoxResizeH.Enabled = enabled;
                        TextBoxResizeW.Enabled = enabled;
                        TextboxResizePercentage.Enabled = enabled;
                        TextBoxSplitPageAt.Enabled = enabled;
                        ComboBoxSplitAtType.Enabled = enabled;
                        CheckboxKeepAspectratio.Enabled = enabled;
                        CheckBoxDontStretch.Enabled = enabled;
                        TextBoxExcludePagesImageProcessing.Enabled = enabled;
                        GetImageProcessExcludesFromSelectedButton.Enabled = enabled;
                        CheckboxIgnoreDoublePages.Enabled = enabled;
                        CheckBoxSplitOnlyIfDoubleSize.Enabled = enabled;
                        TextBoxResizePageIndexReference.Enabled = enabled;

                        break;
                    case "renaming":
                        CheckBoxDoRenamePages.Enabled = enabled;
                        CheckBoxPreview.Enabled = enabled;
                        TextboxStoryPageRenamingPattern.Enabled = enabled;
                        TextboxSpecialPageRenamingPattern.Enabled = enabled;

                        break;
                    default:
                        // Do nothing
                        break;
                }
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

                        MainToolStripProgressBar.Value = 0;

                        ComboBoxConvertPages.SelectedIndex = Win_CBZSettings.Default.ImageConversionMode;
                        TextBoxExcludePagesImageProcessing.Text = "";
                        RenamerExcludePages.Text = "";

                        NewProject();
                    }
                }
                else
                {

                    MainToolStripProgressBar.Value = 0;

                    ComboBoxConvertPages.SelectedIndex = Win_CBZSettings.Default.ImageConversionMode;
                    TextBoxExcludePagesImageProcessing.Text = "";
                    RenamerExcludePages.Text = "";

                    NewProject();
                }
            }
            else
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
                    }
                    else
                    {
                        e.Cancel = true;
                    }
                }
                else
                {
                    QuitApplication();
                }
            }
        }

        private void CancelAllThreads()
        {
            TokenStore.GetInstance().CancellationTokenSourceForName(TokenStore.TOKEN_SOURCE_GLOBAL).Cancel();
        }

        private void QuitApplication()
        {
            if (Win_CBZSettings.Default.RestoreWindowLayout)
            {
                Win_CBZSettings.Default.WindowW = Width;
                Win_CBZSettings.Default.WindowH = Height;
                Win_CBZSettings.Default.Splitter1 = MainSplitBox.SplitterDistance;
                Win_CBZSettings.Default.Splitter2 = SplitBoxPageView.SplitterDistance;
                Win_CBZSettings.Default.Splitter3 = SplitBoxItemsList.SplitterDistance;
                Win_CBZSettings.Default.Splitter4 = PrimarySplitBox.SplitterDistance;
            }

            Win_CBZSettings.Default.IgnoreErrorsOnSave = CheckBoxIgnoreErrorsOnSave.Checked;

            Win_CBZSettings.Default.CustomMetadataFields.Clear();
            Win_CBZSettings.Default.CustomMetadataFields.AddRange(MetaDataFieldConfig.GetInstance().PrepareForConfig());

            Win_CBZSettings.Default.Save();

            WindowClosed = true;
            CancelAllThreads();
            Program.ProjectModel.CancelAllThreads();
            backgroundWorker1.CancelAsync();
            Application.ExitThread();
        }

        private void AddFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LocalFile recentPath;

            if (Win_CBZSettings.Default.RecentAddImagePath != null && Win_CBZSettings.Default.RecentAddImagePath.Length > 0)
            {
                try
                {
                    recentPath = new LocalFile(Win_CBZSettings.Default.RecentAddImagePath);

                    OpenImagesDialog.InitialDirectory = Win_CBZSettings.Default.RecentAddImagePath;
                }
                catch (ApplicationException ae)
                {
                    Win_CBZSettings.Default.RecentAddImagePath = "";
                    MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_WARNING, "" + ae.Message);

                    if (ae.ShowErrorDialog)
                    {
                        //ApplicationMessage.ShowException(ae);
                    }
                }
                catch (ArgumentNullException ane)
                {
                    Win_CBZSettings.Default.RecentAddImagePath = "";
                }
            }

            DialogResult openImageResult = OpenImagesDialog.ShowDialog();

            if (openImageResult == DialogResult.OK)
            {
                var maxIndex = PagesList.Items.Count - 1;
                var newIndex = maxIndex < 0 ? 0 : maxIndex;

                recentPath = new LocalFile(OpenImagesDialog.FileName);
                Win_CBZSettings.Default.RecentAddImagePath = recentPath.FilePath;

                Program.ProjectModel.ParseFiles(new List<String>(OpenImagesDialog.FileNames),
                    Win_CBZSettings.Default.CalculateHash,
                    Win_CBZSettings.Default.InterpolationMode,
                    Win_CBZSettings.Default.FilterByExtension,
                    Win_CBZSettings.Default.ImageExtenstionList,
                    Win_CBZSettings.Default.FilterSpecificFilenames,
                    Win_CBZSettings.Default.FilteredFilenamesList,
                    Win_CBZSettings.Default.DetectDoublePages
                    );
            }
        }

        private void PagesList_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            if (e.Label == null)
            {
                e.CancelEdit = true;

                return;
            }

            if (e.Item > -1)
            {
                ListViewItem changedItem = PagesList.Items[e.Item];
                if (changedItem != null)
                {
                    Page p = changedItem.Tag as Page;
                    if (p?.Name != e.Label)
                    {
                        try
                        {
                            //Page oldPage = new Page(((Page)changedItem.Tag));  // dont load page here! will cause Access denied
                            Program.ProjectModel.RenamePage((Page)changedItem.Tag, e.Label, false, true);

                            Program.ProjectModel.MetaData.UpdatePageIndexMetaDataEntry((Page)changedItem.Tag, ((Page)changedItem.Tag).Key);

                            // dont fire events again!
                            //AppEventHandler.OnPageChanged(sender, new PageChangedEvent(((Page)changedItem.Tag), null, PageChangedEvent.IMAGE_STATUS_RENAMED));
                            //AppEventHandler.OnArchiveStatusChanged(sender, new ArchiveStatusEvent(Program.ProjectModel, ArchiveStatusEvent.ARCHIVE_FILE_UPDATED));
                        }
                        catch (PageDuplicateNameException eduplicate)
                        {
                            e.CancelEdit = true;

                            MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_WARNING, eduplicate.Message);
                            if (eduplicate.ShowErrorDialog)
                            {
                                ApplicationMessage.ShowWarning("Error renaming page\r\n" + eduplicate.Message, "Error renaming page", ApplicationMessage.DialogType.MT_WARNING, ApplicationMessage.DialogButtons.MB_OK);
                            }
                        }
                        catch (PageException pe)
                        {
                            e.CancelEdit = true;
                            MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_WARNING, pe.Message);
                            if (pe.ShowErrorDialog)
                            {
                                ApplicationMessage.ShowException(pe, ApplicationMessage.DialogType.MT_ERROR);
                            }
                        }
                        catch (ApplicationException ae)
                        {
                            e.CancelEdit = true;
                            MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_WARNING, ae.Message);
                            if (ae.ShowErrorDialog)
                            {
                                ApplicationMessage.ShowWarning("Error renaming page\r\n" + ae.Message, "Error renaming page", ApplicationMessage.DialogType.MT_WARNING, ApplicationMessage.DialogButtons.MB_OK);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_ERROR, ex.Message);
                            e.CancelEdit = true;
                        }
                        finally
                        {

                        }
                    }
                }
            }
        }

        private void MoveItemsTo(int newIndex, System.Windows.Forms.ListView.SelectedListViewItemCollection items)
        {

            TokenStore.GetInstance().CancellationTokenSourceForName(TokenStore.TOKEN_SOURCE_THUMBNAIL_SLICE).Cancel();
            //TokenStore.GetInstance().CancellationTokenSourceForName(TokenStore.TOKEN_SOURCE_).Cancel();

            MoveItemsThread = new Thread(MoveItemsToProc);
            MoveItemsThread.Start(new MoveItemsToThreadParams()
            {
                NewIndex = newIndex,
                Items = items,
                PageIndexVersion = MetaDataVersionFlavorHandler.GetInstance().HandlePageIndexVersion(),
                CancelToken = TokenStore.GetInstance().CancellationTokenSourceForName(TokenStore.TOKEN_SOURCE_MOVE_ITEMS).Token,
            });

        }

        private void MoveItemsToProc(object threadParams)
        {
            MoveItemsToThreadParams tparams = threadParams as MoveItemsToThreadParams;

            Page pageOriginal;
            int newIndex = tparams.NewIndex;

            PagesList.Invoke(new Action(() =>
            {
                List<ListViewItem> ItemsSliced = new List<ListViewItem>();
                List<Page> PageViewItemsSliced = new List<Page>();
                List<Page> PagesSliced = new List<Page>();
                if (tparams.NewIndex < 0 || tparams.NewIndex > PagesList.Items.Count - 1)
                {
                    return;
                }

                foreach (ListViewItem item in tparams.Items)
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

                newIndex = tparams.NewIndex;
                foreach (Page p in PagesSliced)
                {
                    p.Index = newIndex;
                    p.Number = newIndex + 1;
                    Program.ProjectModel.Pages.Insert(newIndex, p);

                    AppEventHandler.OnPageChanged(this, new PageChangedEvent(p, null, PageChangedEvent.IMAGE_STATUS_CHANGED));
                    AppEventHandler.OnArchiveStatusChanged(this, new ArchiveStatusEvent(Program.ProjectModel, ArchiveStatusEvent.ARCHIVE_FILE_UPDATED));

                    newIndex++;
                }

                TokenStore.GetInstance().ResetCancellationToken(TokenStore.TOKEN_SOURCE_MOVE_ITEMS);

                AppEventHandler.OnGlobalActionRequired(this, new GlobalActionRequiredEvent(
                    Program.ProjectModel,
                    0,
                    "Page order changed. Rebuild pageindex now?",
                    "Rebuild",
                    GlobalActionRequiredEvent.TASK_TYPE_INDEX_REBUILD,
                    UpdatePageIndexTask.UpdatePageIndex(Program.ProjectModel.Pages,
                        AppEventHandler.OnGeneralTaskProgress,
                        AppEventHandler.OnPageChanged,
                        TokenStore.GetInstance().CancellationTokenSourceForName(TokenStore.TOKEN_SOURCE_MOVE_ITEMS).Token,
                        false,
                        true
                    )
                ));


                newIndex = tparams.NewIndex;
                foreach (Page pageItem in PageViewItemsSliced)
                {
                    //pageItem.Text = NewIndex.ToString();
                    PageThumbsListBox.Items.Insert(newIndex, pageItem);


                    newIndex++;
                }

                //Program.ProjectModel.UpdatePageIndices();
                Program.ProjectModel.IsChanged = true;
            }));
        }

        private void MovePageTo(Page page, int newIndex)
        {

            TokenStore.GetInstance().CancellationTokenSourceForName(TokenStore.TOKEN_SOURCE_UPDATE_PAGE).Cancel();
            TokenStore.GetInstance().CancellationTokenSourceForName(TokenStore.TOKEN_SOURCE_THUMBNAIL_SLICE).Cancel();
            TokenStore.GetInstance().CancellationTokenSourceForName(TokenStore.TOKEN_SOURCE_THUMBNAIL).Cancel();

            MovePagesThread = new Thread(MovePageProc);
            MovePagesThread.Start(new MovePageThreadParams()
            {
                newIndex = newIndex,
                page = page,
                pageIndexVersion = MetaDataVersionFlavorHandler.GetInstance().HandlePageIndexVersion(),
                CancelToken = TokenStore.GetInstance().CancellationTokenSourceForName(TokenStore.TOKEN_SOURCE_MOVE_ITEMS).Token
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
                //Page originalImage;
                //Page updateImage;

                updateItem = FindListViewItemForPage(PagesList, tparams.page);
                updatePage = FindThumbImageForPage(PageThumbsListBox, tparams.page);
                //updateImage = PageImages.Images[PageImages.Images.IndexOfKey(page.Id)];


                originalItem = PagesList.Items[tparams.newIndex];
                //originalPage = FindListViewItemForPage(PageView, (Page)originalItem.Tag);
                originalPage = FindThumbImageForPage(PageThumbsListBox, (Page)originalItem.Tag);
                //originalImage = PageImages.Images[PageImages.Images.IndexOfKey(((Page)originalItem.Tag).Id)];

                int IndexItemToMove = updateItem.Index;

                //if (NewIndex < 0 || NewIndex > PagesList.Items.Count - 1)
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

                    //PageThumbsListBox.Items.Remove(originalPage);
                    PageThumbsListBox.Items.Remove(updatePage);

                }

                Program.ProjectModel.Pages.Remove((Page)originalItem.Tag);
                Program.ProjectModel.Pages.Remove(tparams.page);

                if (tparams.newIndex > Program.ProjectModel.Pages.Count - 1)
                {
                    PagesList.Items.Add(originalItem);
                    PagesList.Items.Add(updateItem);

                    Program.ProjectModel.Pages.Add((Page)originalItem.Tag);
                    Program.ProjectModel.Pages.Add(tparams.page);
                }
                else
                {
                    PagesList.Items.Insert(tparams.newIndex, updateItem);
                    PagesList.Items.Insert(tparams.newIndex + 1, originalItem);

                    Program.ProjectModel.Pages.Insert(tparams.newIndex, tparams.page);
                    Program.ProjectModel.Pages.Insert(tparams.newIndex + 1, (Page)originalItem.Tag);
                }

                if (updatePage != null && originalPage != null)
                {
                    if (tparams.newIndex > Program.ProjectModel.Pages.Count - 1)
                    {
                        PageThumbsListBox.Items.Add(updatePage);
                    }
                    else
                    {
                        PageThumbsListBox.Items.Insert(tparams.newIndex, updatePage);
                    }

                }

                AppEventHandler.OnPageChanged(this, new PageChangedEvent(tparams.page, originalPage, PageChangedEvent.IMAGE_STATUS_CHANGED));
                if (originalPage != null)
                {
                    AppEventHandler.OnPageChanged(this, new PageChangedEvent(originalPage, null, PageChangedEvent.IMAGE_STATUS_CHANGED));
                }

                TokenStore.GetInstance().ResetCancellationToken(TokenStore.TOKEN_SOURCE_UPDATE_PAGE_INDEX);

                AppEventHandler.OnArchiveStatusChanged(this, new ArchiveStatusEvent(Program.ProjectModel, ArchiveStatusEvent.ARCHIVE_FILE_UPDATED));
                AppEventHandler.OnGlobalActionRequired(this,
                    new GlobalActionRequiredEvent(Program.ProjectModel,
                    GlobalActionRequiredEvent.MESSAGE_TYPE_INFO,
                    "Page order changed. Rebuild pageindex now?",
                    "Rebuild",
                    GlobalActionRequiredEvent.TASK_TYPE_INDEX_REBUILD,
                    UpdatePageIndexTask.UpdatePageIndex(Program.ProjectModel.Pages,
                        AppEventHandler.OnGeneralTaskProgress,
                        AppEventHandler.OnPageChanged,
                        TokenStore.GetInstance().CancellationTokenSourceForName(TokenStore.TOKEN_SOURCE_UPDATE_PAGE_INDEX).Token,
                        false,
                        true
                    )
                ));

                Program.ProjectModel.IsChanged = true;
            }));
        }

        private bool ArchiveProcessing()
        {
            return (Program.ProjectModel.ArchiveState == ArchiveStatusEvent.ARCHIVE_SAVING ||
               Program.ProjectModel.ArchiveState == ArchiveStatusEvent.ARCHIVE_OPENING ||
               Program.ProjectModel.ArchiveState == ArchiveStatusEvent.ARCHIVE_EXTRACTING ||
               Program.ProjectModel.ArchiveState == ArchiveStatusEvent.ARCHIVE_CLOSING ||
               Program.ProjectModel.ApplicationState == GeneralTaskProgressEvent.TASK_STATUS_RUNNING ||
               Program.ProjectModel.ThreadRunning()
               );
        }

        private void BtnAddMetaData_Click(object sender, EventArgs e)
        {
            AddMetaData();
        }

        private void BtnRemoveMetaData_Click(object sender, EventArgs e)
        {
            if (Program.ProjectModel != null && Program.ProjectModel.MetaData != null)
            {
                if ((Program.ProjectModel.MetaData.HasValues() || Program.ProjectModel.MetaData.HasRemovedValues()) && Program.ProjectModel.Exists())
                {
                    if (ApplicationMessage.ShowConfirmation("Are you sure you want to remove existing Metadata from Archive?", "Remove existing Meta-Data") == DialogResult.Yes)
                    {
                        RemoveMetaData();
                    }
                }
                else
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

            if (MetaDataGrid.SelectedCells.Count == 1)
            {


                if (MetaDataGrid.SelectedCells[0].ColumnIndex == 1)
                {
                    if (MetaDataGrid.SelectedCells[0] is DataGridViewComboBoxCell)
                    {
                        // && fieldType.FieldType == MetaDataFieldType.METADATA_FIELD_TYPE_COMBO_BOX) {
                        DataGridViewComboBoxCell comboCell = MetaDataGrid.SelectedCells[0] as DataGridViewComboBoxCell;
                        MetaDataGrid.BeginEdit(true);
                    }
                    //else if (senderGrid.Rows[e.RowIndex].Cells[e.ColumnIndex] is DataGridViewTextBoxCell)
                    //{ // && fieldType.FieldType == MetaDataFieldType.METADATA_FIELD_TYPE_AUTO_COMPLETE) {
                    //DataGridViewTextBoxCell textCell = senderGrid.Rows[e.RowIndex].Cells[e.ColumnIndex] as DataGridViewTextBoxCell;
                    //value = textCell.Value?.ToString();
                    //}

                }
            }
        }

        private void RemoveMetadataRowBtn_Click(object sender, EventArgs e)
        {
            if (MetaDataGrid.SelectedRows.Count > 0)
            {

                foreach (DataGridViewRow row in MetaDataGrid.SelectedRows)
                {
                    MetaDataEntry selectedEntry = row.Cells[0].Tag as MetaDataEntry;
                    int index = Program.ProjectModel.MetaData.Remove(selectedEntry.Uid);

                    if (index > -1)
                    {
                        var key = row.Cells[0].Value?.ToString();
                        if (ToolBarSearchInput.Text.Length > 0 || ApplyUserKeyFilter)
                        {

                            MetaDataGrid.Rows.RemoveAt(row.Index);

                        }
                    }
                    else
                    {
                        ApplicationMessage.ShowWarning("Error removing Metadata entry. Given entry does not exist in Collection.", "Error removing Metadata entry", ApplicationMessage.DialogType.MT_WARNING, ApplicationMessage.DialogButtons.MB_OK);
                    }
                }
            }
        }

        private void MetaDataGrid_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            try
            {
                EditorTypeConfig editorTypeConfig = MetaDataGrid.Rows[e.RowIndex].Cells[1].Tag as EditorTypeConfig;
                MetaDataEntry metaDataEntry = Program.ProjectModel.MetaData.EntryByIndex(e.RowIndex);

                String formattedKey = "";
                String formattedValue = "";

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


                if (e.ColumnIndex == 0)
                {
                    formattedKey = e.FormattedValue.ToString();
                    formattedValue = valStr;

                    if (!MetaDataGrid.IsCurrentCellInEditMode)
                    {
                        formattedKey = keyStr;
                    }

                    Program.ProjectModel.MetaData.Validate(new MetaDataEntry(keyStr, valStr), formattedKey, formattedValue, !MetaDataGrid.IsCurrentCellInEditMode);

                    MetaDataGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].ErrorText = null;
                }

                if (e.ColumnIndex == 1)
                {
                    formattedKey = keyStr;
                    formattedValue = e.FormattedValue.ToString().ToLower().Equals(valStr) ? null : e.FormattedValue.ToString();

                    if (!MetaDataGrid.IsCurrentCellInEditMode)
                    {
                        formattedValue = valStr;
                    }

                    if (editorTypeConfig?.Type == "AutoComplete")
                    {
                        //e.FormattedValue
                        //e.Cancel = true;
                    }
                }
            }
            catch (MetaDataValidationException ve)
            {
                DialogResult dlgResult = DialogResult.OK;
                MetaDataGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].ErrorText = ve.Message;

                if (ve.ShowErrorDialog && MetaDataGrid.IsCurrentCellInEditMode)
                {
                    dlgResult = ApplicationMessage.ShowWarning(ve.Message, "Metadata validation", ApplicationMessage.DialogType.MT_WARNING, ApplicationMessage.DialogButtons.MB_OK | ApplicationMessage.DialogButtons.MB_IGNORE);
                }

                if (ve.RemoveEntry && dlgResult != DialogResult.Ignore)
                {
                    e.Cancel = MetaDataGrid.IsCurrentCellInEditMode;
                }

                if (Win_CBZSettings.Default.LogValidationErrors)
                {
                    MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_ERROR, ve.Message);
                }

                MetaDataGrid.InvalidateCell(e.ColumnIndex, e.RowIndex);
            }
        }

        private void MetaDataGrid_ComboBoxDrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0)
            {
                return;
            }

            if (sender as ComboBox == null)
            {
                return;
            }

            MetaDataFieldType fieldType = ((ComboBox)sender).Tag as MetaDataFieldType;

            Pen pen = new Pen(Color.Black, 1);
            Font font = new Font("Verdana", 9f, FontStyle.Regular);

            if (e.State.HasFlag(DrawItemState.Selected))
            {
                e.Graphics.FillRectangle(new SolidBrush(Theme.GetInstance().AccentColor), e.Bounds);
            }
            else
            {
                e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(255, 255, 255)), e.Bounds);
            }
            if (fieldType != null)
            {
                if (fieldType.AutoCompleteImageKey != null && fieldType.AutoCompleteImageKey.Length > 0)
                {
                    Image img = AutocompleteIcons.Images[fieldType.AutoCompleteImageKey];
                    e.Graphics.DrawImage(img, new Point(e.Bounds.X, e.Bounds.Y));
                    e.Graphics.DrawString(((ComboBox)sender).Items[e.Index].ToString(), font, new SolidBrush(Color.Black), new PointF(e.Bounds.X + 18, e.Bounds.Y + 1));
                }
                else
                {
                    e.Graphics.DrawString(((ComboBox)sender).Items[e.Index].ToString(), font, new SolidBrush(Color.Black), new PointF(e.Bounds.X, e.Bounds.Y + 1));
                }
            }
            else
            {
                e.Graphics.DrawString(((ComboBox)sender).Items[e.Index].ToString(), font, new SolidBrush(Color.Black), new PointF(e.Bounds.X, e.Bounds.Y + 1));
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
            if (MetaDataGrid.SelectedCells.Count == 1)
            {

                MetaDataFieldType fieldType = MetaDataGrid.SelectedCells[0].Tag as MetaDataFieldType;
                if (fieldType != null)
                {

                    if (fieldType.FieldType == MetaDataFieldType.METADATA_FIELD_TYPE_COMBO_BOX)
                    {
                        ComboBox comboBox = e.Control as ComboBox;
                        if (comboBox != null)
                        {
                            Font dgFont = MetaDataGrid.DefaultCellStyle.Font;
                            comboBox.Font = new Font("Verdana", 9f, FontStyle.Regular);
                            comboBox.FlatStyle = FlatStyle.Popup;
                            comboBox.Height = 20;
                            comboBox.DrawMode = DrawMode.OwnerDrawFixed;
                            comboBox.ItemHeight = 18;
                            comboBox.DrawItem += MetaDataGrid_ComboBoxDrawItem;
                            comboBox.Tag = fieldType;
                        }
                    }
                    else if (fieldType.FieldType == MetaDataFieldType.METADATA_FIELD_TYPE_TEXT_BOX)
                    {
                        TextBox textBox = e.Control as TextBox;
                        if (textBox != null)
                        {
                            textBox.Font = new Font("Verdana", 9f, FontStyle.Regular);

                        }
                    }
                    else if (fieldType.FieldType == MetaDataFieldType.METADATA_FIELD_TYPE_AUTO_COMPLETE)
                    {
                        TextBox textBox = e.Control as TextBox;
                        if (textBox != null)
                        {
                            textBox.Font = new Font("Verdana", 9f, FontStyle.Regular);

                            textBox.KeyDown += DataGridTextBoxKeyDown;

                            var items = new List<AutocompleteItem>();
                            foreach (var item in fieldType.EditorConfig.AutoCompleteItems)
                                items.Add(new AutocompleteItem(item) { ImageIndex = AutocompleteIcons.Images.IndexOfKey(fieldType.AutoCompleteImageKey) });

                            if (fieldType.AutoCompleteImageKey == "" || fieldType.AutoCompleteImageKey == null)
                                AutoCompleteItems.LeftPadding = 2;
                            else AutoCompleteItems.LeftPadding = 18;

                            AutoCompleteItems.SetAutocompleteItems(items);
                            AutoCompleteItems.SetAutocompleteMenu(textBox, AutoCompleteItems);
                        }
                    }
                    else
                    {
                        TextBox textBox = e.Control as TextBox;
                        if (textBox != null)
                        {
                            textBox.Font = new Font("Verdana", 9f, FontStyle.Regular);
                            AutoCompleteItems.Items = new string[0];
                            AutoCompleteItems.SetAutocompleteMenu(textBox, AutoCompleteItems);
                        }
                    }
                }
                else
                {
                    TextBox textBox = e.Control as TextBox;
                    if (textBox != null)
                    {
                        textBox.Font = new Font("Verdana", 9f, FontStyle.Regular);
                        AutoCompleteItems.Items = new string[0];
                        AutoCompleteItems.SetAutocompleteMenu(textBox, AutoCompleteItems);
                    }
                }
            }
        }

        private void MetaDataGrid_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {

            DataGridView grid = sender as DataGridView;
            Color sortIconColor = Theme.GetInstance().AccentColor;

            if (e.ColumnIndex > -1 && e.RowIndex == -1)
            {
                DataGridViewColumnHeaderCell headerCell = MetaDataGrid.Columns[e.ColumnIndex].HeaderCell;

                e.PaintBackground(e.CellBounds, false);
                e.Paint(e.CellBounds, DataGridViewPaintParts.ContentForeground);
                e.PaintContent(e.CellBounds);

                /*
                if (grid.SortedColumn?.Index == e.ColumnIndex)
                {
                    var sortIcon = grid.SortOrder == SortOrder.Ascending ? "▲" : "▼";

                    TextRenderer.DrawText(e.Graphics, sortIcon,
                        e.CellStyle.Font, e.CellBounds, sortIconColor,
                        TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
                }
                */

                //MetaDataGrid.ColumnHeadersDefaultCellStyle.

                if (Program.ProjectModel.MetaData.IsColumnFiltered(e.ColumnIndex))
                {
                    e.Graphics.DrawImage(Properties.Resources.funnel, e.CellBounds.Right - 32, e.CellBounds.Y + 4, 16, 16);
                }
                //e.Graphics.DrawString(MetaDataGrid.Columns[e.ColumnIndex].HeaderText, MetaDataGrid.ColumnHeadersDefaultCellStyle.Font, new SolidBrush(Color.Black), new PointF(e.CellBounds.X, e.CellBounds.Y + 5));
                e.Handled = true;
            }


            if (e.ColumnIndex > -1 && e.RowIndex > -1)
            {
                MetaDataFieldType fieldType = MetaDataGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag as MetaDataFieldType;

                if (fieldType != null)
                {
                    e.PaintBackground(e.CellBounds, false);
                    e.Paint(e.CellBounds, DataGridViewPaintParts.Background | DataGridViewPaintParts.SelectionBackground | DataGridViewPaintParts.Border);


                    if (fieldType.FieldType == MetaDataFieldType.METADATA_FIELD_TYPE_RATING)
                    {

                    }
                    else
                    {
                        e.PaintContent(e.CellBounds);
                    }

                    e.Handled = true;
                }
            }
        }

        private void DataGridTextBoxKeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                e.Handled = true;
            }
        }

        private void AddMetaData()
        {
            Invoke(new Action(() => { BtnAddMetaData.Enabled = false; }));

            try
            {
                Program.ProjectModel.NewMetaData(true, Win_CBZSettings.Default.MetaDataFilename);
            }
            catch (MetaDataValidationException ve)
            {
                MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_WARNING, "Failed to parse default metadata entry ['" + ve.Item.Key + "']!  [" + ve.Message + "]");

                if (ve.ShowErrorDialog)
                {
                    ApplicationMessage.ShowWarning("Failed to create Metadata.\n" + ve.Message, "Metadata Error", ApplicationMessage.DialogType.MT_WARNING, ApplicationMessage.DialogButtons.MB_OK);
                }
            }
            catch (Exception e)
            {
                MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_WARNING, "Failed to parse default metadata entry! [" + e.Message + "]");
            }

            if (ApplyUserKeyFilter)
            {
                Program.ProjectModel.MetaData.UserFilterMetaData(Win_CBZSettings.Default.KeyFilter?.OfType<string>().ToArray(), Win_CBZSettings.Default.KeyFilterBaseContitionType).FilterMetaData(ToolBarSearchInput.Text);
            }
            else
            {
                Program.ProjectModel.MetaData.UserFilterMetaData().FilterMetaData(ToolBarSearchInput.Text);
            }


            AppEventHandler.OnMetaDataLoaded(this, new MetaDataLoadEvent(Program.ProjectModel.MetaData.Values.ToList()));

            Task updateIndex = UpdateMetadataTask.UpdatePageMetadata(new List<Page>(Program.ProjectModel.Pages.ToArray()),
                            Program.ProjectModel.MetaData,
                            MetaDataVersionFlavorHandler.GetInstance().HandlePageIndexVersion(),
                            AppEventHandler.OnGeneralTaskProgress,
                            AppEventHandler.OnPageChanged,
                            TokenStore.GetInstance().RequestCancellationToken(TokenStore.TOKEN_SOURCE_REBUILD_XML_INDEX),
                            true);

            updateIndex.ContinueWith((t) =>
            {
                Invoke(new Action(() =>
                {
                    AddMetaDataRowBtn.Enabled = true;
                    RemoveMetadataRowBtn.Enabled = false;
                    ToolStripButtonShowRawMetadata.Enabled = true;

                    //AppEventHandler.OnGeneralTaskProgress(this, new GeneralTaskProgressEvent());

                    TextBoxCountKeys.Text = Program.ProjectModel.MetaData.Values.Count.ToString();

                    AppEventHandler.OnApplicationStateChanged(this, new ApplicationStatusEvent(Program.ProjectModel, ApplicationStatusEvent.STATE_READY));
                }));
            });

            updateIndex.Start();
        }

        private void RemoveMetaData()
        {
            if (Program.ProjectModel.MetaData != null)
            {
                AppEventHandler.OnMetaDataChanged(this, new MetaDataChangedEvent(MetaDataChangedEvent.METADATA_DELETED, Program.ProjectModel.MetaData));
                AppEventHandler.OnArchiveStatusChanged(null, new ArchiveStatusEvent(Program.ProjectModel, ArchiveStatusEvent.ARCHIVE_METADATA_DELETED));

            }
        }

        private void MetaDataLoaded(object sender, MetaDataLoadEvent e)
        {
            if (!WindowClosed)
            {
                TextBoxCountKeys.Invoke(new Action(() =>
                {
                    TextBoxCountKeys.Text = Program.ProjectModel.MetaData.Values.Count.ToString();
                }));

                Task fillDataGrid = new Task(() =>
                {

                    //MetaDataGrid.DataSource = e.MetaData;
                    Invoke(new Action(() =>
                    {
                        ButtonFilter.Enabled = false;
                        BtnAddMetaData.Enabled = false;
                        BtnRemoveMetaData.Enabled = true;
                        ToolStripButtonShowRawMetadata.Enabled = true;
                        AddMetaDataRowBtn.Enabled = true;
                    }));

                    DataGridViewColumn firstCol = MetaDataGrid.Columns.GetFirstColumn(DataGridViewElementStates.Visible);
                    if (firstCol != null)
                    {
                        DataGridViewColumn secondCol = MetaDataGrid.Columns.GetNextColumn(firstCol, DataGridViewElementStates.Visible, DataGridViewElementStates.None);
                        if (secondCol != null)
                        {
                            Invoke(new Action(() =>
                            {
                                firstCol.Width = 150;
                                secondCol.Width = 250;
                                firstCol.SortMode = DataGridViewColumnSortMode.Automatic;
                                secondCol.SortMode = DataGridViewColumnSortMode.Automatic;
                            }));
                        }
                    }
                    else
                    {
                        MetaDataGrid.Invoke(new Action(() =>
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
                        }));
                    }

                    MetaDataGrid.Invoke(new Action(() =>
                    {
                        MetaDataGrid.Rows.Clear();

                        foreach (MetaDataEntry entry in e.MetaData)
                        {
                            if (entry.Visible && !entry.UserFiltered)
                            {
                                MetaDataGrid.Rows.Add(entry.Key, entry.Value, null, e.MetaData.IndexOf(entry));
                            }
                        }
                    }));

                    int[] colIndices = new int[] { 0, 2 };
                    Color selectionColor = Theme.GetInstance().AccentColor;

                    // DataGridViewCellStyle currentStyle = null;
                    for (int i = 0; i < MetaDataGrid.RowCount; i++)
                    {

                        //currentStyle = new DataGridViewCellStyle(MetaDataGrid.Rows[i].Cells[2].Style);
                        MetaDataGrid.Rows[i].Cells[2].ReadOnly = true;


                        foreach (MetaDataEntry entry in e.MetaData)
                        {
                            var key = MetaDataGrid.Rows[i].Cells[0].Value;

                            if (key != null && entry.Visible && !entry.UserFiltered)
                            {
                                if (entry.Key == key.ToString())
                                {
                                    DataGridViewTextBoxCell k = new DataGridViewTextBoxCell();
                                    k.Value = entry.Key;
                                    k.Tag = entry;

                                    MetaDataGrid.Invoke(new Action(() => MetaDataGrid.Rows[i].Cells[0] = k));

                                    MetaDataFieldConfig.GetInstance().UpdateAutoCompleteOptions(entry.Key, entry.ValueAsList());
                                    if (entry.Type.FieldType == MetaDataFieldType.METADATA_FIELD_TYPE_COMBO_BOX)
                                    {
                                        bool isAutoComplete = entry.Type.FieldType == MetaDataFieldType.METADATA_FIELD_TYPE_AUTO_COMPLETE;

                                        int selectedIndex = Array.IndexOf(entry.Type.OptionsAsList(), entry.Value);
                                        DataGridViewComboBoxCell c = new DataGridViewComboBoxCell();
                                        c.Items.AddRange(entry.Type.OptionsAsList());

                                        c.Value = entry.Value; // selectedIndex > -1 ? selectedIndex : 0;
                                        c.Tag = entry.Type; //  new EditorConfig("ComboBox", "String", "", " ", false);
                                                            //c.AutoComplete = isAutoComplete;
                                                            //c.DataSource = new List<String>(entry.Options.EditorOptions);
                                        c.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                                        //c.DisplayStyle = isAutoComplete ? DataGridViewComboBoxDisplayStyle.DropDownButton : DataGridViewComboBoxDisplayStyle.ComboBox;
                                        c.DisplayStyleForCurrentCellOnly = false;

                                        c.Style = new DataGridViewCellStyle()
                                        {
                                            Font = new Font("Verdana", 9f, FontStyle.Regular),
                                            SelectionBackColor = ((i + 1) % 2 != 0) ? Color.White : Color.FromKnownColor(KnownColor.ControlLight),
                                            ForeColor = Color.Black,
                                            BackColor = ((i + 1) % 2 != 0) ? Color.White : Color.FromKnownColor(KnownColor.ControlLight),
                                            //Font = new Font("Tahoma", 10f, FontStyle.Regular),
                                        };

                                        MetaDataGrid.Invoke(new Action(() => MetaDataGrid.Rows[i].Cells[1] = c));
                                    }
                                    else if (entry.Type.FieldType == MetaDataFieldType.METADATA_FIELD_TYPE_AUTO_COMPLETE)
                                    {
                                        DataGridViewTextBoxCell c = new DataGridViewTextBoxCell();
                                        //c.Items.AddRange(entry.Options.EditorOptions);
                                        c.Value = entry.Value; // selectedIndex > -1 ? selectedIndex : 0;
                                        c.Tag = entry.Type;  // new EditorConfig("AutoComplete", "String", "", " ", false, entry.Options.EditorOptions);

                                        c.Style = new DataGridViewCellStyle()
                                        {
                                            Font = new Font("Verdana", 9f, FontStyle.Regular),
                                            SelectionBackColor = Theme.GetInstance().AccentColor,
                                            SelectionForeColor = Color.Black,
                                            BackColor = ((i + 1) % 2 != 0) ? Color.White : Color.FromKnownColor(KnownColor.ControlLight),
                                        };
                                        //c. = isAutoComplete;
                                        //c.DataSource = new List<String>(entry.Options.EditorOptions);

                                        //c.DisplayStyle = isAutoComplete ? DataGridViewComboBoxDisplayStyle.DropDownButton : DataGridViewComboBoxDisplayStyle.ComboBox;
                                        //c.DisplayStyleForCurrentCellOnly = isAutoComplete;


                                        MetaDataGrid.Invoke(new Action(() => MetaDataGrid.Rows[i].Cells[1] = c));
                                    }
                                    else if (entry.Type.FieldType == MetaDataFieldType.METADATA_FIELD_TYPE_RATING)
                                    {
                                        DataGridViewImageCell c = new DataGridViewImageCell();

                                        c.Value = entry.Value;
                                        c.Tag = entry.Type;
                                        c.ImageLayout = DataGridViewImageCellLayout.Normal;

                                        MetaDataGrid.Invoke(new Action(() => MetaDataGrid.Rows[i].Cells[1] = c));
                                        c.ReadOnly = true;
                                    }
                                    else
                                    {
                                        DataGridViewTextBoxCell c = new DataGridViewTextBoxCell();
                                        c.Value = entry.Value;
                                        c.Tag = entry.Type;
                                        c.Style = new DataGridViewCellStyle()
                                        {
                                            Font = new Font("Verdana", 9f, FontStyle.Regular),
                                            SelectionBackColor = Theme.GetInstance().AccentColor,
                                            ForeColor = Color.Black,
                                            BackColor = ((i + 1) % 2 != 0) ? Color.White : Color.FromKnownColor(KnownColor.ControlLight),
                                        };

                                        MetaDataGrid.Invoke(new Action(() => MetaDataGrid.Rows[i].Cells[1] = c));

                                        //MetaDataGrid.Invoke(new Action(() => MetaDataGrid.Rows[i].Cells[1].Tag = entry.Type));
                                        //MetaDataGrid.Invoke(new Action(() => MetaDataGrid.Rows[i].Cells[1].Value = entry.Value));
                                    }

                                    if (entry.Type.EditorType != EditorTypeConfig.EDITOR_TYPE_NONE)
                                    {
                                        string btnCaption = "...";

                                        if (entry.Type.EditorType == EditorTypeConfig.EDITOR_TYPE_VARIABLE_EDITOR)
                                        {
                                            btnCaption = "$";
                                        }

                                        if (entry.Type.EditorType == EditorTypeConfig.EDITOR_TYPE_ROMAJI_EDITOR)
                                        {
                                            btnCaption = "?";
                                        }

                                        DataGridViewButtonCell bc = new DataGridViewButtonCell
                                        {
                                            Value = btnCaption,
                                            Tag = entry.Type, // new EditorTypeConfig(EditorTypeConfig.EDITOR_TYPE_MULTI_LINE_TEXT_EDITOR, "String", ",", " ", false, entry.Type.OptionsAsList()),
                                            Style = new DataGridViewCellStyle()
                                            {
                                                SelectionForeColor = Color.White,
                                                SelectionBackColor = Color.White,
                                                ForeColor = Color.Black,
                                                BackColor = Color.White,
                                            },
                                            ToolTipText = entry.Type.EditorConfig.ToString(),
                                        };

                                        MetaDataGrid.Invoke(new Action(() => MetaDataGrid.Rows[i].Cells[2] = bc));
                                    }

                                }
                                else
                                {
                                    //MetaDataGrid.Rows[i].Cells[1] = c;
                                }
                                //c.ReadOnly = true;          
                            }

                            //Thread.Sleep(2);
                        }

                        foreach (int colIndex in colIndices)
                        {
                            selectionColor = Theme.GetInstance().AccentColor;

                            if (MetaDataGrid.Rows[i].Cells[colIndex] is DataGridViewButtonCell)
                            {
                                selectionColor = ((i + 1) % 2 != 0) ? Color.White : Color.FromKnownColor(KnownColor.ControlLight);
                            }

                            MetaDataGrid.Rows[i].Cells[colIndex].Style = new DataGridViewCellStyle()
                            {
                                Font = new Font("Verdana", 9f, FontStyle.Regular),
                                SelectionBackColor = selectionColor,
                                ForeColor = Color.Black,
                                BackColor = ((i + 1) % 2 != 0) ? Color.White : Color.FromKnownColor(KnownColor.ControlLight),
                            };
                        }
                        //Task.Sleep(2);
                    }

                });

                fillDataGrid.ContinueWith((t) =>
                {
                    if (t.IsCompletedSuccessfully)
                    {
                        //
                    }

                    Invoke(new Action(() =>
                    {
                        ButtonFilter.Enabled = true;
                    }));
                });

                fillDataGrid.Start();
            }
        }

        private void MetaDataChanged(object sender, MetaDataChangedEvent e)
        {
            if (!WindowClosed)
            {
                Invoke(new Action(() =>
                {
                    Program.ProjectModel.IsChanged = true;

                    if (Program.ProjectModel.FileName != null)
                    {
                        TextBoxCountKeys.Text = e.Data.Values.Count.ToString();
                        ToolButtonSave.Enabled = true; // Program.ProjectModel.FileName != null && Program.ProjectModel.FileName.Length > 0;
                        SaveToolStripMenuItem.Enabled = true; // Program.ProjectModel.FileName != null && Program.ProjectModel.FileName.Length > 0;
                    }
                }));

                if (e.State == MetaDataChangedEvent.METADATA_NEW ||
                    e.State == MetaDataChangedEvent.METADATA_DELETED)
                {
                    Invoke(new Action(() =>
                    {
                        BtnAddMetaData.Enabled = e.State == MetaDataChangedEvent.METADATA_NEW;
                        BtnRemoveMetaData.Enabled = e.State == MetaDataChangedEvent.METADATA_DELETED;

                    }));
                }

                if (e.State == MetaDataChangedEvent.METADATA_DELETED)
                {
                    e.Data.Values.Clear();
                    MetaDataGrid.DataSource = null;

                    if (!WindowClosed)
                    {
                        Invoke(new Action(() =>
                        {
                            MetaDataGrid.Rows.Clear();
                            MetaDataGrid.Columns.Clear();


                            BtnAddMetaData.Enabled = true;
                            BtnRemoveMetaData.Enabled = false;
                            AddMetaDataRowBtn.Enabled = false;
                            RemoveMetadataRowBtn.Enabled = false;
                            ToolStripButtonShowRawMetadata.Enabled = false;
                        }));

                        TextBoxCountKeys.Invoke(new Action(() =>
                        {
                            TextBoxCountKeys.Text = e.Data.Values.Count.ToString();
                        }));
                    }

                }

                if (e.State == MetaDataChangedEvent.METADATA_NEW)
                {

                }
            }
        }

        private void MetaDataEntryChanged(object sender, MetaDataEntryChangedEvent e)
        {
            if (!WindowClosed)
            {
                TextBoxCountKeys.Invoke(new Action(() =>
                {
                    TextBoxCountKeys.Text = Program.ProjectModel.MetaData.Values.Count.ToString();
                }));

                MetaDataGrid.Invoke(new Action(() =>
                {

                    Program.ProjectModel.IsChanged = true;

                    if (Program.ProjectModel.FileName != null)
                    {

                        AppEventHandler.OnArchiveStatusChanged(sender, new ArchiveStatusEvent(Program.ProjectModel, ArchiveStatusEvent.ARCHIVE_METADATA_CHANGED));

                    }

                    if (e.State == MetaDataEntryChangedEvent.ENTRY_NEW)
                    {

                        foreach (DataGridViewRow r in MetaDataGrid.SelectedRows)
                        {
                            r.Selected = false;
                        }

                        try
                        {
                            int newRowIndex = MetaDataGrid.Rows.Add(e.Entry.Key, e.Entry.Value, "");
                            MetaDataGrid.Rows[newRowIndex].Cells[2].ReadOnly = true;
                            //---

                            if (e.Entry.Key == "" && e.Entry.Value == null)
                            {

                                MetaDataGrid.CurrentCell = MetaDataGrid.Rows[newRowIndex].Cells[0];
                                MetaDataGrid.CurrentCell.Tag = e.Entry;
                                MetaDataGrid.BeginEdit(false);
                            }
                        }
                        catch (Exception e1)
                        {
                            AppEventHandler.OnMessageLogged(sender, new LogMessageEvent(LogMessageEvent.LOGMESSAGE_TYPE_WARNING, e1.Message));
                        }
                    }

                    if (e.State == MetaDataEntryChangedEvent.ENTRY_DELETED)
                    {
                        if (e.Index > -1)
                        {
                            if (ToolBarSearchInput.Text.Length == 0 && !ApplyUserKeyFilter)
                            {
                                try
                                {
                                    MetaDataGrid.Rows.RemoveAt(e.Index);
                                }
                                catch (Exception e)
                                {
                                    AppEventHandler.OnMessageLogged(sender, new LogMessageEvent(LogMessageEvent.LOGMESSAGE_TYPE_WARNING, "MainForm::MetaDataEntryChanged(), Error removing row! [" + e.Message + "]"));
                                }
                            }
                        }
                        else
                        {
                            AppEventHandler.OnMessageLogged(sender, new LogMessageEvent(LogMessageEvent.LOGMESSAGE_TYPE_WARNING, "MainForm::MetaDataEntryChanged(), index < 0!"));
                        }
                    }
                }));
            }
        }

        private void MetaDataGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            var senderGrid = (DataGridView)sender;

            if (e.RowIndex < 0 || e.ColumnIndex < 0)
            {
                return;
            }

            if (e.ColumnIndex == 0)
            {
                if (Win_CBZSettings.Default.MetadataGridInstantEditMode && !Win_CBZSettings.Default.MetadataGridInstantEditModeValueCol)
                {
                    senderGrid.BeginEdit(false);
                }
            }

            if (e.ColumnIndex == 1)
            {
                if (senderGrid.Rows[e.RowIndex].Cells[e.ColumnIndex] is DataGridViewComboBoxCell)
                {
                    // && fieldType.FieldType == MetaDataFieldType.METADATA_FIELD_TYPE_COMBO_BOX) {
                    DataGridViewComboBoxCell comboCell = senderGrid.Rows[e.RowIndex].Cells[e.ColumnIndex] as DataGridViewComboBoxCell;
                    senderGrid.BeginEdit(true);
                }
                else if (senderGrid.Rows[e.RowIndex].Cells[e.ColumnIndex] is DataGridViewTextBoxCell)
                { // && fieldType.FieldType == MetaDataFieldType.METADATA_FIELD_TYPE_AUTO_COMPLETE) {
                  //DataGridViewTextBoxCell textCell = senderGrid.Rows[e.RowIndex].Cells[e.ColumnIndex] as DataGridViewTextBoxCell;
                  //value = textCell.Value?.ToString();
                    if (Win_CBZSettings.Default.MetadataGridInstantEditMode)
                    {
                        senderGrid.BeginEdit(false);
                    }
                }

            }
        }

        private void MetaDataGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (e.RowIndex < 0 || e.ColumnIndex < 0)
            {
                return;
            }

            //if (senderGrid.Rows[e.RowIndex].Cells[e.ColumnIndex] is DataGridViewButtonCell ||
            //    senderGrid.Rows[e.RowIndex].Cells[e.ColumnIndex] is DataGridViewComboBoxCell)
            //{
            String value = "";
            MetaDataFieldType fieldType = senderGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag as MetaDataFieldType;
            if (e.ColumnIndex == 2)
            {
                value = senderGrid.Rows[e.RowIndex].Cells[e.ColumnIndex - 1].Value?.ToString();
            }
            else
            {
                value = senderGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value?.ToString();
            }

            if (e.ColumnIndex == 1)
            {
                if (senderGrid.Rows[e.RowIndex].Cells[e.ColumnIndex] is DataGridViewComboBoxCell)
                {
                    // && fieldType.FieldType == MetaDataFieldType.METADATA_FIELD_TYPE_COMBO_BOX) {
                    DataGridViewComboBoxCell comboCell = senderGrid.Rows[e.RowIndex].Cells[e.ColumnIndex] as DataGridViewComboBoxCell;
                    senderGrid.BeginEdit(true);
                }
                else if (senderGrid.Rows[e.RowIndex].Cells[e.ColumnIndex] is DataGridViewTextBoxCell)
                { // && fieldType.FieldType == MetaDataFieldType.METADATA_FIELD_TYPE_AUTO_COMPLETE) {
                  //DataGridViewTextBoxCell textCell = senderGrid.Rows[e.RowIndex].Cells[e.ColumnIndex] as DataGridViewTextBoxCell;
                  //value = textCell.Value?.ToString();
                }

            }

            if (e.ColumnIndex == 2 && fieldType != null)
            {
                fieldType.EditorConfig.Value = value;

                if (fieldType.FieldType == MetaDataFieldType.METADATA_FIELD_TYPE_RATING)
                {
                    senderGrid.Rows[e.RowIndex].Cells[e.ColumnIndex - 1].Value = 5;
                }

                switch (fieldType.EditorType)
                {
                    case EditorTypeConfig.EDITOR_TYPE_MULTI_LINE_TEXT_EDITOR:
                        {
                            TextEditorForm textEditor = new TextEditorForm(fieldType.EditorConfig);
                            DialogResult r = textEditor.ShowDialog();
                            if (r == DialogResult.OK)
                            {
                                if (textEditor.Config.Result != null)
                                {
                                    senderGrid.Rows[e.RowIndex].Cells[e.ColumnIndex - 1].Value = textEditor.Config.Result.ToString();
                                    // todo: evaluate-> dont directly update autocompletes... only after saving?
                                    //MetaDataFieldConfig.GetInstance().UpdateAutoCompleteOptions(fieldType.Name, textEditor.config.Result.ToString().Split(','));
                                }
                            }
                        }
                        break;
                    case EditorTypeConfig.EDITOR_TYPE_TAG_EDITOR:
                        {
                            TagEditorForm textEditor = new TagEditorForm(fieldType.EditorConfig);
                            DialogResult r = textEditor.ShowDialog();
                            if (r == DialogResult.OK)
                            {
                                if (textEditor.Config.Result != null)
                                {
                                    senderGrid.Rows[e.RowIndex].Cells[e.ColumnIndex - 1].Value = textEditor.Config.Result.ToString();
                                    // todo: evaluate-> dont directly update autocompletes... only after saving?
                                    //MetaDataFieldConfig.GetInstance().UpdateAutoCompleteOptions(fieldType.Name, textEditor.config.Result.ToString().Split(','));
                                }
                            }
                        }
                        break;
                    case EditorTypeConfig.EDITOR_TYPE_LANGUAGE_EDITOR:
                        {
                            LanguageEditorForm langEditor = new LanguageEditorForm(fieldType.EditorConfig);
                            DialogResult r = langEditor.ShowDialog();
                            if (r == DialogResult.OK)
                            {
                                if (langEditor.config.Result != null)
                                {
                                    senderGrid.Rows[e.RowIndex].Cells[e.ColumnIndex - 1].Value = langEditor.config.Result.ToString();
                                    // todo: evaluate-> dont directly update autocompletes... only after saving?
                                    //MetaDataFieldConfig.GetInstance().UpdateAutoCompleteOptions(fieldType.Name, langEditor.config.Result.ToString());
                                }
                            }
                        }
                        break;
                    default:
                        {
                            if (fieldType.FieldType == MetaDataFieldType.METADATA_FIELD_TYPE_COMBO_BOX)
                            {
                                DataGridViewComboBoxCell comboCell = senderGrid.Rows[e.RowIndex].Cells[e.ColumnIndex] as DataGridViewComboBoxCell;
                                comboCell.Style = new DataGridViewCellStyle()
                                {
                                    Font = new Font("Verdana", 9f, FontStyle.Regular),
                                    SelectionForeColor = Color.Black,
                                    SelectionBackColor = Theme.GetInstance().AccentColor,
                                    BackColor = Color.White,
                                };


                            }
                            else if (fieldType.FieldType == MetaDataFieldType.METADATA_FIELD_TYPE_AUTO_COMPLETE)
                            {
                                DataGridViewTextBoxCell textCell = senderGrid.Rows[e.RowIndex].Cells[e.ColumnIndex] as DataGridViewTextBoxCell;

                            }


                            MetaDataGrid.BeginEdit(false);

                        }
                        break;

                }
            }
            //}
        }

        private void MetaDataGrid_Sorted(object sender, EventArgs e)
        {
            //Program.ProjectModel.MetaData.Values.Clear();

            /*
            foreach (DataGridViewRow row in MetaDataGrid.Rows)
            {
                var key = row.Cells[0].Value;
                var val = row.Cells[1].Value;

                //Program.ProjectModel.MetaData.Add(row.Cells[0].Tag as MetaDataEntry);
            }
            */

            int sortedColIndex = MetaDataGrid.SortedColumn.Index;
            SortOrder sortOrder = MetaDataGrid.SortOrder;

            switch (sortedColIndex)
            {
                case 0:
                    {
                        Program.ProjectModel.MetaData.SortByKey(sortOrder);
                    }
                    break;
                case 1:
                    {
                        Program.ProjectModel.MetaData.SortByValue(sortOrder);
                    }
                    break;
                default:
                    {
                        Program.ProjectModel.MetaData.RemoveSort();
                    }
                    break;
            }

            Color selectionColor = Theme.GetInstance().AccentColor;

            for (int i = 0; i < MetaDataGrid.RowCount; i++)
            {
                for (int j = 0; j < MetaDataGrid.ColumnCount; j++)
                {

                    MetaDataFieldType metaDataFieldType = MetaDataGrid.Rows[i].Cells[j].Tag as MetaDataFieldType;

                    if (metaDataFieldType != null)
                    {
                        switch (metaDataFieldType.FieldType)
                        {
                            case MetaDataFieldType.METADATA_FIELD_TYPE_COMBO_BOX:
                                {
                                    selectionColor = ((i + 1) % 2 != 0) ? Color.White : Color.FromKnownColor(KnownColor.ControlLight);
                                }
                                break;
                            default:
                                {
                                    selectionColor = Theme.GetInstance().AccentColor;
                                }
                                break;
                        }
                    }

                    if (MetaDataGrid.Rows[i].Cells[j] is DataGridViewButtonCell)
                    {
                        selectionColor = ((i + 1) % 2 != 0) ? Color.White : Color.FromKnownColor(KnownColor.ControlLight);
                    }

                    MetaDataGrid.Rows[i].Cells[j].Style = new DataGridViewCellStyle()
                    {
                        SelectionBackColor = selectionColor,
                        ForeColor = Color.Black,
                        BackColor = ((i + 1) % 2 != 0) ? Color.White : Color.FromKnownColor(KnownColor.ControlLight),
                    };

                    // reset color
                    selectionColor = Theme.GetInstance().AccentColor;
                }
            }
        }

        private void ToolStripMenuItemDataGridRemoveSort_Click(object sender, EventArgs e)
        {
            if (MetaDataGrid.Columns.Count > 0)
            {

                MetaDataGrid.Rows.Clear();
                MetaDataGrid.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
                MetaDataGrid.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;

                Program.ProjectModel.MetaData.RemoveSort();

                AppEventHandler.OnMetaDataLoaded(this, new MetaDataLoadEvent(Program.ProjectModel.MetaData.Values.ToList()));
            }
        }

        private void MetaDataGrid_ColumnSortModeChanged(object sender, DataGridViewColumnEventArgs e)
        {

        }

        private void MetaDataGrid_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                string Key = "";
                string Val = "";

                object value = MetaDataGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
                int realIndex = Program.ProjectModel.MetaData.Values.IndexOf(MetaDataGrid.Rows[e.RowIndex].Cells[0].Tag as MetaDataEntry);

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

                if (e.ColumnIndex == 0)
                {
                    Program.ProjectModel.MetaData.Validate(new MetaDataEntry(Key, Val), Key, Val, false);
                }
                else
                {
                    Program.ProjectModel.MetaData.Validate(new MetaDataEntry(Key, Val), null, Val, false);
                }
            }
            catch (MetaDataValidationException ve)
            {
                //MetaDataGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].ErrorText = ve.Message;
                //MetaDataGrid.Rows[e.RowIndex].Cells[e.ColumnIndex]. = ve.Message;

                MetaDataGrid.InvalidateCell(e.ColumnIndex, e.RowIndex);

            }
            catch (Exception ex)
            {
                MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_ERROR, ex.Message);
            }

        }

        private void MetaDataGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {

                Program.ProjectModel.IsChanged = true;

                ToolButtonSave.Enabled = true; //Program.ProjectModel.FileName != null && Program.ProjectModel.FileName.Length > 0;
                SaveToolStripMenuItem.Enabled = true; // Program.ProjectModel.FileName != null && Program.ProjectModel.FileName.Length > 0;

                Program.ProjectModel.IsChanged = true;

                string Key = "";
                string Val = "";

                object value = MetaDataGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
                int realIndex = Program.ProjectModel.MetaData.Values.IndexOf(MetaDataGrid.Rows[e.RowIndex].Cells[0].Tag as MetaDataEntry);

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

                MetaDataEntry updatedEntry = Program.ProjectModel.MetaData.UpdateEntry(realIndex, new MetaDataEntry(Key, Val));
                //MetaDataGrid.Rows[e.RowIndex].ErrorText = null;


                //if (updatedEntry.Type.FieldType == MetaDataFieldType.METADATA_FIELD_TYPE_AUTO_COMPLETE)
                // {

                // } else
                // {

                // }

                // todo: evaluate maybe only update upon saving cbz
                MetaDataFieldConfig.GetInstance().UpdateAutoCompleteOptions(updatedEntry.Type.Name, updatedEntry.Value.Split(','));


                if (e.ColumnIndex == 0)
                {
                    var key = MetaDataGrid.Rows[e.RowIndex].Cells[0].Value;
                    if (key != null)
                    {
                        if (updatedEntry.Key == key.ToString())
                        {
                            MetaDataGrid.Rows[e.RowIndex].Cells[2].ReadOnly = true;
                            if (updatedEntry.Type.FieldType == MetaDataFieldType.METADATA_FIELD_TYPE_COMBO_BOX)
                            {
                                if (updatedEntry.Type.Options.Length > 0)
                                {
                                    int selectedIndex = Array.IndexOf(updatedEntry.Type.OptionsAsList(), updatedEntry.Value);
                                    //bool isAutoComplete = updatedEntry.Options.FieldType == EditorFieldMapping.METADATA_FIELD_TYPE_AUTO_COMPLETE;

                                    DataGridViewComboBoxCell c = new DataGridViewComboBoxCell();
                                    c.Items.AddRange(updatedEntry.Type.OptionsAsList());
                                    c.AutoComplete = false;
                                    //c.DataSource = new List<String>(updatedEntry.Options.EditorOptions);


                                    c.Value = value; //selectedIndex > -1 ? selectedIndex : 0;
                                    c.Tag = updatedEntry.Type;
                                    c.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                                    c.DisplayStyleForCurrentCellOnly = false;
                                    c.Style = new DataGridViewCellStyle()
                                    {
                                        Font = new Font("Verdana", 9f, FontStyle.Regular),
                                        SelectionForeColor = Color.Black,
                                        SelectionBackColor = Theme.GetInstance().AccentColor,
                                        BackColor = ((e.RowIndex + 1) % 2 != 0) ? Color.White : Color.FromKnownColor(KnownColor.ControlLight),
                                    };

                                    //c.DisplayStyle = isAutoComplete ? DataGridViewComboBoxDisplayStyle.DropDownButton : DataGridViewComboBoxDisplayStyle.ComboBox;
                                    //c.DisplayStyleForCurrentCellOnly = isAutoComplete;


                                    MetaDataGrid.Rows[e.RowIndex].Cells[1] = c;
                                    //c.ReadOnly = true;
                                }
                                else
                                {
                                    DataGridViewTextBoxCell c = new DataGridViewTextBoxCell
                                    {
                                        Style = new DataGridViewCellStyle()
                                        {
                                            Font = new Font("Verdana", 9f, FontStyle.Regular),
                                            SelectionForeColor = Color.Black,
                                            SelectionBackColor = Theme.GetInstance().AccentColor,
                                            BackColor = ((e.RowIndex + 1) % 2 != 0) ? Color.White : Color.FromKnownColor(KnownColor.ControlLight),
                                        },
                                        Value = updatedEntry.Value,
                                        Tag = updatedEntry.Type,
                                    };

                                    MetaDataGrid.Rows[e.RowIndex].Cells[1] = c;
                                }
                            }
                            else if (updatedEntry.Type.FieldType == MetaDataFieldType.METADATA_FIELD_TYPE_AUTO_COMPLETE)
                            {
                                DataGridViewTextBoxCell c = new DataGridViewTextBoxCell();
                                //c.Items.AddRange(entry.Options.EditorOptions);
                                c.Value = updatedEntry.Value; // selectedIndex > -1 ? selectedIndex : 0;
                                c.Tag = updatedEntry.Type;

                                //c. = isAutoComplete;
                                //c.DataSource = new List<String>(entry.Options.EditorOptions);

                                //c.DisplayStyle = isAutoComplete ? DataGridViewComboBoxDisplayStyle.DropDownButton : DataGridViewComboBoxDisplayStyle.ComboBox;
                                //c.DisplayStyleForCurrentCellOnly = isAutoComplete;
                                c.Style = new DataGridViewCellStyle()
                                {
                                    Font = new Font("Verdana", 9f, FontStyle.Regular),
                                    SelectionForeColor = Color.Black,
                                    SelectionBackColor = Theme.GetInstance().AccentColor,
                                    BackColor = ((e.RowIndex + 1) % 2 != 0) ? Color.White : Color.FromKnownColor(KnownColor.ControlLight),
                                };

                                MetaDataGrid.Rows[e.RowIndex].Cells[1] = c;
                            }
                            else if (updatedEntry.Type.FieldType == MetaDataFieldType.METADATA_FIELD_TYPE_RATING)
                            {
                                DataGridViewImageCell c = new DataGridViewImageCell();

                                c.Value = updatedEntry.Value;
                                c.Tag = updatedEntry.Type;
                                c.ImageLayout = DataGridViewImageCellLayout.Normal;

                                c.Style = new DataGridViewCellStyle()
                                {
                                    Font = new Font("Verdana", 9f, FontStyle.Regular),
                                    SelectionForeColor = Color.Black,
                                    SelectionBackColor = Theme.GetInstance().AccentColor,
                                    BackColor = ((e.RowIndex + 1) % 2 != 0) ? Color.White : Color.FromKnownColor(KnownColor.ControlLight),
                                };

                                MetaDataGrid.Rows[e.RowIndex].Cells[1] = c;
                                c.ReadOnly = true;
                            }
                            else
                            {
                                DataGridViewTextBoxCell c = new DataGridViewTextBoxCell();

                                c.Value = updatedEntry.Value;
                                c.Tag = updatedEntry.Type;

                                c.Style = new DataGridViewCellStyle()
                                {
                                    Font = new Font("Verdana", 9f, FontStyle.Regular),
                                    SelectionForeColor = Color.Black,
                                    SelectionBackColor = Theme.GetInstance().AccentColor,
                                    BackColor = ((e.RowIndex + 1) % 2 != 0) ? Color.White : Color.FromKnownColor(KnownColor.ControlLight),
                                };

                                MetaDataGrid.Rows[e.RowIndex].Cells[1] = c;
                            }

                            if (updatedEntry.Type.EditorType != EditorTypeConfig.EDITOR_TYPE_NONE)
                            {
                                DataGridViewButtonCell bc = new DataGridViewButtonCell
                                {
                                    Value = "...",
                                    Tag = updatedEntry.Type,
                                    Style = new DataGridViewCellStyle()
                                    {
                                        SelectionForeColor = Color.White,
                                        SelectionBackColor = Color.White,
                                        ForeColor = Color.Black,
                                        BackColor = Color.White,
                                    }
                                };


                                MetaDataGrid.Rows[e.RowIndex].Cells[2] = bc;
                            }
                            else
                            {
                                DataGridViewTextBoxCell bc = new DataGridViewTextBoxCell();

                                bc.Value = "";
                                bc.Tag = null;

                                bc.Style = new DataGridViewCellStyle()
                                {
                                    Font = new Font("Verdana", 9f, FontStyle.Regular),
                                    SelectionForeColor = Color.Black,
                                    SelectionBackColor = Theme.GetInstance().AccentColor,
                                    BackColor = ((e.RowIndex + 1) % 2 != 0) ? Color.White : Color.FromKnownColor(KnownColor.ControlLight),
                                };

                                MetaDataGrid.Rows[e.RowIndex].Cells[2] = bc;
                                MetaDataGrid.Rows[e.RowIndex].Cells[2].ReadOnly = true;
                            }
                        }
                    }

                    MetaDataGrid.Invalidate();
                }
            }
            catch (MetaDataValidationException ve)
            {
                DialogResult dlgResult = DialogResult.OK;
                MetaDataGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].ErrorText = ve.Message;

                if (ve.ShowErrorDialog && MetaDataGrid.IsCurrentCellInEditMode)
                {
                    dlgResult = ApplicationMessage.ShowWarning(ve.Message, "Metadata validation", ApplicationMessage.DialogType.MT_WARNING, ApplicationMessage.DialogButtons.MB_OK | ApplicationMessage.DialogButtons.MB_IGNORE);
                }

                if (ve.RemoveEntry && dlgResult != DialogResult.Ignore)
                {
                    //e. = MetaDataGrid.IsCurrentCellInEditMode;
                }

                MetaDataGrid.InvalidateCell(e.ColumnIndex, e.RowIndex);
            }
            catch (Exception ex)
            {
                MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_ERROR, ex.Message);
            }
        }

        private void MetaDataGrid_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            if (e.Exception != null)
            {
                MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_ERROR, e.Exception.Message);
            }
        }

        private void ExtractAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<Page> selectedPages = new List<Page>();

            try
            {
                foreach (ListViewItem item in PagesList.SelectedItems)
                {
                    selectedPages.Add(item.Tag as Page);
                }

                ExtractFilesDialog extractFilesDialog = new ExtractFilesDialog
                {
                    TargetFolder = LastOutputDirectory,
                    SelectedPages = selectedPages
                };

                if (extractFilesDialog.ShowDialog() == DialogResult.OK)
                {
                    ExtractSelectedPages.Enabled = false;
                    if (extractFilesDialog.ExtractType == 0)
                    {

                        Program.ProjectModel.Extract(extractFilesDialog.TargetFolder);
                    }
                    else
                    {
                        Program.ProjectModel.Extract(extractFilesDialog.TargetFolder, selectedPages);
                    }

                    LastOutputDirectory = extractFilesDialog.TargetFolder;
                }
            }
            catch (Exception) { }
        }

        private void PagesList_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            List<ListViewItem> selectedPages = PagesList.SelectedItems.Cast<ListViewItem>().ToList();
            
            bool buttonStateSelected = selectedPages.Count > 0;
            bool propsButtonAvailable = selectedPages.Count == 1;

            ToolButtonRemoveFiles.Enabled = buttonStateSelected;
            deleteToolStripMenuItem.Enabled = buttonStateSelected;
            ToolButtonMovePageDown.Enabled = buttonStateSelected && selectedPages.Count != PagesList.Items.Count;
            ToolButtonMovePageUp.Enabled = buttonStateSelected && selectedPages.Count != PagesList.Items.Count;
            ToolButtonEditImageProps.Enabled = buttonStateSelected;
            ToolButtonImagePreview.Enabled = selectedPages.Count == 1;
            ToolButtonEditImage.Enabled = propsButtonAvailable;
            propertiesToolStripMenuItem.Enabled = buttonStateSelected;
            removeToolStripMenuItem.Enabled = buttonStateSelected;
            SaveSelectedPageAsToolStripMenuItem.Enabled = propsButtonAvailable;

            ToolButtonSetPageType.Enabled = buttonStateSelected;

            if (PagesList.SelectedItems.Count == 1)
            {
                if (Win_CBZSettings.Default.JumpToPage)
                {
                    if (PagesList.SelectedItems[0].Tag != null)
                    {
                        if (PageThumbsListBox.Items.IndexOf(PagesList.SelectedItems[0].Tag as Page) > -1)
                        {
                            PageThumbsListBox.TopIndex = PageThumbsListBox.Items.IndexOf(PagesList.SelectedItems[0].Tag as Page);
                        }
                    }
                }
            }

            if (selectedPages.Count > 0 && selectedPages[0].Tag != null)
            {
                if (buttonStateSelected)
                {
                    if (!((Page)selectedPages[0].Tag).ImageInfoRequested && (((Page)selectedPages[0].Tag).Format == null || (((Page)selectedPages[0].Tag).Format.W == 0 && ((Page)selectedPages[0].Tag).Format.H == 0)))
                    {

                        ImageInfoPagesSlice.Add(((Page)selectedPages[0].Tag));
                    }
                    //((Page)selectedPages[0].Tag).LoadImageInfo();

                    if (((Page)selectedPages[0].Tag).Format != null)
                    {
                        LabelW.Text = ((Page)selectedPages[0].Tag).Format.W.ToString();
                        LabelH.Text = ((Page)selectedPages[0].Tag).Format.H.ToString();
                    }


                }
                else
                {

                }

                ((Page)e.Item.Tag).Selected = e.IsSelected;
            }

            foreach (ListViewItem item in selectedPages)
            {
                if (item.Tag != null)
                {
                    if (((Page)item.Tag).Compressed)
                    {
                        //
                    }
                }

            }
        }

        private void ToolButtonRemoveFiles_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.ListView.SelectedListViewItemCollection selectedPages = this.PagesList.SelectedItems;

            /*
            int MetaVersionSetting = Win_CBZSettings.Default.MetaDataPageIndexVersionToWrite;
            MetaData.PageIndexVersion versionToWrite = PageIndexVersion.VERSION_1;

            switch (MetaVersionSetting)
            {
                case 1:
                    versionToWrite = PageIndexVersion.VERSION_1;
                    break;
                case 2:
                    versionToWrite = PageIndexVersion.VERSION_2;
                    break;
                default:
                    versionToWrite = PageIndexVersion.VERSION_1;
                    break;
            }

            */

            if (selectedPages.Count > 0)
            {
                foreach (ListViewItem img in selectedPages)
                {
                    if (img.Tag != null)
                    {
                        ((Page)img.Tag).Deleted = true;
                        if (!((Page)img.Tag).Compressed)
                        {
                            try
                            {
                                ((Page)img.Tag).DeleteTemporaryFile();
                            }
                            catch (Exception ex)
                            {
                                MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_ERROR, ex.Message);
                            }
                        }
                    }

                    img.ForeColor = Color.Silver;
                    img.BackColor = Color.Transparent;

                    AppEventHandler.OnPageChanged(this, new PageChangedEvent((Page)img.Tag, null, PageChangedEvent.IMAGE_STATUS_DELETED));
                    AppEventHandler.OnArchiveStatusChanged(this, new ArchiveStatusEvent(Program.ProjectModel, ArchiveStatusEvent.ARCHIVE_FILE_DELETED));
                }

                TokenStore.GetInstance().ResetCancellationToken(TokenStore.TOKEN_SOURCE_UPDATE_PAGE_INDEX);

                AppEventHandler.OnGlobalActionRequired(this,
                    new GlobalActionRequiredEvent(Program.ProjectModel,
                        0,
                        "Page order changed. Rebuild pageindex now?",
                        "Rebuild",
                        GlobalActionRequiredEvent.TASK_TYPE_INDEX_REBUILD,
                        UpdatePageIndexTask.UpdatePageIndex(Program.ProjectModel.Pages,
                            AppEventHandler.OnGeneralTaskProgress,
                            AppEventHandler.OnPageChanged,
                            TokenStore.GetInstance().CancellationTokenSourceForName(TokenStore.TOKEN_SOURCE_UPDATE_PAGE_INDEX).Token,
                            false,
                            true
                        )
                    )
                );

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

            if (sender is ToolStripMenuItem)
            {
                if (Win_CBZSettings.Default.WriteXmlPageIndex == false)
                {
                    DialogResult res = ApplicationMessage.ShowConfirmation("Currently writing XML- pageindex is disabled!\r\nCBZ needs to contain XML pageindex in order to define individual pagetypes. Please enable it in Application settings under 'CBZ -> Compatibility' first.", "XML pageindex required", ApplicationMessage.DialogType.MT_INFORMATION, ApplicationMessage.DialogButtons.MB_OK);

                    return;
                }

                if (Win_CBZSettings.Default.WriteXmlPageIndex && !Program.ProjectModel.MetaData.Exists())
                {
                    DialogResult res = ApplicationMessage.ShowConfirmation("Currently no metadata available!\r\nCBZ needs to contain XML metadata (" + Win_CBZSettings.Default.MetaDataFilename + ") in order to define individual pagetypes. Add a new set of Metadata now?", "Metadata required", ApplicationMessage.DialogType.MT_CONFIRMATION, ApplicationMessage.DialogButtons.MB_YES | ApplicationMessage.DialogButtons.MB_NO);
                    if (res == DialogResult.Yes)
                    {
                        BtnAddMetaData_Click(sender, null);
                    }
                    else
                    {
                        return;
                    }
                }
            }
            else
            {
                if (Win_CBZSettings.Default.WriteXmlPageIndex == false)
                {
                    return;
                }

                if (Win_CBZSettings.Default.WriteXmlPageIndex && !Program.ProjectModel.MetaData.Exists())
                {
                    return;
                }
            }

            foreach (ListViewItem item in PagesList.SelectedItems)
            {
                if (sender is ToolStripMenuItem)
                {
                    ((Page)item.Tag).ImageType = (String)((ToolStripMenuItem)sender).Tag;

                    try
                    {
                        if (PagesList.SelectedItems.Count > 1)
                        {
                            String gid = Guid.NewGuid().ToString();

                            TokenStore.GetInstance().ResetCancellationToken(TokenStore.TOKEN_SOURCE_UPDATE_PAGE_INDEX);

                            AppEventHandler.OnGlobalActionRequired(null,
                                new GlobalActionRequiredEvent(Program.ProjectModel,
                                    0,
                                    "Page type changed. Rebuild pageindex now?",
                                    "Rebuild",
                                    GlobalActionRequiredEvent.TASK_TYPE_INDEX_REBUILD,
                                    UpdatePageIndexTask.UpdatePageIndex(Program.ProjectModel.Pages,
                                        AppEventHandler.OnGeneralTaskProgress,
                                        AppEventHandler.OnPageChanged,
                                        TokenStore.GetInstance().CancellationTokenSourceForName(TokenStore.TOKEN_SOURCE_UPDATE_PAGE_INDEX).Token,
                                        false,
                                        true
                                     )
                                )
                            );
                        }
                        else
                        {
                            Program.ProjectModel.MetaData.UpdatePageIndexMetaDataEntry((Page)item.Tag, ((Page)item.Tag).Key);
                        }

                        AppEventHandler.OnPageChanged(this, new PageChangedEvent((Page)item.Tag, null, PageChangedEvent.IMAGE_STATUS_CHANGED));
                        AppEventHandler.OnArchiveStatusChanged(this, new ArchiveStatusEvent(Program.ProjectModel, ArchiveStatusEvent.ARCHIVE_METADATA_CHANGED));
                    }
                    catch (MetaDataPageEntryException ex)
                    {
                        ApplicationMessage.ShowWarning(ex.Message, ex.GetType().Name, ApplicationMessage.DialogType.MT_WARNING, ApplicationMessage.DialogButtons.MB_OK);
                    }
                    catch (Exception eg)
                    {
                        ApplicationMessage.ShowException(eg);
                    }
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
            List<Page> pageProperties = new List<Page>();
            List<Page> originalPages = new List<Page>();
            bool pageIndexUpdateNeeded = false;
            bool pageImageUpdateNeeded = false;
            String indexRebuildMessage = "";

            if (PagesList.SelectedItems.Count > 0)
            {
                try
                {
                    foreach (ListViewItem selectedItem in PagesList.SelectedItems)
                    {
                        originalPages.Add(selectedItem.Tag as Page);
                        pageProperties.Add(selectedItem.Tag as Page /*, (selectedItem.Tag as Page).Compressed*/);
                        //pageProperties.Add(new Page(selectedItem.Tag as Page /*, (selectedItem.Tag as Page).Compressed*/));
                    }
                }
                catch (PageException pe)
                {
                    if (pe.ShowErrorDialog)
                    {
                        ApplicationMessage.ShowException(pe);
                    }
                }

                DialogResult dlgResult = DialogResult.None;
                PageSettingsForm pageSettingsForm = new PageSettingsForm(pageProperties);

                try
                {
                    dlgResult = pageSettingsForm.ShowDialog();
                }
                catch (Exception ex)
                {
                    ApplicationMessage.ShowException(ex);
                }


                if (dlgResult == DialogResult.OK)
                {
                    int i = 0;
                    int thumbIndex = -1;
                    List<Page> pagesResult = pageSettingsForm.GetResult();
                    foreach (Page pageResult in pagesResult)
                    {
                        Page pageToUpdate = Program.ProjectModel.GetPageById(pageResult.Id);
                        if (pageToUpdate != null)
                        {
                            List<Page> pageKey = Program.ProjectModel.GetPagesByKey(pageResult.Key);
                            String pageName = pageResult.Name;

                            if (MetaDataVersionFlavorHandler.GetInstance().TargetVersion() == PageIndexVersion.VERSION_2)
                            {
                                if (pageKey != null && pageKey.Count > 1)
                                {
                                    foreach (Page findPage in pageKey)
                                    {
                                        if (findPage.Id != pageResult.Id)
                                        {
                                            pageName = findPage.Name;
                                        }
                                    }

                                    MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_WARNING, new StringBuilder("Warning! Page ['")
                                        .Append(pageName)
                                        .Append("'] ")
                                        .Append("already uses key ['")
                                        .Append(pageToUpdate.Key)
                                        .Append("'].")
                                        .AppendLine("It is recommended for every page to use a unique key.")
                                        .ToString());

                                    ApplicationMessage.ShowWarning(
                                        new StringBuilder("Warning! Page ['")
                                        .Append(pageName)
                                        .Append("'] ")
                                        .Append("already uses key ['")
                                        .Append(pageToUpdate.Key)
                                        .Append("'].")
                                        .AppendLine("It is recommended for every page to use a unique key.")
                                        .ToString(), "Page duplicate key", ApplicationMessage.DialogType.MT_WARNING, ApplicationMessage.DialogButtons.MB_OK);

                                }
                            }


                            if (pageResult.Deleted != pageProperties[i].Deleted)
                            {
                                pageIndexUpdateNeeded = true;
                                indexRebuildMessage = "Page order changed. Rebuild pageindex now?";
                                //HandleGlobalActionRequired(null, new GlobalActionRequiredEvent(Program.ProjectModel, 0, "Page order changed. Rebuild pageindex now?", "Rebuild", GlobalActionRequiredEvent.TASK_TYPE_INDEX_REBUILD, UpdatePageIndexTask.UpdatePageIndex(Program.ProjectModel.Pages, Program.ProjectModel.MetaData, HandleGlobalTaskProgress, PageChanged)));
                            }

                            if (pageResult.Deleted)
                            {
                                try
                                {
                                    pageResult.DeleteTemporaryFile();
                                }
                                catch (Exception ex)
                                {
                                    MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_ERROR, ex.Message);
                                }

                                pageIndexUpdateNeeded = true;
                                indexRebuildMessage = "Page order changed. Rebuild pageindex now?";
                                //HandleGlobalActionRequired(null, new GlobalActionRequiredEvent(Program.ProjectModel, 0, "Page order changed. Rebuild pageindex now?", "Rebuild", GlobalActionRequiredEvent.TASK_TYPE_INDEX_REBUILD, UpdatePageIndexTask.UpdatePageIndex(Program.ProjectModel.Pages, Program.ProjectModel.MetaData, HandleGlobalTaskProgress, PageChanged)));
                            }



                            if (pageResult.Deleted != pageProperties[i].Deleted ||
                                pageResult.DoublePage != pageProperties[i].DoublePage ||
                                pageResult.Name != pageProperties[i].Name ||
                                pageResult.Key != pageProperties[i].Key ||
                                pageResult.Bookmark != pageProperties[i].Bookmark ||
                                pageResult.Index != pageProperties[i].Index ||
                                pageResult.ImageType != pageProperties[i].ImageType ||
                                pageResult.TemporaryFileId != pageProperties[i].TemporaryFileId
                                )
                            {
                                pageImageUpdateNeeded = pageResult.TemporaryFileId != pageProperties[i].TemporaryFileId;

                                try
                                {
                                    if (pagesResult.Count == 1)
                                    {
                                        Program.ProjectModel.MetaData.UpdatePageIndexMetaDataEntry(pageResult, pageProperties[i].Key);
                                    }
                                    else
                                    {
                                        pageIndexUpdateNeeded = true;
                                    }
                                }
                                catch (MetaDataPageEntryException em)
                                {
                                    MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_WARNING, em.Message);

                                    if (em.ShowErrorDialog)
                                    {
                                        ApplicationMessage.ShowWarning(em.Message, em.GetType().Name, ApplicationMessage.DialogType.MT_WARNING, ApplicationMessage.DialogButtons.MB_OK);
                                    }
                                }

                                thumbIndex = PageThumbsListBox.Items.IndexOf(pageToUpdate);

                                AppEventHandler.OnPageChanged(this, new PageChangedEvent(pageResult, pageProperties[i], PageChangedEvent.IMAGE_STATUS_CHANGED));
                                AppEventHandler.OnArchiveStatusChanged(this, new ArchiveStatusEvent(Program.ProjectModel, ArchiveStatusEvent.ARCHIVE_FILE_UPDATED));
                            }

                            pageToUpdate.UpdatePage(pageResult, false, true);  // dont update name without rename checks!
                            if (pageResult.Deleted)
                            {
                                AppEventHandler.OnPageChanged(this, new PageChangedEvent(pageResult, pageProperties[i], PageChangedEvent.IMAGE_STATUS_DELETED));
                                AppEventHandler.OnArchiveStatusChanged(this, new ArchiveStatusEvent(Program.ProjectModel, ArchiveStatusEvent.ARCHIVE_FILE_DELETED));
                            }

                            if (!pageResult.Deleted)
                            {
                                if (dlgResult == DialogResult.OK && pageImageUpdateNeeded)
                                {
                                    pageToUpdate.ThumbnailInvalidated = true;
                                    if (thumbIndex > -1)
                                    {
                                        PageImages.Images.RemoveByKey(pageToUpdate.Id);
                                        PageImages.Images.Add(pageToUpdate.Id, pageToUpdate.GetThumbnailBitmap());
                                        pageToUpdate.FreeImage();
                                        PageThumbsListBox.Items[thumbIndex] = pageToUpdate;
                                        PageThumbsListBox.Refresh();
                                    }
                                }

                                if (pageProperties[i].Name != pageResult.Name)
                                {
                                    try
                                    {
                                        Program.ProjectModel.RenamePage(pageToUpdate, pageResult.Name, false, true);

                                        try
                                        {
                                            if (pagesResult.Count == 1)
                                            {
                                                Program.ProjectModel.MetaData.UpdatePageIndexMetaDataEntry(pageResult, pageProperties[i].Key);
                                            }
                                            else
                                            {
                                                pageIndexUpdateNeeded = true;
                                                indexRebuildMessage = "Page name changed. Rebuild pageindex now?";
                                            }
                                        }
                                        catch (MetaDataPageEntryException em)
                                        {
                                            MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_WARNING, em.Message);

                                            //if (em.ShowErrorDialog)
                                            //{
                                            //    ApplicationMessage.ShowWarning(em.Message, em.GetType().Name, ApplicationMessage.DialogType.MT_WARNING, ApplicationMessage.DialogButtons.MB_OK);
                                            //}
                                        }

                                        //AppEventHandler.OnGlobalActionRequired(this, new GlobalActionRequiredEvent(Program.ProjectModel, 0, "Page name changed. Rebuild pageindex now?", "Rebuild", GlobalActionRequiredEvent.TASK_TYPE_INDEX_REBUILD, UpdatePageIndexTask.UpdatePageIndex(Program.ProjectModel.Pages, Program.ProjectModel.MetaData, HandleGlobalTaskProgress, PageChanged)));

                                        //AppEventHandler.OnPageChanged(this, new PageChangedEvent(pageResult, pageProperties[i], PageChangedEvent.IMAGE_STATUS_RENAMED));

                                    }
                                    catch (PageDuplicateNameException ae)
                                    {
                                        pageResult.Name = pageProperties[i].Name;
                                        MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_WARNING, ae.Message);
                                        if (ae.ShowErrorDialog)
                                        {
                                            ApplicationMessage.ShowWarning(ae.Message, "Error renaming page", ApplicationMessage.DialogType.MT_WARNING, ApplicationMessage.DialogButtons.MB_OK);
                                        }

                                        AppEventHandler.OnPageChanged(this, new PageChangedEvent(pageResult, pageProperties[i], PageChangedEvent.IMAGE_STATUS_CHANGED));

                                        try
                                        {
                                            Program.ProjectModel.MetaData.UpdatePageIndexMetaDataEntry(pageResult, pageProperties[i].Key);
                                        }
                                        catch (MetaDataPageEntryException)
                                        {
                                            //if (em.ShowErrorDialog)
                                            //{
                                            //    ApplicationMessage.ShowWarning(em.Message, em.GetType().Name, ApplicationMessage.DialogType.MT_WARNING, ApplicationMessage.DialogButtons.MB_OK);
                                            //}
                                        }
                                    }
                                    catch (ApplicationException pe)
                                    {
                                        pageResult.Name = pageProperties[i].Name;
                                        MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_WARNING, pe.Message);
                                        if (pe.ShowErrorDialog)
                                        {
                                            ApplicationMessage.ShowWarning(pe.Message, "Error renaming page", ApplicationMessage.DialogType.MT_WARNING, ApplicationMessage.DialogButtons.MB_OK);
                                        }

                                        AppEventHandler.OnPageChanged(this, new PageChangedEvent(pageResult, pageProperties[i], PageChangedEvent.IMAGE_STATUS_CHANGED));

                                        try
                                        {
                                            Program.ProjectModel.MetaData.UpdatePageIndexMetaDataEntry(pageResult, pageProperties[i].Key);
                                        }
                                        catch (MetaDataPageEntryException)
                                        {
                                            //if (em.ShowErrorDialog)
                                            //{
                                            //    ApplicationMessage.ShowWarning(em.Message, em.GetType().Name, ApplicationMessage.DialogType.MT_WARNING, ApplicationMessage.DialogButtons.MB_OK);
                                            //}
                                        }
                                    }
                                    catch (Exception ex3)
                                    {
                                        MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_ERROR, ex3.Message);
                                    }
                                }

                                try
                                {
                                    if (pagesResult.Count == 1)
                                    {
                                        if (pageResult.Index >= 0 && pageResult.Index < Program.ProjectModel.Pages.Count)
                                        {
                                            MovePageTo(pageToUpdate, pageResult.Index);
                                        }
                                        else
                                        {
                                            ApplicationMessage.ShowWarning("Invalid Pageindex! The target index is out of bounds!", "Failed to move page", ApplicationMessage.DialogType.MT_WARNING, ApplicationMessage.DialogButtons.MB_OK);

                                            RestoreIndex(pageToUpdate, pageProperties[i]);
                                            RestoreIndex(originalPages[i], pageProperties[i]);
                                        }

                                    }
                                    else
                                    {
                                        pageToUpdate.Number = pageResult.Index + 1;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    RestoreIndex(pageToUpdate, pageProperties[i]);
                                    RestoreIndex(originalPages[i], pageProperties[i]);

                                    ApplicationMessage.ShowException(ex);
                                }


                            }
                        }
                    }
                    i++;
                }

                pageSettingsForm.FreeResult();
                pageSettingsForm.Dispose();

                if (dlgResult == DialogResult.OK && pageImageUpdateNeeded)
                {
                    //RefreshPageView();
                }


                if (pageIndexUpdateNeeded)
                {
                    TokenStore.GetInstance().ResetCancellationToken(TokenStore.TOKEN_SOURCE_UPDATE_PAGE_INDEX);

                    AppEventHandler.OnGlobalActionRequired(this,
                        new GlobalActionRequiredEvent(Program.ProjectModel,
                            0,
                            indexRebuildMessage,
                            "Rebuild",
                            GlobalActionRequiredEvent.TASK_TYPE_INDEX_REBUILD,
                            UpdatePageIndexTask.UpdatePageIndex(Program.ProjectModel.Pages,
                                AppEventHandler.OnGeneralTaskProgress,
                                AppEventHandler.OnPageChanged,
                                TokenStore.GetInstance().CancellationTokenSourceForName(TokenStore.TOKEN_SOURCE_UPDATE_PAGE_INDEX).Token,
                                false,
                                true
                            )
                        ));
                }
            }
        }

        private void RestoreIndex(Page updatedPage, Page originalPage)
        {
            updatedPage.Index = originalPage.Index;
            updatedPage.Number = originalPage.Index + 1;
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
                    ToolButtonSave.Enabled = true; // Program.ProjectModel.FileName != null && Program.ProjectModel.FileName.Length > 0; 
                    SaveToolStripMenuItem.Enabled = true; // Program.ProjectModel.FileName != null && Program.ProjectModel.FileName.Length > 0;
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
                try
                {
                    Program.ProjectModel.ClearTempFolder(Win_CBZSettings.Default.TempFolderPath);
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

        private void TogglePagePreviewToolbutton_Click(object sender, EventArgs e)
        {
            TogglePagePreviewToolbutton.Checked = !TogglePagePreviewToolbutton.Checked;
            Win_CBZSettings.Default.PagePreviewEnabled = TogglePagePreviewToolbutton.Checked;
            SplitBoxPageView.Panel1Collapsed = !TogglePagePreviewToolbutton.Checked;
            Program.ProjectModel.PreloadPageImages = TogglePagePreviewToolbutton.Checked;

            if (Win_CBZSettings.Default.PagePreviewEnabled && PageView.Items.Count == 0)
            {
                Task.Factory.StartNew(() =>
                {
                    PageView.Invoke(new Action(() =>
                    {
                        foreach (ListViewItem pageItem in PagesList.Items)
                        {
                            CreatePagePreviewFromItem((Page)pageItem.Tag);
                        }
                    }));
                });


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
            if (Win_CBZSettings.Default.WriteXmlPageIndex && !Program.ProjectModel.MetaData.Exists())
            {
                DialogResult res = ApplicationMessage.ShowConfirmation("Currently no metadata available!\r\nCBZ needs to contain XML metadata (" + Win_CBZSettings.Default.MetaDataFilename + ") in order to define individual pagetypes. Add a new set of Metadata now?", "Metadata required", ApplicationMessage.DialogType.MT_CONFIRMATION, ApplicationMessage.DialogButtons.MB_YES | ApplicationMessage.DialogButtons.MB_NO);
                if (res == DialogResult.Yes)
                {
                    BtnAddMetaData_Click(sender, null);
                }
                else
                {
                    return;
                }
            }

            if (Win_CBZSettings.Default.WriteXmlPageIndex == false)
            {
                DialogResult res = ApplicationMessage.ShowConfirmation("Currently writing XML- pageindex is disabled!\r\nCBZ needs to contain XML pageindex in order to define individual pagetypes. Please enable it in Application settings under 'CBZ -> Compatibility' first.", "XML pageindex required", ApplicationMessage.DialogType.MT_INFORMATION, ApplicationMessage.DialogButtons.MB_OK);

                return;
            }

            foreach (ListViewItem item in PagesList.SelectedItems)
            {
                ((Page)item.Tag).ImageType = (String)((ToolStripSplitButton)sender).Tag;

                if (item.SubItems.Count > 0)
                {
                    item.SubItems[2].Text = (String)((ToolStripSplitButton)sender).Tag;
                }

                try
                {
                    if (PagesList.SelectedItems.Count > 1)
                    {
                        TokenStore.GetInstance().ResetCancellationToken(TokenStore.TOKEN_SOURCE_UPDATE_PAGE_INDEX);

                        AppEventHandler.OnGlobalActionRequired(this,
                            new GlobalActionRequiredEvent(Program.ProjectModel,
                                0,
                                "Page type changed. Rebuild pageindex now?",
                                "Rebuild",
                                GlobalActionRequiredEvent.TASK_TYPE_INDEX_REBUILD,
                                UpdatePageIndexTask.UpdatePageIndex(Program.ProjectModel.Pages,
                                    AppEventHandler.OnGeneralTaskProgress,
                                    AppEventHandler.OnPageChanged,
                                    TokenStore.GetInstance().CancellationTokenSourceForName(TokenStore.TOKEN_SOURCE_UPDATE_PAGE_INDEX).Token,
                                    false,
                                    true
                                )
                            ));
                    }
                    else
                    {
                        Program.ProjectModel.MetaData.UpdatePageIndexMetaDataEntry((Page)item.Tag, ((Page)item.Tag).Key);
                    }

                    AppEventHandler.OnPageChanged(this, new PageChangedEvent((Page)item.Tag, null, PageChangedEvent.IMAGE_STATUS_CHANGED));
                    AppEventHandler.OnArchiveStatusChanged(this, new ArchiveStatusEvent(Program.ProjectModel, ArchiveStatusEvent.ARCHIVE_METADATA_CHANGED));
                }
                catch (MetaDataPageEntryException ex)
                {
                    ApplicationMessage.ShowWarning(ex.Message, ex.GetType().Name, ApplicationMessage.DialogType.MT_WARNING, ApplicationMessage.DialogButtons.MB_OK);
                }
                catch (Exception eg)
                {
                    ApplicationMessage.ShowException(eg);
                }
            }
        }

        private void SettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool currentWriteXMLIndexSetting = Win_CBZSettings.Default.WriteXmlPageIndex;
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
                Win_CBZSettings.Default.MetaDataPageIndexVersionToWrite = settingsDialog.MetaPageIndexWriteVersion;
                Win_CBZSettings.Default.OmitEmptyXMLTags = settingsDialog.OmitEmptyXMLTags;
                Win_CBZSettings.Default.AutoDeleteTempFiles = settingsDialog.DeleteTempFilesImediately;
                Win_CBZSettings.Default.SkipIndexCheck = settingsDialog.SkipIndexCheck;
                Win_CBZSettings.Default.CalculateHash = settingsDialog.CalculateCrc32;
                Win_CBZSettings.Default.InterpolationMode = settingsDialog.InterpolationMode;
                Win_CBZSettings.Default.TempFolderPath = settingsDialog.TempPath;
                Win_CBZSettings.Default.ImageExtenstionList = String.Join('|', settingsDialog.ImageFileExtensions.ToArray());
                Win_CBZSettings.Default.FilteredFilenamesList = String.Join('|', settingsDialog.FilteredFileNames.ToArray());

                Win_CBZSettings.Default.FilterByExtension = settingsDialog.FilterNewPagesByExt;
                Win_CBZSettings.Default.FilterSpecificFilenames = settingsDialog.FilterNewPagesSpecificName;
                Win_CBZSettings.Default.MetadataGridInstantEditMode = settingsDialog.MetadataGridEditMode;
                Win_CBZSettings.Default.MetadataGridInstantEditModeValueCol = settingsDialog.MetadataGridEditModeValueCol;
                Win_CBZSettings.Default.WriteXmlPageIndex = settingsDialog.WriteXMLPageIndex;
                Win_CBZSettings.Default.CompressionLevel = settingsDialog.CompressionLevel;
                Win_CBZSettings.Default.CompatMode = settingsDialog.CompatibilityMode;
                Win_CBZSettings.Default.IgnoreErrorsOnSave = settingsDialog.IgnoreErrors;
                Win_CBZSettings.Default.SkipFilesInSubDirectories = settingsDialog.SkipArchiveSubfolders;

                Win_CBZSettings.Default.LogValidationErrors = settingsDialog.LogValidationErrors;
                Win_CBZSettings.Default.RestoreWindowLayout = settingsDialog.RestoreWindowPosition;
                Win_CBZSettings.Default.JumpToPage = settingsDialog.JumpToSelectedPage;
                Win_CBZSettings.Default.DetectDoublePages = settingsDialog.DetectDoublePages;

                Win_CBZSettings.Default.AutoUpdate = settingsDialog.AutoUpdateCheck;
                Win_CBZSettings.Default.AutoupdateType = settingsDialog.AutoUpdateCheckIntervalType;

                Win_CBZSettings.Default.AccentColor = settingsDialog.AccentColor;
                Win_CBZSettings.Default.ButtonColor = settingsDialog.ButtonColor;

                Theme.GetInstance().SetColorHex("AccentColor", settingsDialog.AccentColor);
                Theme.GetInstance().SetColorHex("ButtonColor", settingsDialog.ButtonColor);

                ApplyTheme();

                switch (settingsDialog.AutoUpdateCheckIntervalType)
                {
                    case 0:
                        Win_CBZSettings.Default.AutoUpdateInterval = UpdateCheckHelper.UPDATE_CHECK_INTERVAL_DAILY;
                        break;
                    case 1:
                        Win_CBZSettings.Default.AutoUpdateInterval = UpdateCheckHelper.UPDATE_CHECK_INTERVAL_WEEKLY;
                        break;
                    case 2:
                        Win_CBZSettings.Default.AutoUpdateInterval = UpdateCheckHelper.UPDATE_CHECK_INTERVAL_MONTHLY;
                        break;

                    default:
                        Win_CBZSettings.Default.AutoUpdateInterval = UpdateCheckHelper.UPDATE_CHECK_INTERVAL_DAILY;
                        break;
                }

                Program.ProjectModel.WorkingDir = PathHelper.ResolvePath(settingsDialog.TempPath);


                Win_CBZSettings.Default.CustomMetadataFields.Clear();
                foreach (String line in settingsDialog.CustomFieldTypesCollection)
                {
                    Win_CBZSettings.Default.CustomMetadataFields.Add(line);
                }

                MetaDataFieldConfig.GetInstance().UpdateFrom(Win_CBZSettings.Default.CustomMetadataFields.OfType<String>().ToArray());

                TokenStore.GetInstance().ResetCancellationToken(TokenStore.TOKEN_SOURCE_UPDATE_PAGES_SETTINGS);

                Task updatePages = new Task((token) =>
                {
                    int current = 0;
                    int updated = 0;

                    SettingsToolStripMenuItem.Enabled = false;

                    foreach (Page page in Program.ProjectModel.Pages)
                    {
                        string newPath = Path.Combine(Program.ProjectModel.WorkingDir, Program.ProjectModel.ProjectGUID);

                        if (!Path.EndsInDirectorySeparator(newPath))
                        {
                            newPath += Path.DirectorySeparatorChar;
                        }

                        if (!Directory.Exists(newPath))
                        {
                            Directory.CreateDirectory(newPath);
                        }

                        //page.ImageTask.ImageAdjustments.ConvertType = Win_CBZSettings.Default.ImageConversionMode;
                        page.ImageTask.ImageAdjustments.Interpolation = Enum.Parse<InterpolationMode>(Win_CBZSettings.Default.InterpolationMode);
                        page.WorkingDir = Path.Combine(Program.ProjectModel.WorkingDir, Program.ProjectModel.ProjectGUID);


                        if (page.TemporaryFile != null && !Path.Equals(page.TemporaryFile.FilePath, newPath))
                        {
                            try
                            {
                                page.CreateLocalWorkingCopy();
                                updated++;
                            }
                            catch (Exception ex)
                            {
                                MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_ERROR, ex.Message);
                            }
                        }

                        if (updated > 0)
                        {
                            AppEventHandler.OnGeneralTaskProgress(null, new GeneralTaskProgressEvent(
                                GeneralTaskProgressEvent.TASK_UPDATE_PAGE_INDEX,
                                GeneralTaskProgressEvent.TASK_STATUS_RUNNING,
                                "Updating pages settings...",
                                current,
                                Program.ProjectModel.Pages.Count,
                                false,
                                true));
                        }
                    }

                    AppEventHandler.OnGeneralTaskProgress(null, new GeneralTaskProgressEvent(
                            GeneralTaskProgressEvent.TASK_UPDATE_PAGE_INDEX,
                            GeneralTaskProgressEvent.TASK_STATUS_COMPLETED,
                            "Ready.",
                            0,
                            0,
                            false,
                            true));

                }, TokenStore.GetInstance().RequestCancellationToken(TokenStore.TOKEN_SOURCE_UPDATE_PAGES_SETTINGS));

                updatePages.ContinueWith(t =>
                {
                    SettingsToolStripMenuItem.Enabled = true;

                    AppEventHandler.OnApplicationStateChanged(null, new ApplicationStatusEvent(Program.ProjectModel, ApplicationStatusEvent.STATE_READY));

                    if (Win_CBZSettings.Default.WriteXmlPageIndex && currentWriteXMLIndexSetting != Win_CBZSettings.Default.WriteXmlPageIndex)
                    {
                        //DialogResult res = ApplicationMessage.Show("The application needs to be restarted in order to apply the changes.\r\nRestart now?", "Restart required", ApplicationMessage.DialogType.MT_INFORMATION, ApplicationMessage.DialogButtons.MB_YES | ApplicationMessage.DialogButtons.MB_NO);

                        //if (res == DialogResult.Yes)
                        //{
                        //    Application.Restart();
                        //}

                        String gid = Guid.NewGuid().ToString();

                        TokenStore.GetInstance().ResetCancellationToken(TokenStore.TOKEN_SOURCE_UPDATE_IMAGE_METADATA);

                        AppEventHandler.OnGlobalActionRequired(this,
                            new GlobalActionRequiredEvent(Program.ProjectModel,
                                0,
                                "Image metadata needs to be updated! Reload image metadata and rebuild pageindex now?",
                                "Rebuild",
                                GlobalActionRequiredEvent.TASK_TYPE_UPDATE_IMAGE_METADATA,
                                ReadImageMetaDataTask.UpdateImageMetadata(Program.ProjectModel.Pages,
                                    AppEventHandler.OnGeneralTaskProgress,
                                    TokenStore.GetInstance().RequestCancellationToken(TokenStore.TOKEN_SOURCE_UPDATE_IMAGE_METADATA),
                                    true,
                                    true
                                ),
                                gid
                            )
                        );
                    }
                });

                updatePages.Start();

                MetaDataFieldConfig.GetInstance().UpdateFrom(Win_CBZSettings.Default.CustomMetadataFields.OfType<String>().ToArray());

                TextBoxMetaDataFilename.Text = settingsDialog.MetaDataFilename;

                ComboBoxCompressionLevel.SelectedIndex = settingsDialog.CompressionLevel;
                CheckBoxCompatibilityMode.Checked = settingsDialog.CompatibilityMode;
                CheckBoxIgnoreErrorsOnSave.Checked = settingsDialog.IgnoreErrors;

                bool enabled = settingsDialog.WriteXMLPageIndex;
                bool realCompatModeSetting = Win_CBZSettings.Default.CompatMode;

                CheckBoxCompatibilityMode.Enabled = enabled;
                CheckBoxCompatibilityMode.Checked = !enabled;
                if (enabled)
                {
                    CheckBoxCompatibilityMode.Checked = realCompatModeSetting;
                }

                Program.ProjectModel.CompatibilityMode = CheckBoxCompatibilityMode.Checked;


                Program.ProjectModel.MetaData.MetaDataFileName = settingsDialog.MetaDataFilename;

                Program.ProjectModel.FilteredFileNames.Clear();
                Program.ProjectModel.FilteredFileNames.Add(Win_CBZSettings.Default.MetaDataFilename.ToLower());
            }
            else
            {
                //
            }
        }

        private void QuitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
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
                borderPen = new Pen(Theme.GetInstance().AccentColor, 2);
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

            }
            else
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

        private void ToolStripButtonShowRawMetadata_Click(object sender, EventArgs e)
        {
            DataValidation dataValidation = new DataValidation();
            ArrayList messages = new ArrayList();

            bool metaDataValidationFailed = dataValidation.ValidateMetaDataInvalidKeys(ref messages);
            if (metaDataValidationFailed)
            {
                AppEventHandler.OnApplicationStateChanged(this, new ApplicationStatusEvent(null, ApplicationStatusEvent.STATE_READY));

                return;
            }

            try
            {
                MetaDataForm metaDataDialog = new MetaDataForm(Program.ProjectModel.MetaData, Win_CBZSettings.Default.WriteXmlPageIndex);
                metaDataDialog.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_ERROR, ex.Message);
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

        private void PagesList_DoubleClick(object sender, EventArgs e)
        {

            //if (e.Button == MouseButtons.Left)
            //{
            ToolButtonEditImageProps_Click(this, e);
            //}
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
                case 3:
                    Program.ProjectModel.CompressionLevel = CompressionLevel.SmallestSize;
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
                Program.ProjectModel.Validate(MetaDataVersionFlavorHandler.GetInstance().TargetVersion(), true);
            }
            catch (ConcurrentOperationException c)
            {
                MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_WARNING, c.Message);
                if (c.ShowErrorDialog)
                {
                    ApplicationMessage.ShowWarning(c.Message, "Concurrency Exception", ApplicationMessage.DialogType.MT_WARNING, ApplicationMessage.DialogButtons.MB_OK);
                }
            }
        }

        private void ImageQualityTrackBar_ValueChanged(object sender, EventArgs e)
        {
            ImageTaskAssignment selectedImageTasks = ImageTaskListView.SelectedItem.Tag as ImageTaskAssignment;

            if (selectedImageTasks != null)
            {
                //selectedImageTasks.ImageAdjustments.Quality = ImageQualityTrackBar.Value;
            }

        }

        private void ImageResizeRadioChanged(object sender, EventArgs e)
        {
            RadioButton radio = (RadioButton)sender;

            Nullable<int> oldValue;

            if (ImageTaskListView.SelectedItem == null)
            {
                return;
            }

            ImageTaskAssignment selectedImageTasks = ImageTaskListView.SelectedItem.Tag as ImageTaskAssignment;


            if (selectedImageTasks != null && radio.Checked)
            {

                bool dontUpdate = radio.Tag != null ? ((bool)radio.Tag) : true;

                oldValue = selectedImageTasks.ImageTask.ImageAdjustments.ResizeMode;


                if (!dontUpdate)
                {
                    switch (radio.Name)
                    {
                        case "RadioButtonResizeNever":
                            selectedImageTasks.ImageTask.ImageAdjustments.ResizeMode = 0;
                            break;
                        case "RadioButtonResizeIfLarger":
                            selectedImageTasks.ImageTask.ImageAdjustments.ResizeMode = 1;
                            break;
                        case "RadioButtonResizeTo":
                            selectedImageTasks.ImageTask.ImageAdjustments.ResizeMode = 2;
                            break;
                        case "RadioButtonResizePercent":
                            selectedImageTasks.ImageTask.ImageAdjustments.ResizeMode = 3;
                            break;
                    }
                }

                if (oldValue != null && oldValue != selectedImageTasks.ImageTask.ImageAdjustments.ResizeMode)
                {
                    if (!dontUpdate)
                    {
                        foreach (Page page in selectedImageTasks.Pages)
                        {
                            AppEventHandler.OnPageChanged(this, new PageChangedEvent(page, null, PageChangedEvent.IMAGE_STATUS_CHANGED, true));
                        }

                        ImageTaskListView.SelectedItem.Text = selectedImageTasks.GetAssignedTaskName();

                        selectedImageTasks.AssignTaskToPages();
                    }

                    AppEventHandler.OnArchiveStatusChanged(this, new ArchiveStatusEvent(Program.ProjectModel, ArchiveStatusEvent.ARCHIVE_FILE_UPDATED));
                }
            }
        }

        private void ImageRotateRadioChanged(object sender, EventArgs e)
        {
            RadioButton radio = (RadioButton)sender;

            Nullable<int> oldValue;

            if (ImageTaskListView.SelectedItem == null)
            {
                return;
            }

            ImageTaskAssignment selectedImageTasks = ImageTaskListView.SelectedItem.Tag as ImageTaskAssignment;


            if (selectedImageTasks != null && radio.Checked)
            {

                oldValue = selectedImageTasks.ImageTask.ImageAdjustments.RotateMode;
                bool dontUpdate = radio.Tag != null ? ((bool)radio.Tag) : true;

                if (!dontUpdate)
                {
                    switch (radio.Name)
                    {
                        case "RadioButtonRotateNone":
                            selectedImageTasks.ImageTask.ImageAdjustments.RotateMode = 0;
                            break;
                        case "RadioButtonRotate90":
                            selectedImageTasks.ImageTask.ImageAdjustments.RotateMode = 1;
                            break;
                        case "RadioButtonRotate180":
                            selectedImageTasks.ImageTask.ImageAdjustments.RotateMode = 2;
                            break;
                        case "RadioButtonRotate270":
                            selectedImageTasks.ImageTask.ImageAdjustments.RotateMode = 3;
                            break;
                    }
                }

                if (oldValue != null && oldValue != selectedImageTasks.ImageTask.ImageAdjustments.RotateMode)
                {
                    if (!dontUpdate)
                    {
                        foreach (Page page in selectedImageTasks.Pages)
                        {
                            AppEventHandler.OnPageChanged(this, new PageChangedEvent(page, null, PageChangedEvent.IMAGE_STATUS_CHANGED, true));
                        }

                        ImageTaskListView.SelectedItem.Text = selectedImageTasks.GetAssignedTaskName();

                        selectedImageTasks.AssignTaskToPages();
                    }

                    AppEventHandler.OnArchiveStatusChanged(this, new ArchiveStatusEvent(Program.ProjectModel, ArchiveStatusEvent.ARCHIVE_FILE_UPDATED));
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
                ToolButtonSave.Enabled = true; // Program.ProjectModel.FileName != null && Program.ProjectModel.FileName.Length > 0;
                CheckBoxPreview.Enabled = false;
                SaveToolStripMenuItem.Enabled = true;

                try
                {
                    Program.ProjectModel.RestoreOriginalNames();
                }
                catch (ConcurrentOperationException c)
                {
                    MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_WARNING, c.Message);

                    if (c.ShowErrorDialog)
                    {
                        ApplicationMessage.ShowWarning(c.Message, "ConcurrentOperationException", ApplicationMessage.DialogType.MT_WARNING, ApplicationMessage.DialogButtons.MB_OK);
                    }
                }
            }
        }

        private void ResetUpdateTags()
        {

            RadioButtonResizeNever.Tag = false;
            RadioButtonResizeIfLarger.Tag = false;
            RadioButtonResizeTo.Tag = false;
            RadioButtonResizePercent.Tag = false;

            RadioButtonRotateNone.Tag = false;
            RadioButtonRotate90.Tag = false;
            RadioButtonRotate180.Tag = false;
            RadioButtonRotate270.Tag = false;


            ComboBoxTaskOrderConversion.Tag = false;
            ComboBoxTaskOrderResize.Tag = false;
            ComboBoxTaskOrderRotation.Tag = false;
            ComboBoxTaskOrderSplit.Tag = false;

            CheckBoxSplitDoublePages.Tag = false;
            TextBoxSplitPageAt.Tag = false;
            ComboBoxSplitAtType.Tag = false;
            TextBoxResizePageIndexReference.Tag = false;
            TextBoxResizeW.Tag = false;
            TextBoxResizeH.Tag = false;
            ComboBoxConvertPages.Tag = false;
            CheckBoxDontStretch.Tag = false;
            TextboxResizePercentage.Tag = false;
            CheckboxKeepAspectratio.Tag = false;
            PictureBoxColorSelect.Tag = false;
            CheckBoxSplitOnlyIfDoubleSize.Tag = false;
            CheckBoxSplitDoublepagesFirst.Tag = false;

        }

        private void UpdateImageAdjustments(object sender, ImageTaskAssignment selected, bool dontUpdate = false)
        {
            ImageTask selectedTask = null;
            Page page = null;

            if (selected != null)
            {

                if (selected.ImageTask != null && !WindowClosed)
                {

                    Invoke(new Action(() =>
                    {
                        //ImageQualityTrackBar.Value = selectedTask.ImageAdjustments.Quality;
                        switch (selected.ImageTask.ImageAdjustments.ResizeMode)
                        {
                            case 0:
                                RadioButtonResizeNever.Tag = dontUpdate;
                                RadioButtonResizeNever.Checked = true;
                                break;
                            case 1:
                                RadioButtonResizeIfLarger.Tag = dontUpdate;
                                RadioButtonResizeIfLarger.Checked = true;
                                break;
                            case 2:
                                RadioButtonResizeTo.Tag = dontUpdate;
                                RadioButtonResizeTo.Checked = true;
                                break;
                            case 3:
                                RadioButtonResizePercent.Tag = dontUpdate;
                                RadioButtonResizePercent.Checked = true;
                                break;

                        }

                        ComboBoxTaskOrderConversion.Tag = dontUpdate;
                        ComboBoxTaskOrderResize.Tag = dontUpdate;
                        ComboBoxTaskOrderRotation.Tag = dontUpdate;
                        ComboBoxTaskOrderSplit.Tag = dontUpdate;

                        ComboBoxTaskOrderConversion.SelectedIndex = ((int)selected.ImageTask.TaskOrder.Convert);
                        ComboBoxTaskOrderResize.SelectedIndex = ((int)selected.ImageTask.TaskOrder.Resize);
                        ComboBoxTaskOrderRotation.SelectedIndex = ((int)selected.ImageTask.TaskOrder.Rotate);
                        ComboBoxTaskOrderSplit.SelectedIndex = ((int)selected.ImageTask.TaskOrder.Split);
                        //PropertyGridTaskOrder.SelectedObject = selectedImageTasks.TaskOrder;

                        switch (selected.ImageTask.ImageAdjustments.RotateMode)
                        {
                            case 0:
                                RadioButtonRotateNone.Tag = dontUpdate;
                                RadioButtonRotateNone.Checked = true;
                                break;
                            case 1:
                                RadioButtonRotate90.Tag = dontUpdate;
                                RadioButtonRotate90.Checked = true;
                                break;
                            case 2:
                                RadioButtonRotate180.Tag = dontUpdate;
                                RadioButtonRotate180.Checked = true;
                                break;
                            case 3:
                                RadioButtonRotate270.Tag = dontUpdate;
                                RadioButtonRotate270.Checked = true;
                                break;

                        }

                        CheckBoxSplitDoublePages.Tag = dontUpdate;
                        TextBoxSplitPageAt.Tag = dontUpdate;
                        ComboBoxSplitAtType.Tag = dontUpdate;
                        TextBoxResizePageIndexReference.Tag = dontUpdate;
                        TextBoxResizeW.Tag = dontUpdate;
                        TextBoxResizeH.Tag = dontUpdate;
                        ComboBoxConvertPages.Tag = dontUpdate;
                        CheckBoxDontStretch.Tag = dontUpdate;
                        TextboxResizePercentage.Tag = dontUpdate;
                        CheckboxKeepAspectratio.Tag = dontUpdate;
                        PictureBoxColorSelect.Tag = dontUpdate;
                        CheckBoxSplitOnlyIfDoubleSize.Tag = dontUpdate;
                        CheckBoxSplitDoublepagesFirst.Tag = dontUpdate;
                        CheckboxIgnoreDoublePages.Tag = dontUpdate;


                        CheckBoxSplitDoublePages.Checked = selected.ImageTask.ImageAdjustments.SplitPage;
                        TextBoxSplitPageAt.Text = selected.ImageTask.ImageAdjustments.SplitPageAt.ToString();
                        ComboBoxSplitAtType.SelectedIndex = selected.ImageTask.ImageAdjustments.SplitType;
                        TextBoxResizePageIndexReference.Text = selected.ImageTask.ImageAdjustments.ResizeToPageNumber.ToString();
                        TextBoxResizeW.Text = selected.ImageTask.ImageAdjustments.ResizeTo.X.ToString();
                        TextBoxResizeH.Text = selected.ImageTask.ImageAdjustments.ResizeTo.Y.ToString();
                        ComboBoxConvertPages.SelectedIndex = selected.ImageTask.ImageAdjustments.ConvertType;
                        CheckBoxDontStretch.Checked = selected.ImageTask.ImageAdjustments.DontStretch;
                        TextboxResizePercentage.Text = selected.ImageTask.ImageAdjustments.ResizeToPercentage.ToString();
                        CheckboxKeepAspectratio.Checked = selected.ImageTask.ImageAdjustments.KeepAspectRatio;
                        PictureBoxColorSelect.BackColor = selected.ImageTask.ImageAdjustments.DetectSplitAtColor;
                        CheckBoxSplitOnlyIfDoubleSize.Checked = selected.ImageTask.ImageAdjustments.SplitOnlyDoublePages;
                        CheckBoxSplitDoublepagesFirst.Checked = selected.ImageTask.ImageAdjustments.SplitDoublePagesFirstResizingToPage;
                        CheckboxIgnoreDoublePages.Checked = selected.ImageTask.ImageAdjustments.IgnoreDoublePagesResizingToPage;


                        RadioButtonResizeNever.Tag = false;
                        RadioButtonResizeIfLarger.Tag = false;
                        RadioButtonResizeTo.Tag = false;
                        RadioButtonResizePercent.Tag = false;

                        RadioButtonRotateNone.Tag = false;
                        RadioButtonRotate90.Tag = false;
                        RadioButtonRotate180.Tag = false;
                        RadioButtonRotate270.Tag = false;


                        ComboBoxTaskOrderConversion.Tag = false;
                        ComboBoxTaskOrderResize.Tag = false;
                        ComboBoxTaskOrderRotation.Tag = false;
                        ComboBoxTaskOrderSplit.Tag = false;

                        CheckBoxSplitDoublePages.Tag = false;
                        TextBoxSplitPageAt.Tag = false;
                        ComboBoxSplitAtType.Tag = false;
                        TextBoxResizePageIndexReference.Tag = false;
                        TextBoxResizeW.Tag = false;
                        TextBoxResizeH.Tag = false;
                        ComboBoxConvertPages.Tag = false;
                        CheckBoxDontStretch.Tag = false;
                        TextboxResizePercentage.Tag = false;
                        CheckboxKeepAspectratio.Tag = false;
                        PictureBoxColorSelect.Tag = false;
                        CheckBoxSplitOnlyIfDoubleSize.Tag = false;
                        CheckBoxSplitDoublepagesFirst.Tag = false;
                        CheckboxIgnoreDoublePages.Tag = false;

                    }));

                }
            }
        }

        private void HandleImageAdjustmentsChanged(object sender, ImageAdjustmentsChangedEvent e)
        {
            Invoke(() =>
            {
                bool updateCtls = false;

                if (e.Page != null)
                {
                    updateCtls = true;
                }

                ImageTaskAssignment assignment = null;

                foreach (ListViewItem item in ImageTaskListView.Items)
                {
                    assignment = item.Tag as ImageTaskAssignment;
                    if (assignment != null)
                    {
                        Page page = assignment.Pages.Find(p => p.Id == e.Page.Id);

                        if (page != null)
                        {
                            updateCtls = true;
                            if (e.Remove)
                            {
                                assignment.Pages.Remove(page);

                                item.Tag = assignment;
                                item.Text = assignment.GetAssignedTaskName();
                                item.SubItems[1].Text = assignment.GetAssignedPageNumbers();

                            }

                            break;
                        }
                    }
                }

                if (assignment != null)
                {
                    if (assignment.Pages.Count == 0)
                    {
                        ListViewItem[] r = ImageTaskListView.Items.Find(assignment.Key, true);

                        if (r.Length > 0)
                        {
                            ImageTaskListView.Items.Remove(r[0]);

                            updateCtls = false;
                        }
                    }
                }


                if (updateCtls && assignment != null)
                {
                    bool dontUpdate = true;
                    //ImageQualityTrackBar.Value = selectedTask.ImageAdjustments.Quality;
                    switch (assignment.ImageTask.ImageAdjustments.ResizeMode)
                    {
                        case 0:
                            RadioButtonResizeNever.Tag = dontUpdate;
                            RadioButtonResizeNever.Checked = true;
                            break;
                        case 1:
                            RadioButtonResizeIfLarger.Tag = dontUpdate;
                            RadioButtonResizeIfLarger.Checked = true;
                            break;
                        case 2:
                            RadioButtonResizeTo.Tag = dontUpdate;
                            RadioButtonResizeTo.Checked = true;
                            break;
                        case 3:
                            RadioButtonResizePercent.Tag = dontUpdate;
                            RadioButtonResizePercent.Checked = true;
                            break;

                    }

                    ComboBoxTaskOrderConversion.Tag = dontUpdate;
                    ComboBoxTaskOrderResize.Tag = dontUpdate;
                    ComboBoxTaskOrderRotation.Tag = dontUpdate;
                    ComboBoxTaskOrderSplit.Tag = dontUpdate;

                    ComboBoxTaskOrderConversion.SelectedIndex = ((int)assignment.ImageTask.TaskOrder.Convert);
                    ComboBoxTaskOrderResize.SelectedIndex = ((int)assignment.ImageTask.TaskOrder.Resize);
                    ComboBoxTaskOrderRotation.SelectedIndex = ((int)assignment.ImageTask.TaskOrder.Rotate);
                    ComboBoxTaskOrderSplit.SelectedIndex = ((int)assignment.ImageTask.TaskOrder.Split);
                    //PropertyGridTaskOrder.SelectedObject = selectedImageTasks.TaskOrder;

                    switch (assignment.ImageTask.ImageAdjustments.RotateMode)
                    {
                        case 0:
                            RadioButtonRotateNone.Tag = dontUpdate;
                            RadioButtonRotateNone.Checked = true;
                            break;
                        case 1:
                            RadioButtonRotate90.Tag = dontUpdate;
                            RadioButtonRotate90.Checked = true;
                            break;
                        case 2:
                            RadioButtonRotate180.Tag = dontUpdate;
                            RadioButtonRotate180.Checked = true;
                            break;
                        case 3:
                            RadioButtonRotate270.Tag = dontUpdate;
                            RadioButtonRotate270.Checked = true;
                            break;

                    }

                    CheckBoxSplitDoublePages.Tag = dontUpdate;
                    TextBoxSplitPageAt.Tag = dontUpdate;
                    ComboBoxSplitAtType.Tag = dontUpdate;
                    TextBoxResizePageIndexReference.Tag = dontUpdate;
                    TextBoxResizeW.Tag = dontUpdate;
                    TextBoxResizeH.Tag = dontUpdate;
                    ComboBoxConvertPages.Tag = dontUpdate;
                    CheckBoxDontStretch.Tag = dontUpdate;
                    TextboxResizePercentage.Tag = dontUpdate;
                    CheckboxKeepAspectratio.Tag = dontUpdate;
                    PictureBoxColorSelect.Tag = dontUpdate;
                    CheckBoxSplitOnlyIfDoubleSize.Tag = dontUpdate;
                    CheckBoxSplitDoublepagesFirst.Tag = dontUpdate;
                    CheckboxIgnoreDoublePages.Tag = dontUpdate;


                    CheckBoxSplitDoublePages.Checked = assignment.ImageTask.ImageAdjustments.SplitPage;
                    TextBoxSplitPageAt.Text = assignment.ImageTask.ImageAdjustments.SplitPageAt.ToString();
                    ComboBoxSplitAtType.SelectedIndex = assignment.ImageTask.ImageAdjustments.SplitType;
                    TextBoxResizePageIndexReference.Text = assignment.ImageTask.ImageAdjustments.ResizeToPageNumber.ToString();
                    TextBoxResizeW.Text = assignment.ImageTask.ImageAdjustments.ResizeTo.X.ToString();
                    TextBoxResizeH.Text = assignment.ImageTask.ImageAdjustments.ResizeTo.Y.ToString();
                    ComboBoxConvertPages.SelectedIndex = assignment.ImageTask.ImageAdjustments.ConvertType;
                    CheckBoxDontStretch.Checked = assignment.ImageTask.ImageAdjustments.DontStretch;
                    TextboxResizePercentage.Text = assignment.ImageTask.ImageAdjustments.ResizeToPercentage.ToString();
                    CheckboxKeepAspectratio.Checked = assignment.ImageTask.ImageAdjustments.KeepAspectRatio;
                    PictureBoxColorSelect.BackColor = assignment.ImageTask.ImageAdjustments.DetectSplitAtColor;
                    CheckBoxSplitOnlyIfDoubleSize.Checked = assignment.ImageTask.ImageAdjustments.SplitOnlyDoublePages;
                    CheckBoxSplitDoublepagesFirst.Checked = assignment.ImageTask.ImageAdjustments.SplitDoublePagesFirstResizingToPage;

                    CheckboxIgnoreDoublePages.Checked = assignment.ImageTask.ImageAdjustments.IgnoreDoublePagesResizingToPage;

                    RadioButtonResizeNever.Tag = false;
                    RadioButtonResizeIfLarger.Tag = false;
                    RadioButtonResizeTo.Tag = false;
                    RadioButtonResizePercent.Tag = false;

                    RadioButtonRotateNone.Tag = false;
                    RadioButtonRotate90.Tag = false;
                    RadioButtonRotate180.Tag = false;
                    RadioButtonRotate270.Tag = false;


                    ComboBoxTaskOrderConversion.Tag = false;
                    ComboBoxTaskOrderResize.Tag = false;
                    ComboBoxTaskOrderRotation.Tag = false;
                    ComboBoxTaskOrderSplit.Tag = false;

                    CheckBoxSplitDoublePages.Tag = false;
                    TextBoxSplitPageAt.Tag = false;
                    ComboBoxSplitAtType.Tag = false;
                    TextBoxResizePageIndexReference.Tag = false;
                    TextBoxResizeW.Tag = false;
                    TextBoxResizeH.Tag = false;
                    ComboBoxConvertPages.Tag = false;
                    CheckBoxDontStretch.Tag = false;
                    TextboxResizePercentage.Tag = false;
                    CheckboxKeepAspectratio.Tag = false;
                    PictureBoxColorSelect.Tag = false;
                    CheckBoxSplitOnlyIfDoubleSize.Tag = false;
                    CheckBoxSplitDoublepagesFirst.Tag = false;
                    CheckboxIgnoreDoublePages.Tag = false;

                }
            });
        }

        private void ComboBoxTaskOrder_SelectedIndexChanged(object sender, EventArgs e)
        {
            var cb = sender as ComboBox;
            bool dontUpdate = cb.Tag != null ? ((bool)cb.Tag) : true;

            if (dontUpdate)
            {
                return;
            }

            if (ImageTaskListView.SelectedItem == null)
            {
                return;
            }

            ImageTaskAssignment selectedImageTasks = ImageTaskListView.SelectedItem.Tag as ImageTaskAssignment;

            switch (cb.Name)
            {
                case "ComboBoxTaskOrderConversion":
                    selectedImageTasks.ImageTask.TaskOrder.Convert = (ImageTaskOrderValue)cb.SelectedIndex;
                    break;
                case "ComboBoxTaskOrderResize":
                    selectedImageTasks.ImageTask.TaskOrder.Resize = (ImageTaskOrderValue)cb.SelectedIndex;
                    break;
                case "ComboBoxTaskOrderRotation":
                    selectedImageTasks.ImageTask.TaskOrder.Rotate = (ImageTaskOrderValue)cb.SelectedIndex;
                    break;
                case "ComboBoxTaskOrderSplit":
                    selectedImageTasks.ImageTask.TaskOrder.Split = (ImageTaskOrderValue)cb.SelectedIndex;
                    break;
            }

            if (!dontUpdate)
            {
                // = selectedImageTasks;

                selectedImageTasks.AssignTaskToPages();
            }
        }

        private void PictureBoxColorSelect_Click(object sender, EventArgs e)
        {
            if (SelectColorDialog.ShowDialog() == DialogResult.OK)
            {
                PictureBoxColorSelect.BackColor = SelectColorDialog.Color;
            }

            Nullable<Color> oldValue;

            if (ImageTaskListView.SelectedItem == null)
            {
                return;
            }

            ImageTaskAssignment selectedImageTasks = ImageTaskListView.SelectedItem.Tag as ImageTaskAssignment;

            if (selectedImageTasks != null)
            {
                bool dontUpdate = PictureBoxColorSelect.Tag != null ? ((bool)PictureBoxColorSelect.Tag) : true;
                oldValue = selectedImageTasks.ImageTask.ImageAdjustments.DetectSplitAtColor;

                selectedImageTasks.ImageTask.ImageAdjustments.DetectSplitAtColor = PictureBoxColorSelect.BackColor;

                if (oldValue != selectedImageTasks.ImageTask.ImageAdjustments.DetectSplitAtColor)
                {

                    if (!dontUpdate)
                    {
                        foreach (Page page in selectedImageTasks.Pages)
                        {
                            AppEventHandler.OnPageChanged(this, new PageChangedEvent(page, null, PageChangedEvent.IMAGE_STATUS_CHANGED, true));
                        }

                        selectedImageTasks.AssignTaskToPages();
                    }


                    AppEventHandler.OnArchiveStatusChanged(this, new ArchiveStatusEvent(Program.ProjectModel, ArchiveStatusEvent.ARCHIVE_FILE_UPDATED));
                }
            }
        }

        private void CheckboxIgnoreDoublePages_CheckedChanged(object sender, EventArgs e)
        {
            Nullable<bool> oldValue;
            CheckBox cb = sender as CheckBox;

            if (ImageTaskListView.SelectedItem == null)
            {
                return;
            }

            ImageTaskAssignment selectedImageTasks = ImageTaskListView.SelectedItem.Tag as ImageTaskAssignment;

            if (selectedImageTasks != null)
            {

                bool dontUpdate = cb.Tag != null ? ((bool)cb.Tag) : true;

                oldValue = selectedImageTasks.ImageTask.ImageAdjustments.IgnoreDoublePagesResizingToPage;


                if (!dontUpdate)
                {
                    selectedImageTasks.ImageTask.ImageAdjustments.IgnoreDoublePagesResizingToPage = CheckboxIgnoreDoublePages.Checked;
                }

                if (CheckboxIgnoreDoublePages.Checked)
                {
                    CheckBoxSplitDoublepagesFirst.Checked = !CheckboxIgnoreDoublePages.Checked;
                }

                if (oldValue != selectedImageTasks.ImageTask.ImageAdjustments.IgnoreDoublePagesResizingToPage)
                {
                    if (!dontUpdate)
                    {
                        foreach (Page page in selectedImageTasks.Pages)
                        {
                            AppEventHandler.OnPageChanged(this, new PageChangedEvent(page, null, PageChangedEvent.IMAGE_STATUS_CHANGED, true));
                        }

                        selectedImageTasks.AssignTaskToPages();
                    }

                    AppEventHandler.OnArchiveStatusChanged(this, new ArchiveStatusEvent(Program.ProjectModel, ArchiveStatusEvent.ARCHIVE_FILE_UPDATED));
                }
            }
        }

        private void CheckBoxSplitDoublepagesFirst_CheckedChanged(object sender, EventArgs e)
        {
            Nullable<bool> oldValue;
            CheckBox cb = sender as CheckBox;

            if (ImageTaskListView.SelectedItem == null)
            {
                return;
            }

            ImageTaskAssignment selectedImageTasks = ImageTaskListView.SelectedItem.Tag as ImageTaskAssignment;

            if (selectedImageTasks != null)
            {
                bool dontUpdate = cb.Tag != null ? ((bool)cb.Tag) : true;

                oldValue = selectedImageTasks.ImageTask.ImageAdjustments.SplitDoublePagesFirstResizingToPage;


                if (!dontUpdate)
                {
                    selectedImageTasks.ImageTask.ImageAdjustments.SplitDoublePagesFirstResizingToPage = CheckBoxSplitDoublepagesFirst.Checked;
                }


                if (CheckBoxSplitDoublepagesFirst.Checked)
                {
                    CheckboxIgnoreDoublePages.Checked = !CheckBoxSplitDoublepagesFirst.Checked;
                }

                if (oldValue != selectedImageTasks.ImageTask.ImageAdjustments.SplitOnlyDoublePages)
                {

                    if (!dontUpdate)
                    {
                        foreach (Page page in selectedImageTasks.Pages)
                        {
                            AppEventHandler.OnPageChanged(this, new PageChangedEvent(page, null, PageChangedEvent.IMAGE_STATUS_CHANGED, true));
                        }

                        selectedImageTasks.AssignTaskToPages();
                    }

                    AppEventHandler.OnArchiveStatusChanged(this, new ArchiveStatusEvent(Program.ProjectModel, ArchiveStatusEvent.ARCHIVE_FILE_UPDATED));
                }
            }
        }

        private void CheckBoxSplitOnlyIfDoubleSize_CheckedChanged(object sender, EventArgs e)
        {
            Nullable<bool> oldValue;
            CheckBox cb = sender as CheckBox;

            if (ImageTaskListView.SelectedItem == null)
            {
                return;
            }

            ImageTaskAssignment selectedImageTasks = ImageTaskListView.SelectedItem.Tag as ImageTaskAssignment;

            if (selectedImageTasks != null)
            {

                bool dontUpdate = cb.Tag != null ? ((bool)cb.Tag) : true;

                oldValue = selectedImageTasks.ImageTask.ImageAdjustments.SplitOnlyDoublePages;


                if (!dontUpdate)
                {
                    selectedImageTasks.ImageTask.ImageAdjustments.SplitOnlyDoublePages = CheckBoxSplitOnlyIfDoubleSize.Checked;
                }

                if (oldValue != selectedImageTasks.ImageTask.ImageAdjustments.SplitOnlyDoublePages)
                {
                    if (!dontUpdate)
                    {
                        foreach (Page page in selectedImageTasks.Pages)
                        {
                            AppEventHandler.OnPageChanged(this, new PageChangedEvent(page, null, PageChangedEvent.IMAGE_STATUS_CHANGED, true));
                        }

                        selectedImageTasks.AssignTaskToPages();
                    }

                    AppEventHandler.OnArchiveStatusChanged(this, new ArchiveStatusEvent(Program.ProjectModel, ArchiveStatusEvent.ARCHIVE_FILE_UPDATED));
                }
            }
        }

        private void ComboBoxConvertPages_SelectedIndexChanged(object sender, EventArgs e)
        {
            Nullable<int> oldValue;
            ComboBox cb = sender as ComboBox;

            if (ImageTaskListView.SelectedItem == null)
            {
                return;
            }

            ImageTaskAssignment selectedImageTasks = ImageTaskListView.SelectedItem.Tag as ImageTaskAssignment;

            if (selectedImageTasks != null && ComboBoxConvertPages.SelectedIndex > -1)
            {
                bool dontUpdate = cb.Tag != null ? ((bool)cb.Tag) : true;

                oldValue = selectedImageTasks.ImageTask.ImageAdjustments.ConvertType;


                if (!dontUpdate)
                {
                    selectedImageTasks.ImageTask.ImageAdjustments.ConvertType = ComboBoxConvertPages.SelectedIndex;
                }

                if (oldValue.Value != selectedImageTasks.ImageTask.ImageAdjustments.ConvertType)
                {
                    if (!dontUpdate)
                    {
                        foreach (Page page in selectedImageTasks.Pages)
                        {
                            AppEventHandler.OnPageChanged(this, new PageChangedEvent(page, null, PageChangedEvent.IMAGE_STATUS_CHANGED, true));
                        }

                        ImageTaskListView.SelectedItem.Text = selectedImageTasks.GetAssignedTaskName();

                        selectedImageTasks.AssignTaskToPages();
                    }

                    AppEventHandler.OnArchiveStatusChanged(this, new ArchiveStatusEvent(Program.ProjectModel, ArchiveStatusEvent.ARCHIVE_FILE_UPDATED));
                }
            }
        }

        private void TextBoxResizePageIndexReference_TextChanged(object sender, EventArgs e)
        {
            int pageNumber = 0;
            TextBox tb = sender as TextBox;
            Nullable<int> oldValue = null;

            if (ImageTaskListView.SelectedItem == null)
            {
                return;
            }

            ImageTaskAssignment selectedImageTasks = ImageTaskListView.SelectedItem.Tag as ImageTaskAssignment;

            if (selectedImageTasks != null)
            {

                bool dontUpdate = tb.Tag != null ? ((bool)tb.Tag) : true;
                oldValue = selectedImageTasks.ImageTask.ImageAdjustments.ResizeToPageNumber;

                if (TextBoxResizePageIndexReference.Text.Length > 0)
                {
                    try
                    {
                        pageNumber = int.Parse(TextBoxResizePageIndexReference.Text);
                    }
                    catch (Exception)
                    {
                        pageNumber = 0;
                    }
                }

                if (!dontUpdate)
                {
                    selectedImageTasks.ImageTask.ImageAdjustments.ResizeToPageNumber = pageNumber;
                }

                if (oldValue != selectedImageTasks.ImageTask.ImageAdjustments.ResizeToPageNumber)
                {
                    if (!dontUpdate)
                    {
                        foreach (Page page in selectedImageTasks.Pages)
                        {
                            AppEventHandler.OnPageChanged(this, new PageChangedEvent(page, null, PageChangedEvent.IMAGE_STATUS_CHANGED, true));
                        }

                        selectedImageTasks.AssignTaskToPages();
                    }

                    AppEventHandler.OnArchiveStatusChanged(this, new ArchiveStatusEvent(Program.ProjectModel, ArchiveStatusEvent.ARCHIVE_FILE_UPDATED));
                }
            }
        }

        private void CheckboxKeepAspectratio_CheckedChanged(object sender, EventArgs e)
        {

            Nullable<bool> oldValue;
            CheckBox cb = sender as CheckBox;

            if (ImageTaskListView.SelectedItem == null)
            {
                return;
            }

            ImageTaskAssignment selectedImageTasks = ImageTaskListView.SelectedItem.Tag as ImageTaskAssignment;

            if (selectedImageTasks != null)
            {

                bool dontUpdate = cb.Tag != null ? ((bool)cb.Tag) : true;

                oldValue = selectedImageTasks.ImageTask.ImageAdjustments.KeepAspectRatio;

                if (!dontUpdate)
                {
                    selectedImageTasks.ImageTask.ImageAdjustments.KeepAspectRatio = CheckboxKeepAspectratio.Checked;
                }

                if (oldValue != selectedImageTasks.ImageTask.ImageAdjustments.KeepAspectRatio)
                {
                    if (!dontUpdate)
                    {
                        foreach (Page page in selectedImageTasks.Pages)
                        {
                            AppEventHandler.OnPageChanged(this, new PageChangedEvent(page, null, PageChangedEvent.IMAGE_STATUS_CHANGED, true));
                        }

                        selectedImageTasks.AssignTaskToPages();
                    }

                    AppEventHandler.OnArchiveStatusChanged(this, new ArchiveStatusEvent(Program.ProjectModel, ArchiveStatusEvent.ARCHIVE_FILE_UPDATED));
                }
            }
        }

        private void TextBoxResizeW_TextChanged(object sender, EventArgs e)
        {
            TextBox tb = sender as TextBox;
            int w = 0;
            Nullable<int> oldValue = null;

            if (ImageTaskListView.SelectedItem == null)
            {
                return;
            }

            ImageTaskAssignment selectedImageTasks = ImageTaskListView.SelectedItem.Tag as ImageTaskAssignment;


            if (selectedImageTasks != null)
            {
                bool dontUpdate = tb.Tag != null ? ((bool)tb.Tag) : true;

                oldValue = selectedImageTasks.ImageTask.ImageAdjustments.ResizeTo.X;

                if (TextBoxResizeW.Text.Length > 0)
                {
                    try
                    {
                        w = int.Parse(TextBoxResizeW.Text);
                    }
                    catch (Exception)
                    {
                        w = 0;
                    }
                    //w = int.Parse(TextBoxResizeW.Text);
                }
                else
                {
                    if (oldValue.HasValue)
                    {
                        w = oldValue.Value;
                    }

                }

                if (w != oldValue && w > 0 && CheckboxKeepAspectratio.Checked)
                {
                    TextBoxResizeH.Text = "";

                    if (!dontUpdate)
                    {
                        selectedImageTasks.ImageTask.ImageAdjustments.ResizeTo = new Point(w, 0);
                    }
                }

                if (!dontUpdate)
                {
                    selectedImageTasks.ImageTask.ImageAdjustments.ResizeTo = new Point(w, selectedImageTasks.ImageTask.ImageAdjustments.ResizeTo.Y);
                }

                if (oldValue != selectedImageTasks.ImageTask.ImageAdjustments.ResizeTo.X)
                {
                    if (!dontUpdate)
                    {
                        foreach (Page page in selectedImageTasks.Pages)
                        {
                            AppEventHandler.OnPageChanged(this, new PageChangedEvent(page, null, PageChangedEvent.IMAGE_STATUS_CHANGED, true));
                        }

                        selectedImageTasks.AssignTaskToPages();
                    }

                    AppEventHandler.OnArchiveStatusChanged(this, new ArchiveStatusEvent(Program.ProjectModel, ArchiveStatusEvent.ARCHIVE_FILE_UPDATED));
                }
            }
        }

        private void TextBoxResizeH_TextChanged(object sender, EventArgs e)
        {
            int h = 0;
            TextBox tb = sender as TextBox;
            Nullable<int> oldValue;

            if (ImageTaskListView.SelectedItem == null)
            {
                return;
            }

            ImageTaskAssignment selectedImageTasks = ImageTaskListView.SelectedItem.Tag as ImageTaskAssignment;

            if (selectedImageTasks != null)
            {

                bool dontUpdate = tb.Tag != null ? ((bool)tb.Tag) : true;

                oldValue = selectedImageTasks.ImageTask.ImageAdjustments.ResizeTo.Y;


                if (TextBoxResizeH.Text.Length > 0)
                {
                    try
                    {
                        h = int.Parse(TextBoxResizeH.Text);
                    }
                    catch (Exception)
                    {
                        h = 0;
                    }
                }
                else
                {
                    if (oldValue.HasValue)
                    {
                        h = oldValue.Value;
                    }
                }

                if (!dontUpdate)
                {
                    if (h != oldValue && h > 0 && CheckboxKeepAspectratio.Checked)
                    {
                        TextBoxResizeW.Text = "";

                        selectedImageTasks.ImageTask.ImageAdjustments.ResizeTo = new Point(0, h);
                    }

                    selectedImageTasks.ImageTask.ImageAdjustments.ResizeTo = new Point(selectedImageTasks.ImageTask.ImageAdjustments.ResizeTo.X, h);
                }

                if (oldValue != selectedImageTasks.ImageTask.ImageAdjustments.ResizeTo.Y)
                {
                    if (!dontUpdate)
                    {
                        foreach (Page page in selectedImageTasks.Pages)
                        {
                            AppEventHandler.OnPageChanged(this, new PageChangedEvent(page, null, PageChangedEvent.IMAGE_STATUS_CHANGED, true));
                        }

                        selectedImageTasks.AssignTaskToPages();
                    }

                    AppEventHandler.OnArchiveStatusChanged(this, new ArchiveStatusEvent(Program.ProjectModel, ArchiveStatusEvent.ARCHIVE_FILE_UPDATED));
                }
            }
        }

        private void TextboxResizePercentage_TextChanged(object sender, EventArgs e)
        {
            float percent = 0;
            TextBox tb = sender as TextBox;
            Nullable<float> oldValue;

            if (ImageTaskListView.SelectedItem == null)
            {
                return;
            }

            ImageTaskAssignment selectedImageTasks = ImageTaskListView.SelectedItem.Tag as ImageTaskAssignment;

            if (selectedImageTasks != null)
            {

                bool dontUpdate = tb.Tag != null ? ((bool)tb.Tag) : true;

                oldValue = selectedImageTasks.ImageTask.ImageAdjustments.ResizeToPercentage;

                if (TextboxResizePercentage.Text.Length > 0)
                {
                    try
                    {
                        percent = float.Parse(TextboxResizePercentage.Text);
                    }
                    catch (Exception)
                    {
                        percent = 100;
                    }
                }
                else
                {
                    if (oldValue.HasValue)
                    {
                        percent = oldValue.Value;
                    }
                }

                if (!dontUpdate)
                {
                    selectedImageTasks.ImageTask.ImageAdjustments.ResizeToPercentage = percent;
                }

                if (oldValue != selectedImageTasks.ImageTask.ImageAdjustments.ResizeToPercentage)
                {
                    if (!dontUpdate)
                    {
                        foreach (Page page in selectedImageTasks.Pages)
                        {
                            AppEventHandler.OnPageChanged(this, new PageChangedEvent(page, null, PageChangedEvent.IMAGE_STATUS_CHANGED, true));
                        }

                        selectedImageTasks.AssignTaskToPages();
                    }

                    AppEventHandler.OnArchiveStatusChanged(this, new ArchiveStatusEvent(Program.ProjectModel, ArchiveStatusEvent.ARCHIVE_FILE_UPDATED));
                }
            }
        }

        private void CheckBoxDontStretch_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            Nullable<bool> oldValue;

            if (ImageTaskListView.SelectedItem == null)
            {
                return;
            }

            ImageTaskAssignment selectedImageTasks = ImageTaskListView.SelectedItem.Tag as ImageTaskAssignment;

            if (selectedImageTasks != null)
            {

                bool dontUpdate = cb.Tag != null ? ((bool)cb.Tag) : true;

                oldValue = selectedImageTasks.ImageTask.ImageAdjustments.DontStretch;


                if (!dontUpdate)
                {
                    selectedImageTasks.ImageTask.ImageAdjustments.DontStretch = CheckBoxDontStretch.Checked;
                }

                if (oldValue != selectedImageTasks.ImageTask.ImageAdjustments.DontStretch)
                {
                    if (!dontUpdate)
                    {
                        foreach (Page page in selectedImageTasks.Pages)
                        {
                            AppEventHandler.OnPageChanged(this, new PageChangedEvent(page, null, PageChangedEvent.IMAGE_STATUS_CHANGED, true));
                        }

                        selectedImageTasks.AssignTaskToPages();
                    }

                    AppEventHandler.OnArchiveStatusChanged(this, new ArchiveStatusEvent(Program.ProjectModel, ArchiveStatusEvent.ARCHIVE_FILE_UPDATED));
                }
            }
        }

        private void TextBoxSplitPageAt_TextChanged(object sender, EventArgs e)
        {
            TextBox tb = sender as TextBox;
            Nullable<int> oldValue;
            string value = "";

            if (ImageTaskListView.SelectedItem == null)
            {
                return;
            }

            ImageTaskAssignment selectedImageTasks = ImageTaskListView.SelectedItem.Tag as ImageTaskAssignment;

            if (selectedImageTasks != null)
            {
                bool dontUpdate = tb.Tag != null ? ((bool)tb.Tag) : true;

                oldValue = selectedImageTasks.ImageTask.ImageAdjustments.SplitPageAt;


                if (TextBoxSplitPageAt.Text.Length > 0)
                {
                    value = TextBoxSplitPageAt.Text;
                }
                else
                {
                    value = "0";
                }

                if (!dontUpdate)
                {
                    selectedImageTasks.ImageTask.ImageAdjustments.SplitPageAt = int.Parse(value);
                }

                if (oldValue != selectedImageTasks.ImageTask.ImageAdjustments.SplitPageAt)
                {
                    if (!dontUpdate)
                    {
                        foreach (Page page in selectedImageTasks.Pages)
                        {
                            AppEventHandler.OnPageChanged(this, new PageChangedEvent(page, null, PageChangedEvent.IMAGE_STATUS_CHANGED, true));
                        }

                        selectedImageTasks.AssignTaskToPages();
                    }

                    AppEventHandler.OnArchiveStatusChanged(this, new ArchiveStatusEvent(Program.ProjectModel, ArchiveStatusEvent.ARCHIVE_FILE_UPDATED));
                }
            }
        }

        private void ComboBoxSplitAtType_SelectedIndexChanged(object sender, EventArgs e)
        {
            Nullable<int> oldValue;
            ComboBox cb = sender as ComboBox;

            if (ImageTaskListView.SelectedItem == null)
            {
                return;
            }

            ImageTaskAssignment selectedImageTasks = ImageTaskListView.SelectedItem.Tag as ImageTaskAssignment;

            if (selectedImageTasks != null)
            {
                bool dontUpdate = cb.Tag != null ? ((bool)cb.Tag) : true;

                oldValue = selectedImageTasks.ImageTask.ImageAdjustments.SplitType;


                if (!dontUpdate)
                {
                    selectedImageTasks.ImageTask.ImageAdjustments.SplitType = ComboBoxSplitAtType.SelectedIndex;
                }

                if (oldValue.Value != selectedImageTasks.ImageTask.ImageAdjustments.SplitType)
                {
                    if (!dontUpdate)
                    {
                        foreach (Page page in selectedImageTasks.Pages)
                        {
                            AppEventHandler.OnPageChanged(this, new PageChangedEvent(page, null, PageChangedEvent.IMAGE_STATUS_CHANGED, true));
                        }

                        selectedImageTasks.AssignTaskToPages();
                    }

                    AppEventHandler.OnArchiveStatusChanged(this, new ArchiveStatusEvent(Program.ProjectModel, ArchiveStatusEvent.ARCHIVE_FILE_UPDATED));
                }
            }
        }

        private void CheckBoxSplitDoublePages_CheckedChanged(object sender, EventArgs e)
        {
            Nullable<bool> oldValue;
            CheckBox cb = sender as CheckBox;

            if (ImageTaskListView.SelectedItem == null)
            {
                return;
            }

            ImageTaskAssignment selectedImageTasks = ImageTaskListView.SelectedItem.Tag as ImageTaskAssignment;

            if (selectedImageTasks != null)
            {

                bool dontUpdate = cb.Tag != null ? ((bool)cb.Tag) : true;

                oldValue = selectedImageTasks.ImageTask.ImageAdjustments.SplitPage;

                if (!dontUpdate)
                {
                    selectedImageTasks.ImageTask.ImageAdjustments.SplitPage = CheckBoxSplitDoublePages.Checked;
                }

                if (oldValue.Value != selectedImageTasks.ImageTask.ImageAdjustments.SplitPage)
                {
                    if (!dontUpdate)
                    {
                        foreach (Page page in selectedImageTasks.Pages)
                        {
                            AppEventHandler.OnPageChanged(this, new PageChangedEvent(page, null, PageChangedEvent.IMAGE_STATUS_CHANGED, true));
                        }

                        ImageTaskListView.SelectedItem.Text = selectedImageTasks.GetAssignedTaskName();

                        selectedImageTasks.AssignTaskToPages();
                    }

                    AppEventHandler.OnArchiveStatusChanged(this, new ArchiveStatusEvent(Program.ProjectModel, ArchiveStatusEvent.ARCHIVE_FILE_UPDATED));
                }
            }
        }

        private void DebugToolsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            df?.Show();
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

                items.Add(lvi);

            }
            DataObject data = new DataObject();
            data.SetData(typeof(System.Windows.Forms.ListView.SelectedListViewItemCollection), PagesList.SelectedItems);
            // pass the Items to move...
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
                borderPen = new Pen(Theme.GetInstance().AccentColor, 2);
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
                    if (RequestThumbnailThread != null)
                    {
                        if (RequestThumbnailThread.IsAlive)
                        {

                            e.Graphics.DrawImage(global::Win_CBZ.Properties.Resources.placeholder_image, new Point(center + 2, e.Bounds.Y + 4));
                            return;
                        }
                    }

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

                        if (!page.Closed && !page.ThumbnailError)
                        {
                            if (ThumbnailPagesSlice.IndexOf(page) == -1)
                            {
                                ThumbnailPagesSlice.Add(page);
                            }

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


            ListViewItem listItem = null;
            foreach (Page item in selectedPages)
            {
                if (item.Index > -1)
                {
                    if (PagesList.Items.Count > item.Index)
                    {
                        listItem = FindListViewItemForPage(PagesList, item);
                        if (listItem != null)
                        {
                            listItem.Selected = true;
                        }
                    }
                }
            }
        }

        private void SelectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {

            Task.Factory.StartNew(() =>
            {

                Invoke(new Action(() =>
                {
                    this.Cursor = Cursors.WaitCursor;

                    TextBox textBox = null;

                    if (PagesList.Focused)
                    {
                        foreach (ListViewItem item in PagesList.Items)
                        {
                            item.Selected = true;
                        }

                    }
                    else if (MessageLogListView.Focused)
                    {
                        foreach (ListViewItem item in MessageLogListView.Items)
                        {
                            item.Selected = true;
                        }
                    }
                    else if (MetaDataGrid.Focused)
                    {
                        MetaDataGrid.SelectAll();
                    }
                    else if (ImageTaskListView.Focused)
                    {
                        foreach (ListViewItem item in ImageTaskListView.Items)
                        {
                            item.Selected = true;
                        }
                    }
                    else
                    {
                        textBox = GetActiveTextBox() as TextBox;

                        if (textBox != null)
                        {
                            textBox.SelectAll();
                        }
                    }
                }));
            }).ContinueWith(t =>
            {
                Invoke(new Action(() =>
                {
                    this.Cursor = Cursors.Default;
                }));
            });
        }

        private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<Page> selectedPages = new List<Page>();
            MemoryStream ms;

            if (PagesList.Focused)
            {
                foreach (ListViewItem itempage in PagesList.SelectedItems)
                {
                    selectedPages.Add(itempage.Tag as Page);
                }

                if (selectedPages.Count > 0)
                {
                    String xmlTextPages = "";
                    var utf8WithoutBom = new System.Text.UTF8Encoding(false);
                    Page copyPage;

                    Cursor = Cursors.WaitCursor;

                    foreach (Page p in selectedPages)
                    {
                        try
                        {
                            p.LoadImageInfo(true);  // load image info in full to include in copy

                            copyPage = new Page(p, true);
                            ms = copyPage.Serialize(Program.ProjectModel.ProjectGUID, true);

                            String metaData = utf8WithoutBom.GetString(ms.ToArray());
                            xmlTextPages += metaData;

                            xmlTextPages += "\r\n";
                        }
                        catch (PageException ex)
                        {
                            if (ex.ShowErrorDialog)
                            {
                                ApplicationMessage.ShowException(ex);
                            }
                        }
                    }

                    DataObject data = new DataObject();
                    data.SetData(DataFormats.UnicodeText, xmlTextPages);

                    Clipboard.SetDataObject(data);

                    PasteToolStripMenuItem.Enabled = true;

                    Cursor = Cursors.Default;
                }
                else
                {
                    PasteToolStripMenuItem.Enabled = false;
                }
            }
            else if (MessageLogListView.Focused)
            {
                String logMessages = "";
                String messageType = "Info";

                foreach (ListViewItem itemMessage in MessageLogListView.SelectedItems)
                {
                    switch (itemMessage.ImageIndex)
                    {
                        case 0:
                            messageType = "INFO";
                            break;
                        case 1:
                            messageType = "WARN";
                            break;
                        case 2:
                            messageType = "ERR";
                            break;
                    }

                    logMessages += "[" + messageType + "]" + "\t[" + itemMessage.SubItems[1].Text + "]\t" + itemMessage.SubItems[2].Text + "\r\n";
                }

                DataObject data = new DataObject();
                data.SetData(DataFormats.UnicodeText, logMessages);

                Clipboard.SetDataObject(data);

                PasteToolStripMenuItem.Enabled = true;
            }
            else if (MetaDataGrid.Focused)
            {
                try
                {
                    MemoryStream copyMemStream = new MemoryStream();
                    var utf8WithoutBom = new System.Text.UTF8Encoding(false);

                    MemoryStream fullCopy = Program.ProjectModel.MetaData.BuildComicInfoXMLStream();

                    XmlDocument doc = new XmlDocument
                    {
                        PreserveWhitespace = true,
                    };

                    XmlReader MetaDataReader = XmlReader.Create(fullCopy);
                    MetaDataReader.Read();
                    doc.Load(MetaDataReader);

                    doc.Save(copyMemStream);
                    copyMemStream.Position = 0;

                    byte[] encoded = new byte[copyMemStream.Length];
                    copyMemStream.Read(encoded, 0, (int)copyMemStream.Length);

                    DataObject data = new DataObject();
                    data.SetData(DataFormats.UnicodeText, utf8WithoutBom.GetString(encoded));

                    Clipboard.SetDataObject(data);

                    PasteToolStripMenuItem.Enabled = true;

                    copyMemStream.Close();
                    fullCopy.Close();
                }
                catch (Exception ex)
                {
                    ApplicationMessage.ShowException(ex);
                }
            }
            else
            {
                TextBox textBox = this.GetActiveTextBox() as TextBox;
                if (textBox != null)
                {
                    if (textBox.SelectedText.Length > 0)
                    {
                        textBox.Copy();
                    }
                }

                PasteToolStripMenuItem.Enabled = Clipboard.ContainsText();
            }
        }

        private void PasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IDataObject clipObject = Clipboard.GetDataObject();
            List<String> pageXMLLines = new List<String>();
            List<Page> copiedPagesList = new List<Page>();
            var utf8WithoutBom = new System.Text.UTF8Encoding(false);
            int pagesUpdated = 0;
            ListViewItem selectedItem = PagesList.SelectedItem;
            Page selectedPage = null;

            try
            {
                if (selectedItem != null)
                {
                    selectedPage = selectedItem.Tag as Page;
                }


                String copiedPages = clipObject.GetData(DataFormats.UnicodeText) as String;
                if (copiedPages.Contains("<?xml") && copiedPages.Contains("<Win_CBZ_Page>"))
                {

                    if (copiedPages.Length > 0)
                    {
                        pageXMLLines.AddRange(copiedPages.Split("\r\n").ToArray());

                    }

                    TokenStore.GetInstance().ResetCancellationToken(TokenStore.TOKEN_SOURCE_UPDATE_PAGE);

                    Task<bool> pastePagesTask = new Task<bool>((token) =>
                    {
                        CancellationToken ct = (CancellationToken)token;

                        foreach (String line in pageXMLLines)
                        {
                            if (line.Length > 0)
                            {
                                if (line.Contains("<?xml") && line.Contains("<Win_CBZ_Page>"))
                                {
                                    MemoryStream ms = new MemoryStream();
                                    byte[] bytes = utf8WithoutBom.GetBytes(line);
                                    Page newPage = null;

                                    ms.Write(bytes, 0, bytes.Length);
                                    ms.Position = 0;

                                    try
                                    {
                                        if (line.Contains("<Win_CBZ_Page>"))
                                        {
                                            newPage = new Page(ms);
                                            Page existingPage = Program.ProjectModel.GetPageById(newPage.Id);

                                            newPage.LoadImageInfo();

                                            if (existingPage == null)
                                            {
                                                newPage.Changed = false;
                                                newPage.Name = "Copy_" + newPage.Name + "";

                                                if (MetaDataVersionFlavorHandler.GetInstance().TargetVersion() == PageIndexVersion.VERSION_1)
                                                {
                                                    newPage.Key = newPage.Name;
                                                }

                                                if (selectedPage != null)
                                                {
                                                    Program.ProjectModel.Pages.Insert(Program.ProjectModel.Pages.IndexOf(selectedPage), newPage);
                                                }
                                                else
                                                {
                                                    Program.ProjectModel.Pages.Add(newPage);
                                                }

                                                AppEventHandler.OnTaskProgress(this, new TaskProgressEvent(newPage, pagesUpdated, pageXMLLines.Count));
                                                AppEventHandler.OnPageChanged(this, new PageChangedEvent(newPage, selectedPage, PageChangedEvent.IMAGE_STATUS_NEW));
                                                AppEventHandler.OnArchiveStatusChanged(this, new ArchiveStatusEvent(Program.ProjectModel, ArchiveStatusEvent.ARCHIVE_FILE_ADDED));
                                            }
                                            else
                                            {
                                                Program.ProjectModel.Pages.Add(newPage);
                                                if (selectedPage != null)
                                                {
                                                    Program.ProjectModel.Pages.Insert(Program.ProjectModel.Pages.IndexOf(selectedPage), newPage);
                                                }
                                                else
                                                {
                                                    Program.ProjectModel.Pages.Add(newPage);
                                                }

                                                AppEventHandler.OnTaskProgress(this, new TaskProgressEvent(newPage, pagesUpdated, pageXMLLines.Count));
                                                AppEventHandler.OnPageChanged(this, new PageChangedEvent(newPage, selectedPage, PageChangedEvent.IMAGE_STATUS_NEW));
                                                AppEventHandler.OnArchiveStatusChanged(this, new ArchiveStatusEvent(Program.ProjectModel, ArchiveStatusEvent.ARCHIVE_FILE_UPDATED));
                                            }

                                            ct.ThrowIfCancellationRequested();

                                            pagesUpdated++;
                                            Thread.Sleep(5);
                                        }
                                    }
                                    catch (OperationCanceledException oc)
                                    {
                                        break;
                                    }
                                    catch (Exception ex)
                                    {
                                        ApplicationMessage.ShowException(ex);
                                    }
                                    finally
                                    {
                                        newPage?.FreeImage();
                                    }
                                }
                            }
                        }

                        AppEventHandler.OnArchiveStatusChanged(this, new ArchiveStatusEvent(Program.ProjectModel, ArchiveStatusEvent.ARCHIVE_READY));

                        if (pagesUpdated > 0)
                        {

                            TokenStore.GetInstance().ResetCancellationToken(TokenStore.TOKEN_SOURCE_UPDATE_PAGE_INDEX);

                            AppEventHandler.OnGlobalActionRequired(this,
                                new GlobalActionRequiredEvent(Program.ProjectModel,
                                    0,
                                    "Page order changed. Rebuild pageindex now?",
                                    "Rebuild",
                                    GlobalActionRequiredEvent.TASK_TYPE_INDEX_REBUILD,
                                    UpdatePageIndexTask.UpdatePageIndex(Program.ProjectModel.Pages,
                                        AppEventHandler.OnGeneralTaskProgress,
                                        AppEventHandler.OnPageChanged,
                                        TokenStore.GetInstance().CancellationTokenSourceForName(TokenStore.TOKEN_SOURCE_UPDATE_PAGE_INDEX).Token,
                                        false,
                                        true
                                    )
                                ));

                        }

                        return true;
                    }, TokenStore.GetInstance().CancellationTokenSourceForName(TokenStore.TOKEN_SOURCE_UPDATE_PAGE).Token);

                    pastePagesTask.Start();
                }
                else if (copiedPages.Contains("<?xml") && copiedPages.Contains("<ComicInfo>"))
                {
                    DialogResult r = ApplicationMessage.ShowConfirmation("Do you want to replace all ComicInfo attributes with pasted ComicInfo.xml attributes?", "Replace Metadata", ApplicationMessage.DialogType.MT_CONFIRMATION, ApplicationMessage.DialogButtons.MB_YES | ApplicationMessage.DialogButtons.MB_NO);
                    if (r == DialogResult.Yes)
                    {
                        MemoryStream xmlStream = new MemoryStream();
                        byte[] bytes = utf8WithoutBom.GetBytes(copiedPages.Replace("\r\n", ""));

                        xmlStream.Write(bytes, 0, bytes.Length);
                        xmlStream.Position = 0;

                        MetaData data = new MetaData(xmlStream, Win_CBZSettings.Default.MetaDataFilename);

                        RemoveMetaData();

                        Program.ProjectModel.MetaData.Values = data.Values;
                        //Program.ProjectModel.MetaData.RebuildPageMetaData(Program.ProjectModel.Pages, MetaDataVersionFlavorHandler.GetInstance().HandlePageIndexVersion());

                        if (Program.ProjectModel.MetaData.Values.Count > 0)
                        {
                            if (ApplyUserKeyFilter)
                            {
                                Program.ProjectModel.MetaData.UserFilterMetaData(Win_CBZSettings.Default.KeyFilter?.OfType<string>().ToArray(), Win_CBZSettings.Default.KeyFilterBaseContitionType).FilterMetaData(ToolBarSearchInput.Text);
                            }
                            else
                            {
                                Program.ProjectModel.MetaData.UserFilterMetaData().FilterMetaData(ToolBarSearchInput.Text);
                            }


                        }

                        //AppEventHandler.OnMetaDataLoaded(this, new MetaDataLoadEvent(data.Values.ToList()));
                        AppEventHandler.OnMetaDataLoaded(this, new MetaDataLoadEvent(Program.ProjectModel.MetaData.Values.ToList()));
                        //AppEventHandler.OnMetaDataChanged(this, new MetaDataChangedEvent(MetaDataChangedEvent.METADATA_UPDATED, Program.ProjectModel.MetaData));
                    }
                }
                else
                {
                    TextBox textBox = this.GetActiveTextBox() as TextBox;
                    if (textBox != null)
                    {
                        textBox.Paste();
                    }
                }

            }
            catch (Exception ex)
            {
                MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_ERROR, ex.Message);

                ApplicationMessage.ShowException(ex);
            }
        }

        private Control GetActiveTextBox()
        {
            TextBox activeTextBox = null;

            SplitContainer activeContainer = ActiveControl as SplitContainer;
            if (activeContainer != null)
            {
                while (activeContainer != null)
                {
                    activeContainer = activeContainer.ActiveControl as SplitContainer;
                    if (activeContainer != null)
                    {
                        activeTextBox = activeContainer.ActiveControl as TextBox;
                    }


                }
            }

            return activeTextBox;
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            while (!backgroundWorker1.CancellationPending)
            {
                if (!WindowClosed)
                {
                    try
                    {
                        MenuBar.Invoke(new Action(() =>
                        {
                            PasteToolStripMenuItem.Enabled = Clipboard.ContainsText();
                        }));
                    }
                    catch (Exception) { }
                }


                Thread.Sleep(2000);
            }

        }

        private void PageThumbsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void ToolButtonEditImage_Click(object sender, EventArgs e)
        {
            if (!Program.DebugMode)
            {
                ApplicationMessage.ShowWarning("Not yet implemented", "Not implemented", ApplicationMessage.DialogType.MT_WARNING, ApplicationMessage.DialogButtons.MB_OK);

                return;
            }

            ListViewItem selectedItem = PagesList.SelectedItem as ListViewItem;

            if (selectedItem != null)
            {
                Page selected = selectedItem.Tag as Page;

                if (selected != null)
                {
                    EditImageForm editForm = new EditImageForm(selected);
                    editForm.ShowDialog();
                }
            }

        }

        private void AutoCompleteItems_Selected(object sender, AutocompleteMenuNS.SelectedEventArgs e)
        {
            e.Control.Text = e.Item.Text;
        }

        private void MetaDataGrid_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                e.Handled = true;
            }
        }

        private void FileNameLabel_Click(object sender, EventArgs e)
        {
            if (FileNameLabel.Text.Length > 0)
            {
                LocalFile helper = new LocalFile(FileNameLabel.Text);

                if (helper.Exists())
                {
                    Process.Start(new ProcessStartInfo(helper.FilePath) { UseShellExecute = true });
                }
            }
        }

        private void ToolBarSearchInput_TextChanged(object sender, EventArgs e)
        {
            if (Program.ProjectModel.MetaData.Values.Count > 0)
            {
                if (ApplyUserKeyFilter)
                {
                    Program.ProjectModel.MetaData.UserFilterMetaData(Win_CBZSettings.Default.KeyFilter?.OfType<string>().ToArray(), Win_CBZSettings.Default.KeyFilterBaseContitionType).FilterMetaData(ToolBarSearchInput.Text);
                }
                else
                {
                    Program.ProjectModel.MetaData.UserFilterMetaData().FilterMetaData(ToolBarSearchInput.Text);
                }

                AppEventHandler.OnMetaDataLoaded(this, new MetaDataLoadEvent(Program.ProjectModel.MetaData.Values.ToList()));
            }
        }

        private void GetImageProcessExcludesFromSelectedButton_Click(object sender, EventArgs e)
        {
            List<String> excludes = new List<String>();
            foreach (ListViewItem item in PagesList.SelectedItems)
            {
                excludes.Add(((Page)item.Tag).Name);
            }

            TextBoxExcludePagesImageProcessing.Lines = excludes.ToArray();
        }

        private void TextBoxExcludePagesImageProcessing_TextChanged(object sender, EventArgs e)
        {
            Program.ProjectModel.ConversionExcludes.Clear();
            Program.ProjectModel.ConversionExcludes.AddRange(TextBoxExcludePagesImageProcessing.Lines);
        }

        private void PagesList_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void ButtonConfigureKeyFilter_Click(object sender, EventArgs e)
        {
            UserFilerKeysForm userFilerKeysForm = new UserFilerKeysForm();
            if (userFilerKeysForm.ShowDialog() == DialogResult.OK)
            {
                Win_CBZSettings.Default.KeyFilter = new System.Collections.Specialized.StringCollection();
                Win_CBZSettings.Default.KeyFilterBaseContitionType = userFilerKeysForm.BaseContitionType;

                foreach (String key in userFilerKeysForm.FilterKeys)
                {
                    Win_CBZSettings.Default.KeyFilter.Add(key);
                }

                if (Program.ProjectModel.MetaData.Values.Count > 0)
                {
                    if (ApplyUserKeyFilter)
                    {
                        Program.ProjectModel.MetaData.UserFilterMetaData(Win_CBZSettings.Default.KeyFilter?.OfType<string>().ToArray(), Win_CBZSettings.Default.KeyFilterBaseContitionType).FilterMetaData(ToolBarSearchInput.Text);
                    }

                    AppEventHandler.OnMetaDataLoaded(this, new MetaDataLoadEvent(Program.ProjectModel.MetaData.Values.ToList()));
                }
            }
        }

        private void ButtonFilter_Click(object sender, EventArgs e)
        {
            ApplyUserKeyFilter = !ApplyUserKeyFilter;

            ButtonFilter.BackColor = ApplyUserKeyFilter ? Theme.GetInstance().AccentColor : SystemColors.Control;
            ButtonFilter.Image = ApplyUserKeyFilter ? Resources.funnel_error_16 : Resources.funnel;

            if (Program.ProjectModel.MetaData.Values.Count > 0)
            {
                if (ApplyUserKeyFilter)
                {
                    Program.ProjectModel.MetaData.UserFilterMetaData(Win_CBZSettings.Default.KeyFilter?.OfType<string>().ToArray(), Win_CBZSettings.Default.KeyFilterBaseContitionType).FilterMetaData(ToolBarSearchInput.Text);
                }
                else
                {
                    Program.ProjectModel.MetaData.UserFilterMetaData().FilterMetaData(ToolBarSearchInput.Text);
                }

                AppEventHandler.OnMetaDataLoaded(this, new MetaDataLoadEvent(Program.ProjectModel.MetaData.Values.ToList()));
            }

            Win_CBZSettings.Default.KeyFilterActive = ApplyUserKeyFilter;
        }

        private void ComboBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0)
            {
                return;
            }

            if (sender as ComboBox == null)
            {
                return;
            }

            Pen pen = new Pen(Color.Black, 1);
            Font font = new Font("Verdana", 9f, FontStyle.Regular);

            if (e.State.HasFlag(DrawItemState.Selected))
            {
                e.Graphics.FillRectangle(new SolidBrush(Theme.GetInstance().AccentColor), e.Bounds);
            }
            else
            {
                e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(255, 255, 255)), e.Bounds);
            }

            String icon = ((ComboBox)sender).Tag as String;

            if (ComboIcons.Images.ContainsKey(icon))
            {
                Image img = ComboIcons.Images[icon];
                e.Graphics.DrawImage(img, new Point(e.Bounds.X, e.Bounds.Y));
                e.Graphics.DrawString(((ComboBox)sender).Items[e.Index].ToString(), font, new SolidBrush(Color.Black), new PointF(e.Bounds.X + 18, e.Bounds.Y + 1));
            }
            else
            {
                e.Graphics.DrawString(((ComboBox)sender).Items[e.Index].ToString(), font, new SolidBrush(Color.Black), new PointF(e.Bounds.X + 1, e.Bounds.Y + 1));
            }
        }

        private void BookmarksToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Win_CBZSettings.Default.WriteXmlPageIndex && !Program.ProjectModel.MetaData.Exists())
            {
                DialogResult res = ApplicationMessage.ShowConfirmation("Currently no metadata available!\r\nCBZ needs to contain XML metadata (" + Win_CBZSettings.Default.MetaDataFilename + ") in order to set Bookmarks. Add a new set of Metadata now?", "Metadata required", ApplicationMessage.DialogType.MT_CONFIRMATION, ApplicationMessage.DialogButtons.MB_YES | ApplicationMessage.DialogButtons.MB_NO);
                if (res == DialogResult.Yes)
                {
                    BtnAddMetaData_Click(sender, null);
                }
                else
                {
                    return;
                }
            }

            if (Win_CBZSettings.Default.WriteXmlPageIndex == false)
            {
                DialogResult res = ApplicationMessage.ShowConfirmation("Currently writing XML- pageindex is disabled!\r\nCBZ needs to contain XML pageindex in order to set Bookmarks. Please enable it in Application settings under 'CBZ -> Compatibility' first.", "XML pageindex required", ApplicationMessage.DialogType.MT_INFORMATION, ApplicationMessage.DialogButtons.MB_OK);

                return;
            }

            ManageBookmarksForm manageBookmarksForm = new ManageBookmarksForm(Program.ProjectModel.MetaData, Program.ProjectModel.Pages);
            if (manageBookmarksForm.ShowDialog() == DialogResult.OK)
            {
                //Program.ProjectModel.MetaData.PageIndex.ea = manageBookmarksForm.Bookmarks;

                //AppEventHandler.OnMetaDataChanged(this, new MetaDataChangedEvent(MetaDataChangedEvent.METADATA_UPDATED, Program.ProjectModel.MetaData));
            }
        }

        private void ToolButtonSetBookmark_Click(object sender, EventArgs e)
        {
            BookmarksToolStripMenuItem_Click(sender, e);
        }

        private void SelectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PageRangeSelectionForm pageRangeSelectionForm = new PageRangeSelectionForm(UseOffset, LastOffset);
            if (pageRangeSelectionForm.ShowDialog() == DialogResult.OK)
            {
                UseOffset = pageRangeSelectionForm.UseOffset;
                LastOffset = pageRangeSelectionForm.Offset;

                PagesList.SelectedItems.Clear();
                bool visibilityEnsured = false;

                pageRangeSelectionForm.Selections.OrderBy(item => item.Start).Each(selection =>
                {
                    if (selection.Start >= 1 && selection.End >= 0)
                    {
                        List<Page> selectedPages = Program.ProjectModel.Pages.Where(p => p.Number >= selection.Start && p.Number <= selection.End).ToList();
                        foreach (Page page in selectedPages)
                        {
                            ListViewItem item = PagesList.Items.Cast<ListViewItem>().FirstOrDefault(i => i.Tag == page);
                            if (item != null)
                            {
                                item.Selected = true;
                                item.Focused = true;
                                if (!visibilityEnsured)
                                {
                                    item.EnsureVisible();
                                    visibilityEnsured = true;
                                }
                            }
                        }
                    }
                });
            }
        }

        private void ListView_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            // Not interested in changing the way columns are drawn - this works fine
            e.DrawDefault = true;
        }

        private void ListView_MouseMove(object sender, MouseEventArgs e)
        {
            if (sender as ListView == null)
            {
                return;
            }

            ListViewItem item = ((ListView)sender).GetItemAt(e.X, e.Y);
            if (item != null)
            {
                ((ListView)sender).Invalidate(item.Bounds);

                Invalidatable tag = item.Tag as Invalidatable;
                //item.Tag = "tagged";

                if (tag != null && !tag.Invalidated)
                {
                    tag.Invalidate();
                }
            }
        }

        private void ListView_Invalidated(object sender, InvalidateEventArgs e)
        {
            if (sender as ListView == null)
            {
                return;
            }

            ListView lv = sender as ListView;

            foreach (ListViewItem item in lv.Items)
            {
                if (item == null) return;
                Invalidatable tag = item.Tag as Invalidatable;
                tag?.Invalidate(false);
            }
        }

        private void ListView_ColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
        {
            if (sender as ListView == null)
            {
                return;
            }

            ListView lv = sender as ListView;

            lv.Invalidate();
        }

        private void ToolButtonAddImageTask_Click(object sender, EventArgs e)
        {
            ImageTaskAssignment imageTaskAssignment = new ImageTaskAssignment(new List<Page>(), new ImageTask(""));
            ListViewItem newTaskItem = ImageTaskListView.Items.Add("New Task");

            newTaskItem.SubItems.Add("--");
            newTaskItem.Name = imageTaskAssignment.Key;
            newTaskItem.Tag = imageTaskAssignment;
        }

        private void ImageTaskListView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (ImageTaskListView.SelectedItems.Count != 1 || e.Item == null || e.Item.Tag == null || e.Item.Tag as ImageTaskAssignment == null)
            {
                UpdateImageAdjustments(sender, new ImageTaskAssignment(new List<Page>(), new ImageTask("") { TaskOrder = new ImageTaskOrder(), ImageAdjustments = new ImageAdjustments() }));
            }
            else
            {
                UpdateImageAdjustments(sender, e.Item.Tag as ImageTaskAssignment, true);
            }

            ToolButtonRemoveImageTask.Enabled = ImageTaskListView.SelectedItems.Count > 0;
            ToolButtonAssignPagesToImageTask.Enabled = ImageTaskListView.SelectedItems.Count > 0;
            AssignAllPagesToolStripMenuItem.Enabled = ImageTaskListView.SelectedItems.Count == 1;
            AssignSelectedPagesToolStripMenuItem.Enabled = ImageTaskListView.SelectedItems.Count == 1;
            ToolButtonSelectAssignedPages.Enabled = ImageTaskListView.SelectedItems.Count == 1;

            SetControlsEnabledState("adjustments", ImageTaskListView.SelectedItems.Count == 1);
        }

        private void ToolButtonRemoveImageTask_Click(object sender, EventArgs e)
        {

            List<ListViewItem> selectedItems = ImageTaskListView.SelectedItems.Cast<ListViewItem>().ToList();

            Task.Factory.StartNew(() =>
            {

                Invoke(new Action(() =>
                {
                    this.Cursor = Cursors.WaitCursor;

                    ToolButtonAssignPagesToImageTask.Enabled = false;
                    ToolButtonSelectAssignedPages.Enabled = false;
                    selectedItems.Each(item =>
                    {

                        ImageTaskAssignment assignment = item.Tag as ImageTaskAssignment;

                        assignment.UnassignTaskFromPages();
                        ImageTaskListView.SelectedItem.Text = assignment.GetAssignedTaskName();
                        ImageTaskListView.SelectedItem.SubItems[1].Text = assignment.GetAssignedPageNumbers();

                        foreach (Page page in assignment.Pages.ToList())
                        {
                            AppEventHandler.OnPageChanged(this, new PageChangedEvent(page, null, PageChangedEvent.IMAGE_STATUS_CHANGED, true));
                        }

                        if (assignment.Pages.Count > 0)
                        {
                            AppEventHandler.OnArchiveStatusChanged(this, new ArchiveStatusEvent(Program.ProjectModel, ArchiveStatusEvent.ARCHIVE_FILE_UPDATED));
                        }

                        assignment.Pages.Clear();

                        ImageTaskListView.SelectedItem.Remove();

                        ToolButtonRemoveImageTask.Enabled = false;
                        ToolButtonAssignPagesToImageTask.Enabled = false;

                        UpdateImageAdjustments(sender, new ImageTaskAssignment(new List<Page>(), new ImageTask("") { TaskOrder = new ImageTaskOrder(), ImageAdjustments = new ImageAdjustments() }));

                        SetControlsEnabledState("adjustments", false);


                    });
                }));
            }).ContinueWith(r =>
            {
                Invoke(new Action(() =>
                {
                    this.Cursor = Cursors.Default;

                    ToolButtonSelectAssignedPages.Enabled = true;
                }));
            });
        }

        private void ToolButtonRemoveAllTasks_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in ImageTaskListView.Items)
            {
                if (item == null) continue;
                ImageTaskAssignment assignment = item.Tag as ImageTaskAssignment;
                assignment.UnassignTaskFromPages();
                item.Text = assignment.GetAssignedTaskName();
                item.SubItems[1].Text = assignment.GetAssignedPageNumbers();
                foreach (Page page in assignment.Pages.ToList())
                {
                    AppEventHandler.OnPageChanged(this, new PageChangedEvent(page, null, PageChangedEvent.IMAGE_STATUS_CHANGED, true));
                }

                if (assignment.Pages.Count > 0)
                {
                    AppEventHandler.OnArchiveStatusChanged(this, new ArchiveStatusEvent(Program.ProjectModel, ArchiveStatusEvent.ARCHIVE_FILE_UPDATED));
                }

                assignment.Pages.Clear();
            }

            ImageTaskListView.Items.Clear();
            ToolButtonRemoveImageTask.Enabled = false;
            ToolButtonAssignPagesToImageTask.Enabled = false;
            UpdateImageAdjustments(sender, new ImageTaskAssignment(new List<Page>(), new ImageTask("") { TaskOrder = new ImageTaskOrder(), ImageAdjustments = new ImageAdjustments() }));

            SetControlsEnabledState("adjustments", false);

        }

        private void AssignSelectedPagesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool addAll = false;
            if (sender as ToolStripMenuItem == null)
            {
                return;
            }

            ToolStripMenuItem menuItem = sender as ToolStripMenuItem;

            if (menuItem.Name == "AssignAllPagesToolStripMenuItem")
            {
                addAll = true;
            }

            if (!addAll && PagesList.SelectedItems.Count == 0 || ImageTaskListView.SelectedItems.Count == 0)
            {
                return;
            }

            if (addAll && PagesList.Items.Count == 0 || ImageTaskListView.SelectedItems.Count == 0)
            {
                return;
            }

            Task.Factory.StartNew(() =>
            {

                Invoke(new Action(() =>
                {
                    this.Cursor = Cursors.WaitCursor;

                    ToolButtonAssignPagesToImageTask.Enabled = false;
                    ToolButtonSelectAssignedPages.Enabled = false;

                    ImageTaskAssignment assignment = ImageTaskListView.SelectedItem.Tag as ImageTaskAssignment;

                    assignment.Pages.Clear();

                    List<Page> pagesToAdd = new List<Page>();

                    if (addAll)
                    {
                        pagesToAdd.AddRange(PagesList.Items.Cast<ListViewItem>().Select(i => i.Tag as Page).ToList());
                    }
                    else
                    {
                        pagesToAdd.AddRange(PagesList.SelectedItems.Cast<ListViewItem>().Select(i => i.Tag as Page).ToList());
                    }

                    // Remove pages from other tasks first
                    foreach (ListViewItem existingTask in ImageTaskListView.Items)
                    {
                        if (existingTask != ImageTaskListView.SelectedItem && existingTask.Tag != null && existingTask.Tag as ImageTaskAssignment != null)
                        {
                            ImageTaskAssignment existingAssignment = existingTask.Tag as ImageTaskAssignment;

                            foreach (Page page in pagesToAdd)
                            {
                                Page existingPage = existingAssignment.Pages.Find(p => p.Id == page.Id);

                                if (existingPage != null)
                                {
                                    existingAssignment.Pages.Remove(existingPage);
                                    existingAssignment.UnassignTask(existingPage);
                                    existingTask.Text = existingAssignment.GetAssignedTaskName();
                                    existingTask.SubItems[1].Text = existingAssignment.GetAssignedPageNumbers();
                                }
                            }
                        }
                    }

                    // Now add pages to the selected task
                    foreach (Page page in pagesToAdd)
                    {

                        if (ImageTaskListView.SelectedItem != null && ImageTaskListView.SelectedItem.Tag != null && ImageTaskListView.SelectedItem.Tag as ImageTaskAssignment != null)
                        {
                            Page existingPage = assignment.Pages.Find(p => p.Id == page.Id);

                            if (existingPage == null)
                            {
                                assignment.Pages.Add(page);
                                assignment.AssignTaskToPages();
                                ImageTaskListView.SelectedItem.Text = assignment.GetAssignedTaskName();
                                ImageTaskListView.SelectedItem.SubItems[1].Text = assignment.GetAssignedPageNumbers();


                                AppEventHandler.OnPageChanged(this, new PageChangedEvent(page, null, PageChangedEvent.IMAGE_STATUS_CHANGED, true));
                            }
                        }
                    }

                    assignment.AssignTaskToPages();

                    if (assignment.Pages.Count > 0)
                    {
                        AppEventHandler.OnArchiveStatusChanged(this, new ArchiveStatusEvent(Program.ProjectModel, ArchiveStatusEvent.ARCHIVE_FILE_UPDATED));
                    }


                }));
            }).ContinueWith((r) =>
            {
                this.Invoke(new Action(() =>
                {
                    this.Cursor = Cursors.Default;

                    ToolButtonAssignPagesToImageTask.Enabled = true;
                    ToolButtonSelectAssignedPages.Enabled = true;

                }));
            });

        }

        private void AssignAllPagesToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void UnAssignAllPagesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ImageTaskListView.SelectedItems.Count == 0 || ImageTaskListView.SelectedItems.Count == 0)
            {
                return;
            }

            List<ListViewItem> selectedItems = ImageTaskListView.SelectedItems.Cast<ListViewItem>().ToList();

            foreach (ListViewItem existingTask in selectedItems)
            {

                ImageTaskAssignment existingAssignment = existingTask.Tag as ImageTaskAssignment;

                existingAssignment.Pages.Clear();
                existingAssignment.UnassignTaskFromPages();
                existingTask.Text = existingAssignment.GetAssignedTaskName();
                existingTask.SubItems[1].Text = existingAssignment.GetAssignedPageNumbers();

            }
        }

        private void ToolButtonSelectAssignedPages_Click(object sender, EventArgs e)
        {
            ListViewItem existingTask = ImageTaskListView.SelectedItem;

            if (existingTask != null && existingTask.Tag != null && existingTask.Tag as ImageTaskAssignment != null)
            {
                ImageTaskAssignment existingAssignment = existingTask.Tag as ImageTaskAssignment;

                if (existingAssignment.Pages.Count == 0)
                {
                    return;
                }

                PagesList.SelectedItems.Clear();
                bool visibilityEnsured = false;

                existingAssignment.Pages.OrderBy(page => page.Index).Each(page =>
                {

                    ListViewItem item = PagesList.Items.Cast<ListViewItem>().FirstOrDefault(i => i.Tag == page);
                    if (item != null)
                    {
                        item.Selected = true;
                        item.Focused = true;
                        if (!visibilityEnsured)
                        {
                            item.EnsureVisible();
                            visibilityEnsured = true;
                        }
                    }
                });
            }
        }

        private void ToolButtonCreateTasksForEach_Click(object sender, EventArgs e)
        {

            Task.Factory.StartNew<TaskResult>(() =>
            {
                TaskResult result = new TaskResult();
                int i = 0;

                Invoke(new Action(() =>
                {

                    this.Cursor = Cursors.WaitCursor;

                    ToolButtonAssignPagesToImageTask.Enabled = false;
                    ToolButtonSelectAssignedPages.Enabled = false;

                    List<ImageTaskAssignment> existingAssignments = new List<ImageTaskAssignment>();
                    ImageTaskListView.Items.Cast<ListViewItem>().Each(item =>
                    {
                        ImageTaskAssignment assignment = item.Tag as ImageTaskAssignment;
                        existingAssignments.Add(assignment);
                    });


                    bool exists;
                    foreach (ListViewItem item in PagesList.Items)
                    {

                        Page page = item.Tag as Page;
                        exists = false;

                        foreach (ImageTaskAssignment existingTask in existingAssignments)
                        {
                            Page existingPage = existingTask.Pages.Find(p => p.Id == page.Id);

                            if (existingPage != null)
                            {
                                exists = true;

                                break;
                            }
                        }

                        if (!exists)
                        {
                            ImageTaskAssignment imageTaskAssignment = new ImageTaskAssignment(new List<Page>(), new ImageTask(""));
                            ListViewItem newTaskItem = ImageTaskListView.Items.Add("New Task " + i);

                            imageTaskAssignment.Pages.Add(page);

                            newTaskItem.SubItems.Add(page.Number.ToString());
                            newTaskItem.Name = imageTaskAssignment.Key;
                            newTaskItem.Tag = imageTaskAssignment;

                            i++;
                        }
                    }
                }));

                result.Completed = i;

                return result;
            }).ContinueWith(r =>
            {
                this.Invoke(new Action(() =>
                {
                    this.Cursor = Cursors.Default;
                    ToolButtonAssignPagesToImageTask.Enabled = true;
                    ToolButtonSelectAssignedPages.Enabled = true;

                    if (r.Result.Completed == 0)
                    {
                        ApplicationMessage.Show("All pages already assigned to a task. No new tasks have been created!", "No Tasks Created", ApplicationMessage.DialogType.MT_INFORMATION, ApplicationMessage.DialogButtons.MB_OK);
                    }
                }));
            });

        }

        private void UpdateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UpdateCheckHelper.CheckForUpdates(this, false);
        }

        private void UpdateCheckTimer_Tick(object sender, EventArgs e)
        {

            DateTime lastCheck = new DateTime(Win_CBZSettings.Default.AutoUpdateLastCheck);

            if (lastCheck.AddSeconds(Win_CBZSettings.Default.AutoUpdateInterval) < DateTime.Now)
            {

                Win_CBZSettings.Default.AutoUpdateLastCheck = DateTime.Now.Ticks;
                Win_CBZSettings.Default.Save();

                UpdateCheckHelper.CheckForUpdates(this, true);

            }

            UpdateCheckTimer.Interval = 60 * 60 * 1000; // 1 hour checks after initial check
        }

        private void SelectAssignedTaskToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (PagesList.SelectedItems.Count == 1)
            {

                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        Invoke(() =>
                        {
                            Page selectedPage = PagesList.SelectedItems[0].Tag as Page;

                            ImageTaskListView.SelectedItems.Clear();
                            ImageTaskListView.Items.Cast<ListViewItem>().Each(item =>
                            {

                                ImageTaskAssignment ita = item.Tag as ImageTaskAssignment;

                                if (ita != null && ita.Pages.Contains(selectedPage))
                                {
                                    item.Selected = true;
                                }
                            });
                        });

                    }
                    catch (Exception) { }
                });
            }
        }

        private void SaveSelectedPageAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            if (PagesList.SelectedItems.Count == 1)
            {
                Page page = PagesList.SelectedItems[0].Tag as Page;

                if (page != null)
                {
                    try
                    {
                        SavePagesDialog.FileName = page.Name;
                        if (SavePagesDialog.ShowDialog() == DialogResult.OK)
                        {
                                

                            page.Save(new LocalFile(SavePagesDialog.FileName), AppEventHandler.OnFileOperation);

                        }
                    } catch (ApplicationException ae)
                    {
                        AppEventHandler.OnMessageLogged(this, new LogMessageEvent(LogMessageEvent.LOGMESSAGE_TYPE_ERROR, "Error saving page to disk [" + ae.Message + "]"));

                        if (ae.ShowErrorDialog)
                        {
                            ApplicationMessage.ShowError("Error saving Page to disk.", "Error saving Page", ApplicationMessage.DialogType.MT_ERROR, ApplicationMessage.DialogButtons.MB_OK);
                        }
                    }
                }
            }
        }
    }
}
