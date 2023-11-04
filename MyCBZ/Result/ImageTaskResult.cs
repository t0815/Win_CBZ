using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Win_CBZ.Result
{
    internal class ImageTaskResult
    {

        List<Page> pages;

        public void AddFinishedPage(Page page)
        {
            pages.Add(page);
        }

        public ImageTaskResult() 
        {
            pages = new List<Page>();
        }
    }
}
