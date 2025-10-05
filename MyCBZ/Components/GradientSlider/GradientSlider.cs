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

namespace Win_CBZ.Components.GradientSlider
{

    [SupportedOSPlatform("windows")]
    [System.Drawing.ToolboxBitmap(typeof(GradientSlider), "Small Smiley.png")]
    public partial class GradientSlider : Control
    {
        public GradientSlider()
        {
            InitializeComponent();
        }

        public GradientSlider(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
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
        private int _thumbWidth;
        private int _thumbHeight;

        private int _thumbMargin;

        public event EventHandler<ValueChangedEventArgs> ValueChanged;
        public event EventHandler MinimumChanged;
        public event EventHandler MaximumChanged;
        public event EventHandler StartColorChanged;
        public event EventHandler EndColorChanged;
        public event EventHandler TrackHeightChanged;
        public event EventHandler TrackWidthChanged;
        public event EventHandler ThumbChanged;

        [DefaultValue(0.0f)]
        public virtual float Value
        {
            get { return _value; }
            set
            {
                if (this.Value != value)
                {
                    _value = value;

                    this.OnValueChanged(new ValueChangedEventArgs(value));
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

            for (int x = 0; x < this.Size.Width; x++)
            {
                // Factor (0.0 bis 1.0)
                float t = x / (float)(this.Size.Width - 1);

                int r = (int)(this.StartColor.R + (this.EndColor.R - this.StartColor.R) * t);
                int g = (int)(this.StartColor.G + (this.EndColor.G - this.StartColor.G) * t);
                int b = (int)(this.StartColor.B + (this.EndColor.B - this.StartColor.B) * t);
                int a = (int)(this.StartColor.A + (this.EndColor.A - this.StartColor.A) * t);

                Color pixelColor = Color.FromArgb(a, r, g, b);

                for (int y = 0; y < this.Size.Height; y++)
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

            // Call the DrawString method to write text.
            // Text, Font, and ClientRectangle are inherited properties.
            pe.Graphics.DrawImage(this.DrawGradient(), new PointF(0, 0));

            base.OnPaint(pe);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {


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
    }
}
