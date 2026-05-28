using DevExpress.XtraBars.Ribbon;
namespace GH.Components
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
