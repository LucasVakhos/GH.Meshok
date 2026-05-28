using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraLayout;
using System.ComponentModel;
namespace GH.Components
{
    public partial class NavFrame
    {
        private bool _isBase = false;
        [GH.ComponentsProperty, Browsable(false)]
        public bool IsBase => _isBase;
        private BarButtonItem _barButton;
        public BarButtonItem BarButton { get => _barButton; set => _barButton = value; }
        private RibbonStatusBar _statusBar;
        FrameHolder Holder => FrameHolder.Holder;
        [GH.ComponentsProperty, DefaultValue(null)]
        public RibbonStatusBar StatusBar { get => _statusBar; set => _statusBar = value; }
        public override void SelectFrame()
        {
            if (ParentForm == null)
                SelectFrame(this);
            if (this is IDetailsFrame details)
            {
                if (details.PageControl != null && details.Page != null)
                    details.PageControl.SelectedTabPage = details.Page;
            }
        }
        private void SelectFrame(Control control)
        {
            if (control is NavFrame navFrame && navFrame.IsBase)
                navFrame.BarButton.PerformClick();
            else
                if (control == null)
                    return;
                else
                    SelectFrame(control.Parent);
        }
        internal void InitAsBase()
        {
            _isBase = true;
            _group = Holder.NavBar.Groups.Add();
            _group.Name = "grp" + Name;
            _barButton = new BarButtonItem();
            _barButton.Name = "btn" + Name;
            Group.Tag = this;
            Group.Caption = Caption;
            BarButton.Caption = Caption;
            BarButton.ItemClick += _barButton_ItemClick;
            Group.ImageOptions.LargeImage = LargeImage;
            Group.ImageOptions.SmallImage = Image;
            BarButton.LargeGlyph = LargeImage;
            BarButton.Glyph = Image;
            Holder.FrameGroup.ItemLinks.Add(BarButton);
            HandleFocusTracking(Controls);
        }
        protected void HandleFocusTracking(ControlCollection controlCollection)
        {
            foreach (Control control in controlCollection)
            {
                if (control is LayoutControl)
                    this.HandleFocusTracking(control.Controls);
                else
                    if (control is AbstractFrame)
                        this.HandleFocusTracking(control.Controls);
                    else
                        control.GotFocus += new EventHandler(control_GotFocus);
            }
        }
        void control_GotFocus(object sender, EventArgs e)
        {
            ActiveControl = sender as Control;
            ControlGotFocus(sender);
        }
        protected virtual void ControlGotFocus(object sender)
        {
        }
        private void _barButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            Group.NavBar.ActiveGroup = Group;
        }
    }
}
