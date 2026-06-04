using System.Configuration;
using System.Reflection;
namespace GH.Components
{
    public class AppIni
    {
        public static AppIni _instance = null;
    public static AppIni Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new AppIni();
                return _instance;
            }
        }
        string _exePath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        string _exeName = Assembly.GetEntryAssembly().GetName().Name;
    public double ReadDouble(string v, double curs)
        {
            throw new NotImplementedException();
        }
    public void WriteDouble(string v, double curs)
        {
            throw new NotImplementedException();
        }

    private string ElementName
        {
            get { return "add"; }
        }

    private string _defaultSection = null;
    public string DefaultSection
        {
            get
            {
                AppSettingsSection appIniSection = AppConfigTool.GetAppSettings();
                if (_defaultSection == null)
                    _defaultSection = _exeName;
                KeyValueConfigurationElement sec = appIniSection.Settings[_defaultSection];
                if (sec == null)
                {
                    sec = new KeyValueConfigurationElement(_defaultSection, _defaultSection);
                    appIniSection.Settings.Add(sec);
                    appIniSection.CurrentConfiguration.Save();
                }
                return _defaultSection;
            }
            set
            {
                _defaultSection = value;
            }
        }
    public string ReadString(string key, string defaultValue)
        {
            if (key != "")
            {
                try
                {
                    return AppConfigTool.GetConfigElement(key).Value;
                    //AppIniSection config = ConfigurationManager.GetSection(DefaultSection) as AppIniSection;
                    //if (ConfigurationManager.GetSection(DefaultSection is AppIniSection appIniSection)
                    //{
                    //    //appIniSection.CurrentConfiguration.
                    //}
                    //&& appIniSection.CurrentConfiguration.AppSettings.wh Count == 0)
                    //config.Instances.. = defaultValue;
                    ////settings.Add(key, defaultValue);
                    //config.
                    //Configuration config;
                    //KeyValueConfigurationCollection settings;
                    //////GetSettings(out config, out settings);
                    ////if (settings[key] == null)
                    ////    settings.Add(key, defaultValue);
                    //////config.Save(ConfigurationSaveMode.Modified); //-->Exception: Method failed with unexpected error code 1.
                    ////config.Save(); //-->Exception: Method failed with unexpected error code 1.
                    //ConfigurationManager.RefreshSection(config.AppSettings.SectionInformation.Name);
                    //return ConfigurationManager.AppSettings[key];
                }
                catch (ConfigurationErrorsException cfg_ex)
                {
                }
                catch (Exception ex)
                {
                }
            }
            return defaultValue;
        }
        // AppIniSection appSettings = ConfigurationManager.GetSection(_defaultSection) as AppIniSection;
        ////Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal);
        ////
        //if (config.AppSettings.Settings.AllKeys.Where(x => x.e))
        //    config.AppSettings.Settings.Add(ElementName, _defaultSection);
        ////KeyValueConfigurationElement[] test = (from KeyValueConfigurationElement e in ((AppSettingsSection)ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None).GetSection("mySection")).Settings
        //                                       where e.Value == "1"
        //                                       select e).ToArray();
        //string configPath = "/aspnet";
        //if (config.AppSettings == null)
        //    config.AppSettings.ad
        // Get the Web application configuration object.
        //Configuration config =
        //  ConfigurationManager.OpenExeСonfiguration(configPath); Configuration config;
        //KeyValueConfigurationCollection settings;
        //GetSettings(out config, out settings);
        //settings.Add(_defaultSection);
        //public AppIni()
        //{
        //    //_path = new FileInfo(_exeName + ".ini").FullName;
        //}
    public bool ReadBool(string key, bool defaultValue)
        {
            if (key != "")
                return bool.Parse(ReadString(key, $"{defaultValue}"));
            return defaultValue;
        }
    public int ReadInteger(string key, int defaultValue)
        {
            if (key != "")
                return int.Parse(ReadString(key, $"{defaultValue}"));
            return defaultValue;
        }
    public void WriteBool(string v, bool baseRemote)
        {
            throw new NotImplementedException();
        }
    public void WriteInteger(string v, int baseDialect)
        {
            throw new NotImplementedException();
        }
    public void WriteString(string key, string defaultValue)
        {
            throw new NotImplementedException();
        }
    }
}
