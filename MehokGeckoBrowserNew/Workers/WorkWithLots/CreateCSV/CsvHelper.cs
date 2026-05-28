using MySql.Data.MySqlClient;
using System;
namespace MeshokBrowser.Workers
{
    public static class CsvHelper
    {
        public const string file_mask = "мешок{0}.txt";
        static string _url_cover_folder = "http://img.bridgenote.com/covers/";
        static string _url_null_cover = "http://img.bridgenote.com/interface/audio-cd.jpg";
        static double _curs = 0;
        static double _comission = 0;
        static double _priceCity = 0;
        static double _priceCountry = 0;
        private static string GetCaption(string param)
        {
            switch (param.ToUpper())
            {
                case "BAR_CODE":
                    return "Баркод";
                case "ARTIST":
                    return "Артист";
                case "TITLE":
                    return "Альбом";
                case "YEAR_OF":
                    return "Год издания";
                case "L_NAME":
                    return "Лейбл";
                case "M_NAME":
                    return "Формат";
                case "CTR_NAME":
                    return "Страна";
                case "ST_NAME":
                    return "Стиль музыки";
                case "QUALITY":
                    return "Качество";
                case "ART_ID":
                    return "Артикул";
                default:
                    return "";
            }
        }
        public static string CreateCSVLine(MySqlDataReader reader)
        {
            const string _dev = "|";
            double price = reader.GetDouble(reader.GetOrdinal("PRICE"));
            string ArtValue(double val)
            {
                val = val * 100;
                return ((int)val).ToString();
            }
            string PreparePrice(double val)
            {
                val = Math.Ceiling((val / (100 - _comission) * 100) * _curs);
                return val.ToString();
            }
            string LotName()
            {
                string artist = reader["ARTIST"].ToString();
                string title = reader["TITLE"].ToString();
                string format = reader["M_NAME"].ToString();
                string quality = reader["QUALITY"].ToString();
                string barcode = reader["BAR_CODE"].ToString();
                return LotNamesHelper.CreateLotName(artist, title, format, quality, barcode);
            }
            string FullInfoTable()
            {
                const string _row_mask = "<tr><td width=\"100\" align=\"right\">{0}</td><td>{1}</td></tr>";
                string full_info = "<table width=\"100%\" border=\"0\" cellspacing=\"2\">";
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    bool stop = false;
                    string capt = GetCaption(reader.GetName(i));
                    string val = LotNamesHelper.SuppresNonCSVChars(reader[i].ToString());
                    stop = (reader[i] == reader["ART_ID"]);
                    if (stop)
                        val += "-" + ArtValue(_curs) + ":" + ArtValue(price) + ":" + ArtValue(_comission);
                    full_info += string.Format(_row_mask, capt, val);
                    if (stop)
                        break;
                }
                return full_info + "</table>";
            }
            string Token()
            {
                string token;
                if (reader["CATEGORY"].ToString() == "")
                    token = LotNamesHelper.SuppresNonCSVChars(reader["M_NAME"].ToString(), true);
                else
                    token = LotNamesHelper.SuppresNonCSVChars(reader["CATEGORY"].ToString(), true);
                return token;
            }
            string Picture()
            {
                string picture;
                if (reader["PICT"].ToString() == "")
                    picture = _url_null_cover;
                else
                    picture = _url_cover_folder + reader["PICT"].ToString();
                return picture;
            }
            string GoodState()
            {
                string[] _yesno = { "N", "Y" };
                return _yesno[reader.GetInt16(reader.GetOrdinal("NEW"))];
            }
            string result = "F";
            //Код отдела
            result += _dev + "2881";
            //Наименование
            result += _dev + LotName();
            //Описание                
            result += _dev + FullInfoTable();
            //Метки 
            result += _dev + Token();
            //Состояние товара  
            result += _dev + GoodState();
            //Количество 
            result += _dev + "1";
            //Фиксированная цена
            result += _dev + PreparePrice(price);
            //Валюта  
            result += _dev + "RUR";
            //Продолжительность  
            result += _dev + "100";
            //Доставка по городу
            result += _dev + _priceCity.ToString();
            //Доставка по стране
            result += _dev + _priceCountry.ToString();
            //Уведомления
            result += _dev + "Y";
            //URL картинки
            result += _dev + Picture();
            return result;
        }
        public static void InitVars(CfgMeshok cfgMeshok)
        {
            _curs = cfgMeshok.Curs;
            _comission = cfgMeshok.Comission;
            _priceCity = cfgMeshok.PriceCity;
            _priceCountry = cfgMeshok.PriceCountry;
        }
        public static string NameFile(int n)
        {
            return string.Format(file_mask, n.ToString().PadLeft(3, '0'));
        }
    }
}
