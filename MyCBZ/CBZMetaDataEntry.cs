using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCBZ
{
    internal class CBZMetaDataEntry
    {
        public String Key { get; set; }

        public String Value { get; set; }

        protected bool ReadOnly { get; set; }


        public CBZMetaDataEntry(String key, String value = null)
        {
            Key = key;
            Value = value;
        }

        public CBZMetaDataEntry(String key, String value = null, bool readOnly = false)
        {
            Key = key;
            Value = value;
            ReadOnly = readOnly;
        }

        public List<String> GetOptionsForType()
        {
            List<String> values = new List<String>();

            if (Key != null)
            {
                if (Key.ToLower().Equals("genre"))
                {
                    values.Add("Adult");
                    values.Add("Fantasy");
                    values.Add("Sci-Fi");
                }
            }

            return values;
        }
    }
}
