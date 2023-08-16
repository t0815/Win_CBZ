using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Win_CBZ
{
    internal class MetaDataEntryChangedEvent
    {

        public const int ENTRY_NEW = 0;
        public const int ENTRY_UPDATED = 1;
        public const int ENTRY_DELETED = 2;

        public MetaDataEntry Entry { get; set; }  

        public int State { get; set; }

        public int Index { get; set; }

        public MetaDataEntryChangedEvent(int state, int index, MetaDataEntry entry)
        {
            Entry = entry;
            State = state;
            Index = index;
        }   
    }
}
