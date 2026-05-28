using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using MeshokBrowser.NHibernate;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
namespace MeshokBrowser.Workers
{
    public partial class ScanSettingLostDeals : ScanSettingFrame
    {
        public ScanSettingLostDeals()
        {
            InitializeComponent();
            gridView.CustomDrawCell += gridView_CustomDrawCell;
        }
        protected override bool CanApply()
        {
            if (gridView.State == GridState.Editing)
            {
                gridView.PostEditor();
            }
            return (dataSource.DataSource as List<OrderLine>).Any(x => x.NeedAdd);
        }
        private void gridView_CustomDrawCell(object sender, RowCellCustomDrawEventArgs e)
        {
            OrderLine d = GetDeal(e.RowHandle);
            if (d != null)
            {
                if (d.NeedAdd)
                {
                    e.Appearance.BackColor = Color.FromArgb(255, Color.DarkOrange);
                    e.Appearance.BackColor2 = e.Appearance.BackColor;
                    e.Appearance.ForeColor = Color.FromArgb(255, Color.White);
                }
                if (e.Column.FieldName != "DealTitle")
                    return;
                else if (d.need_split)
                {
                    e.Appearance.BackColor = Color.FromArgb(255, Color.DarkRed);
                    e.Appearance.BackColor2 = e.Appearance.BackColor;
                    e.Appearance.ForeColor = Color.FromArgb(255, Color.White);
                    e.DisplayText += " (Объединённый, будет разъединён и заново пересканирован.)";
                }
            }
        }
        internal OrderLine GetDeal(int rowHandle)
        {
            int dataSourceRowIndex1 = gridView.GetDataSourceRowIndex(rowHandle);
            if (dataSourceRowIndex1 < 0 || dataSourceRowIndex1 > dataSource.List.Count)
                return null;
            else
                return dataSource.List[dataSourceRowIndex1] as OrderLine;
        }
        private void dataSource_OnOpen(out IList list)
        {
            list = AllPackets.ForPostOrderLines;
        }
        private void actionCheckAll_Update(object sender, EventArgs e)
        {
            if (dataSource.List is IList<OrderLine> orderLines)
                actionCheckAll.Enabled = orderLines.Any(x => !x.NeedAdd);
            else
                actionCheckAll.Enabled = false;
        }
        private void actionCheckAll_Execute(object sender, EventArgs e)
        {
            if (dataSource.List is IList<OrderLine> orderLines)
            {
                foreach (OrderLine item in orderLines.Where(x => !x.NeedAdd))
                {
                    item.NeedAdd = true;
                }
                dataSource.ResetBindings(false);
            }
        }
    }
}
