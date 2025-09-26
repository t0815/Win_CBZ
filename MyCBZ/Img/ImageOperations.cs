using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Win_CBZ.Models;
using System.Runtime.Versioning;

namespace Win_CBZ.Img
{
    [SupportedOSPlatform("windows")]
    internal class ImageOperations
    {

        public static void ConvertImage(Stream source, Stream outputStream, PageImageFormat targetFormat)
        {
            Image sourceImage = Image.FromStream(source);

            sourceImage.Save(outputStream, targetFormat.Format);
            sourceImage.Dispose();
        }

        /// <summary>
        /// Resize the image to the specified width and height.
        /// Using PageImageFormat to define the target width and height.
        /// </summary>
        /// <param name="source">The inout stream of the image to resize.</param>
        /// <param name="OutputStream">The output stream to save the resized image to.</param>
        /// <param name="interpolation">The interpolation mode to use.</param>
        /// <param name="width">The width to resize to.</param>
        /// <param name="height">The height to resize to.</param>
        /// <returns>The resized image.</returns>
        public static void ResizeImage(ref Stream source, ref Stream OutputStream, PageImageFormat targetFormat, InterpolationMode interpolation)
        {
            Image sourceImage = Image.FromStream(source);
            var destRect = new Rectangle(0, 0, targetFormat.W, targetFormat.H);
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

        /// <summary>
        /// Resize the image to the specified width and height.
        /// </summary>
        /// <param name="image">The image to resize.</param>
        /// <param name="bounds">The width and height to resize to.</param>
        /// <param name="backgroundColor">The background color to use for transparent areas</param>
        /// <param name="interpolation">The interpolation mode to apply</param>
        /// <returns>void</returns>
        public static void ResizeImage(ref Image source, Size bounds, Color backgroundColor, InterpolationMode interpolation, CompositingMode compositingMode = CompositingMode.SourceCopy)
        {
            
            var destRect = new Rectangle(0, 0, bounds.Width, bounds.Height);
            var destImage = new Bitmap(bounds.Width, bounds.Height);

            destImage.SetResolution(source.HorizontalResolution, source.VerticalResolution);
            //destImage.MakeTransparent(backgroundColor);
            
            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.Clear(backgroundColor);
                graphics.CompositingMode = compositingMode;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = interpolation;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                
                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.FillRectangle(new SolidBrush(backgroundColor), new Rectangle(0, 0, bounds.Width, bounds.Height));
                    graphics.DrawImage(source, destRect, 0, 0, source.Width, source.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            source = Image.FromHbitmap(destImage.GetHbitmap());

            destImage.Dispose();
        }

        /// <summary>
        /// Resize the image to the specified width and height.
        /// </summary>
        /// <param name="image">The image to resize.</param>
        /// <param name="width">The width to resize to.</param>
        /// <param name="height">The height to resize to.</param>
        /// <returns>The resized image.</returns>
        public static void RotateImage(ref Stream source, ref Stream OutputStream, PageImageFormat targetFormat, int rotation)
        {
            Image sourceImage = Image.FromStream(source);

            if (rotation == 90)
            {
                sourceImage.RotateFlip(RotateFlipType.Rotate90FlipNone);
            }
            else if (rotation == 180)
            {
                sourceImage.RotateFlip(RotateFlipType.Rotate180FlipNone);
            }
            else if (rotation == 270)
            {
                sourceImage.RotateFlip(RotateFlipType.Rotate270FlipNone);
            }
            else if (rotation == 360)
            {
                
            }

            sourceImage.Save(OutputStream, targetFormat.Format);
            sourceImage.Dispose();
        }

        public static void CutImage(ref Stream source, ref Stream OutputStream, PageImageFormat targetFormat, InterpolationMode interpolation)
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
