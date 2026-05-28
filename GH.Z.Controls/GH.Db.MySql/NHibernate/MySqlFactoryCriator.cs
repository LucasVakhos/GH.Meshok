using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using GH.Configs;
using GH.NHibernate;
using GH.UserExceptions;
using GH.Utils;
using NHibernate;
using System;
using System.Windows.Forms;


namespace GH.NHibernate
{
    public class MySqlFactoryCriator<TUser> : FactoryCriator<MySqlFactoryCriator<TUser>, TUser, CfgMySqlConnection>
        where TUser : BaseUser
    {

        protected override void SetServerType()
        {
            _dbServerType = DbServerType.MySql;
        }

        public override ISessionFactory GetSessionFactory()
        {
            return Fluently.Configure()
                .Database(MySQLConfiguration.Standard.ConnectionString(GetConnectionString()))
                .ExposeConfiguration(cfg => cfg.SetProperty("command_timeout", "200"))
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<TUser>())
                .BuildSessionFactory();
        }
    }
}
