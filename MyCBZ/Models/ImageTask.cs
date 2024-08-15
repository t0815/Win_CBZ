using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Win_CBZ.Helper;
using static System.Net.Mime.MediaTypeNames;
using Image = System.Drawing.Image;

namespace Win_CBZ.Models
{
    [SupportedOSPlatform("windows")]
    internal class ImageTask
    {
        public const string TASK_SPLIT_PERCENT = "SplitPercent";
        public const string TASK_DETECT_SPLIT_BY_COLOR = "DetectAndSplitByColor";
        public const string TASK_DETECT_RESIZE = "ResizeImage";

        public ImageAdjustments ImageAdjustments { get; set; }

        public PageImageFormat[] ImageFormat { get; set; }

        public PageImageFormat SourceFormat { get; set; }

        public Image[] ResultImage { get; set; }

        public Image[] ResultThumbnail { get; set; }

        public LocalFile[] ResultFileName { get; set; }

        public LocalFile SourceFileName { get; set; }

        public LocalFile[] PreviewFile { get; set; }

        public List<String> Tasks { get; set; }

        public bool Success = false;

        public Page[] ResultPages { get; set; }

        public Page SourcePage { get; set; }


        public ImageTask()
        {
            Tasks = new List<String>();
            ImageAdjustments = new ImageAdjustments();
            ResultImage = new Image[2];
            PreviewFile = new LocalFile[2];
            ResultThumbnail = new Image[2];
            ResultFileName = new LocalFile[2];
            ImageFormat = new PageImageFormat[2];
            ResultPages = new Page[2];

        }

        public void SetupTasks(Page source, List<String> commandsTodo)
        {
            try
            {
                PreviewFile[0] = new LocalFile(source.TemporaryFile.FilePath + RandomId.getInstance().make() + "0.prev");
                PreviewFile[1] = new LocalFile(source.TemporaryFile.FilePath + RandomId.getInstance().make() + "1.prev");
                File.Copy(source.TemporaryFile.FullPath, PreviewFile[0].FullPath, true);
                File.Copy(source.TemporaryFile.FullPath, PreviewFile[1].FullPath, true);
                Tasks = commandsTodo;
                ImageFormat[0] = source.Format;
                SourceFormat = source.Format;
                SourcePage = source;
            }
            catch (Exception)
            {

            }           
        }

        public void apply()
        {
            foreach (String task in Tasks)
            {
                switch (task)
                {
                    case TASK_DETECT_RESIZE:


                        break;
                }
            }
        }

        public int TaskCount()
        { 
            return Tasks.Count;
        }

        public void CleanUp()
        {

        }
    }
}
