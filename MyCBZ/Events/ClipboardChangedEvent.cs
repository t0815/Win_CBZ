using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Win_CBZ
{
    internal class ClipboardChangedEvent
    {

        public List<Page> Pages;

        public bool ShowErrorsDialog = false;

        public ClipboardChangedEvent(List<Page> copiedPages, bool showError = false) 
        {
            Pages = copiedPages;
            ShowErrorsDialog = showError;
        }
    }
}
