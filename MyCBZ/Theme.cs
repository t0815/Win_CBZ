using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using Win_CBZ.Extensions;
using Win_CBZ.Helper;

namespace Win_CBZ
{

    [SupportedOSPlatform("windows")]
    internal class Theme
    {

        public const String THEME_LIGHT = "Light";
        public const String THEME_DARK = "Dark";
        public const String THEME_SYSTEM = "System";

        public const String COLOR_NAME_ACCENT = "AccentColor";
        public const String COLOR_NAME_LIST_BACKGROUND = "ListBackgroundColor";
        public const String COLOR_NAME_TEXT = "TextColor";
        public const String COLOR_NAME_BUTTON = "ButtonColor";
        public const String COLOR_NAME_BUTTON_TEXT = "ButtonTextColor";
        public const String COLOR_NAME_WINDOW_BACKGROUND = "WindowColor";
        public const String COLOR_NAME_INPUT_FIELD = "InputFieldColor";
        public const String COLOR_NAME_DIALOG_HEADER_BACKGROUND = "DialogHeaderBackground";
        public const String COLOR_NAME_TAB_BACKGROUND = "TabBackgroundColor";
        public const String COLOR_NAME_BUTTON_HIGHLIGHT = "ButtonHighlightColor";
        public const String COLOR_NAME_BUTTON_BORDER = "ButtonBorderColor";


        public Dictionary<string, string> ThemeLightColors = new Dictionary<string, string>()
        {
            { COLOR_NAME_ACCENT, Colors.COLOR_GOLD },
            { COLOR_NAME_LIST_BACKGROUND, "#ffffff" },
            { COLOR_NAME_TEXT, "#000000" },
            { COLOR_NAME_BUTTON, "#d3d3d3" },
            { COLOR_NAME_WINDOW_BACKGROUND, "#f0efed" },
            { COLOR_NAME_INPUT_FIELD, "#ffffff" },
            { COLOR_NAME_DIALOG_HEADER_BACKGROUND, "#ffffff" },
            { COLOR_NAME_TAB_BACKGROUND, "#ffffff" },
            { COLOR_NAME_BUTTON_HIGHLIGHT, "#a9a9a9" },
            { COLOR_NAME_BUTTON_BORDER, "#808080" },
            { COLOR_NAME_BUTTON_TEXT, "#000000" },  
        };

        public Dictionary<string, string> ThemeDarkColors = new Dictionary<string, string>()
        {
            { "AccentColor", Colors.COLOR_GOLD },

        };

        private static Theme Instance;


        public Theme SetColorHex(string colorName, string colorValue)
        {
            if (!colorValue.StartsWith("#"))
            {


                return this;
            }

            switch (colorName)
            {
                case COLOR_NAME_ACCENT:
                    AccentColor = System.Drawing.ColorTranslator.FromHtml(colorValue);
                    break;

                case COLOR_NAME_BUTTON:
                    ButtonColor = System.Drawing.ColorTranslator.FromHtml(colorValue);
                    break;

                case COLOR_NAME_BUTTON_HIGHLIGHT:
                    ButtonHoverColor = System.Drawing.ColorTranslator.FromHtml(colorValue);
                    break;

                case COLOR_NAME_BUTTON_BORDER:
                    ButtonBorderColor = System.Drawing.ColorTranslator.FromHtml(colorValue);
                    break;

                case COLOR_NAME_BUTTON_TEXT:
                    ButtonTextColor = System.Drawing.ColorTranslator.FromHtml(colorValue);
                    break;

                case COLOR_NAME_LIST_BACKGROUND:
                    ListBackgroundColor = System.Drawing.ColorTranslator.FromHtml(colorValue);
                    break;

                case COLOR_NAME_TEXT:
                    TextColor = System.Drawing.ColorTranslator.FromHtml(colorValue);
                    break;

                case COLOR_NAME_WINDOW_BACKGROUND:
                    BackgroundColorApp = System.Drawing.ColorTranslator.FromHtml(colorValue);
                    break;

                case COLOR_NAME_INPUT_FIELD:
                    InputFieldColor = System.Drawing.ColorTranslator.FromHtml(colorValue);
                    break;

                case COLOR_NAME_DIALOG_HEADER_BACKGROUND:
                    DialogHeaderColor = System.Drawing.ColorTranslator.FromHtml(colorValue);
                    break;

                case COLOR_NAME_TAB_BACKGROUND:
                    TabContainerColor = System.Drawing.ColorTranslator.FromHtml(colorValue);
                    break;

            }


            return this;
        }

