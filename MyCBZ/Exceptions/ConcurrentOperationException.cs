using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Win_CBZ.Exceptions
{
    internal class ConcurrentOperationException : ApplicationException
    {
        public ConcurrentOperationException(string message, bool showErrorDialog) : base(message, showErrorDialog)
        {
        }
    }
}
