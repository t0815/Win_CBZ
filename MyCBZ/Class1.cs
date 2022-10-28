using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Win_CBZ.Forms;

namespace Win_CBZ
{
    internal class ApplicationError
    {

        public enum DialogButtons : ushort
        {
            MB_OK,
            MB_CANCEL,
            MB_YES,
            MB_NO,
        };

        public static DialogResult show(String message, String title, DialogButtons buttons)
        {
            ErrorDialog errorDialog = new ErrorDialog();

            return errorDialog.ShowDialog();
        }

        public static DialogResult show(Exception exception)
        {
            ErrorDialog errorDialog = new ErrorDialog();

            return errorDialog.ShowDialog();
        }
    }
}
