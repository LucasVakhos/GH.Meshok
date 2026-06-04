using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Registrator;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Base.Handler;
using DevExpress.XtraGrid.Views.Base.ViewInfo;
namespace GH.Components
{
    public class HLViewGhInfoRegistrator : GridInfoRegistrator
    {
        public override string ViewName => nameof(HighlightingViewGh);
    public override BaseView CreateView(GridControl grid)
        {
            return new HighlightingViewGh(grid);
        }
    public override BaseViewInfo CreateViewInfo(BaseView view)
        {
            return new ViewInfoGh(view as HighlightingViewGh);
        }
    public override BaseViewHandler CreateHandler(BaseView view)
        {
            return new ViewHandlerGh(view as HighlightingViewGh);
        }
    }
}
