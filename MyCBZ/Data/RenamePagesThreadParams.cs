using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Win_CBZ.Data
{
    internal class RenamePagesThreadParams
    {
        public bool IgnorePageNameDuplicates {  get; set; }

        public bool CompatibilityMode { get; set; } 

        public bool ApplyRenaming { get; set; }
    }
}
