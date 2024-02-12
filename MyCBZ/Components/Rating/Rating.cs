using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Win_CBZ.Components.Rating
{
    public partial class Rating : Control
    {

        public int Value { get; set; }

        public ImageList ImageList { get; set; }

        public String ImageKey { get; set; }

        public String HoverImageKey { get; set; }

        public String SelectedImageKey { get; set; }

        public bool CustomShapes { get; set; }

        
        public event EventHandler<RatingChangedEvent> RatingChanged;

        public Rating()
        {
            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);

            //pe.ClipRectangle.
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {


            base.OnMouseMove(e);
        }
    }
}
