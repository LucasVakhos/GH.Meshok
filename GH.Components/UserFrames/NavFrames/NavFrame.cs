using DevExpress.XtraNavBar;
using System.ComponentModel;
using System.Drawing.Design;
namespace GH.Components
{
    [ToolboxItem(false)]
    public partial class NavFrame : SavedFrame, INavBarGroupFrame
    {
        protected NavBarGroup _group;
        [GH.ComponentsProperty, Browsable(false)]
        public virtual NavBarGroup Group
        {
            get
            {
                if (_group == null)
                    _group = FindGroup(Parent);
                return _group;
            }
        }
        public NavFrame()
        {
            InitializeComponent();
        }
        private NavBarGroup FindGroup(Control control)
        {
            if (control == null)
                return null;
            if (control is INavBarGroupFrame nav && nav.IsBase)
                return nav.Group;
            return FindGroup(control.Parent);
        }
        //protected override void Dispose(bool disposing)
        //{
        //    base.Dispose(disposing);
        //}
        private void InitializeComponent()
        {
            //navbaritems = new List<NavBarItem>();
            SuspendLayout();
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ResumeLayout(false);
        }
    }
}
