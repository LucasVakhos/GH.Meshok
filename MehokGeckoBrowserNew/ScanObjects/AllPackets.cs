using Gecko;
using MeshokBrowser.Helpers;
using System.Collections.Generic;
using System.Linq;
namespace MeshokBrowser.NHibernate
{
    public static class AllPackets
    {
        private static readonly List<DeliveryPacket> _deliveryPackets = new List<DeliveryPacket>();
        public static List<DeliveryPacket> DeliveryPackets => _deliveryPackets;
        public static void AddPackket(DeliveryPacket deliveryPacket)
        {
            if (!DeliveryPackets.Contains(deliveryPacket))
                DeliveryPackets.Add(deliveryPacket);
        }
        public static void RemovePackket(DeliveryPacket deliveryPacket)
        {
            DeliveryPackets.Remove(deliveryPacket);
        }
        public static DeliveryPacket GetPacket(OrderLine orderLine, CheckOrder check)
        {
            DeliveryPacket packet = DeliveryPackets.SingleOrDefault(x => x.base_id == check.dp_id &&
                x.md_id == check.dp_md_id && x.mp_id == check.dp_mp_id && x.c_id == check.dp_c_id && x.nick == orderLine.nick);
            if (packet == null)
            {
                packet = new DeliveryPacket(orderLine, check);
                AddPackket(packet);
            }
            return packet;
        }
        public static List<Order> Orders
        {
            get => DeliveryPackets.SelectMany(x => x.Orders).ToList();
        }
        public static void Clear()
        {
            _splited.Clear();
            _orderLines.Clear();
            _deliveryPackets.Clear();
        }
        private static readonly List<OrderLine> _splited = new List<OrderLine>();
        public static List<OrderLine> Splited
        {
            get => _splited;
        }
        private static readonly List<OrderLine> _orderLines = new List<OrderLine>();
        public static List<OrderLine> OrderLines
        {
            get => _orderLines.ToList();
        }
        public static void AddOrderLine(OrderLine line)
        {
            _orderLines.Add(line);
        }
        public static void RemoveOrderLine(OrderLine line)
        {
            _orderLines.Remove(line);
        }
        public static List<Order> ForPostOrders
        {
            get => DeliveryPackets.
                Where(o => o.OrderLines.Where(c => c.Client != null).Count() > 0).
                SelectMany(x => x.Orders).ToList();
        }
        public static List<OrderLine> ForPostOrderLines
        {
            get => OrderLines;
        }
        public static List<Title> ForPostTitles
        {
            get => OrderLines.Where(x => x.Title != null).Select(x => x.Title).Distinct().ToList();
        }
        public static List<Client> ForPostClients
        {
            get => OrderLines.Where(x => x.Client != null).Select(x => x.Client).Distinct().ToList();
        }
        public static bool NeedSplit => OrderLines.Any(x => x.need_split);
        public static OrderLine GetOrderLine(ScanParams scanParams, GeckoHtmlElement row)
        {
            OrderLine orderLine = OrderLines.SingleOrDefault(x => x.deal_id == scanParams.deal_id);
            if (orderLine == null)
            {
                orderLine = new OrderLine(scanParams, row);
                AddOrderLine(orderLine);
            }
            else
                orderLine.SetOrderLine(scanParams, row);
            return orderLine;
        }
        public static void CreatePackets()
        {
            foreach (int site_id in OrderLines.Where(x => x.Client != null).Select(x => x.Client).Select(c => c.site_id).Distinct())
            {
                Client client_base = OrderLines.Select(x => x.Client).Where(c => c.site_id == site_id && c.IsComplete).FirstOrDefault();
                if (client_base == null)
                    client_base = OrderLines.Select(x => x.Client).Where(c => c.site_id == site_id).FirstOrDefault();
                if (client_base != null)
                {
                    foreach (OrderLine orderLine in OrderLines.Where(x => x.Client.site_id == site_id && x.Client.Url != client_base.Url))
                        orderLine.Client = client_base;
                }
            }
            foreach (OrderLine orderLine in OrderLines)
                FbHelper.AddOrderLine(orderLine);
        }
        //public static string[] AllSplitingNicks()
        //{
        //        List<string> res = new List<string>();
        //        foreach (OrderLine item in OrderLines.Where(x => x.need_split))
        //        {
        //            res.Add(item.nick);
        //        }
        //        return res.Distinct().ToArray<string>();
        //}
    }
}
