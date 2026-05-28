using System.ComponentModel;
namespace GH.Components
{
    public class OptionsHelper
    {
        public static string GetObjectText(object obj)
        {
            return OptionsHelper.GetObjectText(obj, false);
        }
        public static void SetOptionValue(object obj, string name, object value)
        {
            TypeDescriptor.GetProperties(obj)[name]?.SetValue(obj, value);
        }
        public static object GetOptionValue(object obj, string name)
        {
            return TypeDescriptor.GetProperties(obj)[name]?.GetValue(obj);
        }
        public static T GetOptionValue<T>(object obj, string name)
        {
            PropertyDescriptor property = TypeDescriptor.GetProperties(obj)[name];
            if (property != null)
                return (T)property.GetValue(obj);
            return default(T);
        }
        public static string GetObjectText(object obj, bool includeSubObjects)
        {
            string str1 = string.Empty;
            try
            {
                foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(obj))
                {
                    if (property.IsBrowsable && property.SerializationVisibility != DesignerSerializationVisibility.Hidden)
                    {
                        if (property.SerializationVisibility == DesignerSerializationVisibility.Content)
                        {
                            if (includeSubObjects)
                            {
                                object obj1 = property.GetValue(obj);
                                string str2 = obj1 != null ? obj1.ToString() : string.Empty;
                                if (!string.IsNullOrEmpty(str2))
                                {
                                    if (str1.Length > 0)
                                        str1 += ", ";
                                    str1 += string.Format("{0} = {{ {1} }}", (object)property.Name, (object)str2);
                                }
                            }
                        }
                        else if (!property.IsReadOnly && property.ShouldSerializeValue(obj))
                        {
                            if (str1.Length > 0)
                                str1 += ", ";
                            str1 += property.Name;
                            object obj1 = property.GetValue(obj);
                            str1 = !property.PropertyType.Equals(typeof(string)) ? str1 + string.Format(" = {0}", obj1) : str1 + string.Format(" = '{0}'", (object)property.Converter.ConvertToString(obj1));
                        }
                    }
                }
            }
            catch
            {
            }
            return str1;
        }
    }
}
