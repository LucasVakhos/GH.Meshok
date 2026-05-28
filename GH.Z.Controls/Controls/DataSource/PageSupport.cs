using DevExpress.XtraEditors;
using DevExpress.XtraLayout;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace GH.Components
{
    #region Other

    [SerializableAttribute]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class PageSupport
    {

        #region поля и свойства
        private readonly DataSource _owner;
        internal bool Supported => !(PageForEdit == null || PageForView == null);
        internal bool EditSelected => Supported && PageControl.SelectedTabPage == PageForEdit;
        internal bool ViewSelected => Supported && PageControl.SelectedTabPage == PageForView;

        TabbedControlGroup _pages = null;
        [Bindable(false)]
        public TabbedControlGroup PageControl
        {
            get
            {
                if (_pages == null)
                {
                    if (PageForView != null)
                    {
                        _pages = PageForView.ParentTabbedGroup as TabbedControlGroup;
                    }
                    else if (PageForEdit != null)
                    {
                        _pages = PageForEdit.ParentTabbedGroup as TabbedControlGroup;
                    }
                }

                return _pages;
            }
        }

        LayoutControlGroup _editPage = null;
        [DefaultValue(null)]
        public LayoutControlGroup PageForEdit
        {
            get => _editPage;
            set
            {
                if (_editPage == value)
                    return;

                if (_owner.IsDesignMode() && value != null && !value.IsInTabbedGroup)
                    return;

                if (_viewPage == value)
                    return;

                if (_viewPage != null && value != null &&
                    value.ParentTabbedGroup != _viewPage.ParentTabbedGroup)
                    return;

                _editPage = value;
            }
        }

        LayoutControlGroup _viewPage = null;
        [DefaultValue(null)]
        public LayoutControlGroup PageForView
        {
            get => _viewPage;
            set
            {
                if (_viewPage == value)
                    return;
                if (_owner.IsDesignMode() && value != null && !value.IsInTabbedGroup)
                    return;

                if (_editPage == value)
                    return;

                if (value != null && _editPage != null &&
                    _editPage.ParentTabbedGroup != value.ParentTabbedGroup)
                    _viewPage = null;

                _viewPage = value;

            }
        }

        LayoutControlGroup _editGroup = null;
        [DefaultValue(null)]
        public LayoutControlGroup EditGroup
        {
            get => _editGroup;
            set
            {
                if (_editGroup == value)
                    return;

                _editGroup = value;
            }
        }



        #endregion

        public PageSupport(DataSource owner)
        {
            _owner = owner;
        }

        #region Methods
        private void Pages_SelectedPageChanged(object sender, LayoutTabPageChangedEventArgs e)
        {
            if (e.PrevPage == PageForEdit)
            {
                if (_owner.EditMode)
                    _owner.Cancel();
            }
        }

        internal void CheckPages(DataState dataState)
        {

            if (!Supported || _owner.SkipPageSupport)
                return;

            switch (dataState)
            {
                case DataState.Inactive:
                case DataState.Browsing:
                    ViewSelect();
                    break;
                case DataState.BeginEditing:
                case DataState.Inserting:
                case DataState.Editing:
                    EditSelect();
                    break;
                default:
                    break;
            }

        }

        internal void EditSelect()
        {
            SelectPage(PageForEdit);
        }

        internal void ViewSelect()
        {
            SelectPage(PageForView);
        }

        private void SelectPage(LayoutGroup page)
        {
            if (PageControl.SelectedTabPage != page)
            {
                PageControl.SelectedPageChanged -= Pages_SelectedPageChanged;
                PageControl.SelectedTabPage = page;
                _owner.Owner.SelectNextControl(null, true, true, true, true);
                //OpenDetails(page);
                PageControl.SelectedPageChanged += Pages_SelectedPageChanged;
            }
        }

        internal bool CheckSupportrd(EditTypes buttonType)
        {
            if (!Supported)
                return true;

            switch (buttonType)
            {
                case EditTypes.Save:
                case EditTypes.Cancel:
                    return ViewSelected || EditSelected;
                default:
                    return ViewSelected;
            }
        }

        internal void EndInit()
        {

            if (Supported)
            {
                PageControl.SelectedTabPage = PageForView;
                PageControl.SelectedPageChanged += Pages_SelectedPageChanged;
            }

            if (EditGroup != null)
                InitGroup(EditGroup);
        }

        private void InitGroup(LayoutItemContainer container)
        {
            if (container is LayoutControlGroup group)
            {
                foreach (var item in group.Items)
                {
                    if (item is LayoutControlItem layoutControl)
                    {
                        Control control = layoutControl.Control;
                        if (control is UserControl || control == null)
                            continue;

                        if (control is BaseEdit baseEdit)
                        {

                            control.KeyDown += Control_KeyDown;
                        }
                        continue;
                    }

                    if (item is LayoutControlGroup layoutControlGroup)
                    {
                        InitGroup(layoutControlGroup);
                        continue;
                    }

                    if (item is TabbedControlGroup tabbedControlGroup)
                    {
                        InitGroup(tabbedControlGroup);
                        continue;
                    }
                }
            }
            else
            if (container is TabbedControlGroup tabs)
            {
                foreach (LayoutControlGroup layoutControlGroup in tabs.TabPages)
                {
                    InitGroup(layoutControlGroup);
                }
            }
        }

        private void Control_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    e.Handled = true;
                    _owner.Cancel();
                    break;
                default:
                    break;
            }
        }


        public override string ToString()
        {
            string res = "";
            if (EditGroup != null)
                res += EditGroup.Name;

            if (PageForView != null)
            {
                if (res != "")
                    res += ", ";
                res += "View = " + PageForView.Name;
            }

            if (PageForEdit != null)
            {
                if (res != "")
                    res += ", ";
                res += "Edit = " + PageForEdit.Name;
            }
            return res;
        }

        #endregion

    }

    #endregion
}

