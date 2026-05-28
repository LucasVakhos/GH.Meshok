using System.ComponentModel.DataAnnotations;
namespace Common
{
    public enum OrderStatus
    {
        //[Display(Name = "===")]
        //None,
        [Display(Name = "Новая")]
        New = 1,
        [Display(Name = "Переговоры")]
        Discution,
        [Display(Name = "Ожидаю оплаты")]
        WaitForPayment,
        [Display(Name = "Оплачено, формирую заказ")]
        PayedProcessOrder,
        [Display(Name = "Оплачено, ожидает отправки")]
        PayedWaitForOrderSend,
        [Display(Name = "Оплачено, отправлено")]
        PayedOrderSent,
        [Display(Name = "НП, формирую заказ")]
        PayOnDeliveryProcessOrder,
        [Display(Name = "НП, ожидает отправки")]
        PayOnDeliveryWaitForOrderSend,
        [Display(Name = "НП, отправлен")]
        PayOnDeliveryOrderSent,
        [Display(Name = "НП, получен")]
        PayOnDeliveryReceived,
        [Display(Name = "Состоялась")]
        DealOK,
        [Display(Name = "Не состоялась")]
        DealCanceled
    }
}
