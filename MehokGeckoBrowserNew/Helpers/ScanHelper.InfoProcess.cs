//#define SKIP_CHANGE_STATUS
//#define SKIP_ADD_TO_BASE
using Common;
using Gecko;
using Gecko.Collections;
using Gecko.DOM;
using Gecko.Events;
using GH.Components;
using GH.Helpers;
using GH.Utils;
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
        public void ProcessCollectedOrders()
        {
            switch (ScanStatus)
            {
                case ScanStatus.ScanLostDeals:
                case ScanStatus.ScanNew:
                case ScanStatus.ScanNotNew:
                    ProcessOrders();
                    return;
                default:
                    return;
            }
        }
        private void ProcessOrders()
        {
#if TEST_NEXT_STATUS
            foreach (OrderLine orderLine in AllPackets.ForPostOrderLines.Where(x => x.site_status == OrderStatus.New))
                SetNextStatus(orderLine);
#else
            if (ScanStatus == ScanStatus.ScanLostDeals)
            {
                if (!ProcessorSettingHelper<ScanSettingLostDeals>.Check())
                {
                    ScanStatus = ScanStatus.Finished;
                    return;
                }
                foreach (OrderLine orderLine in AllPackets.OrderLines.Where(x => !x.NeedAdd).ToList())
                {
                    orderLine.Order.OrderLines.Remove(orderLine);
                    AllPackets.RemoveOrderLine(orderLine);
                }
                foreach (Order pack in AllPackets.Orders.Where(x => x.OrderLines.Count == 0).ToList())
                    AllPackets.Orders.Remove(pack);
            }
            if (ScanStatus == ScanStatus.ScanLostDeals || ScanStatus == ScanStatus.ScanNew)
            {
                if (!ProcessorSettingHelper<ScanSettingClients>.Check())
                {
                    ScanStatus = ScanStatus.Finished;
                    return;
                }
                if (AddToDatabase())
                {
                    SendFirstMessage();
                    foreach (OrderLine orderLine in AllPackets.ForPostOrderLines.Where(x => x.site_status == OrderStatus.New && x.CurrStatus < OrderStatus.DealOK))
                        SetNextStatus(orderLine);
                }
            }
            else if (ScanStatus == ScanStatus.ScanNotNew)
            {
                try
                {
                    Wait(GetPorcessTasks());
                }
                catch (Exception ex)
                {
                    Logger.Error(ex);
                    ScanStatus = ScanStatus.Finished;
                    return;
                }
                foreach (OrderLine orderLine in AllPackets.ForPostOrderLines.Where(x => x.CurrStatus < OrderStatus.DealOK && x.CurrStatus > x.site_status))
                    SetNextStatus(orderLine);
            }
#endif
            IncScanStatus();
            _processor.IncCurrentStep();
        }
        private List<DisposableTask> GetPorcessTasks()
        {
            List<DisposableTask> result = new List<DisposableTask>();
            switch (ScanStatus)
            {
                case ScanStatus.ScanNotNew:
                    if (AllPackets.OrderLines.Any(x => x.HasMessages && !(x.CurrStatus == OrderStatus.DealOK && x.Check.dp_mp_id == 2 /*квитанция сбербанка*/)))
                        result.Add(new TaskPostMessage());
                    if (AllPackets.OrderLines.Any(x => (x.CurrStatus == OrderStatus.DealOK && x.Check.dp_mp_id != 2 /*квитанция сбербанка*/ ) || x.CurrStatus == OrderStatus.DealCanceled))
                        result.Add(new TaskCloseDeals());
                    break;
                case ScanStatus.ScanNewMess:
                    if (AllPackets.OrderLines.Any(x => x.HasMessages))
                        result.Add(new TaskPostMessage());
                    break;
                default:
                    break;
            }
            SetTaskToOrderLienes(result);
            return result;
        }
        private bool AddToDatabase()
        {
#if SKIP_ADD_TO_BASE
            return true;
#else
            return FbHelper.AddToDatabase(this);
#endif
        }
        private void SendFirstMessage()
        {
            /*
            foreach (OrderLine item in AllPackets.Splited)
            {
                OrderLine orderLine = AllPackets.OrderLines.Where(x => x.deal_id == item.deal_id).FirstOrDefault();
                if (orderLine != null)
                {
                    orderLine.need_split = true;
                }
            }
            */
            int cnt = 0;
            foreach (OrderLine orderLine in AllPackets.OrderLines)
            {
                FbHelper.SetDealMessage(orderLine);
                if (orderLine.HasMessages)
                    cnt++;
            }
            if (cnt > 0)
            {
                ScanStatus saveStatus = ScanStatus;
                ScanStatus = ScanStatus.ScanNewMess;
                Wait(GetPorcessTasks());
                ScanStatus = saveStatus;
            }
        }
        private void SetNextStatus(OrderLine orderLine)
        {
#if TEST_EMAIL_MESSAGE || SKIP_CHANGE_STATUS
            return;
#else
            GeckoHtmlElement main = null;
            foreach (GeckoHtmlElement item in orderLine.HtmlRow.GetElementsByTagName("div"))
            {
                if (item.ClassName == "deal_status_info")
                {
                    main = item;
                    break;
                }
            }
            if (main == null)
                return;
            OrderStatus status = orderLine.CurrStatus;
            if (status == OrderStatus.New)
                status++;
            foreach (GeckoHtmlElement cur in main.GetElementsByTagName("div"))
            {
                if (cur.ClassName == "d_s_e_i" && cur.GetAttribute("rel") == orderLine.deal_id)
                {
                    cur.Click();
                    Application.DoEvents();
                    GeckoSelectElement sel = main.GetElementsByTagName("select").FirstOrDefault() as GeckoSelectElement;
                    sel.Focus();
                    Application.DoEvents();
                    sel.Value = status.GetDisplayValue();
                    GeckoHelper.RaiseHTMLEvent(webDocument, sel, "change");
                    Application.DoEvents();
                    return;
                }
            }
#endif
        }
    }
}
