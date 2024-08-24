using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Win_CBZ.Events
{
    internal class BackgroundTaskProgressEvent
    {

        public int Status { get; set; }

        public int Current { get; set; }

        public int Total { get; set; }

        public string Message { get; set; }
    }
}
