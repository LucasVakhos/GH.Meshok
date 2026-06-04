using System.IO;
using System.Drawing;
using System.Collections;
using System;
using DevExpress.Data;
using DevExpress.Utils;
using DevExpress.Utils.Menu;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Menu;
using DevExpress.XtraGrid.Views.Grid;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
namespace GH.Components
{
    public class ViewGh : GridView
    {
        public enum ShowTypes
        {
            [Display(Name = "Выровнять ширину колонок по сетке", Description = "Выровнять ширину колонок по сетке")]
            CollumnsAutoWidth,
            [Display(Name = "Отменить выравнивание ширины колонок по сетке", Description = "Отменить выравнивание ширины колонок по сетке")]
            CollumnsFitNone,
            [Display(Name = "Показать все колонки", Description = "Показать все колонки включая скрытые")]
            ShowAllCollumns,
            [Display(Name = "Восстановить настройки колонок", Description = "Восстановить настройки колонок: отображение, группировку, сортировку")]
            RestoreGridDefaults,
            Other
        }

    private readonly Stream _layoutAsDesigned = new MemoryStream();
    private bool _firstLayout = true;
    internal IList<DXMenuItem> _titleItems = new List<DXMenuItem>();
    protected override string ViewName => nameof(ViewGh);
    public ViewGh()
        {
        }

