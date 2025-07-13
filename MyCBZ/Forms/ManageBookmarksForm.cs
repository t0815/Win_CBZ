using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using Win_CBZ.Handler;

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



            foreach (ListViewItem item in PagesList.SelectedItems)
            {
                Page page = (Page)item.Tag;

                
                page.Bookmark = TextBoxBookmarkName.Text.Trim();
                           
                item.SubItems[2].Text = page.Bookmark;

                //AppEventHandler.OnPageChanged(this, new PageChangedEvent(page, null, PageChangedEvent.IMAGE_STATUS_CHANGED, true));
            }

        }

        private void BookmarksTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node != null)
            {
                _selectedNode = e.Node;
                TextBoxBookmarkName.Text = e.Node.Text;
            }
            else
            {
                _selectedNode = null;
                TextBoxBookmarkName.Text = string.Empty;
            }
        }
    }
}
