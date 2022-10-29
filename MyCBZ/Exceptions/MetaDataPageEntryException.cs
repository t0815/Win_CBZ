using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Win_CBZ
{
    internal class MetaDataPageEntryException : ApplicationException
    {

        public MetaDataEntryPage Entry { get; set; }


        public MetaDataPageEntryException(MetaDataEntryPage entry, String message, bool showErrorDialog = false) : base(message, showErrorDialog)
        {
            Entry = entry;           
        }
    }
}
