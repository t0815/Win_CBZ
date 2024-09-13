using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Win_CBZ.Models
{
    internal class KeyValueItem
    {

        public KeyValueItem() { }

        public KeyValueItem(string key, object value)
        {
            Key = key;
            Value = value;
        }

        public string Key { get; set; }

        public object Value { get; set; }

        public override string ToString()
        {
            return Key + "=" + Value.ToString();
        }
    }
}
