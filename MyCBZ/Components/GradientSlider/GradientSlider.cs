using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using Point = System.Drawing.Point;

namespace Win_CBZ.Components.GradientSlider
{

    [SupportedOSPlatform("windows")]
    [System.Drawing.ToolboxBitmap(typeof(GradientSlider), "Small Smiley.png")]
    public partial class GradientSlider : Control
    {
        public GradientSlider()
        {
            InitializeComponent();

            this.DoubleBuffered = true;
        }

        private float _value = 0f;
        private int _minimum = 0;
        private int _maximum = 100;
        private Color _startColor = Color.Black;
        private Color _endColor = Color.White;
        private int _trackHeight = 20;
        private int _trackWidth = 20;
        private Image _thumb;

        private Color _thumbColor;
        private int _thumbWidth = 1;
        private int _thumbHeight = 100;

        private int _thumbMargin;

        public event EventHandler<ValueChangedEventArgs> ValueChanged;
        public event EventHandler MinimumChanged;
        public event EventHandler MaximumChanged;
        public event EventHandler StartColorChanged;
        public event EventHandler EndColorChanged;
        public event EventHandler TrackHeightChanged;
        public event EventHandler TrackWidthChanged;
        public event EventHandler ThumbChanged;
        public event EventHandler ThumbWidthChanged;
        public event EventHandler ThumbHeightChanged;

        [DefaultValue(0.0f)]
        public virtual float Value
        {
            get { return _value; }
            set
            {
                if (this.Value != value)
                {
                    _value = value;

                    if (_value < this.Minimum)
                    {
                        _value = this.Minimum;
                    }

                    if (_value > this.Maximum)
                    {
                        _value = this.Maximum;
                    }

                    this.OnValueChanged(new ValueChangedEventArgs(_value));

                    this.Invalidate();
                }
            }
        }

        [DefaultValue(0)]
        public virtual int Minimum
        {
            get { return _minimum; }
            set
            {
                if (this.Minimum != value)
                {
                    _minimum = value;

                    this.OnMinimumChanged(new EventArgs());
                }
            }
        }

        [DefaultValue(100)]
        public virtual int Maximum
        {
            get { return _maximum; }
            set
            {
                if (this.Maximum != value)
                {
                    _maximum = value;

                    this.OnMaximumChanged(new EventArgs());
                }
            }
        }

        [Category("Appearance")]
        [DefaultValue(typeof(Color), "Black")]
        public virtual Color StartColor 
        {
            get { return _startColor; }
            set
            {
                if (this.StartColor != value)
                {
                    _startColor = value;

                    this.OnStartColorChanged(new EventArgs());

                    this.Invalidate();
                }
            }
        }

        [Category("Appearance")]
        [DefaultValue(typeof(Color), "White")]
        public virtual Color EndColor
        {
            get { return _endColor; }
            set
            {
                if (this.EndColor != value)
                {
                    _endColor = value;

                    this.OnEndColorChanged(new EventArgs());

                    this.Invalidate();
                }
            }
        }

        [DefaultValue(20)]
        public virtual int TrackHeight
        {
            get { return _trackHeight; }
            set
            {
                if (this.TrackHeight != value)
                {
                    _trackHeight = value;
                    this.OnTrackHeightChanged(new EventArgs());
                }
            }
        }

        [DefaultValue(20)]
        public virtual int TrackWidth
        {
            get { return _trackWidth; }
            set
            {
                if (this.TrackWidth != value)
                {
                    _trackWidth = value;
                    this.OnTrackWidthChanged(new EventArgs());
                }
            }
        }

        [Category("Appearance")]
        [DefaultValue(1)]
        public virtual int ThumbWidth
        {
            get { return _thumbWidth; }
            set
            {
                if (this.ThumbWidth != value)
                {
                    _thumbWidth = value;

                    if (_thumbWidth > this.Width)
                    {
                        _thumbWidth = this.Width / 2;
                    }

                    _thumbMargin = _thumbWidth / 2;

                    this.OnThumbWidthChanged(new EventArgs());

                    this.Invalidate();
                }
            }
        }

