using LoadingIndicator.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Win_CBZ.Img;

namespace Win_CBZ.Forms
{

    [SupportedOSPlatform("windows")]
    public partial class ColorSelect : Form
    {

        private Color _selectedColor = Color.White;

        private Bitmap _palette = null;
        private Bitmap _rb = null;

        private Image palette;
        private Image rainbow;

        private Color initialColor = Color.White;

        private Task<Bitmap> paletteTask;

        public Color SelectedColor
        {
            get
            {
                return _selectedColor;
            }
            set
            {
                _selectedColor = value;
            }
        }

        public string SelectedHexColor
        {
            get
            {
                return HTMLColor.ToHexColor(SelectedColor);
            }
        }

        public ColorSelect(Color color)
        {
            InitializeComponent();


            Theme.GetInstance().ApplyTheme(ColorEditorTableLayout.Controls);

            initialColor = Color.FromArgb(color.R, color.G, color.B);

            //_palette = Properties.Resources.palette.ToBitmap();

            /*
            Task<Bitmap> paletteTask = Tasks.BitmapGenerationTask.CreateThreeColorGradientTask(PictureBoxPalette.Width, PictureBoxPalette.Height, initialColor, new System.Threading.CancellationToken());

            paletteTask.ContinueWith((t) =>
            {
                if (t.IsCompletedSuccessfully)
                {
                    _palette = t.Result;
                    this.Invoke(() =>
                    {
                        PictureBoxPalette.Image = Image.FromHbitmap(_palette.GetHbitmap());
                    });
                }
            });
            */

            Task<Bitmap> rainbowTask = Tasks.BitmapGenerationTask.CreateRainbowTask(PictureBoxRainbow.Width, PictureBoxRainbow.Height, new System.Threading.CancellationToken());

            rainbowTask.ContinueWith((t) =>
            {
                if (t.IsCompletedSuccessfully)
                {
                    _rb = t.Result;
                    this.Invoke(() =>
                    {
                        PictureBoxRainbow.Image = Image.FromHbitmap(_rb.GetHbitmap());
                    });
                }
            });

            //paletteTask.Start();
            rainbowTask.Start();

            //ImageOperations.ResizeImage(ref palette, new Size(PictureBoxPalette.Width, PictureBoxPalette.Height), Color.Black, System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic);

        }

