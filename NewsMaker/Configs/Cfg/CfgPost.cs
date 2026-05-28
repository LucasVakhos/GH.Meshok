using GH.Configs;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
namespace NewsMaker
{
    public class CfgPost : CfgCore
    {
        [DataMember]
        [Display(Name = "Smtp", Description = "Smtp сервер")]
        public string Smtp { get; set; } = "smtp.yandex.ru";
        [DataMember]
        [Display(Name = "Логин", Description = "Логин для подключения")]
        public string User { get; set; } = "bridgenote";
        [DataMember]
        [Display(Name = "Пароль", Description = "Пароль для подключения")]
        public string PassWrd { get; set; } = "Gznsqehj;fq2016";
        [DataMember]
        [Display(Name = "Email", Description = "Обратный адрес")]
        public string BridgeEmail { get; set; } = "bridgenote@yandex.ru";
        [DataMember]
        [Display(Name = "Порт", Description = "Порт")]
        public int Port { get; set; } = 25;
        [DataMember]
        [Display(Name = "Use SSL", Description = "Использовать шифрование")]
        public bool UseSSL { get; set; } = true;
        protected override void CreateSomething()
        {
            //заглушка
        }
    }
}