        public String GetColorHex(string colorName)
        {
            switch (colorName)
            {
                case COLOR_NAME_ACCENT:
                    return HTMLColor.ToHexColor(AccentColor);

                case COLOR_NAME_BUTTON:
                    return HTMLColor.ToHexColor(ButtonColor);

                case COLOR_NAME_BUTTON_HIGHLIGHT:
                    return HTMLColor.ToHexColor(ButtonHoverColor);

                case COLOR_NAME_BUTTON_BORDER:
                    return HTMLColor.ToHexColor(ButtonBorderColor);

               case COLOR_NAME_BUTTON_TEXT:
                    return HTMLColor.ToHexColor(ButtonTextColor);


                case COLOR_NAME_LIST_BACKGROUND:
                    return HTMLColor.ToHexColor(ListBackgroundColor);

                case COLOR_NAME_TEXT:
                    return HTMLColor.ToHexColor(TextColor);

                case COLOR_NAME_WINDOW_BACKGROUND:
                    return HTMLColor.ToHexColor(BackgroundColorApp);

                case COLOR_NAME_INPUT_FIELD:
                    return HTMLColor.ToHexColor(InputFieldColor);

                case COLOR_NAME_DIALOG_HEADER_BACKGROUND:
                    return HTMLColor.ToHexColor(DialogHeaderColor);

                case COLOR_NAME_TAB_BACKGROUND:
                    return HTMLColor.ToHexColor(TabContainerColor);

            }


            return "#000000";
        }

        public Theme SetColor(string colorName, Color colorValue)
        {
            switch (colorName)
            {
                case COLOR_NAME_ACCENT:
                    AccentColor = colorValue;
                    break;
                case COLOR_NAME_BUTTON:
                    ButtonColor = colorValue;
                    break;
                case COLOR_NAME_BUTTON_HIGHLIGHT:
                    ButtonHoverColor = colorValue;
                    break;
                case COLOR_NAME_BUTTON_BORDER:
                    ButtonBorderColor = colorValue;
                    break;
                case COLOR_NAME_BUTTON_TEXT:
                     ButtonTextColor = colorValue;
                     break;
                case COLOR_NAME_LIST_BACKGROUND:
                    ListBackgroundColor = colorValue;
                    break;
                case COLOR_NAME_TEXT:
                    TextColor = colorValue;
                    break;
                case COLOR_NAME_WINDOW_BACKGROUND:
                    BackgroundColorApp = colorValue;
                    break;
                case COLOR_NAME_INPUT_FIELD:
                    InputFieldColor = colorValue;
                    break;
                case COLOR_NAME_DIALOG_HEADER_BACKGROUND:
                    DialogHeaderColor = colorValue;
                    break;

                case COLOR_NAME_TAB_BACKGROUND:
                    TabContainerColor = colorValue;
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
                    case COLOR_NAME_ACCENT:
                        AccentColor = System.Drawing.ColorTranslator.FromHtml(color.Value);
                        break;

                    case COLOR_NAME_LIST_BACKGROUND:
                        ListBackgroundColor = System.Drawing.ColorTranslator.FromHtml(color.Value);
                        break;

                    case COLOR_NAME_TEXT:
                        TextColor = System.Drawing.ColorTranslator.FromHtml(color.Value);
                        break;
                    
                    case COLOR_NAME_BUTTON:
                        ButtonColor = System.Drawing.ColorTranslator.FromHtml(color.Value);
                        break;

                    case COLOR_NAME_BUTTON_HIGHLIGHT:
                        ButtonHoverColor = System.Drawing.ColorTranslator.FromHtml(color.Value);
                        break;

                    case COLOR_NAME_BUTTON_BORDER:
                        ButtonBorderColor = System.Drawing.ColorTranslator.FromHtml(color.Value);
                        break;
                    case COLOR_NAME_BUTTON_TEXT:
                        ButtonTextColor = System.Drawing.ColorTranslator.FromHtml(color.Value);
                        break;

                    case COLOR_NAME_WINDOW_BACKGROUND:
                        BackgroundColorApp = System.Drawing.ColorTranslator.FromHtml(color.Value);
                        break;

                    case COLOR_NAME_INPUT_FIELD:
                        InputFieldColor = System.Drawing.ColorTranslator.FromHtml(color.Value);
                        break;
                    
                    case COLOR_NAME_DIALOG_HEADER_BACKGROUND:
                        DialogHeaderColor = System.Drawing.ColorTranslator.FromHtml(color.Value);
                        break;
                    
                    case COLOR_NAME_TAB_BACKGROUND:
                        TabContainerColor = System.Drawing.ColorTranslator.FromHtml(color.Value);
                        break;

                }
            }
        }


