namespace MeshokBrowser.Workers
{
    public class WorkerScanLots : Worker
    {
        public WorkerScanLots(IMainForm form) : base(form)
        {
            ProcessName = "WorkerScanLots";
        }
    }
}
