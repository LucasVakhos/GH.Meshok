using DevExpress.XtraLayout;

namespace GH.Interfaces
{
    public interface IDetailsFrame
    {
        LayoutControlGroup Page { get; }
        TabbedGroup PageControl { get; }
    }

}
