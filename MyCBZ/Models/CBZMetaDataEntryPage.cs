using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCBZ
{
    internal class CBZMetaDataEntryPage
    {
        public const String COMIC_PAGE_ATTRIBUTE_IMAGE = "Image";
        public const String COMIC_PAGE_ATTRIBUTE_TYPE = "Type";
        public const String COMIC_PAGE_ATTRIBUTE_DOUBLE_PAGE = "DoublePage";
        public const String COMIC_PAGE_ATTRIBUTE_IMAGE_SIZE = "ImageSize";
        public const String COMIC_PAGE_ATTRIBUTE_KEY = "Key";
        public const String COMIC_PAGE_ATTRIBUTE_IMAGE_WIDTH = "ImageWidth";
        public const String COMIC_PAGE_ATTRIBUTE_IMAGE_HEIGHT = "ImageHeight";

        public const String COMIC_PAGE_TYPE_STORY = "Story";
        public const String COMIC_PAGE_TYPE_FRONT_COVER = "FrontCover";
        public const String COMIC_PAGE_TYPE_INNER_COVER = "InnerCover";
        public const String COMIC_PAGE_TYPE_BACK_COVER = "BackCover";
        public const String COMIC_PAGE_TYPE_ROUNDUP = "Roundup";
        public const String COMIC_PAGE_TYPE_ADVERTICEMENT = "Advertisment";
        public const String COMIC_PAGE_TYPE_EDITORIAL = "Editorial";
        public const String COMIC_PAGE_TYPE_LETTERS = "Letters";
        public const String COMIC_PAGE_TYPE_PREVIEW = "Preview";
        public const String COMIC_PAGE_TYPE_OTHER = "Other";
        public const String COMIC_PAGE_TYPE_DELETED = "Deleted";

        /*
         <xs:enumeration value = "FrontCover" />
                    < xs:enumeration value = "InnerCover" />
                    < xs:enumeration value = "Roundup" />
                    < xs:enumeration value = "Story" />
                    < xs:enumeration value = "Advertisment" />
                    < xs:enumeration value = "Editorial" />
                    < xs:enumeration value = "Letters" />
                    < xs:enumeration value = "Preview" />
                    < xs:enumeration value = "BackCover" />
                    < xs:enumeration value = "Other" />
                    < xs:enumeration value = "Deleted" /> 
        */

        public Dictionary<String, String> Attributes;

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

        public CBZMetaDataEntryPage()
        {
            Attributes = new Dictionary<String, String>();
        }

        public CBZMetaDataEntryPage SetAttribute(String name, String value)
        {
            Attributes.Add(name, value);

            return this;
        }

        public String GetAttribute(String key)
        {
            if (Attributes.ContainsKey(key))
            {
                return Attributes[key];
            } else
            {
                return null;
            }
        }
    }
}
