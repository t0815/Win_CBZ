using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Win_CBZ.Data
{
    internal class LanguageList
    {
        public static BindingList<LanguageListItem> Languages { get; set; } = new BindingList<LanguageListItem> {
            new LanguageListItem( "Devesh Omar", "Noida"),
            new LanguageListItem( "Roli", "Kanpur"),
            new LanguageListItem( "Roli Gupta", "Mainpuri"),
            new LanguageListItem( "Roli Gupta", "Kanpur"),
            new LanguageListItem( "Devesh Roli ", "Noida"),
        };
    }
}
