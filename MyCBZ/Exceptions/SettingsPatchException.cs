using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Win_CBZ.Exceptions
{
    internal class SettingsPatchException : ApplicationException
    {
        public int LastSuccessFulPatchedVersion {  get; set; }

        public int CurrentVersion { get; set; } 

        public Exception SourceException { get; set; }

        public SettingsPatchException(string message, int lastSuccessfulPatched, int currentVersion, bool showErrorDialog, Exception source) : base(message, showErrorDialog)
        {
            LastSuccessFulPatchedVersion = lastSuccessfulPatched;
            CurrentVersion = currentVersion;
            SourceException = source;
        }
    }
}
