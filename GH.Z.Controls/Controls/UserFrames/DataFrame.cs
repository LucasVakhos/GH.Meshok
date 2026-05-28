using DevExpress.XtraLayout;
using System;
using System.ComponentModel;
using System.ComponentModel.Design;
//using System.Web.UI.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace GH.Components
{
    [Designer(nameof(DataFrameDesigner))]
    public partial class DataFrame : NavFrame
    {
        public DataFrame()
        {
            InitializeComponent();
        }

        protected void InitDetailFrame(DetailsFrame control, LayoutControlItem place)
        {
            layoutControl.SuspendLayout();
            layoutControl.Controls.Add(control);
            control.Owner = this;
            if (place == null)
            {
                LayoutControlGroup page = dataSource.PageSupport.PageControl.AddTabPage();
                page.Name = control.Name + "Page";
                page.Text = control.Caption;
                place = page.AddItem(control.Caption, control);
                place.Name = "lc" + control.Name;
                place.TextVisible = false;
            }
            else
            {
                ((ISupportInitialize)place).BeginInit();
                Control old_control = place.Control;
                control.Location = old_control.Location;
                control.Size = old_control.Size;
                place.Control = control;
                old_control.Dispose();
                ((ISupportInitialize)place).EndInit();
            }
            place.ShowInCustomizationForm = false;
            place.OptionsCustomization.AllowDrag = ItemDragDropMode.Disable;
            control.Place = place;
            layoutControl.ResumeLayout(false);

            control.MasterSource = dataSource;
        }
    }

    public class DataFrameDesigner : ControlDesigner
    {
        private FrameTypesActionList FrameTypesAction;

        public override void Initialize(IComponent component)
        {
            FrameTypesAction = new FrameTypesActionList(this);
            (GetService(typeof(DesignerActionService)) as DesignerActionService)?.Add(Component, FrameTypesAction);
        }
    }

    public class FrameTypesActionList : DesignerActionList
    {
        private ControlDesigner _designer;
        private IDesignerHost _host;

        public FrameTypesActionList(ControlDesigner owner)
          : base(owner.Component)
        {
            this._designer = owner;
            this._host = this.GetService(typeof(IDesignerHost)) as IDesignerHost;
        }

        private string GetActionName()
        {
            //PropertyDescriptor property = TypeDescriptor.GetProperties((object)this.Component)["Dock"];
            //if (property == null)
            //    return (string)null;
            //if ((DockStyle)property.GetValue((object)this.Component) == DockStyle.Fill)
            //    return System.Design.SR.GetString("DesignerShortcutUndockInParent");
            //return System.Design.SR.GetString("DesignerShortcutDockInParent");
            return "Create Dictionary";
        }

        public override DesignerActionItemCollection GetSortedActionItems()
        {
            DesignerActionItemCollection actionItemCollection = base.GetSortedActionItems();
            if (this.GetActionName() != null)
                actionItemCollection.Add((DesignerActionItem)new DesignerVerbItem(new DesignerVerb(this.GetActionName(), new EventHandler(this.OnDockActionClick))));
            return actionItemCollection;
        }

        private void OnDockActionClick(object sender, EventArgs e)
        {
            DesignerVerb designerVerb = sender as DesignerVerb;
            if (designerVerb == null || this._host == null)
                return;
            using (DesignerTransaction transaction = this._host.CreateTransaction(designerVerb.Text))
            {
                //PropertyDescriptor property = TypeDescriptor.GetProperties((object)this.Component)["Dock"];
                //if ((DockStyle)property.GetValue((object)this.Component) == DockStyle.Fill)
                //    property.SetValue((object)this.Component, (object)DockStyle.None);
                //else
                //    property.SetValue((object)this.Component, (object)DockStyle.Fill);
                //transaction.Commit();
            }
        }
    }

    public class DesignerVerbItem : DesignerActionMethodItem
    {
        private DesignerVerb _targetVerb;

        public DesignerVerbItem(DesignerVerb targetVerb): base (null, null, targetVerb.Text)
        {
            _targetVerb = targetVerb;
        }


        public override string Category
        {
            get
            {
                return "Verbs";
            }
        }

        public override string Description
        {
            get
            {
                return this._targetVerb.Description;
            }
        }

        public override string DisplayName
        {
            get
            {
                return this._targetVerb.Text;
            }
        }

        public override string MemberName
        {
            get
            {
                return (string)null;
            }
        }

        public override bool IncludeAsDesignerVerb
        {
            get
            {
                return false;
            }
        }

        public override void Invoke()
        {
            this._targetVerb.Invoke();
        }
    }

}
