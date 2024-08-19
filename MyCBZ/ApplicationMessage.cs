using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Win_CBZ.Forms;

namespace Win_CBZ
{
    [SupportedOSPlatform("windows")]
    public class ApplicationMessage
    {

        public enum DialogType : ushort
        {
            MT_INFORMATION = 1,
            MT_WARNING = 2,
            MT_ERROR = 3,
            MT_CONFIRMATION = 4,
            MT_CHECK = 5,
        };

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

        public enum AcceptButtonType : ushort
        {
            NONE = 0,
            ACCEPT_BUTTON = 2,
            CANCEL_BUTTON = 3,
        }

        public static DialogResult Show(String message, String title, DialogType type = DialogType.MT_INFORMATION, DialogButtons buttons = DialogButtons.MB_OK, ScrollBars ShowScrollBars = ScrollBars.None)
        {
            ApplicationDialog errorDialog = new ApplicationDialog();
            errorDialog.Buttons = buttons;
            errorDialog.DialogType = type;
            errorDialog.DialogTitle = title;
            errorDialog.Message = message;
            errorDialog.ShowMessageScrollbars(ShowScrollBars);
            

            return errorDialog.ShowDialog();
        }

        public static ApplicationDialog Create(String message, String title, DialogType type = DialogType.MT_INFORMATION, DialogButtons buttons = DialogButtons.MB_OK, ScrollBars ShowScrollBars = ScrollBars.None)
        {
            ApplicationDialog errorDialog = new ApplicationDialog();
            errorDialog.Buttons = buttons;
            errorDialog.DialogType = type;
            errorDialog.DialogTitle = title;
            errorDialog.Message = message;
            errorDialog.ShowMessageScrollbars(ShowScrollBars);


            return errorDialog;
        }

        public static DialogResult ShowCustom(String message, String title, DialogType type = DialogType.MT_INFORMATION, DialogButtons buttons = DialogButtons.MB_OK, ScrollBars ShowScrollBars = ScrollBars.None, int width = 441, int height = 350)
        {
            ApplicationDialog errorDialog = new ApplicationDialog();
            errorDialog.Buttons = buttons;
            errorDialog.DialogType = type;
            errorDialog.DialogTitle = title;
            errorDialog.Message = message;
            errorDialog.ShowMessageScrollbars(ShowScrollBars);
            errorDialog.Width = width;

            return errorDialog.ShowDialog();
        }

        public static DialogResult ShowWarning(String message, String title, DialogType type = DialogType.MT_WARNING, DialogButtons buttons = DialogButtons.MB_YES | DialogButtons.MB_CANCEL, ScrollBars ShowScrollBars = ScrollBars.None)
        {
            ApplicationDialog errorDialog = new ApplicationDialog();
            errorDialog.Buttons = buttons;
            errorDialog.DialogType = type;
            errorDialog.DialogTitle = title;
            errorDialog.Message = message;
            errorDialog.ShowMessageScrollbars(ShowScrollBars);


            return errorDialog.ShowDialog();
        }

        public static DialogResult ShowConfirmation(String message, String title, DialogType type = DialogType.MT_CONFIRMATION, DialogButtons buttons = DialogButtons.MB_YES | DialogButtons.MB_CANCEL, ScrollBars ShowScrollBars = ScrollBars.None)
        {
            ApplicationDialog errorDialog = new ApplicationDialog();
            errorDialog.Buttons = buttons;
            errorDialog.DialogType = type;
            errorDialog.DialogTitle = title;
            errorDialog.Message = message;
            errorDialog.ShowMessageScrollbars(ShowScrollBars);

            return errorDialog.ShowDialog();
        }

        public static DialogResult ShowError(String message, String title, DialogType type = DialogType.MT_ERROR, DialogButtons buttons = DialogButtons.MB_OK)
        {
            ApplicationDialog errorDialog = new ApplicationDialog();
            errorDialog.Buttons = buttons;
            errorDialog.DialogType = type;
            errorDialog.DialogTitle = title;
            errorDialog.Message = message;

            return errorDialog.ShowDialog();
        }

        public static DialogResult ShowException(Exception exception, DialogType type = DialogType.MT_ERROR, DialogButtons buttons = DialogButtons.MB_OK)
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