    public ViewGh(GridControl grid) : base(grid)
        {
            InitOptions();
        }
    private void InitOptions()
        {
            OptionsBehavior.ReadOnly = true;
            OptionsBehavior.Editable = false;
            OptionsBehavior.AllowIncrementalSearch = true;
            OptionsBehavior.SummariesIgnoreNullValues = true;
            OptionsView.EnableAppearanceEvenRow = true;
            OptionsView.ShowGroupPanel = false;
            OptionsView.ColumnAutoWidth = false;
            OptionsView.ShowFooter = true;
        }
    public override void BeginInit()
        {
            base.BeginInit();
        }
    public override void EndInit()
        {
            if (!IsDesignMode && GridControl.DataSource is GH.Components.DataSource data && data.DataSource != null)
            {
                InitOptions();
                FormatColumns();
                SaveLayoutToStream(_layoutAsDesigned);
            }
            base.EndInit();
        }
    private void CreateTileItems(GridViewMenu menu)
        {
            InitTileItems();
            foreach (var item in _titleItems)
            {
                switch ((ShowTypes)item.Tag)
                {
                    case ShowTypes.CollumnsAutoWidth:
                        item.Visible = VisibleColumns.Count > 0 && !OptionsView.ColumnAutoWidth;
                        item.BeginGroup = item.Visible;
                        break;
                    case ShowTypes.CollumnsFitNone:
                        item.Visible = VisibleColumns.Count > 0 && OptionsView.ColumnAutoWidth;
                        item.BeginGroup = item.Visible;
                        break;
                    case ShowTypes.ShowAllCollumns:
                        item.Enabled = Columns.Count != VisibleColumns.Count;
                        break;
                    default:
                        break;
                }
                menu.Items.Add(item);
            }
        }
    internal virtual void InitTileItems()
        {
            if (_titleItems.Count == 0)
            {
                foreach (ShowTypes showType in Enum.GetValues(typeof(ShowTypes)))
                {
                    if (showType == ShowTypes.Other)
                        continue;
                    var displayAtt = showType.GetAttribute<DisplayAttribute>();
                    DXMenuItem item = new DXMenuItem(displayAtt.Name, new EventHandler(MyMenuClick), GetImage(showType));
                    //item.BeginGroup = _titleItems.Count == 0;
                    item.Tag = showType;
                    _titleItems.Add(item);
                }
            }
        }
    private Image GetImage(ShowTypes showType)
        {
            switch (showType)
            {
                case ShowTypes.CollumnsAutoWidth:
                    return DevExpress.Images.ImageResourceCache.Default.GetImage("images/grid/columnautowidth_16x16.png");
                case ShowTypes.CollumnsFitNone:
                    return DevExpress.Images.ImageResourceCache.Default.GetImage("images/grid/fitnone_16x16.png");
                case ShowTypes.ShowAllCollumns:
                    return DevExpress.Images.ImageResourceCache.Default.GetImage("images/grid/gridcolumnheader_16x16.png");
                case ShowTypes.RestoreGridDefaults:
                    return DevExpress.Images.ImageResourceCache.Default.GetImage("images/grid/grid_16x16.png");
                default:
                    break;
            }
            return (Image)null;
        }
    private void MyMenuClick(object sender, EventArgs e)
        {
            switch ((ShowTypes)(sender as DXMenuItem).Tag)
            {
                case ShowTypes.CollumnsAutoWidth:
                case ShowTypes.CollumnsFitNone:
                    OptionsView.ColumnAutoWidth = !OptionsView.ColumnAutoWidth;
                    break;
                case ShowTypes.ShowAllCollumns:
                    foreach (GridColumn item in Columns)
                    {
                        if (item.Visible)
                            continue;
                        item.Visible = true;
                    }
                    break;
                case ShowTypes.RestoreGridDefaults:
                    RestoreDefaultLayout();
                    break;
                default:
                    break;
            }
        }
    protected override void RaisePopupMenuShowing(PopupMenuShowingEventArgs e)
        {
            if (e.MenuType == GridMenuType.Column)
            {
                CreateTileItems(e.Menu);
            }
            base.RaisePopupMenuShowing(e);
        }
    protected override void DoInternalLayout()
        {
            if (IsDesignMode)
            {
                base.DoInternalLayout();
            }
            else
            {
                base.DoInternalLayout();
                if (!_firstLayout)
                    return;
                _firstLayout = false;
            }
        }
    private void FormatColumns()
        {
            GridControl.UseEmbeddedNavigator = false;
            GroupSummary.Clear();
            if (GridControl.DataSource is GH.Components.DataSource data)
            {
                foreach (var item in data.Repository.ConcreteType.GetProperties())
                {
                    Type type = item.PropertyType;
                    GridColumn column = Columns.ColumnByFieldName(item.Name);
                    if (column == null)
                        continue;
                    var att = item.GetCustomAttribute<DisplayAttribute>();
                    if (att != null)
                        column.Caption = att.Name;
                    if (column.ColumnEdit != null)
                        continue;
                    // ДОДЕЛАТЬ column.BestFit();
                    if (column.VisibleIndex == 0)
                    {
                        column.Summary.Clear();
                        column.Summary.Add(SummaryItemType.Count, column.FieldName, "{0}");
                        GroupSummary.Add(SummaryItemType.Count, column.FieldName, column, "{0}");
                    }
                    else
                        if (type == typeof(int))
                        {
                            column.DisplayFormat.FormatType = FormatType.Numeric;
                            column.DisplayFormat.FormatString = "n0";
                            if (item.Name.EndsWith("_qty") || item.Name.EndsWith("_cnt") || item.Name.EndsWith("_total"))
                            {
                                column.Summary.Clear();
                                column.Summary.Add(SummaryItemType.Sum, column.FieldName, "{0}");
                                GroupSummary.Add(SummaryItemType.Sum, column.FieldName, column, "{0}");
                            }
                            //else
                            //    GroupSummary.Add(SummaryItemType.Count, column.FieldName, column, column.SummaryText + "{0}");
                        }
                        else
                            if (type == typeof(double))
                            {
                                column.DisplayFormat.FormatType = FormatType.Numeric;
                                column.DisplayFormat.FormatString = "f2";
                                if (item.Name.Contains("total") || item.Name.EndsWith("_summ"))
                                {
                                    column.Summary.Clear();
                                    column.Summary.Add(SummaryItemType.Sum, column.FieldName, "{0:0.00}");
                                    GroupSummary.Add(SummaryItemType.Sum, column.FieldName, column, "{0:0.00}");
                                }
                                //else
                                //    GroupSummary.Add(SummaryItemType.Count, column.FieldName, column, column.SummaryText + "Сумма = {0}");
                            }
                }
            }
        }
    public void RestoreDefaultLayout(Stream layout = null)
        {
            if (layout == null)
                layout = _layoutAsDesigned;
            layout.Seek(0L, SeekOrigin.Begin);
            RestoreLayoutFromStream(layout);
            Reload();
        }
    protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_titleItems.Count > 0)
                    foreach (var item in _titleItems)
                        item.Dispose();
                _titleItems = null;
            }
            base.Dispose(disposing);
        }
    }
}
