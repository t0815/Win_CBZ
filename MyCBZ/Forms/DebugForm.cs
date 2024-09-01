using AutocompleteMenuNS;
using SharpCompress.Archives.Zip;
using SharpCompress.Readers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Interop;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Win_CBZ.Forms
{
    [SupportedOSPlatform("windows")]
    public partial class DebugForm : Form
    {

        public ListView PageView;

        public DebugForm(ListView source)
        {
            InitializeComponent();

            cdg.Columns.Add(new DataGridViewColumn()
            {
                DataPropertyName = "Key",
                HeaderText = "Key",
                CellTemplate = new DataGridViewTextBoxCell(),
                Width = 150,
                SortMode = DataGridViewColumnSortMode.Automatic,
            });

            PageView = source;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.ShowDialog();

            string filename = ofd.FileName;

            LocalFile input = new LocalFile(filename);
            
            ZipArchive j = ZipArchive.Open(input.LocalFileInfo);
            /*
            using (IReader reader = ReaderFactory.Open(stream))
            {
                if (reader.ArchiveType == SharpCompress.Common.ArchiveType.Zip)
                {

                    List<ZipArchiveEntry> resultComicInfo = j.Entries.Where(s => s.Key.ToLower() == "comicinfo.xml").ToList();
                    List<ZipArchiveEntry> resultFiles = j.Entries.Where(s => s.Key.ToLower() != "comicinfo.xml").ToList();
                    ListViewItem it;

                    foreach (ZipArchiveEntry entry in resultFiles)
                    {
                        string t = entry.Crc.ToString();

                        it = listView1.Items.Add(entry.Key);
                        it.SubItems.Add(t);
                        it.SubItems.Add(entry.Size.ToString());

                        //j.DeflateCompressionLevel = SharpCompress.Compressors.Deflate.CompressionLevel.BestCompression;
                        //using (Stream es = entry.OpenEntryStream())
                        // {
                        //     
                        // }
                    }
                }
                
            }
            */

            List<ZipArchiveEntry> resultComicInfo = j.Entries.Where(s => s.Key.ToLower() == "comicinfo.xml").ToList();
            List<ZipArchiveEntry> resultFiles = j.Entries.Where(s => s.Key.ToLower() != "comicinfo.xml").ToList();
            ListViewItem it;

            foreach (ZipArchiveEntry entry in resultFiles)
            {
                string t = entry.Crc.ToString();

                it = listView1.Items.Add(entry.Key); 
                it.SubItems.Add(t);
                it.SubItems.Add(entry.Size.ToString());

                 //j.DeflateCompressionLevel = SharpCompress.Compressors.Deflate.CompressionLevel.BestCompression;
                //using (Stream es = entry.OpenEntryStream())
               // {
               //     
               // }
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listView2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            listView2.Items.Clear();

            ListViewItem idebug;
            foreach (ListViewItem item in PageView.Items)
            {
                idebug = listView1.Items.Add(item.Name);
                idebug.SubItems.Add(((Page)item.Tag).Index.ToString());
                idebug.SubItems.Add(((Page)item.Tag).Id.ToString());
                idebug.SubItems.Add(((Page)item.Tag).Name);
            }

            foreach (Page p in Program.ProjectModel.Pages)
            {
                idebug = listView2.Items.Add(p.Name);
                idebug.SubItems.Add(p.Index.ToString());
                idebug.SubItems.Add(p.Id.ToString());
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            

            cdg.Rows.Add("");
        }

        private void cdg_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            TextBox textBox = e.Control as TextBox;
            //textBox.KeyDown += DataGridTextBoxKeyDown;

            autocompleteMenu1.Items = new string[] { "abc", "a100", "eee", "defghij", "best item", "new", "test item", "doomed", "that is a test", "some item", "very good item" };
            autocompleteMenu1.SetAutocompleteMenu(textBox, autocompleteMenu1);
        }

        private void autocompleteMenu1_Selected(object sender, SelectedEventArgs e)
        {
            e.Control.Text = e.Item.Text;
        }
    }
}
