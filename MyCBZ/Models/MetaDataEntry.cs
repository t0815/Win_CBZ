using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Win_CBZ.Data;

namespace Win_CBZ
{
    internal class MetaDataEntry
    {
        public String Key { get; set; }

        public String Value { get; set; }

        protected bool ReadOnly { get; set; }

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
            return Value.Split(separator).Select((s) => s.TrimEnd().TrimStart()).ToArray();
        }
    }
}
