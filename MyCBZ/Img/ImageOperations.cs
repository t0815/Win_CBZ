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

namespace Win_CBZ.Img
{
    internal class ImageOperations
    {
        /// <summary>
        /// Resize the image to the specified width and height.
        /// </summary>
        /// <param name="image">The image to resize.</param>
        /// <param name="width">The width to resize to.</param>
        /// <param name="height">The height to resize to.</param>
        /// <returns>The resized image.</returns>
        public static void ResizeImage(Stream source, ref Stream OutputStream, ImageFormat targetFormat, InterpolationMode interpolation)
        {
            Image sourceImage = Image.FromStream(source);
            var destRect = new Rectangle(0, 0, sourceImage.Width, sourceImage.Height);
            var destImage = new Bitmap(sourceImage.Width, sourceImage.Height);

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

            destImage.Save(OutputStream, targetFormat);
            destImage.Dispose();

            sourceImage.Dispose();
        }

        public static void CutImage(Stream source, ref Stream OutputStream, ImageFormat targetFormat, int px, int py, int w, int h, InterpolationMode interpolation)
        {
            var destRect = new Rectangle(0, 0, w, h);
            var destImage = new Bitmap(w, h);
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
                    graphics.DrawImage(sourceImage, destRect, px, py, w, h, GraphicsUnit.Pixel, wrapMode);
                }
            }

            destImage.Save(OutputStream, targetFormat);
            destImage.Dispose();

            sourceImage.Dispose();
        }
        
    }
}
