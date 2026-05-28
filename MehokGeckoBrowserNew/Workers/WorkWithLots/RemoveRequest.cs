namespace MeshokBrowser.Workers
{
    public class RemoveRequest : Worker
    {
        public RemoveRequest(IMainForm form) : base(form)
        {
            ProcessName = "RemoveRequest";
        }
    }
}
