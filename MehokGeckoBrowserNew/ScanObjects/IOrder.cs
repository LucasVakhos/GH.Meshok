using MeshokBrowser.NHibernate;
using System;
using System.Collections.Generic;
namespace Common
{
    public interface IOrder
    {
        int base_id { get; set; }
        DateTime? date { get; set; }
        double total { get; set; }
        int md_id { get; set; }
        DeliveryMethod deliveryMethod { get; }
        int mp_id { get; set; }
        PaymentMethod paymentMethod { get; }
        int base_status { get; set; }
        bool packed { get; set; }
        int c_id { get; set; }
        Client Client { get; set; }
        List<Order> Orders { get; }
        List<OrderLine> OrderLines { get; }
        bool NeedUnion { get; }
        void Clear();
    }
}
