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


        public Dictionary<string, string> ThemeLightColors = new Dictionary<string, string>()
        {
            { "AccentColor", Colors.COLOR_GOLD }
        };

        public Dictionary<string, string> ThemeDarkColors = new Dictionary<string, string>()
        {
            { "AccentColor", Colors.COLOR_GOLD }
        };

        private static Theme Instance;


        public Theme SetColor(string colorName, string colorValue)
        {
            switch (colorName)
            {
                case "AccentColor":
                    AccentColor = System.Drawing.ColorTranslator.FromHtml(colorValue);
                    break;
            }


            return this;
        }

        public Theme SetColorForTheme(string theme, string colorName, string colorValue)
        {
            switch (theme)
            {
                case THEME_LIGHT:
                    if (ThemeLightColors.ContainsKey(colorName))
                    {
                        ThemeLightColors[colorName] = colorValue;
                    }
                    else
                    {
                        ThemeLightColors.Add(colorName, colorValue);
                    }
                    break;
                case THEME_DARK:
                    if (ThemeDarkColors.ContainsKey(colorName))
                    {
                        ThemeDarkColors[colorName] = colorValue;
                    }
                    else
                    {
                        ThemeDarkColors.Add(colorName, colorValue);
                    }
                    break;
            }
            return this;
        }


        /// <summary>
        /// make new theme colors
        /// </summary>
        /// <returns>Theme</returns>
        public Theme Make(string theme = THEME_LIGHT)
        {

            switch (theme)
            {
                case THEME_LIGHT:
                    this.applyTheme(ThemeLightColors);
                    break;
                case THEME_DARK:
                    this.applyTheme(ThemeDarkColors);
                    break;
                default:
                    this.applyTheme(ThemeLightColors);
                    break;
            }

            return this;
        }

        private void applyTheme(Dictionary<string, string> themeColors)
        {
            foreach (var color in themeColors)
            {
                switch (color.Key)
                {
                    case "AccentColor":
                        AccentColor = System.Drawing.ColorTranslator.FromHtml(color.Value);
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

        public Color TextColor { get; set; } = System.Drawing.Color.Black;

        public Color BackgroundColorApp { get; set; } = SystemColors.Window;

        public Color BackgroundColorLists { get; set; } = System.Drawing.Color.White;

        public Color ButtonColor { get; set; } = System.Drawing.Color.LightGray;

        public Color ButtonTextColor { get; set; } = System.Drawing.Color.Black;

        public Color ButtonHoverColor { get; set; } = System.Drawing.Color.Gray;

        public Color ButtonHoverTextColor { get; set; } = System.Drawing.Color.White;

        public Color InputFieldColor { get; set; } = System.Drawing.Color.White;

        public Color InputFieldTextColor { get; set; } = System.Drawing.Color.Black;

        public Color InputFieldBorderColor { get; set; } = System.Drawing.Color.LightGray;

        public Color InputFieldBorderFocusColor { get; set; } = System.Drawing.Color.Gray;

        public Color InputFieldColorReadOnly { get; set; } = System.Drawing.Color.LightGray;
        
        public Color LinkColor { get; set; } = System.Drawing.Color.Blue;

        public Color MenuColor { get; set; } = System.Drawing.Color.LightGray;

        public Color MenuTextColor { get; set; } = System.Drawing.Color.Black;

        public Color MenuHoverColor { get; set; } = System.Drawing.Color.Gray;

        public Color MenuHoverTextColor { get; set; } = System.Drawing.Color.White;

        public Color StatusBarColor { get; set; } = System.Drawing.Color.LightGray;

        public Color StatusBarTextColor { get; set; } = System.Drawing.Color.Black;


    }
}
