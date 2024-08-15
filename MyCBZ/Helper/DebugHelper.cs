using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;

namespace Win_CBZ.Helper
{
    [SupportedOSPlatform("windows")]
    internal class DebugHelper
    {
        public static void SaveImageToFile(Image image, String name)
        {
            if (image != null)
            {
                image.Save("C:\\_cbz_debug\\" + name);
            }
        }

        public static void SavePageToFile(Page page, String name)
        {
            if (page != null)
            {
                MemoryStream result = page.Serialize("", true);
                using (FileStream writer = new FileStream("C:\\_cbz_debug\\" + page.Name + ".xml", FileMode.Truncate))
                { 
                    result.CopyTo(writer);               
                }
            }
        }
    }
}
