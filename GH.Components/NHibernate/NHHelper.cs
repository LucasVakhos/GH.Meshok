using NHibernate;
namespace GH.Components
{
    public enum DbServerType
    {
        None, MSSql, MySql, FireBird
    }
    public static class NHHelper
    {
        internal static IFactoryCriator BaseCriator;
        private static IDictionary<DbServerType, IFactoryCriator> _verasticallyCriators = new Dictionary<DbServerType, IFactoryCriator>();
        private static ISessionFactory _baseFactory;
        private static IDictionary<DbServerType, ISessionFactory> _verasticallyFactoryes = new Dictionary<DbServerType, ISessionFactory>();
        internal static void SetMainFactoryCriator(IFactoryCriator factory)
        {
            BaseCriator = factory;
            _verasticallyCriators.Remove(factory.DbServerType);
        }
        public static void SetVerasticallyFactoryCriator(IFactoryCriator value)
        {
            _verasticallyCriators[value.DbServerType] = value;
        }
        private static ISessionFactory BaseSessionFactory
        {
            get
            {
                if (_baseFactory == null)
                    _baseFactory = BaseCriator.GetSessionFactory();
                return _baseFactory;
            }
        }
        public static ISession OpenMainSession()
        {
            ISession session = BaseSessionFactory.OpenSession();
            session.GetCurrentTransaction().Begin();
            return session;
        }
        public static ISession OpenSession()
        {
            if (BaseCriator == null)
                return null;

            return OpenSession(BaseCriator.DbServerType);
        }

        public static ISession OpenSession(DbServerType serverType)
        {
            try
            {
                ISession session = null;

                if (BaseCriator.DbServerType == serverType)
                    session = BaseSessionFactory.OpenSession();
                else
                    session = _verasticallyFactoryes[serverType].OpenSession();

                session.GetCurrentTransaction().Begin();
                return session;
            }
            catch (Exception ex)
            {
                Logger.Error(nameof(OpenSession), ex);
                return null;
            }
        }        
        public static bool Connect()
        {
            try
            {
                if (BaseSessionFactory != null)
                {
                    using (ISession session = OpenMainSession())
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                DlgHelper.DlgError(ex.ToString());
            }
            return false;
        }
    }
}
