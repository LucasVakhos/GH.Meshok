using DevExpress.Utils;
using DevExpress.Utils.Menu;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
namespace GH.Components
{
    public class HighlightingViewGh : ViewGh
    {
        public HighlightingViewGh()
        {
        }

    public HighlightingViewGh(GridControl grid) : base(grid)
        {
        }
    internal override void InitTileItems()
        {
            base.InitTileItems();
            if (_titleItems.Any(x => (ShowTypes)x.Tag == ShowTypes.Other))
                return;
            DXMenuItem item = new DXMenuItem("Менеджер подсветки", new EventHandler(HighLiteClick),
                DevExpress.Images.ImageResourceCache.Default.GetImage("images/chart/fullstackedbar2_16x16.png"));
            item.BeginGroup = true;
            item.Tag = ShowTypes.Other;
            _titleItems.Add(item);
        }
    private void HighLiteClick(object sender, EventArgs e)
        {
            GridHighLiteProcessor.ShowSetup();
        }
    protected override AppearanceObject RaiseGetRowCellStyle(int rowHandle, GridColumn column, GridRowCellState state, AppearanceObject appearance)
        {
            if (IsDesignMode)
                return base.RaiseGetRowCellStyle(rowHandle, column, state, appearance);
            if (!(DataSource is BindingSource))
                return base.RaiseGetRowCellStyle(rowHandle, column, state, appearance);
            object obj = GetEntyty(rowHandle);
            GridHighLiteProcessor.SetRowCellStyle(obj, appearance, rowHandle == FocusedRowHandle && GridControlIsFocused);
            return base.RaiseGetRowCellStyle(rowHandle, column, state, appearance);
        }
    internal object GetEntyty(int rowHandle)
        {
            if (rowHandle < 0)
                return null;
            if (DataSource is BindingSource source)
            {
                if (DataSource == null || source.Count == 0)
                    return null;
                int dataSourceRowIndex1 = GetDataSourceRowIndex(rowHandle);
                if (dataSourceRowIndex1 < 0 || dataSourceRowIndex1 > source.Count)
                    return null;
                return source[dataSourceRowIndex1];
            }
            return null;
        }
    internal void SelectHighlightr(Type concreteType)
        {
            GridHighLiteProcessor.SelectType(concreteType);
        }
    internal void DeSelectHighlightr()
        {
            GridHighLiteProcessor.DeSelectType();
        }
    }
}
