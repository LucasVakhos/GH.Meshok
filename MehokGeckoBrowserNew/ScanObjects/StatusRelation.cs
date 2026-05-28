using Common;
using GH.NHibernate;
using System;
using System.Collections;
using System.Collections.Generic;
namespace MeshokBrowser.NHibernate
{
    public class StatusRelation
    {
        private static IList<StatusRelation> statusRels = new List<StatusRelation>();
        public static IList<StatusRelation> StatusRels => statusRels;
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
        public static void SetStatusRels(IList value)
        {
            statusRels.Clear();
            foreach (object item in value)
            {
                foreach (DeliveryMethod delivery in Enum.GetValues(typeof(DeliveryMethod)))
                {
                    int base_status_id = (item as BaseEntity).id;
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
                                if (delivery == Common.DeliveryMethod.PostOfRussia && payment == Common.PaymentMethod.PayOnDelivery)
                                    statusRels.Add(new StatusRelation(base_status_id, delivery, payment, OrderStatus.PayOnDeliveryProcessOrder));
                                else
                                    statusRels.Add(new StatusRelation(base_status_id, delivery, payment, OrderStatus.WaitForPayment));
                                break;
                            case 4:
                                if (delivery == Common.DeliveryMethod.PostOfRussia && payment == Common.PaymentMethod.PayOnDelivery)
                                    statusRels.Add(new StatusRelation(base_status_id, delivery, payment, OrderStatus.PayOnDeliveryProcessOrder));
                                else
                                    statusRels.Add(new StatusRelation(base_status_id, delivery, payment, OrderStatus.WaitForPayment));
                                break;
                            case 5:
                                if (delivery == Common.DeliveryMethod.PostOfRussia && payment == Common.PaymentMethod.PayOnDelivery)
                                    statusRels.Add(new StatusRelation(base_status_id, delivery, payment, OrderStatus.PayOnDeliveryProcessOrder));
                                else
                                    statusRels.Add(new StatusRelation(base_status_id, delivery, payment, OrderStatus.PayedProcessOrder));
                                break;
                            case 6:
                                if (delivery == Common.DeliveryMethod.PostOfRussia && payment == Common.PaymentMethod.PayOnDelivery)
                                    statusRels.Add(new StatusRelation(base_status_id, delivery, payment, OrderStatus.PayOnDeliveryWaitForOrderSend));
                                else
                                    statusRels.Add(new StatusRelation(base_status_id, delivery, payment, OrderStatus.PayedWaitForOrderSend));
                                break;
                            case 7:
                                if (delivery == Common.DeliveryMethod.PostOfRussia && payment == Common.PaymentMethod.PayOnDelivery)
                                    statusRels.Add(new StatusRelation(base_status_id, delivery, payment, OrderStatus.PayOnDeliveryOrderSent));
                                else
                                    statusRels.Add(new StatusRelation(base_status_id, delivery, payment, OrderStatus.PayedOrderSent));
                                break;
                            case 8:
                                if (delivery == Common.DeliveryMethod.PostOfRussia && payment == Common.PaymentMethod.PayOnDelivery)
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
        }
    }
}
