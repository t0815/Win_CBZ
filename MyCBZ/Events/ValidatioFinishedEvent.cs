using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Win_CBZ
{
    internal class ValidationFinishedEvent
    {

        public string[] ValidationErrors;

        public bool ShowErrorsDialog = false;

        public ValidationFinishedEvent(string[] validationErrors, bool showError = false) 
        {
            ValidationErrors = validationErrors;
            ShowErrorsDialog = showError;
        }
    }
}
