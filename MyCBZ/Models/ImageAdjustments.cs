using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Win_CBZ.Models
{
    internal class ImageAdjustments
    {
        public const int ADJUSTMENT_SPLIT_TYPE_PERCENT = 0;
        public const int ADJUSTMENT_SPLIT_TYPE_PX = 1;

        public bool SplitPage { get; set; }

        public int SplitPageAt { get; set; }

        public int SplitType { get; set; }

        public Color DetectSplitAtColor { get; set; }

        public Point ResizeTo { get; set; }

        public int ResizeMode { get; set; }

        public int ResizeToPageNumber {  get; set; }
    }
}
