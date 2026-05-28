namespace GH.Components
{
    public class VersionHelper : IComparable<Version>
    {
        static string newVersion = Application.ProductVersion;
        static DirectoryInfo configsDirInfo;
        public static void CheckNewVersion()
        {
            configsDirInfo = new DirectoryInfo(RunContext.ConfigsPath);
            if (new Version(newVersion).CompareTo(new Version(RunContext.AppCfg.ProductVersion)) > 0)
            {
                ClearConfigFolder();
                RunContext.AppCfg.ProductVersion = newVersion;
                RunContext.AppCfg.Save(true);
            }
        }
        private static bool ConfigsExists()
        {
            return configsDirInfo.Exists && ConfigsList().Any();
        }
        private static void ClearConfigFolder()
        {
            if (ConfigsExists())
                foreach (FileInfo item in ConfigsList())
                    item.Delete();
        }
        private static IEnumerable<FileInfo> ConfigsList()
        {
            return configsDirInfo.GetFiles().Where(f => f.Extension.ToUpper() == ".XML");
        }
        int CompareTo(Version other)
        {
            return newVersion.CompareTo(other);
        }
        int IComparable<Version>.CompareTo(Version other)
        {
            return this.CompareTo(other);
        }
    }
}
