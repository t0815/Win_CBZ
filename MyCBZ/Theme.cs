using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
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

        public Stream SaveXML(Dictionary<string, string> theme)
        {
            MemoryStream ms = new MemoryStream();
            XmlWriterSettings writerSettings = new XmlWriterSettings
            {
                OmitXmlDeclaration = false,
                Encoding = Encoding.UTF8,
            };

            XmlWriter xmlWriter = XmlWriter.Create(ms, writerSettings);

            xmlWriter.WriteStartDocument();

            xmlWriter.WriteStartElement("Win_CBZ_Theme");
            foreach (KeyValuePair<string, string> entry in theme)
            {
                
                 xmlWriter.WriteElementString(entry.Key, entry.Value);
               
            }

            xmlWriter.WriteEndElement();

            xmlWriter.WriteEndDocument();

            ms.Position = 0;

            return ms;
        }

        public void LoadXML(String xml, string theme)
        {

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
                    this.ApplyTheme(ThemeLightColors);
                    break;
                case THEME_DARK:
                    this.ApplyTheme(ThemeDarkColors);
                    break;
                default:
                    this.ApplyTheme(ThemeLightColors);
                    break;
            }

            return this;
        }

        private void ApplyTheme(Dictionary<string, string> themeColors)
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
            this.ApplyTheme(ThemeLightColors);
        }

        public Color AccentColor { get; set; } = System.Drawing.ColorTranslator.FromHtml(Colors.COLOR_GOLD);

        public Color TextColor { get; set; } = System.Drawing.Color.Black;

        public Color BackgroundColorApp { get; set; } = SystemColors.Window;

        public Color BackgroundColorLists { get; set; } = System.Drawing.Color.White;

        public Color ListItemInactiveSelectionColor { get; set; } = System.Drawing.Color.LightGray;

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

        public Color DialogHeaderColor { get; set; } = System.Drawing.Color.White;

        public Color DialogHeaderTextColor { get; set; } = System.Drawing.Color.Black;

        public Color DialogBackgroundColor { get; set; } = SystemColors.Control;

        public Color DialogTextColor { get; set; } = System.Drawing.Color.Black;

        public Color TabInactiveColor { get; set; } = System.Drawing.Color.LightGray;

        public Color TabActiveColor { get; set; } = System.Drawing.Color.Gray;

        public Color TabTextColor { get; set; } = System.Drawing.Color.Black;

        public Color TabTextActiveColor { get; set; } = System.Drawing.Color.White;

        public Color ComboTextColor { get; set; } = System.Drawing.Color.Black;

        public Color ComboBackgroundColor { get; set; } = System.Drawing.Color.White;

        public Color ComboBorderColor { get; set; } = System.Drawing.Color.LightGray;

        public Color DataGridBackgroundColor { get; set; } = System.Drawing.Color.Gray;

        public Color DataGridTextColor { get; set; } = System.Drawing.Color.Black;

        public Color DataGridHeaderBackgroundColor { get; set; } = System.Drawing.Color.White;

        public Color DataGridHeaderTextColor { get; set; } = System.Drawing.Color.Black;

        public Color DataGridCellColor { get; set; } = System.Drawing.Color.White;

        public Color DataGridCellAlternateColor { get; set; } = SystemColors.Control;

        public Color DataGridCellTextColor { get; set; } = System.Drawing.Color.Black;


    }
}
