using FluentNHibernate.Mapping;
using GH.NHibernate;
namespace MeshokBrowser.NHibernate
{
    public class OrderLineCallBack : BaseEntity
    {
    }
    public class OrderLineCallBackMap : ClassMap<OrderLineCallBack>
    {
        public OrderLineCallBackMap()
        {
            Table("z$import_cod");
            Id(x => x.id, "cod_id");
        }
    }
}