        public static Theme GetInstance()
        {

            Theme.Instance ??= new Theme();

            return Theme.Instance;
        }

        public Theme Copy()
        {
            Theme theme = new Theme();
            theme.AccentColor = this.AccentColor;
            theme.TextColor = this.TextColor;
            theme.BackgroundColorApp = this.BackgroundColorApp;
            theme.ListBackgroundColor = this.ListBackgroundColor;
            theme.ButtonColor = this.ButtonColor;
            theme.ButtonTextColor = this.ButtonTextColor;
            theme.ButtonHoverColor = this.ButtonHoverColor;
            theme.ButtonHoverTextColor = this.ButtonHoverTextColor;
            theme.ButtonBorderColor = this.ButtonBorderColor;
            theme.ButtonTextColor = this.ButtonTextColor;
            theme.InputFieldColor = this.InputFieldColor;
            theme.InputFieldTextColor = this.InputFieldTextColor;
            theme.InputFieldBorderColor = this.InputFieldBorderColor;
            theme.InputFieldBorderFocusColor = this.InputFieldBorderFocusColor;
            theme.InputFieldColorReadOnly = this.InputFieldColorReadOnly;
            theme.LinkColor = this.LinkColor;
            theme.MenuColor = this.MenuColor;
            theme.MenuTextColor = this.MenuTextColor;
            theme.MenuHoverColor = this.MenuHoverColor;
            theme.MenuHoverTextColor = this.MenuHoverTextColor;
            theme.StatusBarColor = this.StatusBarColor;
            theme.StatusBarTextColor = this.StatusBarTextColor;
            theme.DialogHeaderColor = this.DialogHeaderColor;
            theme.DialogHeaderTextColor = this.DialogHeaderTextColor;
            theme.DialogBackgroundColor = this.DialogBackgroundColor;
            theme.DialogTextColor = this.DialogTextColor;
            theme.TabContainerColor = this.TabContainerColor;
            theme.TabInactiveColor = this.TabInactiveColor;
            theme.TabActiveColor = this.TabActiveColor;
            theme.TabTextActiveColor = this.TabTextActiveColor;
            theme.TabTextActiveColor = this.TabTextActiveColor;
            return theme;
        }

        private Theme()
        {
            this.ApplyTheme(ThemeLightColors);
        }

        public Color AccentColor { get; set; } = System.Drawing.ColorTranslator.FromHtml(Colors.COLOR_GOLD);

        public Color TextColor { get; set; } = System.Drawing.Color.Black;

        public Color BackgroundColorApp { get; set; } = SystemColors.Window;

        public Color ListBackgroundColor { get; set; } = SystemColors.Window;

        public Color ListItemInactiveSelectionColor { get; set; } = System.Drawing.Color.LightGray;

        public Color ButtonColor { get; set; } = System.Drawing.Color.LightGray;

        public Color ButtonTextColor { get; set; } = System.Drawing.Color.Black;

        public Color ButtonHoverColor { get; set; } = System.Drawing.Color.Gray;

        public Color ButtonHoverTextColor { get; set; } = System.Drawing.Color.White;

        public Color ButtonBorderColor { get; set; } = System.Drawing.Color.DarkGray;

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

        public Color TabContainerColor { get; set; } = System.Drawing.Color.White;

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

        public Theme ApplyThemeGeneral(Form form)
        {

            //form.BackColor = BackgroundColorApp;
            //form.ForeColor = TextColor;
            //form.Invalidate();

            return this;
        }

        public Theme ApplyThemeDataGid(DataGridView dg)
        {
            dg.DefaultCellStyle.SelectionBackColor = AccentColor;
            dg.RowsDefaultCellStyle.SelectionBackColor = AccentColor;

            return this;
        }

        public Theme ApplyThemeTab(TabControl tabControl)
        {
            //tabControl.BackColor = BackgroundColorApp;
            //tabControl.ForeColor = TextColor;
            foreach (TabPage tab in tabControl.TabPages)
            {
                //tab.BackColor = Color.Transparent;
                //tab.ForeColor = TextColor;

                ApplyThemeContainer(tab.Container);
            }

            return this;
        }

