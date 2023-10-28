using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Win_CBZ.Data
{
    public class DataValidation
    {

        public static string[] validateDuplicateStrings(string[] values)
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

        public static bool validateTags(ref ArrayList unknownTagsList, bool showError = true)
        {
            bool tagValidationFailed = false;

            MetaDataEntry tagEntry = Program.ProjectModel.MetaData.EntryByKey("Tags");
            System.Collections.Specialized.StringCollection validTags = Win_CBZSettings.Default.ValidKnownTags;
            //ArrayList unknownTags = new ArrayList();

            if (unknownTagsList == null)
            {
                unknownTagsList = new ArrayList();
            }

            if (tagEntry != null && validTags.Count > 0)
            {
                String[] tags = tagEntry.Value.Split(',').Select(s => s.Trim()).ToArray();
                foreach (String tag in tags)
                {
                    if (!validTags.Contains(tag))
                    {
                        unknownTagsList.Add(tag);
                    }
                }
            }
            else
            {
                MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_INFO, "Tag Validation: No Tags to validate.");
            }

            if (unknownTagsList.Count > 0)
            {
                String lines = string.Join("\r\n", unknownTagsList.ToArray());
                String errorText = string.Join(", ", unknownTagsList.ToArray());
                tagValidationFailed = true;

                if (showError)
                {
                    MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_WARNING, "Tag Validation: Failed to validate Tags. Invalid Tags [" + errorText + "] found.");
                    DialogResult r = ApplicationMessage.ShowWarning("Tag Validation failed! The folliwing tags where not found in known list of tags:\r\n\r\n" + lines, "Tag Validation Error", 2, ApplicationMessage.DialogButtons.MB_OK | ApplicationMessage.DialogButtons.MB_IGNORE);

                    if (r == DialogResult.Ignore)
                    {
                        tagValidationFailed = false;
                    }
                }
            }

            return tagValidationFailed;
        }

        public static bool validateTags(bool showError = true)
        {
            ArrayList unknownTagList = new ArrayList();

            return DataValidation.validateTags(ref unknownTagList, showError);
        }

        public static bool validateCBZ(bool showError = true) 
        {
            MetaData metaData = Program.ProjectModel.MetaData;

            


            return true;

        }
    }
}
