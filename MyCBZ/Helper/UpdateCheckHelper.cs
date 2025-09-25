using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Win_CBZ.Events;
using Win_CBZ.Forms;
using Win_CBZ.Handler;
using Win_CBZ.Result;
using Win_CBZ.Tasks;

namespace Win_CBZ.Helper
{

    [SupportedOSPlatform("windows")]
    internal class UpdateCheckHelper
    {

        public static void CheckForUpdates(Control sender, bool silent = false)
        {
            var urls = new List<string>() {
                "https://raw.githubusercontent.com/t0815/win_cbz/master/version.xml" ,
                "https://raw.githubusercontent.com/t0815/Win_CBZ/refs/heads/master/version.xml"
            };

            Task<UpdateCheckTaskResult> updateCheckTask = UpdateCheckTask.CheckForUpdates(urls, false, AppEventHandler.OnGeneralTaskProgress, TokenStore.GetInstance().RequestCancellationToken(TokenStore.TOKEN_SOURCE_AWAIT_THREADS));

            updateCheckTask.ContinueWith(r =>
            {
                sender.Invoke(new Action(() =>
                {
                    if (r.Result.Status)
                    {

                        try
                        {
                            if (!r.Result.IsNewerVersion && r.Result.Silent)
                            {
                                return;
                            }

                            UpdateCheckForm updateCheckForm = new UpdateCheckForm(r.Result);

                            DialogResult res = updateCheckForm.ShowDialog();
                           

                        }
                        catch (Exception ex)
                        {
                            ApplicationMessage.Show("An error occurred while checking for updates!\r\n" + ex.Message, "Error", ApplicationMessage.DialogType.MT_ERROR, ApplicationMessage.DialogButtons.MB_OK);
                        }
                        finally
                        {

                        }
                    }
                    else
                    {
                        ApplicationMessage.Show("An error occurred while checking for updates!\r\n" + r.Exception?.Message, "Error", ApplicationMessage.DialogType.MT_ERROR, ApplicationMessage.DialogButtons.MB_OK);

                    }

                    AppEventHandler.OnApplicationStateChanged(sender, new ApplicationStatusEvent(Program.ProjectModel, ApplicationStatusEvent.STATE_READY));

                }));


            });

            updateCheckTask.Start();
        }
    }
}
