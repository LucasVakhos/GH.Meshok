using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using GH.Configs;
using NewsMaker.NHibernate;
using NHibernate;
namespace GH.NHibernate
{
    public class FactoryCriatorNM : FactoryCriator<FactoryCriatorNM, User, CfgBridgeNote>
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
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<User>())
                .BuildSessionFactory();
        }
    }
}
