using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Win_CBZ.Models;

namespace Win_CBZ.Data
{
    internal class LanguageList
    {


        public LanguageList() { 
        
        }

        public static BindingList<LanguageListItem> Languages { get; set; } = new BindingList<LanguageListItem> {

            new LanguageListItem( "Chinese", "cn"),
            new LanguageListItem( "Czech", "cs"),
            new LanguageListItem( "Danish", "da"),
            new LanguageListItem( "Dutch", "nl"),
            new LanguageListItem( "English", "en"),
            new LanguageListItem( "French", "fr"),
            new LanguageListItem( "German", "de"),
            new LanguageListItem( "Greek", "el"),           
            new LanguageListItem( "Italian", "it"),           
            new LanguageListItem( "Japanese", "ja"),          
            new LanguageListItem( "Korean", "ko"),
            new LanguageListItem( "Norwegian", "no"),           
            new LanguageListItem( "Polish", "pl"),
            new LanguageListItem( "Portuguese", "pt"),
            new LanguageListItem( "Romanian", "ro"),          
            new LanguageListItem( "Russian", "ru"),
            new LanguageListItem( "Spanish", "sp"),
        };
    }
}
