using System.ComponentModel;
using System.Drawing.Design;
using System.Reflection;
using System.Windows.Forms.Design;
namespace GH.Components
{
    public class DocSupportBaseEditor : UITypeEditor
    {
        protected class NameList<T> : ListBox
        {
            private object value;
            private IWindowsFormsEditorService service;
            public NameList(Type type, IWindowsFormsEditorService service, object value)
            {
                this.service = service;
                this.value = value;
                bool isAbstractEntity = IsAbstractEntity(type);
                foreach (PropertyInfo o in type.GetProperties().Where(x => x.PropertyType == typeof(T)))
                {
                    if (isAbstractEntity && o.Name == nameof(AbstractEntity.HasChanges))
                        continue;
                    base.Items.Add(o.Name);
                }
            }
            private static bool IsAbstractEntity(Type type)
            {
                if (type.BaseType != typeof(AbstractEntity) && type.BaseType != typeof(object))
                    return IsAbstractEntity(type.BaseType);
                return type.BaseType == typeof(AbstractEntity);
            }
            protected override void OnClick(EventArgs e)
            {
                base.OnClick(e);
                value = base.SelectedItem;
                service.CloseDropDown();
            }
            public object Value
            {
                get { return value; }
            }
        }
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }
        public override bool IsDropDownResizable
        {
            get { return true; }
        }
        protected DataSource GetDataSource(ITypeDescriptorContext context)
        {
            FieldInfo fi = context.Instance.GetType().GetField("_owner", BindingFlags.NonPublic | BindingFlags.Instance);
            return fi.GetValue(context.Instance) as DataSource;
        }
    }
    public class FieldBoolListEditor : DocSupportBaseEditor
    {
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (provider == null)
                return value;
            IWindowsFormsEditorService service = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            if (service == null)
                return value;
            DataSource _instance = GetDataSource(context);
            if (_instance.EntityType == null)
                return value;
            NameList<bool> ui = new NameList<bool>(_instance.EntityType, service, value);
            service.DropDownControl(ui);
            return ui.Value;
        }
    }
    public class FieldIntListEditor : DocSupportBaseEditor
    {
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (provider == null)
                return value;
            IWindowsFormsEditorService service = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            if (service == null)
                return value;
            DataSource _instance = GetDataSource(context);
            if (_instance.EntityType == null)
                return value;
            NameList<int> ui = new NameList<int>(_instance.EntityType, service, value);
            service.DropDownControl(ui);
            return ui.Value;
        }
    }
}
