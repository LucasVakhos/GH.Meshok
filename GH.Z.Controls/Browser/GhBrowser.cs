using Gecko;
using System.ComponentModel;

namespace GH.Browser
{
    public class GhBrowser : GeckoWebBrowser, ISupportInitialize
    {
        //static readonly string title = @"Wait for loading ...";
        //static readonly string load_html =
        //    @"<html>" +
        //        "<head>" +
        //            $"<title>{title}</title>" +
        //            "<style type = \"text/css\" >" +
        //            "<!-- " +
        //            ".loading {font-size: 18px; font-weight: bold; font-family: Arial, Helvetica, sans-serif; } " +
        //            "-->" +
        //            "</style>" +
        //        "</head>" +
        //        "<body>" +
        //            "<div align = \"center\" class=\"loading\">" +
        //                $"<p>{title}</p>" +
        //            "</div>" +
        //        "</body>" +
        //    "</html>";

        public GhBrowser()
        {
        }

        public void BeginInit()
        {
            if (!DesignMode)
            {
                if (!Xpcom.IsInitialized)
                {
                    string xul = XULRunnerLocator.GetXULRunnerLocation("Firefox");
                    Xpcom.Initialize(xul);
                    //GeckoPreferences.User["general.useragent.override"] = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/90.0.4430.41 YaBrowser/21.5.0.579 Yowser/2.5 Safari/537.36";

                }
            }
        }

        public void EndInit()
        {
            //if (!DesignMode)
            //{
            //    LoadHtml(load_html, "https://localhost");
            //}
        }
    }
}