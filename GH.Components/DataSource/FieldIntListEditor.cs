using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms.Design;
namespace GH.Components
{
    public class FieldIntListEditor : BaseListEditor
    {
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (provider == null)
                return value;
            IWindowsFormsEditorService service = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            if (service == null)
                return value;
            InnerList<DataSource, int> ui = new InnerList<DataSource, int>(this, service, value);
            service.DropDownControl(ui);
            return ui.Value;
        }
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }
        public override bool IsDropDownResizable
        {
            get { return true; }
        }
    }
}
