using DevExpress.XtraNavBar;
namespace GH.Components
{
    public interface INavBarGroupFrame
    {
        NavBarGroup Group { get; }
        bool IsBase { get; }
    }
}
