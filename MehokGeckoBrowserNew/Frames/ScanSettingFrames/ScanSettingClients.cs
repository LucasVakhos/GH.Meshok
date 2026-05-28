using DevExpress.XtraGrid.Views.Grid;
using MeshokBrowser.NHibernate;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
namespace MeshokBrowser.Workers
{
    public partial class ScanSettingClients : ScanSettingFrame
    {
        public ScanSettingClients()
        {
            InitializeComponent();
            Dictionary<int, string> delivery = new Dictionary<int, string>()
            {
                { 1, "Курьером по Москве" },
                { 2, "Почтой России" },
                { 3, "Службой доставки" },
                { 4, "Самовывоз" }
            };
            cboc_md_id.DataSource = delivery.ToArray();
            cboc_md_id.DisplayMember = "Value";
            cboc_md_id.ValueMember = "Key";
            Dictionary<int, string> payment = new Dictionary<int, string>()
            {
                { 1, "Наличным" },
                { 2, "Квитанция Сбербанка" },
                { 3, "Наложенный платеж" },
                { 4, "Онлайн - оплата банковской картой" }
            };
            cboc_mp_id.DataSource = payment.ToArray();
            cboc_mp_id.DisplayMember = "Value";
            cboc_mp_id.ValueMember = "Key";
            gridView.CustomDrawCell += gridView_CustomDrawCell;
        }
        protected override bool CanApply()
        {
            if (gridView.State == GridState.Editing)
                gridView.PostEditor();
            return true;
        }
        private void gridView_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            if (e.Column.FieldName != "colc_city") return;
            Client с = GetClient(e.RowHandle);
            if (с != null && !с.c_enabled)
            {
                e.Appearance.BackColor = Color.FromArgb(255, Color.Red);
                e.Appearance.BackColor2 = e.Appearance.BackColor;
                e.Appearance.ForeColor = Color.FromArgb(255, Color.White);
                e.DisplayText = "Заблокирован";
            }
        }
        internal Client GetClient(int rowHandle)
        {
            int dataSourceRowIndex1 = gridView.GetDataSourceRowIndex(rowHandle);
            if (dataSourceRowIndex1 < 0 || dataSourceRowIndex1 > dataSource.List.Count)
                return null;
            else
                return dataSource.List[dataSourceRowIndex1] as Client;
        }
        private void bindingSource_PositionChanged(object sender, EventArgs e)
        {
            Client client = dataSource.Current as Client;
            detailSource.DataSource = AllPackets.OrderLines.Where(x => x.Client == client /*&& x.Title != null*/).ToList();
        }
        private void dataSource_OnOpen(out System.Collections.IList list)
        {
            list = AllPackets.ForPostClients;
        }
    }
}
