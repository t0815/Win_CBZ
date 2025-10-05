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
            return "#" + color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2");
        }

    }
}