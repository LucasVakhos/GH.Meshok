using System.Reflection;
namespace GH.Components
{
    public static class ObjectHelper
    {
        public static IEnumerable<T> EnumerateFields<T>(object obj)
        {
            return from x in obj.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetProperty)
                   where typeof(T).IsAssignableFrom(x.FieldType)
                   let f = x.GetValue(obj)
                   where f != null
                   select (T)f;
        }
    }
}
