using DevExpress.XtraLayout;
using System.ComponentModel;
namespace GH.Components
{
    public class DetailsFrame : LightDataFrame, IDetailsFrame
    {
        private DataSource _masterSource;
        private LayoutControlItem _place;
        private LayoutControlGroup _page;
        private TabbedGroup _pages;
        [MyProperty, Browsable(false)]
        public LayoutControlItem Place
        {
            get => _place;
            set
            {
                if (_place != null)
                {
                    _page = null;
                    _pages = null;
                }
                _place = value;
                if (_place != null)
                {
                    _page = null;
                    _pages = null;
                    if (Place.Parent != null)
                    {
                        _page = Place.Parent as LayoutControlGroup;
                        _pages = _page.ParentTabbedGroup;
                    }
                }
            }
        }
        public LayoutControlGroup Page => _page;
        public TabbedGroup PageControl => _pages;
        //public DetailsFrame(DataSource masterSource, LayoutControlItem place) : this()
        //{
        //    Place = place;
        //    MasterSource = masterSource;
        //}
        public DetailsFrame()
        {
            InitializeComponent();
        }
        [MyProperty, Browsable(false)]
        public DataSource MasterSource
        {
            get => _masterSource;
            set
            {
                if (_masterSource != null)
                {
                    dataSource.MasterDataSource = null;
                    _masterSource.ListChanged -= _masterSource_ListChanged;
                    _masterSource.PositionChanged -= _masterSource_PositionChanged;
                    _masterSource.AfterCancel -= _masterSource_PositionChanged;
                }
                _masterSource = value;
                if (_masterSource != null)
                {
                    dataSource.MasterDataSource = _masterSource;
                    _masterSource.ListChanged += _masterSource_ListChanged;
                    _masterSource.PositionChanged += _masterSource_PositionChanged;
                    _masterSource.AfterCancel += _masterSource_PositionChanged;
                }
            }
        }
        internal int Document_id
        {
            get
            {
                if (Document == null)
                    return -1000;
                return Document.id;
            }
        }
        internal ProtoEntity Document
        {
            get
            {
                if (MasterSource == null || MasterSource.Current == null)
                    return null;
                return MasterSource.Current as ProtoEntity;
            }
        }
        internal virtual bool Closed
        {
            get
            {
                if (MasterSource == null)
                    return true;
                return MasterSource.Closed;
            }
        }
        private void _masterSource_PositionChanged(object sender, EventArgs e)
        {
            DoStateChanged();
        }
        private void _masterSource_ListChanged(object sender, ListChangedEventArgs e)
        {
            switch (e.ListChangedType)
            {
                case ListChangedType.Reset:
                    break;
                case ListChangedType.ItemAdded:
                    break;
                case ListChangedType.ItemDeleted:
                    break;
                case ListChangedType.ItemMoved:
                    break;
                case ListChangedType.ItemChanged:
                    break;
                case ListChangedType.PropertyDescriptorAdded:
                    break;
                case ListChangedType.PropertyDescriptorDeleted:
                    break;
                case ListChangedType.PropertyDescriptorChanged:
                    break;
                default:
                    break;
            }
            DoStateChanged();
        }
        private void DoStateChanged()
        {
            StateChanged?.Invoke(this, EventArgs.Empty);
        }
        protected override void OnEnter(EventArgs e)
        {
            _masterSource.DisablePages(DisablePagesReason.DetailProcessing);
            base.OnEnter(e);
        }
        protected override void OnLeave(EventArgs e)
        {
            _masterSource.EnablePages(DisablePagesReason.DetailProcessing);
            base.OnLeave(e);
        }
        private void InitializeComponent()
        {
            ((System.ComponentModel.ISupportInitialize)(this.dataSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.actionList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lgRoot)).BeginInit();
            this.SuspendLayout();
            //this.dataSource.Owner = this;
            // 
            // layoutControl
            // 
            this.layoutControl.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(639, 135, 650, 400);
            this.layoutControl.OptionsFocus.AllowFocusGroups = false;
            this.layoutControl.OptionsFocus.AllowFocusReadonlyEditors = false;
            this.layoutControl.OptionsFocus.AllowFocusTabbedGroups = false;
            // 
            // DetailsFrame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.Name = "DetailsFrame";
            ((System.ComponentModel.ISupportInitialize)(this.dataSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.actionList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lgRoot)).EndInit();
            this.ResumeLayout(false);
        }
        [MyEvents]
        public event EventHandler StateChanged;
    }
}
