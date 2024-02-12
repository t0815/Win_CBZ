using ScintillaNET;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Win_CBZ.Components.Rating
{

    public partial class Rating : Control
    {

        private partial class StarShape : Shape
        {

            protected override Geometry DefiningGeometry
            {
                get
                {
                    PathGeometry geo = new PathGeometry();
                    var pathFigure = new PathFigure { StartPoint = Common.WpfPoint(center + new Point(-width, width)) };

                    pathFigure.Segments.Add(new System.Windows.Media.LineSegment(Common.WpfPoint(center), true));
                    pathFigure.Segments.Add(
                        new System.Windows.Media.LineSegment(Common.WpfPoint(center + new Point(width, width)), true));

                    pathFigure.IsFilled = true; 
                    

                    geo.Figures.Add(pathFigure);                    

                    geo.AddGeometry(new LineGeometry() { StartPoint = new Point(0, 0), EndPoint = new Point(3, 3) });
                    geo.AddGeometry(new LineGeometry() { StartPoint = new Point(3, 3), EndPoint = new Point(3, 0) });

                    Width

                    return geo;
                }
            }
        }


        public int Value { get; set; }

        public ImageList ImageList { get; set; }

        public String ImageKey { get; set; }

        public String HoverImageKey { get; set; }

        public String SelectedImageKey { get; set; }

        public bool CustomShapes { get; set; }

        protected Shape Shape { get; set; }

        
        public event EventHandler<RatingChangedEvent> RatingChanged;

        public Rating()
        {
            InitializeComponent();

            Shape = new StarShape();
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
