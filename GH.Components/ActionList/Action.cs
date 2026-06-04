// Decompiled with JetBrains decompiler
// Type: GH.Components.Windows.Forms.Actions.Action
// Assembly: GH.Components.Windows.Forms.Actions, Version=1.3.0.0, Culture=neutral, PublicKeyToken=81de48f2c6979a5b
// MVID: A3CB4EFA-A290-4B96-A454-5604A140D416
// Assembly location: E:\C#\KULAKOVSKY\AmigoPlus\AmigoPlus\bin\Debug\lib\GH.Components.Windows.Forms.Actions.dll
using DevExpress.XtraBars;
using DevExpress.XtraNavBar;
using System.ComponentModel;
using System.Drawing.Design;
using System.Reflection;
//using static System.Windows.Forms.ImageList;
namespace GH.Components
{
    [StandardAction]
    [DefaultEvent("Execute")]
    [ToolboxBitmap(typeof(ActionGh), "Images.Action.bmp")]
    public class ActionGh : Component
    {
        private ActionGh.ActionWorkingState workingState;
        private List<Component> targets;
        private ActionList actionList;
        private string _category;
        private string _caption;
        private CheckState _checkState;
        private bool _enabled;
        private bool _checkOnClick;
        private Keys _shortcutKeys = Keys.None;
        private bool _visible;
        private string _toolTipText;
        private bool _interceptingCheckStateChanged;
        private EventHandler _clickEventHandler;
        private EventHandler _checkStateChangedEventHandler;
        protected internal ActionList ActionList
        {
            get
            {
                return actionList;
            }
            set
            {
                if (actionList == value)
                    return;
                actionList = value;
            }
        }

        public object Tag { get; set; }
        [DefaultValue("")]
        [UpdatableProperty]
        [Localizable(true)]
        public string Caption
        {
            get
            {
                return _caption;
            }
            set
            {
                if (!(_caption != value))
                    return;
                _caption = value;
                if (targets.Count > 0)
                    UpdateAllTargets(nameof(Caption), (object)value);
            }
        }
        [DefaultValue("none")]
        [Localizable(true)]
        public string Category
        {
            get
            {
                if (string.IsNullOrEmpty(_category))
                    return "none";
                return _category;
            }
            set => _category = value;
        }
        [DefaultValue(false)]
        public bool Checked
        {
            get
            {
                return _checkState != CheckState.Unchecked;
            }
            set
            {
                if (value == Checked)
                    return;
                CheckState = value ? CheckState.Checked : CheckState.Unchecked;
            }
        }
        [UpdatableProperty]
        [DefaultValue(CheckState.Unchecked)]
        public CheckState CheckState
        {
            get
            {
                return _checkState;
            }
            set
            {
                if (_checkState == value)
                    return;
                _checkState = value;
                if (targets.Count > 0)
                    UpdateAllTargets(nameof(CheckState), (object)value);
            }
        }
        [UpdatableProperty]
        [DefaultValue(true)]
        public bool Enabled
        {
            get
            {
                if (ActionList == null)
                    return _enabled;
                if (_enabled)
                    return ActionList.Enabled;
                return false;
            }
            set
            {
                if (_enabled == value)
                    return;
                _enabled = value;
                if (targets.Count > 0)
                    UpdateAllTargets(nameof(Enabled), (object)value);
            }
        }

        private Image _image;
        [DefaultValue(null)]
        [UpdatableProperty]
        [Editor("DevExpress.Utils.Design.DXImageEditor, DevExpress.Design.v17.2", typeof(UITypeEditor))]
        public virtual Image Image
        {
            get
            {
                return _image;
            }
            set
            {
                if (_image == value)
                    return;
                _image = value;
                UpdateAllTargets(nameof(Image), value);
            }
        }

