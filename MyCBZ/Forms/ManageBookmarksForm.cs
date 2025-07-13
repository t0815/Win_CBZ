using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using Win_CBZ.Data;
using Win_CBZ.Events;
using Win_CBZ.Handler;
using Win_CBZ.Helper;
using Win_CBZ.Tasks;

namespace Win_CBZ.Forms
{

    [SupportedOSPlatform("windows")]
    internal partial class ManageBookmarksForm : Form
    {

        protected TreeNode _selectedNode = null;

        public ManageBookmarksForm(MetaData metaData, List<Page> pages)
        {
            InitializeComponent();



            pages.ForEach(page =>
            {
                ListViewItem item = new ListViewItem(page.Name)
                {
                    Tag = page
                };

                item.SubItems.Add(page.Number.ToString());
                item.SubItems.Add(page.Bookmark);
                PagesList.Items.Add(item);

                if (page.Bookmark != null && page.Bookmark.Trim().Length > 0)
                {
                    if (BookmarksTree.Nodes.IndexOfKey(page.Bookmark) == -1)
                    {
                        TreeNode node = BookmarksTree.Nodes.Add(page.Bookmark, page.Bookmark);
                        node.Nodes.Add(page.Name, page.Name);

                    }
                    else
                    {
                        TreeNode node = BookmarksTree.Nodes[BookmarksTree.Nodes.IndexOfKey(page.Bookmark)];
                        if (node.Level == 0)
                        {
                            node.Nodes.Add(page.Name, page.Name);
                        }

                    }
                }
            });
        }

        private void ToolButtonCreateBookmark_Click(object sender, EventArgs e)
        {
            if (TextBoxBookmarkName.Text.Trim().Length == 0)
            {

                ApplicationMessage.ShowError("Please enter a bookmark name.", "Error");
                return;
            }

            if (BookmarksTree.Nodes.IndexOfKey(TextBoxBookmarkName.Text.Trim()) != -1)
            {
                if (BookmarksTree.Nodes[BookmarksTree.Nodes.IndexOfKey(TextBoxBookmarkName.Text.Trim())].Level == 0)
                {

                    ApplicationMessage.ShowError("A bookmark with this name already exists.", "Error");
                    TextBoxBookmarkName.Focus();
                    TextBoxBookmarkName.SelectAll();
                    return;
                }
            }

            _selectedNode = BookmarksTree.Nodes.Add(TextBoxBookmarkName.Text.Trim(), TextBoxBookmarkName.Text.Trim());

            TextBoxBookmarkName.Text = string.Empty;

        }

        private void ToolButtonAddPages_Click(object sender, EventArgs e)
        {

            if (PagesList.SelectedItems.Count == 0)
            {
                ApplicationMessage.ShowError("Please select a page to add the bookmark to.", "Error");
                return;
            }

            if (BookmarksTree.SelectedNode == null)
            {
                ApplicationMessage.ShowError("No Bookmark selected.", "Error");
                return;
            }


            foreach (ListViewItem item in PagesList.SelectedItems)
            {
                Page page = (Page)item.Tag;

                String bookmark = BookmarksTree.SelectedNode.Text;
                if (BookmarksTree.SelectedNode.Level > 0)
                {
                    bookmark = BookmarksTree.SelectedNode.Parent.Text;
                }

                //page.Bookmark = TextBoxBookmarkName.Text.Trim();

                item.SubItems[2].Text = bookmark;

                if (BookmarksTree.Nodes.IndexOfKey(bookmark) > -1)
                {
                    TreeNode node = BookmarksTree.Nodes[BookmarksTree.Nodes.IndexOfKey(bookmark)];
                    if (node.Level == 0)
                    {
                        if (node.Nodes.IndexOfKey(page.Name) == -1)
                        {
                            node.Nodes.Add(page.Name, page.Name);
                        }
                    }
                }

                //AppEventHandler.OnPageChanged(this, new PageChangedEvent(page, null, PageChangedEvent.IMAGE_STATUS_CHANGED, true));
            }

        }

        private void BookmarksTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node != null)
            {
                _selectedNode = e.Node;

                if (e.Node.Level > 0)
                {
                    TextBoxBookmarkName.Text = e.Node.Parent.Text;
                }
                else
                {
                    TextBoxBookmarkName.Text = e.Node.Text;
                }


            }
            else
            {
                _selectedNode = null;
                TextBoxBookmarkName.Text = string.Empty;
            }
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            OkButton.Enabled = false;
            int index = 0;

