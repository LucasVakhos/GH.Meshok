using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FirebirdSql.Data.FirebirdClient;
using MeshokBrowser.Objects;
namespace MeshokBrowser
{
    public class StatusRelation
    {
        public int base_status_id;
        public int DeliveryMethod;
        public int PaymentMethod;
        public OrderStatus OrderStatus;
        public StatusRelation(int base_status_id, DeliveryMethod deliveryMethod, PaymentMethod paymentMethod, OrderStatus orderStatus)
        {
            this.base_status_id = base_status_id;
            DeliveryMethod = (int)deliveryMethod;
            PaymentMethod = (int)paymentMethod;
            OrderStatus = orderStatus;
        }
        public override string ToString()
        {
            return $"{base_status_id} - {DeliveryMethod} - {PaymentMethod} - {OrderStatus}";
        }
    }
    public class ReadHelperBaseStatus : ReaderHelperBase
    {
        List<StatusRelation> statusRels = new List<StatusRelation>();
        int base_status_id;
        public ReadHelperBaseStatus(FbDataReader recs) : base(recs)
        {
        }
        public override bool Read()
        {
            if (base.Read())
            {
                base_status_id = (int)recs["base_status_id"];
                return true;
            }
            return false;
        }
        public List<StatusRelation> StatusRels()
        {
            while (Read())
            {
                foreach (DeliveryMethod delivery in Enum.GetValues(typeof(DeliveryMethod)))
                {
                    foreach (PaymentMethod payment in Enum.GetValues(typeof(PaymentMethod)))
                    {
                        switch (base_status_id)
                        {
                            case 0:
                                statusRels.Add(new StatusRelation(base_status_id, delivery, payment, OrderStatus.New));
                                break;
                            case 1:
                                statusRels.Add(new StatusRelation(base_status_id, delivery, payment, OrderStatus.Discution));
                                break;
                            case 2:
                            case 3:
                                if (delivery == DeliveryMethod.PostOfRussia && payment == PaymentMethod.PayOnDelivery)
                                    statusRels.Add(new StatusRelation(base_status_id, delivery, payment, OrderStatus.PayOnDeliveryProcessOrder));
                                else
                                    statusRels.Add(new StatusRelation(base_status_id, delivery, payment, OrderStatus.WaitForPayment));
                                break;
                            case 4:
                                if (delivery == DeliveryMethod.PostOfRussia && payment == PaymentMethod.PayOnDelivery)
                                    statusRels.Add(new StatusRelation(base_status_id, delivery, payment, OrderStatus.PayOnDeliveryProcessOrder));
                                else
                                    statusRels.Add(new StatusRelation(base_status_id, delivery, payment, OrderStatus.WaitForPayment));
                                break;
                            case 5:
                                if (delivery == DeliveryMethod.PostOfRussia && payment == PaymentMethod.PayOnDelivery)
                                    statusRels.Add(new StatusRelation(base_status_id, delivery, payment, OrderStatus.PayOnDeliveryProcessOrder));
                                else
                                    statusRels.Add(new StatusRelation(base_status_id, delivery, payment, OrderStatus.PayedProcessOrder));
                                break;
                            case 6:
                                if (delivery == DeliveryMethod.PostOfRussia && payment == PaymentMethod.PayOnDelivery)
                                    statusRels.Add(new StatusRelation(base_status_id, delivery, payment, OrderStatus.PayOnDeliveryWaitForOrderSend));
                                else
                                    statusRels.Add(new StatusRelation(base_status_id, delivery, payment, OrderStatus.PayedWaitForOrderSend));
                                break;
                            case 7:
                                if (delivery == DeliveryMethod.PostOfRussia && payment == PaymentMethod.PayOnDelivery)
                                    statusRels.Add(new StatusRelation(base_status_id, delivery, payment, OrderStatus.PayOnDeliveryOrderSent));
                                else
                                    statusRels.Add(new StatusRelation(base_status_id, delivery, payment, OrderStatus.PayedOrderSent));
                                break;
                            case 8:
                                if (delivery == DeliveryMethod.PostOfRussia && payment == PaymentMethod.PayOnDelivery)
                                    statusRels.Add(new StatusRelation(base_status_id, delivery, payment, OrderStatus.PayOnDeliveryReceived));
                                else
                                    statusRels.Add(new StatusRelation(base_status_id, delivery, payment, OrderStatus.DealOK));
                                break;
                            default:
                                statusRels.Add(new StatusRelation(base_status_id, delivery, payment, OrderStatus.DealCanceled));
                                break;
                        }
                    }
                }
            }
            return statusRels;
        }
    }
}
