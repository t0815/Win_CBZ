﻿//------------------------------------------------------------------------------
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
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "17.10.0.0")]
    internal sealed partial class Win_CBZSettings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Win_CBZSettings defaultInstance = ((Win_CBZSettings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Win_CBZSettings())));
        
        public static Win_CBZSettings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("%APPDATA%\\WIN_CBZ\\Temp\\")]
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
        [global::System.Configuration.DefaultSettingValueAttribute("0.20.156b")]
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
  <string>{ext}</string>
  <string>{page}</string>
  <string>{pages}</string>
  <string>{index}</string>
  <string>{size}</string>
  <string>{number}</string>
  <string>{year}</string>
  <string>{month}</string>
  <string>{day}</string>
  <string>{lang}</string>
  <string>{series}</string>
  <string>{type}</string>
  <string>{hash}</string>
</ArrayOfString>")]
        public global::System.Collections.Specialized.StringCollection RenamerPlaceholders {
            get {
                return ((global::System.Collections.Specialized.StringCollection)(this["RenamerPlaceholders"]));
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("p_{page}.{ext}")]
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
        [global::System.Configuration.DefaultSettingValueAttribute("{type}_{page}.{ext}")]
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
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::System.Collections.Specialized.StringCollection ValidKnownTags {
            get {
                return ((global::System.Collections.Specialized.StringCollection)(this["ValidKnownTags"]));
            }
            set {
                this["ValidKnownTags"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0")]
        public int ResizeAlgorithm {
            get {
                return ((int)(this["ResizeAlgorithm"]));
            }
            set {
                this["ResizeAlgorithm"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool ValidateTags {
            get {
                return ((bool)(this["ValidateTags"]));
            }
            set {
                this["ValidateTags"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool CompatMode {
            get {
                return ((bool)(this["CompatMode"]));
            }
            set {
                this["CompatMode"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool TagValidationIgnoreCase {
            get {
                return ((bool)(this["TagValidationIgnoreCase"]));
            }
            set {
                this["TagValidationIgnoreCase"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("1071")]
        public int WindowW {
            get {
                return ((int)(this["WindowW"]));
            }
            set {
                this["WindowW"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("681")]
        public int WindowH {
            get {
                return ((int)(this["WindowH"]));
            }
            set {
                this["WindowH"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("319")]
        public int Splitter1 {
            get {
                return ((int)(this["Splitter1"]));
            }
            set {
                this["Splitter1"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("122")]
        public int Splitter2 {
            get {
                return ((int)(this["Splitter2"]));
            }
            set {
                this["Splitter2"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("167")]
        public int Splitter3 {
            get {
                return ((int)(this["Splitter3"]));
            }
            set {
                this["Splitter3"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("439")]
        public int Splitter4 {
            get {
                return ((int)(this["Splitter4"]));
            }
            set {
                this["Splitter4"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0")]
        public int WindowX {
            get {
                return ((int)(this["WindowX"]));
            }
            set {
                this["WindowX"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0")]
        public int WindowY {
            get {
                return ((int)(this["WindowY"]));
            }
            set {
                this["WindowY"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool IgnoreErrorsOnSave {
            get {
                return ((bool)(this["IgnoreErrorsOnSave"]));
            }
            set {
                this["IgnoreErrorsOnSave"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("3ab980acc9ab16b")]
        public string DebugMode {
            get {
                return ((string)(this["DebugMode"]));
            }
            set {
                this["DebugMode"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0")]
        public int ImageConversionMode {
            get {
                return ((int)(this["ImageConversionMode"]));
            }
            set {
                this["ImageConversionMode"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("100")]
        public int ImageConversionQuality {
            get {
                return ((int)(this["ImageConversionQuality"]));
            }
            set {
                this["ImageConversionQuality"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("ComicInfo.xml")]
        public string MetaDataFilename {
            get {
                return ((string)(this["MetaDataFilename"]));
            }
            set {
                this["MetaDataFilename"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<ArrayOfString xmlns:xsd=\"http://www.w3." +
            "org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" />")]
        public global::System.Collections.Specialized.StringCollection CustomMetadataFields {
            get {
                return ((global::System.Collections.Specialized.StringCollection)(this["CustomMetadataFields"]));
            }
            set {
                this["CustomMetadataFields"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("1")]
        public int MetaDataPageIndexVersionToWrite {
            get {
                return ((int)(this["MetaDataPageIndexVersionToWrite"]));
            }
            set {
                this["MetaDataPageIndexVersionToWrite"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool FirstRun {
            get {
                return ((bool)(this["FirstRun"]));
            }
            set {
                this["FirstRun"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0")]
        public int SettingsVersion {
            get {
                return ((int)(this["SettingsVersion"]));
            }
            set {
                this["SettingsVersion"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::System.Collections.Specialized.StringCollection Mru {
            get {
                return ((global::System.Collections.Specialized.StringCollection)(this["Mru"]));
            }
            set {
                this["Mru"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string RecentOpenArchivePath {
            get {
                return ((string)(this["RecentOpenArchivePath"]));
            }
            set {
                this["RecentOpenArchivePath"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string RecentAddImagePath {
            get {
                return ((string)(this["RecentAddImagePath"]));
            }
            set {
                this["RecentAddImagePath"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string RecentSavedArchivePath {
            get {
                return ((string)(this["RecentSavedArchivePath"]));
            }
            set {
                this["RecentSavedArchivePath"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool OmitEmptyXMLTags {
            get {
                return ((bool)(this["OmitEmptyXMLTags"]));
            }
            set {
                this["OmitEmptyXMLTags"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool AutoDeleteTempFiles {
            get {
                return ((bool)(this["AutoDeleteTempFiles"]));
            }
            set {
                this["AutoDeleteTempFiles"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool SkipIndexCheck {
            get {
                return ((bool)(this["SkipIndexCheck"]));
            }
            set {
                this["SkipIndexCheck"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0")]
        public int WhatsNewVersion {
            get {
                return ((int)(this["WhatsNewVersion"]));
            }
            set {
                this["WhatsNewVersion"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool CalculateHash {
            get {
                return ((bool)(this["CalculateHash"]));
            }
            set {
                this["CalculateHash"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool CheckDuplicatePages {
            get {
                return ((bool)(this["CheckDuplicatePages"]));
            }
            set {
                this["CheckDuplicatePages"] = value;
            }
        }
    }
}
