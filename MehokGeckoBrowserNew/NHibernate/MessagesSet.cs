using FluentNHibernate.Mapping;
using GH.NHibernate;
using System.ComponentModel.DataAnnotations;
namespace MeshokBrowser.NHibernate
{
    public class MessagesSet : BaseEntity
    {
        [Display(Name = "Статус в базе")]
        public virtual int zsc_cs_id { get; set; }
        [Display(Name = "Статус на мешке")]
        public virtual int zsc_zs_id { get; set; }
        [Display(Name = "Способ доставки")]
        public virtual int? zsc_md_id { get; set; }
        [Display(Name = "Применять если")]
        public virtual int zsc_case { get; set; }
        [Display(Name = "Текст сообщения")]
        public virtual string zsc_message { get; set; }
    }
    public class MessagesSetMap : ClassMap<MessagesSet>
    {
        public MessagesSetMap()
        {
            Table("z$statuses_cod");
            Id(x => x.id, "zsc_id");
            Map(x => x.zsc_cs_id, "zsc_cs_id");
            Map(x => x.zsc_zs_id, "zsc_zs_id");
            Map(x => x.zsc_md_id, "zsc_md_id").Nullable();
            Map(x => x.zsc_case, "zsc_case");
            Map(x => x.zsc_message, "zsc_message");
        }
    }
}
