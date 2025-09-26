using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Shapes;
using Win_CBZ.Img;
using Rectangle = System.Drawing.Rectangle;

namespace Win_CBZ
{
    [SupportedOSPlatform("windows")]
    public class ExtendetListView : System.Windows.Forms.ListView
    {

        #region Constants

        private const int WM_PAINT = 0xF;

        #endregion

        #region Instance Fields

        private bool _allowItemDrag;

        private Color _insertionLineColor;

        private Color _selectionColor;

        private Color _selectedTextColor;

        private Image _chechBoxIcon;

        private Image _chechBoxIconCheck;

        #endregion

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ListView"/> class.
        /// </summary>
        public ExtendetListView() : base()
        {
            this.DoubleBuffered = true;
            this.InsertionLineColor = Color.Red;
            this.SelectionColor = SystemColors.Highlight;
            this.SelectionTextColor = Color.Black;
            this.InsertionIndex = -1;
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when the AllowItemDrag property value changes.
        /// </summary>
        [Category("Property Changed")]
        public event EventHandler AllowItemDragChanged;

        /// <summary>
        /// Occurs when the InsertionLineColor property value changes.
        /// </summary>
        [Category("Property Changed")]
        public event EventHandler InsertionLineColorChanged;

        /// <summary>
        /// Occurs when the SelectionColor property value changes.
        /// </summary>
        [Category("Property Changed")]
        public event EventHandler SelectionColorChanged;

        /// <summary>
        /// Occurs when the SelectionTextColor property value changes.
        /// </summary>
        [Category("Property Changed")]
        public event EventHandler SelectionTextColorChanged;

        /// <summary>
        /// Occurs when a drag-and-drop operation for an item is completed.
        /// </summary>
        [Category("Drag Drop")]
        public event EventHandler<ListViewItemDragEventArgs> ItemDragDrop;

        /// <summary>
        /// Occurs when the user begins dragging an item.
        /// </summary>
        [Category("Drag Drop")]
        public event EventHandler<CancelListViewItemDragEventArgs> ItemDragging;

        #endregion

        #region Overridden Methods

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.DragDrop" /> event.
        /// </summary>
        /// <param name="drgevent">A <see cref="T:System.Windows.Forms.DragEventArgs" /> that contains the event data.</param>
        protected override void OnDragDrop(DragEventArgs drgevent)
        {
            if (this.IsRowDragInProgress)
            {
                try
                {
                    ListViewItem dropItem;

                    dropItem = this.InsertionIndex != -1 ? this.Items[this.InsertionIndex] : null;

                    if (dropItem != null)
                    {
                        ListViewItem dragItem;
                        int dropIndex;

                        dragItem = (ListViewItem)drgevent.Data.GetData(typeof(ListViewItem));
                        dropIndex = dropItem.Index;

                        if (dragItem.Index < dropIndex)
                        {
                            dropIndex--;
                        }
                        if (this.InsertionMode == InsertionMode.After && dragItem.Index < this.Items.Count - 1)
                        {
                            dropIndex++;
                        }

                        if (dropIndex != dragItem.Index)
                        {
                            ListViewItemDragEventArgs args;
                            Point clientPoint;

                            clientPoint = this.PointToClient(new Point(drgevent.X, drgevent.Y));
                            args = new ListViewItemDragEventArgs(dragItem, dropItem, dropIndex, this.InsertionMode, clientPoint.X, clientPoint.Y);

                            this.OnItemDragDrop(args);

                            if (!args.Cancel)
                            {
                                this.Items.Remove(dragItem);
                                this.Items.Insert(dropIndex, dragItem);
                                this.SelectedItem = dragItem;
                            }
                        }
                    }
                }
                finally
                {
                    this.InsertionIndex = -1;
                    this.IsRowDragInProgress = false;
                    this.Invalidate();
                }
            }

            base.OnDragDrop(drgevent);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.DragLeave" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected override void OnDragLeave(EventArgs e)
        {
            this.InsertionIndex = -1;
            this.Invalidate();

            base.OnDragLeave(e);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.DragOver" /> event.
        /// </summary>
        /// <param name="drgevent">A <see cref="T:System.Windows.Forms.DragEventArgs" /> that contains the event data.</param>
        protected override void OnDragOver(DragEventArgs drgevent)
        {
            if (this.IsRowDragInProgress)
            {
                int insertionIndex;
                InsertionMode insertionMode;
                ListViewItem dropItem;
                Point clientPoint;

                clientPoint = this.PointToClient(new Point(drgevent.X, drgevent.Y));
                dropItem = this.GetItemAt(0, Math.Min(clientPoint.Y, this.Items[this.Items.Count - 1].GetBounds(ItemBoundsPortion.Entire).Bottom - 1));

                if (dropItem != null)
                {
                    Rectangle bounds;

                    bounds = dropItem.GetBounds(ItemBoundsPortion.Entire);
                    insertionIndex = dropItem.Index;
                    insertionMode = clientPoint.Y < bounds.Top + (bounds.Height / 2) ? InsertionMode.Before : InsertionMode.After;

                    drgevent.Effect = DragDropEffects.Move;
                }
                else
                {
                    insertionIndex = -1;
                    insertionMode = this.InsertionMode;

                    drgevent.Effect = DragDropEffects.None;
                }

                if (insertionIndex != this.InsertionIndex || insertionMode != this.InsertionMode)
                {
                    this.InsertionMode = insertionMode;
                    this.InsertionIndex = insertionIndex;
                    this.Invalidate();
                }

                // Scroll top or bottom for dragging item
                if (this.InsertionIndex > -1 && this.InsertionIndex < this.Items.Count)
                {
                    EnsureVisible(this.InsertionIndex);
                }
            }

            base.OnDragOver(drgevent);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.ListView.ItemDrag" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.Windows.Forms.ItemDragEventArgs" /> that contains the event data.</param>
        protected override void OnItemDrag(ItemDragEventArgs e)
        {
            if (this.AllowItemDrag && this.Items.Count > 1)
            {
                CancelListViewItemDragEventArgs args;

                args = new CancelListViewItemDragEventArgs((ListViewItem)e.Item);

                this.OnItemDragging(args);

                if (!args.Cancel)
                {
                    this.IsRowDragInProgress = true;
                    this.DoDragDrop(e.Item, DragDropEffects.Move);
                }
            }

            base.OnItemDrag(e);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.Paint"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs"/> that contains the event data. </param>
        protected override void OnPaint(PaintEventArgs e)
        {
            

            base.OnPaint(e);
        }

        protected override void OnDrawItem(DrawListViewItemEventArgs e)
        {
            TextFormatFlags flags = TextFormatFlags.Left | TextFormatFlags.EndEllipsis;

            if (this.View == View.Details)
            {
                return;
            }

            int indent = 0;
            int checkBoxWidth = 0;

            Rectangle rectangle;
         
            e.Graphics.Clip = new Region(new Rectangle(e.Item.Bounds.X, e.Item.Bounds.Y, e.Item.Bounds.Width + indent, e.Item.Bounds.Height));
            
            rectangle = e.Item.Bounds;
            rectangle.X += e.Item.IndentCount;

            if (e.Item.ImageKey != "" || e.Item.ImageIndex > -1)
            {
                if (this.SmallImageList != null)
                {
                    if (this.SmallImageList.Images.ContainsKey(e.Item.ImageKey))
                    {                   
                        indent = e.Item.IndentCount * 16;
                        indent += this.SmallImageList.ImageSize.Width + 8;                    
                    }
                }
            }

            Color textColor = e.Item.ForeColor;

            // due to a bug in the win-apis listview renderer, we need to draw item backgrounds
            // in subitem ownerdraw method too. Its important, to clip the drawing area to each column
            // and draw the background for each column seperately - Otherwise there will be flickering
            // and broken subitem texts!
            if (e.State.HasFlag(ListViewItemStates.Selected) && e.Item.Selected)
            {
                if (this.HideSelection)
                {
                    if (this.Focused)
                    {
                        e.Graphics.FillRectangle(new SolidBrush(this._selectionColor), rectangle);
                        textColor = this._selectedTextColor;
                    }
                }
                else
                {
                    Color highlightColor = this._selectionColor;
                    if (!this.Focused)
                    {
                        highlightColor = SystemColors.ControlLight;
                    }
                    else
                    {
                        textColor = this._selectedTextColor;
                    }

                    // Draw the background and focus rectangle for a selected item.

                    e.Graphics.FillRectangle(new SolidBrush(highlightColor), rectangle);

                }
            }
            else
            {
                // Draw the background for an unselected item.
                if (e.Item.Selected)
                {
                    if (this.HideSelection)
                    {
                        if (this.Focused)
                        {
                            textColor = this._selectedTextColor;
                            e.Graphics.FillRectangle(new SolidBrush(this._selectionColor), rectangle);

                        }
                    }
                    else
                    {
                        Color highlightColor = this._selectionColor;
                        if (!this.Focused)
                        {
                            highlightColor = SystemColors.ControlLight;
                        }
                        else
                        {
                            textColor = this._selectedTextColor;
                        }

                        // Draw the background and focus rectangle for a selected item.
                        e.Graphics.FillRectangle(new SolidBrush(highlightColor), rectangle);
                    }

                }
                else
                {
                    if (e.State.HasFlag(ListViewItemStates.Grayed) || this.Enabled == false)
                    {
                        e.Graphics.FillRectangle(new SolidBrush(SystemColors.Control), rectangle);
                    }
                    else
                    {
                        e.Graphics.FillRectangle(new SolidBrush(e.Item.BackColor), rectangle);
                    }
                }
            }


            //if (e.ColumnIndex == 0)
            //{
            /*
                if (e.ItemState.HasFlag(ListViewItemStates.Focused))
                {
                    e.Graphics.Clip = new Region(e.Item.Bounds);
                    e.DrawFocusRectangle(e.Item.Bounds);
                }
            */
            //}


            // Draw item text for each subitem, use Textrenderer to allow for ellipsis-text...
            TextRenderer.DrawText(e.Graphics, e.Item.Text, this.Font, new Rectangle(e.Item.Bounds.X + indent, e.Item.Bounds.Y + 2, e.Item.Bounds.Width + indent, e.Item.Bounds.Height), textColor, flags);

            base.OnDrawItem(e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnDrawSubItem(DrawListViewSubItemEventArgs e)
        {
            
            TextFormatFlags flags = TextFormatFlags.Left | TextFormatFlags.EndEllipsis;

            if (this.View != View.Details)
            {
                return;
            }

            int indent = 0;
            int checkBoxWidth = 0;

            if (e.Item.ImageKey != "" || e.Item.ImageIndex > -1)
            {
                if (this.SmallImageList != null)
                {
                    if (this.SmallImageList.Images.ContainsKey(e.Item.ImageKey))
                    {
                        if (e.ColumnIndex == 0)
                        {
                            indent = e.Item.IndentCount * 16;
                            indent += this.SmallImageList.ImageSize.Width + 8;
                        }
                    }
                }
            }

            // Draw the checkbox if needed
            if (this.CheckBoxes && e.ColumnIndex == 0)
            {
                if (this._chechBoxIconCheck != null && this._chechBoxIcon != null)
                {
                    Image check = e.Item.Checked ? this._chechBoxIconCheck : this._chechBoxIcon;

                    ImageOperations.ResizeImage(ref check, new Size(e.Bounds.Height, e.Bounds.Height), this.BackColor, InterpolationMode.HighQualityBicubic, CompositingMode.SourceOver);

                    e.Graphics.DrawImage(check, new Point(e.SubItem.Bounds.X, e.SubItem.Bounds.Y));

                    checkBoxWidth = check.Width;
                }
                else
                {
                   
                    CheckBox checkBox = new CheckBox();
                    checkBox.Padding = new Padding(0);
                    checkBox.Appearance = Appearance.Normal;
                    checkBox.FlatStyle = FlatStyle.Flat;
                    checkBox.Margin = new Padding(0);
                    checkBox.BackColor = this.BackColor;
                    checkBox.Checked = e.Item.Checked;
                    checkBox.Enabled = this.Enabled;
                    checkBox.Size = new Size(e.Bounds.Height, e.Bounds.Height);
                    checkBox.Text = "";
                    
                    Bitmap check = new Bitmap(checkBox.Size.Width, checkBox.Size.Height);
                    checkBox.DrawToBitmap(check, new Rectangle(0, 0, checkBox.Size.Width, checkBox.Size.Height));
                    checkBox.Dispose();
                    e.Graphics.DrawImage(check, new Point(e.SubItem.Bounds.X, e.SubItem.Bounds.Y));

                    checkBoxWidth = check.Width;
                }

                indent += checkBoxWidth;
            }

            e.Graphics.Clip = new Region(e.SubItem.Bounds);

            int itemWidth = e.SubItem.Bounds.Width;
            if (e.ColumnIndex == 0)
            {
                itemWidth = e.Header.Width - indent;

                e.Graphics.Clip = new Region(new Rectangle(e.SubItem.Bounds.X, e.SubItem.Bounds.Y, itemWidth + indent, e.SubItem.Bounds.Height));
            }

            Rectangle rectangle;

            rectangle = e.Item.Bounds;
            rectangle.X += e.Item.IndentCount;

            if (e.ColumnIndex == 0)
            {
                if (e.Item.ImageKey != "" || e.Item.ImageIndex > -1)
                {
                    if (this.SmallImageList != null)
                    {
                        rectangle.X += this.SmallImageList.ImageSize.Width + 8;
                        rectangle.Width -= this.SmallImageList.ImageSize.Width + 8;

                        if (this.SmallImageList.Images.ContainsKey(e.Item.ImageKey))
                        {
                            Image img = this.SmallImageList.Images[e.Item.ImageKey];

                            e.Graphics.DrawImage(img, new Point(e.Bounds.X + 4, e.Bounds.Y + 2));
                        }
                    }
                }

                if (this.CheckBoxes && e.ColumnIndex == 0)
                {
                    rectangle.X += 16;
                }

                if (this.FullRowSelect)
                {
                    rectangle.Width = this.Columns.Cast<ColumnHeader>().Sum(c => c.Width) - indent;
                }
                else
                {
                    rectangle.Width = (int)e.Graphics.MeasureString(e.Item.Text, this.Font).Width + 8;
                }
            }
            else
            {
                rectangle.X = e.SubItem.Bounds.X;
                rectangle.Width = e.SubItem.Bounds.Width;

            }

            Color textColor = e.SubItem.ForeColor;

            // due to a bug in the win-apis listview renderer, we need to draw item backgrounds
            // in subitem ownerdraw method too. Its important, to clip the drawing area to each column
            // and draw the background for each column seperately - Otherwise there will be flickering
            // and broken subitem texts!
            if (e.ItemState.HasFlag(ListViewItemStates.Selected) && e.Item.Selected)
            {
                if (this.HideSelection)
                {
                    if (this.Focused)
                    {
                        if (this.FullRowSelect || e.ColumnIndex == 0)
                        { 
                            e.Graphics.FillRectangle(new SolidBrush(this._selectionColor), rectangle);
                        }
                        
                        textColor = this._selectedTextColor;
                    }
                }
                else
                {
                    Color highlightColor = this._selectionColor;
                    if (!this.Focused)
                    {
                        highlightColor = SystemColors.ControlLight;
                    } else
                    {
                        textColor = this._selectedTextColor;
                    }

                    // Draw the background and focus rectangle for a selected item.

                    if (this.FullRowSelect || e.ColumnIndex == 0)
                    {
                        e.Graphics.FillRectangle(new SolidBrush(highlightColor), rectangle);
                    }
                }
            }
            else
            {
                // Draw the background for an unselected item.
                if (e.Item.Selected)
                {
                    if (this.HideSelection)
                    {
                        if (this.Focused)
                        {
                            textColor = this._selectedTextColor;
                            if (this.FullRowSelect || e.ColumnIndex == 0)
                            {
                                e.Graphics.FillRectangle(new SolidBrush(this._selectionColor), rectangle);
                            }
                        }
                    }
                    else
                    {
                        Color highlightColor = this._selectionColor;
                        if (!this.Focused)
                        {
                            highlightColor = SystemColors.ControlLight;
                        } else
                        {
                            textColor = this._selectedTextColor;
                        }

                        // Draw the background and focus rectangle for a selected item.
                        if (this.FullRowSelect || e.ColumnIndex == 0)
                        {
                            e.Graphics.FillRectangle(new SolidBrush(highlightColor), rectangle);
                        }
                    }
                }
                else
                {
                    if (e.ItemState.HasFlag(ListViewItemStates.Grayed) || this.Enabled == false)
                    {
                        e.Graphics.FillRectangle(new SolidBrush(SystemColors.Control), rectangle);
                    }
                    else
                    {
                        e.Graphics.FillRectangle(new SolidBrush(e.Item.BackColor), rectangle);
                    }
                }
            }


            //if (e.ColumnIndex == 0)
            //{
            /*
                if (e.ItemState.HasFlag(ListViewItemStates.Focused))
                {
                    e.Graphics.Clip = new Region(e.Item.Bounds);
                    e.DrawFocusRectangle(e.Item.Bounds);
                }
            */
            //}


            // Draw item text for each subitem, use Textrenderer to allow for ellipsis-text...
            TextRenderer.DrawText(e.Graphics, e.SubItem.Text, this.Font, new Rectangle(e.SubItem.Bounds.X + indent, e.SubItem.Bounds.Y + 2, itemWidth, e.SubItem.Bounds.Height), textColor, flags);

            base.OnDrawSubItem(e);
        }

        /// <summary>
        /// Overrides <see cref="M:System.Windows.Forms.Control.WndProc(System.Windows.Forms.Message@)" />.
        /// </summary>
        /// <param name="m">The Windows <see cref="T:System.Windows.Forms.Message" /> to process.</param>
        [DebuggerStepThrough]
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            switch (m.Msg)
            {
                case WM_PAINT:
                    this.OnWmPaint(ref m);
                    break;
            }
        }

        #endregion

        #region Public Properties

        [Category("Behavior")]
        [DefaultValue(false)]
        public virtual bool AllowItemDrag
        {
            get { return _allowItemDrag; }
            set
            {
                if (this.AllowItemDrag != value)
                {
                    _allowItemDrag = value;

                    this.OnAllowItemDragChanged(EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Gets or sets the color of the insertion line drawn when dragging Items within the control.
        /// </summary>
        /// <value>The color of the insertion line.</value>
        [Category("Appearance")]
        [DefaultValue(typeof(Color), "Red")]
        public virtual Color InsertionLineColor
        {
            get { return _insertionLineColor; }
            set
            {
                if (this.InsertionLineColor != value)
                {
                    _insertionLineColor = value;

                    this.OnInsertionLineColorChanged(EventArgs.Empty);
                }
            }
        }

        [Category("Appearance")]
        [DefaultValue(typeof(SystemColors), "Highlight")]
        public virtual Color SelectionColor
        {
            get { return _selectionColor; }
            set
            {
                if (this._selectionColor != value)
                {
                    _selectionColor = value;

                    this.OnSelectionColorChanged(EventArgs.Empty);
                }
            }
        }

        [Category("Appearance")]
        [DefaultValue(typeof(Color), "Black")]
        public virtual Color SelectionTextColor
        {
            get { return _selectedTextColor; }
            set
            {
                if (this._selectedTextColor != value)
                {
                    _selectedTextColor = value;

                    this.OnSelectionTextColorChanged(EventArgs.Empty);
                }
            }
        }


        [Category("Appearance")]
        [DefaultValue(typeof(Image), "NULL")]
        public virtual Image CheckBoxIcon
        {
            get { return _chechBoxIcon; }
            set
            {
                if (this._chechBoxIcon != value)
                {
                    _chechBoxIcon = value;

                    //this.OnSelectionTextColorChanged(EventArgs.Empty);
                }
            }
        }

        [Category("Appearance")]
        [DefaultValue(typeof(Image), "NULL")]
        public virtual Image CheckBoxIconChecked
        {
            get { return _chechBoxIconCheck; }
            set
            {
                if (this._chechBoxIconCheck != value)
                {
                    _chechBoxIconCheck = value;

                    //this.OnSelectionTextColorChanged(EventArgs.Empty);
                }
            }
        }


        /// <summary>
        /// Gets or sets the selected <see cref="ListViewItem"/>.
        /// </summary>
        /// <value>The selected <see cref="ListViewItem"/>.</value>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ListViewItem SelectedItem
        {
            get { return this.SelectedItems.Count != 0 ? this.SelectedItems[0] : null; }
            set
            {
                this.SelectedItems.Clear();
                if (value != null)
                {
                    value.Selected = true;
                }
                this.FocusedItem = value;
            }
        }

        #endregion

        #region Protected Properties

        protected int InsertionIndex { get; set; }

        protected InsertionMode InsertionMode { get; set; }

        protected bool IsRowDragInProgress { get; set; }

        #endregion

        #region Protected Members

        /// <summary>
        /// Raises the <see cref="AllowItemDragChanged" /> event.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected virtual void OnAllowItemDragChanged(EventArgs e)
        {
            EventHandler handler;

            handler = this.AllowItemDragChanged;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="InsertionLineColorChanged" /> event.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected virtual void OnInsertionLineColorChanged(EventArgs e)
        {
            EventHandler handler;

            handler = this.InsertionLineColorChanged;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="InsertionLineColorChanged" /> event.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected virtual void OnSelectionColorChanged(EventArgs e)
        {
            EventHandler handler;

            handler = this.SelectionColorChanged;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="InsertionLineColorChanged" /> event.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected virtual void OnSelectionTextColorChanged(EventArgs e)
        {
            EventHandler handler;

            handler = this.SelectionTextColorChanged;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="ItemDragDrop" /> event.
        /// </summary>
        /// <param name="e">The <see cref="ListViewItemDragEventArgs" /> instance containing the event data.</param>
        protected virtual void OnItemDragDrop(ListViewItemDragEventArgs e)
        {
            EventHandler<ListViewItemDragEventArgs> handler;

            handler = this.ItemDragDrop;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="ItemDragging" /> event.
        /// </summary>
        /// <param name="e">The <see cref="CancelListViewItemDragEventArgs" /> instance containing the event data.</param>
        protected virtual void OnItemDragging(CancelListViewItemDragEventArgs e)
        {
            EventHandler<CancelListViewItemDragEventArgs> handler;

            handler = this.ItemDragging;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnWmPaint(ref Message m)
        {
            this.DrawInsertionLine();
        }

        #endregion

        #region Private Members

        private void DrawInsertionLine()
        {
            if (this.InsertionIndex != -1)
            {
                int index;

                index = this.InsertionIndex;

                if (index >= 0 && index < this.Items.Count)
                {
                    Rectangle bounds;
                    int x;
                    int y;
                    int width;

                    bounds = this.Items[index].GetBounds(ItemBoundsPortion.Entire);
                    x = 0; // aways fit the line to the client area, regardless of how the user is scrolling
                    y = this.InsertionMode == InsertionMode.Before ? bounds.Top : bounds.Bottom;
                    width = Math.Min(bounds.Width - bounds.Left, this.ClientSize.Width); // again, make sure the full width fits in the client area

                    this.DrawInsertionLine(x, y, width);
                }
            }
        }

        private void DrawInsertionLine(int x1, int y, int width)
        {
            using (Graphics g = this.CreateGraphics())
            {
                Point[] leftArrowHead;
                Point[] rightArrowHead;
                int arrowHeadSize;
                int x2;

                x2 = x1 + width;
                arrowHeadSize = 7;
                leftArrowHead = new[]
                                {
                          new Point(x1, y - (arrowHeadSize / 2)), new Point(x1 + arrowHeadSize, y), new Point(x1, y + (arrowHeadSize / 2))
                        };
                rightArrowHead = new[]
                                 {
                           new Point(x2, y - (arrowHeadSize / 2)), new Point(x2 - arrowHeadSize, y), new Point(x2, y + (arrowHeadSize / 2))
                         };

                using (Pen pen = new Pen(this.InsertionLineColor))
                {
                    g.DrawLine(pen, x1, y, x2 - 1, y);
                }

                using (Brush brush = new SolidBrush(this.InsertionLineColor))
                {
                    g.FillPolygon(brush, leftArrowHead);
                    g.FillPolygon(brush, rightArrowHead);
                }
            }
        }

        #endregion
    }
}
