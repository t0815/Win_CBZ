using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Win_CBZ
{
    internal class PathHelper
    {

        public static String ResolvePath(String path)
        {
            
            List<String> resolved = new List<string>();
            String env = "";
            String cnf = "";

            if (path == null)
            {
                throw new ArgumentNullException("PathHelper::ResolvePath invalid argument path");
            }

            String[] parts = path.Split(Path.DirectorySeparatorChar);

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
                            resolved.Add(env);
                        }
                        else
                        {
                            try
                            {
                                cnf = Win_CBZSettings.Default[part.Trim('%')].ToString();
                                resolved.Add(cnf);
                            } catch (SettingsPropertyNotFoundException)
                            {
                                resolved.Add(part);
                            }
                        }
                    } else
                    {
                        resolved.Add(part);
                    }                 
                }
            }

            if (resolved.Count == 0)
            {
                return path;
            }

            // Path.combine() does not work here!!! It will omit backslash from drive letter...
            return String.Join(new String(Path.DirectorySeparatorChar, 1), resolved.ToArray());
        }
    }
}
