using DevExpress.XtraNavBar;
using GH.Controls;

namespace GH.Interfaces
{

    public interface INavBarForm
    {
        NavBarControl NavBar { get; }
        FrameHolder FrameHolder { get; }
    }

}
