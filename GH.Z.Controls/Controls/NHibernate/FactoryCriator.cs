using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using GH.Configs;
using GH.UserExceptions;
using GH.Utils;
using NHibernate;
using System;


namespace GH.NHibernate
{
    public class FactoryCriator<TFactoryCriator, TUser, TSetting> : IFactoryCriator
        where TFactoryCriator : IFactoryCriator
        where TUser : BaseUser
        where TSetting : CfgCoreConnection
    {
        protected DbServerType _dbServerType;
        public DbServerType DbServerType => _dbServerType;

        public FactoryCriator()
        {
            SetServerType();
            NHHelper.SetVerasticallyFactoryCriator(this);
        }

        protected virtual void SetServerType()
        {
            throw new NotImplemented(nameof(SetServerType), this);
        }

        public virtual ISessionFactory GetSessionFactory()
        {
            try
            {
                throw new NotImplemented(nameof(GetSessionFactory), this);
            }
            catch (Exception e)
            {
                Logger.Error(e);

                return Fluently.Configure()
                    .Database(MySQLConfiguration.Standard.ConnectionString(GetConnectionString()))
                    .ExposeConfiguration(cfg => cfg.SetProperty("command_timeout", "200"))
                    .Mappings(m => m.FluentMappings.AddFromAssemblyOf<TUser>())
                    .BuildSessionFactory();
            }

        }

        //protected virtual CfgCoreConnection GetCfg()
        //{
        //    throw new NotImplemented(nameof(GetCfg), this);
        //    //пример return IniHelper.Cfg<TSetting>();
        //}

        public string GetConnectionString()
        {
            try
            {
                //return GetCfg().ConnectionString();
                return IniHelper.Cfg<TSetting>().ConnectionString();
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return string.Empty;
            }
        }
    }
}
