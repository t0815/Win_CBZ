using CBZMage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCBZ
{
    internal class CBZArchiveStatusEvent
    {

        public const int ARCHIVE_OPENING = 1;
        public const int ARCHIVE_OPENED = 2;
        public const int ARCHIVE_SAVING = 3;
        public const int ARCHIVE_SAVED = 4;
        public const int ARCHIVE_CLOSING = 5;
        public const int ARCHIVE_CLOSED = 6;

        public CBZArchiveInfo ArchiveInfo { get; set; }

        public int State { get; set; }


        public delegate void Operation();


        public CBZArchiveStatusEvent.Operation Callback;


        public CBZArchiveStatusEvent(CBZArchiveInfo archive, int state)
        {
            this.ArchiveInfo = archive;
            this.State = state;

        }

        public CBZArchiveStatusEvent(CBZArchiveInfo archive, int state, CBZArchiveStatusEvent.Operation callback)
        {
            this.ArchiveInfo = archive;
            this.State = state;
            this.Callback = callback;
        }   
    }
}
