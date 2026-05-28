using DevExpress.Data;
using DevExpress.Export.Xl;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraNavBar;
using DevExpress.XtraPrinting;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
namespace GH.Components.FindFolder
{
    [ToolboxItem(true)]
    public partial class DataSource : BindingSource, ISupportInitialize
    {
        private object _lockInProcces = new object();
        private bool _initializing = true;
        public override ISite Site
        {
            get
            {
                return base.Site;
            }
            set
            {
                base.Site = value;
                if (value == null)
                    return;
                if (!(value.GetService(typeof(IDesignerHost)) is IDesignerHost service))
                    return;
                IComponent rootComponent = service.RootComponent;
                if (!(rootComponent is Control))
                    return;
                Owner = (Control)rootComponent;
            }
        }
        private bool _readOnly;
        public override bool IsReadOnly { get => _readOnly && base.IsReadOnly; }
        private bool _allowNew = true;
        [Browsable(false)]
        public override bool AllowNew
        {
            get => !IsReadOnly && _allowNew && base.AllowNew;
            set
            {
                _allowNew = value;
                base.AllowNew = value;
            }
        }
        private bool _isLocalDataSet = false;
        [GHProperty, DefaultValue(false), Description("Ставьте (IsLocalDataSet=true) если не нужно получать данные из другого источника а не из базы данных")]
        public bool IsLocalDataSet { get => _isLocalDataSet; set => _isLocalDataSet = value; }
        private bool _onlyOneRecordInDataSet = false;
        [GHProperty, DefaultValue(false), Description("Ставьте (OnlyOneRecordInDataSet=true) если нужно отображать только одну запись")]
        public bool OnlyOneRecordInDataSet
        {
            get
            {
                return _onlyOneRecordInDataSet;
            }
            set
            {
                _onlyOneRecordInDataSet = value;
            }
        }
        private DataSource _masterDataSource;
        [GHProperty, DefaultValue(null)]
        [Editor(typeof(DataSourceListEditor), typeof(UITypeEditor))]
        public DataSource MasterDataSource
        {
            get => _masterDataSource;
            set
            {
                if (_masterDataSource == value || value == this)
                    return;
                if (_masterDataSource != null && !DesignMode)
                {
                    _masterDataSource.UnRegDetailSource(this);
                }
                _masterDataSource = value;
                if (_masterDataSource != null && !DesignMode)
                {
                    _masterDataSource.RegDetailSource(this);
                }
            }
        }
        [GHProperty, DefaultValue(true)]
        public bool AllowSaveCancel
        {
            get => _allowSaveCancel;
            set
            {
                _allowSaveCancel = value;
            }
        }
        private GridColumn _colQty;
        [GHProperty, DefaultValue(null)]
        public GridColumn ColQty { get => _colQty; set => _colQty = value; }
        private DetailsList _detailSources = new DetailsList();
        private bool _needFocusGrid = true;
        [GHProperty, DefaultValue(true), Description("Устанавливать фокус на сетку")]
        public bool NeedFocusGrid { get => _needFocusGrid; set => _needFocusGrid = value; }
        private bool _needLoadingAnimate = true;
        [GHProperty, DefaultValue(true), Description("Анимировать загрузку")]
        public bool NeedLoadingAnimate { get => _needLoadingAnimate; set => _needLoadingAnimate = value; }
        private bool _immediatePostInsert = false;
        [GHProperty, DefaultValue(false), Description("Немедленно Post после Insert")]
        public bool ImmediatePostInsert { get => _immediatePostInsert; set => _immediatePostInsert = value; }
        private bool _askForDelete = true;
        [GHProperty, DefaultValue(true), Description("Задать вопрос при удалении")]
        public bool AskForDelete { get => _askForDelete; set => _askForDelete = value; }
        private bool _deleteAsUpdate;
        [GHProperty, DefaultValue(false), Description("Обновить вместо удаления")]
        public bool DeleteAsUpdate { get => _deleteAsUpdate; set => _deleteAsUpdate = value; }
        private bool _refreshAfterPost = true;
        [GHProperty, DefaultValue(true), Description("Обновить вместо удаления")]
        public bool RefreshAfterPost { get => _refreshAfterPost; set => _refreshAfterPost = value; }
        internal EditGrants _editGrants = new EditGrants(false, false, false);
        private bool _allowEdit = true;
        private bool _allowRemove = true;
        [GHProperty, DefaultValue(false)]
        public bool ReadOnly { get => _readOnly; set => _readOnly = value; }
        [GHProperty, DefaultValue(true)]
        public bool AllowInsert
        {
            get => _allowNew;
            set
            {
                _allowNew = value;
            }
        }
        [GHProperty, DefaultValue(true)]
        public bool AllowUdate
        {
            get => _allowEdit;
            set
            {
                _allowEdit = value;
            }
        }
        [GHProperty, DefaultValue(true)]
        public bool AllowDelete
        {
            get => _allowRemove;
            set
            {
                _allowRemove = value;
            }
        }
        private Type _entityType;
        [Browsable(false)]
        public Type EntityType
        {
            get
            {
                if (this.DataSource == null)
                    _entityType = null;
                else if (_entityType == null)
                {
                    FieldInfo fi = typeof(BindingSource).GetField("itemType", BindingFlags.NonPublic | BindingFlags.Instance);
                    _entityType = fi.GetValue(this) as Type;
                }
                return _entityType;
            }
        }
        private bool _allowSaveCancel = true;
        private IList<BindingControlMap> _bindingControls = new List<BindingControlMap>();
        private List<DisablePagesReason> _disablePagesReasons = new List<DisablePagesReason>();
        [Browsable(false)]
        public bool SkipPageSupport => _disablePagesReasons.Count > 0;
        private Control _owner;
        [GHProperty, Browsable(true), DefaultValue(null)]
        public Control Owner
        {
            get
            {
                return _owner;
            }
            set
            {
                if (_owner == value)
                    return;
                if (_owner != null)
                    throw new Exception("switching ActionList to another container is not supported");
                _owner = value;
            }
        }
        private GridControl _grid;
        [GHProperty, DefaultValue(null)]
        public GridControl Grid
        {
            get => _grid;
            set
            {
                if (_grid != null)
                    _grid.DataSource = null;
                _grid = value;
                if (value != null && value.DataSource == null)
                    value.DataSource = this;
            }
        }
        internal GridView View
        {
            get
            {
                if (_grid == null)
                    return null;
                return _grid.MainView as GridView;
            }
        }
        private bool _Opening = false;
        private IList<AbstractEntity> _inProcList = new List<AbstractEntity>();
        internal INHRepository _repository;
        [Browsable(false)]
        public INHRepository Repository
        {
            get
            {
                if (_repository == null)
                    InitRepository();
                return _repository;
            }
        }
        internal bool _refreshingAll = false;
        internal System.Windows.Forms.Timer _reopenTimer;
        private object _saved;
        //public LayoutControlGroup GroupEdit { get; set; }
        //public LayoutControlGroup PageEdit { get; set; }
        //public LayoutControlGroup PageView { get; set; }
        //[ReadOnly(false)]
        //public string CloseOpenField { get => _closeOpenField; set => _closeOpenField = value; }
        internal string _closeOpenField;
        internal string _statusField;
        internal int _statusOpened;
        internal int _statusClosed;
        //[ReadOnly(false)]
        //public string CountField { get => _countField; set => _countField = value; }
        internal string _countField;
        internal bool Closable
        {
            get
            {
                return Count > 0 && _closeOpenField != null;
            }
        }
        [Browsable(false)]
        public bool Closed
        {
            get
            {
                return Closable && Entity.AsBoolean(_closeOpenField);
            }
            private set
            {
                if (Closable)
                    Entity.AsBoolean(_closeOpenField, value);
            }
        }
        internal int DocCnt
        {
            get
            {
                if (Closable && _countField != null)
                    return Entity.AsInteger(_countField);
                return 0;
            }
        }
        internal int Status
        {
            get
            {
                if (Closable && _statusField != null)
                    return Entity.AsInteger(_statusField);
                return 0;
            }
        }
        [Browsable(false)]
        public AbstractEntity Entity
        {
            get
            {
                return Current as AbstractEntity;
            }
        }
        [Browsable(false)]
        public bool EditMode
        {
            get
            {
                return State == DataState.Editing || State == DataState.Inserting;
            }
        }
        private ActionList _actionList;
        [GHProperty, DefaultValue(null)]
        public ActionList ActionList { get => _actionList; set => _actionList = value; }
        private ButtonsPanel _buttonsPanel = null;
        [GHProperty, Browsable(false)]
        public ButtonsPanel ButtonsPanel => _buttonsPanel;
        private PageSupport _pageSupport;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [GHProperty]
        public PageSupport PageSupport => _pageSupport;
        internal bool SupportPages => _pageSupport.Supported;
        private DataState _editState = DataState.Browsing;
        private DocSupport _docSupport;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [GHProperty]
        public DocSupport DocSupport => _docSupport;
        [Browsable(false)]
        public DataState EditState { get => _editState; }
        private DataState _state = DataState.Inactive;
        [Browsable(false), DefaultValue(DataState.Inactive)]
        public DataState State
        {
            get => _state;
            set
            {
                if (_initializing)
                {
                    _state = DataState.Inactive;
                    return;
                }
                if (DesignMode || value == _state)
                    return;
                if (value == _state || (InEditMode(value) && InEditMode(_state)))
                    return;
                if (value == DataState.Refreshing || _state == DataState.Refreshing && value == DataState.Browsing)
                {
                    _state = value;
                    return;
                }
                if (_state == DataState.Browsing && _editState == DataState.Browsing && InEditMode(value))
                {
                    _editState = value;
                    if (OnEditBegin != null)
                        _state = DataState.BeginEditing;
                    if (!SupportPages)
                        if (!ShowGridEditor())
                        {
                            //_state = value;
                            if (OnEditBegin != null)
                            {
                                OnEditBegin?.Invoke(this, EventArgs.Empty);
                                return;
                            }
                        }
                }
                if (InEditMode(_editState) && value == DataState.Browsing)
                    _editState = DataState.Browsing;
                _state = value;
                if (_state == DataState.Browsing)
                    CheckBindingControlsReadOnly();
                CheckPages(_state);
                if (_state != DataState.Refreshing)
                    _pageSupport.CheckPages(value);
            }
        }
        public DataSource(IContainer container) : base(container)
        {
            CreateSupports();
        }
        public DataSource() : base()
        {
            CreateSupports();
        }
        protected override void OnPositionChanged(EventArgs e)
        {
            base.OnPositionChanged(e);
            if (_initializing)
                return;
            GetGrants();
            CheckBindingControlsReadOnly();
            if (_Opening)
                return;
            if (State == DataState.Browsing)
                _detailSources.ReOpenDetailsByTimer();
        }
        protected override void OnDataSourceChanged(EventArgs e)
        {
            base.OnDataSourceChanged(e);
            if (_initializing)
                return;
            if (DataSource == null)
                State = DataState.Inactive;
            else
                State = DataState.Browsing;
            GetGrants();
        }
        protected override void OnListChanged(ListChangedEventArgs e)
        {
            base.OnListChanged(e);
            if (_initializing)
                return;
            //if (_Opening || DataSource == null)
            //{
            //    base.OnListChanged(e);
            //    if (List.Count == 0)
            //        State = DataState.Inactive;
            //    try
            //    {
            //        if (Entity != null)
            //            Entity.CancelEdit();
            //    }
            //    catch { }
            //    return;
            //}
            switch (e.ListChangedType)
            {
                case ListChangedType.Reset:
                    if (State == DataState.Inserting && Count == 1)
                        break;
                    if (State != DataState.Browsing && Count > 0)
                        State = DataState.Browsing;
                    break;
                case ListChangedType.ItemAdded:
                    if (State != DataState.Inserting)
                        State = DataState.Inserting;
                    break;
                case ListChangedType.ItemDeleted:
                    State = DataState.Browsing;
                    break;
                case ListChangedType.ItemChanged:
                    switch (State)
                    {
                        case DataState.Inserting:
                            if (!Entity.HasChanges)
                                State = DataState.Browsing;
                            break;
                        case DataState.Refreshing:
                            Entity.EndEdit();
                            State = DataState.Browsing;
                            break;
                        default:
                            if (Entity == null)
                                State = DataState.Inactive;
                            else if (Entity.HasChanges)
                                State = DataState.Editing;
                            else
                                State = DataState.Browsing;
                            break;
                    }
                    break;
                default:
                    break;
            }
        }
        public virtual void BeginInit()
        {
            _initializing = true;
        }
        public virtual void EndInit()
        {
            if (!this.IsDesignMode() && this.DataSource != null)
            {
                InitRepository();
                InitGrid();
                _pageSupport.EndInit();
                PrepareEditGroup();
                if (!InitActions())
                    return;
                InitRibbonEditGroup();
                InitNavBars();
                InitStripMenu();
            }
            _initializing = false;
        }
        private bool ShowGridEditor()
        {
            if (View != null && ColQty != null)
                View.FocusedColumn = ColQty;
            if (OnEditBegin == null)
            {
                if (View != null && ColQty != null)
                {
                    View.OptionsBehavior.ReadOnly = false;
                    View.OptionsBehavior.Editable = true;
                    ColQty.OptionsColumn.AllowEdit = true;
                    View.ShowEditor();
                    return true;
                }
                else
                {
                    State = DataState.Browsing;
                }
            }
            return false;
        }
        private void InitGrid()
        {
            if (ColQty != null)
            {
                GridView view = ColQty.View as GridView;
                foreach (GridColumn item in view.Columns)
                {
                    item.OptionsColumn.AllowEdit = item == ColQty;
                    item.OptionsColumn.ReadOnly = item != ColQty;
                    if (item == ColQty)
                    {
                        item.OptionsColumn.AllowIncrementalSearch = false;
                        EdQty edQty = new EdQty(this);
                    }
                }
            }
            if (Grid != null)
            {
                Grid.Enter += Ctrl_Enter;
                Grid.Leave += Ctrl_Leave;
            }
            else if (Owner != null)
            {
                Owner.Enter += Ctrl_Enter;
                Owner.Leave += Ctrl_Leave;
            }
        }
        internal void AddBindingControl(BaseEdit edit)
        {
            var map = new BindingControlMap(edit);
            if (_bindingControls.Contains(map))
                return;
            _bindingControls.Add(map);
            edit.Enter += Ctrl_Enter;
            edit.Leave += Ctrl_Leave;
        }
        private bool InEditMode(DataState value)
        {
            return value == DataState.Editing || value == DataState.Inserting;
        }
        private void CreateSupports()
        {
            BeginInit();
            _navbaritems = new List<NavBarItem>();
            _pageSupport = new PageSupport(this);
            _docSupport = new DocSupport(this);
            EndInit();
        }
        public void ReOpenByTimer()
        {
            if (DesignMode)
                return;
            ReopenTimerStop();
            //Close();
            ReopenTimerStart();
        }
        public void Close()
        {
            State = DataState.Inactive;
            if (Count > 0)
            {
                _saved = Entity;
                Clear();
            }
            else
                ResetBindings(false);
        }
        internal void ReopenTimerStart()
        {
            if (DesignMode)
                return;
            if (_reopenTimer == null)
            {
                _reopenTimer = new System.Windows.Forms.Timer();
                _reopenTimer.Interval = 300;
                _reopenTimer.Tick += _reopenTimer_Tick;
            }
            else
                _reopenTimer.Stop();
            _reopenTimer.Start();
        }
        internal void ReopenTimerStop()
        {
            if (_reopenTimer != null)
            {
                _reopenTimer.Stop();
                _reopenTimer.Tick -= _reopenTimer_Tick;
                _reopenTimer.Dispose();
                _reopenTimer = null;
            }
        }
        private void _reopenTimer_Tick(object sender, EventArgs e)
        {
            ReopenTimerStop();
            CloseOpen();
        }
        protected virtual void OnBeforeOpen()
        {
            _inProcList.Clear();
            BeforeOpen?.Invoke(this, EventArgs.Empty);
            _Opening = true;
        }
        private string GetSql(SqlTypes sqlType, BaseEntity item)
        {
            string result = null;
            GetSqlString?.Invoke(sqlType, item, out result);
            return result;
        }
        private void InitRepository()
        {
            if (_repository != null)
                return;
            GetRepository?.Invoke(out _repository);
            if (_repository != null)
            {
                _repository.GetSQL = GetSql;
                _repository.GetParams = WhereParams;
                _repository.GetSorting = GridSorting;
                _repository.DeleteFinish = DeleteFinish;
                _repository.PostFinish = PostFinish;
                _repository.CloseOpenDocFinish = CloseOpenDocFinish;
                _repository.Control = Owner;
                _repository.RefreshAfterPost = RefreshAfterPost;
                _repository.NeedLoadingAnimate = NeedLoadingAnimate;
            }
        }
        private void CloseOpenDocFinish(object entity)
        {
            EndEdit();
            RaiseListChangedEvents = true;
            ResetCurrentItem();
            State = DataState.Browsing;
            CheckBindingControlsReadOnly();
            MasterDataSource?.Refresh();
            GetGrants();
            DoAfterCloseOpenDoc();
            EnablePages(DisablePagesReason.ClosingOrOpening);
        }
        private void PostFinish(object entity)
        {
            if (Entity == entity)
            {
                EndEdit();
                ResetCurrentItem();
            }
            else
                ResetItem(IndexOf(entity));
            CheckBindingControlsReadOnly();
            State = DataState.Browsing;
            AfterPost?.Invoke(this, EventArgs.Empty);
            MasterDataSource?.Refresh();
            EnablePages(DisablePagesReason.Inserting);
            InProcces(entity as AbstractEntity, false);
            GetGrants();
        }
        private void DeleteFinish(object entity)
        {
            if (State == DataState.Editing)
            {
                PostFinish(entity);
                return;
            }
            if (Grid == null)
                Remove(entity);
            else
            {
                int rh = (Grid.MainView as GridView).FocusedRowHandle;
                Remove(entity);
                if ((Grid.MainView as GridView).RowCount == rh)
                    rh--;
                if (rh >= 0)
                    (Grid.MainView as GridView).FocusedRowHandle = rh;
            }
            AfterDelete?.Invoke(this, EventArgs.Empty);
            State = DataState.Browsing;
            MasterDataSource?.Refresh();
            GetGrants();
        }
        private Dictionary<string, bool> GridSorting()
        {
            if (View == null)
                return null;
            Dictionary<string, bool> result = new Dictionary<string, bool>();
            foreach (GridColumn column in View.SortedColumns)
                result.Add(column.FieldName, column.SortOrder == ColumnSortOrder.Ascending);
            if (result.Count == 0)
                return null;
            return result;
        }
        private Dictionary<string, object> WhereParams()
        {
            Dictionary<string, object> whereParams = new Dictionary<string, object>();
            GetWhereParams?.Invoke(this, whereParams);
            if (whereParams.Count == 0)
                return null;
            return whereParams;
        }
        private IList GetList(IList lst)
        {
            if (_isLocalDataSet)
            {
                if (OnOpen == null)
                    throw new Exception("Назначте событие \"OnOpen\" у DataSource на форме " + Owner.Name);
                OnOpen(out lst);
            }
            else
            {
                if (Repository == null)
                {
                    if (GetRepository == null)
                        throw new Exception("Назначте событие GetRepository у " + Owner.Name);
                    InitRepository();
                }
                if (Repository != null)
                    DataSource = Repository.ConcreteType;
                if (Repository != null)
                    lst = Repository.SelectAll();
            }
            return lst;
        }
        internal void CheckBindingControlsReadOnly()
        {
            if (_bindingControls.Count > 0 && !_readOnly)
            {
                foreach (var item in _bindingControls)
                {
                    if (item.ReadOnly)
                        continue;
                    ReadOnlyEventArgs e = new ReadOnlyEventArgs(item.Control);
                    if (!e.ReadOnly)
                    {
                        if (State != DataState.Inserting && Count == 0)
                            e.ReadOnly = true;
                        if (!e.ReadOnly)
                            CheckControlsReadOnly?.Invoke(this, e);
                    }
                    item.Control.ReadOnly = e.ReadOnly;
                }
            }
        }
        protected virtual void OnAfterOpen()
        {
            _detailSources.ReOpenDetailsByTimer();
            //ReOpenDetailsByTimer();
            CheckBindingControlsReadOnly();
            _Opening = false;
            AfterOpen?.Invoke(this, EventArgs.Empty);
        }
        private void FocusGrid()
        {
            if (Grid != null && _needFocusGrid)
            {
                if (Grid.InvokeRequired)
                    return;
                if (Grid.CanFocus)
                    Grid.Focus();
            }
        }
        public void Open()
        {
            OnBeforeOpen();
            try
            {
                IList lst = null;
                lst = GetList(lst);
                if (lst != null)
                {
                    DataSource = lst;
                    if (_saved != null)
                    {
                        int pos = IndexOf(_saved);
                        if (pos > -1)
                            Position = pos;
                    }
                }
            }
            finally
            {
                OnAfterOpen();
                State = DataState.Browsing;
                FocusGrid();
            }
        }
        internal void CheckPages(DataState dataState)
        {
            _pageSupport.CheckPages(dataState);
        }
        private void RegDetailSource(DataSource detail)
        {
            _detailSources.RegDataSource(detail);
        }
        private void UnRegDetailSource(DataSource detail)
        {
            _detailSources.UnRegDataSource(detail);
        }
        internal bool InProcces(AbstractEntity bindable, bool add)
        {
            lock (_lockInProcces)
            {
                if (add)
                {
                    if (_inProcList.IndexOf(bindable) > -1)
                        return true;
                    _inProcList.Add(bindable);
                }
                else
                    _inProcList.Remove(bindable);
            }
            return false;
        }
        private bool DoValidateControl(BaseEdit control)
        {
            if (ValidateControl == null)
                return true;
            ValidateEventArgs e = new ValidateEventArgs(control, true);
            ValidateControl.Invoke(this, e);
            return e.IsValid;
        }
        internal bool ValidateBindingControls()
        {
            if (_bindingControls.Count == 0)
                return true;
            foreach (var item in _bindingControls)
            {
                if (item.ReadOnly)
                    continue;
                if (!item.Control.DoValidate(PopupCloseMode.Cancel) || !DoValidateControl(item.Control))
                {
                    item.Control.SelectAll();
                    item.Control.Focus();
                    DlgHelper.DlgError("Заполните поле!!");
                    return false;
                }
            }
            return true;
        }
        private bool InternalPost()
        {
            if (InProcces(Entity, true))
                return false;
            if (!ValidateBindingControls())
            {
                InProcces(Entity, false);
                return false;
            }
            if (Entity.HasChanges || State == DataState.Editing || State == DataState.Inserting)
            {
                BeforePost?.Invoke(this, EventArgs.Empty);
                if (_isLocalDataSet)
                {
                    OnPost?.Invoke(this, EventArgs.Empty);
                    PostFinish(Entity);
                }
                else if (State == DataState.Inserting)
                    Repository.Save(Current);
                else
                    Repository.SaveOrUpdate(Current);
            }
            else
                InProcces(Entity, false);
            return true;
        }
        public void DisablePages(DisablePagesReason reason)
        {
            if (_disablePagesReasons.IndexOf(reason) == -1)
                _disablePagesReasons.Add(reason);
        }
        public void Insert()
        {
            if (!_editGrants.AllowNew)
                return;
            State = DataState.Inserting;
            AddNew();
            AfterInsert?.Invoke(this, EventArgs.Empty);
            if (_immediatePostInsert)
            {
                DisablePages(DisablePagesReason.Inserting);
                Application.DoEvents();
                Post();
            }
        }
        public virtual void Edit()
        {
            if (_editGrants.AllowEdit)
            {
                if (CheckCanEdit())
                {
                    if (Count == 0)
                    {
                        Insert();
                        return;
                    }
                    State = DataState.Editing;
                }
                else if (Entity != null && Entity.HasChanges)
                    Entity.CancelEdit();
            }
            else if (SupportPages)
                CheckPages(DataState.Editing);
        }
        private bool CheckCanEdit()
        {
            bool canEdit = true;
            if (CanEdit != null)
                canEdit = CanEdit.Invoke();
            if (!canEdit && View != null && View.IsEditorFocused)
                View.HideEditor();
            return canEdit;
        }
        public void Refresh()
        {
            if (Entity != null)
            {
                State = DataState.Refreshing;
                if (OnRefresh != null)
                {
                    OnRefresh();
                }
                else if (_repository != null)
                {
                    _repository.Refresh(Entity);
                }
                RefreshFinish(Entity);
            }
        }
        internal void RefreshFinish(object entity)
        {
            RunContext.Invoke(() =>
            {
                ResetCurrentItem();
                CheckBindingControlsReadOnly();
            });
        }
        private void HideGridEditor()
        {
            if (View != null)
            {
                View.OptionsBehavior.ReadOnly = true;
                View.OptionsBehavior.Editable = false;
                View.HideEditor();
                if (State == DataState.Canceling)
                    View.CancelUpdateCurrentRow();
                else
                    View.UpdateCurrentRow();
            }
            if (State != DataState.Canceling)
                State = DataState.Browsing;
        }
        public void Post()
        {
            if (!InternalPost())
                return;
            if (View != null && View.IsEditorFocused)
                HideGridEditor();
        }
        public void EnablePages(DisablePagesReason reason)
        {
            _disablePagesReasons.Remove(reason);
        }
        protected void GetGrants()
        {
            EditGrants e = new EditGrants(_allowNew && !IsReadOnly && State == DataState.Browsing, _allowEdit && !IsReadOnly && State == DataState.Browsing, _allowRemove && !IsReadOnly && State == DataState.Browsing);
            GetEditGrants?.Invoke(this, e);
            _editGrants = e;
        }
        private void PrepareEditGroup()
        {
            if (!AllowSaveCancel || ReadOnly || PageSupport.EditGroup == null || _buttonsPanel != null)
                return;
            if (PageSupport.EditGroup.Owner is LayoutControlGh layoutControl)
                _buttonsPanel = layoutControl.PrepareGroup(PageSupport.EditGroup);
        }
        public void AddSaveSaveCancelPanel()
        {
            if (_buttonsPanel != null)
                return;
            if (PageSupport.EditGroup.Owner is LayoutControlGh layoutControl)
                _buttonsPanel = layoutControl.AddSaveSaveCancelPanel(PageSupport.EditGroup);
            if (_actionList.Actions.Count > 0)
            {
                foreach (DataAction action in _actionList.Actions.Cast<DataAction>())
                {
                    switch (action.ButtonType)
                    {
                        case EditTypes.Save:
                            _actionList.SetAction(_buttonsPanel.btnOK, action);
                            break;
                        case EditTypes.Cancel:
                            _actionList.SetAction(_buttonsPanel.btnCancel, action);
                            break;
                        default:
                            break;
                    }
                }
            }
            else
            {
                int pos = 0;
                CreateAction(EditTypes.Save, pos);
                CreateAction(EditTypes.Cancel, pos);
            }
        }
        public void ExecuteAction(EditTypes type)
        {
            if (_actionList == null)
                return;
            ActionGh action = _actionList.Actions.Where(x => x is DataAction item && item.ButtonType == type).FirstOrDefault();
            if (action == null)
                return;
            action.DoExecute();
        }
        protected string GetQueryText(SqlTypes sqlType, bool closed = false)
        {
            YesNoTextArgs e = new YesNoTextArgs(sqlType, Entity, closed);
            GetYesNoText?.Invoke(e);
            return e.ToString();
        }
        internal void InternalDelete()
        {
            if (_isLocalDataSet)
            {
                OnDelete?.Invoke(this, EventArgs.Empty);
                DeleteFinish(Entity);
            }
            else if (Repository != null)
                Repository.Delete(Current);
        }
        public void Delete()
        {
            if (!_editGrants.AllowRemove || Count == 0)
                return;
            if (AskForDelete && !DlgHelper.DlgYesNo(GetQueryText(SqlTypes.DeleteSql)))
                return;
            State = DataState.Deleting;
            if (DeleteAsUpdate)
                State = DataState.Editing;
            if (OnDelete != null)
            {
                OnDelete(this, EventArgs.Empty);
            }
            else
                InternalDelete();
        }
        public void Cancel()
        {
            if (DesignMode)
                return;
            switch (State)
            {
                case DataState.Inserting:
                case DataState.Editing:
                    State = DataState.Canceling;
                    break;
                default:
                    return;
            }
            HideGridEditor();
            EnablePages(DisablePagesReason.Inserting);
            switch (State)
            {
                case DataState.Canceling:
                    break;
                default:
                    State = DataState.Browsing;
                    return;
            }
            OnCancel?.Invoke(this, EventArgs.Empty);
            CancelEdit();
            ResetCurrentItem();
            State = DataState.Browsing;
            AfterCancel?.Invoke(this, EventArgs.Empty);
        }
        //private void SearchByHttp(EditTypes buttonsType)
        //{
        //    ITitle article = Entity as ITitle;
        //    if (article == null)
        //        return;
        //    switch (buttonsType)
        //    {
        //        case EditTypes.SearchDiscogs:
        //            if (string.IsNullOrEmpty(article.c_barcode))
        //                return;
        //            /*https://www.discogs.com/search/?q=0090771531315&type=all*/
        //            OpenUrl(new UriBuilder()
        //            {
        //                Scheme = "https",
        //                Host = "www.discogs.com",
        //                Path = "/search/",
        //                Query = $"q={article.c_barcode.Trim()}&type=all"
        //            }.Uri.AbsoluteUri);
        //            break;
        //        case EditTypes.SearchWikipedia:
        //            if (string.IsNullOrEmpty(article.art_name))
        //                return;
        //            OpenUrl(new UriBuilder()
        //            {
        //                Scheme = "http",
        //                Host = "en.wikipedia.org",
        //                Path = "/wiki/Special:Search",
        //                Query = "search=" + HttpUtility.HtmlEncode(Words(article.art_name.Trim() + " " + article.c_title.Trim(), "+"))
        //            }.Uri.AbsoluteUri);
        //            break;
        //        case EditTypes.SearchAllMusic:
        //            if (string.IsNullOrEmpty(article.art_name))
        //                return;
        //            OpenUrl(new UriBuilder()
        //            {
        //                Scheme = "http",
        //                Host = "www.allmusic.com",
        //                Path = "/search/artists/" + HttpUtility.HtmlEncode(Words(article.art_name.Trim(), " ")),
        //                Query = ""
        //            }.Uri.AbsoluteUri);
        //            break;
        //        case EditTypes.SearchGoogle:
        //            OpenUrl(new UriBuilder()
        //            {
        //                Scheme = "http",
        //                Host = "www.google.com",
        //                Path = "/search",
        //                Query = "q=" + HttpUtility.HtmlEncode(Words(article.art_name.Trim() + " " + article.c_title.Trim(), "+"))
        //            }.Uri.AbsoluteUri);
        //            break;
        //        default:
        //            break;
        //    }
        //}
        private string Words(string input, string delimiter)
        {
            StringBuilder stringBuilder = new StringBuilder();
            string str = "";
            foreach (Match match in new Regex("\\b(\\w+)\\b").Matches(input))
            {
                stringBuilder.Append(str + match.Groups[1].Value);
                str = delimiter;
            }
            return stringBuilder.ToString();
        }
        public void ExportData(string toFolder = null)
        {
            if (Grid != null)
            {
                ExportArgs e = new ExportArgs((Owner as ICaption)?.Caption);
                GetExportArguments?.Invoke(e);
                string ext = Path.GetExtension(e.FileName).ToUpper();
                if (ext == ".XLS" || ext == ".XLSX")
                    e.FileName = Path.GetFileNameWithoutExtension(e.FileName);
                if (toFolder == null)
                    toFolder = IniHelper.CfgAppForm().ExportPath;
                e.FileName = Path.Combine(Application.StartupPath, toFolder, string.Format("{0}.xls", e.FileName));
                if (File.Exists(e.FileName))
                {
                    try
                    {
                        File.Delete(e.FileName);
                    }
                    catch
                    {
                        DlgHelper.DlgError($"Файл {e.FileName} открыт в другой программе!\r\nЗакройе его и запустите экспорт снова...");
                        return;
                    }
                }
                else
                    Directory.CreateDirectory(Path.GetDirectoryName(e.FileName));
                XlsExportOptionsEx options = new XlsExportOptionsEx();
                options.LayoutMode = DevExpress.Export.LayoutMode.Table;
                options.ShowTotalSummaries = DevExpress.Utils.DefaultBoolean.True;
                options.AllowFixedColumnHeaderPanel = DevExpress.Utils.DefaultBoolean.True;
                options.BeforeExportTable += be =>
                {
                    be.Table.Style.Name = XlBuiltInTableStyleId.Light1;
                };
                Grid.ExportToXls(e.FileName, options);
                OpenFile(e.FileName);
            }
        }
        private bool CanExecuteSaveAction()
        {
            if (Closed && (Status > _statusClosed || Status < _statusOpened))
                return false;
            return true;
        }
        private void ExecuteSaveAction(DataAction action)
        {
            if (EditMode)
                Post();
            else if (Closable)
                CloseOpenDoc();
        }
        protected void CloseOpenDoc()
        {
            if (!DlgHelper.DlgYesNo(GetQueryText(SqlTypes.CloseDocSql, Closed)))
                return;
            Repository.Refresh(Entity);
            if (!CanExecuteSaveAction())
            {
                DlgHelper.DlgWarning("Открытие документа не может быть произведено по причине того, что статус документа\r\n" + "вышел за пределы рабочих статусов для данного типа документов:\r\n" + $"статус ={Status}; пределы ({_statusOpened} .. {_statusClosed}).\r\n" + "Вероятно, по этому документу уже были произведены последующие операции, не позволяющие его открыть...");
                return;
            }
            DisablePages(DisablePagesReason.ClosingOrOpening);
            RaiseListChangedEvents = false;
            DoBeforeCloseOpenDoc();
            Closed = !Closed;
            Repository.CloseOpenDoc(Entity);
        }
        private void DoBeforeCloseOpenDoc()
        {
            if (SupportPages)
            {
                if (PageSupport.ViewSelected && Closed)
                    PageSupport.EditSelect();
            }
        }
        private void DoAfterCloseOpenDoc()
        {
            if (SupportPages)
            {
                if (PageSupport.EditSelected && Closed)
                    PageSupport.ViewSelect();
            }
            AfterCloseOpenDocument?.Invoke(this, EventArgs.Empty);
        }
        public void CloseOpen()
        {
            Close();
            Open();
        }
        protected void RefreshAll()
        {
            CloseOpen();
        }
        [GHEvents]
        public event EventHandler BeforeOpen;
        [GHEvents]
        public event OpenHandler OnOpen;
        [GHEvents]
        public event EventHandler AfterOpen;
        [GHEvents]
        public event EventHandler BeforePost;
        [GHEvents]
        public event EventHandler OnPost;
        [GHEvents]
        public event EventHandler AfterPost;
        [GHEvents]
        public event EventHandler OnCancel;
        [GHEvents]
        public event EventHandler AfterCancel;
        [GHEvents]
        public event EventHandler AfterCloseOpenDocument;
        [GHEvents]
        public event CloseActionUpdateHandler CloseActionUpdate;
        [GHEvents]
        public event ActionUpdateHandler OnUpdateInnerAction;
        [GHEvents]
        public event GetActionCaptionHandler GetActionCaption;
        [GHEvents]
        public event ExportArgumenstHandler GetExportArguments;
        [GHEvents]
        public event EditGrantHandler GetEditGrants;
        [GHEvents]
        public event EventHandler OnDelete;
        [GHEvents]
        public event EventHandler AfterDelete;
        [GHEvents]
        public event EventHandler AfterInsert;
        [GHEvents]
        public event GetRepository GetRepository;
        [GHEvents]
        public event GetWhereParamsHandler GetWhereParams;
        [GHEvents]
        public event EventHandler OnEditBegin;
        [GHEvents]
        public event CanEditHandler CanEdit;
        [GHEvents]
        public event ActionWithoutParams OnRefresh;
        [GHEvents]
        public event ReadOnlyEventHandler CheckControlsReadOnly;
        [GHEvents]
        public event OnGetSqlString GetSqlString;
        [GHEvents]
        public event ValidateHandler ValidateControl;
        [GHEvents]
        public event YesNoTextHandler GetYesNoText;
    }
}
