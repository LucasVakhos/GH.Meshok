using GH.Attributes;
using GH.Configs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Net.NetworkInformation;
using System.Runtime.Serialization;
namespace MeshokBrowser
{
    public class CfgMeshok : CfgCoreConnection
    {
        private static string _site_name = "meshok.net";
        public static string _refer_url = $"https://{_site_name}/";
        public static string RegCookie { get; internal set; }
        public static string CreateFullUrl(string url)
        {
            if (url.StartsWith("/"))
                url = url.Substring(1);
            return _refer_url + url;
        }
        internal string SiteName => _site_name;
        public static string _url = "https://" + _site_name;
        internal string Base_Url => _url;
        internal string Profile_Url => _profile_url;
        public static string _profile_url = $"https://{_site_name}/profile.php";
        [DataMember]
        [Display(Name = "Логин", Description = "Логин типа (email@mail.com)"), EmailAddress]
        public override string UserLogin { get; set; } = "bridgenote@gmail.com";
        [DataMember]
        [Display(Name = "Пароль", Description = "Пароль"), PasswordPropertyText]
        public override string UserPassword { get; set; } = "DjcmvjqEhj;fq2021";
        [DataMember]
        [Display(Name = "Дополнительно", Description = "Дополнительные условия доставки")]
        public string AddInfo { get; set; } = "Отправка Почтой России зависит от веса и оценочной стоимости ПО. " +
            "В среднем: от 250 до 450 р. на стандартный CD или LP. Остальное обсуждается отдельно! Самовывоз - 50, а курьер - 400 р. за любое кол-во";
        [DataMember]
        [Display(Name = "Курс", Description = "Курс для товара")]
        public double Curs { get; set; } = 85.00;
        [DataMember]
        [Display(Name = "Комиссия", Description = "Комиссия сайта")]
        public double Comission { get; set; } = 2.00;
        [DataMember]
        [Display(Name = "По городу", Description = "Стоимость доставки")]
        public double PriceCity { get; set; } = 400.00;
        [DataMember]
        [Display(Name = "По стране", Description = "Стоимость доставки")]
        public double PriceCountry { get; set; } = 400.00;
        [DataMember]
        [Display(Name = "По миру", Description = "Стоимость доставки")]
        public double PriceWorld { get; set; } = 600.00;
        [DataMember]
        [Display(Name = "Сканирование", Description = "Количество заказов на странице для сканирования...")]
        public int OnPageForScan { get; set; } = 50;
        [DataMember]
        [Display(Name = "Удаление", Description = "Количество лотов на странице для удаления...")]
        public int OnPageForDelete { get; set; } = 500;
        [DataMember]
        [Display(Name = "Загрузка", Description = "Количество лотов на странице для загрузки...")]
        public int OnPageForLoad { get; set; } = 500;
        [DataMember]
        [DbConnectionProperty(Category = Category.DateInterval, Caption = "Дата продаж", ToolTip = "Данные о продажах начиная с этой даты ")]
        public virtual DateTime DateRasxFrom { get; set; }
        [DataMember]
        public virtual DateTime DateRasxTo { get; set; }
        [DataMember]
        [DbConnectionProperty(Category = Category.DateInterval, Caption = "Дата прихода", ToolTip = "Данные о приходах начиная с этой даты ")]
        public virtual DateTime DatePrixFrom { get; set; }
        [DataMember]
        public virtual DateTime DatePrixTo { get; set; }
        [DataMember]
        public Dictionary<string, string> RasxLotNames { get; set; } = new Dictionary<string, string>();
        public override bool TestConnection()
        {
            Ping ping = new Ping();
            PingReply pingReply = ping.Send(SiteName);
            return pingReply.Status == IPStatus.Success;
        }
        protected override void CreateSomething()
        {
            //заглушка
        }
    }
}
