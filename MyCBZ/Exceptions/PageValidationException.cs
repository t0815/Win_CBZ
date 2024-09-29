using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Win_CBZ.Exceptions
{
    internal class PageValidationException : ApplicationException
    {
        public string ControlName { get; set; }

        public bool RemoveEntry { get; set; }

        public Page Page { get; set; }

        public PageValidationException(Page page, string control, String message, bool showErrorDialog = false, bool removeEntry = false) : base(message, showErrorDialog)
        {
            Page = page;
            RemoveEntry = removeEntry;
            ControlName = control;
        }
    }
}
