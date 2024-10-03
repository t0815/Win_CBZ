using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Win_CBZ
{
    internal class ApplicationException : Exception
    {

        public bool ShowErrorDialog { get; set; }

        public ApplicationException() { }


        public ApplicationException(string message) : base(message)
        {
            ShowErrorDialog = true;
        }


        public ApplicationException(String message, bool showErrorDialog) : base(message)
        {
            ShowErrorDialog = showErrorDialog;           
        }
    }
}
