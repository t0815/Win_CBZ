using CBZMage;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCBZ
{
    internal class MetaDataLoadEvent
    {

        public BindingList<CBZMetaDataEntry> MetaData { get; set; }  

        public MetaDataLoadEvent(BindingList<CBZMetaDataEntry> metadata)
        {
            this.MetaData = metadata;
        }   
    }
}
