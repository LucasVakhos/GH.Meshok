using MeshokBrowser;
using MeshokBrowser.templates;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace MeshokBrowser.processors
{
    public partial class BeginForm : BaseForm
    {
        ScanHelper _setting = null;
        public BeginForm(ScanHelper setting)
        {
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
            bindingSource.DataSource = setting;
        }
        private void btnOK_Click(object sender, EventArgs e)
        {
            _setting.SaveSetting();
            DialogResult = DialogResult.OK;
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            _setting.RestoreSetting();
            DialogResult = DialogResult.Cancel;
        }
        private void OrderTypeForm_Shown(object sender, EventArgs e)
        {
            ActiveControl = cboOnPage;
        }
    }
}
