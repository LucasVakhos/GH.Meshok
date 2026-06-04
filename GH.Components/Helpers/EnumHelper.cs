using System.ComponentModel.DataAnnotations;
using System.Reflection;
namespace GH.Components
{
    public static class EnumHelper<T>
    {
        public static KeyValuePair<int, string>[] GetIntKeyLookupSource()
        {
            if (!typeof(T).IsEnum)
                throw new ArgumentException("Type must be an enum");
            Dictionary<int, string> result = new Dictionary<int, string>() { };
            T _enum = default(T);
            foreach (var f in _enum.GetType().GetFields().Distinct())
            {
                if (!f.FieldType.IsEnum)
                    continue;
                int val = (int)f.GetValue(_enum);
                if (result.ContainsKey(val))
                    continue;
                DisplayAttribute da = f.GetCustomAttribute<DisplayAttribute>();
                if (da == null)
                    result.Add(val, val.ToString());
                else
                    result.Add(val, da.Name);
            }
            return result.ToArray();
        }

    public static KeyValuePair<T, string>[] GetEnumKeyLookupSource()
        {
            if (!typeof(T).IsEnum)
                throw new ArgumentException("Type must be an enum");
            Dictionary<T, string> result = new Dictionary<T, string>();
            T _enum = default(T);
            foreach (FieldInfo f in _enum.GetType().GetFields().Distinct())
            {
                if (!f.FieldType.IsEnum)
                    continue;
                DisplayAttribute da = f.GetCustomAttribute<DisplayAttribute>();
                T val = (T)f.GetValue(_enum);
                if (result.ContainsKey(val))
                    continue;
                if (da == null)
                    result.Add(val, val.ToString());
                else
                    result.Add(val, da.Name);
            }
            return result.ToArray();
        }
    public static IList<T> GetValues(Enum value)
        {
            var enumValues = new List<T>();
            foreach (FieldInfo fi in value.GetType().GetFields(BindingFlags.Static | BindingFlags.Public))
            {
                enumValues.Add((T)Enum.Parse(value.GetType(), fi.Name, false));
            }
            return enumValues;
        }
    public static T Parse(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }
    public static T GetValueFromName(string name)
        {
            var type = typeof(T);
            if (!type.IsEnum) throw new InvalidOperationException();
            foreach (var field in type.GetFields())
            {
                var attribute = Attribute.GetCustomAttribute(field, typeof(DisplayAttribute)) as DisplayAttribute;
                if (attribute != null)
                {
                    if (attribute.Name == name)
                    {
                        return (T)field.GetValue(null);
                    }
                }
                else
                {
                    if (field.Name == name)
                        return (T)field.GetValue(null);
                }
            }
            throw new ArgumentOutOfRangeException("name");
        }
    public static IList<string> GetNames(Enum value)
        {
            return value.GetType().GetFields(BindingFlags.Static | BindingFlags.Public).Select(fi => fi.Name).ToList();
        }
    public static IList<string> GetDisplayValues(Enum value)
        {
            return GetNames(value).Select(obj => GetDisplayValue(Parse(obj))).ToList();
        }
    public static string GetDisplayValue(T value)
        {
            var fieldInfo = value.GetType().GetField(value.ToString());
            var descriptionAttributes = fieldInfo.GetCustomAttributes(
                typeof(DisplayAttribute), false) as DisplayAttribute[];
            if (descriptionAttributes == null) return string.Empty;
            return (descriptionAttributes.Length > 0) ? descriptionAttributes[0].Name : value.ToString();
        }
    }

    public static class EnumExtensions
    {
        public static TAttribute GetAttribute<TAttribute>(this Enum enumValue) where TAttribute : Attribute
        {
            return enumValue.GetType().GetMember(enumValue.ToString()).First().GetCustomAttribute<TAttribute>();
        }
    public static string GetDisplayValue(this Enum value)
        {
            FieldInfo fieldInfo = value.GetType().GetField(value.ToString());
            if (fieldInfo == null)
                return null;
            return ((DisplayAttribute)fieldInfo.GetCustomAttribute(typeof(DisplayAttribute))).Name;
        }
    public static int ToInt(this Enum value)
        {
            return (int)(int)Enum.ToObject(value.GetType(), value);
        }
    }
}
