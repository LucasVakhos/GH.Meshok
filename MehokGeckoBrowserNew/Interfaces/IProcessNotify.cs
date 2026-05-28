namespace MeshokBrowser.Workers
{
    public interface IProcessNotify
    {
        void SetTotalSteps(int value);
        void IncCurrentStep();
        void SendMessage(string value);
    }
}
