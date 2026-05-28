using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing.Design;
namespace GH.Components
{
    [Editor("GH.Components.Design.ActionCollectionEditor, GH.Components.Design", typeof(UITypeEditor))]
    public class ActionCollection : Collection<ActionGh>
    {
        private ActionList parent;
        public ActionCollection(ActionList parent)
        {
            this.parent = parent;
        }
        public ActionList Parent
        {
            get
            {
                return parent;
            }
        }
        protected override void ClearItems()
        {
            foreach (ActionGh action in (Collection<ActionGh>)this)
                action.ActionList = (ActionList)null;
            base.ClearItems();
        }
        protected override void InsertItem(int index, ActionGh item)
        {
            base.InsertItem(index, item);
            item.ActionList = Parent;
        }
        protected override void RemoveItem(int index)
        {
            this[index].ActionList = (ActionList)null;
            base.RemoveItem(index);
        }
        protected override void SetItem(int index, ActionGh item)
        {
            if (Count > index)
                this[index].ActionList = (ActionList)null;
            base.SetItem(index, item);
            item.ActionList = Parent;
        }
    }
}
