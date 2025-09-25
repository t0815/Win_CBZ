using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Win_CBZ.Extensions;
using Win_CBZ.Result;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Win_CBZ.Forms
{
    public partial class UpdateCheckForm : Form
    {
        [SupportedOSPlatform("windows")]
        public UpdateCheckForm(UpdateCheckTaskResult updateCheckTaskResult)
        {
            InitializeComponent();

            StringBuilder messageBuilder = new StringBuilder(200);

            messageBuilder.Append("Changelog:");
            messageBuilder.Append(Environment.NewLine);
            messageBuilder.Append(Environment.NewLine);

            updateCheckTaskResult.Changes.Each(change => messageBuilder.AppendLine($"\u2022 {change}"));

            if (updateCheckTaskResult.IsNewerVersion)
            {
                this.Text = "Update Available";
                LabelUpdateHeadline.Text = updateCheckTaskResult.Title;
                TextBoxChanges.Text = messageBuilder.ToString();
                LabelAppVersion.Text = Application.ProductVersion;
                LabelNewVersion.Text = updateCheckTaskResult.LatestVersion;
                LabelNewVersion.Font = new Font(LabelNewVersion.Font, FontStyle.Bold);
                LabelUrl.Text = updateCheckTaskResult.DownloadUrl;
                LabelUrl.Links.Add(0, updateCheckTaskResult.DownloadUrl.Length, updateCheckTaskResult.DownloadUrl);
                label4.Visible = true;
            }
            else
            {
                this.Text = "No Update Available";
                LabelUpdateHeadline.Text = "You are using the latest version.";
                LabelAppVersion.Text = Application.ProductVersion;
                LabelNewVersion.Text = updateCheckTaskResult.LatestVersion;
                TextBoxChanges.Visible = false;
                LabelUrl.Visible = false;
                label4.Visible = false;
            }

        }

        private void OkButton_Click(object sender, EventArgs e)
        {

        }

        private void LabelUrl_Click(object sender, EventArgs e)
        {
            Process.Start(new ProcessStartInfo(LabelUrl.Links[0].LinkData.ToString()) { UseShellExecute = true });
        }
    }
}
