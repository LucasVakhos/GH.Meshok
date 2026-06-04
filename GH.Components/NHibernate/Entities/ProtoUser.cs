// GH.Components\NHibernate\Entities\ProtoUser.cs
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace GH.Components
{
    [DisplayName("User"), Description("Информация о пользователе"), System.ComponentModel.Category("Информация")]
    public class ProtoUser : ProtoEntity
    {
        [Display(Name = "Ф.И.О.", Description = "Инициалы пользователя")]
        public override string name { get; set; }
        [Display(Name = "URL", Description = "URL"), ReadOnly(true)]
        public virtual string url { get; set; } = "(localhost)";
        [Display(Name = "Логин", Description = "Логин"), ReadOnly(true)]
        public virtual string login => name;
        [Display(Name = "Пароль", Description = "Пароль")]
        [MaxLength(127), Required(AllowEmptyStrings = false), PasswordPropertyText]
        public virtual string password { get; set; }
        [Display(Name = "Активен", Description = "Активен"), ReadOnly(true)]
        public virtual bool active { get; set; }
    }
}
