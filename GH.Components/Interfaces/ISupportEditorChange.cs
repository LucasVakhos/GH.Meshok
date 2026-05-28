using System.ComponentModel;
using System.Globalization;
using System.Resources;
namespace GH.Components
{
    [GHDescription(SR.ISupportEditorChangeDescr)]
    public interface ISupportEditorChange
    {
        void EditorChanged(object sender);
    }
    internal sealed class SR
    {
        internal const string ISupportEditorChangeDescr = "ISupportEditorChangeDescr";
        private static SR loader;
        private ResourceManager resources;
        internal SR()
        {
            this.resources = new ResourceManager("System", this.GetType().Assembly);
        }
        private static SR GetLoader()
        {
            if (SR.loader == null)
            {
                SR sr = new SR();
                Interlocked.CompareExchange<SR>(ref SR.loader, sr, (SR)null);
            }
            return SR.loader;
        }
        private static CultureInfo Culture
        {
            get
            {
                return (CultureInfo)null;
            }
        }
        public static ResourceManager Resources
        {
            get
            {
                return SR.GetLoader().resources;
            }
        }
        public static string GetString(string name, params object[] args)
        {
            SR loader = SR.GetLoader();
            if (loader == null)
                return (string)null;
            string format = loader.resources.GetString(name, SR.Culture);
            if (args == null || args.Length == 0)
                return format;
            for (int index = 0; index < args.Length; ++index)
            {
                if (args[index] is string str && str.Length > 1024)
                    args[index] = (object)(str.Substring(0, 1021) + "...");
            }
            return string.Format((IFormatProvider)CultureInfo.CurrentCulture, format, args);
        }
        public static string GetString(string name)
        {
            return SR.GetLoader()?.resources.GetString(name, SR.Culture);
        }
        public static string GetString(string name, out bool usedFallback)
        {
            usedFallback = false;
            return SR.GetString(name);
        }
        public static object GetObject(string name)
        {
            return SR.GetLoader()?.resources.GetObject(name, SR.Culture);
        }
    }
    [AttributeUsage(AttributeTargets.All)]
    internal sealed class GHDescriptionAttribute : DescriptionAttribute
    {
        private bool replaced;
        public GHDescriptionAttribute(string description)
          : base(description)
        {
        }
        public override string Description
        {
            get
            {
                if (!this.replaced)
                {
                    this.replaced = true;
                    this.DescriptionValue = SR.GetString(base.Description);
                }
                return base.Description;
            }
        }
    }
}
