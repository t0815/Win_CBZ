using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Win_CBZ
{
    internal class ApplicationStatusEvent
    {
        public const int STATE_READY = 0;
        public const int STATE_OPENING = 1;
        public const int STATE_CLOSING = 2;
        public const int STATE_SAVING = 3;
        public const int STATE_RENAMING = 4;
        public const int STATE_DELETING = 5;
        public const int STATE_ADDING = 6;
        public const int STATE_ANALYZING = 7;
        public const int STATE_UPDATING_INDEX = 8;
        public const int STATE_PROCESSING = 9;
        public const int STATE_CHECKING_INDEX = 10;

        public ProjectModel ArchiveInfo { get; set; }

        public int State { get; set; }


        public delegate void Operation();


        public CBZArchiveStatusEvent.Operation Callback;


        public ApplicationStatusEvent(ProjectModel project, int state)
        {
            ArchiveInfo = project;
            State = state;

        }

        public ApplicationStatusEvent(ProjectModel project, int state, CBZArchiveStatusEvent.Operation callback)
        {
            ArchiveInfo = project;
            State = state;
            Callback = callback;
        }   
    }
}
