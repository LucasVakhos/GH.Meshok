using DevExpress.XtraNavBar;
namespace GH.Components
{
    public interface INavBarForm
    {
        NavBarControl NavBar { get; }
        FrameHolder FrameHolder { get; }
    }
}
