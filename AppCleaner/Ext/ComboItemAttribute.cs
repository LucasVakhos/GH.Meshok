#nullable disable
namespace AppCleaner
{
    public class ComboItemAttribute : Attribute
    {
        public string Name { get; set; } = string.Empty;
        public string Pattern { get; set; } = string.Empty;
        public string SearchLabel { get; set; } = "Cканировать папку:";
        public string PlaceLabel { get; set; } = "Папка для найденного:";
    }
}
