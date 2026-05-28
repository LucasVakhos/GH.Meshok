using DevExpress.XtraBars.Ribbon;
namespace GH.Components
{
    public interface IRibbonForm
    {
        RibbonControl Ribbon { get; }
        RibbonStatusBar StatusBar { get; }
        RibbonPageGroup FrameGroup { get; }
    }
}
