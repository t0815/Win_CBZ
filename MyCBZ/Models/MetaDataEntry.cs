using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Win_CBZ
{
    internal class MetaDataEntry
    {
        public String Key { get; set; }

        public String Value { get; set; }

        protected bool ReadOnly { get; set; }

        public List<string> Options { get; set; } = new List<string>();

        public MetaDataEntry(String key)
        {
            Key = key;
            Value = "";
        }

        public MetaDataEntry(String key, String value)
        {
            Key = key;
            Value = value;
        }

        public MetaDataEntry(String key, String value, bool readOnly = false)
        {
            Key = key;
            Value = value;
            ReadOnly = readOnly;
        }

        public MetaDataEntry(String key, String value, List<String> options, bool readOnly = false)
        {
            Key = key;
            Value = value;
            Options = options;
            ReadOnly = readOnly;
        }
    }
}
