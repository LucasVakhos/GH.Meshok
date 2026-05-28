namespace GH.Interfaces
{
    public interface ISavedControl
    {
        bool SaveLayout { get; set; }
        void LoadControls();
        void SaveControls();
    }

}