        private Image _largeImage;
        [DefaultValue(null)]
        [UpdatableProperty]
        [Editor("DevExpress.Utils.Design.DXImageEditor, DevExpress.Design.v17.2", typeof(UITypeEditor))]
        public virtual Image LargeImage
        {
            get
            {
                return _largeImage;
            }
            set
            {
                if (_largeImage == value)
                    return;
                _largeImage = value;
                UpdateAllTargets(nameof(LargeImage), value);
            }
        }
        [UpdatableProperty]
        [DefaultValue(false)]
        public bool CheckOnClick
        {
            get
            {
                return _checkOnClick;
            }
            set
            {
                if (_checkOnClick == value)
                    return;
                _checkOnClick = value;
                if (targets.Count > 0)
                    UpdateAllTargets(nameof(CheckOnClick), (object)value);
            }
        }
        [Localizable(true)]
        [DefaultValue(Keys.None)]
        [UpdatableProperty]
        public Keys ShortcutKeys
        {
            get
            {
                return _shortcutKeys;
            }
            set
            {
                if (_shortcutKeys == value)
                    return;
                _shortcutKeys = value;
                if (targets.Count > 0)
                    UpdateAllTargets("ShortcutKeyDisplayString", (object)(string)new KeysConverter().ConvertTo((object)value, typeof(string)));
            }
        }
        [DefaultValue(true)]
        [UpdatableProperty]
        public bool Visible
        {
            get
            {
                return _visible;
            }
            set
            {
                if (_visible == value)
                    return;
                _visible = value;
                if (targets.Count > 0)
                    UpdateAllTargets(nameof(Visible), (object)value);
            }
        }
        [Editor("System.ComponentModel.Design.MultilineStringEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        [DefaultValue("")]
        [UpdatableProperty]
        [Localizable(true)]
        public string ToolTipText
        {
            get
            {
                return _toolTipText;
            }
            set
            {
                if (!(_toolTipText != value))
                    return;
                _toolTipText = value;
                if (targets.Count > 0)
                    UpdateAllTargets(nameof(ToolTipText), (object)value);
            }
        }

        public ActionGh()
        {
            targets = new List<Component>();
            _enabled = true;
            _caption = GetType().Name;
            _category = string.Empty;
            WorkingState = ActionGh.ActionWorkingState.Listening;
            _shortcutKeys = Keys.None;
            _toolTipText = string.Empty;
            _visible = true;
            _clickEventHandler = target_Click;
            _checkStateChangedEventHandler = target_CheckStateChanged;
        }

        protected ActionGh.ActionWorkingState WorkingState
        {
            get
            {
                return workingState;
            }
            set
            {
                workingState = value;
            }
        }

        public event CancelEventHandler BeforeExecute;
        protected virtual void OnBeforeExecute(CancelEventArgs e)
        {
            if (BeforeExecute == null)
                return;
            BeforeExecute((object)this, e);
        }

        public event EventHandler Execute;
        protected virtual void OnExecute(EventArgs e)
        {
            if (ActionList != null)
            {
                Control extendee = ActionList.GetExtendee(this) as Control;
                ActionList.Owner.SelectNextControl(extendee, true, true, false, true);
            }
            if (Execute == null)
                return;
            Execute((object)this, e);
        }

        public event EventHandler AfterExecute;
        protected virtual void OnAfterExecute(EventArgs e)
        {
            if (AfterExecute == null)
                return;
            AfterExecute((object)this, e);
        }

        public event EventHandler Update;
        protected virtual void OnUpdate(EventArgs e)
        {
            if (Update == null)
                return;
            Update((object)this, e);
        }
        public void DoUpdate()
        {
            OnUpdate(EventArgs.Empty);
        }
        internal void InternalRemoveTarget(Component extendee)
        {
            targets.Remove(extendee);
            RemoveHandler(extendee);
            OnRemovingTarget(extendee);
        }
        internal void InternalAddTarget(Component extendee)
        {
            targets.Add(extendee);
            refreshState(extendee);
            AddHandler(extendee);
            OnAddingTarget(extendee);
        }
        protected virtual void OnRemovingTarget(Component extendee)
        {
        }
        protected virtual void OnAddingTarget(Component extendee)
        {
        }
        internal void RefreshEnabledCheckState()
        {
            UpdateAllTargets("Enabled", (object)Enabled);
            UpdateAllTargets("CheckState", (object)CheckState);
        }
        protected void UpdateAllTargets(string propertyName, object value)
        {
            foreach (Component target in targets)
            {
                updateProperty(target, propertyName, value);
            }
        }
        private void updateProperty(Component target, string propertyName, object value)
        {
            WorkingState = ActionGh.ActionWorkingState.Driving;
            try
            {
                if (ActionList == null || SpecialUpdateProperty(target, propertyName, value))
                    return;
                ActionList.TypesDescription[target.GetType()].SetValue(propertyName, (object)target, value);
            }
            finally
            {
                WorkingState = ActionGh.ActionWorkingState.Listening;
            }
        }
        protected virtual bool SpecialUpdateProperty(Component target, string propertyName, object value)
        {
            if (!(propertyName == "ToolTipText"))
                return false;
            if (target is Control control && ActionList.ToolTip.CanExtend((object)control))
            {
                ActionList.ToolTip.SetToolTip(control, (string)value);
                return true;
            }
            if (target is ToolStripItem toolStripItem)
            {
                toolStripItem.ToolTipText = (string)value;
                toolStripItem.AutoToolTip = string.IsNullOrEmpty(toolStripItem.ToolTipText);
                return true;
            }
            return false;
        }
        private void refreshState(Component target)
        {
            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties((object)this, new Attribute[1]
                {
                    (Attribute) new UpdatablePropertyAttribute()
                }))
                updateProperty(target, property.Name, property.GetValue((object)this));
        }
        private void BarItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            throw new NotImplementedException();
        }
        protected virtual void AddHandler(Component extendee)
        {
            EventInfo eventInfo1 = extendee.GetType().GetEvent("Click");
            if (eventInfo1 != (EventInfo)null)
                eventInfo1.AddEventHandler((object)extendee, (Delegate)_clickEventHandler);
            EventInfo eventInfo2 = extendee.GetType().GetEvent("CheckStateChanged");
            if (eventInfo2 != (EventInfo)null)
                eventInfo2.AddEventHandler((object)extendee, (Delegate)_checkStateChangedEventHandler);
            if (extendee is ToolBarButton toolBarButton)
                toolBarButton.Parent.ButtonClick += toolbar_ButtonClick;
            if (extendee is NavBarItem navBarItem)
                navBarItem.LinkClicked += navBarItem_LinkClicked;
            if (extendee is BarItem barItem)
                barItem.ItemClick += BarItem_ItemClick1;
        }
        protected virtual void RemoveHandler(Component extendee)
        {
            EventInfo eventInfo1 = extendee.GetType().GetEvent("Click");
            if (eventInfo1 != (EventInfo)null)
                eventInfo1.RemoveEventHandler((object)extendee, (Delegate)_clickEventHandler);
            EventInfo eventInfo2 = extendee.GetType().GetEvent("CheckStateChanged");
            if (eventInfo2 != (EventInfo)null)
                eventInfo2.RemoveEventHandler((object)extendee, (Delegate)_checkStateChangedEventHandler);
            if (extendee is ToolBarButton toolBarButton)
                toolBarButton.Parent.ButtonClick -= toolbar_ButtonClick;
            if (extendee is NavBarItem navBarItem)
                navBarItem.LinkClicked -= navBarItem_LinkClicked;
            if (extendee is BarItem barItem)
                barItem.ItemClick -= BarItem_ItemClick1;
        }
        private void navBarItem_LinkClicked(object sender, NavBarLinkEventArgs e)
        {
            if (!targets.Contains((Component)e.Link.Item))
                return;
            handleClick((object)e.Link.Item, (EventArgs)e);
        }
        private void BarItem_ItemClick1(object sender, ItemClickEventArgs e)
        {
            if (!targets.Contains((Component)e.Item))
                return;
            handleClick((object)e.Item, (EventArgs)e);
        }
        private void toolbar_ButtonClick(object sender, ToolStripItemClickedEventArgs e)
        {
            if (!targets.Contains((Component)e.Button)) return;
            handleClick((object)e.Button, (EventArgs)e);
        }
        private void target_Click(object sender, EventArgs e)
        {
            handleClick(sender, e);
        }
        private void handleClick(object sender, EventArgs e)
        {
            if (WorkingState != ActionGh.ActionWorkingState.Listening)
                return;
            DoExecute();
        }
        private void target_CheckStateChanged(object sender, EventArgs e)
        {
            handleCheckStateChanged(sender, e);
        }

        internal bool InterceptingCheckStateChanged
        {
            get
            {
                return _interceptingCheckStateChanged;
            }
            set
            {
                _interceptingCheckStateChanged = value;
            }
        }
        private void handleCheckStateChanged(object sender, EventArgs e)
        {
            if (WorkingState != ActionGh.ActionWorkingState.Listening)
                return;
            CheckState = (CheckState)ActionList.TypesDescription[sender.GetType()].GetValue("CheckState", sender);
        }
        public void DoExecute()
        {
            if (!Enabled)
                return;
            CancelEventArgs e = new CancelEventArgs();
            OnBeforeExecute(e);
            if (e.Cancel)
                return;
            OnExecute(EventArgs.Empty);
            OnAfterExecute(EventArgs.Empty);
        }
        internal void ExecuteShortcut()
        {
            if (!Enabled)
                return;
            if (CheckOnClick)
                Checked = !Checked;
            DoExecute();
        }

        protected enum ActionWorkingState
        {
            Listening,
            Driving,
        }
    }
}
