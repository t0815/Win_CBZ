using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCBZ
{
    internal class HTMLColor
    {

        public static Color ToColor(String htmlColor)
        {
            int red = 0;
            int green = 0;
            int blue = 0;

            if (htmlColor != null)
            {
                if (htmlColor.Substring(0, 1).Equals("#"))
                {
                    red = Convert.ToInt32(htmlColor.Substring(1, 2), 16);
                    green = Convert.ToInt32(htmlColor.Substring(3, 2), 16);
                    blue = Convert.ToInt32(htmlColor.Substring(5, 2), 16);
                }
                
            }

            return Color.FromArgb(red, green, blue);
        }

    }
}