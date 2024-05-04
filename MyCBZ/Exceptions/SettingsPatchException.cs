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

        public SettingsPatchException(string message, int lastSuccessfulPatched, bool showErrorDialog) : base(message, showErrorDialog)
        {
            LastSuccessFulPatchedVersion = lastSuccessfulPatched;
        }
    }
}
