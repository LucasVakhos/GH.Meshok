using DevExpress.XtraEditors;
using System.ComponentModel;
namespace GH.Components
{
    [ToolboxItem(false)]
    public partial class InfoFrame : XtraUserControl, ITitleInfo
    {
        public InfoFrame()
        {
            InitializeComponent();
            AppContext.RegInfoPanel(this);
        }
        protected ITitle title => _source.Current as ITitle;
        BindingSource _source = null;
        public void RegDataSource(BindingSource source)
        {
            if (_source != null)
            {
                _source.PositionChanged -= Source_PositionChanged;
                _source = null;
            }
            _source = source;
            if (_source != null)
            {
                _source.PositionChanged += Source_PositionChanged;
                dataSource.CloseOpen();
            }
        }
        private void Source_PositionChanged(object sender, System.EventArgs e)
        {
            dataSource.ReOpenByTimer();
        }
    }
}
