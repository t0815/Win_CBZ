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

namespace CBZMage
{
    public partial class MainForm : Form
    {

        private CBZProjectModel ProjectModel;

        private Thread ClosingTask;

        private Thread OpeningTask;

        private bool WindowClosed = false;

        public MainForm()
        {
            InitializeComponent();
            ProjectModel = new CBZProjectModel(CBZMageSettings.Default.TempFolderPath);
            ProjectModel.ImageProgress += FileLoaded;
            ProjectModel.ArchiveStatusChanged += ArchiveStateChanged;
            ProjectModel.ItemChanged += ItemChanged;
            ProjectModel.MetaDataLoaded += MetaDataLoaded;
            ProjectModel.ItemExtracted += ItemExtracted;
            MessageLogger.Instance.SetHandler(MessageLogged);
            NewProject();            
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

        private void FileLoaded(object sender, ItemLoadProgressEvent e)
        {
            toolStripProgressBar.Control.Invoke(new Action(() =>
            {
                toolStripProgressBar.Maximum = e.Total;
                toolStripProgressBar.Value = e.Index;
            }));

            PagesList.Invoke(new Action(() =>
            {
                ListViewItem item = PagesList.Items.Add(e.Image.Name, -1);
                item.SubItems.Add(e.Image.Number.ToString());
                item.SubItems.Add(e.Image.ImageType.ToString());
                item.SubItems.Add(e.Image.LastModified.ToString());
                item.SubItems.Add(e.Image.Size.ToString());
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

                PageImages.Images.Add(e.Image.GetThumbnail(ThumbAbort, Handle));
            }));

            PageView.Invoke(new Action(() => {
                ListViewItem page = PageView.Items.Add(e.Image.Filename, e.Index);
                page.SubItems.Add(e.Image.Index.ToString());
                page.Tag = e.Image;
            }));
            
        }

        private void MetaDataLoaded(object sender, MetaDataLoadEvent e)
        {
            metaDataGrid.Invoke(new Action(() =>
            {
                metaDataGrid.DataSource = e.MetaData;

                btnAddMetaData.Enabled = false;
                btnRemoveMetaData.Enabled = true;
                DataGridViewColumn firstCol = metaDataGrid.Columns.GetFirstColumn(DataGridViewElementStates.Visible);
                if (firstCol != null)
                {
                    DataGridViewColumn secondCol = metaDataGrid.Columns.GetNextColumn(firstCol, DataGridViewElementStates.Visible, DataGridViewElementStates.None);
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


        private bool ThumbAbort()
        {

            return true;
        }

        private void ItemChanged(object sender, ItemChangedEvent e)
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
            }
          
            try
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(() =>
                    {
                        fileNameLabel.Text = filename;
                        applicationStatusLabel.Text = info;
                        ProjectModel.ArchiveState = e.State;

                        DisableControllsForArchiveState(e.State);
                    }));
                }
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
                                logItem.ImageIndex = 5;
                                break;
                            case LogMessageEvent.LOGMESSAGE_TYPE_WARNING:
                                logItem.ImageIndex = 4;
                                break;
                            case LogMessageEvent.LOGMESSAGE_TYPE_ERROR:
                                logItem.ImageIndex = 3;
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
                    toolStripButton3.Enabled = false;
                    break;

                case CBZArchiveStatusEvent.ARCHIVE_OPENED:
                    newToolStripMenuItem.Enabled = true;
                    openToolStripMenuItem.Enabled = true;
                    ToolButtonNew.Enabled = true;
                    ToolButtonOpen.Enabled = true;
                    addFilesToolStripMenuItem.Enabled = true;
                    toolStripButton3.Enabled = true;
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
                        System.Threading.Thread.Sleep(100);
                    }
                }

                ClosingTask = ProjectModel.Close();
            });
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!WindowClosed)
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
           
            toolStripButton4.Enabled = buttonState;
            toolStripButton5.Enabled = buttonState;
            toolStripButton6.Enabled = buttonState;

            toolStripButton9.Enabled = selectedPages.Count == 1;

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

            if (ProjectModel.ArchiveState == CBZArchiveStatusEvent.ARCHIVE_SAVING ||
                ProjectModel.ArchiveState == CBZArchiveStatusEvent.ARCHIVE_OPENING ||
                ProjectModel.ArchiveState == CBZArchiveStatusEvent.ARCHIVE_EXTRACTING ||
                ProjectModel.ArchiveState == CBZArchiveStatusEvent.ARCHIVE_CLOSING) {
                    e.Cancel = true;
            }
        }

        private void AddFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {

            DialogResult openImageResult = openImagesDialog.ShowDialog();
            List<CBZLocalFile> files = new List<CBZLocalFile>();

            if (openImageResult == DialogResult.OK)
            {
                var maxIndex = PagesList.Items.Count - 1;
                var newIndex = maxIndex < 0 ? 0 : maxIndex;
                files = ProjectModel.ParseFiles(new List<String>(openImagesDialog.FileNames));
                if (files.Count > 0)
                {
                    ProjectModel.AddImages(files, newIndex);
                }

            }
        }

        private void PagesList_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            //PagesList.
            //e.Label;
        }

        private void BtnAddMetaData_Click(object sender, EventArgs e)
        {
            if (ProjectModel.MetaData == null)
            {
                ProjectModel.MetaData = new CBZMetaData(true);
            }

            ProjectModel.MetaData.FillMissingDefaultProps();

            MetaDataLoaded(sender, new MetaDataLoadEvent(ProjectModel.MetaData.Values));

            btnAddMetaData.Enabled = false;
            btnRemoveMetaData.Enabled = true;
            AddMetaDataRowBtn.Enabled = true;
            RemoveMetadataRowBtn.Enabled = false;
        }

        private void BtnRemoveMetaData_Click(object sender, EventArgs e)
        {
            if (ProjectModel.MetaData != null)
            {
                metaDataGrid.DataSource = null;

                ProjectModel.MetaData.Values.Clear();
                btnAddMetaData.Enabled = true;
                btnRemoveMetaData.Enabled = false;
                AddMetaDataRowBtn.Enabled = false;
                RemoveMetadataRowBtn.Enabled = false;
            }
        }

        private void AddMetaDataRowBtn_Click(object sender, EventArgs e)
        {
            ProjectModel.MetaData.Values.Add(new CBZMetaDataEntry(""));
            metaDataGrid.Refresh();
        }

        private void MetaDataGrid_SelectionChanged(object sender, EventArgs e)
        {
            RemoveMetadataRowBtn.Enabled = metaDataGrid.SelectedRows.Count > 0;   
        }

        private void RemoveMetadataRowBtn_Click(object sender, EventArgs e)
        {
            if (metaDataGrid.SelectedRows.Count > 0)
            {
                foreach (DataGridViewRow row in metaDataGrid.SelectedRows)
                {
                    if (row.DataBoundItem is CBZMetaDataEntry)
                    {
                        ProjectModel.MetaData.Values.Remove((CBZMetaDataEntry)row.DataBoundItem);
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
                    ProjectModel.MetaData.Validate((CBZMetaDataEntry)metaDataGrid.Rows[e.RowIndex].DataBoundItem, e.FormattedValue.ToString());
                }
            } catch (MetaDataValidationException ve)
            {
                metaDataGrid.Rows[e.RowIndex].ErrorText = ve.Message;
                //e.Cancel = true;
            }
        }

        private void extractAllToolStripMenuItem_Click(object sender, EventArgs e)
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

            toolStripButton4.Enabled = buttonState;
            toolStripButton5.Enabled = buttonState;
            toolStripButton6.Enabled = buttonState;

            toolStripButton9.Enabled = selectedPages.Count == 1;
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            ListView.SelectedListViewItemCollection selectedPages = this.PagesList.SelectedItems;

            if (selectedPages.Count > 0)
            {
                foreach (ListViewItem img in selectedPages)
                {
                    ((CBZImage)img.Tag).Deleted = true;
                    if (!((CBZImage)img.Tag).Compressed)
                    {
                        try
                        {
                            ((CBZImage)img.Tag).DeleteTemporaryFile();
                        } catch (Exception ex)
                        {
                            MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_ERROR, ex.Message);
                        }
                    }
                    img.ForeColor = Color.Silver;
                    img.BackColor = Color.Transparent;
                }
            }
        }
    }
}
