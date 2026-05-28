using FluentNHibernate.Mapping;
using GH.NHibernate;
namespace MeshokBrowser.NHibernate
{
    public class RelBaseSiteStatus : BaseEntity
    {
    }
    public class RelBaseSiteStatusMap : ClassMap<RelBaseSiteStatus>
    {
        public RelBaseSiteStatusMap()
        {
            Table("z$import_statuses");
            Id(x => x.id, "cs_id");
            Map(x => x.Name, "cs_name");
        }
    }
}
