using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Win_CBZ.Data
{
    internal class ThumbSliceThreadParams : ThreadParam
    {

        public List<Page> ThumbnailPagesSlice {  get; set; }

        public List<Page> ThumbnailQueue { get; set; }
    }
}
