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

        private Image palette;

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

            _palette = Properties.Resources.palette.ToBitmap();
            palette = Image.FromHbitmap(_palette.GetHbitmap());

            ImageOperations.ResizeImage(ref palette, new Size(PictureBoxPalette.Width, PictureBoxPalette.Height), Color.Black, System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic);


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
                PictureBoxHoverColor.BackColor = palette.ToBitmap().GetPixel(e.X, e.Y);
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
    }
}
