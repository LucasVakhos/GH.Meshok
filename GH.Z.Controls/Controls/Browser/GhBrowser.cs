using Gecko;
using Gecko.WebIDL;
using System.ComponentModel;

namespace GH.Components
{
    public class GhBrowser: GeckoWebBrowser, ISupportInitialize
    {
        static string title = @"Wait for loading ...";
        static string load_html =
            @"<html>"+
                "<head>" +
                    $"<title>{title}</title>" +
                    "<style type = \"text/css\" >" +
                    "<!-- " +
                    ".loading {font-size: 18px; font-weight: bold; font-family: Arial, Helvetica, sans-serif; } " +
                    "-->" +
                    "</style>" +
                "</head>" +
                "<body>" +
                    "<div align = \"center\" class=\"loading\">" +                       
                        $"<p>{title}</p>" +
                    "</div>" +
                "</body>" +
            "</html>";
        
        public GhBrowser()
        {
        }

        public void BeginInit()
        {
        }

        public void EndInit()
        {
            if (!DesignMode)
            {
                LoadHtml(load_html, "https://localhost");
            }
        }
    }
}
