namespace MeshokBrowser.Workers
{
    public class POScanPacket : Worker
    {
        public POScanPacket(IMainForm form) : base(form)
        {
            ProcessName = "Сканирование пакетов";
        }
    }
}
