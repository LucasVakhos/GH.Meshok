namespace Common
{
    public interface IDeliveryObject
    {
        int base_id { get; set; }
        int base_status { get; set; }
        int md_id { get; set; }
        int mp_id { get; set; }
        OrderStatus CurrStatus { get; }
    }
}