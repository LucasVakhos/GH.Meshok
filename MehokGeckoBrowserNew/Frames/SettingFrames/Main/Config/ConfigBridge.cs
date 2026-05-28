using GH.Components;
using MySql.Data.MySqlClient;
using System.ComponentModel.DataAnnotations;
namespace MeshokBrowser
{
    public class SettingBridge : PrivateSetting
    {
        public virtual string Server { get; set; } = "bd.bridgenote.com";
        public virtual string Base { get; set; } = "bridgenote";
        public override string User { get; set; } = "bridge";
        public override string PassWrd { get; set; } = "1MaB5zIrOndfqfvUJc";
    }
    public class ConfigBridge : PrivateConfig
    {
        private static uint _port = 3306;
        [Display(Name = "Server", Description = "Серевер или имя компьютера")]
        public virtual string Server { get; set; }
        //private string _base = "bridgenote";
        [Display(Name = "Database", Description = "Путь к базе данных")]
        public virtual string Base { get; set; }
        //private string _user = "bridge";
        public override string User { get; set; }
        //private string _passWrd = "1MaB5zIrOndfqfvUJc";
        public override string PassWrd { get; set; }
        public override IIniFile GetIni()
        {
            return new IniFille<SettingBridge>();
        }
        public override void LoadFromIni()
        {
            if (ini.Setting is SettingBridge setting)
            {
                Server = setting.Server;
                Base = setting.Base;
                User = setting.User;
                PassWrd = setting.PassWrd;
            }
        }
        public override void SaveToIni()
        {
            if (ini.Setting is SettingBridge setting)
            {
                setting.Server = Server;
                setting.Base = Base;
                setting.User = User;
                setting.PassWrd = PassWrd;
                ini.Save();
            }
        }
        public override bool TestConnection()
        {
            using (MySqlConnection sqlConnection = CreateConnection())
            {
                try
                {
                    sqlConnection.Open();
                    sqlConnection.Close();
                }
                catch
                {
                    return false;
                }
                return true;
            }
        }
        public static MySqlConnection CreateConnection()
        {
            ConfigBridge bridgeNote = ConfigMain.GetConfig<ConfigBridge>();
            MySqlConnectionStringBuilder csb = new MySqlConnectionStringBuilder();
            csb.Server = bridgeNote.Server;
            csb.UserID = bridgeNote.User;
            csb.Password = bridgeNote.PassWrd;
            csb.Database = bridgeNote.Base;
            csb.ConnectionProtocol = MySqlConnectionProtocol.Tcp;
            csb.Port = _port;
            csb.CharacterSet = "utf8";
            csb.SslMode = MySqlSslMode.None;
            return new MySqlConnection(csb.ConnectionString);
        }
    }
}
