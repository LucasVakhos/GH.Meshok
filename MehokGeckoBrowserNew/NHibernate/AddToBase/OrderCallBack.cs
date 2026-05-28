using FluentNHibernate.Mapping;
using GH.NHibernate;
namespace MeshokBrowser.NHibernate
{
    public class OrderCallBack : BaseEntity
    {
    }
    public class OrderCallBackMap : ClassMap<OrderCallBack>
    {
        public OrderCallBackMap()
        {
            Table("z$import_co");
            Id(x => x.id, "co_id");
        }
    }
}
