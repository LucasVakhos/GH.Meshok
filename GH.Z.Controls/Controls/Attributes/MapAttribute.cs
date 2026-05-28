using System;

namespace GH.Components
{
    public class MapAttribute : Attribute
    {
        static string small_ext = "_16x16.png";
        static string large_ext = "_32x32.png";

        public string Category { get; }
        protected string Path { get; }
        protected string ResourceName { get; }
        public EditTypes Button { get; set; }


        public string SmallFullPath => Path + ResourceName + small_ext;

        public string LargeFullPath => Path + ResourceName + large_ext;


        public MapAttribute(string category, string path, string resourceName, EditTypes button)
        {
            Category = category;
            Path = path.Replace(" ", "%20");
            ResourceName = resourceName;
            Button = button;
        }
    }
}
