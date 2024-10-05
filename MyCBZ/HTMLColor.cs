using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Win_CBZ
{
    internal class HTMLColor
    {

        public static Color ToColor(String htmlColor, int alpha = 255)
        {
            int red = 0;
            int green = 0;
            int blue = 0;

            if (htmlColor != null && htmlColor.Length > 0)
            {
                if (htmlColor[..1].Equals("#"))
                {
                    red = Convert.ToInt32(htmlColor.Substring(1, 2), 16);
                    green = Convert.ToInt32(htmlColor.Substring(3, 2), 16);
                    blue = Convert.ToInt32(htmlColor.Substring(5, 2), 16);
                }
                
            }

            return Color.FromArgb(alpha, red, green, blue);
        }

        public static String ToHexColor(Color color)
        {
            byte red = 0;
            byte green = 0;
            byte blue = 0;
            byte alpha = 0;
           
            red = color.R; 
            green = color.G; 
            blue = color.B;
            alpha = color.A;  // not supported

            return "#" + red.ToString("X2") + green.ToString("X2") + blue.ToString("X2");
        }

    }
}