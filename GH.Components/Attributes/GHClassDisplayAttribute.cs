namespace GH.Components
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class GHClassDisplayAttribute : Attribute
    {
        public string Caption { get; set; }

    public string ToolTip { get; set; }
    }
}
