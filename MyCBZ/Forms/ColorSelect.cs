using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Win_CBZ.Forms
{
    public partial class ColorSelect : Form
    {

        private Color _selectedColor = Color.White;

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

        public ColorSelect(Color initialColor)
        {
            InitializeComponent();

            SelectedColor = initialColor;


        }
    }
}
