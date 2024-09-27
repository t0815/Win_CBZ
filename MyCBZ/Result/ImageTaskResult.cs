using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using Win_CBZ.Data;

namespace Win_CBZ.Result
{
    [SupportedOSPlatform("windows")]
    internal class ImageTaskResult
    {

        public List<Page> Pages { get; }

        public List<StackItem> Stack { get; set; } = new List<StackItem>();

        public void AddFinishedPage(Page page)
        {
            Pages.Add(page);
        }

        public ImageTaskResult() 
        {
            Pages = new List<Page>();
        }
    }
}
