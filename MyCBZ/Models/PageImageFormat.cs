using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Win_CBZ.Models
{
    internal class PageImageFormat
    {

        protected static Dictionary<string, ImageFormat> ImageFormatMap = new Dictionary<string, ImageFormat>()
        {
            { "jpg", ImageFormat.Jpeg },
            { "png", ImageFormat.Png },
            { "bmp", ImageFormat.Bmp  },
            { "tif", ImageFormat.Tiff },
        };

        public string Name { get; set; }

        public ImageFormat Format { get; set; } 

        public int X { get; set; }

        public int Y { get; set; }

        public int W { get; set; }

        public int H { get; set; }

        public PixelFormat PixelFormat { get; set; }     
        
        public float DPI { get; set; }

        public ColorPalette ColorPalette { get; set; }


        public static ImageFormat GuessFormat(String extension)
        {
            ImageFormat lookupResult = null;

            if (extension != null)
            {
                ImageFormatMap.TryGetValue(extension.ToLower().TrimStart('.'), out lookupResult);
            }

            return lookupResult;
        }


        public PageImageFormat(String extension = null)
        {
            ImageFormat lookupResult = null;

            W = 0;
            H = 0;
            DPI = 0;
            Format = null;
            ColorPalette = null;
            PixelFormat = PixelFormat.Format32bppRgb;
            
            if (extension != null)
            {
                ImageFormatMap.TryGetValue(extension.ToLower().TrimStart('.'), out lookupResult);

                Format = lookupResult;
            }           
        }

        public PageImageFormat(Image image)
        {
            
            if (!image.Size.IsEmpty)
            {
                W = image.Width;
                H = image.Height;
            }
            Format = image.RawFormat;
            PixelFormat = image.PixelFormat;
            ColorPalette = image.Palette;
            DPI = image.VerticalResolution;
        }

        public void Update(Image image)
        {
            if (!image.Size.IsEmpty)
            {
                W = image.Width;
                H = image.Height;
            } else
            {
                W = 0;
                H = 0;
            }
            Format = image.RawFormat;
            PixelFormat = image.PixelFormat;
            ColorPalette = image.Palette;
            DPI = image.VerticalResolution;
        }
    }
}
