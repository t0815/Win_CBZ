﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Win_CBZ.Models
{
    [SupportedOSPlatform("windows")]
    internal class PageImageFormat
    {

        protected static Dictionary<string, ImageFormat> ExtensionToImageFormatMap = new Dictionary<string, ImageFormat>()
        {
            { "jpg", ImageFormat.Jpeg },
            { "png", ImageFormat.Png },
            { "bmp", ImageFormat.Bmp },
            { "tif", ImageFormat.Tiff },
        };

        protected static Dictionary<string, string> ExtensionToFormatNameMap = new Dictionary<string, string>()
        {
            { "jpg", "Jpeg Image" },
            { "png", "PNG Image" },
            { "bmp", "Bitmap Image" },
            { "tif", "TIFF Image" },
        };

        protected static Dictionary<ImageFormat, string> ImageFormatToNameMap = new Dictionary<ImageFormat, string>()
        {
            { ImageFormat.Jpeg, "Jpeg Image" },
            { ImageFormat.Png, "PNG Image" },
            { ImageFormat.Bmp, "Bitmap Image" },
            { ImageFormat.Tiff, "TIFF Image" },
        };

        protected static Dictionary<ImageFormat, string> ImageFormatToExtensionMap = new Dictionary<ImageFormat, string>()
        {
            { ImageFormat.Jpeg, "jpg" },
            { ImageFormat.Png, "png" },
            { ImageFormat.Bmp, "bmp" },
            { ImageFormat.Tiff, "tif" },
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
                ExtensionToImageFormatMap.TryGetValue(extension.ToLower().TrimStart('.'), out lookupResult);
            }

            return lookupResult;
        }


        public PageImageFormat(String extension = null)
        {
            ImageFormat lookupResult = null;
            String nameResult = null;

            W = 0;
            H = 0;
            DPI = 0;
            Format = null;
            ColorPalette = null;
            PixelFormat = PixelFormat.Format32bppRgb;
            
            if (extension != null)
            {
                ExtensionToImageFormatMap.TryGetValue(extension.ToLower().TrimStart('.'), out lookupResult);
                ExtensionToFormatNameMap.TryGetValue(extension.ToLower().TrimStart('.'), out nameResult);

                Format = lookupResult;
                Name = nameResult;
            }           
        }

        public PageImageFormat(PageImageFormat source, string newFormat = null)
        {
            ImageFormat lookupResult = null;
            String nameResult = null;

            W = source.W;
            H = source.H;
            DPI = source.DPI;
            Format = source.Format;
            ColorPalette = source.ColorPalette;
            PixelFormat = source.PixelFormat;
            Name = source.Name;

            if (newFormat != null)
            {
                ExtensionToImageFormatMap.TryGetValue(newFormat.ToLower().TrimStart('.'), out lookupResult);
                ExtensionToFormatNameMap.TryGetValue(newFormat.ToLower().TrimStart('.'), out nameResult);

                Format = lookupResult;
                Name = nameResult;
            }
        }

        public PageImageFormat(Image image)
        {
            string lookupResult = "";
            if (!image.Size.IsEmpty)
            {
                W = image.Width;
                H = image.Height;
            }
            Format = image.RawFormat;
            PixelFormat = image.PixelFormat;
            ColorPalette = image.Palette;
            DPI = image.VerticalResolution;
            if (Format != null)
            {
                ImageFormatToNameMap.TryGetValue(Format, out lookupResult);
              
                Name = lookupResult;
            }
        }

        public void Update()
        {
            String nameResult = "";
            if (Format != null)
            {
                ImageFormatToNameMap.TryGetValue(Format, out nameResult);

                Name = nameResult;
            }
        }

        public string ExpectedExtension()
        {
            string resultExt;

            if (ImageFormatToExtensionMap.TryGetValue(Format, out resultExt))
            {
                return "." + resultExt;
            }

            return "";
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

        public PageImageFormat FormatFromGUID(String guid)
        {
            Format = new ImageFormat(Guid.Parse(guid));

            return this;
        }

        public PageImageFormat FormatFromGUID(Guid guid)
        {
            Format = new ImageFormat(guid);

            return this;
        }

        public PageImageFormat FormatFromString(String name)
        {
            ImageFormat resultFormat;
            string resultName;

            if (name != null)
            {
                ExtensionToImageFormatMap.TryGetValue(name?.ToLower().TrimStart('.'), out resultFormat);
                ExtensionToFormatNameMap.TryGetValue(name?.ToLower().TrimStart('.'), out resultName);

                if (resultFormat != null)
                {
                    Format = resultFormat;
                    Name = resultName;
                }
            }

            return this;
        }
    }
}
