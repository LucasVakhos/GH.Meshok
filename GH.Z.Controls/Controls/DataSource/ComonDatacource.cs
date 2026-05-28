using DevExpress.Data;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Threading.Tasks;
using System.Windows.Forms;
using static GH.Components.UtilsGh;
using Timer = System.Windows.Forms.Timer;

namespace GH.Components
{
    [ToolboxItem(false)]
    public partial class CommonDataSource : BindingSource, IFinalInit, ISupportInitialize
    {
        

        #region Fields & props
        internal bool _refreshingAll = false;
        internal Timer _reopenTimer;

        private List<DisablePagesReason> _disablePagesReasons = new List<DisablePagesReason>();

        private IList<BindingControlMap> _bindingControls = new List<BindingControlMap>();


        internal virtual bool SupportPages => false;
        private DataState _editState = DataState.Browsing;

        [Browsable(false)]
        public DataState EditState { get => _editState; }

        private DataState _state = DataState.Inactive;

        [Browsable(false)]
        public virtual DataState State
        {
            get => _state;
            set
            {
                if (value == _state || (EditMode(value) && EditMode(_state)))
                    return;

                if (value == DataState.Refreshing || _state == DataState.Refreshing && value == DataState.Browsing)
                {
                    _state = value;
                    return;
                }

                if (_state == DataState.Browsing && _editState == DataState.Browsing && EditMode(value))
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

                if (EditMode(_editState) && value == DataState.Browsing)
                    _editState = DataState.Browsing;


                _state = value;


                if (_state == DataState.Browsing)
                    CheckBindingControlsReadOnly();

                CheckPages(_state);
            }
        }

        private bool _needFocusGrid = true;
        [MyProperty, DefaultValue(true), Description("Устанавливать фокус на сетку")]
        public bool NeedFocusGrid { get => _needFocusGrid; set => _needFocusGrid = value; }

        private bool _needLoadingAnimate = true;
        [MyProperty, DefaultValue(true), Description("Анимировать загрузку")]
        public bool NeedLoadingAnimate { get => _needLoadingAnimate; set => _needLoadingAnimate = value; }

        private bool _immediatePostInsert = false;
        [MyProperty, DefaultValue(false), Description("Немедленно Post после Insert")]
        public bool ImmediatePostInsert { get => _immediatePostInsert; set => _immediatePostInsert = value; }

        private bool _askForDelete = true;
        [MyProperty, DefaultValue(true), Description("Задать вопрос при удалении")]
        public bool AskForDelete { get => _askForDelete; set => _askForDelete = value; }

        private bool _deleteAsUpdate;
        [MyProperty, DefaultValue(false), Description("Обновить вместо удаления")]
        public bool DeleteAsUpdate { get => _deleteAsUpdate; set => _deleteAsUpdate = value; }

        private bool _refreshAfterPost = true;
        [MyProperty, DefaultValue(true), Description("Обновить вместо удаления")]
        public bool RefreshAfterPost { get => _refreshAfterPost; set => _refreshAfterPost = value; }

        protected bool IsDesignMode
        {
            get
            {
                return this.IsDesignMode();
            }
        }

