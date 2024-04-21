using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Win_CBZ.Data;

namespace Win_CBZ
{
    internal class MetaDataEntry
    {

        protected string uid = Guid.NewGuid().ToString();

        public String Key { get; set; }

        public String Value { get; set; }

        protected bool ReadOnly { get; set; }

        public bool Visible { get; set; } = true;

        public MetaDataFieldType Type { get; set; }

        public MetaDataEntry(String key)
        {
            Key = key;
            Value = "";
            Type = new MetaDataFieldType(key);
        }

        public MetaDataEntry(String key, String value)
        {
            Key = key;
            Value = value;
            Type = new MetaDataFieldType(key);
        }

        public MetaDataEntry(String key, String value, MetaDataFieldType fieldType, bool readOnly = false)
        {
            Key = key;
            Value = value;
            ReadOnly = readOnly;
            Type = fieldType;
            ReadOnly = readOnly;
        }

        public String[] ValueAsList(char separator = ',')
        {
            if (Value != null)
            {
                return Value.Split(separator).Select((s) => s.TrimEnd().TrimStart()).ToArray();
            } else
            {
                return new string[0];
            }
            
        }
    }
}
