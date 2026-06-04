using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Registrator;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Base.Handler;
using DevExpress.XtraGrid.Views.Base.ViewInfo;
namespace GH.Components
{
    public class ViewGhInfoRegistrator : GridInfoRegistrator
    {
        public ViewGhInfoRegistrator()
        {
        }

    public override string ViewName => nameof(ViewGh);
    public override BaseView CreateView(GridControl grid)
        {
            return new ViewGh(grid);
        }
    public override BaseViewInfo CreateViewInfo(BaseView view)
        {
            return new ViewInfoGh(view as ViewGh);
        }
    public override BaseViewHandler CreateHandler(BaseView view)
        {
            return new ViewHandlerGh(view as ViewGh);
        }
    }
}
