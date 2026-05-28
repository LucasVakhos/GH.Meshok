using FluentNHibernate.Mapping;
using GH.NHibernate;
namespace MeshokBrowser.NHibernate
{
    public class SiteStatus : BaseEntity
    {
    }
    public class SiteStatusMap : ClassMap<SiteStatus>
    {
        public SiteStatusMap()
        {
            Table("z$statuses");
            Id(x => x.id, "zs_id");
            Map(x => x.Name, "zs_status_name");
        }
    }
}
