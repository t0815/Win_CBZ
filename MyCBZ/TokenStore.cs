using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Win_CBZ.Helper;

namespace Win_CBZ
{
    internal class TokenStore
    {
        public const string TOKEN_SOURCE_GLOBAL = "global";
        public const string TOKEN_SOURCE_LOAD_ARCHIVE = "loadarchive";
        public const string TOKEN_SOURCE_CLOSE_ARCHIVE = "closearchive";
        public const string TOKEN_SOURCE_EXTRACT_ARCHIVE = "extractarchive";
        public const string TOKEN_SOURCE_SAVE_ARCHIVE = "savearchive";
        public const string TOKEN_SOURCE_DELETE_FILE = "deletefile";
        public const string TOKEN_SOURCE_UPDATE_PAGE_INDEX = "updatepageindex";
        public const string TOKEN_SOURCE_UPDATE_IMAGE_METADATA = "updateimagemetadata";
        public const string TOKEN_SOURCE_REBUILD_XML_INDEX = "rebuildxmlindex";
        public const string TOKEN_SOURCE_UPDATE_PAGE = "pageupdate";
        public const string TOKEN_SOURCE_UPDATE_IMAGE = "imageupdate";
        public const string TOKEN_SOURCE_PROCESS_FILES = "processaddedfiles";
        public const string TOKEN_SOURCE_PARSE_FILES = "parsefilenames";
        public const string TOKEN_SOURCE_RENAME = "rename";
        public const string TOKEN_SOURCE_AUTO_RENAME = "autorename";
        public const string TOKEN_SOURCE_RESTORE_RENAMING = "restorerenaming";
        public const string TOKEN_SOURCE_CBZ_VALIDATION = "archivevalidation";
        public const string TOKEN_SOURCE_THUMBNAIL_SLICE = "thumbnailslice";
        public const string TOKEN_SOURCE_THUMBNAIL = "thumbnail";
        public const string TOKEN_SOURCE_MOVE_ITEMS = "moveitems";
        public const string TOKEN_SOURCE_UPDATE_PAGE_VIEW = "updatepageview";
        public const string TOKEN_SOURCE_AWAIT_THREADS = "awaitthreads";
        public const string TOKEN_SOURCE_UPDATE_PAGES_SETTINGS = "updatepagessettings";

        private static TokenStore Instance;

        private CancellationTokenSource DefaultSource;

        private Dictionary<string, CancellationTokenSource> CancellationTokenStore;

        private Dictionary<string, Tuple<string, bool>> DefaultCancellationTokens = new Dictionary<string, Tuple<string, bool>>()
        {
           {TOKEN_SOURCE_GLOBAL, Tuple.Create("", false) },
           {TOKEN_SOURCE_LOAD_ARCHIVE, Tuple.Create(TOKEN_SOURCE_GLOBAL, true) },
           {TOKEN_SOURCE_CLOSE_ARCHIVE, Tuple.Create(TOKEN_SOURCE_GLOBAL, true) },
           {TOKEN_SOURCE_EXTRACT_ARCHIVE, Tuple.Create(TOKEN_SOURCE_GLOBAL, true) },
           {TOKEN_SOURCE_SAVE_ARCHIVE, Tuple.Create(TOKEN_SOURCE_GLOBAL, true) },
           {TOKEN_SOURCE_DELETE_FILE, Tuple.Create(TOKEN_SOURCE_GLOBAL, true) },
           {TOKEN_SOURCE_UPDATE_PAGE, Tuple.Create(TOKEN_SOURCE_GLOBAL, true) },
           {TOKEN_SOURCE_PROCESS_FILES, Tuple.Create(TOKEN_SOURCE_GLOBAL, true) },
           {TOKEN_SOURCE_PARSE_FILES, Tuple.Create(TOKEN_SOURCE_GLOBAL, true) },
           {TOKEN_SOURCE_RENAME, Tuple.Create(TOKEN_SOURCE_GLOBAL, true) },
           {TOKEN_SOURCE_AUTO_RENAME, Tuple.Create(TOKEN_SOURCE_GLOBAL, true) },
           {TOKEN_SOURCE_RESTORE_RENAMING, Tuple.Create(TOKEN_SOURCE_GLOBAL, true) },
           {TOKEN_SOURCE_CBZ_VALIDATION, Tuple.Create(TOKEN_SOURCE_GLOBAL, true) },
           {TOKEN_SOURCE_UPDATE_IMAGE, Tuple.Create(TOKEN_SOURCE_GLOBAL, true) },
           {TOKEN_SOURCE_THUMBNAIL_SLICE, Tuple.Create(TOKEN_SOURCE_GLOBAL, true) },
           {TOKEN_SOURCE_MOVE_ITEMS, Tuple.Create(TOKEN_SOURCE_GLOBAL, true) },
           {TOKEN_SOURCE_UPDATE_PAGE_VIEW, Tuple.Create(TOKEN_SOURCE_GLOBAL, true) },
           {TOKEN_SOURCE_UPDATE_PAGE_INDEX, Tuple.Create(TOKEN_SOURCE_GLOBAL, true) },
           {TOKEN_SOURCE_UPDATE_IMAGE_METADATA, Tuple.Create(TOKEN_SOURCE_GLOBAL, true) },
           {TOKEN_SOURCE_REBUILD_XML_INDEX, Tuple.Create(TOKEN_SOURCE_GLOBAL, true) },
           {TOKEN_SOURCE_AWAIT_THREADS, Tuple.Create(TOKEN_SOURCE_GLOBAL, true) },
           {TOKEN_SOURCE_UPDATE_PAGES_SETTINGS, Tuple.Create(TOKEN_SOURCE_GLOBAL, true) },
        };


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public CancellationToken RequestCancellationToken(string name)
        {
            CancellationTokenSource resultSource;

            if (CancellationTokenStore.TryGetValue(name, out resultSource))
            {
                return resultSource.Token;
            }

            if (CancellationTokenStore.TryGetValue("global", out resultSource))
            {
                return resultSource.Token;
            }

            return DefaultSource.Token;
        }

