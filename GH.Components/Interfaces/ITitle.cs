namespace GH.Components
{
    public interface ITitle
    {
        int t_id { get; }
        string c_barcode { get; }
        string art_name { get; }
        string c_title { get; }
        DateTime? c_release { get; }
        bool t_enabled { get; }
        int ts_id { get; }
    }
}
