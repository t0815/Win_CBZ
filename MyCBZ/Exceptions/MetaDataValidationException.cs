using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Win_CBZ
{
    internal class MetaDataValidationException : ApplicationException
    {

        public MetaDataEntry Item;

        public bool RemoveEntry;

        public MetaDataValidationException(MetaDataEntry item, String message, bool showErrorDialog = false, bool removeEntry = false) : base(message, showErrorDialog)
        {
            Item = item;
            RemoveEntry = removeEntry;
        }
    }
}
