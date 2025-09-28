using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Win_CBZ.Forms
{

    [SupportedOSPlatform("windows")]
    public partial class PageFilterForm : Form
    {
        public PageFilterForm()
        {
            InitializeComponent();

            Theme.GetInstance().ApplyTheme(ItemEditorTableLayout.Controls);
        }
    }
}
