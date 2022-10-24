using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MyCBZ
{
    internal class PathHelper
    {

        public static String ResolvePath(String path)
        {
            
            String resolved = path;
            String source = "";
            String env = "";

            if (path == null)
            {
                throw new ArgumentNullException("path");
            }

            String[] parts = path.Split('\\');

            source = parts.First();
            if (source != null)
            {
                Match test = Regex.Match(source, "(\\%{1}[A-Za-z0-9]{1,}\\%{1})");
                if (test.Success)
                {
                    source = source.Trim('%');
                    env = Environment.GetEnvironmentVariable(source);
                    if (env != null)
                    {
                        resolved = env;
                        foreach (String r in parts.Skip(1))
                        {
                            resolved += "\\" + r;
                        }
                    }
                }
            }

            return resolved;
        }

    }
}
