using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Win_CBZ.Exceptions
{
    class ImageConversionException : ApplicationException
    {

        public Page Page { get; set; }

        public bool IsGlobal { get; set; } 

        public string ImagePath { get; set; }

        public string ControlName { get; set; }

        public ImageConversionException(string message) : base(message, true)
        {
        }

        public ImageConversionException(string message, bool showErrorDialog) : base(message, showErrorDialog)
        {
        }

        public ImageConversionException(string message, bool showErrorDialog, Page page) : base(message, showErrorDialog)
        {
            Page = page;
        }

        public ImageConversionException(string message, bool showErrorDialog, Page page, bool isGlobal, string imagePath, string controlName) : base(message, showErrorDialog)
        {
            Page = page;
            IsGlobal = isGlobal;
            ImagePath = imagePath;
            ControlName = controlName;
        }
    }
}
