using FluentNHibernate.Mapping;
using GH.NHibernate;
namespace NewsMaker.NHibernate
{
    public class User : BaseUser
    {
    }
    public class UserMap : ClassMap<User>
    {
        public UserMap()
        {
            Table("users");
            Id(x => x.id, "id");
            Map(x => x.Name, "name");
            Map(x => x.Password, "password");
            Map(x => x.Active, "active");
        }
    }
}
