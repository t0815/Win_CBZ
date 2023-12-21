using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Win_CBZ
{
    internal class PageMemoryIOException : PageException
    {

        public bool reloadFromFile = false;

        public PageMemoryIOException(Page page, String message, bool reload = false, bool showErrorDialog = false, Exception source = null) : base(page, message, showErrorDialog)
        {
            Page = page;  
            SourceEx = source != null ? source : base.InnerException;
            reloadFromFile = reload;
        }
    }
}
