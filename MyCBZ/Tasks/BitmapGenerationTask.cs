using SharpCompress.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Win_CBZ.Data;
using Win_CBZ.Events;
using Win_CBZ.Img;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Win_CBZ.Tasks
{
    [SupportedOSPlatform("windows")]
    internal class BitmapGenerationTask
    {

        public static Task<Bitmap> CreateGradientTask(int w, int h, CancellationToken cancellationToken, Nullable<Color> fromColor = null)
        {
            return new Task<Bitmap>((token) =>
            {
                Bitmap palette = new Bitmap(w, h);
                
                for (int x = 0; x < w; x++)
                {
                    for (int y = 0; y < h; y++)
                    {
                        int r = (int)((x / (float)w) * 255f);
                        int g = (int)((y / (float)h) * 255f);
                        int b = fromColor.HasValue ? fromColor.Value.B : 128;
                        palette.SetPixel(x, y, Color.FromArgb(r, g, b));
                    }
                }

                //ImageOperations.ResizeImage(ref palette, new Size(w, h), Color.Black, System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic);

                return palette;
            }, cancellationToken);
        }

        public static Task<Bitmap> CreatePaletteTask(int w, int h, CancellationToken cancellationToken, Nullable<Color> fromColor = null)
        {
            return new Task<Bitmap>((token) =>
            {
                Bitmap palette = new Bitmap(w, h);

                int totalPixels = w * h;

                int pixelIndex = 0;

                for (int x = 0; x < w; x++)
                {
                    for (int y = 0; y < h; y++)
                    {
                        // Normalisierte Koordinaten (0.0 bis 1.0)
                        float normalizedX = x / (float)(w - 1);
                        float normalizedY = pixelIndex / (float)(totalPixels - 1);
                        float normalizedZ = y / (float)(h - 1);

                        // RGB-Werte basierend auf 3D-Position im Farbraum
                        int r = (int)(normalizedX * 255f);
                        int g = (int)(normalizedY * 255f);
                        int b = (int)(normalizedZ * 255f);

                        // Stelle sicher, dass Werte im gültigen Bereich sind
                        r = Math.Max(0, Math.Min(255, r));
                        g = Math.Max(0, Math.Min(255, g));
                        b = Math.Max(0, Math.Min(255, b));

                        palette.SetPixel(x, y, Color.FromArgb(r, g, b));
                        pixelIndex++;

                    }
                }

                //ImageOperations.ResizeImage(ref palette, new Size(w, h), Color.Black, System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic);

                return palette;
            }, cancellationToken);
        }

        public static Task<Bitmap> CreateRainbowTask(int w, int h, CancellationToken cancellationToken, Nullable<Color> fromColor = null)
        {
            return new Task<Bitmap>((token) =>
            {
                Bitmap palette = new Bitmap(w, h);

                int totalPixels = w * h;

                int pixelIndex = 0;

                for (int x = 0; x < w; x++)
                {
                    for (int y = 0; y < h; y++)
                    {
                        // Berechne Hue basierend auf x-Position (0-360 Grad)
                        float hue = (y / (float)(h - 1)) * 360f;

                        // Verwende volle Saturation und Value für lebendige Farben
                        float saturation = 1.0f;
                        float value = 1.0f;

                        // Konvertiere HSV zu RGB
                        Color color = Colors.ColorFromHSV(hue, saturation, value);
                        palette.SetPixel(x, y, color);
                    }
                }

                return palette;
            }, cancellationToken);
        }
    }
}