        [Category("Appearance")]
        [DefaultValue(100)]
        public virtual int ThumbHeight
        {
            get { return _thumbHeight; }
            set
            {
                if (this.ThumbHeight != value)
                {
                    _thumbHeight = value;

                    if (_thumbHeight > this.Height)
                    {
                        _thumbHeight = this.Height;
                    }

                    this.OnThumbHeightChanged(new EventArgs());

                    this.Invalidate();
                }
            }
        }

        [Category("Appearance")]
        [DefaultValue(null)]
        public virtual Image Thumb
        {
            get { return _thumb; }
            set
            {
                if (this.Thumb != value)
                {
                    _thumb = value;

                    if (_thumb != null)
                    {
                        _thumbWidth = _thumb.Width;
                        _thumbHeight = _thumb.Height;
                        _thumbMargin = _thumbWidth / 2;
                    }
                    else
                    {
                        _thumbWidth = 2;
                        _thumbHeight = this.Height;
                        _thumbMargin = 0;
                    }

                    this.OnThumbChanged(new EventArgs());

                    this.Invalidate();
                }
            }
        }

        private Lazy<Bitmap> GradientBitmap => new Lazy<Bitmap>(() => DrawGradient());

        public Color GetCurrentColor()
        {
            float ratio = (Value - Minimum) / (Maximum - Minimum);
            int r = (int)(StartColor.R + (EndColor.R - StartColor.R) * ratio);
            int g = (int)(StartColor.G + (EndColor.G - StartColor.G) * ratio);
            int b = (int)(StartColor.B + (EndColor.B - StartColor.B) * ratio);
            
            return Color.FromArgb(r, g, b);
        }

        private Bitmap DrawGradient()
        {
            
            Bitmap gradient = new Bitmap(this.Size.Width - (_thumbMargin * 2), this.Size.Height);

            for (int x = 0; x < gradient.Width - 1; x++)
            {
                // Factor (0.0 bis 1.0)
                float t = x / (float)(gradient.Width - 1);

                int r = (int)(this.StartColor.R + (this.EndColor.R - this.StartColor.R) * t);
                int g = (int)(this.StartColor.G + (this.EndColor.G - this.StartColor.G) * t);
                int b = (int)(this.StartColor.B + (this.EndColor.B - this.StartColor.B) * t);
                int a = (int)(this.StartColor.A + (this.EndColor.A - this.StartColor.A) * t);

                Color pixelColor = Color.FromArgb(a, r, g, b);

                for (int y = 0; y < gradient.Height; y++)
                {
                    gradient.SetPixel(x, y, pixelColor);
                }
            }

            return gradient;
        }

        protected virtual void OnValueChanged(ValueChangedEventArgs e)
        {
            ValueChanged?.Invoke(this, e);
        }

        protected virtual void OnMinimumChanged(EventArgs e)
        {
            MinimumChanged?.Invoke(this, e);
        }

        protected virtual void OnMaximumChanged(EventArgs e)
        {
            MinimumChanged?.Invoke(this, e);
        }

        protected virtual void OnStartColorChanged(EventArgs e)
        {
            StartColorChanged?.Invoke(this, e);
        }

        protected virtual void OnEndColorChanged(EventArgs e)
        {
            EndColorChanged?.Invoke(this, e);
        }

        protected virtual void OnTrackHeightChanged(EventArgs e)
        {
            TrackHeightChanged?.Invoke(this, e);
        }

        protected virtual void OnTrackWidthChanged(EventArgs e)
        {
            TrackWidthChanged?.Invoke(this, e);
        }

        protected virtual void OnThumbChanged(EventArgs e)
        {
            ThumbChanged?.Invoke(this, e);
        }

        protected virtual void OnThumbWidthChanged(EventArgs e)
        {
            ThumbWidthChanged?.Invoke(this, e);
        }

        protected virtual void OnThumbHeightChanged(EventArgs e)
        {
            ThumbHeightChanged?.Invoke(this, e);
        }

