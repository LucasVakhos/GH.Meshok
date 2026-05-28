using DevExpress.XtraGrid.Menu;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.Handler;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
namespace GH.Components
{
    public class ViewHandlerGh : GridHandler
    {
        private GridHitInfo _hitInfo;
        public ViewHandlerGh(GridView view) : base(view)
        {
        }
        protected override GridViewMenu CreateMenuEx(GridViewMenu menu)
        {
            GridViewMenu newmenu = base.CreateMenuEx(menu);
            return newmenu;
        }
        protected override bool OnMouseDown(MouseEventArgs e)
        {
            _hitInfo = View.CalcHitInfo(new Point(e.X, e.Y));
            return base.OnMouseDown(e);
        }
        protected override void OnDoubleClick(MouseEventArgs ev)
        {
            base.OnDoubleClick(ev);
            if (View.FocusedRowHandle != _hitInfo.RowHandle)
                return;
            if (View.RowCount == 0)
                return;
            if (View.DataSource is DataSource source)
            {
                source.Edit();
            }
        }
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (View.RowCount == 0)
            {
                base.OnKeyDown(e);
                return;
            }
            if (View.DataSource is DataSource source)
            {
                if (source.ActionList != null && e.KeyCode != Keys.Return)
                {
                    base.OnKeyDown(e);
                    return;
                }
                switch (e.KeyCode)
                {
                    case Keys.Return:
                        source.Edit();
                        break;
                    case Keys.Delete when e.Control:
                        source.Delete();
                        break;
                    case Keys.Insert when e.Control:
                        source.Insert();
                        break;
                    default:
                        break;
                }
            }
            base.OnKeyDown(e);
        }
    }
}