        private void PictureBoxPalette_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                PictureBoxHoverColor.BackColor = _palette.GetPixel(e.X, e.Y);
            }
            catch
            {

            }
        }

        private void PictureBoxPalette_Click(object sender, EventArgs e)
        {

            UpdateSelectedColorState(PictureBoxHoverColor.BackColor, true);
        }

        private void PictureBoxPalette_Resize(object sender, EventArgs e)
        {
            if (palette != null)
            {
                palette.Dispose();

                palette = Image.FromHbitmap(_palette.GetHbitmap());

                ImageOperations.ResizeImage(ref palette, new Size(PictureBoxPalette.Width, PictureBoxPalette.Height), Color.Black, System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic);
            }
        }

        private void PictureBoxRainbow_MouseClick(object sender, MouseEventArgs e)
        {

            UpdateSelectedColorState(PictureBoxHoverColor.BackColor);
        }

        private void PictureBoxRainbow_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                PictureBoxHoverColor.BackColor = initialColor = _rb.GetPixel(e.X, e.Y);
            }
            catch
            {

            }
        }

        private void PictureBoxRainbow_Resize(object sender, EventArgs e)
        {
            if (rainbow != null)
            {
                rainbow.Dispose();

                rainbow = Image.FromHbitmap(_rb.GetHbitmap());

                ImageOperations.ResizeImage(ref rainbow, new Size(PictureBoxRainbow.Width, PictureBoxRainbow.Height), Color.Black, System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic);
            }
        }

        private void PalettesTabControl_Selecting(object sender, TabControlCancelEventArgs e)
        {
            String tab = e.TabPage.Name;

            switch (tab)
            {
                case "TabPageDefaultPalette":
                    LoadPaletteForTabHtml(FlowLayoutDefaultPalette, Colors.GetPalette());
                    break;

                case "TabPageSystemPalette":

                    Dictionary<string, Color> colours = typeof(SystemColors)
                        .GetRuntimeProperties()
                        .Select(c => new
                        {
                            Color = (Color)c.GetValue(null),
                            Name = c.Name
                        }).ToDictionary(x => x.Name, x => x.Color);

                    LoadPaletteForTab(FlowLayoutSystemPalette, colours.Values.ToList());
                    break;
                default: break;
            }
        }

        private void PaletteColorSwatch_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;

            //PictureBoxSelectedColor.BackColor = button.BackColor;

            UpdateSelectedColorState(button.BackColor);
        }

        private void PictureBoxRGBSlider_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                PictureBox pictureBox = sender as PictureBox;

                PictureBoxHoverColor.BackColor = pictureBox.Image.ToBitmap().GetPixel(e.X, e.Y);

                if (e.Button == MouseButtons.Left)
                {
                    UpdateSelectedColorState(PictureBoxHoverColor.BackColor);
                }
            }
            catch
            {

            }
        }

        private void PictureBoxRGBSlider_MouseClick(object sender, EventArgs e)
        {
            try
            {
                PictureBox pictureBox = sender as PictureBox;

                UpdateSelectedColorState(PictureBoxHoverColor.BackColor);
            }
            catch
            {

            }
        }

        private void LoadPaletteForTabHtml(FlowLayoutPanel container, List<string> palette)
        {
            Task.Factory.StartNew(() =>
            {
                Invoke(() =>
                {
                    container.Controls.Clear();

                    foreach (string example in palette)
                    {
                        if (example != null)
                        {
                            Button newExample = new Button();
                            newExample.FlatStyle = FlatStyle.Flat;
                            newExample.FlatAppearance.BorderColor = Color.Black;
                            newExample.FlatAppearance.BorderSize = 1;

                            newExample.BackColor = HTMLColor.ToColor(example);
                            newExample.Width = 20;
                            newExample.Height = 20;
                            newExample.Margin = new Padding(10, 8, 10, 8);
                            newExample.Cursor = Cursors.Hand;
                            //newExample.
                            newExample.Click += PaletteColorSwatch_Click;

                            ColorSelectTooltip.SetToolTip(newExample, Colors.GetColorName(example));

                            container.Controls.Add(newExample);

                        }
                    }
                });
            });
        }

        private void LoadPaletteForTab(FlowLayoutPanel container, List<Color> palette)
        {
            Task.Factory.StartNew(() =>
            {
                Invoke(() =>
                {
                    container.Controls.Clear();

                    foreach (Color example in palette)
                    {

                        Button newExample = new Button();
                        newExample.FlatStyle = FlatStyle.Flat;
                        newExample.FlatAppearance.BorderColor = Color.Black;
                        newExample.FlatAppearance.BorderSize = 1;

                        newExample.BackColor = example;
                        newExample.Width = 20;
                        newExample.Height = 20;
                        newExample.Margin = new Padding(10, 8, 10, 8);
                        newExample.Cursor = Cursors.Hand;
                        //newExample.
                        newExample.Click += PaletteColorSwatch_Click;

                        ColorSelectTooltip.SetToolTip(newExample, Colors.GetColorName(HTMLColor.ToHexColor(example)));

                        container.Controls.Add(newExample);

                    }
                });
            });
        }

        private void ColorSelect_Load(object sender, EventArgs e)
        {
            LoadPaletteForTabHtml(FlowLayoutDefaultPalette, Colors.GetPalette());
            UpdateSelectedColorState(initialColor);

            GradientSliderChannelR.Value = initialColor.R;
            GradientSliderChannelG.Value = initialColor.G;
            GradientSliderChannelB.Value = initialColor.B;
        }

        private void UpdateSelectedColorState(Color color, bool dontRegeneratePaletteGradient = false)
        {


            if (!dontRegeneratePaletteGradient)
            {
                if (paletteTask == null || paletteTask.IsCompleted)
                {

                    paletteTask = Tasks.BitmapGenerationTask.CreateThreeColorGradientTask(PictureBoxPalette.Width, PictureBoxPalette.Height, color, new System.Threading.CancellationToken());

                    paletteTask.ContinueWith((t) =>
                    {
                        if (t.IsCompletedSuccessfully)
                        {
                            _palette = t.Result;
                            this.Invoke(() =>
                            {
                                PictureBoxPalette.Image.Dispose();
                                PictureBoxPalette.Image = Image.FromHbitmap(_palette.GetHbitmap());
                                PictureBoxPalette.Refresh();
                            });
                        }
                    });

                    paletteTask.Start();
                }
            }



            PictureBoxSelectedColor.BackColor = color;

            SelectedColor = color;


            GradientSliderChannelR.ValueChanged -= GradientSliderChannelR_ValueChanged;
            GradientSliderChannelG.ValueChanged -= GradientSliderChannelG_ValueChanged;
            GradientSliderChannelB.ValueChanged -= GradientSliderChannelB_ValueChanged;

            GradientSliderChannelR.Value = SelectedColor.R;
            GradientSliderChannelG.Value = SelectedColor.G;
            GradientSliderChannelB.Value = SelectedColor.B;

            GradientSliderChannelR.ValueChanged += GradientSliderChannelR_ValueChanged;
            GradientSliderChannelG.ValueChanged += GradientSliderChannelG_ValueChanged;
            GradientSliderChannelB.ValueChanged += GradientSliderChannelB_ValueChanged;


            Color colorR_1 = Color.FromArgb(0, SelectedColor.G, SelectedColor.B);
            Color colorR_2 = Color.FromArgb(255, SelectedColor.G, SelectedColor.B);

            Color colorG_1 = Color.FromArgb(SelectedColor.R, 0, SelectedColor.B);
            Color colorG_2 = Color.FromArgb(SelectedColor.R, 255, SelectedColor.B);

            Color colorB_1 = Color.FromArgb(SelectedColor.R, SelectedColor.G, 0);
            Color colorB_2 = Color.FromArgb(SelectedColor.R, SelectedColor.G, 255);


            GradientSliderChannelR.StartColor = colorR_1;
            GradientSliderChannelR.EndColor = colorR_2;

            GradientSliderChannelG.StartColor = colorG_1;
            GradientSliderChannelG.EndColor = colorG_2;

            GradientSliderChannelB.StartColor = colorB_1;
            GradientSliderChannelB.EndColor = colorB_2;

            TextBoxR.TextChanged -= TextBoxR_TextChanged;
            TextBoxG.TextChanged -= TextBoxG_TextChanged;
            TextBoxB.TextChanged -= TextBoxB_TextChanged;
            TextBoxHex.TextChanged -= TextBoxHex_TextChanged;

            TextBoxR.Text = SelectedColor.R.ToString();
            TextBoxG.Text = SelectedColor.G.ToString();
            TextBoxB.Text = SelectedColor.B.ToString();

            TextBoxHex.Text = HTMLColor.ToHexColor(SelectedColor);

            TextBoxR.TextChanged += TextBoxR_TextChanged;
            TextBoxG.TextChanged += TextBoxG_TextChanged;
            TextBoxB.TextChanged += TextBoxB_TextChanged;
            TextBoxHex.TextChanged += TextBoxHex_TextChanged;

            float h, s, v;

            Colors.ColorToHSV(SelectedColor, out h, out s, out v);

            TextBoxH.Text = ((int)h).ToString();
            TextBoxS.Text = ((int)(s * 100)).ToString();
            TextBoxV.Text = ((int)(v * 100)).ToString();
        }

        private void GradientSliderChannelR_ValueChanged(object sender, Win_CBZ.Components.GradientSlider.ValueChangedEventArgs e)
        {
            Color newColor = Color.FromArgb((byte)e.NewValue, SelectedColor.G, SelectedColor.B);

            UpdateSelectedColorState(newColor);
        }

        private void GradientSliderChannelG_ValueChanged(object sender, Win_CBZ.Components.GradientSlider.ValueChangedEventArgs e)
        {
            Color newColor = Color.FromArgb(SelectedColor.R, (byte)e.NewValue, SelectedColor.B);

            UpdateSelectedColorState(newColor);
        }

        private void GradientSliderChannelB_ValueChanged(object sender, Win_CBZ.Components.GradientSlider.ValueChangedEventArgs e)
        {
            Color newColor = Color.FromArgb(SelectedColor.R, SelectedColor.G, (byte)e.NewValue);

            UpdateSelectedColorState(newColor);
        }

        private void TextBoxR_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int r = int.Parse(TextBoxR.Text);

                Color newColor = Color.FromArgb((byte)r, SelectedColor.G, SelectedColor.B);

                UpdateSelectedColorState(newColor);
            } catch
            {
                return;
            }

            
        }

        private void TextBoxG_TextChanged(object sender, EventArgs e)
        {
            try { 
                int g = int.Parse(TextBoxG.Text);

                Color newColor = Color.FromArgb(SelectedColor.R, (byte)g, SelectedColor.B);

                UpdateSelectedColorState(newColor);
            } catch
            {
                return;
            }

            
        }

        private void TextBoxB_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int b = int.Parse(TextBoxB.Text);

                Color newColor = Color.FromArgb(SelectedColor.R, SelectedColor.G, (byte)b);
                UpdateSelectedColorState(newColor);
            }
            catch
            {
                return;
            }

            
        }

        private void TextBoxHex_TextChanged(object sender, EventArgs e)
        {
            try
            {
                UpdateSelectedColorState(HTMLColor.ToColor(TextBoxHex.Text));
            }
            catch
            {
            }
        }
    }
}
