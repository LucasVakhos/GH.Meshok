using FluentNHibernate.Mapping;
using GH.NHibernate;
namespace MeshokBrowser.NHibernate
{
    public class TitleCallBack : BaseEntity
    {
        public virtual int ts_id { get; set; }
    }
    public class TitleCallBackMap : ClassMap<TitleCallBack>
    {
        public TitleCallBackMap()
        {
            Table("z$import_t");
            Id(x => x.id, "t_id");
            Map(x => x.ts_id, "ts_id");
        }
    }
}
