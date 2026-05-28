using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms.Design;
namespace GH.Components
{
    public class GHControlDesignerAttribute : ControlDesigner
    {
        private DesignerActionListCollection actionLists;
        public override DesignerActionListCollection ActionLists
        {
            get
            {
                if (this.UseVerbsAsActionList)
                    return base.ActionLists;
                if (this.actionLists == null || this.AlwaysCreateActionLists)
                    this.actionLists = this.CreateActionLists();
                return this.actionLists;
            }
        }
        public override void Initialize(IComponent component)
        {
            base.Initialize(component);
            PropertyGridExtender.Check((ComponentDesigner)this);
        }
        protected virtual DesignerActionListCollection CreateActionLists()
        {
            DesignerActionListCollection list = new DesignerActionListCollection();
            this.RegisterActionLists(list);
            return list;
        }
        protected virtual void RegisterActionLists(DesignerActionListCollection list)
        {
        }
        protected virtual void ResetActionLists()
        {
            this.actionLists = (DesignerActionListCollection)null;
        }
        protected virtual bool CanUsePropertiesEditor
        {
            get
            {
                return true;
            }
        }
        protected virtual bool UseVerbsAsActionList
        {
            get
            {
                return false;
            }
        }
        protected virtual bool AlwaysCreateActionLists
        {
            get
            {
                return false;
            }
        }
        public bool IsUseComponentSmartTags
        {
            get
            {
                return this.CanUseComponentSmartTags;
            }
        }
        protected virtual bool CanUseComponentSmartTags
        {
            get
            {
                return false;
            }
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
            }
            base.Dispose(disposing);
        }
    }
}
