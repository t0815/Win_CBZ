using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Win_CBZ.Forms;

namespace Win_CBZ
{
    public class ApplicationMessage
    {
        public const short MT_INFORMATION = 1;
        public const short MT_WARNING = 2;
        public const short MT_ERROR = 3;
        public const short MT_CONFIRMATION = 4;

        [Flags]
        public enum DialogButtons : ushort
        {
            MB_OK = 1,
            MB_CANCEL = 2,
            MB_YES = 4,
            MB_NO = 8,
            MB_ABORT = 16,
            MB_RETRY = 32,
            MB_IGNORE = 64,
            MB_QUIT = 128
        };

        public static DialogResult Show(String message, String title, short type = MT_INFORMATION, DialogButtons buttons = DialogButtons.MB_OK | DialogButtons.MB_CANCEL)
        {
            ApplicationDialog errorDialog = new ApplicationDialog();
            errorDialog.Buttons = buttons;
            errorDialog.DialogType = type;
            errorDialog.DialogTitle = title;
            errorDialog.Message = message;

            return errorDialog.ShowDialog();
        }

        public static DialogResult ShowWarning(String message, String title, short type = MT_WARNING, DialogButtons buttons = DialogButtons.MB_YES | DialogButtons.MB_CANCEL)
        {
            ApplicationDialog errorDialog = new ApplicationDialog();
            errorDialog.Buttons = buttons;
            errorDialog.DialogType = type;
            errorDialog.DialogTitle = title;
            errorDialog.Message = message;

            return errorDialog.ShowDialog();
        }

        public static DialogResult ShowConfirmation(String message, String title, short type = MT_CONFIRMATION, DialogButtons buttons = DialogButtons.MB_YES | DialogButtons.MB_CANCEL)
        {
            ApplicationDialog errorDialog = new ApplicationDialog();
            errorDialog.Buttons = buttons;
            errorDialog.DialogType = type;
            errorDialog.DialogTitle = title;
            errorDialog.Message = message;

            return errorDialog.ShowDialog();
        }

        public static DialogResult ShowException(Exception exception, short type = MT_WARNING, DialogButtons buttons = DialogButtons.MB_OK | DialogButtons.MB_CANCEL)
        {
            ApplicationDialog errorDialog = new ApplicationDialog();
            errorDialog.Buttons = buttons;
            errorDialog.DialogType = type;
            errorDialog.DialogTitle = exception.GetType().Name;
            errorDialog.Message = exception.ToString();           

            return errorDialog.ShowDialog();
        }
    }
}
