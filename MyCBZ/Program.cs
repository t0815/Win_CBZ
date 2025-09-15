using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Versioning;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Win_CBZ
{
    [SupportedOSPlatform("windows")]
    internal static class Program
    {

        public static ProjectModel ProjectModel { get; set; }

        public static bool DebugMode { get; set; } = false;

        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            try
            {
                Application.Run(new MainForm());
            }
            catch (Exception ex)
            {
                MessageBox.Show("An unhandled exception occured:\n" + ex.Message + "\n\nat: " + ex.Source + "\r\nLine: " + ex.StackTrace + "\r\nApplication will terminate.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
