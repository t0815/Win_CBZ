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
            return new Task<Bitmap>(token =>
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

        public static Task<Bitmap> CreatePaletteTask(int w, int h, CancellationToken cancellationToken, Nullable<Color> color = null)
        {
            return new Task<Bitmap>(token =>
            {
                Bitmap palette = new Bitmap(w, h);

                int totalPixels = w * h;

                int maxBlockSize = (int)Math.Ceiling(totalPixels / Colors.GetPalette().Count / 2f);

                for (int y = 0; y < h; y++)
                {
                    for (int x = 0; x < w; x++)
                    {
                        

                    }
                }

                //ImageOperations.ResizeImage(ref palette, new Size(w, h), Color.Black, System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic);

                return palette;
            }, cancellationToken);
        }

        public static Task<Bitmap> CreateRainbowTask(int w, int h, CancellationToken cancellationToken, Nullable<Color> fromColor = null)
        {
            return new Task<Bitmap>(token =>
            {
                Bitmap palette = new Bitmap(w, h);

                int totalPixels = w * h;

                int pixelIndex = 0;

                for (int x = 0; x < w; x++)
                {
                    for (int y = 0; y < h; y++)
                    {
                        // Calculate Hue based on x (0-360 deg)
                        float hue = (y / (float)(h - 1)) * 360f;

                        // use full saturation and value
                        float saturation = 1.0f;
                        float value = 1.0f;

                        // Convert HSV to RGB
                        Color color = Colors.ColorFromHSV(hue, saturation, value);
                        palette.SetPixel(x, y, color);
                    }
                }

                return palette;
            }, cancellationToken);
        }

        public static Task<Bitmap> CreateThreeColorGradientTask(int w, int h, Color middleColor, CancellationToken cancellationToken)
        {
            return new Task<Bitmap>(token =>
            {
                Bitmap palette = new Bitmap(w, h);
                
                Color black = Color.Black;
                Color white = Color.White;

                float maxDistance = (float)Math.Sqrt(w * w + h * h);

                for (int x = 0; x < w; x++)
                {
                    for (int y = 0; y < h; y++)
                    {
                        // Distance from top-left corner (0,0)
                        float distance = (float)Math.Sqrt(x * x + y * y);

                        // normalized distance (0.0 to 1.0)
                        float normalizedDistance = distance / maxDistance;

                        Color pixelColor;

                        if (normalizedDistance <= 0.5f)
                        {
                            // first half: From Black to Middle Color
                            float t = normalizedDistance * 2f; // 0-0.5 wird zu 0-1
                            int r = (int)(black.R + (middleColor.R - black.R) * t);
                            int g = (int)(black.G + (middleColor.G - black.G) * t);
                            int b = (int)(black.B + (middleColor.B - black.B) * t);
                            pixelColor = Color.FromArgb(r, g, b);
                        }
                        else
                        {
                            // second half: From Middle Color to White
                            float t = (normalizedDistance - 0.5f) * 2f; // 0.5-1 wird zu 0-1
                            int r = (int)(middleColor.R + (white.R - middleColor.R) * t);
                            int g = (int)(middleColor.G + (white.G - middleColor.G) * t);
                            int b = (int)(middleColor.B + (white.B - middleColor.B) * t);
                            pixelColor = Color.FromArgb(r, g, b);
                        }

                        palette.SetPixel(x, y, pixelColor);
                    }
                }

                return palette;
            }, cancellationToken);
        }

        public static Task<Bitmap> CreateHorizontalGradientTask(int w, int h, Color startColor, Color endColor, CancellationToken cancellationToken)
        {
            return new Task<Bitmap>(token =>
            {
                Bitmap gradient = new Bitmap(w, h);

                for (int x = 0; x < w; x++)
                {
                    // Factor (0.0 bis 1.0)
                    float t = x / (float)(w - 1);

                    int r = (int)(startColor.R + (endColor.R - startColor.R) * t);
                    int g = (int)(startColor.G + (endColor.G - startColor.G) * t);
                    int b = (int)(startColor.B + (endColor.B - startColor.B) * t);
                    int a = (int)(startColor.A + (endColor.A - startColor.A) * t);

                    Color pixelColor = Color.FromArgb(a, r, g, b);

                    for (int y = 0; y < h; y++)
                    {
                        gradient.SetPixel(x, y, pixelColor);
                    }
                }

                return gradient;
            }, cancellationToken);
        }
    }
}
