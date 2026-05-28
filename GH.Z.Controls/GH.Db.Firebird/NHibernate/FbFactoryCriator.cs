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
    public class FbFactoryCriator<TUser> : FactoryCriator<FbFactoryCriator<TUser>, TUser, CfgFbConnection>
        where TUser : BaseUser
    {
        public FbFactoryCriator(): base()
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
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<TUser>())
                .BuildSessionFactory();
        }
    }
}
