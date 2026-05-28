using GH.NHibernate;

namespace GH.Configs
{
    public class FbLoginForm<TUser> : LoginFormType<FbLoginFrame<TUser>>
      where TUser : BaseUser
    {
        
    }
}