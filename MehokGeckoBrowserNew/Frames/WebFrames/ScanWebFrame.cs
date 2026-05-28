using System;
using DevExpress.XtraEditors;
namespace MeshokBrowser.Workers
{
    public partial class ScanWebFrame : XtraUserControl
    {
        public ScanWebFrame()
        {
            InitializeComponent();
        }
        public void BrowserEnabled(bool enabled )
        {
            layoutControl.Enabled = enabled;
        }
    }
}
