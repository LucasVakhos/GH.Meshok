namespace MeshokBrowser.Workers
{
    public class WorkerCreateCSV : Worker
    {
        public WorkerCreateCSV(IMainForm form) : base(form)
        {
            ProcessName = "WorkerCreateCSV";
        }
    }
}
