using GH.Components.Attributes;
using GH.NHibernate;
using System.ComponentModel.DataAnnotations;
using static GH.Configs.CfgAppFields;

namespace GH.Configs
{
    public class SectionPathes 
    {
        public SectionPathes()
        {
        }

        public SectionPathes(AbstractEntity owner) : base(owner)
        {
        }

        protected override void Init()
        {
            this[ExportPath] = "Exports";
            this[CfgPath] = "Configs";
            this[DownloadWebFolder] = "нет данных";
        }

    }

}

