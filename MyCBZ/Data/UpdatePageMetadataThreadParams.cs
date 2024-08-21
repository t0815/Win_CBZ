using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Win_CBZ.Data
{
    internal class UpdatePageMetadataThreadParams : ThreadParam
    {
        public List<Page> Pages { get; set; }
    }
}
