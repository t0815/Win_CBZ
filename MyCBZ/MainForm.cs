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

        private CBZProjectModel model;

        private Thread ClosingTask;

        private Thread OpeningTask;

        private bool WindowClosed = false;

        public MainForm()
        {
            InitializeComponent();
            model = new CBZProjectModel(Program.Path);
            model.ImageProgress += FileLoaded;
            model.ArchiveStatusChanged += ArchiveStateChanged;
            model.ItemChanged += ItemChanged;
            model.MetaDataLoaded += MetaDataLoaded;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {

            DialogResult openCBFResult = OpenCBFDialog.ShowDialog();

            if (openCBFResult == DialogResult.OK)
            {
                ClosingTask = model.Close();

                Task.Factory.StartNew(() =>
                {
                    if (ClosingTask != null)
                    {
                        while (ClosingTask.IsAlive)
                        {
                            System.Threading.Thread.Sleep(100);
                        }
                    }

                    OpeningTask = model.Open(OpenCBFDialog.FileName, ZipArchiveMode.Read);
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
                item.SubItems.Add(e.Image.LastModified.ToString());
                item.SubItems.Add(e.Image.Size.ToString());
                if (!e.Image.Compressed) { 
                    item.BackColor = Color.Orange;
                }

                if (e.Image.Deleted)
                {
                    item.ForeColor = Color.Silver;
                }

                PageImages.Images.Add(e.Image.GetThumbnail(ThumbAbort, Handle));
            }));

            PageView.Invoke(new Action(() => {
                ListViewItem page = PageView.Items.Add(e.Image.Filename, e.Index);
                page.SubItems.Add(e.Image.Index.ToString());
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


        private void ArchiveStateChanged(object sender, CBZArchiveStatusEvent e)
        {
            String info = "Ready.";
            String filename = "<NO FILE>";

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

           
            if (e.State == CBZArchiveStatusEvent.ARCHIVE_OPENED)
            {
                filename = e.ArchiveInfo.FileName;
            }

          
            if (e.State == CBZArchiveStatusEvent.ARCHIVE_OPENING)
            {
                filename = e.ArchiveInfo.FileName;
                info = "Reading archive...";
            }


            if (e.State == CBZArchiveStatusEvent.ARCHIVE_CLOSING)
            {
                info = "Closing file...";
            }

            if (e.State == CBZArchiveStatusEvent.ARCHIVE_CLOSED)
            {
                //
            }


            try
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(() =>
                    {
                        fileNameLabel.Text = filename;
                        applicationStatusLabel.Text = info;
                    }));
                }
            } catch (Exception)
            {

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

                ClosingTask = model.Close();
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

        private void PageView_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListView.SelectedListViewItemCollection selectedPages = this.PageView.SelectedItems;

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

            //ClearProject();
        }

        private void addFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {

            DialogResult openImageResult = openImagesDialog.ShowDialog();
            List<CBZLocalFile> files = new List<CBZLocalFile>();

            if (openImageResult == DialogResult.OK)
            {
                var maxIndex = PagesList.Items.Count - 1;
                var newIndex = maxIndex < 0 ? 0 : maxIndex;
                files = model.parseFiles(new List<String>(openImagesDialog.FileNames));
                if (files.Count > 0)
                {
                    model.AddImages(files, newIndex);
                }

            }
        }

        private void PagesList_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            //PagesList.
            //e.Label;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (model.MetaData == null)
            {
                model.MetaData = new CBZMetaData(true);
            }

            model.MetaData.FillMissingDefaultProps();

            MetaDataLoaded(sender, new MetaDataLoadEvent(model.MetaData.Values));

            btnAddMetaData.Enabled = false;
            btnRemoveMetaData.Enabled = true;
            AddMetaDataRowBtn.Enabled = true;
            RemoveMetadataRowBtn.Enabled = false;
        }

        private void btnRemoveMetaData_Click(object sender, EventArgs e)
        {
            if (model.MetaData != null)
            {
                metaDataGrid.DataSource = null;

                model.MetaData.Values.Clear();
                btnAddMetaData.Enabled = true;
                btnRemoveMetaData.Enabled = false;
                AddMetaDataRowBtn.Enabled = false;
                RemoveMetadataRowBtn.Enabled = false;
            }
        }

        private void AddMetaDataRowBtn_Click(object sender, EventArgs e)
        {
            model.MetaData.Values.Add(new CBZMetaDataEntry(""));
            metaDataGrid.Refresh();
        }

        private void metaDataGrid_SelectionChanged(object sender, EventArgs e)
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
                        model.MetaData.Values.Remove((CBZMetaDataEntry)row.DataBoundItem);
                    }
                }
            }
        }

        private void metaDataGrid_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 0)
                {
                    model.MetaData.Validate((CBZMetaDataEntry)metaDataGrid.Rows[e.RowIndex].DataBoundItem, e.FormattedValue.ToString());
                }
            } catch (MetaDataValidationException ve)
            {
                metaDataGrid.Rows[e.RowIndex].ErrorText = ve.Message;
                //e.Cancel = true;
            }
        }
    }
}
