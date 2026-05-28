using System.ComponentModel;
using System.Drawing.Design;
using System.Reflection;
using System.Windows.Forms.Design;
namespace GH.Components
{
    public class BaseListEditor : UITypeEditor
    {
        protected class InnerList<TClass, TValuesList> : ListBox where TClass : class
        {
            private object value;
            private IWindowsFormsEditorService service;
            public InnerList(UITypeEditor host, IWindowsFormsEditorService service, object value)
            {
                this.service = service;
                this.value = value;
                if (typeof(TClass) == typeof(TValuesList))
                {
                    TypeConverter conv = TypeDescriptor.GetConverter(typeof(TClass));
                    foreach (object o in conv.GetStandardValues())
                        base.Items.Add(o);
                }
                else
                {
                    foreach (PropertyInfo o in typeof(TClass).GetProperties().Where(x => x.GetType() == typeof(TValuesList)))
                    {
                        base.Items.Add(o.Name);
                    }
                }
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
    }
}
