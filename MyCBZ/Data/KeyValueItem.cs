using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Win_CBZ.Data
{
    internal class KeyValueItem
    {
        public KeyValueItem(String key, String value)
        {
            Key = key;
            Value = value;
        }

        public String Key { get; set; }

        public String Value { get; set; }

        public override String ToString()
        {
            return Key + "=" + Value;
        }
    }
}