        protected override void OnResize(EventArgs e)
        {
            this.Invalidate();
            base.OnResize(e);
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            StringFormat style = new();

            

            // Create the brush and automatically dispose it.
            using SolidBrush foreBrush = new(ForeColor);

            Bitmap gradient = this.DrawGradient();

            // Call the DrawString method to write text.
            // Text, Font, and ClientRectangle are inherited properties.
            pe.Graphics.DrawImage(gradient, new PointF(_thumbMargin, 0));

            // Draw Thumb
            float ratio = (Value - Minimum) / (Maximum - Minimum);
            int thumbX = (int)(ratio * (this.Width - (_thumbMargin * 2))) + _thumbMargin - (_thumbWidth / 2);
            int thumbY = 0;

            try
            {
                Color currentBg = gradient.GetPixel(thumbX, thumbY);



                if (this.Thumb != null)
                {
                    pe.Graphics.DrawImage(this.Thumb, new Rectangle(thumbX, thumbY, _thumbWidth, _thumbHeight));
                }
                else
                {
                    using SolidBrush thumbBrush = new SolidBrush(this.ForeColor);
                                    
                    uint luminance = (uint)((0.299 * currentBg.R) + (0.587 * currentBg.G) + (0.114 * currentBg.B));
                    if (luminance > 186)
                    {
                        // Bright color, use black thumb
                        thumbBrush.Color = Color.Black;
                    }
                    else
                    {
                        // Dark color, use white thumb
                        thumbBrush.Color = Color.White;
                    }

                    

                    pe.Graphics.FillRectangle(thumbBrush, new Rectangle(thumbX, thumbY, _thumbWidth, _thumbHeight));
                    pe.Graphics.DrawLine(new Pen(thumbBrush), new Point(thumbX - 4 - _thumbWidth, _thumbHeight), new Point(thumbX + _thumbWidth + 4, _thumbHeight));
                    pe.Graphics.DrawLine(new Pen(thumbBrush), new Point(thumbX - 3 - _thumbWidth, _thumbHeight - 1), new Point(thumbX + _thumbWidth + 3, _thumbHeight - 1));
                    pe.Graphics.DrawLine(new Pen(thumbBrush), new Point(thumbX - 2 - _thumbWidth, _thumbHeight - 2), new Point(thumbX + _thumbWidth + 2, _thumbHeight - 2));
                    pe.Graphics.DrawLine(new Pen(thumbBrush), new Point(thumbX - 1 - _thumbWidth, _thumbHeight - 3), new Point(thumbX + _thumbWidth + 1, _thumbHeight - 3));
                    pe.Graphics.DrawLine(new Pen(thumbBrush), new Point(thumbX - _thumbWidth, _thumbHeight - 4), new Point(thumbX + _thumbWidth, _thumbHeight - 4));
                    //pe.Graphics.DrawRectangle(new Pen(thumbBrush), new Rectangle(thumbX - 2, _thumbHeight - 2, _thumbWidth + 6, 1));
                    //pe.Graphics.DrawRectangle(new Pen(thumbBrush), new Rectangle(thumbX - 1, _thumbHeight - 3, _thumbWidth + 5, 1));
                }
            } catch (Exception ex)
            {

            }

            base.OnPaint(pe);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                float ratio = e.X / (float)this.Width;
                this.Value = this.Minimum + ratio * (this.Maximum - this.Minimum);

                this.Invalidate();
            }

            base.OnMouseMove(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                float ratio = e.X / (float)this.Width;
                this.Value = this.Minimum + ratio * (this.Maximum - this.Minimum);
                
                this.Invalidate();
            }

            base.OnMouseDown(e);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            
            if (_thumbHeight > this.Height)
            {
                _thumbHeight = this.Height;
            }

            if (_thumbWidth > this.Width)
            {
                _thumbWidth = this.Width / 2;
                _thumbMargin = _thumbWidth / 2;
            }

            base.OnSizeChanged(e);
        }
    }
}
