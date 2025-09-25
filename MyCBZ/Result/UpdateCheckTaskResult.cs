using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Win_CBZ.Result
{
    public class UpdateCheckTaskResult
    {

        public bool Status { get; set; }

        public string Message { get; set; } = "";

        public string Title { get; set; } = string.Empty;

        public List<string> Changes { get; set; } = new List<string>();

        public string LatestVersion { get; set; } = string.Empty;

        public bool Silent { get; set; } = false;

        public bool IsNewerVersion { get; set; } = false;

        public string DownloadUrl { get; set; } = string.Empty;

    }
}
