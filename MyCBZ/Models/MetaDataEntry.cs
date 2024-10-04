using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Win_CBZ.Models;

namespace Win_CBZ
{
    internal class MetaDataEntry
    {

        public string Uid { get; set; } = Guid.NewGuid().ToString();

        public String Key { get; set; }

        public String Value { get; set; }

        public bool ReadOnly { get; set; }

        public bool Visible { get; set; } = true;

        public bool UserFiltered { get; set; } = false;

        public MetaDataFieldType Type { get; set; }

        // FilterMatchSourceCol[0] = True, Filter on Key, FilterMatchSourceCol[1] = True, Filter on Value
        public bool[] FilterMatchSourceCol { get; set; } = new bool[2];

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

        public MetaDataEntry(String key, String value, MetaDataFieldType fieldType, bool readOnly = false, string uid = null)
        {
            Key = key;
            Value = value;
            ReadOnly = readOnly;
            Type = fieldType;
            ReadOnly = readOnly;
            Uid = uid != null ? uid : Guid.NewGuid().ToString();
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
