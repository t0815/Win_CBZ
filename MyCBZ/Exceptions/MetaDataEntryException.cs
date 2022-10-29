using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Win_CBZ
{
    internal class MetaDataEntryException : ApplicationException
    {

        public MetaDataEntry MetaDataEntry { get; set; }


        public MetaDataEntryException(MetaDataEntry entry, String message, bool showErrorDialog = false) : base(message, showErrorDialog)
        {
            MetaDataEntry = entry;           
        }
    }
}
