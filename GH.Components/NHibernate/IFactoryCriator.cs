using NHibernate;
namespace GH.Components
{
    public interface IFactoryCriator
    {
        DbServerType DbServerType { get; }
        ISessionFactory GetSessionFactory();
    }
}
