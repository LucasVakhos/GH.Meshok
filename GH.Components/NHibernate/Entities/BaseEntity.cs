using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace GH.Components
{
    public class BaseEntity : AbstractEntity
    {
        [UpdatableProperty(Key = true, Caption = "ID", ToolTip = "ID записи"), Editable(false), ReadOnly(true)]
        public virtual int id { get; set; }
        [UpdatableProperty(Caption = "Наименование", ToolTip = "Наименование")]
        public virtual string Name { get; set; }
    public override bool Equals(object obj)
        {
            if (obj is BaseEntity objEntity)
                return (id == 0 && base.Equals(obj)) || id == objEntity.id;
            return false;
        }
    public override int GetHashCode()
        {
            return 1877310944 + id.GetHashCode();
        }
    public override void CancelEdit()
        {
            if (id == 0)
                return;
            base.CancelEdit();
        }
    }
}
