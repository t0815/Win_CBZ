//------------------------------------------------------------------------------
// <auto-generated>
//     Dieser Code wurde von einem Tool generiert.
//     Laufzeitversion:4.0.30319.42000
//
//     Änderungen an dieser Datei können falsches Verhalten verursachen und gehen verloren, wenn
//     der Code erneut generiert wird.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Win_CBZ {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "17.3.0.0")]
    internal sealed partial class Win_CBZSettings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Win_CBZSettings defaultInstance = ((Win_CBZSettings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Win_CBZSettings())));
        
        public static Win_CBZSettings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("%APPDATA%\\CBZMage\\Temp\\")]
        public string TempFolderPath {
            get {
                return ((string)(this["TempFolderPath"]));
            }
            set {
                this["TempFolderPath"] = value;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0.7.7b")]
        public string Version {
            get {
                return ((string)(this["Version"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute(@"<?xml version=""1.0"" encoding=""utf-16""?>
<ArrayOfString xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
  <string>{name}</string>
  <string>{title}</string>
  <string>{page}</string>
  <string>{pages}</string>
  <string>{index}</string>
  <string>{size}</string>
  <string>{number}</string>
  <string>{month}</string>
  <string>{year}</string>
  <string>{lang}</string>
  <string>{series}</string>
  <string>{type}</string>
</ArrayOfString>")]
        public global::System.Collections.Specialized.StringCollection RenamerPlaceholders {
            get {
                return ((global::System.Collections.Specialized.StringCollection)(this["RenamerPlaceholders"]));
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("{title}_p{page}")]
        public string StoryPageRenamePattern {
            get {
                return ((string)(this["StoryPageRenamePattern"]));
            }
            set {
                this["StoryPageRenamePattern"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("{type}_{index}")]
        public string SpecialPageRenamePattern {
            get {
                return ((string)(this["SpecialPageRenamePattern"]));
            }
            set {
                this["SpecialPageRenamePattern"] = value;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute(@"<?xml version=""1.0"" encoding=""utf-16""?>
<ArrayOfString xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
  <string>count(&lt;property&gt;, &lt;{attribute}&gt;)</string>
  <string>{IF (&lt;conditions&gt;) THEN '&lt;pattern&gt;|&lt;string&gt;' ELSE '&lt;pattern&gt;|&lt;string&gt;'}</string>
</ArrayOfString>")]
        public global::System.Collections.Specialized.StringCollection RenamerFunctions {
            get {
                return ((global::System.Collections.Specialized.StringCollection)(this["RenamerFunctions"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Win_CBZ")]
        public string AppName {
            get {
                return ((string)(this["AppName"]));
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool PagePreviewEnabled {
            get {
                return ((bool)(this["PagePreviewEnabled"]));
            }
            set {
                this["PagePreviewEnabled"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::System.Collections.Specialized.StringCollection CustomDefaultProperties {
            get {
                return ((global::System.Collections.Specialized.StringCollection)(this["CustomDefaultProperties"]));
            }
            set {
                this["CustomDefaultProperties"] = value;
            }
        }
    }
}
