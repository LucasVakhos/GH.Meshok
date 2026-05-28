using FluentNHibernate.Mapping;
using GH.NHibernate;
using System;
namespace MeshokBrowser.NHibernate
{
    public class CheckOrder : BaseEntity
    {
        public static CheckOrder NewCheckOrder(OrderLine orderLine)
        {
            CheckOrder check = new CheckOrder();
            check.co_c_id = orderLine.Client.base_id;
            check.co_creation_date = orderLine.date;
            check.co_md_id = orderLine.Client.md_id;
            check.co_mp_id = orderLine.Client.mp_id;
            check.co_status = orderLine.base_status;
            return check;
        }
        public virtual int co_id { get; set; } = 0;
        public virtual int co_c_id { get; set; } = 0;
        public virtual DateTime co_creation_date { get; set; }
        public virtual int co_md_id { get; set; } = 1;
        public virtual int co_mp_id { get; set; } = 1;
        public virtual int co_status { get; set; } = 0;
        public virtual int dp_id { get; set; } = 0;
        public virtual int dp_md_id { get; set; } = 1;
        public virtual int dp_mp_id { get; set; } = 1;
        public virtual int dp_c_id { get; set; } = 0;
        public virtual double dp_totalsumm { get; set; } = 0;
        public virtual int dp_status { get; set; } = 0;
        public virtual bool dp_packed { get; set; } = false;
        public virtual DateTime? dp_creation_date { get; set; } = null;
    }
    public class CheckOrderMap : ClassMap<CheckOrder>
    {
        public CheckOrderMap()
        {
            Table("z$check_order");
            Id(x => x.id, "cod_id");
            Map(x => x.co_id, "co_id");
            Map(x => x.co_c_id, "co_c_id");
            Map(x => x.co_creation_date, "co_creation_date");
            Map(x => x.co_md_id, "co_md_id");
            Map(x => x.co_mp_id, "co_mp_id");
            Map(x => x.co_status, "co_status");
            Map(x => x.dp_id, "dp_id");
            Map(x => x.dp_c_id, "dp_c_id");
            Map(x => x.dp_md_id, "dp_md_id");
            Map(x => x.dp_mp_id, "dp_mp_id");
            Map(x => x.dp_totalsumm, "dp_totalsumm");
            Map(x => x.dp_status, "dp_status");
            Map(x => x.dp_packed, "dp_packed");
            Map(x => x.dp_creation_date, "dp_creation_date").Nullable();
        }
    }
}
