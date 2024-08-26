using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Win_CBZ.Events
{
    internal class MetaDataLoadEvent
    {

        public List<MetaDataEntry> MetaData { get; set; }  

        public MetaDataLoadEvent(List<MetaDataEntry> metadata)
        {
            MetaData = metadata;
        }   
    }
}
