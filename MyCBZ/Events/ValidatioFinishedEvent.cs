
namespace Win_CBZ
{
    internal class ValidationFinishedEvent
    {

        public string[] ValidationErrors { get; set; }

        public bool ShowErrorsDialog { get; set; } = false;

        public ValidationFinishedEvent(string[] validationErrors, bool showError = false) 
        {
            ValidationErrors = validationErrors;
            ShowErrorsDialog = showError;
        }
    }
}
