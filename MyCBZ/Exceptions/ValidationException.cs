using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Win_CBZ.Exceptions
{
    internal class ValidationException : ApplicationException
    {

        public String Value { get; set; }

        public String ControlName { get; set; }

        public bool RemoveEntry { get; set; }

        public ValidationException(String value, string control, String message, bool showErrorDialog = false, bool removeEntry = false) : base(message, showErrorDialog)
        {
            Value = value;
            RemoveEntry = removeEntry;
            ControlName = control;
        }
    }
}
