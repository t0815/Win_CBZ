using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace Win_CBZ.Extensions
{

    class Native
    {
        public const uint WM_KEYDOWN = 0x100;
        [DllImport("user32.dll")]
        public static extern bool PostMessage(IntPtr hWnd, UInt32 Msg, Int32 wParam, Int32 lParam);
    }
    //the column that will be added to dgv
    public class CustomTextBoxColumn : DataGridViewColumn
    {
        public CustomTextBoxColumn() : base(new CustomTextCell()) { }
        public override DataGridViewCell CellTemplate
        {
            get { return base.CellTemplate; }
            set
            {
                if (value != null && !value.GetType().IsAssignableFrom(typeof(CustomTextCell)))
                {
                    throw new InvalidCastException("Must be a CustomTextCell");
                }
                base.CellTemplate = value;
            }
        }
    }
    //the cell used in the previous column
    public class CustomTextCell : DataGridViewTextBoxCell
    {
        public override Type EditType
        {
            get { return typeof(CustomTextBoxEditingControl); }
        }
    }
    //the edit control that will take data from user
    public class CustomTextBoxEditingControl : DataGridViewTextBoxEditingControl
    {
        protected override void WndProc(ref Message m)
        {
            //we need to handle the keydown event
            if (m.Msg == Native.WM_KEYDOWN)
            {
                if ((ModifierKeys & Keys.Shift) == 0)//make sure that user isn't entering new line in case of warping is set to true
                {
                    Keys key = (Keys)m.WParam;
                    if (key == Keys.Enter)
                    {
                        if (this.EditingControlDataGridView != null)
                        {
                            if (this.EditingControlDataGridView.IsHandleCreated)
                            {
                                //sent message to parent dvg
                                Native.PostMessage(this.EditingControlDataGridView.Handle, (uint)m.Msg, m.WParam.ToInt32(), m.LParam.ToInt32());
                                m.Result = IntPtr.Zero;
                            }
                            return;
                        }
                    }
                }
            }
            base.WndProc(ref m);
        }
    }
}
