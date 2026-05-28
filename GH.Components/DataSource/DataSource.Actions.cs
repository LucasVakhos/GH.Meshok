using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraNavBar;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
namespace GH.Components
{
    public partial class DataSource
    {
        private IList<NavBarItem> _navbaritems = null;
        private bool _supportDataActions = true;
        [GH.ComponentsProperty, DefaultValue(true), Description("Ставьте (SupportDataActions=false) если не нужно поддерживать ActionList")]
        public bool SupportDataActions { get => _supportDataActions; set => _supportDataActions = value; }
        private bool _supportPopupMenu = true;
        private bool? _cansearch;
        [GH.ComponentsProperty, DefaultValue(true), Description("Ставьте (SupportPopupMenu=false) если не нужно поддерживать ActionList")]
        public bool SupportPopupMenu { get => _supportPopupMenu; set => _supportPopupMenu = value; }
        [GH.ComponentsProperty, DefaultValue(false)]
        public bool CreateAdditionalActions { get; set; } = false;
        private bool InitActions()
        {
            if (_actionList == null)
                return false;
            if (!_supportDataActions)
                return true;
            int cnt = 0;
            foreach (EditTypes editType in Enum.GetValues(typeof(EditTypes)))
            {
                switch (editType)
                {
                    case EditTypes.Insert:
                        if (ReadOnly || !AllowInsert)
                            continue;
                        break;
                    case EditTypes.Edit:
                        if (ReadOnly || !AllowUdate)
                            continue;
                        break;
                    case EditTypes.Delete:
                        if (ReadOnly || !AllowDelete)
                            continue;
                        break;
                    case EditTypes.Save:
                        if (ReadOnly || !AllowSaveCancel)
                            continue;
                        break;
                    case EditTypes.Cancel:
                        if (ReadOnly || !AllowSaveCancel)
                            continue;
                        break;
                    case EditTypes.RefreshAll:
                    case EditTypes.Preview:
                        if (Grid == null)
                            continue;
                        break;
                    case EditTypes.Additional:
                        if (OnAdditionalActionCreate == null || Grid == null)
                            continue;
                        break;
                    default:
                        continue;
                }
                cnt = CreateAction(editType, cnt);
            }
            return /*cnt > 0 || */_actionList.Actions.Count > 0;
        }
        private int CreateAction(EditTypes editType, int cnt)
        {
            if (editType == EditTypes.Additional && !CreateAdditionalActions)
                return cnt;
        from_begin:
            ActionDataGh action = new ActionDataGh(editType);
            var text = GetActionTexts(editType);
            action.Caption = text.Caption;
            action.ToolTipText = text.ToolTip;
            action.Execute += Action_Execute;
            action.Update += Action_Update;
            action.Enabled = false;
            _actionList.Actions.Insert(cnt, action);
            cnt++;
            switch (editType)
            {
                case EditTypes.Insert:
                    action.ShortcutKeys = Keys.Control | Keys.Insert;
                    break;
                case EditTypes.Edit:
                    action.ShortcutKeys = Keys.Control | Keys.F2;
                    break;
                case EditTypes.Delete:
                    action.ShortcutKeys = Keys.Control | Keys.Delete;
                    break;
                case EditTypes.Save:
                    action.ShortcutKeys = Keys.Control | Keys.Return;
                    if (_buttonsPanel != null)
                        _actionList.SetAction(_buttonsPanel.btnOK, action);
                    break;
                case EditTypes.Cancel:
                    if (_buttonsPanel != null)
                        _actionList.SetAction(_buttonsPanel.btnCancel, action);
                    break;
                case EditTypes.RefreshAll:
                    action.ShortcutKeys = Keys.F5;
                    break;
                case EditTypes.Preview:
                    action.ShortcutKeys = Keys.Control | Keys.P;
                    break;
                case EditTypes.Additional:
                    break;
                default:
                    break;
            }
            if (editType == EditTypes.Additional)
            {
                ActionCreateParams e = new ActionCreateParams(action);
                CreateAdditional(e);
                if (!e.Save)
                {
                    _actionList.Actions.Remove(action);
                    cnt--;
                }
                else
                    if (e.CreateNext)
                        goto from_begin;
            }
            return cnt;
        }
        private void CreateAdditional(ActionCreateParams e)
        {
            OnAdditionalActionCreate?.Invoke(e);
        }
        private void Action_Update(object sender, EventArgs e)
        {
            if (sender is ActionDataGh action)
            {
                if (action.ButtonType == EditTypes.Additional && !CreateAdditionalActions)
                    return;
                if (State == DataState.Inactive)
                {
                    action.Enabled = false;
                    return;
                }
                bool enabled = State == DataState.Browsing && PageSupport.CheckSupportrd(action.ButtonType);
                switch (action.ButtonType)
                {
                    case EditTypes.Insert:
                        enabled = enabled && _editGrants.AllowNew;
                        break;
                    case EditTypes.Edit:
                    case EditTypes.Delete:
                        enabled = enabled && _editGrants.AllowRemove && Count > 0;
                        break;
                    case EditTypes.Save:
                        CaptionItem text = GetActionTexts(action.ButtonType);
                        Image image = ActtionsImages.Instance.SmallImages.Images[(int)action.ButtonType];
                        Image largeImage = ActtionsImages.Instance.LargeImages.Images[(int)action.ButtonType];
                        enabled = EditMode && PageSupport.CheckSupportrd(action.ButtonType);
                        if (!EditMode && Closable && DocCnt > 0)
                        {
                            if (Closed)
                            {
                                text.Caption = "Открыть";
                                text.ToolTip = "Открыть этот документ";
                                image = ActtionsImages.Instance.SmallImages.Images[(int)EditTypes.OpenDocument];
                                largeImage = ActtionsImages.Instance.LargeImages.Images[(int)EditTypes.OpenDocument];
                            }
                            else
                            {
                                text.Caption = "Закрыть";
                                text.ToolTip = "Закрыть этот документ";
                                image = ActtionsImages.Instance.SmallImages.Images[(int)EditTypes.CloseDocument];
                                largeImage = ActtionsImages.Instance.LargeImages.Images[(int)EditTypes.CloseDocument];
                            }
                            CloseActionUpdateArgs closeActionUpdateArgs = new CloseActionUpdateArgs(Closed, CanExecuteSaveAction(), Entity, text.Caption, text.ToolTip);
                            if (closeActionUpdateArgs.Enabled)
                                CloseActionUpdate?.Invoke(closeActionUpdateArgs);
                            text.Caption = closeActionUpdateArgs.Caption;
                            text.ToolTip = closeActionUpdateArgs.ToolTip;
                            enabled = closeActionUpdateArgs.Enabled;
                        }
                        action.Image = image;
                        action.LargeImage = largeImage;
                        action.Caption = text.Caption;
                        action.ToolTipText = text.ToolTip;
                        action.Enabled = enabled;
                        return;
                    case EditTypes.Cancel:
                        enabled = EditMode && PageSupport.CheckSupportrd(action.ButtonType);
                        break;
                    case EditTypes.RefreshAll:
                        enabled = PageSupport.CheckSupportrd(action.ButtonType);
                        break;
                    case EditTypes.Preview:
                        enabled = enabled && Count > 0 && Grid != null;
                        break;
                    case EditTypes.Additional:
                        //enabled = true;
                        UpdateAdditionalAction(action);
                        return;
                    default:
                        break;
                }
                ActionUpdateArgs actionUpdateArgs = new ActionUpdateArgs(action.ButtonType, enabled, Entity, action.Caption, action.ToolTipText);
                OnUpdateInnerAction?.Invoke(actionUpdateArgs);
                action.Caption = actionUpdateArgs.Caption;
                action.ToolTipText = actionUpdateArgs.ToolTip;
                action.Enabled = actionUpdateArgs.Enabled;
            }
        }
        private void UpdateAdditionalAction(ActionDataGh action)
        {
            OnAdditionalActionUpdate?.Invoke(action, EventArgs.Empty);
        }
        private void Action_Execute(object sender, EventArgs e)
        {
            if (sender is ActionDataGh action)
            {
                if (action.ButtonType == EditTypes.Additional && !CreateAdditionalActions)
                    return;
                switch (action.ButtonType)
                {
                    case EditTypes.Insert:
                        Insert();
                        break;
                    case EditTypes.Edit:
                        Edit();
                        break;
                    case EditTypes.Delete:
                        Delete();
                        break;
                    case EditTypes.Save:
                        ExecuteSaveAction(action);
                        break;
                    case EditTypes.Cancel:
                        Cancel();
                        break;
                    case EditTypes.RefreshAll:
                        RefreshAll();
                        break;
                    case EditTypes.Preview:
                        ExportData();
                        break;
                    case EditTypes.Additional:
                        ExecuteAdditionalAction(action);
                        break;
                    default:
                        break;
                }
            }
        }
        private void ExecuteAdditionalAction(ActionDataGh action)
        {
            OnAdditionalActionExecute?.Invoke(action, EventArgs.Empty);
        }
        private void InitRibbonEditGroup()
        {
            if (ActionList == null)
                return;
            if (Owner is IRibbonControlFrame ribbonFrame)
            {
                RibbonControl ribbonControl = ribbonFrame.RibbonControl;
                if (ribbonControl == null)
                {
                    ribbonControl = new RibbonControl();
                    ribbonFrame.RibbonControl = ribbonControl;
                }
                ribbonControl.Items.Add(ribbonControl.ExpandCollapseItem);
                if (ribbonFrame.RibbonStatusBar == null)
                    ribbonFrame.RibbonStatusBar = new RibbonStatusBar();
                if (ribbonFrame.EditPage == null)
                    ribbonFrame.EditPage = new RibbonPage();
                RibbonPage editPage = ribbonFrame.EditPage;
                foreach (var categoy in ActionList.Actions.GroupBy(u => u.Category).Select(u => u.Key).ToList())
                {
                    List<ActionGh> actions = ActionList.Actions.Where(x => x.Category == categoy).ToList();
                    if (actions.Count > 0)
                        ActionsCreateHelper.CreateRibbonPageGroup(categoy, actions, editPage, ribbonFrame.RibbonControl.Manager);
                }
                ;
                ribbonControl.EndInit();
            }
        }
        private void InitStripMenu()
        {
            if (!SupportPopupMenu || ActionList == null)
                return;
            ContextMenuStrip menuStrip = Grid?.ContextMenuStrip;
            if (menuStrip == null)
            {
                menuStrip = new ContextMenuStrip();
                if (Grid == null)
                    Owner.ContextMenuStrip = menuStrip;
                else
                    Grid.ContextMenuStrip = menuStrip;
            }
            else
                menuStrip.Items.Clear();
            foreach (var categoy in _actionList.Actions.GroupBy(u => u.Category).Select(u => u.Key).ToList())
            {
                var actions = _actionList.Actions.Where(x => x.Category == categoy).ToList();
                if (actions.Count > 0)
                    ActionsCreateHelper.CreateMenuGroup(categoy, actions, menuStrip);
            }
        }
        private void InitNavBars()
        {
            if (ActionList == null)
                return;
            if (Owner is INavBarGroupFrame navBarGroupFrame)
            {
                Owner.LostFocus += Ctrl_Leave;
                Owner.GotFocus += Ctrl_Enter;
                foreach (var categoy in ActionList.Actions.GroupBy(u => u.Category).Select(u => u.Key).ToList())
                {
                    var actions = ActionList.Actions.Where(x => x.Category == categoy).ToList();
                    if (actions.Count > 0)
                        ActionsCreateHelper.CreateNavBarItemGroup(categoy, actions, _navbaritems);
                }
            }
        }
        internal void Ctrl_Leave(object sender, EventArgs e)
        {
            if (ActionList != null)
                ActionList.ListenKeyDown = false;
            if (Owner.ContainsFocus && Owner is INavBarGroupFrame frame)
            {
                if (frame.Group.ItemLinks.Count > 0)
                    frame.Group.ItemLinks.Clear();
            }
        }
        internal void Ctrl_Enter(object sender, EventArgs e)
        {
            if (ActionList != null)
            {
                if (_navbaritems.Count > 0 && Owner is INavBarGroupFrame frame && frame.Group != null)
                {
                    foreach (NavBarItemLink item in frame.Group.ItemLinks)
                        if (_navbaritems.Contains(item.Item))
                            return;
                    if (frame.Group.ItemLinks.Count > 0)
                        frame.Group.ItemLinks.Clear();
                    foreach (NavBarItem barItem in _navbaritems)
                        frame.Group.ItemLinks.Add(barItem);
                }
                ActionList.ListenKeyDown = true;
            }
        }
        private CaptionItem GetActionTexts(EditTypes action)
        {
            var displayAtt = action.GetAttribute<DisplayAttribute>();
            CaptionItem captionItem = new CaptionItem(action, displayAtt.Name, displayAtt.Description);
            GetActionCaption?.Invoke(captionItem);
            return captionItem;
        }
        private bool CanSearch()
        {
            if (_cansearch == null)
            {
                FieldInfo fi = typeof(BindingSource).GetField("itemType", BindingFlags.NonPublic | BindingFlags.Instance);
                Type title = fi.GetValue(this) as Type;
                title = title.GetInterface("ITitle");
                _cansearch = Grid != null && title != null;
            }
            return (bool)_cansearch;
        }
        [GH.ComponentsEvents]
        public event CreateAdditional OnAdditionalActionCreate;
        [GH.ComponentsEvents]
        public event EventHandler OnAdditionalActionExecute;
        [GH.ComponentsEvents]
        public event EventHandler OnAdditionalActionUpdate;
    }
}
