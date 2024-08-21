using System;
using System.Collections.Generic;

namespace Win_CBZ.Data
{
    internal class RenamePagesThreadParams : ThreadParam
    {

        public List<Page> Pages { get; set; }

        public bool IgnorePageNameDuplicates { get; set; }

        public bool SkipIndexUpdate { get; set; }

        public bool CompatibilityMode { get; set; } 

        public bool ApplyRenaming { get; set; }

        public String RenameStoryPagePattern { get; set; }

        public String RenameSpecialPagePattern { get; set; }

        public bool ContinuePipeline { get; set; }

    }
}
