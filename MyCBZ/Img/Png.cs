using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Drawing.Drawing2D;
using System.Drawing;

namespace Win_CBZ.Img
{
    internal class Png
    {

        public void Encode(Image source)
        {
            int stride = source.Width;

            byte[] pixels = new byte[source.Width * source.Height];

            //Bitmap bitmap = source.


            // Define the image palette
            BitmapPalette myPalette = BitmapPalettes.Halftone216;

            // Creates a new empty image with the pre-defined palette

            BitmapSource image = BitmapSource.Create(
                source.Width,
                source.Height,
                96,
                96,
                PixelFormats.Indexed8,
                myPalette,
                pixels,
                stride);

            

            FileStream stream = new FileStream("new.png", FileMode.Create);
            PngBitmapEncoder encoder = new PngBitmapEncoder();
            //TextBlock myTextBlock = new TextBlock();
            //myTextBlock.Text = "Codec Author is: " + encoder.CodecInfo.Author.ToString();
            encoder.Interlace = PngInterlaceOption.On;
            encoder.Frames.Add(BitmapFrame.Create(image));
            encoder.Save(stream);

        }


        public Image Decode(string source)
        {
            // Open a Stream and decode a PNG image
            Stream imageStreamSource = new FileStream(source, FileMode.Open, FileAccess.Read, FileShare.Read);
            PngBitmapDecoder decoder = new PngBitmapDecoder(imageStreamSource, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
            BitmapSource bitmapSource = decoder.Frames[0];

            System.Drawing.Bitmap bitmap;
            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapSource));
                enc.Save(outStream);
                bitmap = new System.Drawing.Bitmap(outStream);
            }


            // make Image
            return Image.FromHbitmap(bitmap.GetHbitmap());
        }
    }
}
