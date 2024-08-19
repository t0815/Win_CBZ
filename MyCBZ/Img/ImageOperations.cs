using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using System.IO;
using Win_CBZ.Models;
using System.Runtime.Versioning;

namespace Win_CBZ.Img
{
    [SupportedOSPlatform("windows")]
    internal class ImageOperations
    {

        public static void ConvertImage(Stream source, Stream outputStream, ref PageImageFormat targetFormat)
        {
            Image sourceImage = Image.FromStream(source);

            sourceImage.Save(outputStream, targetFormat.Format);
            sourceImage.Dispose();
        }

        /// <summary>
        /// Resize the image to the specified width and height.
        /// </summary>
        /// <param name="image">The image to resize.</param>
        /// <param name="width">The width to resize to.</param>
        /// <param name="height">The height to resize to.</param>
        /// <returns>The resized image.</returns>
        public static void ResizeImage(Stream source, ref Stream OutputStream, ref PageImageFormat targetFormat, InterpolationMode interpolation)
        {
            Image sourceImage = Image.FromStream(source);
            var destRect = new Rectangle(0, 0, targetFormat.W, targetFormat.W);
            var destImage = new Bitmap(targetFormat.W, targetFormat.H);

            destImage.SetResolution(sourceImage.HorizontalResolution, sourceImage.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = interpolation;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(sourceImage, destRect, 0, 0, sourceImage.Width, sourceImage.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            destImage.Save(OutputStream, targetFormat.Format);
            destImage.Dispose();

            sourceImage.Dispose();
        }

        public static void CutImage(Stream source, ref Stream OutputStream, PageImageFormat targetFormat, InterpolationMode interpolation)
        {
            var destRect = new Rectangle(0, 0, targetFormat.W, targetFormat.H);
            var destImage = new Bitmap(targetFormat.W, targetFormat.H);
            Image sourceImage = Image.FromStream(source);

            destImage.SetResolution(sourceImage.HorizontalResolution, sourceImage.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = interpolation;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(sourceImage, destRect, targetFormat.X, targetFormat.Y, targetFormat.W, targetFormat.H, GraphicsUnit.Pixel, wrapMode);
                }
            }

            destImage.Save(OutputStream, targetFormat.Format);
            destImage.Dispose();

            sourceImage.Dispose();
        }
        
    }
}
