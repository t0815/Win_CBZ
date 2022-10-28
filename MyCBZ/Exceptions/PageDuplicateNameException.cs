using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Win_CBZ
{
    internal class PageDuplicateNameException : Exception
    {

        public Page Page;

        public new String Message;


        public PageDuplicateNameException(Page page, String message)
        {
            Page = page;
            Message = message;
        }
    }
}
