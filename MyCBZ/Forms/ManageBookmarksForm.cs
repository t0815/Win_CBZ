using SharpCompress.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using Win_CBZ.Data;
using Win_CBZ.Events;
using Win_CBZ.Extensions;
using Win_CBZ.Handler;
using Win_CBZ.Helper;
using Win_CBZ.Tasks;

namespace Win_CBZ.Forms
{

    [SupportedOSPlatform("windows")]
    internal partial class ManageBookmarksForm : Form
    {

        protected TreeNode _selectedNode = null;

        protected Page previewPage = null;

        protected Page previewPageFull = null;

        protected ImagePreviewForm previewForm = null;

        protected bool useOffset = false;

        protected int lastOffset = 0;

        public ManageBookmarksForm(MetaData metaData, List<Page> pages)
        {
            InitializeComponent();



            pages.ForEach(page =>
            {
                ListViewItem item = new ListViewItem(page.Name)
                {
                    Tag = page,
                    Name = page.Name
                };

                item.SubItems.Add(page.Number.ToString());
                item.SubItems.Add(page.Bookmark);
                PagesList.Items.Add(item);

                if (page.Bookmark != null && page.Bookmark.Trim().Length > 0)
                {
                    if (BookmarksTree.Nodes.IndexOfKey(page.Bookmark) == -1)
                    {
                        TreeNode node = BookmarksTree.Nodes.Add(page.Bookmark, page.Bookmark);
                        TreeNode subNode = node.Nodes.Add(page.Name, page.Name);
                        subNode.Tag = page;

                    }
                    else
                    {
                        TreeNode node = BookmarksTree.Nodes[BookmarksTree.Nodes.IndexOfKey(page.Bookmark)];
                        if (node.Level == 0)
                        {
                            TreeNode subNode = node.Nodes.Add(page.Name, page.Name);
                            subNode.Tag = page;
                        }

                    }
                }
            });

            Program.ProjectModel.TemporaryBookmarks.ForEach(bookmark =>
            {
                if (BookmarksTree.Nodes.IndexOfKey(bookmark) == -1)
                {
                    BookmarksTree.Nodes.Add(bookmark, bookmark);
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

            string oldBookmark = string.Empty;

            foreach (ListViewItem item in PagesList.SelectedItems)
            {
                Page page = (Page)item.Tag;

                String bookmark = BookmarksTree.SelectedNode.Text;
                if (BookmarksTree.SelectedNode.Level > 0)
                {
                    bookmark = BookmarksTree.SelectedNode.Parent.Text;
                }

                //page.Bookmark = TextBoxBookmarkName.Text.Trim();

                oldBookmark = item.SubItems[2].Text;

                if (oldBookmark != null && oldBookmark.Trim().Length > 0)
                {
                    if (BookmarksTree.Nodes.IndexOfKey(oldBookmark) > -1)
                    {
                        TreeNode node = BookmarksTree.Nodes[BookmarksTree.Nodes.IndexOfKey(oldBookmark)];
                        if (node.Level == 0)
                        {
                            if (node.Nodes.IndexOfKey(page.Name) > -1)
                            {
                                node.Nodes.RemoveByKey(page.Name);
                            }
                        }
                    }
                }

                item.SubItems[2].Text = bookmark;

                if (BookmarksTree.Nodes.IndexOfKey(bookmark) > -1)
                {
                    TreeNode node = BookmarksTree.Nodes[BookmarksTree.Nodes.IndexOfKey(bookmark)];
                    if (node.Level == 0)
                    {
                        if (node.Nodes.IndexOfKey(page.Name) == -1)
                        {
                            TreeNode subNode = node.Nodes.Add(page.Name, page.Name);
                            subNode.Tag = page;
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
                    PagesList.SelectedItems.Clear();
                    if (PagesList.Items.Count > 0)
                    {
                        ListViewItem[] findItems = PagesList.Items.Find(e.Node.Text, false);
                        if (findItems.Length == 1)
                        {
                            findItems[0].Selected = true;
                            findItems[0].Focused = true;
                            PagesList.EnsureVisible(findItems[0].Index);
                        }
                    }
                }
                else
                {
                    TextBoxBookmarkName.Text = e.Node.Text;
                }

                toolStripButton5.Enabled = true;
                toolStripButton2.Enabled = true;
            }
            else
            {
                _selectedNode = null;
                TextBoxBookmarkName.Text = string.Empty;
                toolStripButton5.Enabled = false;
                toolStripButton2.Enabled = false;
            }
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            OkButton.Enabled = false;
            int index = 0;

            Program.ProjectModel.TemporaryBookmarks.Clear();
            foreach (TreeNode n in BookmarksTree.Nodes)
            {
                if (n.Level == 0 && n.Text.Trim().Length > 0)
                {
                    Program.ProjectModel.TemporaryBookmarks.Add(n.Text.Trim());
                }
            }

            foreach (ListViewItem item in PagesList.Items)
            {
                Page page = (Page)item.Tag;

                if (page.Bookmark != item.SubItems[2].Text)
                {
                    page.Bookmark = item.SubItems[2].Text;

                    if (Program.ProjectModel.TemporaryBookmarks.IndexOf(item.SubItems[2].Text) > -1)
                    {
                        Program.ProjectModel.TemporaryBookmarks.Remove(item.SubItems[2].Text);
                    }

                    //Program.ProjectModel.MetaData.UpdatePageIndexMetaDataEntry((Page)item.Tag, ((Page)item.Tag).Key);

                    AppEventHandler.OnPageChanged(this, new PageChangedEvent(page, null, PageChangedEvent.IMAGE_STATUS_CHANGED, true));
                    AppEventHandler.OnArchiveStatusChanged(sender, new ArchiveStatusEvent(Program.ProjectModel, ArchiveStatusEvent.ARCHIVE_METADATA_CHANGED));

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
                        //AppEventHandler.OnArchiveStatusChanged(sender, new ArchiveStatusEvent(Program.ProjectModel, ArchiveStatusEvent.ARCHIVE_METADATA_CHANGED));
                    }
                    else
                    {
                        AppEventHandler.OnMessageLogged(this, new LogMessageEvent(LogMessageEvent.LOGMESSAGE_TYPE_ERROR, "Failed to update page index metadata: " + task.Exception?.Message));
                    }


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

                if (BookmarksTree.SelectedNode != null)
                {
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
                if (BookmarksTree.Nodes.Count == 0)
                {
                    toolStripButton5.Enabled = false;
                }
            }
        }

        private void ToolStripButton4_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in PagesList.SelectedItems)
            {
                Page page = (Page)item.Tag;

                String bookmark = item.SubItems[2].Text;
                String pageName = item.Text;

                foreach (TreeNode lv0Node in BookmarksTree.Nodes)
                {
                    if (lv0Node.Nodes.IndexOfKey(pageName) > -1)
                    {
                        TreeNode node = lv0Node.Nodes[lv0Node.Nodes.IndexOfKey(pageName)];

                        if (node != null)
                        {
                            lv0Node.Nodes.Remove(node);
                        }
                    }
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

                int index = BookmarksTree.Nodes.IndexOfKey(bookmark);

                if (index > -1)
                {
                    TreeNode node = BookmarksTree.Nodes[index];

                    if (node != null)
                    {
                        int subIndex = node.Nodes.IndexOfKey((String)pageName);

                        if (subIndex > -1)
                        {
                            TreeNode subNode = node.Nodes[subIndex];

                            node.Nodes.Remove(subNode);
                        }
                    }

                    item.SubItems[2].Text = "";
                }
            }
        }

        private void PagesList_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void ToolStripButton2_Click(object sender, EventArgs e)
        {

            TreeNode selected = BookmarksTree.SelectedNode;

            string bookmarkNameNew = "";

            if (selected != null)
            {
                bookmarkNameNew = selected.Text.Trim();

                if (BookmarksTree.SelectedNode.Level > 0)
                {
                    selected = BookmarksTree.SelectedNode.Parent;
                }

                string bookmarkName = selected.Text;

                if (TextBoxBookmarkName.Text.Trim().Length == 0)
                {
                    ApplicationMessage.ShowError("Please enter a bookmark name.", "Error");
                    TextBoxBookmarkName.Focus();
                    TextBoxBookmarkName.SelectAll();
                    return;
                }

                int index = BookmarksTree.Nodes.IndexOfKey(bookmarkNameNew);

                if (index > -1 && selected.Index != index)
                {
                    ApplicationMessage.ShowError("A bookmark with this name already exists.", "Error");
                    TextBoxBookmarkName.Focus();
                    TextBoxBookmarkName.SelectAll();
                    return;
                }

                foreach (ListViewItem item in PagesList.Items)
                {
                    Page page = (Page)item.Tag;
                    if (page.Bookmark == bookmarkName)
                    {
                        item.SubItems[2].Text = bookmarkNameNew;
                        page.Bookmark = bookmarkNameNew;
                        //AppEventHandler.OnPageChanged(this, new PageChangedEvent(page, null, PageChangedEvent.IMAGE_STATUS_CHANGED, true));
                    }
                }

                selected.Text = bookmarkNameNew;
                selected.Name = bookmarkNameNew;
            }

        }

        private void PagesList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (PagesList.SelectedItems.Count > 0)
            {

                previewPageFull = new Page((Page)PagesList.SelectedItems[0].Tag, true);

                previewForm = new ImagePreviewForm(previewPageFull);
                previewForm.ShowDialog();
                previewForm.Dispose();

                previewForm = null;

                previewPageFull.FreeImage();
                previewPageFull = null;
            }
        }

        private void ManageBookmarksForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                DialogResult = DialogResult.Cancel;
                Close();
            }
        }

        private void PagesList_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            toolStripButton4.Enabled = PagesList.SelectedItems.Count > 0;

            if (PagesList.SelectedItems.Count == 1)
            {
                if ((e.Item.Tag as Page).Id != previewPage?.Id)
                {
                    if (previewPage != null)
                    {
                        PreviewPictureBox.Image = null;
                        previewPage.FreeImage();
                    }

                    previewPage = new Page((Page)e.Item.Tag, true);
                    PreviewPictureBox.Image = previewPage.GetThumbnail(286, 396);
                }
            }
            /*
            else
            {
                if (PagesList.SelectedItems.Count == 0)
                {
                    PreviewPictureBox.Image = null;
                    if (previewPage != null)
                    {
                        previewPage.FreeImage();
                    }

                    //previewPage = null;
                }
            }
            */
        }

        private void ToolButtonSelectRange_Click(object sender, EventArgs e)
        {
            PageRangeSelectionForm pageRangeSelectionForm = new PageRangeSelectionForm(useOffset, lastOffset);
            if (pageRangeSelectionForm.ShowDialog() == DialogResult.OK)
            {
                useOffset = pageRangeSelectionForm.UseOffset;
                lastOffset = pageRangeSelectionForm.Offset;

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
                                    PagesList.EnsureVisible(item.Index);
                                    visibilityEnsured = true;
                                }
                            }
                        }
                    }

