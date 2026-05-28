using Common;
using Gecko;
using Gecko.Collections;
using Gecko.DOM;
using Gecko.Events;
using GH.Helpers;
using MeshokBrowser.NHibernate;
using MeshokBrowser.Workers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
namespace MeshokBrowser.Helpers
{
    public partial class ScanHelper
    {
        public bool CollectOrders()
        {
            switch (ScanStatus)
            {
                case ScanStatus.ScanLostDeals:
                case ScanStatus.ScanNotNew:
                case ScanStatus.ScanNew:
                    return CollectOrdersRows();
                default:
                    break;
            }
            return false;
        }
        private bool CollectOrdersRows()
        {
            // очищаем список посылок
            AllPackets.Clear();
            // метка "Был ли Spliting в OrderLines"
            //bool Spliting = false;
        //string[] allSplitingNicks = new string[0];
        from_begin:
            if (!ProcessRunHelper.Executing)
                // запрещаем дальнейшую обработку
                return false;
            // заполняем scanParams.scan_form и scanParams.scan_table
            CollectForm();
            ScanParams scanParams = ScanParams.Instance();
            if (scanParams.scan_table == null)
                // документ пустой
                return false;
            // получает все ряды в документе
            IEnumerable<GeckoElement> rows = scanParams.scan_table.GetElementsByTagName("tr").
                    Where(x => x.GetAttribute("class") == "r1" || x.GetAttribute("class") == "r2");
            if (rows != null || rows.Count() > 0)
            {
                // если SplitingWay открыт;
                /*
                if (Spliting)
                {
                    //allSplitingNicks = AllPackets.AllSplitingNicks();
                    // только новые и потерянные
                    rows = rows.Where(x => x.GetElementsByTagName("div").Count() > 0
                            && (x.GetElementsByTagName("div").Where(d => d.GetAttribute("class") == "d_s_e_i" &&
                               d.TextContent == OrderStatus.Discution.GetDisplayValue() || d.TextContent == OrderStatus.New.GetDisplayValue())
                            .Count() > 0));
                }
                else
                {
                */
                    switch (ScanStatus)
                    {
                        case ScanStatus.ScanLostDeals:
                            // только потерянные 
                            rows = rows.Where(x => x.GetElementsByTagName("div").Count() > 0
                                    && (x.GetElementsByTagName("div").Where(d => d.GetAttribute("class") == "d_s_e_i" &&
                                       d.TextContent == OrderStatus.Discution.GetDisplayValue() || d.TextContent == OrderStatus.New.GetDisplayValue())
                                    .Count() > 0));
                            break;
                        case ScanStatus.ScanNew:
                            // только новые
                            rows = rows.Where(x => x.GetElementsByTagName("div").Count() > 0
                                && (x.GetElementsByTagName("div").Where(d => d.GetAttribute("class") == "d_s_e_i" &&
                                   d.TextContent == OrderStatus.New.GetDisplayValue())
                                .Count() > 0));
                            break;
                        default:
                            break;
                    }
                //}
            }
            if (rows == null || rows.Count() == 0)
                // нет таких лотов
                return false;
            foreach (GeckoHtmlElement row in rows)
            {
                if (!ProcessRunHelper.Executing)
                    // запрещаем дальнейшую обработку
                    return false;
                // сканирование
                CollectOrdersRow(row);
#if USE_TEST_LABEL
                if (!scanParams.Is_test)
                {
                    // не тестовая запись
                    continue;
                }
                else
#else
                if (scanParams.Is_test)
                {
                    //тестовая запись
                    continue;
                }
                else
#endif
                if (ScanStatus == ScanStatus.ScanLostDeals && FbHelper.HasInBase(scanParams.deal_id))
                {
                    // не потеряна
                    continue;
                }
                else
                if (ScanStatus == ScanStatus.ScanNotNew && !FbHelper.HasInBase(scanParams.deal_id))
                {
                    // нет в базе
                    continue;
                }
                OrderLine orderLine = AllPackets.GetOrderLine(scanParams, row);
            }
            if (AllPackets.OrderLines.Count == 0)
                // запрещаем дальнейшую обработку
                return false;
#if TEST_NEXT_STATUS
            return true;
#else
            /*
            if (ScanStatus == ScanStatus.ScanLostDeals || ScanStatus == ScanStatus.ScanNew)
            {
                // если SplitingWay=false и есть x.need_split
                if (!Spliting && AllPackets.NeedSplit)
                {
                    // открываем SplitingWay
                    Spliting = SplitOrders();
                    if (Spliting)
                        // сканируем по новой
                        goto from_begin;
                }
                else
                    // закрываем SplitingWay
                    Spliting = false;
            }
            */
            if (!ProcessRunHelper.Executing)
                // запрещаем дальнейшую обработку
                return false;
            // исполняем все сканирования
            Wait(GetCollectTasks());
            if (!ProcessRunHelper.Executing)
                // запрещаем дальнейшую обработку
                return false;
            // формируем лист заказов по слиентам
            AllPackets.CreatePackets();
            if (ScanStatus == ScanStatus.ScanNotNew && Union())
            {
                // очищаем список посылок
                AllPackets.Clear();
                goto from_begin;
            }
            // получам данные из базы данных
            FbHelper.SetOtherInfos(ScanStatus);
            // разрешаем дальнейшую обработку
            return true;
#endif
        }
        protected void CollectForm()
        {
            ScanParams sp = ScanParams.Instance(true);
            foreach (GeckoFormElement item in webDocument.Forms.Where(x => x.Id == "form2" &&
                x.GetAttribute("method") == "POST" &&
                x.GetElementsByTagName("table") != null
                ))
            {
                if (!ProcessRunHelper.Executing)
                    return;
                foreach (GeckoHtmlElement table in item.GetElementsByTagName("table"))
                {
                    if (table.ClassName == "standart_listing")
                    {
                        sp.scan_table = table;
                        sp.scan_form = item;
                        return;
                    }
                }
            }
        }
        protected void CollectOrdersRow(GeckoHtmlElement row)
        {
            ScanParams scanParams = ScanParams.Instance();
            scanParams.NewScan();
#if USE_TEST_LABEL
            foreach (GeckoHtmlElement div in row.GetElementsByTagName("div").Where(x => x.GetAttribute("class") == "deal_note"))
            {
                if (div.TextContent.ToUpper().Contains("TEST"))
                {
                    scanParams.Is_test = true;
                    break;
                }
            }
            if (!scanParams.Is_test)
                return;
#endif
            foreach (GeckoHtmlElement item in row.GetElementsByTagName("td"))
            {
                IDomHtmlCollection<GeckoElement> HrefCollettion = item.GetElementsByTagName("a");
                IDomHtmlCollection<GeckoElement> DivCollettion = item.GetElementsByTagName("div");
                if (string.IsNullOrEmpty(scanParams.deal_url) && HrefCollettion.Count() == 0)
                    continue;
                if (string.IsNullOrEmpty(scanParams.deal_url) && HrefCollettion.Count() == 2)
                {
                    foreach (GeckoElement a in item.GetElementsByTagName("a"))
                    {
                        string url = a.GetAttribute("href");
                        if (url.Contains("item"))
                            scanParams.title_url = url;
                        else
                        if (url.Contains("deal"))
                        {
                            scanParams.deal_id = a.TextContent;
                            scanParams.deal_url = url;
                            scanParams.need_split = Splits.Contains(url);
                        }
                    }
                    continue;
                }
                if (string.IsNullOrEmpty(scanParams.deal_title) && HrefCollettion.Count() == 1 && HrefCollettion.First().GetAttribute("href") == scanParams.deal_url)
                {
                    scanParams.deal_title = item.GetElementsByTagName("a").First().TextContent;
                    //scanParams.need_split = item.GetElementsByTagName("span").Where(x => x.GetAttribute("class") == "atten").FirstOrDefault() != null;
                    continue;
                }
                if (string.IsNullOrEmpty(scanParams.deal_title))
                    continue;
                if (string.IsNullOrEmpty(scanParams.price))
                {
                    if (item.ClassName == "list r")
                        scanParams.price = item.TextContent.Trim();
                }
                else
                if (string.IsNullOrEmpty(scanParams.date))
                {
                    if (item.ClassName == "list r")
                        scanParams.date = item.TextContent.Trim();
                }
                else
                if (string.IsNullOrEmpty(scanParams.c_nic) && item.GetElementsByTagName("nobr").Count() > 0)
                {
                    scanParams.c_nic = item.GetElementsByTagName("nobr").First().TextContent.Trim();
                }
                else
                if (string.IsNullOrEmpty(scanParams.deal_status) && item.ClassName == "list l"
                    && DivCollettion.Where(x => x.GetAttribute("class") == "deal_status_info").Count() > 0)
                {
                    scanParams.deal_status = DivCollettion.Where(x => x.GetAttribute("class") == "d_s_e_i").First().TextContent.Trim();
                }
            }
        }
        private List<DisposableTask> GetCollectTasks()
        {
            List<DisposableTask> result = new List<DisposableTask>();
            switch (ScanStatus)
            {
                case ScanStatus.ScanLostDeals:
                case ScanStatus.ScanNew:
                    if (AllPackets.ForPostClients.Any(x => !x.IsComplete))
                        result.Add(new TaskClientParse());
                    if (AllPackets.ForPostTitles.Any(x => x.t_base_id == 0))
                        result.Add(new TaskTitleParse());
                    break;
                default:
                    break;
            }
            SetTaskToOrderLienes(result);
            return result;
        }
    }
}
