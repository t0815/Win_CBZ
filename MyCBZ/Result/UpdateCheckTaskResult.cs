using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Win_CBZ.Result
{
    internal class UpdateCheckTaskResult
    {

        public int Status { get; set; } = -1; // -1 = Error, 0 = No Update, 1 = Update Available

        public string Message { get; set; } = "Not Checked";

        public string LatestVersion { get; set; } = string.Empty;

        public string DownloadUrl { get; set; } = string.Empty;

    }
}
