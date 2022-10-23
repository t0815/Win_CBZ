using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCBZ
{
    internal class CBZArchiveInfo
    {

        private String WorkingDir;

        public String FileName { get; set; }

        public long FileSize { get; set; } = 0;


        public CBZArchiveInfo()
        {

        }

        public CBZArchiveInfo(String workingDir)
        {
            WorkingDir = workingDir;
        }

    }
}
