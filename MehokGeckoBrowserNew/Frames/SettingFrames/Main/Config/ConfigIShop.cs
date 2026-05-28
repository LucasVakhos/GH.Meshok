using FirebirdSql.Data.FirebirdClient;
using GH.Components;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
namespace MeshokBrowser
{
    public class SettingIShop : PrivateSetting
    {
        private bool _isComplete = false;
        [UpdatableProperty]
        public virtual string DataSource { get; set; } = "localhost";
        [UpdatableProperty]
        public virtual bool BaseRemote { get; set; } = false;
        [UpdatableProperty]
        public virtual int BaseDialect { get; set; } = 3;
        [UpdatableProperty]
        public virtual int BasePort { get; set; } = 3050;
        [UpdatableProperty]
        public virtual string BaseCharset { get; set; } = "WIN1251";
        [UpdatableProperty]
        public virtual string BaseLogin { get; set; } = "SYSDBA";
        [UpdatableProperty]
        public virtual string BasePassword { get; set; } = "42220";
        [UpdatableProperty]
        public virtual string Database { get; set; }
        [UpdatableProperty]
        public override string User { get; set; }
        [UpdatableProperty]
        public override string PassWrd { get; set; }
        public virtual bool UserWantRememberMe { get; set; }
        public bool IsComplete
        {
            get
            {
                if (!_isComplete)
                {
                    foreach (PropertyInfo item in GetType().GetProperties())
                    {
                        if (item.GetValue(this) == null)
                            break;
                        if (item.Name == "User")
                        {
                            _isComplete = true;
                            break;
                        }
                    }
                }
                return _isComplete;
            }
            set
            {
                _isComplete = value;
            }
        }
    }
    public class ConfigIShop : PrivateConfig<SettingIShop>, IConnectSetting
    {
        [Display(Name = "Server", Description = "Серевер или имя компьютера")]
        public virtual string DataSource { get; set; } = "localhost";
        [Display(Name = "Remote", Description = "База на другом компьютере")]
        public virtual bool BaseRemote { get; set; } = false;
        [Display(Name = "Dialect", Description = "Dialect Базы данных")]
        public virtual int BaseDialect { get; set; } = 3;
        [Display(Name = "Port", Description = "Port Базы данных")]
        public virtual int BasePort { get; set; } = 3050;
        [Display(Name = "Charset", Description = "Charset Базы данных")]
        public virtual string BaseCharset { get; set; } = "WIN1251";
        [Display(Name = "Login", Description = "Login  для подключения к Базе данных")]
        public virtual string BaseLogin { get; set; } = "SYSDBA";
        [Display(Name = "Password", Description = "Password для подключения к Базе данных")]
        public virtual string BasePassword { get; set; } = "42220";
        private string _base = "";
        [Display(Name = "База данных", Description = "Локальный путь к базе данных включая название файла")]
        public virtual string Database { get => _base; set => _base = value; }
        [Display(Name = "Remember Me", Description = "Входить без подтверждения авторизации")]
        public virtual bool UserWantRememberMe { get; set; }
        public bool IsComplete
        {
            get
            {
                return Setting.IsComplete;
            }
            set
            {
                Setting.IsComplete = value;
            }
        }
        //bool IConnectSetting.IsComplete { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public SettingIShop Setting => ini.Setting as SettingIShop;
        public bool Refresh()
        {
            throw new System.NotImplementedException();
        }
        //public override void LoadFromIni()
        //{
        //    if (Setting != null)
        //    {
        //        DataSource = Setting.DataSource;
        //        BaseDialect = Setting.BaseDialect;
        //        BasePort = Setting.BasePort;
        //        BaseCharset = Setting.BaseCharset;
        //        BaseRemote = Setting.BaseRemote;
        //        Database = Setting.Database;
        //        BaseLogin = Setting.BaseLogin;
        //        BasePassword = Setting.BasePassword;
        //        User = Setting.User;
        //        PassWrd = Setting.PassWrd;
        //        UserWantRememberMe = Setting.UserWantRememberMe;
        //    }
        //}
        public override void SaveToIni()
        {
            if (Setting != null)
            {
                Setting.DataSource = DataSource;
                Setting.BaseDialect = BaseDialect;
                Setting.BasePort = BasePort;
                Setting.BaseCharset = BaseCharset;
                Setting.BaseRemote = BaseRemote;
                Setting.Database = Database;
                Setting.BaseLogin = BaseLogin;
                Setting.BasePassword = BasePassword;
                Setting.User = User;
                Setting.PassWrd = PassWrd;
                Setting.UserWantRememberMe = UserWantRememberMe;
                ini.Save();
            }
        }
        //TODO public override bool TestConnection()
        //{
        //    using (FbConnection connection = new FbConnection(Connections<ConfigIShop>.CreateConnectionString()))
        //    {
        //        try
        //        {
        //            connection.Open();
        //            return connection.State == System.Data.ConnectionState.Open;
        //        }
        //        catch (System.Exception ex)
        //        {
        //            Logger.Error(ex);
        //            return false;
        //        }
        //    }
        //}
    }
}
