using DevExpress.XtraBars.Ribbon;

namespace GH.Interfaces
{
    public interface IRibbonForm
    {
        RibbonControl Ribbon { get; }
        RibbonStatusBar StatusBar { get; }
        RibbonPageGroup FrameGroup { get; }
    }

}
