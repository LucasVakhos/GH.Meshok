using System.Windows.Forms;

namespace GH.Components
{
    public interface IAppContext
    {
        Form MaitForm { get; set; }
        Form GetMaitForm();
    }
}