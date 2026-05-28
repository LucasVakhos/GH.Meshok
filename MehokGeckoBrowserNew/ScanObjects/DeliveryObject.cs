using Common;
using System.ComponentModel;
using System.Linq;
namespace MeshokBrowser.NHibernate
{
    public class DeliveryObject : BaseScanEntity, IDeliveryObject
    {
        private int _base_id;
        [DisplayName("ID в базе")]
        public virtual int base_id { get => _base_id; set => _base_id = value; }
        private int _base_status = 0;
        [DisplayName("Статус в базе")]
        public virtual int base_status { get => _base_status; set => _base_status = value; }
        private int _md_id = 4;
        [DisplayName("Доставка")]
        public virtual int md_id { get => _md_id; set => _md_id = value; }
        private int _mp_id = 1;
        [DisplayName("Оплата")]
        public virtual int mp_id { get => _mp_id; set => _mp_id = value; }
        public virtual OrderStatus CurrStatus => StatusRelation.StatusRels.
            Where(x => x.base_status_id == base_status && x.DeliveryMethod == md_id && x.PaymentMethod == mp_id).FirstOrDefault().OrderStatus;
    }
}
