using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Win_CBZ.Models
{
    internal class LanguageListItem
    {

        public LanguageListItem(string name, string iso)
        {
            Name = name;
            Iso = iso;
        }

        public string Name { get; set; }

        public string Iso { get; set; }

    }
}
