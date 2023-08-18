using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Win_CBZ.Data;

namespace Win_CBZ.Tasks
{
    internal class ReadImageMetaDataTask
    {

        public static Task<TaskResult> UpdateImageMetadata(List<Page> pages)
        {
            return new Task<TaskResult>(() =>
            {
                TaskResult result = new TaskResult();

                foreach (Page p in pages)
                {
                    p.LoadImageInfo();
                }


                return result;
            });
        }
    }
}
