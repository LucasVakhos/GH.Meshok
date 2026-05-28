using Common;
using GH.Configs;
using GH.Helpers;
using GH.NHibernate;
using GH.Utils;
using MeshokBrowser.Helpers;
using MeshokBrowser.NHibernate;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
namespace MeshokBrowser
{
    public static class FbHelper
    {
        static int _siteNo = -1;
        public static void Init()
        {
            if (_siteNo > 0)
                return;
            SetSiteNo();
            AddSiteStatusesToBase();
            GetRelationsStatusesFromBase();
        }
        private static void SetSiteNo()
        {
            INHRepository repository = new NHRepository<CheckSite>();
            repository.GetParams = () =>
            {
                Dictionary<string, object> valuePairs = new Dictionary<string, object>
                {
                    { nameof(CheckSite.Name), IniHelper.Cfg<CfgMeshok>().SiteName }
                };
                return valuePairs;
            };
            CheckSite data = (CheckSite)repository.SelectOne();
            _siteNo = data.id;
        }
        private static void GetRelationsStatusesFromBase()
        {
            INHRepository repository = new NHRepository<BaseStatus>();
            repository.GetSorting = () =>
            {
                Dictionary<string, bool> valuePairs = new Dictionary<string, bool>
                {
                    { "id", true }
                };
                return valuePairs;
            };
            StatusRelation.SetStatusRels(repository.SelectAll());
        }
        private static void AddSiteStatusesToBase()
        {
            List<string> list = new List<string>();
            foreach (OrderStatus item in Enum.GetValues(typeof(OrderStatus)))
            {
                list.Add($"execute procedure z$import_statuses({_siteNo}, {item.ToInt()}, '{item.GetDisplayValue()}')");
            }
            INHRepository repository = new NHRepository<CheckSite>();
            repository.ExequteQuery(list.ToArray());
        }
        public static void AddOrderLine(OrderLine orderLine)
        {
            INHRepository repository = new NHRepository<CheckOrder>();
            repository.GetSQL = (sqlTypes, protoEntity) =>
            {
                return $"select * from z$check_order({_siteNo}, {orderLine.deal_id})";
            };
            if (!(repository.SelectOne() is CheckOrder check))
                check = CheckOrder.NewCheckOrder(orderLine);
            orderLine.Check = check;
            try
            {
                AddToPacket(orderLine, check);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }
        private static void AddToPacket(OrderLine orderLine, CheckOrder check)
        {
            DeliveryPacket packet = AllPackets.GetPacket(orderLine, check);
            packet.AddOrderLine(orderLine, check);
        }
        public static void SetOtherInfos(ScanStatus scanStatus)
        {
            switch (scanStatus)
            {
                case ScanStatus.ScanLostDeals:
                case ScanStatus.ScanNew:
                    SetClients();
                    break;
                case ScanStatus.ScanNotNew:
                case ScanStatus.ScanNewMess:
                    SetDealMessages();
                    break;
                default:
                    break;
            }
        }
        private static void SetClients()
        {
            INHRepository repository = new NHRepository<CheckClient>();
            foreach (Client c in AllPackets.ForPostClients)
            {
                repository.GetSQL = (sqlTypes, protoEntity) =>
                {
                    return $"select * from z$import_clients_inf({_siteNo}, {c.site_id})";
                };
                try
                {
                    CheckClient info = repository.SelectOne() as CheckClient;
                    c.base_info = info;
                }
                catch (Exception ex)
                {
                    Logger.Error(ex);
                }
            }
        }
        private static void SetDealMessages()
        {
            foreach (OrderLine orderLine in AllPackets.OrderLines)
            {
                SetDealMessage(orderLine);
            }
        }
        public static void SetDealMessage(OrderLine orderLine)
        {
            MessageCase messageCase = MessageCase.AnyCase;
        from_begin:
            switch (orderLine.CurrStatus)
            {
                case OrderStatus.New:
                case OrderStatus.Discution:
                    /*
                    if (orderLine.need_split)
                        messageCase = MessageCase.NeedSplit;
                    else
                    */
                        messageCase = MessageCase.EmailCheck;
                    break;
                default:
                    if (orderLine.Check.dp_packed)
                        messageCase = MessageCase.DocClosed;
                    break;
            }
            INHRepository repository = new NHRepository<CheckMesage>();
            repository.GetSQL = (sqlTypes, protoEntity) =>
            {
                return $"select * from z$check_message({_siteNo}, {orderLine.deal_id}, { messageCase.ToInt()})";
            };
            foreach (CheckMesage mesage in repository.SelectAll())
            {
                try
                {
                    mesage.SetDealMessagesFor(orderLine);
                }
                catch (Exception ex)
                {
                    Logger.Error(ex);
                }
            }
            if (messageCase == MessageCase.NeedSplit)
            {
                orderLine.need_split = false;
                goto from_begin;
            }
        }
        public static bool AddToDatabase(ScanHelper scanHelper)
        {
            INHRepository repository = new NHRepository<ClientCallBack>();
            foreach (Client c in AllPackets.ForPostClients)
            {
                string sql = "select c_id from z$import_clients" +
                    $"({_siteNo}, :site_id, :md_id, :mp_id, :c_name, :c_phone, :c_email, :c_zipcode, :site_address, :change_address)";
                try
                {
                    ClientCallBack b = repository.SelectFormProcedure(c, sql) as ClientCallBack;
                    if (b.id != c.base_id)
                        c.base_id = b.id;
                }
                catch (Exception ex)
                {
                    Logger.Error(ex);
                    return false;
                }
            }
            repository = new NHRepository<TitleCallBack>();
            foreach (Title t in AllPackets.ForPostTitles)
            {
                SetTitleToRelist(t);
                string sql = "select t_id, ts_id " +
                    "from z$import_t(:t_base_id, :t_bar_code, :t_artist, :t_title, :t_year, " +
                    ":l_name, :m_name, :ctr_name, :st_name, :ts_st_no, :ts_st_id, :ts_quality, :t_price)";
                try
                {
                    TitleCallBack b = repository.SelectFormProcedure(t, sql) as TitleCallBack;
                    t.t_id = b.id;
                    t.ts_id = b.ts_id;
                }
                catch (Exception ex)
                {
                    Logger.Error(ex);
                    return false;
                }
            }
            repository = new NHRepository<OrderCallBack>();
            foreach (Order o in AllPackets.ForPostOrders)
            {
                o.RetriveFields();
                string sql = "select co_id from z$import_co(:c_id, :co_date, :md_id, :mp_id, :co_curs)";
                try
                {
                    OrderCallBack b = repository.SelectFormProcedure(o, sql) as OrderCallBack;
                    o.base_id = b.id;
                }
                catch (Exception ex)
                {
                    Logger.Error(ex);
                    return false;
                }
            }
            repository = new NHRepository<OrderLineCallBack>();
            foreach (OrderLine l in AllPackets.ForPostOrderLines)
            {
                string sql = "select cod_id " +
                    $"from z$import_cod({_siteNo}, {l.deal_id}, :co_id, :t_id, :ts_id, :qty, :price, :commition, :price_ru)";
                try
                {
                    repository.SelectFormProcedure(l, sql);
                }
                catch (Exception ex)
                {
                    Logger.Error(ex);
                    return false;
                }
            }
            return true;
        }
        public static void SetIdToRelist(int st_no, int base_id, int st_id )
        {
            SetTitleToRelist(new Title(st_no, base_id, st_id));
        }
        private static void SetTitleToRelist(Title t)
        {
            string sql = $"CALL meshok_relist_set({t.ts_st_no}, {t.t_base_id}, {t.ts_st_id})";
            try
            {
                using (MySqlConnection sqlConnection = CfgBridgeNote.CreateConnection())
                {
                    sqlConnection.Open();
                    using (MySqlTransaction trans = sqlConnection.BeginTransaction())
                    {
                        using (MySqlCommand cmd = new MySqlCommand(sql, sqlConnection, trans))
                        {
                            cmd.ExecuteNonQuery();
                            trans.Commit();
                        }
                    }
                    sqlConnection.Clone();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }
        public static bool HasInBase(string deal_id)
        {
            INHRepository repository = new NHRepository<CheckSite>();
            repository.GetSQL = (sqlTypes, protoEntity) => $"select * from z$site_id_s({_siteNo}, {deal_id})";
            try
            {
                var res = repository.SelectOne();
                return res != null && res.id > 0;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return false;
        }
    }
}
