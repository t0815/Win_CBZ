using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using Win_CBZ.Components.Rating;

namespace Win_CBZ.Extensions
{
    [SupportedOSPlatform("windows")]
    class Native
    {
        public const uint WM_KEYDOWN = 0x100;
        [DllImport("user32.dll")]
        public static extern bool PostMessage(IntPtr hWnd, UInt32 Msg, Int32 wParam, Int32 lParam);
    }

    [SupportedOSPlatform("windows")]
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
   
    public class CustomTextCell : DataGridViewTextBoxCell
    {
        public override Type EditType
        {
            get { return typeof(CustomTextBoxEditingControl); }
        }
    }


    public class RatingCell : Rating, IDataGridViewEditingControl
    {
        

        public DataGridView EditingControlDataGridView { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public object EditingControlFormattedValue { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int EditingControlRowIndex { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool EditingControlValueChanged { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public System.Windows.Forms.Cursor EditingPanelCursor => throw new NotImplementedException();

        public bool RepositionEditingControlOnValueChange => throw new NotImplementedException();

        public void ApplyCellStyleToEditingControl(DataGridViewCellStyle dataGridViewCellStyle)
        {
            throw new NotImplementedException();
        }

        public bool EditingControlWantsInputKey(Keys keyData, bool dataGridViewWantsInputKey)
        {
            throw new NotImplementedException();
        }

        public object GetEditingControlFormattedValue(DataGridViewDataErrorContexts context)
        {
            throw new NotImplementedException();
        }

        public void PrepareEditingControlForEdit(bool selectAll)
        {
            throw new NotImplementedException();
        }
    }


    public class CustomTextBoxEditingControl :  DataGridViewTextBoxEditingControl
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
