using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Win_CBZ.Extensions;

namespace Win_CBZ.Components.CustomDataGridView
{
    [SupportedOSPlatform("windows")]
    public partial class CustomDataGridView : System.Windows.Forms.DataGridView
    {
  
        protected override void WndProc(ref Message m)
        {
            //the enter key is sent by edit control
            if (m.Msg == Native.WM_KEYDOWN)
            {
                if ((ModifierKeys & Keys.Shift) == 0)
                {
                    Keys key = (Keys)m.WParam;
                    if (key == Keys.Enter)
                    {
                        if (!IsCurrentCellInEditMode)
                        {
                            //MoveToNextCell();
                        }
                        m.Result = IntPtr.Zero;
                        return;
                    }
                }
            }

            base.WndProc(ref m);
        }

        //move the focus to the next cell in same row or to the first cell in next row then begin editing
        public void MoveToNextCell()
        {
            int CurrentColumn, CurrentRow;
            CurrentColumn = this.CurrentCell.ColumnIndex;
            CurrentRow = this.CurrentCell.RowIndex;
            if (CurrentColumn == this.Columns.Count - 1 && CurrentRow != this.Rows.Count - 1)
            {
                this.CurrentCell = Rows[CurrentRow + 1].Cells[CurrentColumn];//0 index is for No and readonly
                this.BeginEdit(false);
            }
            else if (CurrentRow != this.Rows.Count - 1)
            {
                base.ProcessDataGridViewKey(new KeyEventArgs(Keys.Tab));
                this.BeginEdit(false);
            }
        }

        protected override bool ProcessDataGridViewKey(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Left || e.KeyCode == Keys.Right)
            {
                //return false;
                if (IsCurrentCellInEditMode)   // disable the keys only in EditMode
                {
                    return false;
                }
            }
            return base.ProcessDataGridViewKey(e);
        }

        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (keyData == Keys.Enter)
            {
                //EndEdit();
                return true;
            }
            return base.ProcessDialogKey(keyData);
        }
    }
}
