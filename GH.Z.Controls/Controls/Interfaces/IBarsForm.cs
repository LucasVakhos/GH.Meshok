using DevExpress.XtraBars;

namespace GH.Components
{
    public interface IBarsForm
    {
        BarManager BarManager { get; }
        Bar StatusBar { get; }
    }

}
