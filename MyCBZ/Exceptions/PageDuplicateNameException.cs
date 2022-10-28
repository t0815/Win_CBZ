using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Win_CBZ
{
    internal class PageDuplicateNameException : ApplicationException
    {

        public Page Page;


        public PageDuplicateNameException(Page page, String message, bool showErrorDialog = false) : base(message, showErrorDialog)
        {
            Page = page;           
        }
    }
}
