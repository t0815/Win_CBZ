using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Win_CBZ
{
    internal class MetaDataValidationException : Exception
    {

        public MetaDataEntry Item;

        public new String Message;


        public MetaDataValidationException(MetaDataEntry item, String message)
        {
            Message = message;
            Item = item;
        }
    }
}
