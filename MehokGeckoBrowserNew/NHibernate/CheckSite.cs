using FluentNHibernate.Mapping;
using GH.NHibernate;
namespace MeshokBrowser.NHibernate
{
    public class CheckSite : BaseEntity
    {
    }
    public class CheckSitedMap : ClassMap<CheckSite>
    {
        public CheckSitedMap()
        {
            Table("z$site_id");
            Id(x => x.id, "z$s_id");
            Map(x => x.Name, "z$s_name");
        }
    }
}
