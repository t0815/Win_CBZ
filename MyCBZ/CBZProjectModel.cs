using MyCBZ;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CBZMage
{
    internal class CBZProjectModel
    {
        public String Name { get; set; }
        
        public String Description { get; set; }

        public String WorkingDir { get; set; }

        protected Boolean IsNew = false;

        protected Boolean IsSaved = false;

        protected Boolean IsClosed = false;

        public CBZArchiveInfo CurrentFile { get; set; }

        public List<CBZImage> Pages { get; set; }

        public CBZMetaData MetaData { get; set; }

        public long TotalSize { get; set; }

        private System.IO.Compression.ZipArchive Archive { get; set; }

        private ZipArchiveMode Mode;

        public event EventHandler<CBZArchiveInfo> ArchiveChanged;

        public event EventHandler<ItemChangedEvent> ItemChanged;

        public event EventHandler<ItemLoadProgressEvent> ImageProgress;

        public event EventHandler<ItemFailedEvent> ItemFailed;

        public event EventHandler<CBZArchiveStatusEvent> ArchiveStatusChanged;

        public event EventHandler<MetaDataLoadEvent> MetaDataLoaded;

        private Thread LoadArchiveThread;

        private Thread CloseArchiveThread;

       


        public CBZProjectModel(String workingDir)
        {

            CurrentFile = new CBZArchiveInfo();
            Pages = new List<CBZImage>();
            MetaData = new CBZMetaData();
        }


        public Thread Open(String name, ZipArchiveMode mode)
        {
            Name = name;
            Mode = mode;

            if (LoadArchiveThread != null)
            {
                if (LoadArchiveThread.IsAlive)
                {
                    LoadArchiveThread.Abort();
                }
            }

            LoadArchiveThread = new Thread(new ThreadStart(OpenArchiveProc));
            LoadArchiveThread.Start();

            return LoadArchiveThread;
        }

        public void AddImages(List<CBZLocalFile> fileList, int maxIndex = 0)
        {
            int index = maxIndex;

            foreach (CBZLocalFile fileObject in fileList)
            {
                CBZImage cBZImage = new CBZImage(fileObject.FileInfo);
                cBZImage.Size = fileObject.FileSize;
                cBZImage.Number = index + 1;
                cBZImage.Index = index;
                cBZImage.Filename = fileObject.FullPath;
                cBZImage.Compressed = false;
                cBZImage.LastModified = fileObject.LastModified;

                Pages.Add(cBZImage);

                OnImageLoaded(new ItemLoadProgressEvent(index, fileList.Count, cBZImage));

                index++;
            }

        }

        public List<CBZLocalFile> LoadDirectory(String path)
        {
            List<CBZLocalFile> files = new List<CBZLocalFile>();

            DirectoryInfo di = new DirectoryInfo(path);

            try
            {
                foreach (var fi in di.EnumerateFiles())
                {
                    CBZLocalFile localFile = new CBZLocalFile(fi.FullName);
                    localFile.FilePath = di.FullName;
                    localFile.FileName = fi.FullName;
                    localFile.FileSize = fi.Length;

                    files.Add(localFile);
                }
            } catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            } finally
            {
                
            }

            return files;
        }

        public List<CBZLocalFile> parseFiles(List<String> files)
        {
            List<CBZLocalFile> filesObjects = new List<CBZLocalFile>();

            try
            {
                foreach (String fname in files)
                {
                    var fi = new FileInfo(fname);
                     
                    CBZLocalFile localFile = new CBZLocalFile(fi.FullName);
                    localFile.FilePath = fi.Directory.FullName;
                    localFile.FileName = fi.Name;
                    localFile.FileSize = fi.Length;
                    localFile.FileInfo = fi;

                    //fi.SetAccessControl()

                    filesObjects.Add(localFile);
                }

            } catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            } finally
            {
                //
            }

            return filesObjects;
        }

        public Thread Close()
        {
            if (LoadArchiveThread != null)
            {
                if (LoadArchiveThread.IsAlive)
                {
                    LoadArchiveThread.Abort();
                }
            }

            CloseArchiveThread = new Thread(new ThreadStart(CloseArchiveProc));
            CloseArchiveThread.Start();

            return CloseArchiveThread;
        }

        protected void OpenArchiveProc()
        {
            long itemSize = 0;
            int index = 0;
            long totalSize = 0;

            OnArchiveStatusChanged(new CBZArchiveStatusEvent(CurrentFile, CBZArchiveStatusEvent.ARCHIVE_OPENING));

            int count = 0;
            try
            {
                Archive = ZipFile.Open(Name, Mode);
                count = Archive.Entries.Count;

                try
                {
                    ZipArchiveEntry metaDataEntry = Archive.GetEntry("ComicInfo.xml");

                    if (metaDataEntry != null)
                    {
                        MetaData = new CBZMetaData(metaDataEntry.Open(), metaDataEntry.FullName);

                        OnMetaDataLoaded(new MetaDataLoadEvent(MetaData.Values));
                    }
                } catch (Exception)
                { }

                foreach (ZipArchiveEntry entry in Archive.Entries)
                {
                    if (!entry.FullName.ToLower().Contains("comicinfo.xml"))
                    {
                        itemSize = entry.Length;
                        CBZImage cBZImage = new CBZImage(entry.Open(), entry.FullName);
                        cBZImage.Size = itemSize;
                        cBZImage.Number = index + 1;
                        cBZImage.Index = index;
                        cBZImage.Filename = entry.FullName;
                        cBZImage.Compressed = true;
                        cBZImage.LastModified = entry.LastWriteTime;

                        Pages.Add(cBZImage);
                        OnImageLoaded(new ItemLoadProgressEvent(index, count, cBZImage));

                        totalSize += itemSize;
                        index++;
                    }

                    Thread.Sleep(100);
                }
            } catch (Exception e)
            {
                MessageBox.Show("Error opening Archive\n" + e.Message, "Error");
            }

            CurrentFile.FileSize = totalSize;

            OnArchiveStatusChanged(new CBZArchiveStatusEvent(CurrentFile, CBZArchiveStatusEvent.ARCHIVE_OPENED));
        }

        protected void CloseArchiveProc()
        {
            OnArchiveStatusChanged(new CBZArchiveStatusEvent(CurrentFile, CBZArchiveStatusEvent.ARCHIVE_CLOSING));

            if (Archive != null)
            {
                Archive.Dispose();
            }

            CurrentFile.FileSize = 0;
            foreach (CBZImage page in Pages)
            {
                page.FreeImage();
                OnItemChanged(new ItemChangedEvent(page.Index, Pages.Count, page));
                Thread.Sleep(100);
            }

            Pages.Clear();
            MetaData.Free();

            Name = "";

            OnArchiveStatusChanged(new CBZArchiveStatusEvent(CurrentFile, CBZArchiveStatusEvent.ARCHIVE_CLOSED));
        }

        protected virtual void OnImageLoaded(ItemLoadProgressEvent e)
        {
            EventHandler<ItemLoadProgressEvent> handler = ImageProgress;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnItemChanged(ItemChangedEvent e)
        {
            EventHandler<ItemChangedEvent> handler = ItemChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnArchiveStatusChanged(CBZArchiveStatusEvent e)
        {
            EventHandler<CBZArchiveStatusEvent> handler = ArchiveStatusChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnMetaDataLoaded(MetaDataLoadEvent e)
        {
            EventHandler<MetaDataLoadEvent> handler = MetaDataLoaded;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }
}
