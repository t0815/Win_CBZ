using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCBZ
{
    internal class CBZMetaDataEntryPage
    {
        public const String COMIC_PAGE_TYPE_STORY = "Story";


        public int Image { get; set; }

        public String ImageType { get; set; }

        public bool DoublePage { get; set; }

        public long ImageSize { get; set; }

        public String Key { get; set; }

        public int ImageWidth { get; set; }

        public int ImageHeight { get; set; }

        /*
         * 
         
        <xs:attribute name="Image" type="xs:int" use="required" />
        <xs:attribute default="Story" name="Type" type="ComicPageType" />
        <xs:attribute default="false" name="DoublePage" type="xs:boolean" />
        <xs:attribute default="0" name="ImageSize" type="xs:long" />
        <xs:attribute default="" name="Key" type="xs:string" />
        <xs:attribute default="-1" name="ImageWidth" type="xs:int" />
        <xs:attribute default="-1" name="ImageHeight" type="xs:int" />

         * 
         */

        public CBZMetaDataEntryPage(int image, long size = 0)
        {
            Image = image;
            ImageSize = size;
        }

        public CBZMetaDataEntryPage(int image, long size = 0, String imageType = CBZMetaDataEntryPage.COMIC_PAGE_TYPE_STORY)
        {
            Image = image;
            ImageSize = size;
            ImageType = ImageType;
        }
    }
}
