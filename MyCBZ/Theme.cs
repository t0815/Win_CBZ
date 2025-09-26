using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Win_CBZ.Helper;

namespace Win_CBZ
{
    internal class Theme
    {

        public const String THEME_LIGHT = "Light";
        public const String THEME_DARK = "Dark";
        public const String THEME_SYSTEM = "System";


        public List<Tuple<string, string>> ThemeLightColors = new List<Tuple<string, string>>()
        {
            new Tuple<string, string>("AccentColor", Colors.COLOR_GOLD)
        };

        public List<Tuple<string, string>> ThemeDarkColors = new List<Tuple<string, string>>()
        {
            new Tuple<string, string>("AccentColor", Colors.COLOR_GOLD)
        };

        private static Theme Instance;

        
        /// <summary>
        /// make new theme colors
        /// </summary>
        /// <returns>Theme</returns>
        public Theme Make(string theme = THEME_SYSTEM)
        {

            switch (theme)
            {
                case THEME_LIGHT:
                    this.applyTheme(ThemeLightColors);
                    break;
                case THEME_DARK:
                    this.applyTheme(ThemeDarkColors);
                    break;
                case THEME_SYSTEM:
                    break;
                default:
                    this.applyTheme(ThemeLightColors);
                    break;
            }

            return this;
        }

        private void applyTheme(List<Tuple<string, string>> themeColors)
        {
            foreach (var color in themeColors)
            {
                switch (color.Item1)
                {
                    case "AccentColor":
                        AccentColor = System.Drawing.ColorTranslator.FromHtml(color.Item2);
                        break;
                }
            }
        }


        public static Theme GetInstance()
        {

            Theme.Instance ??= new Theme();

            return Theme.Instance;
        }

        private Theme()
        {
            this.applyTheme(ThemeLightColors);
        }

        public Color AccentColor { get; set; } = System.Drawing.ColorTranslator.FromHtml(Colors.COLOR_GOLD);

    }
}
