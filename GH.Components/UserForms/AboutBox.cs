using System.Reflection;
namespace GH.Components
{
    public partial class AboutBox : SimpleForm
    {
        //static string s_webadres = "bridgenote.com";
        public AboutBox()
        {
            InitializeComponent();
        }
        private void DisplayProductInformation()
        {
            Text = string.Format("About {0}", AssemblyTitle);
            aboutGroup.Text = AssemblyTitle;
            lblCopyright.Text = string.Format("Copyright: {0}", AssemblyCopyright);
            lblProductname.Text = string.Format("Product: {0} running on {1}", AssemblyProduct, Environment.MachineName);
            lblVersion.Text = string.Format("Version {0}", AssemblyVersion);
            aboutExtraLabel.Text = AssemblyDescription;
            lblCompany.Text = AssemblyCompany;
            //lblCompany.Click += LblLinkToSite_Click;
        }
        //private void LblLinkToSite_Click(object sender, EventArgs e)
        //{
        //    lblCompany.LinkVisited = true;
        //    Process.Start("https://" + s_webadres);
        //}
        public static string AssemblyTitle
        {
            get
            {
                object[] customAttributes = Assembly.GetEntryAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                if (customAttributes.Length > 0)
                {
                    AssemblyTitleAttribute assemblyTitleAttribute = (AssemblyTitleAttribute)customAttributes[0];
                    if (!string.IsNullOrEmpty(assemblyTitleAttribute.Title))
                        return assemblyTitleAttribute.Title;
                }
                return Path.GetFileNameWithoutExtension(Assembly.GetEntryAssembly().CodeBase);
            }
        }
        public static string AssemblyVersion
        {
            get
            {
                return Assembly.GetEntryAssembly().GetName().Version.ToString();
            }
        }
        public static string AssemblyDescription
        {
            get
            {
                object[] customAttributes = Assembly.GetEntryAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
                if (customAttributes.Length == 0)
                    return "";
                return ((AssemblyDescriptionAttribute)customAttributes[0]).Description;
            }
        }
        public static string AssemblyProduct
        {
            get
            {
                object[] customAttributes = Assembly.GetEntryAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                if (customAttributes.Length == 0)
                    return "";
                return ((AssemblyProductAttribute)customAttributes[0]).Product;
            }
        }
        public static string AssemblyCopyright
        {
            get
            {
                object[] customAttributes = Assembly.GetEntryAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                if (customAttributes.Length == 0)
                    return "";
                return ((AssemblyCopyrightAttribute)customAttributes[0]).Copyright;
            }
        }
        public static string AssemblyCompany
        {
            get
            {
                object[] customAttributes = Assembly.GetEntryAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
                if (customAttributes.Length == 0)
                    return "";
                return ((AssemblyCompanyAttribute)customAttributes[0]).Company;
            }
        }
        private void AboutBox_Load(object sender, EventArgs e)
        {
            if (DesignMode)
                return;
            DisplayProductInformation();
        }
    }
}
