using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Win_CBZ.Data
{
    internal class ParseFilesThreadParams
    {
        public List<StackItem> Stack {  get; set; }
         
        public List<string> FileNamesToAdd {  get; set; }
    }
}
