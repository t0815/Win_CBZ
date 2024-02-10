using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Win_CBZ.Extensions;

namespace Win_CBZ.Components.CustomDataGridView
{
    internal partial class CustomDataGridView : System.Windows.Forms.DataGridView
    {

        public bool DisableArrowNavigationMode { get; set; }
   
        public CustomDataGridView()
        {
            DisableArrowNavigationMode = false;
        }
   
        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (DisableArrowNavigationMode)
            if (EditingControl != null)
                if (keyData == Keys.Enter)
                    return false;
            return base.ProcessDialogKey(keyData);
        }

        protected override bool ProcessDataGridViewKey(KeyEventArgs e)
        {
                if (DisableArrowNavigationMode)
                    if (EditingControl != null)
                    if (e.KeyCode == Keys.Down || e.KeyCode == Keys.Up || e.KeyCode == Keys.Enter)
                        return false;
                return base.ProcessDataGridViewKey(e);
        }
    }
}
