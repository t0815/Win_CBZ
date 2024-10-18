using Microsoft.IdentityModel.Protocols.WsTrust;
using SharpCompress.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using Win_CBZ.Events;
using static Win_CBZ.Handler.AppEventHandler;

namespace Win_CBZ.Data
{
    [SupportedOSPlatform("windows")]
    internal class DataValidation
    {

        public TaskProgressDelegate OnTaskProgress { get; set; }    

        public DataValidation() 
        {
            
        }

        public bool ValidateItemExists(string itemName, string[] values, bool ignoreCase = true)
        {
            if (itemName == null)
            {
                return false;
            }

            if (ignoreCase)
            {
                return values.Any(x => x.ToLower().Equals(itemName.ToLower()));
            }
            else
            {
                return values.Any(x => x.Equals(itemName));
            }
        }

        public int ValidateItemOccurence(string itemName, string[] values, bool ignoreCase = true)
        {
            if (itemName == null)
            {
                return 0;
            }

            if (ignoreCase)
            {
                return values.Where((s, i) => s.ToLower().Equals(itemName.ToLower())).Count();
            }
            else
            {
                return values.Where((s, i) => s.Equals(itemName)).Count();
            }
        }

        public string[] ValidateDuplicateStrings(string[] values, CancellationToken ?cancellationToken = null)
        {
            List<String> duplicates = new List<String>();

            string pattern = @"(?<=,|^)([^,]+)(?=(?>,[^,]*)*,\1(?>,|$)),";
            string input = String.Join(',', values);
            foreach (Match match in Regex.Matches(input, pattern, RegexOptions.IgnoreCase))
            {
                duplicates.Add(match.Groups[1].Value);
            }
                
            /*
             *  Use regex for duplicate detection

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

                    if (cancellationToken != null)
                    {
                        if (cancellationToken.Value.IsCancellationRequested)
                        {
                            break;
                        }
                    }
                }

                if (occurence > 1 && duplicates.IndexOf(entryA) == -1)
                {
                    duplicates.Add(entryA);
                }

                if (cancellationToken != null)
                {
                    if (cancellationToken.Value.IsCancellationRequested)
                    {
                        break;
                    }
                }
            }
            */

            return duplicates.ToArray();
        }

        public bool ValidateMetaDataInvalidKeys(ref ArrayList metaDataEntryErrors, bool showError = true, CancellationToken? cancellationToken = null)
        {
            bool error = false;

            foreach (MetaDataEntry entry in Program.ProjectModel.MetaData.Values)
            {
                try
                {
                    if (Program.ProjectModel.MetaData.ProtectedKeys.IndexOf(entry.Key.ToLower()) != -1)
                    {
                        throw new MetaDataValidationException(entry, null, "Key with name ['" + entry.Key + "'] is not allowed!", showError);
                    }

                    //Regex re = Regex.("/[a-z]+$/gi");

                    if (entry != null && !Regex.IsMatch(entry.Key, @"^[a-z]+$", RegexOptions.IgnoreCase))
                    {
                        throw new MetaDataValidationException(entry, null, "Key with name ['" + entry.Key + "'] must contain only values between ['a-zA-Z']!", showError);
                    }

                  
                    if (cancellationToken != null)
                    {
                        if (cancellationToken.Value.IsCancellationRequested)
                        {
                            break;
                        }
                    }
                }
                catch (MetaDataValidationException ex)
                {
                    metaDataEntryErrors.Add(ex.Message);
                    error = true;
                }
            }

            string[] duplicates = ValidateDuplicateStrings(Program.ProjectModel.MetaData.Values.ToArray().Select(s => s.Key).ToArray());

            if (duplicates.Length > 0)
            {
                metaDataEntryErrors.Add("Duplicate keys ['" + String.Join(",", duplicates) + "'] not allowed!");
                error = true;
            }

            if (error)
            {
                String lines = string.Join("\r\n", metaDataEntryErrors.ToArray());
                

                if (showError)
                {
                    MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_WARNING, "Metadata validation failed!");

                    foreach (String errorText in metaDataEntryErrors)
                    {
                        MessageLogger.Instance.Log(LogMessageEvent.LOGMESSAGE_TYPE_WARNING, errorText);
                    }
                                       
                    DialogResult r = ApplicationMessage.ShowWarning("Metadata validation failed!\r\nThe following keys are not valid:\r\n\r\n" + lines, "Metadata Validation Error", ApplicationMessage.DialogType.MT_WARNING, ApplicationMessage.DialogButtons.MB_OK);                  
                }
            }

            return error;
        }

        public bool ValidateMetaDataDuplicateKeys(ref ArrayList metaDataEntryErrors, bool showError = true, CancellationToken? cancellationToken = null)
        {
            //int occurence = 0;
            bool error = false;
            List<String> duplicates = new List<String>();

            string[] values = Program.ProjectModel.MetaData.Values.Select(x => x.Key).ToArray();

            string pattern = @"(?<=,|^)([^,]+)(?=(?>,[^,]*)*,\1(?>,|$)),";
            string input = String.Join(',', values);
            foreach (Match match in Regex.Matches(input, pattern, RegexOptions.IgnoreCase))
            {
                duplicates.Add(match.Groups[1].Value);
            }

            /*
            foreach (MetaDataEntry entryA in Program.ProjectModel.MetaData.Values)
            {
                occurence = 0;
                foreach (MetaDataEntry entryB in Program.ProjectModel.MetaData.Values)
                {

                    if (entryA.Key.ToLower().Equals(entryB.Key.ToLower()))
                    {
                        occurence++;
                    }

                    if (cancellationToken != null)
                    {
                        if (cancellationToken.Value.IsCancellationRequested)
                        {
                            break;
                        }
                    }
                }

                if (occurence > 1 && metaDataEntryErrors.IndexOf(entryA.Key) == -1)
                {
                    metaDataEntryErrors.Add(entryA.Key);
                    error = true;
                }

                if (cancellationToken != null)
                {
                    if (cancellationToken.Value.IsCancellationRequested)
                    {
                        break;
                    }
                }
            }
            */

            if (duplicates.Count > 0)
            {
                error = true;

                metaDataEntryErrors.AddRange(duplicates);

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

        public bool ValidateTags(ref ArrayList unknownTagsList, bool showError = true, bool calcProgress = false, int startProgress = 0, int totalProgress = 0, CancellationToken? cancellationToken = null)
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

                    if (cancellationToken != null)
                    {
                        if (cancellationToken.Value.IsCancellationRequested)
                        {
                            break;
                        }
                    }

                    OnTaskProgress?.Invoke(this, new TaskProgressEvent(null, progressIndex, overallProgress));

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

        
    }
}
