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

        protected MetaData.PageIndexVersion MetaWriteVersion;


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

        public MetaDataVersionFlavorHandler SetTargetVersion(PageIndexVersion version)
        {
            MetaWriteVersion = version;

            return this;
        }

        public MetaData.PageIndexVersion HandlePageIndexVersion(bool onlyTarget = false)
        {

            MetaVersion = Program.ProjectModel.MetaData.IndexVersionSpecification;
            MetaWriteVersion = TargetVersion();

            if (MetaWriteVersion != Program.ProjectModel.MetaData.IndexVersionSpecification)
            {
                MetaVersion = Program.ProjectModel.MetaData.IndexVersionSpecification;
            }

            return MetaVersion;
        }

        public MetaData.PageIndexVersion TargetVersion()
        {
            MetaVersionSetting = Win_CBZSettings.Default.MetaDataPageIndexVersionToWrite;
            
            switch (MetaVersionSetting)
            {
                case 1:
                    MetaWriteVersion = PageIndexVersion.VERSION_1;
                    break;
                case 2:
                    MetaWriteVersion = PageIndexVersion.VERSION_2;
                    break;
                default:
                    MetaWriteVersion = PageIndexVersion.VERSION_1;
                    break;
            }

            return MetaWriteVersion;
        }

        public MetaDataEntryPage CreateIndexEntry(Page page)
        {
            MetaDataEntryPage newPageEntry = new MetaDataEntryPage();

            if (MetaWriteVersion.HasFlag(PageIndexVersion.VERSION_2))
            {
                newPageEntry.SetAttribute(MetaDataEntryPage.COMIC_PAGE_ATTRIBUTE_IMAGE, page.Name)
                    .SetAttribute(MetaDataEntryPage.COMIC_PAGE_ATTRIBUTE_KEY, page.Key);
            }
            else if (MetaWriteVersion.HasFlag(PageIndexVersion.VERSION_1))
            {
                newPageEntry.SetAttribute(MetaDataEntryPage.COMIC_PAGE_ATTRIBUTE_IMAGE, page.Number.ToString())
                    .SetAttribute(MetaDataEntryPage.COMIC_PAGE_ATTRIBUTE_KEY, page.Name);
            }

            return newPageEntry;
        }
    }
}
