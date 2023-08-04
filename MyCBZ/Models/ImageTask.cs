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

        public int SplitPagePercent { get; set; }

        public Color DetectSplitAtColor { get; set; }

        public Point ResizeTo { get; set; }

        public Image Result { get; set; }

        public Image ResultThumbnail { get; set; }

        public String SourceFileName { get; set; }

        public string PreviewFileName { get; set; }


        public ImageTask(String sourceImage)
        {
            try
            {
                PreviewFileName = sourceImage + "_0";
                File.Copy(sourceImage, PreviewFileName, true);
            } catch (Exception)
            {

            }
        }


        public void Resize()
        {

        }


        public void CutDobulePage()
        {
            FileStream fs = File.Open(PreviewFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            Result = Image.FromStream(fs);
            fs.Position = 0;
            using (Graphics gfxContext = Graphics.FromImage(Result))
            {
                //gfxContext.DrawImageUnscaledAndClipped()
            }

            //Result = Image.FromStream(fs);
        }

    }
}
