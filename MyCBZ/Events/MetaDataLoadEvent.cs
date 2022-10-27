using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Win_CBZ
{
    internal class MetaDataLoadEvent
    {

        public BindingList<MetaDataEntry> MetaData { get; set; }  

        public MetaDataLoadEvent(BindingList<MetaDataEntry> metadata)
        {
            this.MetaData = metadata;
        }   
    }
}
