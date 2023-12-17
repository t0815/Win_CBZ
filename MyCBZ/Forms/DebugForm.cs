﻿using SharpCompress.Archives.Zip;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Win_CBZ.Forms
{
    public partial class DebugForm : Form
    {

        public ListView PageView;

        public DebugForm(ListView source)
        {
            InitializeComponent();

            PageView = source;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.ShowDialog();

            string filename = ofd.FileName;

            LocalFile input = new LocalFile(filename);

            ZipArchive j = ZipArchive.Open(input.LocalFileInfo);

            string[] items = j.Entries.Select(s => s.Key).ToArray();

            List<ZipArchiveEntry> result = j.Entries.Where(s => s.Key.ToLower() == "comicinfo.xml").ToList();
            ListViewItem it;

            foreach (ZipArchiveEntry entry in j.Entries)
            {
                string t = entry.Crc.ToString();

                it = listView1.Items.Add(""); 
                it.SubItems.Add(t);
                it.SubItems.Add(entry.Key);

                 //j.DeflateCompressionLevel = SharpCompress.Compressors.Deflate.CompressionLevel.BestCompression;
                using (Stream es = entry.OpenEntryStream())
                {
                    
                }
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
    }
}
