using DevExpress.Utils;
using DevExpress.XtraDataLayout;
using DevExpress.XtraEditors;
using DevExpress.XtraLayout;
using DevExpress.XtraLayout.Utils;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Reflection;
namespace GH.Components
{
    public class LayoutControlGh : DataLayoutControl, ISavedControl
    {
        private bool _saveLayout = false;
        [GH.ComponentsProperty, DefaultValue(false)]
        public bool SaveLayout { get => _saveLayout; set => _saveLayout = value; }
        [Browsable(false), GH.ComponentsProperty, DefaultValue(false)]
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
        private ContainerControl _owner;
        [GH.ComponentsProperty]
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
        public LayoutControlGh()
        {
            //Dock = DockStyle.Fill;
            OptionsFocus.EnableAutoTabOrder = true;
        }
        public override void BeginInit()
        {
        }
        public override void EndInit()
        {
            if (!Created)
            {
                AllowCustomization = false;
                OptionsFocus.AllowFocusGroups = false;
                OptionsFocus.AllowFocusReadonlyEditors = false;
                OptionsFocus.AllowFocusTabbedGroups = false;
                OptionsItemText.AlignControlsWithHiddenText = OptionsItemText.AlignControlsWithHiddenText;
                if (DesignMode || Owner == null || DataSource == null || !LastLevel() || !(DataSource is DataSource))
                    return;
                InitLayoutControlItems();
                //Owner.Size = SizeHeper.NewSize(this);
                //Location = Point.Empty;
            }
            base.EndInit();
        }
        private bool LastLevel()
        {
            if (DataSource is DataSource source)
                return (source.EntityType != null);
            else
                return ObjectHelper.EnumerateFields<DataSource>(Owner).Any(x => x.EntityType != null);
        }
        private void InitLayoutControlItems()
        {
            if (DataSource is DataSource dataSource)
                InitByDataSource(dataSource);
            else
                foreach (DataSource data in ObjectHelper.EnumerateFields<DataSource>(Owner).Where(x => x.PageSupport.EditGroup != null))
                    InitByDataSource(data);
        }
        private void InitByDataSource(DataSource data)
        {
            if (data.PageSupport.EditGroup == null || !Root.Contains(data.PageSupport.EditGroup))
                return;
            LayoutControlGroup EditGroup = data.PageSupport.EditGroup;
            Type typeEntity = data.EntityType;
            foreach (LayoutControlItem baseItem in Items.Where(x => x is LayoutControlItem controlItem && controlItem.Control != null && !(controlItem.Control is UserControl)))
            {
                if (baseItem.Control.DataBindings.Count > 0)
                {
                    string field = baseItem.Control.DataBindings[0].BindingMemberInfo.BindingField;
                    PropertyInfo prop = typeEntity.GetProperty(field);
                    if (prop != null)
                    {
                        var att = prop.GetCustomAttribute<UpdatablePropertyAttribute>();
                        if (att != null)
                        {
                            baseItem.Text = att.Caption + ":";
                        }
                    }
                }
                if (baseItem.Control is BaseEdit baseEdit && baseItem.Parent == data.PageSupport.EditGroup)
                {
                    if (!baseEdit.ReadOnly)
                        baseEdit.ReadOnly = data.ReadOnly || !data.AllowEdit;
                    if (!baseEdit.ReadOnly)
                        data.AddBindingControl(baseEdit);
                }
                if (baseItem.Control is LookUpEdit lookUpEdit)
                {
                    lookUpEdit.Properties.ShowFooter = false;
                    lookUpEdit.Properties.ShowHeader = false;
                }
                if (baseItem.TextVisible)
                {
                    if (baseItem.TextLocation == Locations.Default || baseItem.TextLocation == Locations.Left)
                    {
                        baseItem.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                    }
                    else
                        if (baseItem.TextLocation == DevExpress.Utils.Locations.Top || baseItem.TextLocation == DevExpress.Utils.Locations.Top)
                        {
                            baseItem.AppearanceItemCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top;
                        }
                }
            }
        }
        public void LoadControls()
        {
            foreach (var item in Controls)
            {
                if (item is ISavedControl saved)
                    saved.LoadControls();
            }
        }
        public void SaveControls()
        {
            foreach (var item in Controls)
            {
                if (item is ISavedControl saved)
                    saved.SaveControls();
            }
        }
        public ButtonsPanel PrepareGroup(LayoutItemContainer item)
        {
            if (item is TabbedControlGroup || !(item is LayoutControlGroup))
                return null;
            LayoutControlGroup editGrop = item as LayoutControlGroup;
            LayoutControlGroup parentGroup = editGrop.Parent as LayoutControlGroup;
            editGrop.DefaultLayoutType = LayoutType.Vertical;
            editGrop.TextVisible = false;
            editGrop.Padding = new DevExpress.XtraLayout.Utils.Padding(5);
            editGrop.Text = "Группа редактирования";
            editGrop.CustomizationFormText = "Edit group";
            //ButtonsPanel panel = new ButtonsPanel(parentGroup);
            //parentGroup.AddItem(panel, editGrop, InsertType.Bottom);
            //foreach (BaseLayoutItem space in parentGroup.Items.OfType<EmptySpaceItem>().ToArray())
            //{
            //    panel.Parent.Remove(space);
            //    space.Dispose();
            //}
            //parentGroup.AddItem(new EmptySpaceItem(), panel, InsertType.Bottom);
            return AddSaveSaveCancelPanel(editGrop);
        }
        internal ButtonsPanel AddSaveSaveCancelPanel(LayoutControlGroup editGroup)
        {
            ButtonsPanel panel = new ButtonsPanel(editGroup.Parent);
            editGroup.Parent.AddItem(panel, editGroup, InsertType.Bottom);
            foreach (BaseLayoutItem space in editGroup.Parent.Items.OfType<EmptySpaceItem>().ToArray())
            {
                panel.Parent.Remove(space);
                space.Dispose();
            }
            editGroup.Parent.AddItem(new EmptySpaceItem(), panel, InsertType.Bottom);
            return panel;
        }
        public LayoutControlGroup AppControlToPage(Control control, IPagesFrame pageParam, LayoutControlGroup newPage = null)
        {
            if (control is CfgBaseFrame yesNoFrame)
            {
                yesNoFrame.layoutControl.Dock = DockStyle.None;
                yesNoFrame.Size = yesNoFrame.layoutControl.GetPreferredSize(yesNoFrame.Size);
                yesNoFrame.layoutControl.Dock = DockStyle.Fill;
            }
            Root.BeginInit();
            BeginInit();
            if (pageParam.PagesGroup == null)
            {
                pageParam.PagesGroup = AddTabbedGroup();
                pageParam.PagesGroup.Name = "pagesMain";
                pageParam.PagesGroup.SelectedPageChanged += PagesGroup_SelectedPageChanged;
            }
            if (newPage == null)
            {
                newPage = pageParam.PagesGroup.AddTabPage(control.Text);
            }
            newPage.Name = "page" + control.Name;
            newPage.Padding = new DevExpress.XtraLayout.Utils.Padding(0);
            LayoutItem layoutItem = newPage.AddItem("", control);
            layoutItem.Padding = new DevExpress.XtraLayout.Utils.Padding(0);
            layoutItem.Name = "lc" + control.Name;
            layoutItem.TextVisible = false;
            pageParam.PagesGroup.SelectedTabPageIndex = 0;
            EndInit();
            Root.EndInit();
            return newPage;
        }
        private void PagesGroup_SelectedPageChanged(object sender, LayoutTabPageChangedEventArgs e)
        {
            var control = e.Page.Items.OfType<LayoutControlItem>().Where(x => x.Control != null).OrderBy(o => o.Control.TabIndex).FirstOrDefault();
            if (control != null)
            {
                if (control.Control is AbstractFrame userControl)
                    userControl.FocusIt();
                else
                    if (control.Control is BaseEdit baseEdit)
                        FocusIt(baseEdit);
                    else
                        control.Control.Focus();
            }
        }
        private static void FocusIt(BaseEdit baseEdit)
        {
            baseEdit.SelectAll();
            baseEdit.Focus();
        }
    }
}
