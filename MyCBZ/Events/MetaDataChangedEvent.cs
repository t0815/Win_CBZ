using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Win_CBZ.Events
{
    internal class MetaDataChangedEvent
    {
        public const int METADATA_NEW = 0;

        public const int METADATA_UPDATED = 1;

        public const int METADATA_DELETED = 2;


        public MetaData Data { get; set; }

        public int State { get; set; }

        public MetaDataChangedEvent(int state, MetaData data)
        {
            Data = data;
            State = state;
        }
    }
}
