namespace GH.Components
{
    public interface ISavedControl
    {
        bool SaveLayout { get; set; }
        void LoadControls();
        void SaveControls();
    }
}
