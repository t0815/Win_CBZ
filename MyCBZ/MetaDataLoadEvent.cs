using CBZMage;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCBZ
{
    internal class MetaDataLoadEvent
    {

        public ObservableCollection<CBZMetaDataEntry> MetaData { get; set; }  

        public MetaDataLoadEvent(ObservableCollection<CBZMetaDataEntry> metadata)
        {
            this.MetaData = metadata;
        }   
    }
}
