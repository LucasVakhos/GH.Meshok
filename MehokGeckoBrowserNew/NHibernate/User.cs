using FluentNHibernate.Mapping;
using GH.Attributes;
using GH.NHibernate;
namespace MeshokBrowser.NHibernate
{
    public class User : BaseUser
    {
        [UpdatableProperty(Caption = "Логин", ToolTip = "Логин", Group = "Данные пользователя")]
        public override string Login
        {
            get { return Name; }
            set { Name = value; }
        }
    }
    public class UserMap : ClassMap<User>
    {
        public UserMap()
        {
            Table("MANAGERS");
            Id(x => x.id, "MN_ID");
            Map(x => x.Name, "MN_NAME");
            //Map(x => x.Login, "MN_NAME");
            Map(x => x.Password, "MN_PASSWORD");
            Map(x => x.Active, "MN_ACTIVE");
        }
    }
}
