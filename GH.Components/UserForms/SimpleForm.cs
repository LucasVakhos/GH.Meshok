using DevExpress.XtraEditors;
using System.ComponentModel;
namespace GH.Components
{
    [ToolboxItem(false)]
    public class SimpleForm : XtraForm
    {
        public bool IsDesignMode => this.IsDesignMode();
    public bool IsRuntimeMode => !IsDesignMode;
    public SimpleForm()
        {
        }
    protected override void OnLoad(EventArgs e)
        {
            if (IsRuntimeMode)
            {
                Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            }
            base.OnLoad(e);
        }
    }
}
