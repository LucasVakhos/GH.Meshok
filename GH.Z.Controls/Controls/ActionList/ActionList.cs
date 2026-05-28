using ActionMessageHook;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraNavBar;
using GH.AppContext;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace GH.Components
{
    [ToolboxItemFilter("System.Windows.Forms")]
    [ProvideProperty("Action", typeof(Component))]
    [ToolboxBitmap(typeof(ActionList), "Images.ActionList.bmp")]
    [DefaultProperty("Actions")]
    public class ActionList : Component, IExtenderProvider, ISupportInitialize
    {
        #region поля и свойства
        public event EventHandler Update;

        private bool _enabled;
        private ActionCollection _actions;
        private Dictionary<Component, ActionGh> _targets;
        private Dictionary<Type, ActionTargetDescriptionInfo> _typesDescription;
        private bool _initializing;
        private ContainerControl _owner;
        private bool _listenKeyDown = false;
        //private bool _attached = false;

        private ToolTip _tooltip;
        public ToolTip ToolTip
        {
            get
            {
                return _tooltip;
            }
        }

        [DefaultValue(true)]
        public bool Enabled
        {
            get
            {
                return _enabled;
            }
            set
            {
                if (_enabled == value)
                    return;
                _enabled = value;
                refreshActions();
            }
        }

        [Browsable(false)]
        public bool Active
        {
            get
            {
                if (_owner == null)
                    return false;

                return _owner.CanFocus;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ActionCollection Actions
        {
            get
            {
                return _actions;
            }
        }
        #endregion


        public ActionList()
        {
            _actions = new ActionCollection(this);
            _targets = new Dictionary<Component, ActionGh>();
            _typesDescription = new Dictionary<Type, ActionTargetDescriptionInfo>();
            _enabled = true;
            _tooltip = new ToolTip();
            if (this.IsDesignMode())
                return;

        }

        #region методы
        public void BeginInit()
        {
            _initializing = true;
        }

        public void EndInit()
        {
            _initializing = false;
            MessageHook.RegForQueue(this);
            checkInternalCollections();
            refreshActions();
        }

        private void OnUpdate(EventArgs e)
        {
            if (this.Update != null)
                this.Update((object)this, e);
            foreach (var action in Actions)
                action.DoUpdate();
        }

        private void ActionList_Disposed(object sender, EventArgs e)
        {
            MessageHook.UnRegQueue(this);
            //RunContext.UnRegQueue(this);
        }
        public void DoUpdate()
        {
            DoUpdate(EventArgs.Empty);
        }

        internal virtual void DoUpdate(EventArgs eventArgs)
        {
            if (Update != null)
                Update(this, eventArgs);

            if (Enabled)
            {
                foreach (ActionGh action in _actions)
                    action.DoUpdate();
            }
        }
        #endregion


        public ActionGh GetAction(Component extendee)
        {
            if (_targets.ContainsKey(extendee))
                return _targets[extendee];
            return null;
        }
        public Component GetExtendee(ActionGh action)
        {
            return _targets.Where(x => x.Value == action).FirstOrDefault().Key;            
        }


        public void SetAction(Component extendee, ActionGh action)
        {
            if (!_initializing)
            {
                if (extendee == null)
                    throw new ArgumentNullException(nameof(extendee));
                if (action != null && action.ActionList != this)
                    throw new ArgumentException("The Action you selected is owned by another ActionList");
            }
            if (_targets.ContainsKey(extendee))
            {
                _targets[extendee].InternalRemoveTarget(extendee);
                _targets.Remove(extendee);
            }
            if (action == null)
                return;
            if (!_typesDescription.ContainsKey(extendee.GetType()))
                _typesDescription.Add(extendee.GetType(), new ActionTargetDescriptionInfo(extendee.GetType()));
            _targets.Add(extendee, action);
            action.InternalAddTarget(extendee);
        }

        bool IExtenderProvider.CanExtend(object extendee)
        {
            Type type = extendee.GetType();
            foreach (Type supportedType in GetSupportedTypes())
            {
                if (supportedType.IsAssignableFrom(type))
                    return true;
            }
            return false;
        }

        internal bool OwnerKillFocus()
        {
            throw new NotImplementedException();
        }

        internal bool OwnerSetFocus()
        {
            throw new NotImplementedException();
        }

        public virtual Type[] GetSupportedTypes()
        {
            List<Type> lst = new List<Type>()
            {
                typeof (ButtonBase),
                typeof (BaseButton),
                typeof (ToolStripMenuItem),
                typeof (BarButtonItem),
                typeof (BarSubItem),
                typeof (NavBarItem)
            };

            return lst.ToArray();
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public Dictionary<Type, ActionTargetDescriptionInfo> TypesDescription
        {
            get
            {
                return _typesDescription;
            }
        }

        private void refreshActions()
        {
            if (this.IsDesignMode())
                return;
            foreach (ActionGh action in _actions)
                action.RefreshEnabledCheckState();
        }

        private void checkInternalCollections()
        {
            foreach (ActionGh action in _targets.Values)
            {
                if (!Actions.Contains(action) || action.ActionList != this)
                    throw new InvalidOperationException("Action owned by another action list or invalid Action.ActionList");
            }
        }

        public ContainerControl Owner
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

        [Browsable(false)]
        public Control ActiveControl
        {
            get
            {
                return getActiveControl(Owner);
            }
        }

        private Control getActiveControl(ContainerControl containerControl)
        {
            if (containerControl == null)
                return null;
            if (containerControl.ActiveControl is ContainerControl)
                return getActiveControl((ContainerControl)containerControl.ActiveControl);
            return containerControl.ActiveControl;
        }

        public void CheckShortcuts(object sender, KeyEventArgs e)
        {
            if (!ListenKeyDown)
                return;

            foreach (ActionGh action in _actions.Where(x=> x.ShortcutKeys == e.KeyData))
            {
                action.ExecuteShortcut();
            }
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
                if (!(rootComponent is ContainerControl))
                    return;
                Owner = (ContainerControl)rootComponent;
            }
        }

        [Browsable(false), DefaultValue(false)]
        public bool ListenKeyDown { get => _listenKeyDown; set => _listenKeyDown = value; }
    }

}

