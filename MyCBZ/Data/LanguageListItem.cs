using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Win_CBZ.Data
{
    internal class LanguageListItem
    {

        public LanguageListItem(String name, String iso) 
        {
            Name = name;
            Iso = iso;
        }

        public String Name { get; set; }

        public String Iso { get; set; }

    }
}
