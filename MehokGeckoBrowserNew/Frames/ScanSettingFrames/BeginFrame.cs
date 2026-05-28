using System;
using System.Collections.Generic;
using System.Linq;
using MeshokBrowser.Helpers;
namespace MeshokBrowser.Workers
{
    public partial class BeginingForm : BaseFrame
    {
        ScanSetting _setting = null;
        private bool _dialogRes = false;
        private bool _dialogFinised = false;
        public bool DialogResult
        {
            get => _dialogRes;
            set {
                _dialogRes = value;
                _dialogFinised = true;
            }
        }
        public bool DialogFinised { get => _dialogFinised; }
        public BeginingForm(ScanSetting setting)
        {
            setting.AddLostDeals = false;
            InitializeComponent();
            cboOnPage.Properties.DataSource = new Dictionary<int, string>() {
                { 10, "По 10 на странице"},
                { 20, "По 20 на странице"},
                { 50, "По 50 на странице"},
                { 100, "По 100"},
                { 200, "По 200"},
                { 500, "По 500"},
                { 1000 ,"По 1000"}
            }.ToArray();
            _setting = setting;
            dataSource.DataSource = setting;
        }
        private void btnOK_Click(object sender, EventArgs e)
        {
            DialogResult = true;
            _setting.SaveSetting();
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = false;
        }
        public override void CLose()
        {
            DialogResult = false;
        }
    }
}
