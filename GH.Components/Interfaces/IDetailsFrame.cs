using DevExpress.XtraLayout;
namespace GH.Components
{
    public interface IDetailsFrame
    {
        LayoutControlGroup Page { get; }
        TabbedGroup PageControl { get; }
    }
}
