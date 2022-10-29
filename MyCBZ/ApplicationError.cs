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
    public class ApplicationError
    {
        public const short MT_INFORMATION = 1;
        public const short MT_WARNING = 2;
        public const short MT_ERROR = 3;

        public enum DialogButtons : ushort
        {
            MB_OK,
            MB_CANCEL,
            MB_YES,
            MB_NO,
            MB_ABORT,
            MB_RETRY,
            MB_IGNORE,
        };

        public static DialogResult Show(String message, String title, short type = MT_INFORMATION, DialogButtons buttons = DialogButtons.MB_OK & DialogButtons.MB_CANCEL)
        {
            ErrorDialog errorDialog = new ErrorDialog();
            errorDialog.Buttons = buttons;
            errorDialog.DialogType = type;
            errorDialog.DialogTitle = title;
            errorDialog.Message = message;

            return errorDialog.ShowDialog();
        }

        public static DialogResult ShowException(Exception exception, short type = MT_WARNING, DialogButtons buttons = DialogButtons.MB_OK & DialogButtons.MB_CANCEL)
        {
            ErrorDialog errorDialog = new ErrorDialog();
            errorDialog.Buttons = buttons;
            errorDialog.DialogType = type;
            errorDialog.DialogTitle = exception.GetType().Name;
            errorDialog.Message = exception.ToString();           

            return errorDialog.ShowDialog();
        }
    }
}
