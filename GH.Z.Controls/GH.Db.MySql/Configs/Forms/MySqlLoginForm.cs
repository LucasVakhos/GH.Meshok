using GH.NHibernate;

namespace GH.Configs
{
    public class MySqlLoginForm<TUser> : LoginFormType<MySqlLoginFrame<TUser>>
      where TUser : BaseUser
    {
        
    }
}