using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Windows.Media;

namespace Win_CBZ.Img
{
    internal class Png
    {

        public void Encode(Stream source, ref Stream destination, int w, int h)
        {
            int stride = w;

            byte[] pixels = new byte[w * h];

            for (int x = 1;x<w + 1; x++)
            {
                for (int y = 1;y<h + 1; y++)
                {
                    pixels[x * h + y] = (byte)source.ReadByte();
                }
            }
            
            // Define the image palette
            BitmapPalette myPalette = BitmapPalettes.Halftone216;


            BitmapSource image = BitmapSource.Create(
                w,
                h,
                96,
                96,
                PixelFormats.Indexed8,
                myPalette,
                pixels,
                stride);
         
            PngBitmapEncoder encoder = new PngBitmapEncoder();
            //TextBlock myTextBlock = new TextBlock();
            //myTextBlock.Text = "Codec Author is: " + encoder.CodecInfo.Author.ToString();
            encoder.Interlace = PngInterlaceOption.On;
            encoder.Frames.Add(BitmapFrame.Create(image));
            encoder.Save(destination);

        }


        public void Decode(Stream source, ref Stream destination)
        {
            PngBitmapDecoder decoder = new PngBitmapDecoder(source, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
            BitmapSource bitmapSource = decoder.Frames[0];

            BitmapEncoder enc = new BmpBitmapEncoder();
            enc.Frames.Add(BitmapFrame.Create(bitmapSource));
            enc.Save(destination);          
        }
    }
}
