using MySql.Data.MySqlClient;
namespace GH.Components
{
    public class CfgCoreMySqlConnection : CfgCoreConnection
    {
        public string Server { get; set; }

    public string Database { get; set; }

    public string UserID { get; set; }

    public string Password { get; set; }

    public uint Port { get; set; } = 3306;
    public string CharacterSet { get; set; } = "utf8";
    public MySqlConnectionProtocol ConnectionProtocol { get; set; } = MySqlConnectionProtocol.Tcp;
    public MySqlSslMode SslMode { get; set; } = MySqlSslMode.Disabled;
    public override string ConnectionString()
        {
            var csb = new MySqlConnectionStringBuilder();
            csb.Server = Server;
            csb.Database = Database;
            csb.UserID = UserID;
            csb.Password = Password;
            csb.ConnectionProtocol = ConnectionProtocol;
            csb.Port = Port;
            csb.CharacterSet = CharacterSet;
            csb.SslMode = SslMode;
            return csb.ConnectionString;
        }
    public override bool TestConnection()
        {
            using (MySqlConnection conn = new MySqlConnection(ConnectionString()))
            {
                try
                {
                    conn.Open();
                    return true;
                }
                catch (Exception e)
                {
                    Logger.Error(e);
                }
                return false;
            }
        }
    public override bool IsRemoteDataBase()
        {
            return !(Server == Environment.MachineName || Server == "127.0.0.1" || Server == "localhost");
        }
    }
}
