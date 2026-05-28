using GH.Components;
using System.ComponentModel.DataAnnotations;
namespace MeshokBrowser
{
    public class SettingMeshok : PrivateSetting
    {
        public virtual string AddInfo { get; set; } = "Стоимость отправки CD или LP Почтой России зависит от веса и оценочной стоимости ПО и составляет от 250 до 450 рублей " +
            "на стандартный CD или LP. Остальное обсуждается отдельно!";
        public virtual double Curs { get; set; } = 70;
        public virtual double Comission { get; set; } = 2;
        public virtual double PriceCity { get; set; } = 300;
        public virtual double PriceCountry { get; set; } = 400;
        public virtual double PriceWorld { get; set; } = 600;
    }
    public class ConfigMeshok : PrivateConfig<SettingMeshok>
    {
        public static int _siteNo = -1;
        public static string _url = "https://meshok.net";
        public static string _refer_url = "https://meshok.net/";
        public static string _profile_url = "https://meshok.net/profile.php";
        public virtual string Site { get => _url.Substring(_url.IndexOf('/')).Replace('/', ' ').Trim(); }
        public virtual string Base_Url { get => _url; }
        public virtual string Profile_Url { get => _profile_url; }
        private string _user = "bridgenote@gmail.com";
        public override string User { get => _user; set => _user = value; }
        private string _passWrd = "CtlmvjqEhj;fq2018";
        public override string PassWrd { get => _passWrd; set => _passWrd = value; }
        private string _addInfo = "Стоимость отправки CD или LP Почтой России зависит от веса и оценочной стоимости ПО и составляет от 250 до 450 рублей " +
            "на стандартный CD или LP. Остальное обсуждается отдельно!";
        [Display(Name = "Дополнительно", Description = "Дополнительные условия доставки")]
        public virtual string AddInfo { get => _addInfo; set => _addInfo = value; }
        private double _curs = 70;
        [Display(Name = "Курс", Description = "Курс для товара")]
        public virtual double Curs { get => _curs; set => _curs = value; }
        private double _comission = 2;
        [Display(Name = "Комиссия", Description = "Комиссия сайта")]
        public virtual double Comission { get => _comission; set => _comission = value; }
        private double _priceCity = 300;
        [Display(Name = "По городу", Description = "Стоимость доставки")]
        public virtual double PriceCity { get => _priceCity; set => _priceCity = value; }
        private double _priceCountry = 400;
        [Display(Name = "По стране", Description = "Стоимость доставки")]
        public virtual double PriceCountry { get => _priceCountry; set => _priceCountry = value; }
        private double _priceWorld = 600;
        [Display(Name = "По миру", Description = "Стоимость доставки")]
        public virtual double PriceWorld { get => _priceWorld; set => _priceWorld = value; }
        //public override IIniFile GetIni()
        //{
        //    return new IniFille<SettingMeshok>();
        //}
        public override void LoadFromIni()
        {
            if (ini.Setting is SettingMeshok setting)
            {
                User = setting.User;
                PassWrd = setting.PassWrd;
                Curs = setting.Curs;
                Comission = setting.Comission;
                PriceCity = setting.PriceCity;
                PriceCountry = setting.PriceCountry;
                PriceWorld = setting.PriceWorld;
                AddInfo = setting.AddInfo;
            }
        }
        public override void SaveToIni()
        {
            if (ini.Setting is SettingMeshok setting)
            {
                setting.PassWrd = PassWrd;
                setting.Curs = Curs;
                setting.Comission = Comission;
                setting.PriceCity = PriceCity;
                setting.PriceCountry = PriceCountry;
                setting.PriceWorld = PriceWorld;
                setting.AddInfo = AddInfo;
                ini.Save();
            }
        }
    }
}
