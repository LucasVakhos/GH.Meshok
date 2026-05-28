using System.Configuration;
namespace GH.Components
{
    public class AppConfigTool
    {
        //private AppConfigTool _instance;
        //public AppConfigTool Instance {
        //    get => _instance; }
        private static Configuration _cfg = null;
        public static Configuration GetConfig()
        {
            if (_cfg == null)
            {
                if (HostingEnvironment.IsHosted) // running inside asp.net ?
                {
                    _cfg = WebConfigurationManager.OpenWebConfiguration(HostingEnvironment.ApplicationVirtualPath);
                }
                else
                {
                    _cfg = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                }
            }
            return _cfg;
        }
        public static AppSettingsSection GetAppSettings()
        {
            var cfg = GetConfig();
            return cfg == null ? null : cfg.AppSettings;
        }
        public static KeyValueConfigurationElement GetConfigElement(string key)
        {
            Configuration cfg = GetConfig();
            if (cfg == null)
                return null;
            KeyValueConfigurationElement sec = cfg.AppSettings.Settings[key];
            if (sec == null)
            {
                sec = new KeyValueConfigurationElement(key, key);
                cfg.AppSettings.Settings.Add(sec);
                cfg.Save();
            }
            return sec;
        }
    }
    //public class AppIniSection : ConfigurationSection
    //{
    //    [ConfigurationProperty("", IsRequired = true, IsDefaultCollection = true)]
    //    public AppyConfigCollection Instances
    //    {
    //        get { return (AppyConfigCollection)this[""]; }
    //        set { this[""] = value; }
    //    }
    //}
    //public class AppyConfigCollection : ConfigurationElementCollection
    //{
    //    protected override ConfigurationElement CreateNewElement()
    //    {
    //        return new AppConfigElement();
    //    }
    //    protected override object GetElementKey(ConfigurationElement element)
    //    {
    //        return ((AppConfigElement)element).Name;
    //    }
    //}
    //public class AppConfigElement : ConfigurationElement
    //{
    //    //Make sure to set IsKey=true for property exposed as the GetElementKey above
    //    [ConfigurationProperty("name", IsKey = true, IsRequired = true)]
    //    public string Name
    //    {
    //        get { return (string)base["name"]; }
    //        set { base["name"] = value; }
    //    }
    //    [ConfigurationProperty("code", IsRequired = true)]
    //    public string Code
    //    {
    //        get { return (string)base["code"]; }
    //        set { base["code"] = value; }
    //    }
    //}
}
