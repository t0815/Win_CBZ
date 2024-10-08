﻿

namespace Win_CBZ.Events
{
    internal class ArchiveStatusEvent
    {
        public const int ARCHIVE_NEW = 0;
        public const int ARCHIVE_OPENING = 1;
        public const int ARCHIVE_OPENED = 2;
        public const int ARCHIVE_SAVING = 3;
        public const int ARCHIVE_SAVED = 4;
        public const int ARCHIVE_CLOSING = 5;
        public const int ARCHIVE_CLOSED = 6;
        public const int ARCHIVE_EXTRACTING = 7;
        public const int ARCHIVE_EXTRACTED = 8;
        public const int ARCHIVE_FILE_ADDED = 9;
        public const int ARCHIVE_FILE_DELETED = 10;
        public const int ARCHIVE_FILE_RENAMED = 11;
        public const int ARCHIVE_FILE_UPDATED = 12;
        public const int ARCHIVE_METADATA_ADDED = 13;
        public const int ARCHIVE_METADATA_CHANGED = 14;
        public const int ARCHIVE_METADATA_DELETED = 15;
        public const int ARCHIVE_ERROR_SAVING = 16;
        public const int ARCHIVE_READY = 17;


        public ProjectModel ArchiveInfo { get; set; }

        public int State { get; set; }


        public delegate void CallbackDelegate(object param);


        public CallbackDelegate Callback;


        public ArchiveStatusEvent(ProjectModel project, int state)
        {
            ArchiveInfo = project;
            State = state;
        }

        public ArchiveStatusEvent(ProjectModel project, int state, CallbackDelegate callback)
        {
            ArchiveInfo = project;
            State = state;
            Callback = callback;
        }   
    }
}
