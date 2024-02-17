using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Win_CBZ.Helper
{
    internal class DebugHelper
    {
        public static void saveImageToFile(Image image, String name)
        {
            if (image != null)
            {
                image.Save("C:\\_cbz_debug\\" + name);
            }
        }
    }
}