        public CancellationTokenSource CancellationTokenSourceForName(string name)
        {
            CancellationTokenSource resultSource;

            if (CancellationTokenStore.TryGetValue(name, out resultSource))
            {
                return resultSource;
            }

            if (CancellationTokenStore.TryGetValue("global", out resultSource))
            {
                return resultSource;
            }

            return DefaultSource;
        }

        public CancellationTokenSource AddNewCancellationToken(string name, string linkTo = null)
        {
            CancellationTokenSource linkSource;
            CancellationTokenSource newSource = null;
            if (linkTo != null)
            {
                if (CancellationTokenStore.TryGetValue(linkTo, out linkSource))
                {
                    newSource = CancellationTokenSource.CreateLinkedTokenSource(linkSource.Token);
                }
            }

            if (newSource == null)
            {
                newSource = new CancellationTokenSource();
            }

            if (!CancellationTokenStore.ContainsKey(name))
            {
                CancellationTokenStore.Add(name, newSource);
            }
            
            return newSource;
        }

        public bool ResetCancellationToken(string name, string link = null)
        {
            if (CancellationTokenStore.ContainsKey(name))
            {
                CancellationTokenStore.Remove(name);
                AddNewCancellationToken(name, link);
            }



            return true;
        }

        public void ResetCancellationTokens()
        {

        }

        /// <summary>
        /// Creates predifined tokens
        /// </summary>
        /// <returns></returns>
        public void Make()
        {
            if (CancellationTokenStore == null)
            {
                throw new ApplicationException("Error! TokenStore- Provider not initialized. Call 'GetInstance()' first!", false);
            }

            string name = string.Empty;
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            CancellationTokenSource linkedSource;
            foreach (var x in DefaultCancellationTokens)
            {
                name = x.Key;
                if (x.Value.Item1.Length > 0 && x.Value.Item2)
                {
                    if (CancellationTokenStore.ContainsKey(x.Value.Item1))
                    {
                        cancellationTokenSource = AddNewCancellationToken(name, x.Value.Item1);
                    }
                }

                if (!CancellationTokenStore.ContainsKey(name))
                {
                    AddNewCancellationToken(name);
                }
            }
        }

        public static TokenStore GetInstance()
        {
            TokenStore.Instance ??= new TokenStore();
            
            return TokenStore.Instance;
        }

        private TokenStore()
        {
            CancellationTokenStore = new Dictionary<string, CancellationTokenSource>();
            Make();
        }

    }
}