                    return false; // Continue to next selection
                });
            }
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            try
            {
                if (BookmarksTree.Nodes.Count == 0)
                {
                    return;
                }


                MemoryStream copyMemStream = new MemoryStream();
                var utf8WithoutBom = new System.Text.UTF8Encoding(false);

                MemoryStream ms = new MemoryStream();

                XmlWriterSettings writerSettings = new XmlWriterSettings
                {
                    OmitXmlDeclaration = true,
                    Encoding = Encoding.UTF8,
                };

                XmlWriter xmlWriter = XmlWriter.Create(ms, writerSettings);

                xmlWriter.WriteStartDocument();

                xmlWriter.WriteStartElement("Bookmarks");
                foreach (TreeNode lv0Node in BookmarksTree.Nodes)
                {
                    xmlWriter.WriteStartElement("Bookmark");
                    xmlWriter.WriteAttributeString("Name", lv0Node.Text);


                    foreach (TreeNode lv1Node in lv0Node.Nodes)
                    {
                        xmlWriter.WriteStartElement("Page");
                        xmlWriter.WriteAttributeString("Name", lv1Node.Text);
                        xmlWriter.WriteAttributeString("Index", (lv1Node.Tag as Page).Index.ToString());
                        xmlWriter.WriteEndElement();
                    }

                    xmlWriter.WriteEndElement();
                }


                xmlWriter.WriteEndElement();

                //if (!withoutXMLHeaderTag)
                //{
                xmlWriter.WriteEndDocument();
                //}

                xmlWriter.Close();

                ms.Position = 0;


                XmlDocument doc = new XmlDocument
                {
                    PreserveWhitespace = true,
                };

                XmlReader MetaDataReader = XmlReader.Create(ms);
                MetaDataReader.Read();
                doc.Load(MetaDataReader);
                doc.Save(copyMemStream);
                copyMemStream.Position = 0;

                byte[] encoded = new byte[copyMemStream.Length];
                copyMemStream.Read(encoded, 0, (int)copyMemStream.Length);

                DataObject data = new DataObject();
                data.SetData(DataFormats.UnicodeText, utf8WithoutBom.GetString(encoded));

                Clipboard.SetDataObject(data);

                copyMemStream.Close();
                ms.Close();
            }
            catch (Exception ex)
            {
                ApplicationMessage.ShowException(ex);
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {

        }
    }
}
