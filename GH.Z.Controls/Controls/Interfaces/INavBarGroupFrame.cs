using DevExpress.XtraNavBar;

namespace GH.Interfaces
{

    public interface INavBarGroupFrame
    {
        NavBarGroup Group { get; }
        bool IsBase { get; }
    }

}