        private Control _owner;
        [MyProperty, Browsable(true), DefaultValue(null)]
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
                //RegisterToOwner();

            }
        }

        private GridColumn _colQty;
        [MyProperty, DefaultValue(null)]
        public GridColumn ColQty { get => _colQty; set => _colQty = value; }

        CommonDataSource _masterDataSource;

        [MyProperty, DefaultValue(null)]
        [Editor(typeof(DataSourceListEditor), typeof(UITypeEditor))]
        public CommonDataSource MasterDataSource
        {
            get => _masterDataSource;
            set
            {
                if (_masterDataSource == value || value == this)
                    return;

                if (_masterDataSource != null && !IsDesignMode)
                {
                    _masterDataSource.UnRegDetailSource(this);
                }

                _masterDataSource = value;

                if (_masterDataSource != null && !IsDesignMode)
                {
                    _masterDataSource.RegDetailSource(this);
                }
            }
        }

        private void RegDetailSource(CommonDataSource detail)
        {
            _detailSources.RegDataSource(detail);
        }

        private void UnRegDetailSource(CommonDataSource detail)
        {
            _detailSources.UnRegDataSource(detail);
        }

        DetailsList _detailSources = new DetailsList();
        //private DetailsList DetailSources { get => _detailSources; }


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

        private GridControl _grid;
        [MyProperty, DefaultValue(null)]
        public GridControl Grid
        {
            get => _grid;
            set
            {
                if (_grid != null)
                    _grid.DataSource = null;
                _grid = value;
                if (_grid != null)
                    _grid.DataSource = this;

            }
        }

        protected GridView View
        {
            get
            {
                if (_grid == null)
                    return null;
                return _grid.MainView as GridView;
            }
        }

        internal IUnitOfWork _repository;

        [Browsable(false)]
        public IUnitOfWork Repository
        {
            get
            {
                if (_repository == null)
                    InitRepository();

                return _repository;
            }
        }


        [Browsable(false)]
        public ProtoEntity Entity
        {
            get
            {
                return Current as ProtoEntity;
            }
            //set
            //{
            //    if (Count > 0)
            //    {
            //        List[Position] = value;
            //        SyncContext?.Post(ResetBindingsFor, this);
            //    }
            //}
        }

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

        internal EditGrants _editGrants = new EditGrants(false, false, false);
        private bool _allowNew = true;
        private bool _allowEdit = true;
        private bool _allowRemove = true;
        private bool _readOnly;

        [MyProperty, DefaultValue(false)]
        public bool ReadOnly { get => _readOnly; set => _readOnly = value; }

        public override bool IsReadOnly { get => _readOnly && base.IsReadOnly; }

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

        [MyProperty, DefaultValue(true)]
        public bool AllowInsert
        {
            get => _allowNew;
            set
            {
                _allowNew = value;
            }
        }

        [MyProperty, DefaultValue(true)]
        public bool AllowUdate
        {
            get => _allowEdit;
            set
            {
                _allowEdit = value;
            }
        }

        [MyProperty, DefaultValue(true)]
        public bool AllowDelete
        {
            get => _allowRemove;
            set
            {
                _allowRemove = value;
            }
        }

        private bool _allowSaveCancel = true;
        private object _saved;
        private bool _Opening;
        private IList<ProtoEntity> _inProcList = new List<ProtoEntity>();
        private object _lockInProcces = new object();
        internal bool InProcces(ProtoEntity bindable, bool add)
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

        [MyProperty, DefaultValue(true)]
        public bool AllowSaveCancel
        {
            get => _allowSaveCancel;
            set
            {
                _allowSaveCancel = value;
            }
        }

        [Browsable(false)]
        public override bool AllowEdit
        {
            get
            {
                return !IsReadOnly && _allowEdit && base.AllowEdit;
            }
        }

        [Browsable(false)]
        public override bool AllowRemove
        {
            get
            {
                return !IsReadOnly && _allowRemove && base.AllowRemove;
            }
        }


        #endregion

        #region Creatos 
        public CommonDataSource(IContainer container) : base(container)
        {
            //CurrencyManager.Bindings.CollectionChanged += Bindings_CollectionChanged;
        }

        public CommonDataSource()
        {
        }
        #endregion

        #region Events
        [MyEvents]
        public event EditGrantHandler GetEditGrants;
        [MyEvents]
        public event EventHandler AfterCancel;
        [MyEvents]
        public event EventHandler AfterPost;
        [MyEvents]
        public event EventHandler AfterDelete;
        [MyEvents]
        public event EventHandler BeforePost;
        [MyEvents]
        public event EventHandler AfterInsert;
        [MyEvents]
        public event EventHandler OnPost;
        [MyEvents]
        public event EventHandler OnDelete;
        [MyEvents]
        public event EventHandler BeforeOpen;
        [MyEvents]
        public event OpenHandler OnOpen;
        [MyEvents]
        public event EventHandler AfterOpen;
        [MyEvents]
        public event GetRepository GetRepository;
        [MyEvents]
        public event GetWhereParamsHandler GetWhereParams;
        [MyEvents]
        public event EventHandler OnEditBegin;
        [MyEvents]
        public event CanEditHandler OnCanEdit;
        [MyEvents]
        public event ActionWithoutParams OnRefresh;
        [MyEvents]
        public event ReadOnlyEventHandler CheckControlsReadOnly;
        [MyEvents]
        public event OnGetSqlString GetSqlString;
        [MyEvents]
        public event ValidateHandler ValidateControl;
        [MyEvents]
        public event YesNoTextHandler GetYesNoText;

        //[MyEvents]
        //public event NeedOpenHandler NeedOpenQuery;

        #endregion

        #region Methods

        protected string GetQueryText(SqlTypes sqlType, bool closed = false)
        {
            YesNoTextArgs e = new YesNoTextArgs(sqlType, Entity, closed);
            GetYesNoText?.Invoke(e);
            return e.ToString();
        }

        public void DisablePages(DisablePagesReason reason)
        {
            if (_disablePagesReasons.IndexOf(reason) == -1)
                _disablePagesReasons.Add(reason);
        }

        public void EnablePages(DisablePagesReason reason)
        {
            _disablePagesReasons.Remove(reason);
        }

        private void InitRepository()
        {
            if (_repository != null)
                return;

            GetRepository?.Invoke(out _repository);
            if (_repository != null)
                _repository.DataSource = this;
        }

        protected override void OnListChanged(ListChangedEventArgs e)
        {
            if (DataSource == null)
                return;

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
                            else
                            if (Entity.HasChanges)
                                State = DataState.Editing;
                            else
                                State = DataState.Browsing;
                            break;
                    }
                    break;
                default:
                    break;
            }

            base.OnListChanged(e);
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

        bool EditMode(DataState state)
        {
            switch (state)
            {
                case DataState.Inserting:
                case DataState.Editing:
                    return true;
                default:
                    return false;
            }
        }

        internal virtual void OnInit()
        {
            InitRepository();

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

        internal virtual void Ctrl_Enter(object sender, EventArgs e) { }
        internal virtual void Ctrl_Leave(object sender, EventArgs e) { }

        protected void GetGrants()
        {
            EditGrants e = new EditGrants(
                _allowNew && !IsReadOnly && State == DataState.Browsing,
                _allowEdit && !IsReadOnly && State == DataState.Browsing,
                _allowRemove && !IsReadOnly && State == DataState.Browsing
                );

            GetEditGrants?.Invoke(this, e);

            _editGrants = e;
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

            CancelEdit();
            ResetCurrentItem();
            State = DataState.Browsing;
            AfterCancel?.Invoke(this, EventArgs.Empty);
        }

        public void Delete()
        {
            if (!_editGrants.AllowRemove || Count == 0)
                return;


            if (AskForDelete && !UtilsGh.DlgYesNo(GetQueryText(SqlTypes.DeleteSql)))
                    return;

            State = DataState.Deleting;

            if (DeleteAsUpdate)
                State = DataState.Editing;

            if (OnDelete != null)
                OnDelete(this, EventArgs.Empty);
            else
                InternalDelete();
        }



        internal void DeleteFinish(object entity)
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
        }

        internal void InternalDelete()
        {
            if (Repository != null)
                Repository.Delete(Current);
        }

        //private void RegisterToOwner()
        //{
        //    if (_owner == null || IsDesignMode)
        //        return;

        //    (_owner as IFinalizeHolder)?.Register(this);
        //}

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

        internal void ReopenTimerStart()
        {
            if (IsDesignMode)
                return;

            if (_reopenTimer == null)
            {
                _reopenTimer = new Timer();
                _reopenTimer.Interval = 300;
                _reopenTimer.Tick += _reopenTimer_Tick;
            }
            else
                _reopenTimer.Stop();

            _reopenTimer.Start();
        }

        private void _reopenTimer_Tick(object sender, EventArgs e)
        {
            ReopenTimerStop();
            Open();
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ReopenTimerStop();
            }
            base.Dispose(disposing);
        }


        internal virtual void ReOpenByTimer()
        {
        }


        public void Close()
        {
            State = DataState.Inactive;
            if (Count > 0)
            {
                _saved = Entity;
                Clear();
            }

            ResetBindings(false);
        }

        public void CloseOpen()
        {
            Close();
            Open();
        }

        public async void Open()
        {
            //if (!QueryToOpen())
            //    return;

            OnBeforeOpen();
            try
            {
                IList lst = null;
                lst = await GetList(lst);
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

        //private bool QueryToOpen()
        //{
        //    bool need = true;
        //    NeedOpenQuery?.Invoke(out need);
        //    return need;
        //}

        private async Task<IList> GetList(IList lst)
        {
            if (OnOpen != null)
                OnOpen(out lst);
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
                    lst = await Repository.SelectAllAsync(GridSorting(), WhereParams());
            }

            return lst;
        }

        private Dictionary<string, object> WhereParams()
        {
            Dictionary<string, object> whereParams = new Dictionary<string, object>();

            GetWhereParams?.Invoke(this, whereParams);
            return whereParams.Count == 0 ? null : whereParams;
        }


        protected override void OnPositionChanged(EventArgs e)
        {
            base.OnPositionChanged(e);
            GetGrants();
            CheckBindingControlsReadOnly();
            if (_Opening)
                return;
            if (State == DataState.Browsing)
                _detailSources.ReOpenDetailsByTimer();

            //ReOpenDetailsByTimer();
        }

        protected override void OnDataSourceChanged(EventArgs e)
        {
            base.OnDataSourceChanged(e);
            if (DataSource == null)
                State = DataState.Inactive;
            else
                State = DataState.Browsing;
            GetGrants();
        }

        private void FocusGrid()
        {
            if (Grid != null && _needFocusGrid)
            {
                if (Grid.CanFocus)
                    Grid.Focus();
            }
        }

        internal virtual void CheckPages(DataState dataState) { }

        public void Post()
        {
            if (!InnerPost())
                return;

            if (View != null && View.IsEditorFocused)
                HideGridEditor();
        }

        internal void PostFinish(object entity)
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
            InProcces(entity as ProtoEntity, false);
            GetGrants();
        }


        private bool InnerPost()
        {

            if (InProcces(Entity, true))
                return false;

            if (OnPost == null && Repository == null)
            {
                InProcces(Entity, false);
                return false;
            }

            if (!ValidateBindingControls())
            {
                InProcces(Entity, false);
                return false;
            }

            if (Entity.HasChanges || State == DataState.Editing || State == DataState.Inserting)
            {
                BeforePost?.Invoke(this, EventArgs.Empty);
                if (OnPost != null)
                {
                    OnPost(this, EventArgs.Empty);
                    PostFinish(Entity);
                }
                else
                if (State == DataState.Inserting)
                    Repository.Save(Current);
                else
                    Repository.Update(Current);

            }
            else
                InProcces(Entity, false);

            return true;
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
                else
                if (Entity != null && Entity.HasChanges)
                    Entity.CancelEdit();
            }
            else if (SupportPages)
                CheckPages(DataState.Editing);
        }

        private bool CheckCanEdit()
        {
            bool canEdit = true;
            OnCanEdit?.Invoke(out canEdit);
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
                else
                if (_repository != null)
                {
                    _repository.Refresh(Entity);
                }
                RefreshFinish(Entity);
            }
        }

        internal void RefreshFinish(object entity)
        {
            InvokeIfRequired(()=>
            {
                ResetCurrentItem();
                CheckBindingControlsReadOnly();
            });
        }

        protected virtual void OnAfterOpen()
        {
            _detailSources.ReOpenDetailsByTimer();
            //ReOpenDetailsByTimer();
            CheckBindingControlsReadOnly();
            _Opening = false;
            AfterOpen?.Invoke(this, EventArgs.Empty);
        }

        //protected virtual void ReOpenDetailsByTimer()
        //{
        //}

        protected virtual void OnBeforeOpen()
        {
            _inProcList.Clear();
            BeforeOpen?.Invoke(this, EventArgs.Empty);
            _Opening = true;
        }

        public string GetSql(SqlTypes sqlType, ProtoEntity item)
        {
            string result = null;
            GetSqlString?.Invoke(sqlType, item, out result);
            return result;
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
                    DlgError("Заполните поле!!");
                    return false;
                }
            }
            return true;
        }

        private bool DoValidateControl(BaseEdit control)
        {
            if (ValidateControl == null)
                return true;
            ValidateEventArgs e = new ValidateEventArgs(control, true);
            ValidateControl.Invoke(this, e);
            return e.IsValid;
        }

        protected virtual void CloseOpenDoc() { }

        internal virtual void CloseOpenDocFinish(object entity) { }

        void IFinalInit.FinalInit()
        {
            OnInit();
        }

        #endregion
    }

    #region other
    public delegate void ValidateHandler(object sender, ValidateEventArgs e);
    public class ValidateEventArgs
    {
        public BaseEdit Control;
        public bool IsValid;

        public ValidateEventArgs(BaseEdit control, bool isValid)
        {
            Control = control;
            IsValid = isValid;
        }
    }


    public delegate void ReadOnlyEventHandler(object sender, ReadOnlyEventArgs e);
    public class ReadOnlyEventArgs
    {
        private BaseEdit _control;
        private bool _readOnly;

        public ReadOnlyEventArgs(BaseEdit control)
        {
            _control = control;
            _readOnly = false;
        }

        public BaseEdit Control { get => _control; }
        public bool ReadOnly { get => _readOnly; set => _readOnly = value; }
    }

   

    public enum DataState
    {
        Inactive,
        Browsing,
        BeginEditing,
        Inserting,
        Editing,
        Deleting,
        Canceling,
        Refreshing
    }

    public enum DisablePagesReason
    {
        Inserting,
        ClosingOrOpening,
        DetailProcessing
    }

    public class EditGrants
    {
        public EditGrants(bool allowNew, bool allowEdit, bool allowRemove)
        {
            AllowNew = allowNew;
            AllowEdit = allowEdit;
            AllowRemove = allowRemove;
        }

        public bool AllowNew;
        public bool AllowEdit;
        public bool AllowRemove;
    }

    public enum EditTypes
    {
        [Display(Name = "Добавить", Description = "Добавить новую запись")]
        Insert,
        [Display(Name = "Изменить", Description = "Изменить запись")]
        Edit,
        [Display(Name = "Удалить", Description = "Удалить запись")]
        Delete,
        [Display(Name = "Сохранить", Description = "Сохранить изменения")]
        Save,
        [Display(Name = "Отменить", Description = "Отменить изменения")]
        Cancel,
        [Display(Name = "Обновить всё", Description = "Обновить все записи")]
        RefreshAll,
        [Display(Name = "Вывести для печати", Description = "Вывести для печати")]
        PrintPreview,
        [Display(Name = "Найти на Discogs", Description = "Найти на Discogs по баркоду")]
        SearchDiscogs,
        [Display(Name = "Найти на Wikipedia", Description = "Найти на Wikipedia артиста")]
        SearchWikipedia,
        [Display(Name = "Найти на Allmusic", Description = "Найти на Allmusic артиста")]
        SearchAllMusic,
        [Display(Name = "Найти на Google", Description = "Найти на Google артиста")]
        SearchGoogle,
    }

    public delegate void GetWhereParamsHandler(object sender, IDictionary whereParams);
    public delegate void ActionWithoutParams();
    public delegate void OnAction(out ProtoEntity item);

    public delegate void EditGrantHandler(object sender, EditGrants e);
    public delegate void OpenHandler(out IList list);
    public delegate void CanEditHandler(out bool allowEdit);
    public delegate void NeedOpenHandler(out bool needOpen);

    public class YesNoTextArgs
    {
        protected readonly ProtoEntity Entity;
        public string QueryText;
        public readonly bool Closed;

        public YesNoTextArgs(SqlTypes sqlType, ProtoEntity entity, bool closed)
        {
            Entity = entity;
            Closed = closed;
            switch (sqlType)
            {
                case SqlTypes.CloseDocSql:
                    if (closed)
                        QueryText = "Желаете открыть?";
                    else
                        QueryText = "Желаете закрыть?";
                    break;
                case SqlTypes.DeleteSql:
                    if (closed)
                        QueryText = "Удалить документ?";
                    else
                        QueryText = "Удалить запись?";
                    break;
                default:
                    break;
            }
        }

        public override string ToString()
        {
            return Entity.ToString() + "\r\n\r\n" + QueryText;
        }
    }

    public delegate void YesNoTextHandler(YesNoTextArgs e);

    #endregion

}
