using LoadingIndicator.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
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

            Task<Bitmap> paletteTask = Tasks.BitmapGenerationTask.CreatePaletteTask(PictureBoxPalette.Width, PictureBoxPalette.Height, new System.Threading.CancellationToken(), initialColor);

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
            PictureBoxSelectedColor.BackColor = PictureBoxHoverColor.BackColor;

            SelectedColor = PictureBoxSelectedColor.BackColor;

            TextBoxR.Text = SelectedColor.R.ToString();
            TextBoxG.Text = SelectedColor.G.ToString();
            TextBoxB.Text = SelectedColor.B.ToString();

            TextBoxHex.Text = HTMLColor.ToHexColor(SelectedColor);


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
            Task<Bitmap> paletteTask = Tasks.BitmapGenerationTask.CreatePaletteTask(PictureBoxPalette.Width, PictureBoxPalette.Height, new System.Threading.CancellationToken(), initialColor);

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

            PictureBoxSelectedColor.BackColor = PictureBoxHoverColor.BackColor;

            SelectedColor = PictureBoxSelectedColor.BackColor;

            TextBoxR.Text = SelectedColor.R.ToString();
            TextBoxG.Text = SelectedColor.G.ToString();
            TextBoxB.Text = SelectedColor.B.ToString();

            TextBoxHex.Text = HTMLColor.ToHexColor(SelectedColor);
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
    }
}
