using System.Reflection;
namespace GH.Components
{
    public static class EditTypesExtension
    {
        public static string GetCategory(this EditTypes value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());
            return !(Attribute.GetCustomAttribute(field, typeof(MapAttribute)) is MapAttribute attribute) ? value.ToString() : attribute.Category;
        }
        public static string GetPath(this EditTypes value, bool large = true)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());
            return !(Attribute.GetCustomAttribute(field, typeof(MapAttribute)) is MapAttribute attribute) ? value.ToString() : large ? attribute.LargeFullPath : attribute.SmallFullPath;
        }
    }
}
