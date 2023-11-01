using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Win_CBZ
{
    internal class PageException : ApplicationException
    {

        public Page Page { get; set; }

        public Exception SourceEx { get; set; }


        public PageException(Page page, String message, bool showErrorDialog = false, Exception source = null) : base(message, showErrorDialog)
        {
            Page = page;  
            SourceEx = source != null ? source : base.InnerException;
        }
    }
}