            foreach (ListViewItem item in PagesList.Items)
            {
                Page page = (Page)item.Tag;

                if (page.Bookmark != item.SubItems[2].Text)
                {
                    page.Bookmark = item.SubItems[2].Text;

                    //Program.ProjectModel.MetaData.UpdatePageIndexMetaDataEntry((Page)item.Tag, ((Page)item.Tag).Key);

                    AppEventHandler.OnPageChanged(this, new PageChangedEvent(page, null, PageChangedEvent.IMAGE_STATUS_CHANGED, true));
                }

                AppEventHandler.OnGeneralTaskProgress(this, new GeneralTaskProgressEvent(GeneralTaskProgressEvent.TASK_UPDATE_PAGE_INDEX, GeneralTaskProgressEvent.TASK_STATUS_RUNNING, "Updating page metadata for bookmark changes...", index, PagesList.Items.Count, false));

                index++;

                Thread.Sleep(2);
            }

            if (index > 0)
            {
                TokenStore.GetInstance().ResetCancellationToken(TokenStore.TOKEN_SOURCE_UPDATE_PAGE);

                Task<TaskResult> t = UpdateMetadataTask.UpdatePageMetadata(Program.ProjectModel.Pages,
                     Program.ProjectModel.MetaData,
                            MetaDataVersionFlavorHandler.GetInstance().HandlePageIndexVersion(),
                            AppEventHandler.OnGeneralTaskProgress,
                            AppEventHandler.OnPageChanged,
                            TokenStore.GetInstance().CancellationTokenSourceForName(TokenStore.TOKEN_SOURCE_UPDATE_PAGE).Token,
                            false,
                            true
                );

                t.ContinueWith((task) =>
                {
                    if (task.IsCompletedSuccessfully)
                    {
                        //
                    }
                    else
                    {
                        AppEventHandler.OnMessageLogged(this, new LogMessageEvent(LogMessageEvent.LOGMESSAGE_TYPE_ERROR, "Failed to update page index metadata: " + task.Exception?.Message));
                    }

                    AppEventHandler.OnArchiveStatusChanged(sender, new ArchiveStatusEvent(Program.ProjectModel, ArchiveStatusEvent.ARCHIVE_READY));
                    AppEventHandler.OnApplicationStateChanged(sender, new ApplicationStatusEvent(Program.ProjectModel, ApplicationStatusEvent.STATE_READY));
                });

                AppEventHandler.OnGlobalActionRequired(this,
                    new GlobalActionRequiredEvent(Program.ProjectModel,
                        0,
                        "Index-Metadata changed. Rebuild pageindex now?",
                        "Rebuild",
                        GlobalActionRequiredEvent.TASK_TYPE_INDEX_REBUILD,
                        t
                    ));
            }

            AppEventHandler.OnGeneralTaskProgress(this, new GeneralTaskProgressEvent(GeneralTaskProgressEvent.TASK_UPDATE_PAGE_INDEX, GeneralTaskProgressEvent.TASK_STATUS_COMPLETED, "Ready.", 0, 0, false));
            AppEventHandler.OnApplicationStateChanged(this, new ApplicationStatusEvent(Program.ProjectModel, ApplicationStatusEvent.STATE_READY));

        }

        private void ToolStripButton5_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in PagesList.Items)
            {
                Page page = (Page)item.Tag;

                String bookmark = BookmarksTree.SelectedNode.Text;
                if (BookmarksTree.SelectedNode.Level > 0)
                {
                    bookmark = BookmarksTree.SelectedNode.Parent.Text;
                }

                //page.Bookmark = TextBoxBookmarkName.Text.Trim();

                if (item.SubItems[2].Text == bookmark)
                {
                    item.SubItems[2].Text = string.Empty;
                }

            }

            if (BookmarksTree.SelectedNode != null)
            {
                TreeNode node = BookmarksTree.SelectedNode;
                if (node.Level > 0)
                {
                    node = node.Parent;
                }

                //if (node.Nodes.Count > 0)
                //{
                BookmarksTree.Nodes.Remove(node);
                // }

            }
        }

        private void ToolStripButton4_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in PagesList.SelectedItems)
            {
                Page page = (Page)item.Tag;

                String bookmark = item.SubItems[2].Text;
                String pageName = item.Text;

                TreeNode node = BookmarksTree.Nodes[BookmarksTree.Nodes.IndexOfKey(pageName)];

                if (node != null)
                {
                    BookmarksTree.Nodes.Remove(node);
                }

                item.SubItems[2].Text = "";
            }
        }

        private void ToolStripButton1_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in PagesList.Items)
            {
                Page page = (Page)item.Tag;

                String bookmark = item.SubItems[2].Text;
                String pageName = item.Text;

                TreeNode node = BookmarksTree.Nodes[BookmarksTree.Nodes.IndexOfKey(pageName)];

                if (node != null)
                {
                    BookmarksTree.Nodes.Remove(node);
                }

                item.SubItems[2].Text = "";
            }
        }
    }
}
