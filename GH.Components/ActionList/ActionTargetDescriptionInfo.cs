using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraNavBar;
using System.Reflection;
namespace GH.Components
{
    public class ActionTargetDescriptionInfo
    {
        private Dictionary<string, PropertyInfo> properties;
        private Type targetType;
        public ActionTargetDescriptionInfo(Type targetType)
        {
            properties = new Dictionary<string, PropertyInfo>();
            this.targetType = targetType;
            foreach (PropertyInfo property in targetType.GetProperties())
                properties.Add(property.Name, property);
        }
        public Type TargetType
        {
            get
            {
                return targetType;
            }
        }
        private string checkPropertyName(string propertyName)
        {
            //List<Type> lst = new List<Type>()
            //{
            //    typeof (ButtonBase),
            //    typeof (BaseButton),
            //    typeof (ToolStripMenuItem),
            //    typeof (BarButtonItem),
            //    typeof (BarSubItem),
            //    typeof (NavBarItem)
            //};
            switch (propertyName)
            {
                case "ToolTipText":
                    if (targetType.IsAssignableFrom(typeof(BarButtonItem)))
                        return "Hint";
                    break;
                case "Caption":
                    if (targetType.IsAssignableFrom(typeof(NavBarItem)))
                        break;
                    propertyName = "Text";
                    break;
                case "Image":
                    if (targetType.IsAssignableFrom(typeof(NavBarItem)))
                        return "SmallImage";
                    if (targetType.IsAssignableFrom(typeof(SimpleButton)))
                        return "Image";
                    if (targetType.IsAssignableFrom(typeof(ToolStripMenuItem)))
                        return "Image";
                    if (targetType.IsAssignableFrom(typeof(BarButtonItem)))
                        return "Glyph";
                    break;
                case "LargeImage":
                    if (targetType.IsAssignableFrom(typeof(NavBarItem)))
                        return "LargeGlyph";
                    if (targetType.IsAssignableFrom(typeof(BarButtonItem)))
                        return "LargeGlyph";
                    break;
                default:
                    break;
            }
            return propertyName;
        }
        internal void SetValue(string propertyName, object target, object value)
        {
            string[] new_propertyNames = checkPropertyName(propertyName).Split('.');
            if (!properties.ContainsKey(new_propertyNames[0]))
                return;
            try
            {
                if (new_propertyNames.Length > 1)
                {
                    target = properties[new_propertyNames[0]].GetValue(target);
                    target.GetType().GetProperty(new_propertyNames[1]).SetValue(target, value);
                }
                else
                    properties[new_propertyNames[0]].SetValue(target, value, (object[])null);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }
        internal object GetValue(string propertyName, object source)
        {
            propertyName = checkPropertyName(propertyName);
            if (properties.ContainsKey(propertyName))
                return properties[propertyName].GetValue(source, (object[])null);
            return (object)null;
        }
    }
}
