using System;
using System.Windows.Forms;
using Common;
using GH.Components;
using GH.Helpers;
using GH.NHibernate;
using MeshokBrowser.Frames;
using MeshokBrowser.NHibernate;
using static GH.Utils.UtilsGh;
namespace MeshokBrowser.Workers
{
    public partial class MessagesSetting : SettingFrame
    {
        const string select_order_message = "select cod_id, c_id, c_name, md_id, md_name, mp_id, mp_name, cs_id, cs_name, dp_totalsumm, dp_packed, md_address, md_treck_num, md_tracking_url, zsc_case, zsc_message from z$check_message(0, 0, 0) zom";
        //MyAdapter MyAdapter { get; set; }
        public MessagesSetting()
        {
            InitializeComponent();
            SetupLookups(this);
            comboCS_ID.Properties.DataSource = (new NHRepository<BaseStatus>() as INHRepository).KeyIntLookupList();
            comboCS_ID.KeyDown += combo_KeyDown;
            baseStatusCombo.DataSource = (new NHRepository<BaseStatus>() as INHRepository).KeyIntLookupList();
            comboZS_ID.Properties.DataSource = (new NHRepository<SiteStatus>() as INHRepository).KeyIntLookupList();
            comboZS_ID.KeyDown += combo_KeyDown;
            siteStatusCombo.DataSource = (new NHRepository<SiteStatus>() as INHRepository).KeyIntLookupList(); 
            comboMD_ID.Properties.DataSource = (new NHRepository<ModeDelivery>() as INHRepository).KeyIntLookupList(); 
            comboMD_ID.KeyDown += combo_KeyDown;
            deliveryCombo.DataSource = (new NHRepository<ModeDelivery>() as INHRepository).KeyIntLookupList();
            caseCombo.DataSource = EnumHelper<MessageCase>.GetIntKeyLookupSource();
            comboZSC_CASE.Properties.DataSource = EnumHelper<MessageCase>.GetIntKeyLookupSource();
            comboZSC_CASE.KeyDown += combo_KeyDown;
            dataSource.Open();
            foreach (Field item in new CheckMesage().GetFields())
            {
                ToolStripMenuItem stripItem = new ToolStripMenuItem($"Вставить жетон \"{item.CaptionText}\"", null, tokenText_Click);
                stripItem.Name = item.Name;
                pmuMessage.Items.Add(stripItem);
            }
        }
        private void mniFish_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(memMessage.Text) && memMessage.Text != memMessage.Properties.NullText)
                if (!DlgYesNo("Желаете перезаписать?"))
                    return;
            memMessage.Focus();
            string msg = "Уважаемый(ая) #c_name#.\r\n";
            msg += "Cтатус вашего заказа: \"#cs_name#\".\r\n";
            int ss = msg.Length;
            msg += "\r\n";
            msg += "С уважением, ваш Bridgenote.";
            memMessage.Text = msg;
            memMessage.SelectionStart = ss;
        }
        private void tokenText_Click(object sender, EventArgs e)
        {
            string token = $"#{((ToolStripMenuItem)sender).Name}#";
            Clipboard.SetText(token);
            Application.DoEvents();
            memMessage.Paste();
        }
        //protected override bool SettingChanged()
        //{
        //    return true;// tableZSC.Select(null, null, DataViewRowState.ModifiedCurrent).Count() > 0;
        //}
        private void btnCancel_Click(object sender, EventArgs e)
        {
        }
        private void InfoSettings_OnApply(object sender, EventArgs e)
        {
            gridControl.Focus();
        }
        private void InfoSettings_OnCancel(object sender, EventArgs e)
        {
            //foreach (DataRow row in tableZSC.Select(null, null, DataViewRowState.ModifiedCurrent))
            //    row.RejectChanges();
            //gridControl.Focus();
        }
        private void combo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
                (sender as DevExpress.XtraEditors.LookUpEdit).EditValue = null;
        }
        private void bindingSource_GetSqlString(SqlTypes sqlType, BaseEntity item, out string sqlString)
        {
            sqlString = null;
            switch (sqlType)
            {
                case SqlTypes.SelectSql:
                    //sqlString = $"select zsc_id, zsc_cs_id, zsc_zs_id, zsc_md_id, zsc_case, zsc_message from z$statuses_cod_s({ConfigMeshok._siteNo})";
                    break;
                //case SqlTypes.UpdateSql:
                //    sqlString = "update z$statuses_cod zsc " +
                //        "set zsc.zsc_cs_id = :zsc_cs_id, " +
                //        "zsc.zsc_zs_id = :zsc_zs_id, " +
                //        "zsc.zsc_md_id = :zsc_md_id, " +
                //        "zsc.zsc_case = :zsc_case, " +
                //        "zsc.zsc_message = :zsc_message " +
                //        "where (zsc.zsc_id = :zsc_id)";
                //    break;
                default:
                    break;
            }
        }
        private void bindingSource_GetRepository(out INHRepository repo)
        {
            repo = new NHRepository<MessagesSet>();
        }
    }
}
