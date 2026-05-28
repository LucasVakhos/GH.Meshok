using GH.Attributes;
using System.ComponentModel.DataAnnotations;
namespace MeshokBrowser.NHibernate
{
    public class Title : BaseScanEntity
    {
        //"Баркод",
        [Display(Name = "Баркод")]
        [UpdatableProperty(Caption = "Баркод")]
        public string t_bar_code { get; set; }
        //"Артист",
        [Display(Name = "Артист")]
        [UpdatableProperty(Caption = "Артист")]
        public string t_artist { get; set; }
        //"Альбом",
        [Display(Name = "Альбом")]
        [UpdatableProperty(Caption = "Альбом")]
        public string t_title { get; set; }
        //"Год издания",
        [Display(Name = "Год издания")]
        [UpdatableProperty(Caption = "Год издания")]
        public string t_year { get; set; }
        //"Лейбл",
        [Display(Name = "Лейбл")]
        [UpdatableProperty(Caption = "Лейбл")]
        public string l_name { get; set; }
        //"Формат",
        [Display(Name = "Формат")]
        [UpdatableProperty(Caption = "Формат")]
        public string m_name { get; set; }
        //"Страна",
        [Display(Name = "Страна")]
        [UpdatableProperty(Caption = "Страна")]
        public string ctr_name { get; set; }
        //"Стиль музыки",
        [Display(Name = "Стиль музыки")]
        [UpdatableProperty(Caption = "Стиль музыки")]
        public string st_name { get; set; }
        //"Качество",
        [Display(Name = "Качество")]
        [UpdatableProperty(Caption = "Качество")]
        public string ts_quality { get; set; }
        //"Артикул"
        private string _artArticul = "";
        [Display(Name = "Артикул")]
        [UpdatableProperty(Caption = "Артикул")]
        public string artArticul
        {
            get => _artArticul;
            set
            {
                _artArticul = value;
                ParsingSaccess = !string.IsNullOrEmpty(_artArticul);
                if (ParsingSaccess)
                {
                    string art = _artArticul;
                    for (int i = 0; i < 6; i++)
                    {
                        if (art == "")
                            break;
                        if (i == 0)// skl
                        {
                            string val;
                            int pos = art.IndexOf("-");
                            if (pos == -1)
                            {
                                val = art;
                                art = "";
                            }
                            else
                            {
                                val = art.Substring(0, pos);
                                pos++;
                                art = art.Substring(pos);
                            }
                            int.TryParse(val, out _ts_st_no);
                        }
                        else if (i == 1)//code
                        {
                            string val;
                            int pos = art.IndexOf("-");
                            if (pos == -1)
                            {
                                val = art;
                                art = "";
                            }
                            else
                            {
                                val = art.Substring(0, pos);
                                pos++;
                                art = art.Substring(pos);
                            }
                            int.TryParse(val, out _t_base_id);
                        }
                        else if (i == 2)//skl_id
                        {
                            string val;
                            int pos = art.IndexOf("-");
                            if (pos == -1)
                            {
                                val = art;
                                art = "";
                            }
                            else
                            {
                                val = art.Substring(0, pos);
                                pos++;
                                art = art.Substring(pos);
                            }
                            int.TryParse(val, out _ts_st_id);
                        }
                        else if (i == 3)//curs
                        {
                            string val;
                            int pos = art.IndexOf(":");
                            if (pos == -1)
                            {
                                val = art;
                                art = "";
                            }
                            else
                            {
                                val = art.Substring(0, pos);
                                pos++;
                                art = art.Substring(pos);
                            }
                            if (double.TryParse(val, out _curs))
                            {
                                _curs = _curs / 100;
                            }
                        }
                        else if (i == 4)//price
                        {
                            string val;
                            int pos = art.IndexOf(":");
                            if (pos == -1)
                            {
                                val = art;
                                art = "";
                            }
                            else
                            {
                                val = art.Substring(0, pos);
                                pos++;
                                art = art.Substring(pos);
                            }
                            if (double.TryParse(val, out _t_price))
                            {
                                _t_price = _t_price / 100;
                            }
                        }
                        else if (i == 5)//_comission
                        {
                            string val;
                            int pos = art.IndexOf(":");
                            if (pos == -1)
                            {
                                val = art;
                                art = "";
                            }
                            else
                            {
                                val = art.Substring(0, pos);
                                pos++;
                                art = art.Substring(pos);
                            }
                            if (double.TryParse(val, out _commision))
                            {
                                _commision = _commision / 100;
                            }
                        }
                    } //1-233232-0-_curs:price:_comission
                }
            }
        }
        private double _curs = 0;
        public double t_curs { get => _curs; set => _curs = value; }
        private double _commision = 0;
        public double t_commision { get => _commision; set => _commision = value; }
        public int t_id { get; set; }
        private int _ts_st_no = 0;
        public int ts_st_no { get => _ts_st_no; }
        private int _t_base_id = 0;
        public int t_base_id { get => _t_base_id; }
        private int _ts_st_id = 0;
        public int ts_st_id { get => _ts_st_id; }
        public int ts_id { get; set; }
        private double _t_price = 0;
        public double t_price { get => _t_price; }
        public OrderLine orderLine { get; set; }
        public override bool Equals(object obj)
        {
            var title = obj as Title;
            return title != null &&
                   base.Equals(obj) &&
                   Url == title.Url;
        }
        public override int GetHashCode()
        {
            return 624022166 + base.GetHashCode();
        }
        public Title()
        {
        }
        public Title(int st_no, int base_id, int st_id)
        {
            _ts_st_no = st_no;
            _t_base_id = base_id;
            _ts_st_id = st_id;
        }
    }
}
