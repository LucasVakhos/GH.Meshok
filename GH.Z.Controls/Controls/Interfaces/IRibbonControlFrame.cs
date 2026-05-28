using DevExpress.XtraBars.Ribbon;

namespace GH.Interfaces
{
    public interface IRibbonControlFrame
    {
        RibbonControl MainRibbonControl { get; }
        RibbonControl RibbonControl { get; set; }
        RibbonBarManager Manager { get; }
        RibbonStatusBar RibbonStatusBar { get; set; }
        RibbonPage EditPage { get; set; }
        void ShowRibbon();
        void HideRibbon();
    }

}
