using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using GH.Configs;
using MeshokBrowser.NHibernate;
using NHibernate;
namespace GH.NHibernate
{
    public class FactoryCriatorMB : FactoryCriator<FactoryCriatorMB, User, CfgIShop>
    {
        public FactoryCriatorMB() : base()
        {
        }
        protected override void SetServerType()
        {
            _dbServerType = DbServerType.FireBird;
        }
        public override ISessionFactory GetSessionFactory()
        {
            string cs = GetConnectionString();
            FirebirdConfiguration fbc = new FirebirdConfiguration();
            return Fluently.Configure().Database(fbc.ConnectionString(cs))
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<User>())
                .BuildSessionFactory();
        }
    }
}
