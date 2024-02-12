using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Win_CBZ.Data
{
    internal class RenamePagesThreadParams
    {
        public bool IgnorePageNameDuplicates { get; set; }

        public bool SkipIndexUpdate { get; set; }

        public bool CompatibilityMode { get; set; } 

        public bool ApplyRenaming { get; set; }

        public String RenameStoryPagePattern { get; set; }

        public String RenameSpecialPagePattern { get; set; }

        public bool ContinuePipeline { get; set; } 

        public List<StackItem> Stack { get; set; }

    }
}
