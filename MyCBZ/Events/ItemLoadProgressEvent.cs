﻿using CBZMage;
using CBZMage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCBZ
{
    internal class ItemLoadProgressEvent
    {

        public int Index { get; set; }  

        public int Total { get; set; }

        public Page Image { get; set; }


        public ItemLoadProgressEvent(int index, int total, Page image)
        {
            this.Index = index;
            this.Total = total;
            this.Image = image;
        }   
    }
}
