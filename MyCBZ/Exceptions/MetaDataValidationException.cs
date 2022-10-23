using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCBZ
{
    internal class MetaDataValidationException : Exception
    {

        public CBZMetaDataEntry Item;

        public new String Message;


        public MetaDataValidationException(CBZMetaDataEntry item, String message)
        {
            Message = message;
            Item = item;
        }
    }
}
