namespace MeshokBrowser.Workers
{
    public class WorkerProcessingOrders : Worker
    {
        public WorkerProcessingOrders(IMainForm form) : base(form)
        {
            ProcessName = "Обработка заказов";
        }
    }
}
