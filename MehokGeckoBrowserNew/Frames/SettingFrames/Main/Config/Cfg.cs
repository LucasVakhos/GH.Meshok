using GH.Components;
using System.ComponentModel.DataAnnotations;
namespace MeshokBrowser
{
    public class Cfg: ProtoEntity
    {
        public virtual void LoadFromIni() { }
        public virtual void SaveToIni() { }
        public virtual bool TestConnection() => true;
        public virtual string Section { get => this.GetType().Name; }
        [Display(Name = "User", Description = "Пользователь")]
        public virtual string User { get; set; }
        [Display(Name = "Password", Description = "Пароль")]
        public virtual string PassWrd { get; set; }
    }
}
