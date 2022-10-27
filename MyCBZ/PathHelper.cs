using System;
using System.Collections.Generic;
using System.Configuration;
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
            
            String resolved = "";
            String env = "";
            String cnf = "";

            if (path == null)
            {
                throw new ArgumentNullException("path");
            }

            String[] parts = path.Split('\\');

            foreach (String part in parts)
            {
                if (part.Length > 0)
                {
                    // check for placeholders in format %NAME%
                    Match test = Regex.Match(part, "(\\%{1}[A-Za-z0-9]{1,}\\%{1})");
                    if (test.Success)
                    {
                        env = Environment.GetEnvironmentVariable(part.Trim('%'));
                        if (env != null)
                        {
                            resolved += "\\" + env;
                        }
                        else
                        {
                            try
                            {
                                cnf = Win_CBZSettings.Default[part.Trim('%')].ToString();
                                resolved += "\\" + cnf;
                            } catch (SettingsPropertyNotFoundException)
                            {
                                resolved += "\\" + part;
                            }
                        }
                    } else
                    {
                        resolved += "\\" + part;
                    }                 
                }
            }

            if (resolved.Length == 0)
            {
                resolved = path;
            }

            return resolved.TrimStart('\\') + "\\";
        }
    }
}
