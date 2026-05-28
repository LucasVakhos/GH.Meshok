using NHibernate;

namespace GH.NHibernate
{
    public interface IFactoryCriator
    {
        DbServerType DbServerType { get; }
        ISessionFactory GetSessionFactory();
    }
}