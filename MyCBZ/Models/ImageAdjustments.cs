using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
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

        public Point MaxDimensions { get; set; }

        public bool KeepAspectRatio { get; set; }

        public int ResizeMode { get; set; }

        public int ResizeToPageNumber {  get; set; }

        public float ResizeToPercentage { get; set; }

        public int ConvertType { get; set; }

        public bool DontStretch { get; set; }

        public bool Grayscale { get; set; }

        public PageImageFormat ConvertFormat { get; set; }

        public InterpolationMode Interpolation { get; set; }   

        public ImageAdjustments(ImageAdjustments copyFrom = null)
        {
            if (copyFrom != null)
            {
                SplitPage = copyFrom.SplitPage;
                SplitPageAt = copyFrom.SplitPageAt;
                SplitType = copyFrom.SplitType;
                DetectSplitAtColor = copyFrom.DetectSplitAtColor;
                ResizeTo = copyFrom.ResizeTo;
                ResizeMode = copyFrom.ResizeMode;
                ResizeToPageNumber = copyFrom.ResizeToPageNumber;
                ConvertType = copyFrom.ConvertType;
                ConvertFormat = copyFrom.ConvertFormat;
                Interpolation = copyFrom.Interpolation;
                MaxDimensions = copyFrom.MaxDimensions;
                KeepAspectRatio = copyFrom.KeepAspectRatio;
                ResizeToPercentage = copyFrom.ResizeToPercentage;
                DontStretch = copyFrom.DontStretch;
                Grayscale = copyFrom.Grayscale;
            }
        }
    }
}
