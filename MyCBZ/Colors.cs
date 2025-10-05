using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Win_CBZ
{
    internal class Colors
    {
        public const String COLOR_LIGHT_ORANGE = "#DAF7A6";
        public const String COLOR_LIGHT_GREEN = "#42f590";
        public const String COLOR_LIGHT_BLUE = "#62dfee";
        public const String COLOR_LIGHT_PURPLE = "#CBC3E3";
        public const String COLOR_LIGHT_GREY = "#f0efed";
        public const String COLOR_ERROR_RED = "#9c3838";

        public const String COLOR_BLACK = "#000000";
        public const String COLOR_WHITE = "#FFFFFF";

        public const String COLOR_GOLD = "#FFD700";
        public const String COLOR_TANGERINE = "#F28500";
        public const String COLOR_PLUM = "#8E4585";
        public const String COLOR_LAVENDAR_PINKISH = "#cc99cc";
        public const String COLOR_MANGO = "#fdbe02";
        public const String COLOR_CRYOLA = "#FF5800";
        public const String COLOR_GRAPE_LIGHT_PINK = "#FF878D";
        public const String COLOR_NEON_GREEN = "#9FE888";
        public const String COLOR_SKY_BLUE = "#83BEE4";
        public const String COLOR_POWDER_BLUE = "#B0E0E6";
        public const String COLOR_TURQUOISE = "#40E0D0";
        public const String COLOR_PATRIARCH = "#800080";
        public const String COLOR_CRIMSON = "#E32636";
        public const String COLOR_AQUAMARINE = "#7FFFD4";
        public const String COLOR_BABY_BLUE = "#89CFF0";
        public const String COLOR_BABY_PINK = "#F4C2C2";
        public const String COLOR_BANANA_YELLOW = "#FFE135";
        public const String COLOR_CORNFLOWER_BLUE = "#6495ED";
        public const String COLOR_FOREST_GREEN = "#228B22";
        public const String COLOR_COBALT = "#0047AB";
        public const String COLOR_INDIGO = "#4B0082";
        public const String COLOR_COTTON_CANDY = "#FFBCD9";
        public const String COLOR_CERISE = "#FF0066";
        public const String COLOR_JONQUIL = "FFCC00";
        public const String COLOR_PERIWINKLE = "#FF00FF";
        public const String COLOR_PEACH = "#FFE5B4";

        public const String COLOR_DARK_GRAY_WINDOW_BG = "#1F1F1F";
        public const String COLOR_DARK_GRAY_LIST_CONTROL_BG = "#252526";
        public const String COLOR_DARK_GRAY_TEXT_CONTROL_BG = "#333333";
        public const String COLOR_DARK_GRAY_INACTIVE_TAB = "#2E2E2E";
        public const String COLOR_DARK_GRAY_ACTIVE_TAB = "#3D3D3D";
        public const String COLOR_DARK_GRAY_BUTTON_FACE_ACTIVE = "#54545C";
        public const String COLOR_DARK_GRAY_BUTTON_FACE = "#3F3F46";
        public const String COLOR_DARK_GRAY_STATUS_BAR = "#424242";
        public const String COLOR_DARK_GRAY_TEXT_COLOR = "#1F1F1F";


        private readonly static List<String> SkipPalette = new List<String>()
        {
            COLOR_DARK_GRAY_WINDOW_BG,
            COLOR_DARK_GRAY_LIST_CONTROL_BG,
            COLOR_DARK_GRAY_TEXT_CONTROL_BG,
            COLOR_DARK_GRAY_INACTIVE_TAB,
            COLOR_DARK_GRAY_ACTIVE_TAB,
            COLOR_DARK_GRAY_BUTTON_FACE_ACTIVE,
            COLOR_DARK_GRAY_BUTTON_FACE,
            COLOR_DARK_GRAY_STATUS_BAR,
            COLOR_DARK_GRAY_TEXT_COLOR,
            COLOR_ERROR_RED,
        };

        private readonly static Dictionary<String, String> NamedColors = new Dictionary<String, String>()
        {
            { COLOR_LIGHT_ORANGE, "Light Orange" },
            { COLOR_LIGHT_GREEN, "Light Green" },
            { COLOR_LIGHT_BLUE, "Light Blue" },
            { COLOR_LIGHT_PURPLE, "Light Purple" },
            { COLOR_LIGHT_GREY, "Light Grey" },
            { COLOR_BLACK, "Black" },
            { COLOR_WHITE, "White" },
            { COLOR_GOLD, "Gold" },
            { COLOR_TANGERINE, "Tangerine" },
            { COLOR_PLUM, "Plum" },
            { COLOR_LAVENDAR_PINKISH, "Lavendar Pinkish" },
            { COLOR_MANGO, "Mango" },
            { COLOR_CRYOLA, "Cryola" },
            { COLOR_GRAPE_LIGHT_PINK, "Grape Light Pink" },
            { COLOR_NEON_GREEN, "Neon Green" },
            { COLOR_SKY_BLUE, "Sky Blue" },
            { COLOR_POWDER_BLUE, "Powder Blue" },
            { COLOR_TURQUOISE, "Turquoise" },
            { COLOR_PATRIARCH, "Patriarch" },
            { COLOR_CRIMSON, "Crimson" },
            { COLOR_ERROR_RED, "Error Red" },
            { COLOR_AQUAMARINE, "Aquamarine" },
            { COLOR_BABY_BLUE, "Baby Blue" },
            { COLOR_BABY_PINK, "Baby Pink" },
            { COLOR_BANANA_YELLOW, "Banana Yellow" },
            { COLOR_CORNFLOWER_BLUE, "Cornflower Blue" },
            { COLOR_FOREST_GREEN, "Forest Green" },
            { COLOR_COBALT, "Cobalt" },
            { COLOR_INDIGO, "Indigo" },
            { COLOR_COTTON_CANDY, "Cotton Candy" },
            { COLOR_CERISE, "Cerise" },
            { COLOR_JONQUIL, "Jonquil" },
            { COLOR_PERIWINKLE, "Periwinkle" },
            { COLOR_PEACH, "Peach" },
        };

        public static List<String> GetPalette()
        {
            return NamedColors.Keys.ToList().Where<string>(s => !SkipPalette.Contains(s)).ToList();
        }

        public static String GetColorName(String hexColor)
        {
            if (NamedColors.ContainsKey(hexColor))
            {
                return NamedColors[hexColor];
            }

            return hexColor;
        }

        public static Color ColorFromHSV(float hue, float saturation, float value)
        {
            int hi = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
            float f = (float) (hue / 60 - Math.Floor(hue / 60));

            value = value * 255;
            int v = Convert.ToInt32(value);
            int p = Convert.ToInt32(value * (1 - saturation));
            int q = Convert.ToInt32(value * (1 - f * saturation));
            int t = Convert.ToInt32(value * (1 - (1 - f) * saturation));

            if (hi == 0)
                return Color.FromArgb(255, v, t, p);
            else if (hi == 1)
                return Color.FromArgb(255, q, v, p);
            else if (hi == 2)
                return Color.FromArgb(255, p, v, t);
            else if (hi == 3)
                return Color.FromArgb(255, p, q, v);
            else if (hi == 4)
                return Color.FromArgb(255, t, p, v);
            else
                return Color.FromArgb(255, v, p, q);
        }

        public static void ColorToHSV(Color color, out float hue, out float saturation, out float value)
        {
            int max = Math.Max(color.R, Math.Max(color.G, color.B));
            int min = Math.Min(color.R, Math.Min(color.G, color.B));
            hue = color.GetHue();
            saturation = (max == 0) ? 0 : 1f - (1f * min / max);
            value = max / 255f;
        }

        public static String InvertColor(String hexColor)
        {
            if (hexColor.StartsWith("#"))
            {
                hexColor = hexColor.Substring(1);
            }
            if (hexColor.Length != 6)
            {
                throw new ArgumentException("Invalid hex color format. Expected format: RRGGBB");
            }
            // Parse the hex color components
            int r = Convert.ToInt32(hexColor.Substring(0, 2), 16);
            int g = Convert.ToInt32(hexColor.Substring(2, 2), 16);
            int b = Convert.ToInt32(hexColor.Substring(4, 2), 16);
            // Invert the colors
            r = 255 - r;
            g = 255 - g;
            b = 255 - b;
            // Return the inverted color in hex format
            return $"#{r:X2}{g:X2}{b:X2}";
        }
    }
}
