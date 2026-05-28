using Common;
using Gecko;
using Gecko.DOM;
using GH.Helpers;
namespace MeshokBrowser.Helpers
{
    public class ScanParams
    {
        static ScanParams _instance = null;
        public static ScanParams Instance(bool asNew = false)
        {
            if (_instance == null || asNew)
                _instance = new ScanParams();
            return _instance;
        }
        public GeckoHtmlElement scan_table = null;
        public GeckoFormElement scan_form = null;
        private string _c_nic;
        public string title_url;
        public string deal_url;
        public string deal_id;
        public string deal_status;
        public string deal_title;
        public string price;
        public string date;
        public bool need_split;
        public bool Is_test;
        public bool Is_new { get => deal_status == OrderStatus.New.GetDisplayValue(); }
        public string c_nic
        {
            get => _c_nic;
            set
            {
                _c_nic = value;
                int n = _c_nic.IndexOf("(");
                if (n > 0)
                    _c_nic = _c_nic.Substring(0, n).Trim();
            }
        }
        public ScanParams()
        {
            NewScan();
        }
        internal void NewScan()
        {
            _c_nic = "";
            title_url = "";
            deal_url = "";
            deal_id = "";
            deal_status = "";
            deal_title = "";
            price = "";
            date = "";
            need_split = false;
            Is_test = false;
        }
    }
}
