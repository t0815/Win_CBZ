using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Win_CBZ.Data
{
    internal class ExtractArchiveThreadParams
    {
        public String OutputPath { get; set; }

        public List<Page> Pages { get; set; }

        public CancellationToken CancelToken { get; set; }
    }
}
