using DevExpress.Utils.Design;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using System.ComponentModel.Design;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Forms.Design;
namespace GH.Components
{
    public class PropertyGridExtender
    {
        private ContainerControl container;
        private PropertyGridExtender.FilterTextEdit filterEdit;
        private PropertyGrid propertyGrid;
        private int internalSizing;
        private object[] selectedObjects;
        public static void Check(ComponentDesigner designer)
        {
            if (designer == null || designer.Component == null)
                return;
            PropertyGridExtender.Check((System.IServiceProvider)designer.Component.Site);
        }
        public static void Check(System.IServiceProvider serviceProvider)
        {
            if (!RegistryDesignerSkinHelper.AllowPropertyGridFiltering)
                return;
            new PropertyGridExtender().ExtendPropertyGrid(serviceProvider, false);
        }
        public static void Disable(System.IServiceProvider serviceProvider)
        {
            RegistryDesignerSkinHelper.AllowPropertyGridFiltering = false;
            new PropertyGridExtender().ExtendPropertyGrid(serviceProvider, true);
        }
        public void ExtendPropertyGrid(System.IServiceProvider services, bool removeOnly = false)
        {
            if (services == null || !removeOnly && !RegistryDesignerSkinHelper.AllowPropertyGridFiltering)
                return;
            object service = services.GetService(typeof(PropertyGridExtender.IVSMDPropertyBrowser));
            if (service == null)
                return;
            PropertyInfo property = service.GetType().GetProperty("Window");
            if (property == (PropertyInfo)null)
                return;
            this.container = property.GetValue(service, (object[])null) as ContainerControl;
            if (this.container == null)
                return;
            IEnumerable<Control> source = this.container.Controls.Cast<Control>();
            Control control = source.FirstOrDefault<Control>((Func<Control, bool>)(q => q.Name == "dxFindFilter"));
            if (control != null)
            {
                if (!removeOnly || !(control.Tag is PropertyGridExtender tag))
                    return;
                tag.Remove(false);
            }
            else
            {
                if (removeOnly || (!(source.FirstOrDefault<Control>((Func<Control, bool>)(q => q is PropertyGrid)) is PropertyGrid pg) || !this.ReplacePropertyTab(pg, false)))
                    return;
                pg.SelectedObjectsChanged += new EventHandler(this.OnSelectedObjectsChanged);
                pg.Resize += new EventHandler(this.OnResizeGrid);
                pg.PropertyTabChanged += new PropertyTabChangedEventHandler(this.OnPropertyTabChanged);
                pg.BackColorChanged += new EventHandler(this.OnStyleChanged);
                pg.Disposed += new EventHandler(this.OnGridDisposed);
                this.propertyGrid = pg;
                this.filterEdit = this.CreateFilterEdit(services);
                this.container.Controls.Add((Control)this.filterEdit);
                this.OnResizeGrid((object)this, EventArgs.Empty);
            }
        }
        private void OnGridDisposed(object sender, EventArgs e)
        {
            this.Remove(true);
        }
        private void OnStyleChanged(object sender, EventArgs e)
        {
            this.OnResizeGrid((object)this, EventArgs.Empty);
        }
        private bool ReplacePropertyTab(PropertyGrid pg, bool restoreOriginal = false)
        {
            //FieldInfo field = typeof(PropertyGrid).GetField("viewTabs", BindingFlags.Instance | BindingFlags.NonPublic);
            //if (field == (FieldInfo)null || !(field.GetValue((object)pg) is PropertyTab[] propertyTabArray))
            //    return false;
            //int index = ((IList<PropertyTab>)propertyTabArray).FindIndex<PropertyTab>((Predicate<PropertyTab>)(q => q is PropertiesTab));
            //if (index < 0)
            //    return false;
            //PropertyTab wrappedTab = propertyTabArray[index];
            //if (restoreOriginal)
            //{
            //    if (wrappedTab == this.propertiesTab)
            //    {
            //        propertyTabArray[index] = this.propertiesTab.wrappedTab;
            //        pg.Refresh();
            //    }
            //    return true;
            //}
            //if (wrappedTab.GetType().Name != "VSPropertiesTab")
            //    return false;
            //this.propertiesTab = new MyPropertiesTab();
            //this.propertiesTab.SetWrappedTab(this, wrappedTab);
            //propertyTabArray[index] = (PropertyTab)this.propertiesTab;
            //pg.Refresh();
            return true;
        }
        private void OnPropertyTabChanged(object s, PropertyTabChangedEventArgs e)
        {
            this.OnResizeGrid((object)this, EventArgs.Empty);
        }
        protected void Remove(bool propertyGridDestroyed = false)
        {
            this.propertyGrid.Resize -= new EventHandler(this.OnResizeGrid);
            this.propertyGrid.SelectedObjectsChanged -= new EventHandler(this.OnSelectedObjectsChanged);
            this.propertyGrid.PropertyTabChanged -= new PropertyTabChangedEventHandler(this.OnPropertyTabChanged);
            this.propertyGrid.BackColorChanged -= new EventHandler(this.OnStyleChanged);
            this.propertyGrid.Disposed -= new EventHandler(this.OnGridDisposed);
            if (!propertyGridDestroyed)
            {
                this.container = (ContainerControl)null;
                this.ReplacePropertyTab(this.propertyGrid, true);
            }
            this.filterEdit.Dispose();
            this.filterEdit = (PropertyGridExtender.FilterTextEdit)null;
        }
        private PropertyGridExtender.FilterTextEdit CreateFilterEdit(
          System.IServiceProvider services)
        {
            PropertyGridExtender.FilterTextEdit filterTextEdit = new PropertyGridExtender.FilterTextEdit();
            filterTextEdit.Tag = (object)this;
            filterTextEdit.Name = "dxFindFilter";
            filterTextEdit.TabIndex = 0;
            filterTextEdit.TabStop = true;
            filterTextEdit.LookAndFeel.UseDefaultLookAndFeel = false;
            filterTextEdit.Properties.ShowNullValuePromptWhenFocused = true;
            filterTextEdit.Properties.NullValuePrompt = "Type here to search properties";
            filterTextEdit.Properties.NullValuePromptShowForEmptyValue = true;
            filterTextEdit.EditValueChanged += new EventHandler(this.OnFilterEditValueChanged);
            filterTextEdit.LostFocus += new EventHandler(this.OnEditorListFocus);
            return filterTextEdit;
        }
        private void OnEditorListFocus(object sender, EventArgs e)
        {
            ((PropertyGridExtender.FilterTextEdit)sender).UpdateNullText();
        }
        private bool IsAllowFilter()
        {
            return this.propertyGrid.SelectedTab == null || !(this.propertyGrid.SelectedTab is EventsTab);
        }
        private void OnResizeGrid(object sender, EventArgs e)
        {
            if (this.internalSizing > 0 || this.filterEdit == null || this.propertyGrid == null)
                return;
            ++this.internalSizing;
            this.filterEdit.Location = new Point(this.propertyGrid.Left + 1, this.propertyGrid.Top);
            this.filterEdit.Width = this.propertyGrid.Width - 2;
            if (!this.IsAllowFilter())
            {
                this.filterEdit.Visible = false;
            }
            else
            {
                this.filterEdit.Visible = true;
                this.propertyGrid.SetBounds(this.propertyGrid.Left, this.filterEdit.Bottom, this.propertyGrid.Width, this.propertyGrid.Bottom - this.filterEdit.Bottom);
            }
            --this.internalSizing;
        }
        private void OnSelectedObjectsChanged(object sender, EventArgs e)
        {
            PropertyGrid pg = (PropertyGrid)sender;
            if (this.filterEdit == null || (pg.SelectedObject == null || string.IsNullOrEmpty(this.Filter)))
                return;
            if (this.CheckSelectedDifferentObjects(pg.SelectedObjects))
            {
                this.filterEdit.ResetFilter();
            }
            this.UpdatePropertyGridSelectedObjects(pg, false);
        }
        private bool CheckSelectedDifferentObjects(object[] currentSelectedObjects)
        {
            object[] selectedObjects = this.selectedObjects;
            this.selectedObjects = (object[])currentSelectedObjects.Clone();
            if (selectedObjects == null || this.selectedObjects == null)
                return true;
            for (int index = 0; index < Math.Min(selectedObjects.Length, this.selectedObjects.Length); ++index)
            {
                if (object.ReferenceEquals(selectedObjects[index], this.selectedObjects[index]) || selectedObjects[index] != null && this.selectedObjects[index] != null && selectedObjects[index].GetType().Equals(this.selectedObjects[index].GetType()))
                    return false;
            }
            return true;
        }
        private void UpdatePropertyGridSelectedObjects(PropertyGrid pg, bool saveState = false)
        {
            pg.Refresh();
        }
        internal string Filter
        {
            get
            {
                return this.filterEdit == null ? string.Empty : this.filterEdit.Text.ToLower();
            }
        }
        private void OnFilterEditValueChanged(object sender, EventArgs e)
        {
            this.UpdatePropertyGridSelectedObjects(this.propertyGrid, true);
        }
        private class FilterTextEdit : ButtonEdit
        {
            public FilterTextEdit()
            {
                this.Properties.Buttons.Clear();
                this.Properties.Buttons.Add(new DevExpress.XtraEditors.Controls.EditorButton()
                {
                    Kind = ButtonPredefines.Clear,
                    Enabled = false,
                    ToolTip = "Clear filter"
                });
                this.ButtonClick += new ButtonPressedEventHandler(this.FilterTextEdit_ButtonClick);
            }
            private void FilterTextEdit_ButtonClick(object sender, ButtonPressedEventArgs e)
            {
                this.EditValue = (object)"";
            }
            protected override void OnEditValueChanged()
            {
                base.OnEditValueChanged();
                if (this.Properties.Buttons.LastOrDefault<DevExpress.XtraEditors.Controls.EditorButton>() == null)
                    return;
                this.Properties.Buttons.LastOrDefault<DevExpress.XtraEditors.Controls.EditorButton>().Enabled = !string.IsNullOrEmpty((this.EditValue ?? (object)"").ToString());
            }
            internal void ResetFilter()
            {
                this.Text = "";
                if (this.ContainsFocus)
                    return;
                this.UpdateNullText();
            }
            internal void UpdateNullText()
            {
                this.IsModified = false;
                this.OnLeave(EventArgs.Empty);
                this.UpdateMaskBoxDisplayText();
                this.LayoutChanged();
            }
            protected override bool ProcessDialogKey(Keys keyData)
            {
                switch (keyData)
                {
                    case Keys.Insert | Keys.Shift:
                    case Keys.V | Keys.Control:
                        this.Paste();
                        return true;
                    case Keys.C | Keys.Control:
                        this.Copy();
                        return true;
                    case Keys.X | Keys.Control:
                        this.Cut();
                        return true;
                    default:
                        return base.ProcessDialogKey(keyData);
                }
            }
        }
        [Guid("74946810-37A0-11D2-A273-00C04F8EF4FF")]
        [InterfaceType(1)]
        [ComImport]
        private interface IVSMDPropertyBrowser
        {
            [DispId(1610678272)]
            uint WindowGlyphResourceID { [MethodImpl(MethodImplOptions.InternalCall)] get; }
            [MethodImpl(MethodImplOptions.InternalCall)]
            [return: MarshalAs(UnmanagedType.Interface)]
            object CreatePropertyGrid();
            [MethodImpl(MethodImplOptions.InternalCall)]
            void Refresh();
        }
    }
}