        public Theme ApplyTheme(TableLayoutControlCollection container)
        {
            //Theme theme = Theme.GetInstance();

            container.OfType<Button>().Each(b =>
            {
                b.BackColor = ButtonColor;
                b.ForeColor = ButtonTextColor;
                b.FlatAppearance.MouseOverBackColor = ButtonHoverColor;
                b.FlatAppearance.BorderColor = ButtonBorderColor;

                //b.FlatAppearance.MouseOverBackColor = HTMLColor.AdjustBrightness(HTMLColor.ToColor(ButtonColor), 0.8f);
                //b.FlatAppearance.MouseDownBackColor = HTMLColor.AdjustBrightness(HTMLColor.ToColor(ButtonColor), 0.6f);
            });

            container.OfType<Label>().Each<Label>(l =>
            {
                //l.ForeColor = TextColor;
            });

            container.OfType<CheckBox>().Each<CheckBox>(c =>
            {
                //c.ForeColor = TextColor;
                //c.BackColor = Color.Transparent;
            });

            container.OfType<RadioButton>().Each<RadioButton>(r =>
            {
                //r.ForeColor = TextColor;
                //r.BackColor = Color.Transparent; ;
            });

            container.OfType<ComboBox>().Each<ComboBox>(cb =>
            {
                //cb.BackColor = InputFieldColor;
                //cb.ForeColor = TextColor;
            });

            container.OfType<TextBox>().Each<TextBox>(tb =>
            {
                //tb.BackColor = InputFieldColor;
                //tb.ForeColor = TextColor;
            });

            container.OfType<ListBox>().Each<ListBox>(lb =>
            {
                //lb.BackColor = ListBackgroundColor;
                //lb.ForeColor = TextColor;
               
            });

            container.OfType<ExtendetListView>().Each<ExtendetListView>(lv =>
            {
                //lv.BackColor = ListBackgroundColor;
                //lv.ForeColor = TextColor;
                lv.SelectionColor = AccentColor;
            });


            //Control.ControlCollection controls = container.OfType<Control>().SelectMany(c => c.Controls.OfType<Control>());

            return this;
        }

        public Theme ApplyThemeContainer(IContainer container)
        {
            container?.Components.OfType<Button>().Each(b =>
            {
                b.BackColor = ButtonColor;
                b.ForeColor = ButtonTextColor;
                b.FlatAppearance.MouseOverBackColor = ButtonHoverColor;
                b.FlatAppearance.BorderColor = ButtonBorderColor;
                //b.FlatAppearance.MouseOverBackColor = HTMLColor.AdjustBrightness(HTMLColor.ToColor(ButtonColor), 0.8f);
                //b.FlatAppearance.MouseDownBackColor = HTMLColor.AdjustBrightness(HTMLColor.ToColor(ButtonColor), 0.6f);
            });

            container?.Components.OfType<Label>().Each<Label>(l =>
            {
                //l.ForeColor = TextColor;
            });

            container?.Components.OfType<CheckBox>().Each<CheckBox>(c =>
            {
                //c.ForeColor = TextColor;
                //c.BackColor = Color.Transparent; ;
            });

            container?.Components.OfType<RadioButton>().Each<RadioButton>(r =>
            {
                //r.ForeColor = TextColor;
                //r.BackColor = Color.Transparent; ;
            });

            container?.Components.OfType<ComboBox>().Each<ComboBox>(cb =>
            {
                //cb.BackColor = InputFieldColor;
                //cb.ForeColor = TextColor;
                //cb.FlatStyle = FlatStyle.Flat;
               
            });

            container?.Components.OfType<TextBox>().Each<TextBox>(tb =>
            {
                //tb.BackColor = InputFieldColor;
                //tb.ForeColor = TextColor;
            });

            container?.Components.OfType<ListBox>().Each<ListBox>(lb =>
            {
                //lb.BackColor = ListBackgroundColor;
                //lb.ForeColor = TextColor;

            });

            container?.Components.OfType<ExtendetListView>().Each<ExtendetListView>(lv =>
            {
                //lv.BackColor = ListBackgroundColor;
                //lv.ForeColor = TextColor;
                lv.SelectionColor = AccentColor;
            });

            return this;
        }
    }
}
