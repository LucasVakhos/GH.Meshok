using Microsoft.Web.WebView2.WinForms;
using System.ComponentModel;
namespace GH.Components
{
    public class GhBrowser : WebView2, ISupportInitialize
    {
        private static string title = @"Wait for loading ...";
    private static string load_html =
            @"<html>" +
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
    public virtual void BeginInit()
        {
        }
    public virtual void EndInit()
        {
            if (!DesignMode)
            {
                NavigateToString(load_html);
            }
        }
    }
}
