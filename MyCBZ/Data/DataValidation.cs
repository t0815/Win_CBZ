using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Win_CBZ.Data
{
    internal class DataValidation
    {

        public event EventHandler<TaskProgressEvent> TaskProgress;

        public DataValidation() 
        {
            
        }

        public string[] ValidateDuplicateStrings(string[] values)
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

                if (occurence > 1 && duplicates.IndexOf(entryA) == -1)
                {
                    duplicates.Add(entryA);
                }
            }

            return duplicates.ToArray();
        }

        public bool ValidateMetaDataInvalidKeys(ref ArrayList metaDataEntryErrors, bool showError = true)
        {
            bool error = false;

            foreach (MetaDataEntry entryA in Program.ProjectModel.MetaData.Values)
            {
                if (Program.ProjectModel.MetaData.ProtectedKeys.IndexOf(entryA.Key.ToLower()) != -1 ||
                    entryA.Key.Length == 0)
                {
                    metaDataEntryErrors.Add(entryA.Key);
                    error = true;
                }
            }

            if (error)
            {
                String lines = string.Join("\r\n", metaDataEntryErrors.ToArray());
                String errorText = string.Join(", ", metaDataEntryErrors.ToArray());

                if (showError)
                {
                    MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_WARNING, "Metadata Validation failed! Invalid Keys [" + errorText + "] found.");
                    DialogResult r = ApplicationMessage.ShowWarning("Metadata Validation failed! The folliwing keys are no allowed:\r\n\r\n" + lines, "Metadata Validation Error", ApplicationMessage.DialogType.MT_WARNING, ApplicationMessage.DialogButtons.MB_OK);

                    
                }
            }

            return error;
        }

        public bool ValidateMetaDataDuplicateKeys(ref ArrayList metaDataEntryErrors, bool showError = true)
        {
            int occurence = 0;
            bool error = false;

            foreach (MetaDataEntry entryA in Program.ProjectModel.MetaData.Values)
            {
                occurence = 0;
                foreach (MetaDataEntry entryB in Program.ProjectModel.MetaData.Values)
                {

                    if (entryA.Key.ToLower().Equals(entryB.Key.ToLower()))
                    {
                        occurence++;
                    }
                }

                if (occurence > 1 && metaDataEntryErrors.IndexOf(entryA.Key) == -1)
                {
                    metaDataEntryErrors.Add(entryA.Key);
                    error = true;
                }
            }

            if (error)
            {
                String lines = string.Join("\r\n", metaDataEntryErrors.ToArray());
                String errorText = string.Join(", ", metaDataEntryErrors.ToArray());

                if (showError)
                {
                    MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_WARNING, "Metadata Validation failed! Duplicate Keys [" + errorText + "] found.");
                    DialogResult r = ApplicationMessage.ShowWarning("Metadata Validation failed! The folliwing keys are duplicated:\r\n\r\n" + lines, "Metadata Validation Error", ApplicationMessage.DialogType.MT_WARNING, ApplicationMessage.DialogButtons.MB_OK | ApplicationMessage.DialogButtons.MB_IGNORE);

                    if (r == DialogResult.Ignore)
                    {
                        error = false;
                    }
                }
            }

            return error;
        }

        public bool ValidateTags(ref ArrayList unknownTagsList, bool showError = true, bool calcProgress = false, int startProgress = 0, int totalProgress = 0)
        {
            bool tagValidationFailed = false;
            int progressIndex = startProgress;
            int overallProgress = totalProgress;

            MetaDataEntry tagEntry = Program.ProjectModel.MetaData.EntryByKey("Tags");
            System.Collections.Specialized.StringCollection validTags = Win_CBZSettings.Default.ValidKnownTags;
            System.Collections.Specialized.StringCollection validTagsLcase = new System.Collections.Specialized.StringCollection();
            //ArrayList unknownTags = new ArrayList();

            if (unknownTagsList == null)
            {
                unknownTagsList = new ArrayList();
            }

            if (Win_CBZSettings.Default.TagValidationIgnoreCase) {
                String[] bufferStrings = new string[validTags.Count];
                validTags.CopyTo(bufferStrings, 0);
                validTagsLcase.AddRange(bufferStrings.Select(s => s.ToLower()).ToArray());
            }

            if (tagEntry != null && tagEntry.Value != null && tagEntry.Value.Length > 0 && validTags.Count > 0)
            {
                String[] tags = new String[1];
                if (Win_CBZSettings.Default.TagValidationIgnoreCase)
                {
                    tags = tagEntry.Value.Split(',').Select(s => s.Trim().ToLower()).ToArray();
                } else
                {
                    tags = tagEntry.Value.Split(',').Select(s => s.Trim()).ToArray();
                }

                foreach (String tag in tags)
                {
                    if (Win_CBZSettings.Default.TagValidationIgnoreCase)
                    {
                        if (!validTagsLcase.Contains(tag.ToLower()))
                        {
                            unknownTagsList.Add(tag);
                        }
                    }
                    else
                    {
                        if (!validTags.Contains(tag))
                        {
                            unknownTagsList.Add(tag);
                        }
                    }

                    OnTaskProgress(new TaskProgressEvent(null, progressIndex, overallProgress));

                    progressIndex++;
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
                    DialogResult r = ApplicationMessage.ShowWarning("Tag Validation failed! The folliwing tags where not found in known list of tags:\r\n\r\n" + lines, "Tag Validation Error", ApplicationMessage.DialogType.MT_WARNING, ApplicationMessage.DialogButtons.MB_OK | ApplicationMessage.DialogButtons.MB_IGNORE);

                    if (r == DialogResult.Ignore)
                    {
                        tagValidationFailed = false;
                    }
                }
            }

            return tagValidationFailed;
        }

        public bool ValidateTags(bool showError = true)
        {
            ArrayList unknownTagList = new ArrayList();

            return this.ValidateTags(ref unknownTagList, showError);
        }

        public bool ValidateCBZ(bool showError = true) 
        {
            MetaData metaData = Program.ProjectModel.MetaData;

            
            return true;

        }

        protected virtual void OnTaskProgress(TaskProgressEvent e)
        {
            TaskProgress?.Invoke(this, e);
        }
    }
}
