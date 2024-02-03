using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Win_CBZ.MetaData;

namespace Win_CBZ.Helper
{
    internal class MetaDataVersionFlavorHandler
    {
        
        protected static MetaDataVersionFlavorHandler Instance;

        protected int MetaVersionSetting;

        protected MetaData.PageIndexVersion MetaVersion;


        protected MetaDataVersionFlavorHandler() 
        {
            
        }

        public static MetaDataVersionFlavorHandler GetInstance()
        {
            if (MetaDataVersionFlavorHandler.Instance == null)
            {
                MetaDataVersionFlavorHandler.Instance = new MetaDataVersionFlavorHandler();
            }

            //MetaDataVersionFlavorHandler.Instance.HandlePageIndexVersion();

            return MetaDataVersionFlavorHandler.Instance;
        }

        public MetaData.PageIndexVersion HandlePageIndexVersion()
        {
            MetaVersionSetting = Win_CBZSettings.Default.MetaDataPageIndexVersionToWrite;
            MetaVersion = PageIndexVersion.VERSION_1;

            switch (MetaVersionSetting)
            {
                case 1:
                    MetaVersion = PageIndexVersion.VERSION_1;
                    break;
                case 2:
                    MetaVersion = PageIndexVersion.VERSION_2;
                    break;
                default:
                    MetaVersion = PageIndexVersion.VERSION_1;
                    break;
            }

            if (MetaVersion != Program.ProjectModel.MetaData.IndexVersionSpecification)
            {
                MetaVersion = Program.ProjectModel.MetaData.IndexVersionSpecification;
            }

            return MetaVersion;
        }

        public MetaDataEntryPage CreateIndexEntry(Page page)
        {
            MetaDataEntryPage newPageEntry = new MetaDataEntryPage();

            if (MetaVersion.HasFlag(PageIndexVersion.VERSION_1))
            {
                newPageEntry.SetAttribute(MetaDataEntryPage.COMIC_PAGE_ATTRIBUTE_IMAGE, page.Name)
                    .SetAttribute(MetaDataEntryPage.COMIC_PAGE_ATTRIBUTE_KEY, page.Key);
            }
            else if (MetaVersion.HasFlag(PageIndexVersion.VERSION_2))
            {
                newPageEntry.SetAttribute(MetaDataEntryPage.COMIC_PAGE_ATTRIBUTE_IMAGE, page.Number.ToString())
                    .SetAttribute(MetaDataEntryPage.COMIC_PAGE_ATTRIBUTE_KEY, page.Name);
            }

            return newPageEntry;
        }
    }
}
