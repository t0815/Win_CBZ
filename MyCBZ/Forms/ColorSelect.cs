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

        public ColorSelect(Color initialColor)
        {
            InitializeComponent();

            SelectedColor = initialColor;

            Theme.GetInstance().ApplyTheme(ColorEditorTableLayout.Controls);

            //_palette = Properties.Resources.palette.ToBitmap();

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

            paletteTask.Start();
            rainbowTask.Start();

            //ImageOperations.ResizeImage(ref palette, new Size(PictureBoxPalette.Width, PictureBoxPalette.Height), Color.Black, System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic);


            PictureBoxSelectedColor.BackColor = SelectedColor;

            TextBoxR.Text = SelectedColor.R.ToString();
            TextBoxG.Text = SelectedColor.G.ToString();
            TextBoxB.Text = SelectedColor.B.ToString();

            TextBoxHex.Text = HTMLColor.ToHexColor(SelectedColor);


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
        }

        private void UpdateSelectedColorState(Color color, bool dontRegeneratePaletteGradient = false)
        {
            Task<Bitmap> paletteTask;

            if (!dontRegeneratePaletteGradient)
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



            PictureBoxSelectedColor.BackColor = color;

            SelectedColor = color;

            TextBoxR.Text = SelectedColor.R.ToString();
            TextBoxG.Text = SelectedColor.G.ToString();
            TextBoxB.Text = SelectedColor.B.ToString();

            Color colorR_1 = Color.FromArgb(0, SelectedColor.G, SelectedColor.B);
            Color colorR_2 = Color.FromArgb(255, SelectedColor.G, SelectedColor.B);

            Color colorG_1 = Color.FromArgb(SelectedColor.R, 0, SelectedColor.B);
            Color colorG_2 = Color.FromArgb(SelectedColor.R, 255, SelectedColor.B);

            Color colorB_1 = Color.FromArgb(SelectedColor.R, SelectedColor.G, 0);
            Color colorB_2 = Color.FromArgb(SelectedColor.R, SelectedColor.G, 255);

            Task<Bitmap> paletteRTask = Tasks.BitmapGenerationTask.CreateHorizontalGradientTask(PictureBoxColorRangeR.Width, PictureBoxColorRangeR.Height, colorR_1, colorR_2, new System.Threading.CancellationToken());

            Task<Bitmap> paletteRFollow = paletteRTask.ContinueWith((t) =>
            {
                if (t.IsCompletedSuccessfully)
                {
                    this.Invoke(() =>
                    {
                        PictureBoxColorRangeR.Image?.Dispose();
                        PictureBoxColorRangeR.Image = Image.FromHbitmap(t.Result.GetHbitmap());
                        PictureBoxColorRangeR.Refresh();
                    });
                }

                return t.Result;
            });

            Task<Bitmap> paletteGTask = Tasks.BitmapGenerationTask.CreateHorizontalGradientTask(PictureBoxColorRangeG.Width, PictureBoxColorRangeG.Height, colorG_1, colorG_2, new System.Threading.CancellationToken());

            Task<Bitmap> paletteGFollow = paletteGTask.ContinueWith((t) =>
            {
                if (t.IsCompletedSuccessfully)
                {
                    this.Invoke(() =>
                    {
                        PictureBoxColorRangeG.Image?.Dispose();
                        PictureBoxColorRangeG.Image = Image.FromHbitmap(t.Result.GetHbitmap());
                        PictureBoxColorRangeG.Refresh();
                    });
                }

                return t.Result;
            });

            Task<Bitmap> paletteBTask = Tasks.BitmapGenerationTask.CreateHorizontalGradientTask(PictureBoxColorRangeB.Width, PictureBoxColorRangeB.Height, colorB_1, colorB_2, new System.Threading.CancellationToken());

            Task<Bitmap> paletteBFollow = paletteBTask.ContinueWith((t) =>
            {
                if (t.IsCompletedSuccessfully)
                {
                    this.Invoke(() =>
                    {
                        PictureBoxColorRangeB.Image?.Dispose();
                        PictureBoxColorRangeB.Image = Image.FromHbitmap(t.Result.GetHbitmap());
                        PictureBoxColorRangeB.Refresh();
                    });

                    
                }

                return t.Result;
            });

            paletteRTask.Start();
            paletteGTask.Start();
            paletteBTask.Start();

            TextBoxHex.Text = HTMLColor.ToHexColor(SelectedColor);
        }
    }
}
