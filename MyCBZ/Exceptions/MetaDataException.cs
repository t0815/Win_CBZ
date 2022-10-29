using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Win_CBZ
{
    internal class MetaDataException : ApplicationException
    {

        public MetaData MetaData { get; set; }


        public MetaDataException(MetaData data, String message, bool showErrorDialog = false) : base(message, showErrorDialog)
        {
            MetaData = data;           
        }
    }
}
