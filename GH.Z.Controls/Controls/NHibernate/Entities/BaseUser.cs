using FluentNHibernate.Mapping;
using GH.Components.Attributes;
using System;
using System.ComponentModel;


namespace GH.NHibernate
{
    [DisplayName("User"), Description("Информация о пользователе"), Category("Информация")]
    public class BaseUser : BaseEntity
    {

        [UpdatableProperty(Caption = "Ф.И.О.", ToolTip = "Инициалы пользователя", Group = "Данные пользователя")]
        public override string Name { get; set; }

        [UpdatableProperty(Caption = "Логин", ToolTip = "Логин", Group = "Данные пользователя")]
        public virtual string Login { get; set; }

        [UpdatableProperty(Caption = "Пароль", ToolTip = "Пароль от 8 до 25 символов", Group = "Данные пользователя"
            , Required = true, MaxLength = 25)]
        [PasswordPropertyText]
        public virtual string Password
        {
            get
            {
                return _password;
            }
            set
            {
                if (_password == value)
                    return;
                if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
                    throw new Exception("Пароль не должен быть пустым!");
                _password = value;

            }
        }
        private string _password = null;


        [UpdatableProperty(Caption = "Активен", ToolTip = "Активен", Group = "Данные пользователя")]
        public virtual bool Active { get; set; }
    }

    public class UserMap : ClassMap<BaseUser>
    {
        public UserMap()
        {
            Table("users");
            Id(x => x.id, "u_id");
            Map(x => x.Name, "u_name");
            Map(x => x.Login, "u_e_mail");
            Map(x => x.Password, "u_password");
            Map(x => x.Active, "u_active");
        }
    }
}
