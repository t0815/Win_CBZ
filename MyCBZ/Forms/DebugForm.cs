using SharpCompress.Archives.Zip;
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

namespace Win_CBZ.Forms
{
    public partial class DebugForm : Form
    {
        public DebugForm()
        {
            InitializeComponent();
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

            foreach (ZipArchiveEntry entry in j.Entries)
            {
                string t = entry.Crc.ToString();
                 //j.DeflateCompressionLevel = SharpCompress.Compressors.Deflate.CompressionLevel.BestCompression;
                using (Stream es = entry.OpenEntryStream())
                {
                    
                }

            }
        }
    }
}
