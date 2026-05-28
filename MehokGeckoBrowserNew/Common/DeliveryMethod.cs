using System.ComponentModel.DataAnnotations;
namespace Common
{
    public enum DeliveryMethod
    {
        [Display(Name = "Курьером по Москве")]
        CourierMoscow = 1,
        [Display(Name = "Почтой России")]
        PostOfRussia = 2,
        [Display(Name = "Службой доставки TTK")]
        DeliveryService = 3,
        [Display(Name = "Самовывоз")]
        PunktPickup = 4
    }
}
