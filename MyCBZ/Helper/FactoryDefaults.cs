using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using Win_CBZ.Data;

namespace Win_CBZ.Helper
{
    internal class FactoryDefaults
    {
        public static readonly String[] DefaultMetaDataFieldTypes =
        {
            "AgeRating|ComboBox||Unknown,Adults Only 18+,Early Childhood,Everyone,Everyone 10+,G,Kids to Adults,M,MA15+,Mature 17+,PG,R18+,Rating Pending,Teen,X18+",
            "Manga|ComboBox||Unknown,Yes,YesAndLeftToRight,No",
            "BlackAndWhite|ComboBox||Unknown,Yes,No",
            "LanguageISO|Text|LanguageEditor|",
            "Tags|Text|TagEditor|"
        };

        public const String DefaultMetaDataFileName = "ComicInfo.xml";

        public const int DefaultMetaDataFileIndexVersion = 1;
    }
}
