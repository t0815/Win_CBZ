using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Win_CBZ.Data
{
    public class Validation
    {

        public static string[] hasDuplicates(string[] values)
        {
            List<String> duplicates = new List<String>();

            int occurence = 0;
            foreach (String entryA in values)
            {
                occurence = 0;
                foreach (String entryB in values)
                {
                    if (entryA.ToLower().Equals(entryB.ToLower()))
                    {
                        occurence++;
                    }
                }

                if (occurence > 1)
                {
                    duplicates.Add(entryA);
                }
            }

            return duplicates.ToArray();
        }
    }
}
