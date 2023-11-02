using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using Image = System.Drawing.Image;

namespace Win_CBZ.Models
{
    internal class ImageTask
    {
        public const string TASK_SPLIT_PERCENT = "SplitPercent";
        public const string TASK_DETECT_SPLIT_BY_COLOR = "DetectAndSplitByColor";
        public const string TASK_DETECT_RESIZE = "ResizeImage";

        public ImageAdjustments ImageAdjustments { get; set; }

        public Image[] ResultImage { get; set; }

        public Image[] ResultThumbnail { get; set; }

        public String[] ResultFileName { get; set; }

        public String SourceFileName { get; set; }

        public String[] PreviewFileName { get; set; }

        public List<String> CommandsTodo { get; set; }

        public bool Success = false;


        public ImageTask()
        {
            CommandsTodo = new List<String>();
            ImageAdjustments = new ImageAdjustments();
            ResultImage = new Image[2];
            PreviewFileName = new String[2];
            ResultThumbnail = new Image[2];
            ResultFileName = new String[2];

        }

        public void SetupTasks(Page source, List<String> commandsTodo)
        {
            try
            {
                PreviewFileName[0] = source.TempPath + "_0";
                File.Copy(source.TempPath, PreviewFileName[0], true);
                CommandsTodo = commandsTodo;
            }
            catch (Exception)
            {

            }
            
        }

        public void PerformCommands()
        {
            foreach (var item in CommandsTodo)
            {

            }
        }

        public Page[] Result()
        {
            Page[] result = new Page[2];

            //result.Append();

            return result;
        }


        public void Resize()
        {

        }


        public void CutDobulePage()
        {
            FileStream fs = File.Open(PreviewFileName[0], FileMode.OpenOrCreate, FileAccess.ReadWrite);
            ResultImage[0] = Image.FromStream(fs);
            fs.Position = 0;
            using (Graphics gfxContext = Graphics.FromImage(ResultImage[0]))
            {
                //gfxContext.DrawImageUnscaledAndClipped()
            }

            //Result = Image.FromStream(fs);
        }

    }
}
